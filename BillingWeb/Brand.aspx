<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Brand.aspx.cs" Inherits="Brand" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script>
        function ShowData(id)
        {
            location.href = "/Brand.aspx?id=" + id;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server" ViewStateMode="Disabled">
    <div class="panel panel-info">
        <div class="panel-heading">
            <table class="table-condensed" style="width: 50%">
                <tr>
                    <td class="hidden">ID:<asp:HiddenField ID="hdID" Value="0" runat="server"/>
                    </td>
                    <td class="hidden">Name</td>
                    <td>
                        <asp:TextBox ID="txtName" runat="server"  required="required" placeholder="Add new brand"></asp:TextBox></td>
                    <td>
                        <asp:FileUpload ID="FileUpload1" runat="server" /></td>
                    <td>
                        <asp:Button ID="btnSave" CssClass="btn btn-sm btn-primary" runat="server" Text="Save" OnClick="btnSave_Click" OnClientClick="return Validate();" />
                        <asp:Label ID="lblStatus" runat="server"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <table style="width: 100%;">
        <tr>
            <td style="width: 500px;vertical-align:top;">
                <asp:Label ID="lblDataTable" runat="server" Text=""></asp:Label>
            </td>
            <td style="vertical-align: top; text-align: left;">
                <img src="#" id="imgBrand" runat="server" />
            </td>
        </tr>
    </table>
</asp:Content>

