/// <reference path="date.js" />
var newOrderList = {};
function MeatMart()
{
    this.FavoriteList = new Array();
    this.CartList = new Array();
    this.OrderList = new Array();
    //this.OrderID = 0;
    //this.CustomerName = "";
    //this.CustomerMobile = "";
    //this.CustomerAddress = "";
    //this.OrderStatus = "";
}
var M = new MeatMart();
$(document).ready(function ()
{
    $("#top-nav ul li a").click(function ()
    {
        $("#top-nav ul li").removeClass('active');
        $(this).parent('li').addClass('active');
    });

    SaveInLocalStorage("Mobile", "");
    var q = new QueryString();
    q.read();
    var mobile = q.getQueryString("mobile");
    if (typeof mobile != 'undefined')
        SaveInLocalStorage("Mobile", mobile);

    M.Load();
    OpenCart(false);

    HeighlightFav();
    HeighlightcartItem();
    $('#nav_search').keydown(function (e)
    {
        if (e.keyCode == 13)
        {
            if ($(this).val().length > 2)
                SerachItem();
        }
    });
    $('#txtSociety').hide();

    $("#divCart").on("keypress", ".onlydigit", isNumberKey);

    //fix footter at bottom
    var docHeight = $(window).height();
    var footerHeight = $('.footer').height();
    var footerTop = $('.footer').position().top + footerHeight;
    if (footerTop < docHeight)
    {
        $('.footer').css('margin-top', 10 + (docHeight - footerTop) + 'px');
    }
});

function SerachItem()
{
    //location.href = "/serach/" + $("#nav_search").val();
    $.ajax({
        url: "/Data.aspx?Action=GetItems&Data1=search&Data2=" + $("#nav_search").val(), cache: false, success: function (data)
        {
            $("#divItem").html(data);
        }
    });
}

function ShowFavorite()
{
    var BredCrumbitems = "<li><a href='/'>Home</a></li><li>Favourite</li>";
    $(".breadcrumb").html(BredCrumbitems);

    var items = "";
    $(M.FavoriteList).each(function (k, v)
    {
        items += "<div class='col-sm-3 itemdiv favItems' id='" + v.ID + "'>"
                + "<table>"
                + "<tr><td><a href='#'><img style='width:80%;' src='/data.aspx?Action=Image&Data1=" + v.ID + "'></a></td></tr>"
                + "<tr><td><a class='item-name' href='#'>" + v.Name + "</a></td></tr>"
                + "<tr><td><div>" + v.MRP + "&nbsp;<span>/pc</span></div></td></tr>"
                + "<tr style='height:10px'><td>"
                + "<table><tr><td>Qty</td><td><input style='width:60px;' type='number' min='1' max='50' name='quantity' class='form-control' value='1' id='txtQuantity" + v.ID + "'></td>"
                + "<td><a id='addCart" + v.ID + "' class='btn btn-primary btnAddToCart' onclick='AddToCart(" + v.ID + ",\"" + v.Name + "\"," + v.MRP + ")'>Add</a></td>"
                + "<td><a htref='#' id='like" + v.ID + "' class='glyphicon glyphicon-heart gray like' style='font-size:30px;text-decoration:none;' onclick='AddFavouriteItems(" + v.ID + ",\"" + v.Name + "\"," + v.MRP + ")'></a></td></tr></table>"
                + "</td></tr>"
                + "</table>"
                + "</div>"
    });
    $("#divItem").html(items);
    HeighlightFav();
}

function HeighlightFav()
{
    $('a.like').each(function (i, li)
    {
        $("#" + $(this).prop("id")).removeClass("red");
        if (FoundInFavorite($(this).prop("id").replace("like", "")))
        {
            $("#" + $(this).prop("id")).addClass("red");
        }
    });
}

function HeighlightcartItem()
{
    $('a.btnAddToCart').each(function ()
    {
        var itemID = $(this).prop("id").replace("addCart", "");

        $("#" + $(this).prop("id")).text("Add");
        $("#" + $(this).prop("id")).removeClass("btn-primary").addClass("btn-success");
        $("#" + itemID).removeClass("item-selected");
        if (FoundInCart(itemID))
        {
            $("#" + $(this).prop("id")).removeClass("btn-success").addClass("btn-primary");
            $("#" + $(this).prop("id")).text("Remove");
            $("#" + itemID).addClass("item-selected");
        }
    });
}

