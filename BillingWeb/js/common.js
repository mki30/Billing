$(window).resize(resizediv);

$(document).ready(function ()
{

    resizediv();
});

function Resize()
{

    var H = $(window).height();
    var DataHeight = $("div#divData1").height();
    var Margin = $("header.header").height() + $("footer.footer").height();

    if (DataHeight < H - Margin)
    {
        if (location.href == "http://localhost:50033/" || location.href == "http://localhost:50033/Default2.aspx" || location.href == "http://localhost:50033/dsc" || location.href == "http://fincomindia.in/" || location.href == "http://fincomindia.in/Default2.aspx" || location.href == "http://fincomindia.in/dsc")
            return;
        else
            $("div#divData1").css({ 'min-height': (H - Margin - 100) + "px" });
    }
}


function resizediv()
{
    Resize();

    var maxHeading = 75, maxSubheading = 205, maxdata = 0;
    var Heading = [$("#heading-1").outerHeight(), $("#heading-2").height(), $("#heading-3").height(), $("#heading-4").height()];
    var SubHeading = [$("#sub-heading-1").height(), $("#sub-heading-2").height(), $("#sub-heading-3").height(), $("#sub-heading-4").height()];
    var data = [$("#data1").height(), $("#data2").height(), $("#data3").height(), $("#data4").height()];
    for (var i = 0; i < 4; i++)
    {
        if (Heading[i] > maxHeading)
            maxHeading = Heading[i];
        if (SubHeading[i] > maxSubheading)
            maxSubheading = SubHeading[i];
        if (data[i] > maxdata)
            maxdata = data[i];
    }

    console.log(maxHeading, maxSubheading, maxdata);

    $("#heading-1").height(maxHeading); $("#heading-2").height(maxHeading); $("#heading-3").height(maxHeading); $("#heading-4").height(maxHeading);
    $("#sub-heading-1").height(maxSubheading); $("#sub-heading-2").height(maxSubheading); $("#sub-heading-3").height(maxSubheading); $("#sub-heading-4").height(maxSubheading);
    $("#data1").height(maxdata); $("#data2").height(maxdata); $("#data3").height(maxdata); $("#data4").height(maxdata);
    $(".btn-align").height(50);
}
//function Resize() {
//    debugger;
//    var H = $(window).height();
//    var DataHeight = $("#divDataArea").height();
//    var Margin = $("div.navbar").height() + $("footer.panel-footer").height();

//    if (DataHeight < H - Margin)
//        $("#divDataArea").css({ 'min-height': (H - Margin - 100) + "px" });
//}

function fnValidatePAN(Obj)   //Pan Validaion
{
    //  debugger;
    if (Obj == null) Obj = window.event.srcElement;
    if (Obj.val() != "")
    {
        ObjVal = Obj.val();
        var panPat = /^([a-zA-Z]{5})(\d{4})([a-zA-Z]{1})$/;
        var code = /([C,P,H,F,A,T,B,L,J,G])/;
        var code_chk = ObjVal.substring(3, 4);
        if (ObjVal.search(panPat) == -1)
        {
            //alert("Invalid Pan No");
            Obj.focus();
            return false;
        }
        if (code.test(code_chk) == false)
        {
            //alert("Invaild PAN Card No.");
            return false;
        }
    }
}

$(".decimal").on('keypress', function (e)
{
    //Only allow 0-9, '.' and backspace (charCode 0 in Firefox)
    if (e.charCode == 46)
    {
        var currentContents = $input.val();
        return !(currentContents.indexOf('.') !== -1);
    }
    return (e.charCode >= 48 && e.charCode <= 57) || e.charCode === 0;
})

function isNumberKey(evt)//for preventing the character value in integer textboxes //$(".Class").keypress(isNumberKey);
{
    var charCode = (evt.which) ? evt.which : event.keyCode
    if (charCode > 31 && (charCode < 48 || charCode > 57))
        return false;
    return true;
}

function isNumberKeyFloatNoAllowed(evt)//for stopping the character value in integer textboxes //$(".Class").keypress(isNumberKey);
{
    var charCode = (evt.which) ? evt.which : event.keyCode
    if (charCode > 31 && (charCode < 48 || charCode > 57 || charCode == 46))
        return false;
    return true;
}

function isAlphabet(evt)//for stopping the character value in integer textboxes //$(".Class").keypress(isAlphabet);
{
    var charCode = (evt.which) ? evt.which : event.keyCode
    if (!(charCode > 31 && (charCode < 48 || charCode > 57)))
        return false;

    return true;
}

function IsEmail(email)
{
    var regex = /^([a-zA-Z0-9_\.\-\+])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
    if (!regex.test(email))
    {
        return false;
    } else
    {
        return true;
    }
}

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







