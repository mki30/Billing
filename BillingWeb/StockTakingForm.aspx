<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="StockTakingForm.aspx.cs" Inherits="StockTrackingForm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="js/TableSearch.js"></script>
    <script type="text/javascript">

        $(document).ready(function ()
        {
            $("#txtItemSearch").hide();
            $("#txtSearch").keyup(function (e)
            {
                if ($(this).val() == "")
                    $("#spanItemName").text("");
            });

            $('#txtSearch').on('keypress', function (event)
            {
                if (event.which === 13)
                    findTableRow();
            });

            $('#txtQuantity').on('keypress', function (event)
            {
                if (event.which === 13)
                    SaveStockDetail();
            });

            $("#DataTable tr").click(function ()  //highlight row
            {
                var selected = $(this).hasClass("highlight");
                $("#DataTable tr").removeClass("highlight");
                if (!selected)
                    $(this).addClass("highlight");
            });

            $("#divData").height($(window).height() - $("#divStockTakingDetail").height() - $(".panel-heading").height()-115);
        });

        function findTableRow()
        {
            $('#DataTable tr').each(function ()
            {
                if ($(this).find('td').eq(2).text() == $('#txtSearch').val())
                {
                    $(this).find('td').eq(1).find('a').click();
                }
            });
        }

        function SaveStockDetail()
        {
            if ($("#txtSearch").val() == "")
            {
                alert("Enter item PLU");
                $("#txtSearch").focus();
                return;
            }
            if ($("#txtQuantity").val() == "")
            {
                if ($("#txtOpeningStock").val() == "")
                {
                    alert("Enter Quantity");
                    $("#txtQuantity").focus();
                    return;
                }
            }

            var dt = $("#filterMonth option:selected").data("enddate");
            var itemid = $("#txtItemID").val();
            var openingstock = $("#txtOpeningStock ").val();
            var qty = $("#txtQuantity ").val();

            $.ajax({
                type: "POST",
                data: { ItemID: itemid, OpeningStock: openingstock, Quantity: qty, Date: dt },
                url: "/Data.aspx?Action=AddManualClosingStock&Data1=", cache: false, success: function (data)
                {
                    $("#tdQty" + itemid).html(qty);
                    $("#tdDate" + itemid).html(dt);

                    $("#txtItemID").val("");
                    $("#txtSearch").val("");
                    $("#spanItemName").text("");
                    $("#txtQuantity").val("");
                    $("#txtSearch").focus();

                    //location.reload();
                    //ShowStock();
                }
            });
        }

        function GetStock(ItemID, PLU, Name, Opening, Closing, WeightTypeString)
        {
            $("#txtItemID").val(ItemID);
            $("#txtSearch").val(PLU);
            $("#txtOpeningStock ").val(Opening);
            $("#txtQuantity ").val(Closing).focus().select();
            $("#spanUnitType").text(WeightTypeString == "none" ? "" : WeightTypeString);
            $("#spanItemName").text(Name);
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server" ViewStateMode="Disabled">
    <table>
        <tr>
            <td colspan="3">
                <asp:Label ID="lblStockList" runat="server" Text="" EnableViewState="false"></asp:Label>
            </td>
        </tr>
    </table>
    <div class="panel panel-info" id="divStockTakingDetail" runat="server" visible="false">
        <div class="panel-heading">
            <table style="width:100%">
                <tr style="display: none;">
                    <td>
                        <asp:TextBox ID="txtID" runat="server" placeholder="ID"></asp:TextBox></td>
                    <td>
                        <asp:TextBox ID="txtStockID" runat="server" placeholder="Stock ID"></asp:TextBox></td>
                    <td>ItemID:<asp:TextBox ID="txtItemID" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>Stock Update&nbsp;<asp:TextBox ID="txtSearch" runat="server" Style="width: 150px;" placeholder="PLU" tilte="select item to update stock"></asp:TextBox></td>
                    <td style="width:30%"><span id="spanItemName" class="label label-danger" style="font-size: 12px;"></span></td>
                    <td style="visibility: hidden">Opening
                    <asp:TextBox ID="txtOpeningStock" runat="server" placeholder="Opening Stock" Style="width: 50px; visibility: hidden" title="Opening Stock"></asp:TextBox></td>
                    <td>Quantity
                    <asp:TextBox ID="txtQuantity" runat="server" placeholder="Qty" Style="width: 50px;text-align:center" title="Closing Stock"></asp:TextBox>&nbsp;<span id="spanUnitType" class="active"></span></td>
                    <td><a id="linkSaveStockDetail" href="#" onclick="SaveStockDetail();" class="btn btn-success btn-sm">Save</a></td>

                    <td>
                        <asp:Literal ID="ltDateFilter" runat="server"></asp:Literal></td>
                </tr>
            </table>
        </div>
    </div>
    <div id="divDetailRecords"></div>
    <table>
        <tr>
            <td>
                <asp:DropDownList ID="ddBrand" runat="server" onchange="ChangeURL(this,'brandid');"></asp:DropDownList><a href="#" onclick="DownloadFile('Stock.csv',[2])">CSV<i class="glyphicon glyphicon-download-alt"></i></a>&nbsp;&nbsp;&nbsp;&nbsp;</td>
            <td><a href="/StockTakingForm.aspx?filterdatefrom=<%=FilterDateFrom.ToString("%d-MMM-yyyy") %>&monthlyreport=1">Monthly Report</a></td>
            <td><a href="/StockTakingForm.aspx?filterdatefrom=<%=FilterDateFrom.ToString("%d-MMM-yyyy") %>&filterdateto=<%=FilterDateTo.ToString("%d-MMM-yyyy") %>&updatestock=1" style="margin-left: 20px;">UpdateStock</a></td>
        </tr>
    </table>
    <div id="divData" runat="server"  style='width: 100%; overflow: auto;'></div>
</asp:Content>

