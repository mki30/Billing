<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Employee.aspx.cs" Inherits="Employee" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/ecmascript">
        var employeeID = 0;
        var storeID = 0;
        function UpdateStore()
        {
            $.ajax({
                url: "/Data.aspx?Action=AssignStore&Data1=" + employeeID + "&Data2=" + $("#ddStoreEmp").val(), cache: false, success: function (data)
                {
                    if (data == "")
                        location.reload();
                }
            });
        }

        function RemoveStore()
        {
            $.ajax({
                url: "/Data.aspx?Action=RemoveStore&Data1=" + employeeID + "&Data2=" + storeID, cache: false, success: function (data)
                {
                    if (data == "")
                        location.reload();
                }
            });
        }

        function assignStore(empId, storeId)
        {
            $("#linkRemove").show();
            if (storeId == "0")
                $("#linkRemove").hide();
            $("#ddStoreEmp").val(storeId);
            employeeID = empId;
            storeID = storeId;
            OpenPopup();
        }
    </script>
    <style>
        table.datatable td:nth-child(2)
        {
            white-space: nowrap;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="divDialog2" class="dialog" style="width:40%;">
        <span onclick="ClosePopup2()" style="cursor: pointer;" class="glyphicon glyphicon-remove" aria-hidden="true"></span>
        <table class='table-condensed' style="width: 100%">
            <tr>
                <td class="hidden">ID:<asp:Label ID="lblID" runat="server" Text="0"></asp:Label></td>
                <td style="width:10%;">Name</td>
                <td>
                    <asp:TextBox ID="txtName" runat="server" required="required" MaxLength="50" placeholder="Name"></asp:TextBox></td>
            </tr>
            <tr>
                <td>DOJ</td>
                <td>
                    <asp:TextBox ID="txtDOJ" runat="server" CssClass="datepicker" required="required" MaxLength="12" Width="120px" placeholder="DOJ"></asp:TextBox></td>
            </tr>
            <tr>
                <td>DOB</td>
                <td>
                    <asp:TextBox ID="txtDOB" runat="server" CssClass="datepicker" required="required" MaxLength="12" Width="120px" placeholder="DOB"></asp:TextBox></td>
            </tr>
            <tr>
                <td>Mobile</td>
                <td>
                    <asp:TextBox ID="txtMobile" runat="server" required="required" MaxLength="10" placeholder="Mobile" Width="120px"></asp:TextBox></td>
            </tr>
            <tr>
                <td>Email</td>
                <td colspan="2">
                    <asp:TextBox ID="txtEmail" runat="server" required="required" type="email" MaxLength="50" placeholder="Email" Width="200"></asp:TextBox></td>

            </tr>
            <tr>
                <td>Address</td>
                <td colspan="3">
                    <asp:TextBox ID="txtAddress" runat="server" Style="width: 100%;" required="required" MaxLength="100" placeholder="Address"></asp:TextBox></td>
            </tr>
            <tr>
                <td>Password</td>
                <td>
                    <asp:TextBox ID="txtPassword" runat="server" required="required" MaxLength="20" Width="120px" placeholder="Password"></asp:TextBox></td>
            </tr>
            <tr>
                <td>Store</td>
                <td>
                    <asp:DropDownList ID="ddStore" runat="server"></asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td></td>
                <td>
                    <asp:CheckBox ID="chkAdmin" runat="server" Text="Admin" />
                    <asp:CheckBox ID="ChkDelete" runat="server" Text="Delete" />
                </td>
            </tr>
            <tr>
                <td>Designation</td>
                <td>
                    <asp:DropDownList ID="ddDesignation" runat="server"></asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td></td>
                <td>
                    <asp:Button CssClass="btn btn-sm btn-primary" ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" />
                    <asp:Label ID="lblStatus" runat="server"></asp:Label>
                    <asp:Label ID="lblStores" runat="server"></asp:Label>
                </td>
            </tr>
        </table>
    </div>
    <asp:Label ID="lblDataTable" runat="server" Text=""></asp:Label>
    <div id="divDialog" class="dialog">
        <span onclick="ClosePopup()" style="cursor: pointer;" class="pull-right glyphicon glyphicon-remove" aria-hidden="true"></span>
        <div style="width: 50%;">
            <asp:DropDownList ID="ddStoreEmp" runat="server" CssClass="form-control"></asp:DropDownList>
            <br />
            <a href="#" class="btn btn-sm btn-warning" onclick="UpdateStore()">Add</a>
            <a href="#" id="linkRemove" onclick="RemoveStore()" class="btn btn-sm btn-success">Remove</a>
        </div>
    </div>
    <div id="divDialogfade" class="black_overlay"></div>
</asp:Content>

