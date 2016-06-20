<%@ Page Title="" Language="C#"  AutoEventWireup="true" CodeFile="Company.aspx.cs" Inherits="Company" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head  runat="server">
    <title></title>
    <link href="/BootStrap/css/bootstrap.min.css" rel="stylesheet" />
    <style>
         /*Boootstrap overritten css*/
        input, select
        {
            color: black;
        }

        a
        {
            color: #1479FB;
        }

        .panel .panel-heading, .panel .panel-title
        {
            font-size: 13px;
        }
        /*Boootstrap overritten css*/
    </style>
</head>
<body>
    <form id="form1" runat="server" autocomplete="off">
    <div class="panel panel-info">
        <div class="panel-heading">
            <table class='table-condensed' style="width: 100%">
                <tr>
                    <td class="hidden">ID:<asp:Label ID="lblID" runat="server" Text="0"></asp:Label></td>
                    <td>Name</td>
                    <td>
                        <asp:TextBox ID="txtName" runat="server" required="required" MaxLength="40"></asp:TextBox></td>
                    <td>TIN</td>
                    <td>
                        <asp:TextBox ID="txtTIN" runat="server" required="required" MaxLength="11"></asp:TextBox></td>
                    <td>TAN</td>
                    <td>
                        <asp:TextBox ID="txtTAN" runat="server" required="required" MaxLength="10"></asp:TextBox></td>
                    <td>PAN</td>
                    <td>
                        <asp:TextBox ID="txtPAN" runat="server" required="required" MaxLength="10"></asp:TextBox></td>
                    <td>
                        <a href="/Company.aspx" class="btn btn-sm btn-success">New</a>
                    </td>
                </tr>
                <tr>
                    <td>CST</td>
                    <td>
                        <asp:TextBox ID="txtCst" runat="server" required="required" MaxLength="10" Style="width:100px;"></asp:TextBox>
                    </td>
                    <td>CIN</td>
                    <td>
                        <asp:TextBox ID="txtCIN" runat="server"  required="required" MaxLength="30"></asp:TextBox></td>
                    <td>Address</td>
                    <td colspan="3">
                        <asp:TextBox ID="txtAddress" runat="server" required="required" MaxLength="100" style="width:100%;"></asp:TextBox></td>
                    <td>
                        <asp:Button ID="btnSave" CssClass="btn btn-sm btn-primary" runat="server" Text="Save" OnClick="btnSave_Click" />
                        <asp:Label ID="lblStatus" runat="server"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <asp:Label ID="lblDataTable" runat="server" Text=""></asp:Label>
 </form>
</body>
</html>

