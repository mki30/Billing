<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ExpenseLog.aspx.cs" Inherits="ExpenseLog" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        $(document).ready(function ()
        {
            $("#txtItemSearch").hide();
        }
        );
        function AddNewExpense()
        {
            EditExpense(0);
        }
        function EditExpense(ID)
        {
            debugger;
            OpenPopup();
            if (ID == 0)
                return;
            $.ajax({
                url: "/Data.aspx?Action=GetExpense&Data1=" + ID, cache: false, success: function (obj)
                {
                    $("#txtID").val(obj.ID);
                    $("#ddTerminal").val(obj.TerminalID);
                    $("#txtName").val(obj.Name);
                    $("#txtAmount").val(obj.Amount);
                    $("#txtDate").val(obj.DateString);
                    $("#ddExpenseType").val(obj.expenseType);
                }
            });
        }
        function Save()
        {
            if ($("#txtName").val() == "")
            {
                alert("Please enter Name");
                $("#txtName").focus();
                return;

            }
            if ($("#txtAmount").val() == "")
            {
                alert("Please enter Amt");
                $("#txtAmount").focus();
                return;
            }
            if ($("#txtDate").val() == "")
            {
                alert("Please enter Date");
                $("#txtDate").focus();
                return;
            }

            var str = $("#ExpenceForm :input").serialize();
            var data = str.replace(/ctl00%24ContentPlaceHolder1%24/g, '');
            $.ajax({
                type: "POST",
                url: "/Data.aspx?Action=SaveExpense&Data1=",
                data: data,
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

    <div class="panel panel-info">
        <div class="panel-heading">
            <table style="width: 100%">
                <tr>
                    <td>
                        <a onclick="AddNewExpense()" class="btn btn-success btn-sm">
                            <i class="glyphicon glyphicon-plus"></i>Add New Expense
                        </a>
                    </td>
                    <td style="width: 300px">
                        <asp:Literal ID="ltFilters" runat="server"></asp:Literal></td>
                </tr>
            </table>

        </div>
    </div>

    <div id="divData">
        <asp:DropDownList ID="ddExpenseTypeSelect" runat="server" onchange="location.href='/ExpenseLog.aspx?filterdatefrom='+FilterDateFrom+'&filterdateto='+FilterDateTo+'&expensetype=' + $(this).val()"></asp:DropDownList><asp:Label ID="lblDataTable" runat="server" Text="Label"></asp:Label>
    </div>
    <div id="divDialog" class="dialog">
        <span onclick="ClosePopup()" style="cursor: pointer;" class="pull-right glyphicon glyphicon-remove" aria-hidden="true"></span>
        <div id="ExpenceForm">
            <table class='table-condensed' style="width: 50%">
                <tr>
                    <td class="hidden">ID:<asp:Label ID="lblID" runat="server" Text="0"></asp:Label>
                        <asp:TextBox ID="txtID" runat="server" Text="0"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>Terminal</td>
                    <td>
                        <asp:DropDownList ID="ddTerminal" runat="server" required="required"></asp:DropDownList></td>
                </tr>
                <tr>
                    <td>Name</td>
                    <td>
                        <asp:TextBox ID="txtName" runat="server" placeholder="Name" required="required" MaxLength="50"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>Exp Type</td>
                    <td>
                        <asp:DropDownList ID="ddExpenseType" runat="server"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>Amt</td>
                    <td>
                        <asp:TextBox ID="txtAmount" runat="server" placeholder="Amount" required="required" MaxLength="5"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>Date</td>
                    <td>
                        <asp:TextBox ID="txtDate" runat="server" placeholder="Date" CssClass="datepicker" required="required" MaxLength="11"></asp:TextBox></td>
                </tr>
                <tr>
                    <td></td>
                    <td>
                        <a href="#" class="btn btn-sm btn-primary" onclick="Save()" style="margin-top: 5px;">Save</a>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div id="divDialogfade" class="black_overlay"></div>
</asp:Content>

