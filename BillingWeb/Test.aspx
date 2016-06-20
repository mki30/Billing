<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Test.aspx.cs" Inherits="Testaspx" %>

<!DOCTYPE html>
<link href="css/bootstrap.min.css" rel="stylesheet" />
<link href="css/bootstrap-datetimepicker.min.css" rel="stylesheet" />
<script src="js/jquery-1.11.3.min.js"></script>
<script src="js/bootstrap.min.js"></script>
<script src="js/bootstrap-datetimepicker.min.js"></script>
<script type="text/javascript">
    $(document).ready(function ()
    {
        $(".form_datetime").datetimepicker({
            format: 'dd-M-yyyy hh:ii',
            //weekStart: 1,
            //todayBtn: 1,
            autoclose: 1,
            //todayHighlight: 1,
            startView: 2,
            forceParse: 0,
            showMeridian: 1
        });
    });
</script>
<html>
<body>
    <form runat="server">
        <br />
        <br />
        <input type="text" class="form_datetime">        
     </form>
</body>

</html>
