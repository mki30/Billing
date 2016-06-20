<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Items.aspx.cs" Inherits="Items" ClientIDMode="Static" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="js/common.js"></script>
    <script src="js/TableSearch.js"></script>
    <script>
        $(document).ready(function ()
        {
            $("#txtPluCode").keypress(isNumberKey);

            var contentHeight = $(window).height() - $("#divFilters").height() - $(".navbar").height() - $("#footer").height();
            $("#divItem").height(contentHeight - 30);
            $("#frameItemForm").height(contentHeight - 40)
        });

        function LinkItemWithMenu(itemID)
        {
            $.post("/Data.aspx?Action=UpdateItemMenuLink&Data1=" + itemID + "&Data2=" + menuId,
                {
                }, function (r)
                {
                    if (r !== "ok")
                        alert(r);
                });
        }
        function ClearSearch()
        {
            $("#txtSearch").val("");
            ShowAllRows();
        }
        function ShowEditItem(id, obj)
        {
            $("#frameItemForm")[0].src = "/ItemEdit.aspx?id=" + id;
        }
        function EditLoaded(id)
        {
            if (id == 0)
                return;
            $.ajax({
                url: "/Data.aspx?Action=GetItemForList&Data1=" + id, cache: false, success: function (data)
                {
                    var i = JSON.parse(data);
                    $("#DataTable > tbody > tr").removeClass("highlight");
                    if ($("#tr" + id).length == 0)
                        $("#DataTable tr:first").after("<tr id='tr" + id + "'><td><td><a href='#' onclick='ShowEditItem(" + id + ",this)'></a><td><td><td><td><td><td>");

                    $("#tr" + id).addClass("highlight");
                    $("a", "#tr" + id).first().html(i.Name);
                    $("td:nth-child(3)", "#tr" + id).html(i.PLU);
                    $("td:nth-child(5)", "#tr" + id).html(i.TaxRate);
                    $("td:nth-child(6)", "#tr" + id).html(i.Cost);
                    $("td:nth-child(8)", "#tr" + id).html(i.MRP);
                    $("td:nth-child(9)", "#tr" + id).html(i.BrandName);
                }
            });
        }
        function ChangeURL(obj)
        {
            var brandSelctedValue = $("#ddBrand :selected").val();
            var adjust = $("#ddToAdjust :selected").val();
            var objChkShowAll = $("#chkShowAll").is(":checked");

            location.href = "/Items.aspx?q=1" + (brandSelctedValue == "" ? "" : "&brandid=" + brandSelctedValue)
                + (objChkShowAll == false ? "" : "&showall=1")
                + (adjust == "" ? "" : "&adjust=" + adjust);
        }

        var itemArray = [];
        function OpenMultiLinkBox()
        {
            if (itemArray.length == 0)
                alert("No item is selected please select items using checkbox!")
            else
                OpenPopup();
        }

        function PushId(obj)
        {
            var id = $(obj).val();
            if (itemArray.indexOf(id) != -1)
                itemArray.splice(itemArray.indexOf(id), 1)
            else
                itemArray.push(id);
        }

        function SaveMultiLinks()
        {
            var brandID = $("#ddBrandMultilink").val();
            var categoryID = $("#ddCategoryMultilink").val();
            if (brandID == "" && categoryID == "")
                alert("Please select brand or catagory!");
            else
            {
                $.ajax({
                    url: "/Data.aspx?Action=ItemMultilLink&Data1=" + brandID + "&Data2=" + categoryID + "&Data3=" + itemArray.toString(), cache: false, success: function (data)
                    {
                        itemArray.length = 0;
                        $('#DataTable tr td input[type="checkbox"]').each(function ()
                        {
                            $(this).prop('checked', false);
                        });
                        alert(data);
                    }
                });
            }
        }
        function SelectAllCheckBox(obj)
        {
            if ($("#chkAll").is(":checked"))
            {
                $("#DataTable tr:not(:first)").each(function ()
                {
                    if ($(this).css('display') != 'none')
                    {
                        $("input:checkbox", $(this)).prop('checked', $("input:checkbox", $(this)).is(":checked"))
                        var obj = $("input:checkbox", $(this))
                        PushId(obj[0]);
                        $(itemArray).each(function (index)
                        {
                            $("#chk" + itemArray[index]).prop('checked', true);
                        });
                    }
                });
            }
            else
            {
                $(itemArray).each(function (index)
                {
                    $("#chk" + itemArray[index]).prop('checked', false);
                });
                itemArray.length = 0;
            }
        }

        function UpdateFieldValue()
        {
            if ($("#txtFieldValue").val() == "")
            {
                alert("Please select a value");
                $("#txtFieldValue").focus();
            }
            else
            {
                $.ajax({
                    url: "/Data.aspx?Action=UpdateFieldValue&Data1=" + $("#ddFieldName").val() + "&Data2=" + $("#txtFieldValue").val()
                        + "&Data3=Item" + "&Data4=" + itemArray.toString(), cache: false, success: function (data)
                        {
                            alert(data);
                            itemArray.length = 0;
                            $('#DataTable tr td input[type="checkbox"]').each(function ()
                            {
                                $(this).prop('checked', false);
                            });
                        }
                });
            }
        }
    </script>
    <style>
        #DataTable td:nth-child(4)
        {
            text-align: left;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server" ViewStateMode="Disabled" ValidateRequestMode="Disabled">
    <div>
        <div id="divFilters">
            <table>
                <tr>
                    <td>
                        <asp:TextBox ID="txtSearch" runat="server" placeholder="Search" autocomplete="off"></asp:TextBox></td>
                    <td>
                        <input type="button" value="Clear" onclick="ClearSearch()" class="btn btn-xs btn-warning" />
                    </td>
                    <td>
                        <asp:DropDownList ID="ddBrand" runat="server" onchange="ChangeURL(this);"></asp:DropDownList>
                    </td>
                    <td>
                        <a href="#" onclick='DownloadFile("Items.csv",[2])'>CSV&nbsp;<i class='glyphicon glyphicon-download-alt'></i></a>
                    </td>
                    <td>
                        <asp:Button ID="lblUpdateHidden" runat="server" Text="Update Inactive Items" OnClick="lblUpdateHidden_Click" />
                    </td>
                    <td>
                        <asp:CheckBox ID="chkShowAll" runat="server" onchange="ChangeURL(this);" />
                        Only Active
                    </td>
                    <td>
                        <asp:DropDownList ID="ddToAdjust" runat="server" onchange="ChangeURL(this);">
                            <asp:ListItem Value="">-Select-</asp:ListItem>
                            <asp:ListItem Value="1">Adjust</asp:ListItem>
                            <asp:ListItem Value="2">Bill</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td><a href='#' onclick="OpenMultiLinkBox();">Multi Link</a></td>
                </tr>
            </table>
        </div>
        <div style="width:100%;">
            <div style="width: 75%;display: block; position: fixed;">
                <div id="divItem" style="height: 780px; overflow: auto;">
                        <asp:Literal ID="lblDataTable" runat="server" Text="" EnableViewState="false"></asp:Literal>
                    </div>
            </div>
            <div style="width:23%;display: block;float: right;">
                <iframe src="/ItemEdit.aspx" id="frameItemForm" style="height: 500px; border: 1px solid #ddd; padding: 5px;"></iframe>
            </div>
        </div>
    </div>
    <div id="divDialog" class="dialog">
        <span onclick="ClosePopup()" style="cursor: pointer;" class="glyphicon glyphicon-remove" aria-hidden="true"></span>
        <div id="divDialogContent">
            <table>
                <tr>
                    <td>Brand</td>
                    <td>
                        <asp:DropDownList ID="ddBrandMultilink" runat="server" Width="100%"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>Category</td>
                    <td>
                        <asp:DropDownList ID="ddCategoryMultilink" runat="server" Width="100%">
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
            <a href="#" onclick="SaveMultiLinks()" class="btn btn-success">Link</a>
            <table>
                <caption>Update Field Value</caption>
                <tr>
                    <td>
                        <asp:DropDownList ID="ddFieldName" runat="server" Width="100%">
                            <asp:ListItem Value="ToAdjust">ToAdjust</asp:ListItem>
                        </asp:DropDownList></td>
                    <td>
                        <asp:TextBox ID="txtFieldValue" runat="server"></asp:TextBox>
                    </td>
                    <td><a href="#" onclick="UpdateFieldValue()" class="btn btn-success">Update</a></td>
                </tr>
            </table>
            <span id="infoText"></span>
        </div>
    </div>
    <div id="divDialogfade" class="black_overlay"></div>
</asp:Content>

