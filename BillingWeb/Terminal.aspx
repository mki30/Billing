<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeFile="Terminal.aspx.cs" Inherits="Terminal" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">

<head id="Head1" runat="server">
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
                        <td class="hidden">
                            <asp:Label ID="lblID" runat="server" Text="0"></asp:Label>
                        </td>
                        <td>Store</td>
                        <td>
                            <asp:Label ID="lblStoreID" runat="server" Text="0" CssClass="hidden"></asp:Label>
                            <asp:Label ID="lblStore" runat="server" Text=""></asp:Label>
                        </td>   
                        <td>Name</td>
                        <td>
                            <asp:TextBox ID="txtName" runat="server" required="required" MaxLength="20"></asp:TextBox></td>
                        <td>Bill No</td>
                        <td style="width: 100px;">
                            <asp:TextBox ID="txtBillNo" runat="server" required="required" MaxLength="10"></asp:TextBox>
                        </td>
                        <td>Prefix</td>
                        <td>
                            <asp:TextBox ID="txtPrefix" runat="server" required="required" MaxLength="10" Width="50"></asp:TextBox>
                        </td>
                        <td>
                            <asp:CheckBox ID="chkIsVertual" runat="server" /> Virtual
                        </td>
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
