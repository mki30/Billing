<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ReturnControl.ascx.cs" Inherits="Contols_ReturnControl" %>
<script type="text/javascript">

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
            {
                SearchItem($(this).val(), 0);
            }
        });

        $('#txtPLU').on('keypress', function (event)
        {
            if (event.which === 13)
            {
                findTableRow();
            }
        });

        $("#txtPLU").focus();

        $('#txtQuantity').on('keypress', function (event)  //on press enter save when focus is on quanity.
        {
            if (event.which === 13)
                Save();
        });
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

    function FechInventory(itemId, vendorID)
    {
        GetInventotyByItem(itemId, vendorID);
    }

    var ItemsSearched = null;
    function SearchItem(term, itemID)   //Search item list -first lavel
    {
        $.ajax({
            url: "/Data.aspx?Action=GetSerchedItem2&Data1=" + term
                + "&Data2=" + RecordType + "&Data3=" + $("#ddVendor").val() + "&Data4=",
            cache: false, success: function (data)
            {
                ItemsSearched = $.parseJSON(data);
                var VendorID = 0;
                //if ((RecordType == "1" || RecordType == "2"))
                //    VendorID = $("#ddVendor").val();
                search = "<table id='itemsearch' class='table table-condensed table-striped table-bordered' style='background:white;'><tr><th>PLU<th>Name<th>Rate<th>MRP<th>Tax";
                $.each(ItemsSearched, function (i, value)
                {
                    search += "<tr><td>" + value.PLU + "<td><a href='#' onclick='FechInventory(" + value.ID + "," + VendorID + ")'>" + value.Name + "</a><td>" + value.Cost + "<td>" + value.MRP + "<td>" + (value.TaxRate != "0" ? value.TaxRate : "");
                });
                search += "</table>";
                $("#searchResult").html(search);
            }
        });
    }

    function GetInventotyByItem(itemID, vendorID)  //Search invetory list for paricular item-Second level
    {
        $.ajax({
            url: "/Data.aspx?Action=GetInventoryByItem&Data1=" + itemID + "&Data2" + vendorID + "&Data3=" + RecordType,
            cache: false, success: function (data)
            {
                ItemsSearched = $.parseJSON(data);
                var search = "Please select purchase record<table id='itemsearch' class='table table-condensed table-striped table-bordered'><tr><th>PLU<th>Name<th>Rate<th>MRP<th>Tax<th>Date<th>RcNo";
                if (ItemsSearched.length == 0)
                    search = "No purchase found for this item!";

                $.each(ItemsSearched, function (i, value)
                {
                    search += "<tr><td>" + value.PLU + "<td><a href='#' onclick='UpdateItem(" + i + ")'>" + value.Name + "</a><td>"
                        + value.Cost + "<td>" + value.MRP + "<td>"
                        + (value.Tax != "0" ? value.Tax : "")
                        + "<td>" + value.DateString + "<td>" + value.RcNo;
                });
                search += "</table>";
                $("#txtPLU").val("");
                $("#searchResult").html(search);

                if (ItemsSearched.length == 1)  //if single item then auto click
                {
                    $("#itemsearch").find('tr').eq(1).find('td').eq(1).find('a').click();
                }
            }
        });
    }

    function UpdateItem(index)  //Fill Item Information in edit form on second level search click
    {
        var I = ItemsSearched[index];
        $("#hdPurchaseID").val(I.PurchaseID);
        $("#hdInventoryID").val(I.ID);
        $("#hidItemID").val(I.ItemID);
        $("#lblItemName").text(I.Name);
        $("#hdItemName").val(I.Name);
        $("#txtPLU").val(I.PLU);
        $("#txtRcNo").val(I.RcNo);
        $("#searchResult").html("");
        $("#txtQuantity").focus();
        $("#txtCost").val(I.Cost);
        $("#txtMRP").val(I.MRP);

        //alert(I.VendorID);
        $("#ddVendor").val(I.VendorID);
    }

    function Addnew()
    {
        window.location.href = "/Inventory.aspx?purchaseId=" + $("#hdPurchaseID").val();
    }

    function DeleteConfirm()
    {
        return confirm("Are you sure you want to delete ?");
    }

    function ShowEdit(id)     //Show Edit Return/Damage/In/Out
    {
        ShowDialog(true);

        $.ajax({
            url: "/Data.aspx?Action=GetInventory&Data1=" + id,
            cache: false,
            success: function (data)
            {
                var I = data;
                $("#hdID").val(I.ID);
                $("#txtSNo").val(I.SNo);
                $("#hdPurchaseID").val(I.PurchaseID);
                $("#hidItemID").val(I.ItemID);
                $("#lblItemName").text(I.Name);
                $("#hdItemName").val(I.Name);
                $("#txtPLU").val(I.PLU);
                $("#txtQuantity").val(I.Quantity);
                $("#txtDate").val(I.DateString);
                $("#txtRcNo").val(I.RcNo);
                $("#txtCost").val(I.Cost);
                $("#txtMRP").val(I.MRP);

                $("#searchResult").html("");
                $("#itemDataTable > tbody > tr").removeClass("highlight");
                $("#tr" + I.ID).addClass("highlight");
                $("#btnDelete").css("display", "block");
                $("#txtPLU").prop('disabled', true);

                $("#ddVendor").val(I.VendorID);
            }
        });
        return false;
    }

    function ShowDialog(flag, Heading)
    {
        if (flag)
        {
            //$('#divDialog').css("top", $(document).scrollTop() + ($(window).height() - $('#divDialog').height()) * .5);
            //$('#divDialog').show();
            //$('#divDialogfade').css("top", $(document).scrollTop());
            //$('#divDialogfade').show();
            OpenPopup();
            $("#txtPLU").focus();
            $("#DialogHeading").html(Heading);
        }
        else
            $("#divDialog").hide();
        New();
        return false;
    }

    function Save()   //Save Return/Damage/In/Out
    {
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
        
        $("#btnSave").val("Saving...");
        $("#btnSave").prop('disabled', true);
        $.ajax({
            type: "POST",
            url: "/Data.aspx?Action=SaveReturn&Data1=" + RecordType,
            data: {
                ID: $("#hdID").val(),
                InventoryID: $("#hdInventoryID").val(),  //Id of actual invetory to which rerurn/damage/In/Out is savd.
                PurchaseID: $("#hdPurchaseID").val(),
                ItemID: $("#hidItemID").val(),
                Quantity: $("#txtQuantity").val(),

                Cost: $("#txtCost").val(),
                MRP: $("#txtMRP").val(),
                RcNo: $("#txtRcNo").val(),
                VendorID: $("#ddVendor").val(),
                ToStore: $("#ddToStore").val(),
                TransactionDate: $("#txtDate").val(),
            },
            cache: false,
            success: function (obj)
            {
                $("#btnSave").prop('disabled', false);
                $("#txtPLU").prop('disabled', false);
                $("#btnSave").val("Saved");
                GetInventoryList();
                New();
            }
        });
        ShowDialog();
        return false;
    }

    function New()
    {
        $("#btnSave").val("Save");
        $("#inventoryform input[type=text]").val("");
        $("#lblItemName").html("");
        $("#txtDate").val(TodayDate),
        $("#lblTotalAmount").html("");
        $("#txtPLU").focus();

        $("#btnDelete").hide();
        $("#btnSave").prop('disabled', false);
        $("#txtPLU").prop('disabled', false);
        $("#btnSave").val("Saved");
        return false;
    }
