<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Purchase.aspx.cs" Inherits="Company" %>

<%@ Register Src="Contols/ReturnControl.ascx" TagName="ReturnControl" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="js/common.js"></script>
    <script src="js/bootstrap-typeahead.min.js"></script>
    <script type="text/javascript">
        var RecordType = 0;
        var RECORDTYPE_IN = 3;
        var RECORDTYPE_OUT = 4;
        var RECORDTYPE_TRANSFER = 5;
        $(document).ready(function ()
        {
            if (RecordType == 0)
                $("#txtItemSearch").hide();
            if (RecordType != 0)
            {
                GetInventoryList();
            }
        });

        function GetInventoryList()  // Get Return/Damage/In/Out record table.
        {
            var URL = "/Data.aspx?Action=GetInventoryList&Data1=" + $("#ddVendor").val() + "&Data2=" + RecordType + "&Data3=" + FilterDateFrom + "&Data4=" + FilterDateTo + "&Data5=" + 0 + "&Data6=" + ItemID;
            if (RecordType == RECORDTYPE_IN)
            {
                URL = "/Data.aspx?Action=GetInventoryTransferList&Data1=" + FilterDateFrom + "&Data2=" + FilterDateTo + "&Data3=" + 0 + "&Data4=" + Store + "&Data5=" + ItemID + "&Data6=" + RecordType;
            }
            else if (RecordType == RECORDTYPE_OUT)
            {
                URL = "/Data.aspx?Action=GetInventoryTransferList&Data1=" + FilterDateFrom + "&Data2=" + FilterDateTo + "&Data3=" + Store + "&Data4=" + 0 + "&Data5=" + ItemID + "&Data6=" + RecordType;
            }
            $.ajax({
                url: URL,
                cache: false, success: function (data)
                {
                    $("#lblDataTable").html(data);
                }
            });
        }

        function PurchaseSelect(ID)
        {
            window.open("/Inventory.aspx?purchaseId=" + ID, "_blank");
            return false;
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server" ViewStateMode="Disabled">
    <asp:Label ID="lblDate" runat="server" Text="" Style="font-weight: bold"></asp:Label>
    <div class="panel panel-info">
        <div class="panel-heading">
            <div style="display: none">
                <asp:Label ID="lblID" runat="server" Text="0" ViewStateMode="Disabled"></asp:Label>
            </div>
            <table style="width: 100%" class='table-condensed'>
                <tr>
                    <td style="width: 130px;">
                        <asp:DropDownList ID="ddVendor" Width="120px" runat="server" onchange="ChangeURL(this,'vendorid')" Style="width: 100%;"></asp:DropDownList>
                    </td>
                    <td>
                        <a class="btn btn-sm btn-success" runat="server" id="newLink" href="#"><i class="glyphicon glyphicon-plus"></i>&nbsp;Purchase</a>
                    </td>
                    <td style="width: 300px;">
                        <asp:Literal ID="ltDateFilter" runat="server" ViewStateMode="Disabled"></asp:Literal>
                        <div id="divItemSearch" class='searchresult'></div>
                    </td>
                </tr>
            </table>
            <asp:Label ID="lblStatus" runat="server" ViewStateMode="Disabled"></asp:Label>
            <table>
            </table>
        </div>
    </div>
    <div id="lblDataTable" runat="server"></div>
    <div id="divDialog" class="dialog">
        <span onclick="ClosePopup()" style="cursor: pointer;" class="pull-right glyphicon glyphicon-remove" aria-hidden="true"></span>
        <uc1:ReturnControl ID="ReturnControl1" runat="server" />
    </div>
    <div id="divDialogfade" class="black_overlay"></div>
</asp:Content>

