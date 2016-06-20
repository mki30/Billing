<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="EmpAttendance.aspx.cs" Inherits="EmpAttendance" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script>
        function SaveAttendance(EmpID, In)
        {
            $.ajax({
                url: "/Data.aspx?Action=SaveAttendance&Data1=" + EmpID + "&data2=" + In, cache: false, success: function (data)
                {
                    location.reload();
                }
            });
        }

        function AddNewAttendance()
        {
            if ($("#ddEmployee").val() == "")
                alert("Please select employee");
            else
                EditAttendance(0, $("#ddEmployee").val());
        }

        function EditAttendance(ID, empID)
        {
            OpenPopup();
            if (ID == 0)
            {
                $("#txtEmployeeID").val(empID);
                return;
            }
            $.ajax({
                url: "/Data.aspx?Action=GetAttendance&Data1=" + ID, cache: false, success: function (obj)
                {
                    $("#txtID").val(obj.ID);
                    $("#txtDate").val(obj.DateString);
                    $("#txtIn").val(obj.DateInString.split(' ')[1]);
                    $("#txtOut").val(obj.DateOutString.split(' ')[1]);
                    $("#ddLeaveType").val(obj.LeaveType);
                    $("#txtEmployeeID").val(obj.EmployeeID);
                }
            });
        }

        function SaveAttendance2()
        {
            var str = $("#AttEdit :input").serialize();
            var data = str.replace(/ctl00%24ContentPlaceHolder1%24/g, '');
            $.ajax({
                type: "POST",
                url: "/Data.aspx?Action=SaveAttendance2&Data1=",
                data: data,
                cache: false,
                success: function (obj)
                {
                    location.reload();
                }
            });
        }

        function DeleteAttandance()
        {

            $.ajax({
                url: "/Data.aspx?Action=DeleteAttendance&Data1=" + $("#txtID").val(),
                cache: false,
                success: function (obj)
                {
                    location.reload();
                }
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="panel panel-info">
        <div class="panel-heading">
            <table style="width: 100%">
                <tr>
                    <td>
                        <asp:DropDownList ID="ddEmployee" runat="server" onchange="ChangeURL(this,'employeeid')"></asp:DropDownList>
                    </td>
                    <td>
                        <a onclick="AddNewAttendance()" class="btn btn-success btn-sm">
                            <i class="glyphicon glyphicon-plus"></i>Add New Attendance
                        </a>
                    </td>
                    <td style="width: 300px">
                        <asp:Literal ID="ltFilters" runat="server"></asp:Literal></td>
                </tr>

            </table>
        </div>
    </div>

    <table style="width: 100%">
        <tr>
            <td style="vertical-align: top">
                <asp:Label ID="ltEmployeeList" runat="server"></asp:Label>
            </td>
            <td style="vertical-align: top">
                <asp:Label ID="lblDataTable" runat="server" Text=""></asp:Label></td>
        </tr>
    </table>

    <div id="divDialog" class="dialog" style="width: 400px; margin: 0 auto !important;">
        <span onclick="ClosePopup()" style="cursor: pointer;" class="pull-right glyphicon glyphicon-remove" aria-hidden="true"></span>
        <div id="AttEdit">
            <table class="table-condensed">
                <tr>
                    <td style="display: none">
                        <asp:TextBox ID="txtID" runat="server" Text="0"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="display: none">
                        <asp:TextBox ID="txtEmployeeID" runat="server" Text="0"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>Date</td>
                    <td>
                        <asp:TextBox ID="txtDate" runat="server" CssClass="datepicker" required="reqiuired" MaxLength="12"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>In</td>
                    <td>
                        <asp:TextBox ID="txtIn" runat="server" required="reqiuired" MaxLength="5" placeholder="HH:MM"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>Out</td>
                    <td>
                        <asp:TextBox ID="txtOut" runat="server" required="reqiuired" MaxLength="5" placeholder="HH:MM"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>Leave</td>
                    <td>
                        <asp:DropDownList ID="ddLeaveType" runat="server"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <a href="#" class="btn btn-sm btn-primary" id="btnSave" onclick="SaveAttendance2()">Save</a>
                    </td>
                    <td>
                        <a href="#" class="btn btn-sm btn-warning" id="btnDelete" onclick="DeleteAttandance()">Delete</a>
                    </td>
                </tr>
            </table>
        </div>
    </div>

    <div id="divDialogfade" class="black_overlay"></div>

</asp:Content>

