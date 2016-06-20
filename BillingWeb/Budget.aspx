<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Budget.aspx.cs" Inherits="Budget" ClientIDMode="Static" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        .MainRow
        {
            background-color: wheat;
        }

        td, th
        {
            text-align: right;
        }

        .RowHead
        {
            text-align: left;
        }

        .Negative
        {
            background-color: pink;
        }

        .HasValue
        {
            background-color: #F5F6CE;
        }

        .small-dialog
        {
            display: none;
            position: absolute;
            -moz-box-shadow: 0 4px 8px 0 rgba(0, 0, 0, 0.2), 0 6px 20px 0 rgba(0, 0, 0, 0.19);
            -webkit-box-shadow: 0 4px 8px 0 rgba(0, 0, 0, 0.2), 0 6px 20px 0 rgba(0, 0, 0, 0.19);
            box-shadow: 0 4px 8px 0 rgba(0, 0, 0, 0.2), 0 6px 20px 0 rgba(0, 0, 0, 0.19);
            -moz-border-radius: 5px;
            -webkit-border-radius: 5px;
            border-radius: 5px;
            background-color: #f8f5f0;
            padding: 16px;
            z-index: 1002;
            overflow: auto;
        }

            .small-dialog span:first-child
            {
                position: absolute;
                top: 2px;
                right: 2px;
                color: #fe4040;
                cursor: pointer;
            }
    </style>
    <script>
        function EditSalary(EmpID, Date, EmpName)
        {
            $("#spanEmpName").html(EmpName);
            $("input:text").val("");
            $("#txtEnployeeID").val(EmpID);
            $("#txtSalaryDate").val(Date);

            $.ajax({
                url: "/Data.aspx?Action=GetSalary&Data1=" + EmpID + "&Data2=" + Date,
                cache: false,
                success: function (obj)
                {
                    $('input:checkbox').removeAttr('checked');
                    if (obj != "")
                    {
                        $("#txtSalaryID").val(obj.ID);
                        $("#ddStoreSelect").val(obj.StoreID);
                        $("#txtEnployeeID").val(obj.EmployeeID);
                        $("#txtSalaryDate").val(obj.SalaryDateString);
                        $("#txtBasic").val(obj.Basic);
                        $("#txtHRA").val(obj.HRA);
                        $("#txtTA").val(obj.TA);
                        $("#txtMedical").val(obj.Medical);
                        $("#txtAdvance").val(obj.Advance);
                        $("#txtTDS").val(obj.TDS);
                        $("#txtLoan").val(obj.Loan);
                    }
                }
            });
            OpenDialogeBox("divSalryEdit");
            $("#txtBasic").focus();
            return false;
        }

          function SaveSalary()
          {
              var formData = $("#divSalryEdit :input").serialize();
              console.log(formData);
              $.ajax({
                  type: "POST",
                  url: "/Data.aspx?Action=SaveSalary&Data1=",
                  data: formData,
                  cache: false,
                  success: function (obj)
                  {
                      location.reload();
                  }
              });
          }

          var objectEdit;
          function EditMonthlyTotal(Date, amountType, storeID, endDate)
          {
              $.ajax({
                  url: "/Data.aspx?Action=GetMonthlyTotal&Data1=" + Date + "&Data2=" + amountType + "&Data3=" + storeID,
                  cache: false,
                  success: function (obj)
                  {
                      objectEdit = obj;
                      $("#tdMonthDate").html(obj.TotalDateString);
                      $("#tdMonthAutoAmount").html(obj.Amount);
                      $("#txtAmountManual").val(obj.AmountManual);
                      $("#txtAmountManual").focus();
                  }
              });
              OpenDialogeBox("divMonthlyTotal");
              switch (amountType)
              {
                  case 1://purchase
                      $("#linkDetail").prop("href", "/Purchase.aspx?filterdatefrom=" + Date + "&filterdateto=" + endDate + "&StoreID=" + storeID).show();
                      break;
                  case 2: //Sale
                      $("#linkDetail").prop("href", "/Report.aspx?FilterDateFrom=" + Date + "&FilterDateTo=" + endDate + "&ReportType=1&StoreID=" + storeID).show();
                      break;
                  case 3://Daily Expense
                      $("#linkDetail").prop("href", "/ExpenseLog.aspx?filterdatefrom=" + Date + "&filterdateto=" + endDate + "&storeid=" + storeID).show();
                      break;
                  case 10: //payment
                      $("#linkDetail").prop("href", "/Payment.aspx?filterdatefrom=" + Date + "&filterdateto=" + endDate + "&storeid=" + storeID).show();
                      break;
                  default:
                      $("#linkDetail").prop("href", "#").hide();
              }
              return false;
          }

          function SaveMonthlyTotal()
          {
              objectEdit.AmountManual = $("#txtAmountManual").val();
              objectEdit["chkUpdateAllMonths"] = $("#chkUpdateAllMonths").prop("checked") ? 1 : 0;

              $.ajax({
                  type: "POST",
                  url: "/Data.aspx?Action=SaveMonthlyTotal&Data1=" + $("#ddStoreBudget").val(),
                  data: objectEdit,
                  cache: false,
                  success: function (obj)
                  {
                      location.reload();
                  }
              });
              CloseDialogeBox();
              $("#divData").html("Loading....");
          }

          function OpenDialogeBox(dialogeID)  //open dialouge 
          {
              var obj = $("#" + dialogeID);
              $(obj).css({ left: ($(window).width() - $(obj).width()) * .5, top: $(document).scrollTop() + ($(window).height() - $(obj).height()) * .5 });
              $(obj).show();

              $('#divDialogfade').show();
              $('#divDialogfade').css("top", $(document).scrollTop());
          }
          $(document).on('keydown', function (e)  //close popup on escape press
          {
              if (e.keyCode === 27)
              {
                  CloseDialogeBox();
              }
          });
          function CloseDialogeBox()  //close popup
          {
              $('.small-dialog').hide();
              $('#divDialogfade').hide();
          }

          function ChangeLocation(update)
          {
              update = update ? update : 0;
              location.href = '?' + $("#ddYear").val() + "&storeid=" + $("#ddStoreBudget").val() + (update ? "&update=1" : "");
          }

          </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:DropDownList ID="ddStoreBudget" runat="server" onchange="ChangeLocation()"></asp:DropDownList>
    <asp:DropDownList ID="ddYear" runat="server" onchange="ChangeLocation()"></asp:DropDownList>
    <input type="button" onclick="ChangeLocation(1)" value="Update Budget" title="Will update auto calculated values for all the months" />
    <asp:Literal ID="ltData" runat="server" EnableViewState="false"></asp:Literal>

    <div id="divSalryEdit" class="small-dialog">
        <span onclick="CloseDialogeBox()" class="pull-right glyphicon glyphicon-remove"></span>
        <table>
            <tr style="display: none;">
                <td>ID</td>
                <td>
                    <input type="text" id="txtSalaryID" name="txtSalaryID" />
                </td>
            </tr>
            <tr>
                <td>Employee</td>
                <td id="spanEmpName" class="font-bold"></td>
            </tr>
            <tr class="hidden">
                <td></td>
                <td>
                    <input type="text" id="txtEnployeeID" name="txtEnployeeID" />
                </td>
            </tr>
            <tr>
                <td>Store</td>
                <td>
                    <asp:DropDownList ID="ddStoreSelect" runat="server" Style="width: 88%;"></asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>Date</td>
                <td>
                    <input type="text" id="txtSalaryDate" name="txtSalaryDate" />
                </td>
            </tr>
            <tr>
                <td>Basic</td>
                <td>
                    <input type="text" id="txtBasic" name="txtBasic" />
                </td>
            </tr>

            <tr>
                <td>HRA</td>
                <td>
                    <input type="text" id="txtHRA" name="txtHRA" />
                </td>
            </tr>

            <tr>
                <td>TA</td>
                <td>
                    <input type="text" id="txtTA" name="txtTA" />
                </td>
            </tr>

            <tr>
                <td>Medical</td>
                <td>
                    <input type="text" id="txtMedical" name="txtMedical" />
                </td>
            </tr>
            <tr>
                <td>Advance</td>
                <td>
                    <input type="text" id="txtAdvance" name="txtAdvance" />
                </td>
            </tr>

            <tr>
                <td>TDS</td>
                <td>
                    <input type="text" id="txtTDS" name="txtTDS" />
                </td>
            </tr>

            <tr>
                <td>Loan</td>
                <td>
                    <input type="text" id="txtLoan" name="txtLoan" />
                </td>
            </tr>
            <tr>
                <td></td>
                <td style="text-align: left;">
                    <input id="chkUpdateRemaining" type="checkbox" name="chkUpdateRemaining" />
                    Update Remaining Months
                </td>
            </tr>
            <tr>
                <td></td>
                <td>
                    <input type="button" value="Save" class="btn btn-success btn-sm" onclick="SaveSalary();" /></td>
            </tr>
        </table>
    </div>
    <div id="divMonthlyTotal" class="small-dialog">
        <span onclick="CloseDialogeBox()" class="pull-right glyphicon glyphicon-remove"></span>
        <table>
            <tr>
                <td>Date</td>
                <td id="tdMonthDate"></td>
            </tr>
            <tr>
                <td>Amount Manual</td>
                <td>
                    <input type="text" id="txtAmountManual" maxlength="6" />
                </td>
            </tr>

            <tr>
                <td>Auto Calculated</td>
                <td id="tdMonthAutoAmount"></td>
            </tr>
            <tr>
                <td></td>
                <td style="text-align: left;">
                    <input id="chkUpdateAllMonths" type="checkbox" />Update Remaining Months
                </td>
            </tr>
            <tr>
                <td></td>
                <td>
                    <a href="#" id="linkDetail" target="_blank" title="view detail report">view detail</a>
                    <input type="button" value="Save" class="btn btn-success btn-sm" onclick="SaveMonthlyTotal();" />
                </td>
            </tr>
        </table>
    </div>
    <div id="divDialogfade" class="black_overlay"></div>
</asp:Content>

