var CurrentNav = "";
$(document).ready(function ()
{
    if (CurrentNav!="")
    $("li." + CurrentNav).addClass("active");

    $('.datepicker').datepicker({
        startView: 0,
        orientation: "auto",
        format: 'dd-M-yyyy',
        autoclose: true
    });

    $('.datepickerDisableFutureDates').datepicker({
        startView: 0,
        orientation: "auto",
        format: 'dd-M-yyyy',
        endDate: '+0d',
        autoclose: true
    });

    $("input").click(function ()
    {
        $(this).select();
    });
});

function ShowPopup(id)
{
    var o = $(id);
    $(o).css({
        position: "absolute",
        top: ($(window).height() - $(o).height()) * .5 + $(document).scrollTop(),
        left: ($(window).width() - $(o).width()) * .5
    });
    $(o).show();
}

function ChangeURL(obj, name)
{
    location.href = "?FilterDateFrom=" + FilterDateFrom
        + "&FilterDateTo=" + FilterDateTo
        + "&ReportType=" + ReportType
        + (vendorid != 0 && name != 'vendorid' ? "&vendorid=" + vendorid : "")
        + (tax != 0 && name != 'tax' ? "&tax=" + tax : "")
        + (brandid != 0 && name != 'brandid' ? "&brandid=" + brandid : "")
        + "&" + name + "=" + $(obj).val();
}

function GetBillingList(fromDate, toDate, mobile, terminalID, billno, futureorder, deliverytype, deliveryboy)
{
    $("#divbillList").html("<img src='/images/progress2.gif'>");
    $.ajax({
        url: "/Data.aspx?Action=GetBillingList&Data1=" + fromDate + "&data2=" + toDate + "&Data3=" + mobile + "&Data4=" + terminalID
            + "&Data5=" + billno + "&Data6=" + futureorder + "&Data7=" + deliverytype + "&Data8=" + deliveryboy, cache: false, success: function (data)
        {
            $("#divbillList").html(data);
        }
    });
}

function ItemSearchJSON(obj)
{
    $("#divItemSearch").css('display', 'none');
    $("#divItemSearch").html("");
    $.ajax({
        url: "/Data.aspx?Action=GetItemJSON&query=" + $(obj).val(), success: function (data)
        {
            if (data.length > 0)
            {
                var str = "<table>";
                $(data).each(function (i, v)
                {
                    if (typeof RecordType === 'undefined')
                        RecordType = ReportType
                    str += "<tr><td>" + v.plu + "<td><a href='?vendorid=" + (typeof ($("#ddVendor").val()) === 'undefined' ? "" : $("#ddVendor").val()) + "&reporttype=" + RecordType + "&filterdatefrom=" + FilterDateFrom + "&filterdateto=" + FilterDateTo + "&itemid=" + v.id + "'>" + v.name + "</a>";
                });
                $("#divItemSearch").show();
                $("#divItemSearch").html(str + "</table>");
            }
        }
    });
}

function GetBillDetail(billID)
{
    $("#divPaymentMode").css('display', 'none');
    OpenPopup();
    $("#divbillDetail").html("<img src='/images/progress2.gif'>");
    $.ajax({
        url: "/Data.aspx?Action=GetBillDetail&Data1=" + billID, cache: false, success: function (data)
        {
            $("#divbillDetail").html(data);
        }
    });
    return false;
}

function GetCustomerList(mobile)
{
    $("#divCustomerList").html("<img src='/images/progress2.gif'>");
    $.ajax({
        url: "/Data.aspx?Action=GetCustomerList&Data1=" + mobile, cache: false, success: function (data)
        {
            $("#divCustomerList").html(data);
        }
    });
}

//popup
document.onkeydown = function (evt)
{
    evt = evt || window.event;
    if (evt.keyCode == 27)
    {
        ClosePopup();
        ClosePopup2();
    }
};

function OpenPopup()
{
    $('#divDialog').css("top", $(document).scrollTop() + ($(window).height() - $('#divDialog').height()) * .5);
    $('#divDialog').css("left", ($(window).width() - $('#divDialog').width()) * .5);
    $('#divDialogfade').css("top", $(document).scrollTop());
    $('#divDialog').show();
    $('#divDialogfade').show();
}
function ClosePopup()
{
    document.getElementById('divDialog').style.display = 'none';
    document.getElementById('divDialogfade').style.display = 'none';
}
function OpenPopup2()
{
    $('#divDialog2').css("top", $(document).scrollTop() + ($(window).height() - $('#divDialog2').height()) * .5);
    $('#divDialog2').css("left", ($(window).width() - $('#divDialog2').width()) * .5);
    $('#divDialogfade').css("top", $(document).scrollTop());
    $('#divDialog2').show();
    $('#divDialogfade').show();
}
function ClosePopup2()
{
    document.getElementById('divDialog2').style.display = 'none';
    document.getElementById('divDialogfade').style.display = 'none';
}

