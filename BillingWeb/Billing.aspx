<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Billing.aspx.cs" Inherits="Billing" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        var cashAmount, cardAmount, billingID;
        $(document).ready(function ()
        {
            $("#txtItemSearch").hide();
            $("#ddTerminal").empty();
            $.ajax({
                url: "/Data.aspx?Action=GetTerminalList&Data1=", cache: false, success: function (data)
                {
                    $("#ddTerminal").append($("<option value=''>-Select-</option>"));
                    $.each($.parseJSON(data), function (key, value)
                    {
                        $("#ddTerminal").append($("<option value='" + value.ID + "'>" + value.Name + "</option>"));
                    });
                }
            });
            $('#txtBilNo').keypress(function (e)
            {
                if (e.which == 13)
                    ShowBillList();
            });

            $("#ddTerminal").change(function ()
            {
                ShowBillList();
            }
            );
            ShowBillList();

            var datetext = "";
            if (FilterDateFrom == FilterDateTo)
                datetext = "Bill On " + FilterDateFrom
            else
                datetext = "Bill Between " + FilterDateFrom + " and " + FilterDateTo;
            $("#billhead").html(datetext);
        }
        );

        function ShowBillList()
        {
            var billNo = ($("#txtBilNo").val() == "" ? 0 : $("#txtBilNo").val());
            var terminalid = "";
            terminalid = ($("#ddTerminal").val() == null ? "" : $("#ddTerminal").val());
            GetBillingList($('#txtFrom').val(), $('#txtTo').val(), "", terminalid, billNo, futureorder, deliverytype,deliveryboy);
        }

        function ShowPaymentMode(ID, mode, cash, card)
        {
            OpenPopup();
            cashAmount = cash;
            cardAmount = card;
            $("#spanTotal").text(cash + card);
            $("#txtCash").val(cash);
            $("#txtCard").val(card);
            billingID = ID;
            $("#divbillDetail").html("");
            $("#divPaymentMode").css('display', '');
            $("#ddPaymentMode").val(mode);
            return false;
        }

        function UpdatePaymentMode()
        {
            if (billingID != null)
            {
                $.ajax({
                    url: "/Data.aspx?Action=UpdatePaymentMode&Data1=" + billingID + "&Data2=" + $("#txtCash").val() + "&Data3=" + $("#txtCard").val(), cache: false, success: function (data)
                    {
                        if (data == "ok")
                        {
                            var cashlink = "<a href='#' onclick='ShowPaymentMode(" + billingID + ",0," + $("#txtCash").val() + "," + $("#txtCard").val() + ")'>" + $("#txtCash").val() + "</a>";
                            var cardlink = "<a href='#' onclick='ShowPaymentMode(" + billingID + ",1," + $("#txtCash").val() + "," + $("#txtCard").val() + ")'>" + $("#txtCard").val() + "</a>";
                            $("#tr" + billingID + " td:nth-child(6)").html(cashlink);
                            $("#tr" + billingID + " td:nth-child(7)").html(cardlink);
                            alert("Done");
                            ClosePopup();
                        }
                        else
                        {
                            alert(data);
                        }
                    }
                });
            }
        }

        function UpdateBill(billItemID)
        {
            $.ajax({
                type: "POST",
                data: {
                    ID: billItemID,
                    Rate: $("#txtMRP" + billItemID).val(),
                    Quantity: $("#txtQuantity" + billItemID).val(),
                    Cost: $("#txtCost" + billItemID).val()
                },
                url: "/Data.aspx?Action=UpdateBill&Data1=" + billItemID + "&Data2=", cache: false,
                success: function (obj)
                {
                    $("#tdAmount" + billItemID).html(obj.Amount);
                    alert("Done");
                }
            });
        }

        function UpdateDeliveryBoy(billingID, obj)
        {
            var deliveryBy = $(obj).find("option:selected").val();
            $.ajax({
                url: "/Data.aspx?Action=UpdateDeliveryBoy&Data1=" + billingID + "&Data2=" + deliveryBy, cache: false, success: function (data)
                {
                    $(obj).val(data);
                    alert("Saved!");
                    $("#spanDeliveryBy" + billingID).text("DeliveryBy:" + data).css('background-color', '#FDFDA6 ');
                }
            });
        }

        function UpdateDeliveryPayment(billingID, obj)
        {
            var paymrecieved = $(obj).is(":checked") ? 1 : 0;
            $.ajax({
                url: "/Data.aspx?Action=UpdateSingleField&Data1=Billing&Data2=" + billingID + "&Data3=PaymentRecieved&Data4=" + paymrecieved, cache: false, success: function (data)
                {
                    if (paymrecieved == 1)
                        $("#spanDeliveryBy" + billingID).text("DeliveryBy:" + data).css('background-color', '#D4F3D4');
                }
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server" ViewStateMode="Disabled">
    <div class='panel panel-info'>
        <div class="panel-heading">
            <table class='table-condensed' style="width: 50%">
                <asp:HiddenField ID="hdStoreID" runat="server" />
                <asp:HiddenField ID="hdBillNo" runat="server" />
                <tr>
                    <td style="width: 50px;">Terminal</td>
                    <td style="width: 120px;">
                        <asp:DropDownList ID="ddTerminal" runat="server"></asp:DropDownList></td>
                    <td>
                        <asp:Label ID="ltDateFilter" runat="server" Text=""></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddDilivery" runat="server" onchange="ChangeURL(this,'deliverytype')">
                            <asp:ListItem Value="0">All</asp:ListItem>
                            <asp:ListItem Value="1">Delivery</asp:ListItem>
                            <asp:ListItem Value="2">Non Delivery</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddDeliveryBoy" runat="server" onchange="ChangeURL(this,'deliveryboy')">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:TextBox ID="txtBilNo" runat="server" placeholder="Search By bill No"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div id="billhead" style="font-weight:bold;"></div>
    <div id="divbillList"></div>
    <div id="divDialog" class="dialog">
        <span onclick="ClosePopup()" style="cursor: pointer;" class="glyphicon glyphicon-remove" aria-hidden="true"></span>
        <div id="divDialogContent">
            <div id="divbillDetail">
            </div>
            <div id="divPaymentMode" style="display: none;">
                <table>
                    <tr>
                        <td>Cash</td>
                        <td>Card</td>
                        <td>Total</td>
                    </tr>
                    <tr>
                        <td>
                            <input id="txtCash" class="updatetext" type="text" runat="server" /></td>
                        <td>
                            <input id="txtCard" class="updatetext" type="text" runat="server" /></td>
                        <td>
                            <span id="spanTotal">0</span>
                        </td>
                        <td>
                            <a href="#" class="btn btn-sm btn-success" id="btnUpdatePayment" onclick='UpdatePaymentMode()'>Update</a>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    <div id="divDialogfade" class="black_overlay"></div>
</asp:Content>

