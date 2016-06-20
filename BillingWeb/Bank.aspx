<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Bank.aspx.cs" Inherits="Bank" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script>
        function ShowData(id)
        {
            location.href = "/Brand.aspx?id=" + id;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="panel panel-info">
        <div class="panel-heading">
            <table class="table-condensed">
                <tr>
                    <td class="hidden">ID:<asp:Label ID="lblID" runat="server" Text="0"></asp:Label></td>
                    <td>Name</td>
                    <td>
                        <asp:TextBox ID="txtName" runat="server"  required="required" placeholder="Bank Name"></asp:TextBox></td>
                    <td>Account No</td>
                    <td>
                        <asp:TextBox ID="txtAccountNo" runat="server"  required="required" placeholder="Account Number"></asp:TextBox></td>
                    <td>Address</td>
                    <td>
                        <asp:TextBox ID="txtAddress" runat="server"  required="required" placeholder="Address"></asp:TextBox></td>

                    <td>
                        <asp:Button ID="btnSave" CssClass="btn btn-sm btn-primary" runat="server" Text="Save" OnClick="btnSave_Click" OnClientClick="return Validate();" />
                        <asp:Label ID="lblStatus" runat="server"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <br />
    <asp:Label ID="lblDataTable" runat="server" Text=""></asp:Label>


</asp:Content>

