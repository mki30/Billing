<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="LoginDemo" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Login</title>
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <!-- The above 3 meta tags *must* come first in the head; any other head content must come *after* these tags -->
    <meta name="description" content="" />
    <meta name="author" content="" />
    <link rel="shortcut icon" type="image/x-icon" href="data:image/x-icon;base64, iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAACwElEQVR42qWTWUhUcRTGv//dRmdpHPcxDXFFTCYji3QwInLLxIKEIHuwB6koKUMNEnowomw1tAWiIhSTFJJoM8Olhwi0cSnKNTWX1HEb70yz3LldhcIIs+i8Hs7vfOc75xD8Z5A/JQtcZFvDgrV3IYKzWCzVNpsj/9iwkV8RcJymVZ5qxYPU7AS9Z3L6uN3Mm/mhQd9PlY/lg10jSTlDE6+XBUjFHsFK6m3c9rVB3vv3iR4f68wQRYXIcrCE6ycNVc/V7a86kiVI/W8AqdglUE4ZdkS4h7tm7gYDM5TDPXBM8BAcAjitGvbYpLnm0hqmp3PATxpn9hfAKY4uOZwedkRBCKwZe8F+7QZa2uES6AEHkYFv7Ycywhem9dtMj/JLDdk9o/FkSfc1u4Lk3bE1hZz1aiXmNurBMQKc9U0ATUGWewbW1no4n9aBjdM5eke+kS/99is/AXksfS4nJybPi9Bw9hoxGhAAdXIKrDdugXZhQCUmgouOx3RBLhT+bjDH6/mXJXX3yE0dTbLbBPG8gv2QGqaO0HqrwDAURqYs8LpcDLH8OsQ5HvywCXSgL5yjRtAsDTZzp+lh3p0XpKEobbNttvvCwP3Pm6LdZbSPRg5Wkmyct0KTHgtX3TrMlVVgVYA7WIVsUe1o+wg0J7Pmqw9dM5Dmi3vE2IQYlKUUIYQVoJVzULmycIrAOG9D2OksUDMzMJY3gpKUCQ4Rqkg/OPW6ydrCqmek9mjUmEzl4TP/ro+faZmQa11BaIoCs+COtA2WYxByIgOcUgn7GwNYHw3omCi8v/3E1tLYt2HRxOp0nwbT4OSB9jYwGo5UhKrp1QqOclsw/8exeEf6w00fCkrGCcONXdaOzqn6g71jacudso4iyPKWUVukaTzlNKEk5Q7eLpJpm9PgoJjisxZr04rPtAS4oIS6JAiWf/rGv4nvrmgICZULEkoAAAAASUVORK5CYII=" />
    <link href="css/bootstrap.min.css" rel="stylesheet" />
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js"></script>
    <meta name="google-signin-client_id" content="189254627461-tp89v4lq28cqt4o51ubev8ct7f5uq22g.apps.googleusercontent.com" />
    <%--<script src="https://apis.google.com/js/platform.js"></script>--%>

    <script>
        var DoSignOut = false;
        var Impersonate = "";
        function onSignIn(googleUser)
        {
            // The ID token you need to pass to your backend:
            var id_token = googleUser.getAuthResponse().id_token;

            if (DoSignOut)
                signOut();
            else
            {
                $("div.g-signin2").html("<img src='/images/progress2.gif'>&nbsp;Loading...");
                $.ajax({
                    type: "POST",
                    cache: false,
                    url: "data.aspx?Action=SignIn&Data1=" + Impersonate,
                    data: { token: id_token },
                    success: function (data)
                    {
                        if (data == "AdminOK")
                            location.href = "/Billing.aspx";
                        else if (data == "OK")
                        {
                            StoreAccessList();
                        }
                        else
                        {
                            $("#message").val(data);
                        }
                    }, error: function () { }
                });
            }
        }
        function signOut()
        {
            var auth2 = gapi.auth2.getAuthInstance();
            auth2.signOut().then(function ()
            {
                location.href = "/login.aspx";
            });
        }

        function StoreAccessList()
        {
            $.ajax({
                type: "GET",
                cache: false,
                url: "data.aspx?Action=GetStoreAccess",
                success: function (data)
                {
                    var obj = JSON.parse(data);
                    if (obj.length > 1)
                    {
                        var str = "<select class='form-control' onchange='GoClick($(this).val())'><option value=''>Select Store"
                        $.each(obj, function (index, object)
                        {
                            str += "<option value=" + this.ID + ">" + this.Name;
                        });
                        str += "</select>";
                        $("#divStoreSelection").html(str).show();
                    }
                    else
                        GoClick(obj[0].ID);
                }, error: function () { }
            });
        }

        function GoClick(store)
        {
            $.ajax({
                type: "GET",
                cache: false,
                url: "data.aspx?Action=SetStore&data1=" + store,
                success: function (data)
                {
                    if (data == "OK")
                        location.href = "/Billing.aspx";
                    else
                        alert(data);
                }, error: function () { }
            });
        }
        function onFailure(error)
        {
            console.log(error);
        }
        function renderButton()
        {
            gapi.signin2.render('g-signin2', {
                'scope': 'profile email',
                'width': 240,
                'height': 50,
                'theme': 'dark',
                'onsuccess': onSignIn,
                'onfailure': onFailure
            });
        }
    </script>
    <script src="https://apis.google.com/js/platform.js?onload=renderButton"></script>
</head>
<body>
    <form id="form1" runat="server">
        <%--<div style="width: 250px; margin: auto; margin-top: 50px;">
            <div style="text-align: center;">
                <div class="g-signin2" data-onsuccess="onSignIn" style="width: 100%;"></div>
            </div>
            <br />
            <br />
        </div>--%>
        <div style="width: 400px; margin: 0 auto; padding: 50px;">
            <div id="g-signin2"></div>
            <br />
            <div id="divStoreSelection" style="display: none; width: 240px;">
            </div>
            <div id="message"></div>
        </div>
    </form>
</body>
</html>
