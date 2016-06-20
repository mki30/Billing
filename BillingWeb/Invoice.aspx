<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Invoice.aspx.cs" MasterPageFile="~/MasterPage.master" Inherits="Invoice" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table style="min-width:250px;">
        <tr>
            <td>
                ID
            </td>
            <td>
                <asp:Label ID="lblID" runat="server" Text="0"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                Invoice No
            </td>
            <td>
                 <asp:TextBox ID="txtInvoiceNo" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                Date
            </td>
            <td>
                <asp:TextBox ID="txtDate" runat="server" class="datepicker"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                Book No
            </td>
            <td>
                <asp:TextBox ID="txtBookNo" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>Client</td>
            <td>
                <asp:DropDownList ID="ddClient" runat="server"></asp:DropDownList></td>
        </tr>
        <tr>
            <td>BillNo</td>
            <td>
                <asp:TextBox ID="txtBillNo" runat="server"></asp:TextBox></td>
        </tr>
        <tr>
            <td>PO No</td>
            <td>
                <asp:TextBox ID="txtPONo" runat="server"></asp:TextBox></td>
        </tr>
        <tr>
            <td>Transport</td>
            <td>
                <asp:TextBox ID="txtTransport" runat="server"></asp:TextBox></td>
        </tr>
        <tr>
            <td>Vehicle No</td>
            <td>
                <asp:TextBox ID="txtVehicleNo" runat="server"></asp:TextBox></td>
        </tr>
        <tr>
            <td>Station</td>
            <td>
                <asp:TextBox ID="txtStation" runat="server"></asp:TextBox></td>
        </tr>
        <tr>
            <td></td>
            <td>
                <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-sm btn-success" OnClick="btnSave_Click"  />
                <a href="/Invoice.aspx" class="btn btn-sm btn-primary">New</a>
                <asp:Button ID="btnGenerateInvoice" runat="server" Text="Genrate Invoice" CssClass="btn btn-sm btn-info" OnClick="btnGenerateInvoice_Click" Visible="false"/>
                <asp:Label ID="lblBillNo" runat="server" Text="" Visible="false"></asp:Label>
                <asp:Label ID="lblMsg" runat="server" Text=""></asp:Label>
            </td>
        </tr>
    </table>

    <asp:Label ID="lblInviceList" runat="server" Text=""></asp:Label>

</asp:Content>
