<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Inventory.aspx.cs" Inherits="Inventory" ClientIDMode="Static" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="js/TableSearch.js"></script>
    <script type="text/javascript">
        $(document).ready(function ()
        {
            GetPurchaseInventoryList();
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

            $("#txtPLU").keyup(function (e)
            {
                if ($(this).val() != "")
                    SearchItem($(this).val());
                else
                    $("#searchResult").html("");
            });

            $('#txtPLU').on('keypress', function (event)
            {
                if (event.which === 13)
                {
                    findTableRow();
                }
            });
            $("#txtPLU").focus();
        });

        function GetPurchaseInventoryList(selctedRowId)
        {
            $.ajax({
                url: "/Data.aspx?Action=GetInventoryHTML&Data1=" + $("#hdIDPurchase").val(),
                cache: false,
                success: function (data)
                {
                    $("#lblDataTable").html(data);
                    if (selctedRowId != 'undefined')
                    {
                        $("#tr" + selctedRowId).addClass("highlight");
                        $("#lblBillAmount").text($("#tdTotalAfterTax").text());
                    }
                },
                error: function (data)
                {
                }
            });
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

        function GetDecimalNumber(n)
        {
            return parseFloat(Math.round(n * 100) / 100).toFixed(2);
        }

        var ItemsSearched = null;
        function SearchItem(term)
        {
            $("#searchResult").html("");
            $.ajax({
                url: "/Data.aspx?Action=GetSerchedItem&Data1=" + term, cache: false, success: function (data)
                {
                    var search = "";
                    if (data.length > 0)
                    {
                        ItemsSearched = $.parseJSON(data);
                        search = "<table id='itemsearch' class='table table-condensed table-striped table-bordered' style='background-color:white;'><tr><th>PLU<th>Name<th>Rate<th>MRP<th>Tax";
                        $.each(ItemsSearched, function (i, value)
                        {
                            search += "<tr><td>" + value.PLU + "<td><a href='#' onclick='UpdateItem(" + i + ")'>" + value.Name + "</a><td>" + value.Cost + "<td>" + value.MRP + "<td>" + (value.TaxRate != "0" ? value.TaxRate : "");
                        });
                        search += "</table>";
                    }
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

        //Show Inventory Edit
        function ShowEdit(id)
        {
            //ShowPopup("#inventoryform");
            OpenPopup();
            $("#itemDataTable > tbody > tr").removeClass("highlight");
            if (id == 0)
            {
                $("#hdID").val("0");
                $("#inventoryform input[type=text]").val("");
                $("#inventoryform select").val("0");
                $("#lblItemName").html("");
                $("#lblTotalAmount").html("");
                $("#txtPLU").focus();
                $("#btnDelete").hide();
            }
            else
            {
                $("#tr" + id).addClass("highlight");

                $.ajax({
                    url: "/Data.aspx?Action=GetInventory&Data1=" + id,
                    cache: false,
                    success: function (I)
                    {
                        if (typeof I != "object" && I.indexOf("Not logged") > -1)
                            location.href = "/login.aspx";

                        $("#txtSNo").val(I.SNo);
                        $("#hdID").val(I.ID);
                        $("#hidItemID").val(I.ItemID);
                        $("#lblItemName").text(I.Name);
                        $("#hdItemName").val(I.Name);
                        $("#txtPLU").val(I.PLU);
                        $("#txtQuantity").val(I.Quantity);
                        $("#txtCost").val(I.Cost);
                        $("#ddCostUnitType").val(I.CostUnitType);
                        $("#txtDiscount").val(I.Discount);
                        $("#ddTax").val(I.Tax);
                        $("#txtMRP").val(I.MRP);
                        $("#txtUnit").val(I.Unit);
                        $("#ddUnitType").val(I.UnitType);
                        $("#txtExpiry").val(I.ExpiryString);
                        $("#lblTotalAmount").text(I.TotalAmount);
                        $("#HiddenTotalAmount").val(I.TotalAmount);
                        $("#txtQuantity").focus();
                        $("#searchResult").html("");
                        $("#btnDelete").css("display", "");
                    },
                    error: function (data)
                    {
                        alert(data);
                        console.log(data);
                    }
                });
            }
            return false;
        }

        function UpdateItem(index)
        {
            var I = ItemsSearched[index];
            $("#hidItemID").val(I.ID);
            $("#lblItemName").text(I.Name);
            $("#hdItemName").val(I.Name);
            $("#txtPLU").val(I.PLU);
            $("#txtCost").val(I.Cost);
            $("#ddCostUnitType").val(I.RateType);
            $("#txtUnit").val(I.Weight);
            $("#ddUnitType").val(I.WeightType);
            $("#ddTax").val(I.TaxRate);
            $("#txtMRP").val(I.MRP);
            $("#txtQuantity").focus();
            $("#searchResult").html("");
        }

        //Save Inventory
        function Save()
        {
            if ($("#txtPLU").val() == "")
            {
                alert("Enter item PLU");
                $("#txtPLU").focus();
                return;
            }

            if ($("#txtQuantity").val() == "")
            {
                alert("Enter Quantity");
                $("#txtQuantity").focus();
                return;
            }
            var str = $("#inventoryform :input").serialize();
            var data = str.replace(/ctl00%24ContentPlaceHolder1%24/g, '');

            $("#btnSave").val("Saving...");
            $.ajax({
                type: "POST",
                url: "/Data.aspx?Action=SaveInventory&Data1=",
                data: data,
                cache: false,
                success: function (obj)
                {
                    ShowEdit(0);
                    $("#btnSave").val("Save");
                    GetPurchaseInventoryList(obj.id);
                }
            });
            return false;
        }

        //Save purchase
        function SavePurchase()
        {
            //prevent add purchase of future date in case of add new purchase
            var date = new Date(Date.parse($("#txtDate").val()));
            date.setHours(0, 0, 0, 0);
            var currentDate = new Date();
            currentDate.setHours(0, 0, 0, 0);
            if (date.getTime() > currentDate.getTime())
            {
                alert("Future date is not allowed.Please check date.");
                return;
            }

            var str = $("#purchaseForm :input").serialize();
            var data = str.replace(/ctl00%24ContentPlaceHolder1%24/g, '');
            $("#btnSavePurchase").val("Saving...");
            $.ajax({
                type: "POST",
                url: "/Data.aspx?Action=SavePurchase&Data1=",
                data: data,
                cache: false,
                success: function (obj)
                {
                    location.href = "/Inventory.aspx?purchaseId=" + obj;
                }
            });
            return false;
        }


        //Change Vendor
        function ShowVendorChangeForm()
        {
            var obj = $("#divVendorEdit");
            $(obj).css({ left: ($(window).width() - $(obj).width()) * .5, top: $(document).scrollTop() + ($(window).height() - $(obj).height()) * .5 });
            $(obj).show();
            $("#ddVendor").val($("#hdVendorID").val());
        }
        function changeVendor()
        {
            if ($("#ddVendor").val() == "")
            {
                alert("Please select a vendor");
                return;
            }
            $.ajax({
                url: "/Data.aspx?Action=ChangeVendor&Data1=" + $("#hdIDPurchase").val() + "&Data2=" + $("#ddVendor").val(),
                cache: false,

                success: function (obj)
                {
                }
            });
        }
        $(document).on('keydown', function (e)  //close popup on escape press
        {
            if (e.keyCode === 27)
            {
                $('#divVendorEdit').hide();
            }
        });
        //Change Vendor

    </script>
    <style>
        td
        {
            padding-top: 0px!important;
            padding-bottom: 0px!important;
        }


        /*set close on top right corner of a div*/
        #divVendorEdit
        {
            position: relative;
        }

            #divVendorEdit span
            {
                position: absolute;
                top: 0px;
                right: 0px;
            }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="panel panel-info">
        <div class="panel-heading" id="purchaseForm">
            <div style="display: none">
                <asp:HiddenField ID="hdIDPurchase" Value="0" runat="server" />
            </div>
            <table class='table-condensed' style="width: 100%">
                <tr>
                    <td>Vendor (ID-<asp:Literal ID="ltPurchaseID" Text="0" runat="server"></asp:Literal>)
                    </td>
                    <td>Date</td>
                    <td>Invoice No</td>
                    <td>RC No
                    </td>
                    <td>CST</td>
                    <td>Bill Amount</td>
                    <td>
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td>
                        <asp:Label Style="font-weight: bold;" ID="lblVendor" runat="server" Text=""></asp:Label>
                        <asp:HiddenField ID="hdVendorID" Value="0" runat="server" />
                    </td>
                    <td>
                        <asp:TextBox ID="txtDate" runat="server" required="required" Width="120px" CssClass="datepickerDisableFutureDates"></asp:TextBox>
                    </td>
                    <td>
                        <asp:TextBox ID="txtInvoiceNo" runat="server" MaxLength="20" Width="120px"></asp:TextBox></td>
                    <td>
                        <asp:TextBox ID="txtRCNo" runat="server" MaxLength="20" Width="120px"></asp:TextBox></td>
                    <td>
                        <asp:TextBox ID="txtCST" runat="server" MaxLength="4" Width="50px"></asp:TextBox></td>
                    <td>
                        <asp:Label ID="lblBillAmount" runat="server" Text="0" ></asp:Label></td>
                    <td>
                        <asp:CheckBox ID="chkIncludingTax" runat="server" Text="Rate Including Tax" />
                    </td>
                    <td>
                        <asp:CheckBox ID="chkForm38" runat="server" Text="Form38" />
                    </td>
                    <td>
                        <asp:CheckBox ID="chkIsNonUpPurchase" runat="server" Text="Non UP" />
                        <asp:CheckBox ID="chkDelete" runat="server" Text="Delete" />
                    </td>
                    <td class="pull-right">
                        <input type="button" id="btnSavePurchase" class="btn btn-sm btn-primary" runat="server" value="Save" onclick="return SavePurchase();" />
                    </td>
                </tr>
            </table>
            <asp:Label ID="lblPurchaseMsg" runat="server"></asp:Label>
            <table>
            </table>
        </div>
    </div>

    <%--<div class='popup' id="inventoryform" style="display: none;width:60%;" >--%>
    <div id="inventoryform">
    <div id="divDialog" class="dialog" style="height:70%;">
        <span onclick="ClosePopup()" style="cursor: pointer;" class="pull-right glyphicon glyphicon-remove" aria-hidden="true"></span>
        <table style="width:100%;">
            <tr>
                <td>
                    <h4>Purchase Item Details</h4>
                </td>
                <td style="text-align: right; width:60%;">
                    <%--<span onclick="$('#inventoryform').hide()" class="glyphicon glyphicon-remove" style="color:red;cursor:pointer;"></span>--%></td>
            </tr>
            <tr>
                <td style="vertical-align: top">
                    <table style="width: 100%">
                        <tr class="hidden">
                            <td>ID:<asp:Label ID="lblID" runat="server" Text="0"></asp:Label><asp:HiddenField ID="hdID" Value="0" runat="server" />
                            </td>
                            <td>
                                <asp:HiddenField ID="hdPurchaseID" Value="0" runat="server" />
                                <asp:HiddenField ID="hdRcNo" Value="" runat="server" />
                                <asp:HiddenField ID="hdDatePurchase" Value="" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width:10%;">
                                <asp:TextBox ID="txtSNo" Width="100%" runat="server" placeholder="SNo"></asp:TextBox><asp:HiddenField ID="hidItemID" runat="server" />
                            </td>
                            <td style="width: 90%;">
                                <input type='text' id="txtPLU" runat="server" class="autCalculate" required="required" maxlength="30" placeholder="PLU  Min 3 char required to search" style="width: 100%;" /></td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <input id="hdItemName" type="hidden" runat="server" />
                                <asp:Label ID="lblItemName" runat="server" Text="" CssClass="label label-warning"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>Qty</td>
                            <td>
                                <asp:TextBox ID="txtQuantity" runat="server" CssClass="autCalculate " required="reqiuired" MaxLength="10" placeholder="Qty" Width="80px"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>Rate</td>
                            <td style="width: 200px;">
                                <asp:TextBox ID="txtCost" runat="server" CssClass="autCalculate " required="reqiuired" MaxLength="10" placeholder="Rate" Width="80px"></asp:TextBox>/<asp:DropDownList ID="ddCostUnitType" runat="server"></asp:DropDownList></td>
                        </tr>
                        <tr>
                            <td>Disc %</td>
                            <td>
                                <asp:TextBox ID="txtDiscount" runat="server" CssClass="autCalculate " placeholder="Disc." Width="80px"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>Tax</td>
                            <td style="width: 100px;">
                                <asp:DropDownList ID="ddTax" runat="server" CssClass=""></asp:DropDownList></td>
                        </tr>
                        <tr>
                            <td>MRP</td>
                            <td style="width: 100px;">
                                <asp:TextBox ID="txtMRP" runat="server" CssClass="" required="reqiuired" MaxLength="10" placeholder="MRP" Width="80px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Weight</td>
                            <td>
                                <asp:TextBox ID="txtUnit" runat="server" MaxLength="4" Width="80px"></asp:TextBox>
                                <asp:DropDownList ID="ddUnitType" runat="server"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>Expiry:
                            </td>
                            <td>
                                <asp:TextBox ID="txtExpiry" runat="server" CssClass="datepicker" MaxLength="10" placeholder="Expiry" Width="80px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2"><strong>Amt:</strong>
                                <strong>
                                    <asp:Label ID="lblTotalAmount" runat="server"></asp:Label></strong>
                                <asp:HiddenField ID="HiddenTotalAmount" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <input id="btnSave" type="button" value="Save" class="btn btn-primary btn-sm" onclick="return Save()" />
                                <asp:LinkButton ID="btnDelete" runat="server" Text="Del" OnClick="btnDelete_Click" CssClass="btn btn-sm btn-danger" OnClientClick="return DeleteConfirm()" />
                            </td>

                        </tr>
                    </table>
                </td>
                <td style="max-width: 50%;">
                    <div id="searchResult" style="height: 400px; overflow: auto"></div>
                </td>
            </tr>
        </table>
    </div>
    </div>
    <asp:Label ID="lblStatus" runat="server"></asp:Label>
    <table style="width: 100%" id="DataTable">
        <tr>
            <td style="vertical-align: top">
                <asp:Label ID="lblDataTable" runat="server" Text=""></asp:Label>
            </td>
        </tr>
    </table>

    <div id="divVendorEdit" style="display: none; position: absolute; background-color: wheat; padding: 20px; border: 1px solid #c0c0c0; box-shadow: 0 4px 8px 0 rgba(0, 0, 0, 0.2), 0 6px 20px 0 rgba(0, 0, 0, 0.19);">
        <span onclick="$('#divVendorEdit').hide()" class="glyphicon glyphicon-remove" style="cursor: pointer; color: #eb5353;"></span>
        <asp:DropDownList ID="ddVendor" runat="server" Style="height: 30px;"></asp:DropDownList>
        <a href="#" onclick="changeVendor()" class="btn btn-sm btn-success">Save</a>
    </div>
    <div id="divDialogfade" class="black_overlay"></div>

</asp:Content>

