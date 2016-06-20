<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ReturnReport.aspx.cs" Inherits="Company" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="js/common.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:Label ID="lblDate" runat="server" Text="" Style="font-weight: bold"></asp:Label>
    <div class="panel panel-info">
        <div class="panel-heading">
            <div style="display: none">
                <asp:Label ID="lblID" runat="server" Text="0"></asp:Label>
            </div>
            <table style="width:100%" class='table-condensed'>
                <tr>
                    <td style="width: 100px;">
                        <asp:DropDownList ID="ddVendor" runat="server" required="required" onchange="ChangeURL(this,'VendorID')"></asp:DropDownList>
                    </td>
                    <td>
                        <a class="btn btn-sm btn-success" runat="server" id="newLink"><i class="glyphicon glyphicon-plus"></i> Add New Return</a>
                    </td>
                    <td style="width: 300px;">
                        <asp:Literal ID="ltDateFilter" runat="server"></asp:Literal>
                    </td>
                </tr>
            </table>
            <asp:Label ID="lblStatus" runat="server"></asp:Label>
            <table>
            </table>
        </div>
    </div>
    <asp:Label ID="lblDataTable" runat="server" Text=""></asp:Label>
</asp:Content>

