<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Returns.aspx.cs" Inherits="Returns1" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="js/jquery-1.11.3.min.js"></script>
    <link href="BootStrap/css/bootstrap.min.css" rel="stylesheet" />
    <link href="css/datepicker.css" rel="stylesheet" />
    <script src="js/bootstrap-datepicker.js"></script>
    <script src="js/TableSearch.js"></script>
    <script type="text/javascript">
        var RecordType = 0;
        $(document).ready(function ()
        {
            $(".autCalculate").keyup(function ()
            {
                if ($("#txtQuantity").val() == "" || $("#txtCost").val() == "")
                {
                    $("#txtTotalAmount").val("");
                    return;
                }
                var grossAmount = parseFloat($("#txtQuantity").val()) * parseFloat($("#txtCost").val());
                var taxRate = $("#txtDiscount").val() == "" ? 0 : parseFloat($("#txtDiscount").val());
                var Amt = grossAmount - ((grossAmount * taxRate) / 100)

                $("#lblTotalAmount").text(GetDecimalNumber(Amt));
                $("#HiddenTotalAmount").val($("#lblTotalAmount").text());
            });

            $('.datepicker').datepicker({
                startView: 0,
                orientation: "auto",
                format: 'dd-M-yyyy'
            });

            $('.datepicker').on('changeDate', function (ev)
            {
                $(this).datepicker('hide');
            });

            $("#txtPLU").keyup(function (e)
            {
                if ($(this).val() != "")
                    SearchItem($(this).val());
            });

            $('#txtPLU').on('keypress', function (event)
            {
                if (event.which === 13)
                {
                    findTableRow();
                }
            });

            $("#txtPLU").focus();


            if (id != 0)
                ShowEdit(id);
        });

        function GetDecimalNumber(n)
        {
            return parseFloat(Math.round(n * 100) / 100).toFixed(2);
        }

        function findTableRow()
        {
            $('#itemsearch tr').each(function ()
            {
                if ($(this).find('td').eq(0).text() == $('#txtPLU').val())
                {
                    $(this).find('td').eq(1).find('a').click();
                }
            });
        }

        var ItemsSearched = null;

        function SearchItem(term)
        {
            alert();
            $("#searchResult").html("");
            $.ajax({
                url: "../Data.aspx?Action=GetSerchedItem2&Data1=" + term + "&Data2="
                    + $("#hdPurchaseID").val() + "&Data3=1&Data4="
                    + $("#hdVendorID").val(), cache: false, success: function (data)
                    {
                        var search = "<table id='itemsearch' class='table table-condensed table-striped table-bordered'><tr><th>PLU<th>Name<th>Rate<th>MRP<th>Tax<th>Date<th>RcNo";
                        ItemsSearched = $.parseJSON(data);
                        console.log(ItemsSearched);
                        $.each(ItemsSearched, function (i, value)
                        {
                            search += "<tr><td>" + value.PLU + "<td><a href='#' onclick='UpdateItem(" + i + ")'>" + value.Name + "</a><td>"
                                + value.Cost + "<td>" + value.MRP + "<td>"
                                + (value.Tax != "0" ? value.Tax : "")
                                + "<td>" + value.DateString + "<td>" + value.RcNo;
                        });
                        search += "</table>";
                        $("#searchResult").html(search);
                    }
            });
        }
        function Addnew()
        {
            window.location.href = "/Inventory.aspx?purchaseId=" + $("#hdPurchaseID").val();
        }

        function DeleteConfirm()
        {
            return confirm("Are you sure you want to delete ?");
        }

        function ShowEdit(id)
        {
            $.ajax({
                url: "/Data.aspx?Action=GetInventory&Data1=" + id,
                cache: false,
                success: function (data)
                {
                    var I = data;
                    $("#txtSNo").val(I.SNo);
                    $("#hdID").val(I.ID);
                    $("#hdPurchaseID").val(I.PurchaseID);
                    $("#hidItemID").val(I.ItemID);
                    $("#lblItemName").text(I.Name);
                    $("#hdItemName").val(I.Name);
                    $("#txtPLU").val(I.PLU);
                    $("#txtQuantity").val(I.Quantity);
                    $("#txtDate").val(I.DateString);
                    $("#txtRcNo").val(I.RcNo);
                    $("#searchResult").html("");
                    $("#itemDataTable > tbody > tr").removeClass("highlight");
                    $("#tr" + I.ID).addClass("highlight");
                    $("#btnDelete").css("display", "block");
                }
            });
            return false;
        }

        function UpdateItem(index)
        {
            var I = ItemsSearched[index];
            $("#hdPurchaseID").val(I.PurchaseID);
            $("#hdInventoryID").val(I.ID);
            $("#hidItemID").val(I.ID);
            $("#lblItemName").text(I.Name);
            $("#hdItemName").val(I.Name);
            $("#txtPLU").val(I.PLU);
            $("#txtRcNo").val(I.RcNo);
            $("#txtQuantity").val(I.Quantity);
            $("#searchResult").html("");
        }

        function Save()
        {
            debugger;
            if ($("#txtPLU").val() == "")
            {
                alert("Please enter PLU");
                $("#txtPLU").focus();
                return;

            }

            if ($("#txtQuantity").val() == "")
            {
                alert("Please enter Qty");
                $("#txtQuantity").focus();
                return;
            }

            var str = $("#inventoryform :input").serialize();
            var data = str.replace(/ctl00%24ContentPlaceHolder1%24/g, '');
            $.ajax({
                type: "POST",
                url: "/Data.aspx?Action=SaveReturn&Data1=" + RecordType,
                data: data,
                cache: false,
                success: function (obj)
                {
                    $("#itemDataTable > tbody > tr").removeClass("highlight");
                    $("#tr" + obj.id).remove();
                    $("#itemDataTable tr:first").after(obj.html);
                    $("#tr" + obj.id).addClass("highlight");
                    parent.location.reload();
                }
            });
            return false;
        }

        function New()
        {
            $("#inventoryform input[type=text]").val("");
            $("#lblItemName").html("");
            $("#txtPLU").focus();
            return false;
        }
    </script>
    <style>
        td
        {
            padding-top: 0px!important;
            padding-bottom: 0px!important;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <h3>
            <asp:Literal ID="ltVendor" runat="server"></asp:Literal></h3>
        <div class='panel panel-info'>
            <div class="panel-heading" id="inventoryform">
                <div style="display: none">
                    <asp:TextBox ID="hdID" Value="0" runat="server"></asp:TextBox>
                    <asp:TextBox ID="hdPurchaseID" Value="0" runat="server"></asp:TextBox>
                    <asp:TextBox ID="hdInventoryID" Value="0" runat="server"></asp:TextBox>
                    <asp:TextBox ID="hdVendorID" Value="0" runat="server"></asp:TextBox>
                    <input id="hdItemName" type="hidden" runat="server" />
                    <input id="hidItemID" type="hidden" runat="server" />
                </div>
                <table style="width: 100%">
                    <tr>
                        <td>PLU</td>
                        <td>Qty</td>
                        <td>Date</td>
                        <td>RcNo</td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtPLU" runat="server" CssClass="autCalculate" required="reqiuired" MaxLength="10" placeholder="PLU  Min 3 char required to search" Width="250"></asp:TextBox>
                        </td>

                        <td>
                            <asp:TextBox ID="txtQuantity" runat="server" CssClass="autCalculate" required="reqiuired" MaxLength="10" placeholder="Qty" Width="80px"></asp:TextBox></td>
                        <td>
                            <asp:TextBox ID="txtDate" Width="100" runat="server" CssClass="datepicker" placeceholder="Date"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRcNo" runat="server" placeceholder="RcNo"></asp:TextBox>
                        </td>
                        <td>&nbsp;</td>
                        <td>
                            <input id="btnSave" type="button" value="Save" class="btn btn-sm btn-primary" onclick="return Save()" style="Width: 100%" />
                        </td>
                        <td>
                            <asp:LinkButton ID="btnDelete" runat="server" Text="Delete" OnClick="btnDelete_Click" CssClass="pull-right" OnClientClick="return DeleteConfirm()" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5">
                            <asp:Label ID="lblItemName" runat="server" Text=""></asp:Label></td>
                        <td></td>
                    </tr>

                </table>
            </div>
        </div>
        <asp:Label ID="lblStatus" runat="server"></asp:Label>
        <div id="searchResult"></div>
        <asp:Label ID="lblDataTable" runat="server" Text=""></asp:Label>
        <br />
    </form>
</body>
</html>
