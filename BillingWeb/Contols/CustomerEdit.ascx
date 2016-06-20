<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CustomerEdit.ascx.cs" Inherits="Contols_CostomerEdit" %>
<script type="text/javascript">
    $(document).ready(function ()
    {
        //$("#ddProject").change(function ()
        //{
        //    GetApartments($(this).val());
        //});
    });
    function ShowEdit(id)
    {
        OpenPopup2();
        $('#CustomerForm').find('input:text').val('');
        if (id != 0)
        {
            $.ajax({
                url: "/Data.aspx?Action=GetCustomer&Data1=" + id,
                cache: false,
                success: function (data)
                {
                    var C = data;
                    console.log(C);
                    $("#txtID").val(C.ID);
                    $("#txtName").val(C.Name);
                    $("#txtMobile").val(C.Mobile);
                    $("#txtCost").val(C.Cost);
                    $("#txtHouseNo").val(C.HouseNumber);
                    $("#txtAddress").val(C.Address);
                    $("#txtArea").val(C.Area);
                    $("#txtCity").val(C.City);
                    $("#txtPin").val(C.PIN);
                    $("#txtBirthday").val(C.BirthdayString);
                    $("#txtAnniverary").val(C.AnniveraryString);
                    $("#txtLatAdvertized").val(C.LastAdvertizedDateString);
                    $("#ddCustomerStore").val(C.StoreID);

                    if (C.ProjectID != 0)
                        $("#txtProjectID").val(C.ProjectID);
                    else
                        $("#txtProjectID").val("");

                    GetSocietyName(C.ProjectID);
                    GetApartments(C.ProjectID, C.ApartmentID);

                    //if (C.ApartmentID != 0)
                    //    $("#ddProjectApartment").val(C.ApartmentID);
                }
            });
        }
        return false;
    }

    function GetSocietyName(SocietyID)
    {
        $.ajax({
            url: "/Data.aspx?Action=GetProjectName&Data1=" + SocietyID,
            cache: false,
            success: function (obj)
            {
                $("#txtProject").val(obj);
            }   
        });
    }

    //function FillSociety(Name)
    //{
    //    if ($("#txtHouseNo").val() == "" && $("#txtMobile").val() != "")
    //        $("#txtHouseNo").val($("#txtMobile").val());

    //    $("#txtAddress").val(Name);
    //    $("#txtArea").val("Sector 104");
    //    $("#txtCity").val("Noida");
    //    $("#txtPin").val("201301");

    //}
    function Save()
    {
        var str = $("#CustomerForm :input").serialize();
        console.log(str);
        var data = str.replace(/ctl00%24ContentPlaceHolder1%24CostomerEdit%24/g, '');

        $("#btnSave").val("Saving...");
        $.ajax({
            type: "POST",
            url: "/Data.aspx?Action=SaveCustomer&Data1=",
            data: data,
            cache: false,
            success: function (obj)
            {
                $("#btnSave").val("Save");
                location.reload();
                //New();
            }
        });
        return false;
    }

    function GetApartments(ProjectID, ApartmentID)
    {
        if (ProjectID != "" && ProjectID != 0)
        {
            $.ajax({
                url: "/Data.aspx?Action=GetProjectApartments&Data1=" + ProjectID,
                cache: false,
                success: function (obj)
                {
                    console.log(obj);
                    var selctItems = "";
                    $(obj).each(function ()
                    {
                        selctItems += "<option value='" + this.ID + "'>" + this.FloorNo + "</option>";
                    });
                    $("#ddProjectApartment").html(selctItems);
                    if (ApartmentID != 0)
                        $("#ddProjectApartment").val(ApartmentID);
                }
            });
        }
        else
            $("#ddProjectApartment").empty();

        $("#linkMsg").text("");
    }

    //function AutolinkApartmentWithCustomer(ProjectID)
    //{
    //    $.ajax({
    //        url: "/Data.aspx?Action=AutolinkApartmentWithCustomer&Data1=" + ProjectID
    //            + "&Data2=" + $("#txtMobile").val()
    //            + "&Data3=" + $("#txtHouseNo").val()
    //            + "&Data4=" + $("#txtID").val(),
    //        cache: false,
    //        success: function (obj)
    //        {
    //            $("#linkMsg").text(obj);
    //        }
    //    });
    //}

    function ProjectSearchJSON(obj)
    {
        $("#divItemSearch").css('display', 'none');
        $("#divItemSearch").html("");
        $.ajax({
            url: "/Data.aspx?Action=GetProjectJSON&query=" + $(obj).val(), success: function (data)
            {
                if (data.length > 0)
                {
                    var str = "<table>";
                    $(data).each(function (i, v)
                    {
                        if (typeof RecordType === 'undefined')
                            RecordType = ReportType
                        str += "<tr><td>" + v.id + "<td><a onclick='SelectSociety("+v.id+",\""+v.name+"\")'>" + v.name + "</a>";
                    });
                    $("#divItemSearch").show();
                    $("#divItemSearch").html(str + "</table>");
                }
            }
        });
    }

    function SelectSociety(SocityID,SocityName)
    {
        $("#txtProjectID").val(SocityID);
        $("#txtProject").val(SocityName);
        GetApartments(SocityID, 0);
        $("#divItemSearch").hide();
    }
