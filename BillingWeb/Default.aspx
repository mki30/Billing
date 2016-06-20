<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Default" ViewStateMode="Disabled" MasterPageFile="~/MasterPage2.master" ClientIDMode="Static" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="css/bootstrap-datetimepicker.min.css" rel="stylesheet" />
    <script src="js/bootstrap-datetimepicker.min.js"></script>
    <script src="/js/common.js"></script>
    <script src="/js/QueryString.js"></script>
    <script src="/js/MeatMart.js"></script>
    <script src="/js/date.js"></script>
    <style type="text/css">
        .drp
        {
            height: 30px;
        }
        #divCart .panel .col-md-6
        {
            padding-left: 1px;
            padding-right: 1px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="container">
    </div>
    <div class="container">
        <ol class="breadcrumb">
        </ol>
        <div class="row" id="divTopData">
            <div id="divCategoryList" runat="server" style="display: none">
                <div class='panel panel-info'>
                    <div class='panel-heading'>Product Categories</div>
                    <div class='panel-body'>
                        <asp:Literal ID="ltCategory" runat="server" ViewStateMode="Disabled"></asp:Literal>
                    </div>
                </div>
            </div>
            <div id="divBrandList" runat="server" style="display: none">
                <div class='panel panel-info'>
                    <div class='panel-heading'>Product Brands</div>
                    <div class='panel-body'>
                        <asp:Literal ID="ltBrands" runat="server" ViewStateMode="Disabled"></asp:Literal>
                    </div>
                </div>
            </div>
            <div id="divCartList" runat="server" style="display: none">
                <div id="divCart">
                </div>
            </div>
            <div id="divOrderList" runat="server" style="display: none">
            </div>
        </div>
        <div class="row" id="divItem">
            <asp:Literal ID="ltItems" runat="server" ViewStateMode="Disabled"></asp:Literal>
        </div>
    </div>
</asp:Content>
