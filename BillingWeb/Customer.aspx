<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Customer.aspx.cs" Inherits="Customer" %>

<%@ Register Src="~/Contols/CustomerEdit.ascx" TagPrefix="uc1" TagName="CostomerEdit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        $(document).ready(function ()
        {
            $('#txtSearch').keypress(function (e)
            {
                if (e.which == 13)
                {
                    event.preventDefault();
                    ShowCuntomerData($("#txtSearch").val(), 0);
                }
            });
        });
        function ShowCuntomerData(mobile)
        {
            $("#divbillList").html("");
            $("#divbillDetail").html("");
            OpenPopup();
            if (mobile != "")
            {
                GetCustomerList(mobile);
                GetBillingList('', '', mobile);
            }
            return false;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server" ViewStateMode="Disabled">
    <div style="width: 25%">
    </div>
    <table>
        <tr>
            <td>
                <asp:TextBox ID="txtSearch" runat="server" placeholder="Type mobile no and press enter" Style="width: 300px;" MaxLength="10"></asp:TextBox></td>
            <td><a onclick="return ShowEdit(0);" class="btn btn-success btn-sm">
                <i class="glyphicon glyphicon-plus"></i>Add New Customer
            </a></td>
        </tr>
    </table>
    <div id="divDialog" class="dialog">
        <span onclick="ClosePopup()" style="cursor: pointer; color: red;" class="pull-right glyphicon glyphicon-remove"></span>
        <div id="divCustomerList"></div>
        <div id="divbillList"></div>
        <div id="divbillDetail"></div>
    </div>
    <uc1:CostomerEdit runat="server" ID="CostomerEdit" ViewStateMode="Disabled" />
    <div id="divDialogfade" class="black_overlay"></div>
    <asp:Label ID="customerList" runat="server" Text="" EnableViewState="false"></asp:Label>
</asp:Content>

