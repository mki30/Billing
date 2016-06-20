<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Payment.aspx.cs" Inherits="Payment" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        #paymentTable td:nth-child(2),td:nth-child(4)
        {
            white-space:nowrap;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server" ViewStateMode="Disabled">
    <script>
        $(document).ready(function ()
        {
            $("#txtItemSearch").hide();

        });

        function AddNewPayment(paymentMode)
        {
            debugger;
            if ($("#ddVendor").val() == "")
            {
                alert("Please select a vendor");
                return;
            }
            if (paymentMode == 'cash')
                $(".cheque").hide();
            else
                $(".cash").hide();
            EditPayment(0);
        }

        function EditPayment(ID)
        {
            OpenPopup();
            $('#txtCash').focus()
            if (ID == 0)
            {
                $("#txtVendorID").val($("#ddVendor").val());
                return;
            }
            $.ajax({
                url: "/Data.aspx?Action=GetPayment&Data1=" + ID, cache: false, success: function (obj)
                {
                    $("#txtID").val(obj.ID);
                    $("#txtPurchaseID").val(obj.PurchaseID);
                    $("#txtVendorID").val(obj.VendorID);
                    $("#txtCash").val(obj.Cash);
                    $("#txtDate").val(obj.PaymentDateString);
                    $("#txtCheque").val(obj.Cheque);
                    $("#txtChequeNo").val(obj.ChequeNo);
                    $("#ddBank").val(obj.BankID);
                    $("#txtLedgerNo").val(obj.LedgerNo);
                    $("#ddPaymentStore").val(obj.StoreID);
                    $("#chkDelete").prop("checked", obj.IsDelete);

                    if (obj.Cash == 0)
                        $(".cash").hide();
                    else
                        $(".cheque").hide();

                    GetPurchasePaymentLink();
                }
            });
        }

        function Save()
        {
            var str = $("#paymentForm :input").serialize();
            var data = str.replace(/ctl00%24ContentPlaceHolder1%24/g, '');
            $.ajax({
                type: "POST",
                url: "/Data.aspx?Action=SavePayment",
                data: data,
                cache: false,
                success: function (obj)
                {
                    location.reload();
                }
            });
        }

        function UpdatePurchaseLinkAmount(ID)
        {
            var amt = prompt("Please enter paid amount against this purchase", "");
            if (amt == null)
                return;
            $.ajax({
                url: "/Data.aspx?Action=UpdatePurchasePaymentLink&Data1=" + ID + "&Data4=" + amt,
                cache: false,
                success: function (data)
                {
                    GetPurchasePaymentLink();
                }
            });
            return false;
        }

        function PurchaseSelect(ID) //Purchase payment link
        {
            $.ajax({
                url: "/Data.aspx?Action=UpdatePurchasePaymentLink&Data1=0&Data2=" + $("#txtID").val() + "&Data3=" + ID,
                cache: false,
                success: function (data)
                {
                    GetPurchasePaymentLink();

                    if ($("#tdlinkedPurchase").html() != "")
                        GetPurchaseList();
                }
            });
            return false;
        }

        function GetPurchasePaymentLink()
        {
            $.ajax({
                url: "/Data.aspx?Action=GetPurchasePaymentLink&Data1=" + $("#txtID").val(),
                cache: false,
                success: function (data)
                {
                    $("#tdPurchaseResult").html(data);
                }
            });
        }

        function GetPurchaseList()
        {
            if ($("#txtID").val() === "0")
            {
                alert("Please save payment first then search.")
                return;
            }
            $.ajax({
                url: "/Data.aspx?Action=GetPurchaseList&Data1=" + $("#txtDateSearch").val() + "&Data2=" + $("#txtDateSearch").val() + "&Data3=" + $("#txtVendorID").val() + "&Data4=" + $("#txtID").val(),
                cache: false,
                success: function (data)
                {
                    $("#tdlinkedPurchase").html(data);
                }
            });
        }

        function DeletePurchasePaymentLink(PurchasePaymentLinkID)
        {
            $.ajax({
                url: "/Data.aspx?Action=DeletePurchasePaymentLink&Data1=" + PurchasePaymentLinkID,
                cache: false,
                success: function (data)
                {
                    if (data == "")
                    {
                        GetPurchasePaymentLink();
                        if ($("#tdlinkedPurchase").html() != "")
                            GetPurchaseList();
                    }
                }
            });
        }
        var fromDate = '<%=FilterDateFrom.ToString("dd-MMM-yyyy")%>', toDate = '<%=FilterDateTo.ToString("dd-MMM-yyyy")%>';
    </script>
    <div class="panel panel-info">
        <div class="panel-heading">
            <table style="width: 100%">
                <tr>
                    <td style="width: 100px;">
                        <asp:DropDownList ID="ddVendor" runat="server" onchange="location.href='/Payment.aspx?filterdatefrom='+fromDate+'&filterdateto='+toDate+'&vendorid=' + $(this).val()"></asp:DropDownList>
                    </td>
                    <td>
                        <a onclick="AddNewPayment('cash')" class="btn btn-success btn-sm">
                            <i class="glyphicon glyphicon-plus"></i>Add Cash
                        </a>
                        <a onclick="AddNewPayment('cheque')" class="btn btn-primary btn-sm">
                            <i class="glyphicon glyphicon-plus"></i>Add Cheque
                        </a>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddSelectStore" runat="server" onchange="location.href='/Payment.aspx?filterdatefrom='+fromDate+'&filterdateto='+toDate+'&vendorid=' + $('#ddVendor').val()+'&store='+$(this).val()"></asp:DropDownList>
                    </td>
                    <td style="width: 300px;">
                        <asp:Literal ID="ltFilters" runat="server"></asp:Literal>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div>
        <asp:Label ID="lblPaymentList" runat="server" Text=""></asp:Label>
    </div>
    <div>
        <asp:Label ID="lblDueList" runat="server" Text=""></asp:Label>
    </div>
    <div>
        <asp:Label ID="lblItemList" runat="server" Text=""></asp:Label>
    </div>

    <div id="divDialog" class="dialog">
        <span onclick='location.reload()' style="cursor: pointer;" class="pull-right glyphicon glyphicon-remove" aria-hidden="true"></span>
        <div id="paymentForm">
            <table>
                <tr>
                    <td>
                        <table>
                            <tr>
                                <td style="vertical-align: top">
                                    <table class="table-condensed">
                                        <tr style="display: none;">
                                            <td>
                                                <asp:TextBox ID="txtID" runat="server" Text="0"></asp:TextBox></td>
                                            <td>
                                                <asp:TextBox ID="txtPurchaseID" runat="server" Text="0"></asp:TextBox></td>
                                            <td>
                                                <asp:TextBox ID="txtVendorID" runat="server" Text="0"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td class="cash">Cash</td>
                                            <td class="cheque">Cheque Amount</td>
                                            <td class="cheque">Cheque No</td>
                                            <td class="cheque">Bank</td>
                                            <td>Ledger No</td>
                                            <td>Payment Date</td>
                                            <td></td>
                                        </tr>
                                        <tr>
                                            <td class="cash">
                                                <asp:TextBox ID="txtCash" runat="server" Placeholder="Cash"></asp:TextBox></td>
                                            <td class="cheque">
                                                <asp:TextBox ID="txtCheque" runat="server" Placeholder="Cheque Amount"></asp:TextBox></td>
                                            <td class="cheque">
                                                <asp:TextBox ID="txtChequeNo" runat="server" Placeholder="Cheque No"></asp:TextBox></td>
                                            <td class="cheque">
                                                <asp:DropDownList ID="ddBank" runat="server"></asp:DropDownList></td>
                                            <td>
                                                <asp:TextBox ID="txtLedgerNo" runat="server" Placeholder="Ledger No" Style="width: 100px;"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtDate" runat="server" CssClass="datepicker" Placeholder="  Date" required="required" Width="7em"></asp:TextBox></td>
                                            <td>
                                                <asp:DropDownList ID="ddPaymentStore" runat="server" Visible="false"></asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkDelete" runat="server" />
                                                Delete 
                                            </td>
                                            <td>
                                                <a href="#" class="btn btn-sm btn-primary" onclick="Save()">Save</a>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblMsg" runat="server" Text=""></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>

                                    <table>
                                        <tr>
                                            <td>
                                                <input id="txtDateSearch" type="text" class="datepicker" placeholder="Bill Date" /><input id="btnSearch" type="button" value="Search" onclick="GetPurchaseList()" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td id="tdPurchaseResult"></td>
                                        </tr>
                                        <tr>
                                            <td id="tdlinkedPurchase"></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>

        </div>
    </div>
    <div id="divDialogfade" class="black_overlay"></div>
</asp:Content>

