<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Tax.aspx.cs" Inherits="Tax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="js/common.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="panel panel-info">
        <div class="panel-heading">
            <table class="table-condensed" style="width: 35%">
                <tr>
                    <td class="hidden">ID:
                <asp:Label ID="lblID" runat="server" Text="0"></asp:Label></td>
                    <td>Rate</td>
                    <td>
                        <asp:TextBox ID="txtRate" runat="server" CssClass="decimal" required="required" MaxLength="4" placeholder="Rate"></asp:TextBox></td>
                    <td>
                        <asp:Button ID="btnSave" CssClass="btn btn-sm btn-primary" runat="server" Text="Save" OnClick="btnSave_Click" OnClientClick="return Validate();" />
                        <a class="btn btn-sm btn-success"  href="/Tax.aspx">New</a>
                        <asp:Label ID="lblStatus" runat="server"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <asp:Label ID="lblDataTable" runat="server" Text=""></asp:Label>
</asp:Content>

