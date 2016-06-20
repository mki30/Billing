<%@ Page Language="C#" AutoEventWireup="true" CodeFile="LoginDemo.aspx.cs" Inherits="LoginDemo" %>

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
    <link rel="shortcut icon" type="image/x-icon" href="data:image/x-icon;base64, AAABAAEAEBAQAAAAAAAoAQAAFgAAACgAAAAQAAAAIAAAAAEABAAAAAAAwAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAACAAACAAAAAgIAAgAAAAIAAgACAgAAAwMDAAICAgAAAAP8AAP8AAAD//wD/AAAA/wD/AP//AAD///8A//////////////////////////////////APAPAPAP//mZmZmZmZn/+ZmZmZmZn//5mZmZmZn///n/mZmZmf//+f+ZmZmZ//+ZmZ//mf////////+Z/////w/////////w8PD/D/////8PDw//////////////////////////8AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA" />
    <link href="css/bootstrap.min.css" rel="stylesheet" />
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js"></script>
    <meta name="google-signin-client_id" content="189254627461-tp89v4lq28cqt4o51ubev8ct7f5uq22g.apps.googleusercontent.com" />
    <script src="https://apis.google.com/js/platform.js"></script>
    <script>
        var DoSignOut = false;
        function onSignIn(googleUser)
        {
            // The ID token you need to pass to your backend:
            var id_token = googleUser.getAuthResponse().id_token;

            if (DoSignOut)
                signOut();
            else
            {
                $.ajax({
                    type: "POST",
                    cache: false,
                    url: "data.aspx?Action=SignIn",
                    data: { token: id_token },
                    success: function (data)
                    {
                        if (data == "OK")
                        {
                            StoreAccessList();
                        }
                        else
                            alert(data);
                    }, error: function () { }
                });
            }
        }
        function signOut()
        {
            var auth2 = gapi.auth2.getAuthInstance();
            auth2.signOut().then(function ()
            {
                location.href = "/logindemo.aspx";
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
                    $.each(obj, function (index, object)
                    {
                        $("#ddStore").append("<option value=" + this.ID + ">" + this.Name + "</option>");
                    });
                    if (obj.length > 1)
                        $("#divStoreSelection").show();
                    else
                        GoClick();
                }, error: function () { }
            });
        }

        function GoClick()
        {
            $.ajax({
                type: "GET",
                cache: false,
                url: "data.aspx?Action=SetStore&data1=" + $("#ddStore").val(),
                success: function (data)
                {
                    if (data == "OK")
                        location.href = "/Billing.aspx";
                    else
                        alert(data);
                }, error: function () { }
            });
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div style="width: 300px; margin: auto; padding: 50px;" class="label-primary">
            <div class="g-signin2" data-onsuccess="onSignIn"></div>
            <div id="divStoreSelection" style="display: none;">
                <br />
                <table>
                    <tr>
                        <td>
                            <asp:DropDownList ID="ddStore" runat="server" CssClass="form-control"></asp:DropDownList></td>
                        <td></td>
                        <asp:Button ID="btnGo" class="btn btn-md btn-success pull-right" runat="server" Text="Go" OnClientClick="GoClick()" />
                    </tr>
                </table>
                <br />
            </div>
        </div>
    </form>
</body>
</html>