function OpenPopup3()
{
    $('#divDialog3').css("top", $(document).scrollTop() + ($(window).height() - $('#divDialog3').height()) * .5);
    $('#divDialogfade').css("top", $(document).scrollTop());

    $('#divDialog3').show();
    $('#divDialogfade').show();
}
function ClosePopup3()
{
    document.getElementById('divDialog3').style.display = 'none';
    document.getElementById('divDialogfade').style.display = 'none';
}


/*
    * Date Format 1.2.3
    * (c) 2007-2009 Steven Levithan <stevenlevithan.com>
    * MIT license
    *
    * Includes enhancements by Scott Trenda <scott.trenda.net>
    * and Kris Kowal <cixar.com/~kris.kowal/>
    *
    * Accepts a date, a mask, or a date and a mask.
    * Returns a formatted version of the given date.
    * The date defaults to the current date/time.
    * The mask defaults to dateFormat.masks.default.
    */

var dateFormat = function ()
{
    var token = /d{1,4}|m{1,4}|yy(?:yy)?|([HhMsTt])\1?|[LloSZ]|"[^"]*"|'[^']*'/g,
        timezone = /\b(?:[PMCEA][SDP]T|(?:Pacific|Mountain|Central|Eastern|Atlantic) (?:Standard|Daylight|Prevailing) Time|(?:GMT|UTC)(?:[-+]\d{4})?)\b/g,
        timezoneClip = /[^-+\dA-Z]/g,
        pad = function (val, len)
        {
            val = String(val);
            len = len || 2;
            while (val.length < len) val = "0" + val;
            return val;
        };

    // Regexes and supporting functions are cached through closure
    return function (date, mask, utc)
    {
        var dF = dateFormat;

        // You can't provide utc if you skip other args (use the "UTC:" mask prefix)
        if (arguments.length == 1 && Object.prototype.toString.call(date) == "[object String]" && !/\d/.test(date))
        {
            mask = date;
            date = undefined;
        }

        // Passing date through Date applies Date.parse, if necessary
        date = date ? new Date(date) : new Date;
        if (isNaN(date)) throw SyntaxError("invalid date");

        mask = String(dF.masks[mask] || mask || dF.masks["default"]);

        // Allow setting the utc argument via the mask
        if (mask.slice(0, 4) == "UTC:")
        {
            mask = mask.slice(4);
            utc = true;
        }

        var _ = utc ? "getUTC" : "get",
            d = date[_ + "Date"](),
            D = date[_ + "Day"](),
            m = date[_ + "Month"](),
            y = date[_ + "FullYear"](),
            H = date[_ + "Hours"](),
            M = date[_ + "Minutes"](),
            s = date[_ + "Seconds"](),
            L = date[_ + "Milliseconds"](),
            o = utc ? 0 : date.getTimezoneOffset(),
            flags = {
                d: d,
                dd: pad(d),
                ddd: dF.i18n.dayNames[D],
                dddd: dF.i18n.dayNames[D + 7],
                m: m + 1,
                mm: pad(m + 1),
                mmm: dF.i18n.monthNames[m],
                mmmm: dF.i18n.monthNames[m + 12],
                yy: String(y).slice(2),
                yyyy: y,
                h: H % 12 || 12,
                hh: pad(H % 12 || 12),
                H: H,
                HH: pad(H),
                M: M,
                MM: pad(M),
                s: s,
                ss: pad(s),
                l: pad(L, 3),
                L: pad(L > 99 ? Math.round(L / 10) : L),
                t: H < 12 ? "a" : "p",
                tt: H < 12 ? "am" : "pm",
                T: H < 12 ? "A" : "P",
                TT: H < 12 ? "AM" : "PM",
                Z: utc ? "UTC" : (String(date).match(timezone) || [""]).pop().replace(timezoneClip, ""),
                o: (o > 0 ? "-" : "+") + pad(Math.floor(Math.abs(o) / 60) * 100 + Math.abs(o) % 60, 4),
                S: ["th", "st", "nd", "rd"][d % 10 > 3 ? 0 : (d % 100 - d % 10 != 10) * d % 10]
            };

        return mask.replace(token, function ($0)
        {
            return $0 in flags ? flags[$0] : $0.slice(1, $0.length - 1);
        });
    };
}();