</script>


<table style="width: 100%">
    <tr>
        <td>
            <h4 id="DialogHeading"></h4>
        </td>
        <td></td>
    </tr>
</table>

<div class='panel panel-info'>
    <div class="panel-heading" id="inventoryform">
        <div runat="server" id="hiddenIDs" style="display: none">
            <asp:TextBox ID="hdID" Value="" runat="server" placeholder="ID"></asp:TextBox>
            <asp:TextBox ID="hdPurchaseID" Value="" runat="server" placeholder="Purchase ID"></asp:TextBox>
            <asp:TextBox ID="hdInventoryID" Value="" runat="server" placeholder="Inventory ID"></asp:TextBox>
            <input id="hidItemID" type="text" runat="server" placeholder="Item ID" />
        </div>
        <table style="width: 100%">
            <tr>
                <td>
                    <asp:Literal ID="ltStore" runat="server">To</asp:Literal></td>
                <td>Item</td>
                <td>Qty</td>
                <td>Date</td>
                <td>Cost</td>
                <td>MRP</td>
                <td>RcNo</td>
            </tr>
            <tr>
                <td>
                    <asp:DropDownList ID="ddToStore" runat="server"></asp:DropDownList>
                </td>
                <td>
                    <asp:TextBox ID="txtPLU" runat="server" CssClass="autCalculate" required="reqiuired" MaxLength="20" placeholder="PLU  Min 3 char required to search" Width="250"></asp:TextBox>
                </td>

                <td>
                    <asp:TextBox ID="txtQuantity" runat="server" CssClass="autCalculate" required="reqiuired" MaxLength="10" placeholder="Qty" Width="80px"></asp:TextBox></td>
                <td>
                    <asp:TextBox ID="txtDate" Width="100" runat="server" CssClass="datepicker" placeceholder="Date"></asp:TextBox>
                </td>
                <td>
                    <asp:TextBox ID="txtCost" runat="server" placeholder="Cost" Width="80px"></asp:TextBox>
                </td>
                <td>
                    <asp:TextBox ID="txtMRP" runat="server" placeholder="MRP" Width="80px"></asp:TextBox>
                </td>
                <td>
                    <asp:TextBox ID="txtRcNo" runat="server" placeholder="RcNo" Width="80px"></asp:TextBox>
                </td>
                <td>&nbsp;</td>
                <td>
                    <input id="btnSave" type="button" value="Save" class="btn btn-sm btn-primary" onclick="return Save()" style="Width: 100%" />
                </td>
                <td>
                    <asp:LinkButton ID="btnDelete" Style="display: none;" runat="server" Text="Delete" OnClick="btnDelete_Click" CssClass="pull-right btn btn-sm btn-warning" OnClientClick="return DeleteConfirm()" />
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
