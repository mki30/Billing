<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Apartments.aspx.cs" Inherits="Apartments" %>

<%@ Register Src="~/Contols/CustomerEdit.ascx" TagPrefix="uc1" TagName="CostomerEdit"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script>
        $(document).ready(function ()
        {
            $('#txtMobileSearch').on('keypress', function (event)
            {
                if (event.which === 13)
                {
                    serchCustomer();
                }
            });

            $("#ddProjectCity").change(function ()
            {
                location.href = "/Apartments.aspx?City=" + $(this).val();
            });
        });

        function ShowCustomerLink(AprtmentID)
        {
            $("#linkCustomer").hide();
            $("#txtApartmentID").val(AprtmentID);
            $("#txtCustomerID").val();
            $("#divCustomer").html("");
            $("#txtMobileSearch").val("");
            OpenPopup();
            return false;
        }

        function UpdateLastPurchase(mobile, customerID)
        {
            $.ajax({
                url: "/Data.aspx?Action=UpdateLastPurchase&Data1=" + mobile + "&Data2=" + customerID,
                cache: false,
                success: function (obj)
                {
                    location.reload();
                }
            });
        }

        function serchCustomer()
        {
            $.ajax({
                url: "/Data.aspx?Action=GetCustomerByMobile&Data1=" + $("#txtMobileSearch").val(),
                cache: false,
                success: function (obj)
                {
                    if (obj != "")
                    {
                        var data = JSON.parse(obj);
                        $("#divCustomer").html("<table class='table table-bordered'><tr><td>" + data.Name + "<td>" + data.Address + "<td><a id='linkCustomer' href='#' onclick='LinkCustomerWithApartment()' class='btn btn-success btn-sm'>Link</a></table>");
                        $("#txtCustomerID").val(data.ID);
                        $("#linkCustomer").show();
                    }
                    else
                        $("#divCustomer").html("No Customer Found for this contact!");
                }
            });
            return false
        }

        function LinkCustomerWithApartment()
        {
            $.ajax({
                url: "/Data.aspx?Action=LinkCustomerWithApartment&Data1=" + $("#txtCustomerID").val() + "&Data2=" + $("#txtApartmentID").val() + "&Data3=" + $("#ddProjects").val(),
                cache: false,
                success: function (obj)
                {
                    location.reload();
                }
            });
            return false
        }

        function GetProject(ID)
        {
            OpenPopup3();
            $.ajax({
                url: "/Data.aspx?Action=GetProject&Data1=" + ID,
                cache: false,
                success: function (obj)
                {
                    $("#txtID").val(obj.ID),
                    $("#txtProjectName").val(obj.ProjectName),
                    $("#txtProjectAddress").val(obj.Address),
                    $("#txtProjectArea").val(obj.Area),
                    $("#txtProjectCity").val(obj.City),
                    $("#txtState").val(obj.State),
                    $("#txtPIN").val(obj.Pin)
                }
            });
            return false;
        }

        function AddNewProject()
        {
            OpenPopup3();
            $(".dialog input:text").val("");
        }

        function SaveProject()
        {
            if ($("#txtProjectName").val() == "")
            {
                aert("Please enter project name");

            }
            $("#btnSave").val("Saving...");
            $.ajax({
                type: "POST",
                url: "/Data.aspx?Action=SaveProject&Data1=",
                data: {
                    ID: $("#txtID").val(),
                    ProjectName: $("#txtProjectName").val(),
                    Address: $("#txtProjectAddress").val(),
                    Area: $("#txtProjectArea").val(),
                    City: $("#txtProjectCity").val(),
                    State: $("#txtState").val(),
                    PIN: $("#txtPIN").val()
                },
                cache: false,
                success: function (obj)
                {
                    location.reload();
                }
            });
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server" ViewStateMode="Disabled">
    <div id="divDialog" class="dialog">
        <span onclick="ClosePopup()" style="cursor: pointer; color: red;" class="pull-right glyphicon glyphicon-remove"></span>
        <div style="display: none">
            <input type="text" id="txtApartmentID" />
            <input type="text" id="txtCustomerID" />
        </div>
        <input type="text" id="txtMobileSearch" placeholder="Type mobile no and press enter to serach.." maxlength="12" />
        <br />
        <div id="divCustomer"></div>
    </div>
    <a href="#" onclick="AddNewProject()" class="btn btn-sm btn-success">+Add New Project</a>
    <asp:DropDownList ID="ddProjectCity" runat="server">
    </asp:DropDownList>
    &nbsp;<table>
        <tr>
            <td style="width: 30%; vertical-align: top;">
                <asp:Label ID="lblProjectList" runat="server" Text="" ViewStateMode="Disabled"></asp:Label></td>
            <td style="vertical-align: top;">
                <asp:Literal ID="lblCustomerList" runat="server" ViewStateMode="Disabled"></asp:Literal></td>
        </tr>
    </table>
    <uc1:CostomerEdit runat="server" ID="CostomerEdit" ViewStateMode="Disabled"/>

    <div id="divDialog3" class="dialog" style="display: none;">
        <span onclick="ClosePopup3()" style="cursor: pointer; color: red;" class="pull-right glyphicon glyphicon-remove"></span>
        <div>
            <table>
                <tr style="display: none;">
                    <td>ID</td>
                    <td>
                        <input type="text" id="txtID" /></td>
                </tr>
                <tr>
                    <td>Project</td>
                    <td>
                        <input type="text" id="txtProjectName" /></td>
                </tr>
                <tr>
                    <td>Address</td>
                    <td>
                        <input type="text" id="txtProjectAddress" /></td>
                </tr>
                <tr>
                    <td>Area</td>
                    <td>
                        <input type="text" id="txtProjectArea" /></td>
                </tr>
                <tr>
                    <td>City</td>
                    <td>
                        <input type="text" id="txtProjectCity" /></td>
                </tr>
                <tr>
                    <td>State</td>
                    <td>
                        <input type="text" id="txtState" /></td>
                </tr>
                <tr>
                    <td>PIN</td>
                    <td>
                        <input type="text" id="txtPIN" /></td>
                </tr>
                <tr>
                    <td></td>
                    <td><a href="#" onclick="SaveProject()" id="btnSave" class="btn btn-sm btn-success" style="margin-top: 5px;">Save</a></td>
                </tr>
            </table>
        </div>
    </div>
    <div id="divDialogfade" class="black_overlay"></div>
</asp:Content>

