<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Order.aspx.cs" Inherits="Order" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="/js/bootstrap-typeahead.min.js"></script>
    <script src="js/date.js"></script>
    <script type="text/javascript">
        var bill;
        $(document).ready(function ()
        {
            $("#txtItemSearch").hide();
            $('.typeahead').typeahead(
            {
                onSelect: function (item)
                {
                    $("#txtItemID").val(item.value);
                },
                ajax: {
                    url: "/Data.aspx?Action=GetItemJSON",
                    timeout: 500,
                    triggerLength: 1,
                    method: "get",
                    loadingClass: "loading-circle",
                    preDispatch: function (query)
                    {
                        return {
                            query: query
                        }
                    },
                    preProcess: function (data)
                    {
                        return data;
                    }
                }
            });

            var days = getThirtyDates();
            var dayOptions = "";
            $(days).each(function (i, v)
            {
                if (i > 6)
                    return;
                dayOptions += "<option value='" + v.toString("d-MMM-yyyy") + "'>" + v.toString("d-MMM-yy") + "</option>";

            });
            var hourOptions = "", dt = new Date(2001, 1, 1, 10), dt2 = new Date(2001, 1, 1, 22), currentTime = new Date(), SelectedCurrent = false;
            while (dt <= dt2)
            {
                hourOptions += "<option value='" + dt.toString("HH:mm") + "'";
                if (dt.getHours() > currentTime.getHours() && SelectedCurrent == false)
                {
                    SelectedCurrent = true;
                    hourOptions += " selected='selected'";
                }

                hourOptions += ">" + dt.toString("hh:mm tt") + "</option>";
                dt.addMinutes(15);
            }
            $("#ddDay").html(dayOptions);
            $("#ddHH").html(hourOptions);
        });

        function GetOrderItem(orderID)
        {
            OpenPopup();
            $("#OrderItemList").html("<img src='/images/progress2.gif'>");
            TraceOrder(orderID);
        }

        function TraceOrder(orderID)
        {
            $.ajax({
                url: "/Data.aspx?Action=TraceOrder&Data1=" + orderID, cache: false, success: function (data)
                {
                    var itemList = "<table class='table table-condensed table-striped table-bordered'><tr><th>Item<th style='width:20px;'>Qty.<th style='width:30px;'>Price"
                    var totalQuantity = 0, totalAmount = 0;
                    bill = $.parseJSON(data);
                    console.log(bill);
                    $.each($.parseJSON(data).ItemList, function (key, value)
                    {
                        totalQuantity += value.Quantity;
                        totalAmount += (value.Quantity * value.MRP);
                        itemList += "<tr><td><a href='#' onclick='ShowOrderItemData(" + JSON.stringify(value) + ")'>" + value.Name + "</a><td>" + value.Quantity + "<td>" + value.MRP;
                    });
                    itemList += "<tr><th>TOTAL<th>" + totalQuantity + "<th>" + totalAmount;
                    $("#OrderItemList").html(itemList);

                    $("#txtOrderID").val(bill.ID);
                    $("#dropStore").val(bill.store.ID);

                    var customer = (bill.customer);
                    $("#txtMobile").val(customer.Mobile);
                    $("#txtCustomerName").val(customer.Name);
                    $("#txtAddress").val(customer.Address);
                    $("#ddSociety").val(customer.ProjectID == 0 ? "" : customer.ProjectID);
                    $("#txtApartment").val(customer.ApartmentID);

                    $("#ddDay").val(bill.DeliveryTimeString.split(" ")[0]);
                    $("#ddHH").val(bill.DeliveryTimeString.split(" ")[1]);
                    FillDelieveryManDropdown(bill.DeliveryBy);
                }
            });
            //ResetEdit();
        }

        function AssignOrder(obj)
        {
            if ($(obj).val() == "0")
            {
                alert("Please select store");
                return;
            }
            $.ajax({
                url: "/Data.aspx?Action=OrderAssign&Data1=" + $("#txtOrderID").val() + "&Data2=" + $(obj).val(), cache: false, success: function (data)
                {
                    if (data != "")
                        alert(data);
                    FillDelieveryManDropdown("");
                }
            });
        }

        function FillDelieveryManDropdown(deliveryPerson)
        {

            $.ajax({
                url: "/Data.aspx?Action=GetDeliveryManList&Data1=" + $("#dropStore").val(), cache: false, async: false, success: function (data)
                {
                    var obj = JSON.parse(data);
                    $("#ddDelieveryMan").html("");
                    $("#ddDelieveryMan").append("<option value=''>--Select--</option>");
                    $.each(obj, function (key, val)
                    {
                        $("#ddDelieveryMan").append("<option value='" + this.Name + "'>" + this.Name + "</option>");
                    });
                    $("#ddDelieveryMan").val(deliveryPerson);
                }
            });

        }

        function AssignDelieveryMan()
        {
            $.ajax({
                url: "/Data.aspx?Action=AssignDelieveryMan&Data1=" + $("#txtOrderID").val() + "&Data2=" + $("#ddDelieveryMan").val()
                    + "&Data3=" + $("#ddDelieveryMan option:selected").text(), cache: false, success: function (data)
                    {
                        location.reload();
                    }
            });
        }
        function ShowStatusDialog(orderID, currentState)
        {
            $("#txtOrderID").val(orderID);
            $("#ddStatus").val(currentState);
            OpenPopup();
        }
        //function ChangeStatus(obj,OrderID)
        //{
        //    $.ajax({
        //        url: "/Data.aspx?Action=ChangeOrderStatus&Data1=" + OrderID + "&Data2=" + $(obj).val(), cache: false, success: function (data)
        //        {
        //            location.reload();
        //            $("#spanOrderStatus").text(data);
        //        }
        //    });
        //}
        function changeOrderStatus(OrderID, nextPrev)
        {
            $.ajax({
                url: "/Data.aspx?Action=ChangeOrderStatus&Data1=" + OrderID + "&Data2=" + $("#spanOrderStatusID").text() + "&Data3=" + nextPrev, cache: false, success: function (data)
                {
                    var statusStrig = data.split('-')[0];
                    var statusid = data.split('-')[1];
                    $("#spanOrderStatusID").text(statusid)
                    $("#spanOrderStatus").text(statusStrig);
                }
            });
        }

        function ShowOrderItemData(objOrderItem)
        {
            $("#txtOrderItemID").val(objOrderItem.ID);
            $("#txtItemID").val(objOrderItem.ItemID);
            $("#txtItemName").val(objOrderItem.Name);
            $("#txtQuantty").val(objOrderItem.Quantity);
        }

        function ResetEdit()
        {
            $("#ItemEditForm").find("input[type = text]").val("");
            $("#txtOrderItemID").val(0);
        }

        function SaveOrderItem()
        {
            if ($("#txtOrderItemID").val() == "0")
            {
                console.log($("#txtItemID").val());
                if ($("#txtItemID").val() == "0")
                {
                    alert("Please select an item.");
                    $("#txtItemName").focus();
                    return;
                }
            }
            var objPost = {
                orderID: $("#txtOrderID").val(),
                mobile: $("#txtMobile").val(),
                address: $("#txtAddress").val(),
                customerName: $("#txtCustomerName").val(),
                society: $("#ddSociety").val(),
                apartment: $("#txtApartment").val(),
                deliverydateTime: $("#ddDay").val() + " " + $("#ddHH").val(),
                orderItemID: $("#txtOrderItemID").val(),
                itemID: $("#txtItemID").val(),
                quantity: $("#txtQuantty").val()
            };

            $.ajax({
                type: "POST",
                url: "/Data.aspx?Action=SaveOrder&Data7=1", //Data7=1 means request is from edit.
                data: objPost,
                success: function (data)
                {
                    TraceOrder($("#txtOrderID").val());
                    ResetEdit();
                }
            })
        }
    </script>
    <style type="text/css">
        .noborder td
        {
            background-color: #d9edf7;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server" ViewStateMode="Disabled">
    <div class='panel panel-info'>
        <div class="panel-heading">
            <table>
                <tr>
                    <td style="width: 70%;"></td>
                    <td>
                        <asp:Literal ID="ltDateFilter" runat="server"></asp:Literal></td>
                </tr>
            </table>
        </div>
    </div>
    <div id="OrderList" runat="server"></div>
    <div id="divDialog" class="dialog">
        <span onclick="ClosePopup()" style="cursor: pointer;" class="pull-right glyphicon glyphicon-remove" aria-hidden="true"></span>
        <%--<div id="divStateChange">
            <asp:DropDownList ID="ddStatus" runat="server" onchange="ChangeStatus(this)"></asp:DropDownList>
        </div>--%>
        <div id="divOrerAndItem">
            <div id="OrderItemList"></div>
            <table>
                <tr id="tdAssign">
                    <td>
                        <input id="txtOrderID" value="0" type="text" style="display: none;" />
                        <strong>Assign to</strong>
                        <asp:DropDownList ID="dropStore" runat="server" onchange="AssignOrder(this)"></asp:DropDownList>
                        <asp:DropDownList ID="ddDelieveryMan" runat="server" onchange="AssignDelieveryMan()"></asp:DropDownList>
                    </td>
                </tr>
            </table>
            <br />
            <table id="OrderEditForm" class="noborder" style="width: 100%;">
                <tr style="margin-top: 5px;">
                    <td style="width: 50px;"><strong>Mobile </strong></td>
                    <td>
                        <asp:TextBox ID="txtMobile" runat="server"></asp:TextBox></td>
                    <td><strong>Name </strong></td>
                    <td>
                        <asp:TextBox ID="txtCustomerName" runat="server"></asp:TextBox></td>
                    <td><strong>Address</strong></td>
                    <td>
                        <asp:TextBox ID="txtAddress" runat="server" Style="width: 80%;"></asp:TextBox></td>
                </tr>
                <tr>
                    <td><strong>Society</strong></td>
                    <td>
                        <asp:DropDownList ID="ddSociety" runat="server" Style="width: 200px;"></asp:DropDownList></td>
                    <td><strong>Apartment</strong></td>
                    <td>
                        <asp:TextBox ID="txtApartment" runat="server" Style="width: 50px;"></asp:TextBox></td>
                    <td><strong>Delivery Time</strong></td>
                    <td>
                        <select id='ddDay' class='drp'></select><select id='ddHH' class='drp'></select></td>
                </tr>
            </table>
            <table style="width: 100%;" id="ItemEditForm" class="noborder">
                <tr>
                    <td style="display: none;">ID:<input type="text" id="txtOrderItemID" value="0" style="width: 30px;" /><input type="text" id="txtItemID" value="0" style="width: 40px;" /></td>
                    <td style="width: 56px;">
                        <strong>Item </strong></td>
                    <td>
                        <input type="text" id="txtItemName" class="typeahead" style="width: 50%;" />

                        <strong>Quantity </strong>
                        <input id="txtQuantty" type='number' min="1" max="50" style="width: 40px;" />
                        <a href='#' class="btn btn-sm btn-success" onclick="SaveOrderItem()">Save</a>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div id="divDialogfade" class="black_overlay"></div>
</asp:Content>