var LastDiv = "";
function ShowTopData(divToShow)
{
    $("#divCategoryList").hide();
    $("#divBrandList").hide();
    $("#divCartList").hide();

    if (LastDiv == divToShow)
    {
        LastDiv = "";
        return;
    }
    $("#" + divToShow).show();
    LastDiv = divToShow;

    if (divToShow == "divCartList")
        OpenCart();
    if (divToShow == "divBrandList")
        $(".breadcrumb").html("<li><a href='/'>Home</a></li><li>Brand</li>");
    if (divToShow == "divCategoryList")
        $(".breadcrumb").html("<li><a href='/'>Home</a></li><li>Category</li>");
}

function GetOrderStatus(Mobile)
{
    if (!Mobile)
    {
        alert("Please enter mobile no");
        $("#txtMobileTrck").focus();
        return;
    }
    history.pushState('', '', "/?mobile=" + Mobile);
    ShowOrderStatus(0, Mobile);
}

function ShowOrderStatus(OrderID, Mobile)
{
    //if (typeof (OrderID) == 'undefined' && typeof (Mobile) == 'undefined')
    //{
    //    $("#divCartList").show();
    //    OpenCart(true);
    //    return;
    //}
    $("#spanCartDetail").html(M.CartList.length);
    $("#divBrandList").hide();
    $("#divCategoryList").hide();
    $.ajax({
        url: "/Data.aspx?Action=TraceOrder&Data1=" + OrderID + "&Data2=" + Mobile, cache: false, success: function (data)
        {
            console.log(data);
            var obj = JSON.parse(data);
            newOrderList = {
                OrderItemList: obj.OrderItemList,
                OrderID: obj.ID,
                CustomerName: obj.Name,
                CustomerMobile: obj.Mobile,
                CustomerAddress: obj.Address,
                OrderStatus: obj.OrderStatus,
                ProjectName: obj.ProjectName,
                ApartmentNo: obj.ApartmentNo,
                DeliveryDate: obj.DeliveryDateTimeString,
                OrderMessage: obj.Message
            };
            //if (newOrderList.OrderMessage != "")
            //{
            //    alert(newOrderList.OrderMessage);
            //    return;
            //}
            $("#divCartList").show();
            OpenCart(true);
        }
    });
}

function AddToCart(ItemID, Name, Price)
{
    var Quantity = $("#txtQuantity" + ItemID).val();
    if (!FoundInCart(ItemID))
    {
        M.CartList.push({ ItemID: ItemID, ItemName: Name, Price: Price, Quantity: Quantity });
    }
    else
    {
        $(M.CartList).each(function (index)
        {
            if (this.ItemID == ItemID)
                FoundIndex = index;
        })
        M.CartList.splice(FoundIndex, 1);
    }
    M.Save();
    HeighlightcartItem();

    OpenCart(false);
}

function getThirtyDates()
{
    var date = new Date();
    var days = [];
    for (var i = 0; i < 30; i++)
    {
        days.push(new Date(date));
        date.addDays(1);
    }
    return days;
}

function ChangeItemQuantity(index, txtQuantityObj)
{
    M.CartList[index].Quantity = $(txtQuantityObj).val() == "" ? "0" : $(txtQuantityObj).val();
    M.Save();
    OpenCart();
}

