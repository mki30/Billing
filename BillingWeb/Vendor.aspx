<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Vendor.aspx.cs" Inherits="Vendor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script>
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="panel panel-info">
        <div class="panel-heading">
            <table class='table-condensed' style="width: 90%">
                <tr>
                    <td class="hidden">ID:<asp:Label ID="lblID" runat="server" Text="0"></asp:Label></td>
                    <td>Name</td>
                    <td>
                        <asp:TextBox ID="txtName" runat="server" required="required" MaxLength="50" placeholder="Name"></asp:TextBox></td>
                    <td>Mobile</td>
                    <td>
                        <asp:TextBox ID="txtMobile" runat="server" required="required" MaxLength="12" placeholder="Mobile"></asp:TextBox></td>
                    <td>TIN</td>
                    <td>
                        <asp:TextBox ID="txtTIN" runat="server" required="required" MaxLength="20" placeholder="TIN"></asp:TextBox></td>
                    <td colspan="5">
                        <asp:CheckBox ID="chkIsClient" runat="server" />
                        Is Client
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td>Address</td>
                    <td>
                        <asp:TextBox ID="txtAddress" runat="server" required="required" MaxLength="50" placeholder="Address"></asp:TextBox></td>
                    <td>Area</td>
                    <td>
                        <asp:TextBox ID="txtArea" runat="server" required="required" MaxLength="50" placeholder="Area"></asp:TextBox></td>
                    <td>City</td>
                    <td>
                        <asp:TextBox ID="txtCity" runat="server" required="required" MaxLength="50" placeholder="City"></asp:TextBox></td>
                    <td>State</td>
                    <td>
                        <asp:DropDownList ID="ddState" runat="server">
                            <asp:ListItem Value="">State</asp:ListItem>
                            <asp:ListItem Value="2">ANDHRA PRADESH</asp:ListItem>
                            <asp:ListItem Value="1">ANDMAN NICOBAR</asp:ListItem>
                            <asp:ListItem Value="3">ARUNACHAL PR</asp:ListItem>
                            <asp:ListItem Value="4">ASSAM</asp:ListItem>
                            <asp:ListItem Value="5">BIHAR</asp:ListItem>
                            <asp:ListItem Value="6">CHANDIGARH</asp:ListItem>
                            <asp:ListItem Value="33">CHHATISHGARH</asp:ListItem>
                            <asp:ListItem Value="7">DADR &amp; NAGAR H</asp:ListItem>
                            <asp:ListItem Value="8">DAMAN AND DIU</asp:ListItem>
                            <asp:ListItem Value="9">DELHI</asp:ListItem>
                            <asp:ListItem Value="10">GOA</asp:ListItem>
                            <asp:ListItem Value="11">GUJARAT</asp:ListItem>
                            <asp:ListItem Value="12">HARYANA</asp:ListItem>
                            <asp:ListItem Value="13">HIMACHAL PR</asp:ListItem>
                            <asp:ListItem Value="14">JAMMU KASHMIR</asp:ListItem>
                            <asp:ListItem Value="35">JHARKHAND</asp:ListItem>
                            <asp:ListItem Value="15">KARNATAKA</asp:ListItem>
                            <asp:ListItem Value="16">KERALA</asp:ListItem>
                            <asp:ListItem Value="17">LAKHSWADEEP</asp:ListItem>
                            <asp:ListItem Value="18">MADHYA PRADESH</asp:ListItem>
                            <asp:ListItem Value="19">MAHARASHTRA</asp:ListItem>
                            <asp:ListItem Value="20">MANIPUR</asp:ListItem>
                            <asp:ListItem Value="21">MEGHALAYA</asp:ListItem>
                            <asp:ListItem Value="22">MIZORAM</asp:ListItem>
                            <asp:ListItem Value="23">NAGALAND</asp:ListItem>
                            <asp:ListItem Value="24">ORISSA</asp:ListItem>
                            <asp:ListItem Value="99">OTHER</asp:ListItem>
                            <asp:ListItem Value="25">PONDICHERRY</asp:ListItem>
                            <asp:ListItem Value="26">PUNJAB</asp:ListItem>
                            <asp:ListItem Value="27">RAJASTHAN</asp:ListItem>
                            <asp:ListItem Value="28">SIKKIM</asp:ListItem>
                            <asp:ListItem Value="29">TAMILNADU</asp:ListItem>
                            <asp:ListItem Value="36">TELENGANA</asp:ListItem>
                            <asp:ListItem Value="30">TRIPURA</asp:ListItem>
                            <asp:ListItem Value="31">UTTAR PRADESH</asp:ListItem>
                            <asp:ListItem Value="34">UTTRAKHAND</asp:ListItem>
                            <asp:ListItem Value="32">WEST BENGAL</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>Pin</td>
                    <td>
                        <asp:TextBox ID="txtPin" runat="server" Style="width: 100px" required="required" MaxLength="6" placeholder="PIN"></asp:TextBox></td>
                    <td>
                        <asp:Button CssClass="btn btn-sm btn-primary" ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" />
                        <a href="/Vendor.aspx" class="btn btn-sm btn-success">New</a>
                        <asp:Label ID="lblStatus" runat="server"></asp:Label>
                    </td>
                </tr>

            </table>
        </div>
    </div>
    <asp:Label ID="lblDataTable" runat="server" Text=""></asp:Label>
</asp:Content>