// Some common format strings
dateFormat.masks = {
    "default": "ddd mmm dd yyyy HH:MM:ss",
    shortDate: "m/d/yy",
    mediumDate: "mmm d, yyyy",
    longDate: "mmmm d, yyyy",
    fullDate: "dddd, mmmm d, yyyy",
    shortTime: "h:MM TT",
    mediumTime: "h:MM:ss TT",
    longTime: "h:MM:ss TT Z",
    isoDate: "yyyy-mm-dd",
    isoTime: "HH:MM:ss",
    isoDateTime: "yyyy-mm-dd'T'HH:MM:ss",
    isoUtcDateTime: "UTC:yyyy-mm-dd'T'HH:MM:ss'Z'"
};

// Internationalization strings
dateFormat.i18n = {
    dayNames: [
        "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat",
        "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"
    ],
    monthNames: [
        "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec",
        "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"
    ]
};

// For convenience...
Date.prototype.format = function (mask, utc)
{
    return dateFormat(this, mask, utc);
};



//---------Local Storage--------------------//
function SaveInLocalStorage(Key, val)
{
    if (typeof (localStorage) != 'undefined')
    {
        window.localStorage.removeItem(Key);
        window.localStorage.setItem(Key, val);
        return true;
    }
    else
    {
        alert("Your browser does not support local storage, please upgrade to latest browser");
        return false;
    }
}

function LoadFromLocalStorage(Key, DefaultValue)
{
    var valoutput;
    if (typeof (window.localStorage) != 'undefined')
    {
        valoutput = window.localStorage.getItem(Key);
    }
    else
    {
        throw "window.localStorage, not defined";
    }
    if (DefaultValue && !valoutput)
        return DefaultValue;
    else
        return valoutput;
}
function RemoveFromLocalStorage(Key)
{
    window.localStorage.removeItem(Key);
}
function ClearLocalStoreage()
{
    if (typeof (window.localStorage) != 'undefined')
    {
        window.localStorage.clear();
    }
    else
    {
        throw "window.localStorage, not defined";
    }
}
//

function getNextThirtyDates()
{
    var days = getThirtyDates();
    var dayOptions = "";
    $(days).each(function (i)
    {
        var m_names = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
        var date = days[i].getDate() + "-" + m_names[days[i].getMonth()] + "-" + days[i].getFullYear();
        dayOptions += "<option value='" + date + "'>" + date + "</option>";
    });
    return dayOptions;
}

function getHowers()
{
    var hours = [10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22];
    var hourOptions = "";
    $(hours).each(function (i)
    {
        hourOptions += "<option value='" + hours[i] + "'>" + hours[i] + "</option>";
    });
    return hourOptions;
}

function getMinutes()
{
    var minutes = ["00", 10, 20, 30, 40, 50, 60];
    var minuteOptions = "";
    $(minutes).each(function (i)
    {
        minuteOptions += "<option value='" + minutes[i] + "'>" + minutes[i] + "</option>";
    });
    return minuteOptions;
}

function getThirtyDates()
{
    var currentTime = new Date();
    var date = new Date(currentTime.getFullYear(), currentTime.getMonth(), currentTime.getDate());
    var days = [];
    for (var i = 0; i < 30; i++)
    {
        days.push(new Date(date));
        date.setDate(date.getDate() + 1);
    }
    return days;
}

//Download html table as csv
function DownloadFile(filename, TextCols)
{
    var str = "";
    $("table.csvTable tr").each(function ()
    {
        //console.log(this);
        $("td,th", this).each(function (i)
        {
            var txt = $(this).text().replace(/,/g, '');
            if ($.inArray(i, TextCols) > -1)
                str += "=\"" + txt + "\",";
            else
                str += txt + ",";
        });
        str += "\n";
    });
    var element = document.createElement('a');
    element.setAttribute('href', 'data:text/plain;charset=utf-8,' + encodeURIComponent(str));
    element.setAttribute('download', filename);
    element.style.display = 'none';
    document.body.appendChild(element);
    element.click();
    document.body.removeChild(element);
}



