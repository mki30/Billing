<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeFile="Store.aspx.cs" Inherits="Store" %>


<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">

<head id="Head1" runat="server">
    <title>Store Edit</title>
    <link href="/BootStrap/css/bootstrap.min.css" rel="stylesheet" />
    <style type="text/css">
        select
        {
            padding: 3px;
        }

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
                <table class='table-condensed'>
                    <tr>
                        <td class="hidden">
                            <asp:Label ID="lblID" runat="server" Text="0"></asp:Label>
                        </td>
                        <td>Company</td>
                        <td>
                            <asp:Label ID="lblCompanyID" runat="server" Text="0" CssClass="hidden"></asp:Label>
                            <asp:Label ID="lblCompany" runat="server" Text=""></asp:Label>
                          </td>
                        <td>Name</td>
                        <td>
                            <asp:TextBox ID="txtName" runat="server" required="required" MaxLength="20"></asp:TextBox></td>
                        <td>Phone</td>
                        <td>
                            <asp:TextBox ID="txtPhone" runat="server" required="required" MaxLength="100"></asp:TextBox>
                        </td>
                        <td>
                            <asp:CheckBox ID="chkIsFranchise" runat="server" Text="&nbsp;Is Franchise" />
                        </td>
                    </tr>
                    <tr>
                        <td>Address1</td>
                        <td colspan="3">
                            <asp:TextBox ID="txtAddress1" runat="server" required="required" MaxLength="40" Style="width: 100%;"></asp:TextBox>
                        </td>
                        <td>Address2</td>
                        <td colspan="2">
                            <asp:TextBox ID="txtAddress2" runat="server" required="required" MaxLength="50" Style="width: 100%;"></asp:TextBox>
                        </td>
                        <td colspan="2">
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