</script>
<div id="divDialog2" class="dialog">
    <span onclick="ClosePopup2()" style="cursor: pointer; color: red;" class="pull-right glyphicon glyphicon-remove"></span>
    <div style="float: left;" id="CustomerForm">
        <div style="float: left;">
            <table class='table-condensed' style="width: 50%">
                <tr style="display: none;">
                    <td>ID</td>
                    <td>
                        <input type="text" name="txtID" id="txtID" value="0" />
                    </td>
                </tr>
                <tr>
                    <td>Name</td>
                    <td>
                        <asp:TextBox ID="txtName" runat="server" Placeholder="Name" Required="required" MaxLength="50"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>Mobile</td>
                    <td>
                        <asp:TextBox ID="txtMobile" runat="server" Placeholder="Mobile" Required="required" MaxLength="12" Width="100%"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>House No</td>
                    <td>
                        <asp:TextBox ID="txtHouseNo" runat="server" Placeholder="House No" Width="100%" MaxLength="25"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>Address</td>
                    <td>
                        <asp:TextBox ID="txtAddress" runat="server" Placeholder="Address" MaxLength="50"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>Area</td>

                    <td>
                        <asp:TextBox ID="txtArea" runat="server" Placeholder="Area" MaxLength="50"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>City</td>
                    <td>
                        <asp:TextBox ID="txtCity" runat="server" Placeholder="City" MaxLength="50"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>Pin</td>
                    <td>
                        <asp:TextBox ID="txtPin" runat="server" Placeholder="Pin" MaxLength="6" Width="100%"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>Birthday</td>
                    <td>
                        <asp:TextBox ID="txtBirthday" runat="server" Placeholder="Birthday" Width="100%" CssClass="datepicker"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>Anniverary</td>
                    <td>
                        <asp:TextBox ID="txtAnniverary" runat="server" Placeholder="Anniverary" Width="100%" CssClass="datepicker"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>Store</td>
                    <td>
                        <asp:DropDownList ID="ddCustomerStore" runat="server"></asp:DropDownList>
                    </td>
                </tr>
                <tr>

                    <td></td>
                    <td>
                        <input type="button" id="btnSave" value="Save" class="btn btn-sm btn-primary" onclick="Save();" style="margin: 10px;" />
                        <asp:Label ID="lblMsg" runat="server" Text=""></asp:Label></td>
                </tr>
            </table>
        </div>
        <div style="float: left;">
            <table class='table-condensed'>
                <tr>
                    <td>Society</td>
                    <td>
                        <asp:TextBox ID="txtProjectID" runat="server" style="width:30px;display:none;"></asp:TextBox>
                        <asp:TextBox ID="txtProject" runat="server" onkeyup="ProjectSearchJSON(this)"></asp:TextBox>
                        <div id="divItemSearch" class="searchresult"></div>
                    </td>
                </tr>
                <tr>
                    <td>Apartment</td>
                    <td>
                        <asp:DropDownList ID="ddProjectApartment" runat="server"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>Last Advt.</td>
                    <td>
                        <asp:TextBox ID="txtLatAdvertized" runat="server" Placeholder="Last Advt." Width="100" CssClass="datepicker"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </div>
        <div style="float: left;">
            <asp:Label ID="lblProjectList" runat="server" Text="" EnableViewState="false"></asp:Label>
            <br />
            <span id="linkMsg"></span>
        </div>
    </div>
</div>
