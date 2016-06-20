<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Report.aspx.cs" Inherits="Report" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="js/bootstrap-typeahead.min.js"></script>
    <script type="text/javascript">

        function ShowItemDetail(itemID, RecordType)
        {
            OpenPopup();
            GetInventoryList(itemID, RecordType)
        }

        function GetInventoryList(ItemID, RecordType)
        {
            $.ajax({
                url: "/Data.aspx?Action=GetInventoryList&Data1=" + $("#ddVendor").val() + "&Data2=" + RecordType + "&Data3=" + FilterDateFrom
                    + "&Data4=" + FilterDateTo + "&Data5=" + $("#ddFromToStore").val() + "&Data6=" + ItemID,
                cache: false, success: function (data)
                {
                    $("#lblDataTable").html(data);
                }
            });
            return false;
        }

        function GetInventoryListFull(ItemID)
        {
            $.ajax({
                url: "/Data.aspx?Action=GetInventoryListFull&Data1=" + FilterDateFrom + "&Data2=" + FilterDateTo + "&Data3=" + ItemID,
                cache: false, success: function (data)
                {
                    $("#lblDataTable").html(data);
                }
            });
            OpenPopup();
            return false;
        }

        function UpdateItemPrice(itemID, MRP)
        {
            var P = prompt("New Item Price", MRP);
            if (P != null)
            {
                $.ajax({
                    url: "/Data.aspx?Action=UpdateItemPrice&Data1=" + itemID + "&Data2=" + P,
                    cache: false,
                    success: function (data)
                    {
                        location.reload();
                    }
                });
                return false;
            }
        }
    </script>
    <style type="text/css">
        .alert
        {
            padding: 5px;
            margin-bottom: 5px;
            border: 1px solid transparent;
            -moz-border-radius: 4px;
            -webkit-border-radius: 4px;
            border-radius: 4px;
        }

        @media (max-width: 600px) {
            .mo {
                display: none;
            }
        }

    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server" ViewStateMode="Disabled">
    <div class='panel panel-info'>
        <div class="panel-heading">
            <table style="width: 100%">
                <tr>
                    <td style="width: 200px;">
                        <asp:DropDownList ID="ddBrand" runat="server" Visible="false" onchange="ChangeURL(this,'brandid')"></asp:DropDownList>
                    </td>
                    <td style="width: 150px;">
                        <asp:DropDownList ID="ddTax" runat="server" Style="width: 100%;" Visible="false" onchange="ChangeURL(this,'tax')"></asp:DropDownList></td>
                    <td style="width: 150px;">
                        <asp:DropDownList ID="ddVendor" runat="server" Style="width: 100%;" Visible="false" onchange="ChangeURL(this,'vendorid')"></asp:DropDownList>
                        <%--<asp:DropDownList ID="ddTerminal" runat="server" Style="width: 100%;" Visible="false" onchange="ChangeURL(this,'terminalid')"></asp:DropDownList>--%>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddEmployee" runat="server" onchange="ChangeURL(this,'employeeid')" Visible="false"></asp:DropDownList>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddStore" runat="server" onchange="ChangeURL(this,'storeid')" Visible="false"></asp:DropDownList>
                    </td>
                    <td style="width: 300px;">
                        <asp:Literal ID="ltDateFilter" runat="server"></asp:Literal>
                        <div id="divItemSearch" class="searchresult"></div>
                    </td>
                </tr>
            </table>
        </div>
    </div>

    <div>
        <asp:Button ID="btnUpdateHiddenIn" runat="server" Text="Reduce sale" Visible="false" OnClick="btnUpdateHiddenIn_Click" />
        <asp:TextBox ID="txtAmountToReduce" runat="server" Visible="false" placeholder="Amount to reduce"></asp:TextBox>
        <asp:TextBox ID="txtPriveVatReducable" runat="server" Visible="false" ToolTip="Maximum vat which can be reduced based on the items in the adjustment list" ></asp:TextBox>
        <asp:Literal ID="ltData" runat="server" EnableViewState="false"></asp:Literal>
    </div>
    <div id="divDialog" class="dialog">
        <span onclick="ClosePopup()" style="cursor: pointer;" class="pull-right glyphicon glyphicon-remove" aria-hidden="true"></span>
        <div id="divDialogContent">
            <div id="lblDataTable"></div>
        </div>
    </div>
    <div id="divDialogfade" class="black_overlay"></div>
</asp:Content>

