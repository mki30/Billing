<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="RRAadmin.aspx.cs" Inherits="RRAadmin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script>
        $(document).ready(function ()
        {
            ShowCompany(1);
        });
        function ShowCompany(id)
        {
            $("#editFrame")[0].src = "/Company.aspx?ID=" + id;
        }

        function ShowStore(id, companyID)
        {
            $("#editFrame")[0].src = "/Store.aspx?ID=" + id + "&companyid=" + companyID;
        }

        function ShowTerminal(id, storeID)
        {
            $("#editFrame")[0].src = "/Terminal.aspx?ID=" + id + "&storeid=" + storeID;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table>
        <tr>
            <td style="vertical-align: top;">
                <asp:Literal ID="ltList" runat="server"></asp:Literal>
            </td>
            <td>
                <iframe id="editFrame" style="width: 1000px; height: 500px; border: 1px solid #e7e7e7;" />
            </td>
        </tr>

    </table>
</asp:Content>