function OpenCart(forOrder)
{
    var BredCrumbitems = "";
    var BredCrumbitems = "<li><a href='/'>Home</a></li><li>Cart</li>";
    if (forOrder)
        BredCrumbitems = "<li><a href='/'>Home</a></li><li>Current Order</li>";
    $(".breadcrumb").html(BredCrumbitems);

    $.ajax({
        url: "/Data.aspx?Action=GetSocietyList", cache: false, success: function (data)
        {
            var str = "<option value=''>-Select-";
            $.each($.parseJSON(data), function (key, value)
            {
                str += "<option value='" + value.ID + "'>" + value.ProjectName;
            });
            $("#ddSociety").html(str + "<option value='other'>Other");
        }
    });

    var List = forOrder ? newOrderList.OrderItemList : M.CartList;
    var itemList = "<div class='panel panel-info'><div class='panel-heading'>" + (forOrder ? "Order Details" : "Cart") + "</div><div class='panel-body'>";

    if (List && List.length > 0)
    {
        var total = 0, ctr = 0;
        itemList += "<div class='col-md-6'><table class='table table-striped table-bordered'><tr><th>#<th><th>Item";
        var qty = 0;

        $(List).each(function (index)
        {
            ctr++;
            itemList += "<tr><td>" + ctr + "<td><img src='/data.aspx?Action=Image&Data1=" + this.ItemID + "' style='height:50px' /><td>" + this.ItemName
                + "<br/>" + (forOrder ? this.Quantity : "<input type='number' min='1' value=" + this.Quantity + " onchange='ChangeItemQuantity(" + index + ",this)' style='width:40px;'></input>") + " X " + this.Price
                + (forOrder ? "" : "<br/><a style='font-family: sans-serif;' href='#' onclick='return AddToCart(" + this.ItemID + ")'><span class='glyphicon glyphicon-remove-circle'></span>Remove</a>");
            total += (this.Price * this.Quantity);
            qty += parseFloat(this.Quantity);
        });

        var days = getThirtyDates();
        var dayOptions = "";
        $(days).each(function (i, v)
        {
            if (i > 2)
                return;
            dayOptions += "<option value='" + v.toString("dd-MMM-yyyy") + "'>" + v.toString("dd-MMM-yy") + "</option>";
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

        itemList += "<tr><td colspan='3'><strong>Total Items " + qty + "&nbsp;&nbsp;&nbsp;TOTAL Rs " + total + "</strong></table></div>"
        + "<div class='col-md-6'><table class='table table-striped table-bordered'>"
        + "<tr><td>Mobile<td>" + (forOrder ? newOrderList.CustomerMobile : "<input type='text' id='txtMobile' maxlength='10' class='form-control onlydigit' placeholder='Order confirmation mobile'></input>")
        + "<tr><td>Name<td>" + (forOrder ? newOrderList.CustomerName : "<input type='text' id='txtCustomerName' maxlength='100' class='form-control' placeholder='Customer Name'></input>")
        + "<tr><td>Society<td>" + (forOrder ? newOrderList.ProjectName : "<select  id='ddSociety' class='form-control' onchange='ShowAddress(this)' ></select>")
        + "<tr id='trApt'><td>Apartment<td>" + (forOrder ? newOrderList.ApartmentNo : "<input type='text' id='txtApartment' class='form-control'></input>")
        + "<tr id='trAddress' style='" + (forOrder ? "" : "display:none") + "'><td>Address<td>" + (forOrder ? newOrderList.CustomerAddress : "<textarea rows='4' cols='50' type='text' id='txtAddress' maxlength='100' class='form-control' placeholder='Delivery address'/>")
        + "<tr><td>Delivery Time<td>" + (forOrder ? newOrderList.DeliveryDate : "<select  id='ddDay' class='drp'>" + dayOptions + "</select><select  id='ddHH' class='drp'>" + hourOptions + "</select>")
        + (forOrder ? "" : "<tr><td><td style='padding:5px'><a href='#' class='btn btn-success pull-right' onclick='SaveOrder()'>Submit Order</a>")
        + (forOrder ? "<tr><td>Status<td>" + newOrderList.OrderStatus : "")
        + "</table></div>";

        var mobile = LoadFromLocalStorage('Mobile');  //Get data by mobile and autofill customer data;
        if (mobile != "")
        {
            $.ajax({
                url: "/Data.aspx?Action=GetCustomerByMobile&Data1=" + mobile, cache: false, success: function (data)
                {
                    if (data != "")
                    {
                        var cust = JSON.parse(data);
                        $("#txtCustomerName").val(cust.Name);
                        $("#txtMobile").val(cust.Mobile);
                        $("#ddSociety").val(cust.ProjectID);
                        $("#txtApartment").val(cust.ApartmentNo);
                        $("#txtAddress").val(cust.Address);
                    }
                }
            });
        }
    }
    else
    {
        itemList += "<h2>" + (forOrder ? "Trace your order!" : "No items found in the cart") + "</h2>";
        if (forOrder)
        {
            itemList += "<table><tr><td>Mobile&nbsp;<td><input type='text' id='txtMobileTrck' maxlength='10' class='form-control onlydigit' placeholder='Enter mobile to get order details'></input>"
                + "<td><a href='#' onclick='GetOrderStatus($(\"#txtMobileTrck\").val())' class='btn btn-success'>Get My Order</a></table>";
        }
    }
    itemList += "</div></div>";
    $("#divCart").html(itemList);

    $("#spanCartDetail").html(M.CartList.length);
}

function ShowAddress(obj)
{
    var disp = $(obj).val() == "other" ? "" : "none";
    var disp2 = $(obj).val() == "other" ? "none" : "";
    $('#trApt').css("display", disp2);
    $('#trAddress').css("display", disp);
}

function SaveOrder()
{
    if ($("#txtMobile").val().length < 10)
    {
        alert("Please enter 10 digit mobile No.");
        $("#txtMobile").focus();
        return;
    }

    if ($("#txtCustomerName").val() == "")
    {
        alert("Please enter name");
        $("#txtCustomerName").focus();
        return;
    }

    if ($("#ddSociety").val() == "other")
    {
        if ($("#txtAddress").val() == "")
        {
            alert("Please enter Address.");
            $("#txtAddress").focus();
            return;
        }
    }

    console.log(M.CartList);
    var SocietyInfo = ($("#ddSociety").val() == "other" ? "" : $("#ddSociety").val() + "^" + $("#txtApartment").val());
    var deliverydateTime = $("#ddDay").val() + " " + $("#ddHH").val();
    $.post("/Data.aspx?Action=SaveOrder&Data1=0&Data2=" + $("#txtMobile").val() + "&Data3=" + $("#txtAddress").val() + "&Data4=" + $("#txtCustomerName").val() + "&Data5=" + SocietyInfo + "&Data6=" + deliverydateTime,
        {
            data: JSON.stringify(M.CartList),
        },
        function (data)
        {
            if (data != "error")
            {
                if (!FoundInOrdered(data))
                {
                    //M.OrderID = data;
                    M.CartList.length = 0;
                    M.Save();

                    alert("Your order has been placed and is pending for confirmation, soon you will receive a call on provided mobile for confirmation.");
                    HeighlightcartItem();
                    ShowOrderStatus(data, 0);
                }
            }
            else
                alert(data);
        });
}

function AddFavouriteItems(ID, Name, MRP)
{
    var Found = FoundInFavorite(ID);
    if (!Found)
        M.FavoriteList.push({ ID: ID, Name: Name, MRP: MRP });
    else
    {
        $(M.FavoriteList).each(function (index)
        {
            if (this.ID == ID)
                FoundIndex = index;
        })
        M.FavoriteList.splice(FoundIndex, 1);
    }
    M.Save();
    HeighlightFav();
}

MeatMart.prototype.Save = function ()     //save all Items in local storage
{
    for (var propertyName in this)
    {
        if (this[propertyName] != null)
        {
            if ((typeof (this[propertyName])).toString() != "function")
            {
                SaveInLocalStorage(propertyName, JSON.stringify(this[propertyName]));
            }
        }
    }
}

MeatMart.prototype.Load = function ()    //load all items from local storage 
{
    try
    {
        for (var propertyName in this)
        {
            var data = LoadFromLocalStorage(propertyName);
            if (data != "undefined" && data != "null" && data != null && data != undefined)
            {
                if ((typeof (this[propertyName])).toString() != "function")
                    this[propertyName] = JSON.parse(data);
            }
        }
    }
    catch (e)
    {
        ClearLocalStoreage();
    }
}

function FoundInFavorite(ID)       // check if the selected Item exists in the favorite list
{
    var Found = false;
    $(M.FavoriteList).each(function ()
    {
        if (this.ID == ID)
            Found = true;
    });
    return Found;
}

function FoundInCart(ItemID)       // check if the selected Item exists in the cart list
{
    var Found = false;
    $(M.CartList).each(function ()
    {
        if (this.ItemID == ItemID)
            Found = true;
    });
    return Found;
}

function FoundInOrdered(ID)       // check if the selected Item exists in the cart list
{
    var Found = false;
    $(M.OrderList).each(function ()
    {
        if (this.ID == ID)
            Found = true;
    });
    return Found;
}

//function SearchItems()
//{
//    $("#typeahead").hide();
//    $.ajax({
//        url: "/Data.aspx?Action=GetItemJSON&query=" + $("#nav_search").val(), cache: false, success: function (data)
//        {
//            var searchResult = "";
//            $(data).each(function ()
//            {
//                searchResult += "<li id=" + this.id + "><a href='#' onclick='ShowItem(" + this.id + ")'>" + this.name + "</a></li>";
//            });
//            if (searchResult != "")
//            {
//                $("#typeahead").show();
//                $("#typeahead").html(searchResult);
//            }
//        }
//    });
//}

function ShowItem(obj)
{
    $("#typeahead").hide();
    $("#nav_search").val(obj);
    $("#typeahead").html("");
}