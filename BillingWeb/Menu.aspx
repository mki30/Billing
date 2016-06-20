<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Menu.aspx.cs" Inherits="Menu" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="css/jstree.css" rel="stylesheet" />
    <script src="js/common.js"></script>
    <script src="js/jquery.jstree.js"></script>
    <script src="js/ajaxTree.js"></script>
    <script src="js/TableSearch.js"></script>
    <script src="js/bootstrap-typeahead.min.js"></script>
    <script>
        $(document).ready(function ()
        {
            BuildTree("#Node1", 1);

            //$('.typeahead').typeahead(
            //{
            //    onSelect: function (item)
            //    {
            //        LinkItemWithMenu(item.value, item.text);
            //    },
            //    ajax: {
            //        url: "/Data.aspx?Action=GetItemJSON",
            //        timeout: 500,
            //        triggerLength: 1,
            //        method: "get",
            //        loadingClass: "loading-circle",
            //        preDispatch: function (query)
            //        {
            //            return {
            //                query: query
            //            }
            //        },
            //        preProcess: function (data)
            //        {
            //            return data;
            //        }
            //    }
            //});
            //GetMenueLinkedItems(4);
        });

        var ItemIds = [];
        function ItemSearch(obj)
        {
            ItemIds.length = 0;
            $("#divItemSearch").css('display', 'none');
            $("#divItemSearch").html("");
            $.ajax({
                url: "/Data.aspx?Action=GetItemJSONSkipLinked&query=" + $(obj).val() + "&Data1=" + menuId, success: function (data)
                {
                    if (data.length > 0)
                    {
                        var str = "<a href='#' onclick='LinkItemWithMenuMultiple()' class='label label-success'>Link all checked items</a><table id='searchItemTable' class='table table-bordered'>&nbsp;&nbsp;<input type='checkbox' onclick='SelectAllCheckBox(this)'>Select All";
                        $(data).each(function (i, v)
                        {
                            if (typeof RecordType === 'undefined')
                                RecordType = ReportType
                            str += "<tr id='tr" + v.id + "'><td><input type='checkbox' value='" + v.id + "' onclick='AddInArray(this)'><td>" + v.plu + "<td>" + v.name;
                        });
                        $("#divItemSearch").show();
                        $("#divItemSearch").html(str + "</table>");
                    }
                }
            });
        }

        function SelectAllCheckBox(obj)
        {
            $("#searchItemTable input:checkbox").prop('checked', $(obj).is(":checked"))
            $('#searchItemTable input:checkbox').each(function ()
            {
                    AddInArray(this);
            });
        }

        function AddInArray(obj)
        {
            itemid = $(obj).val();
            var index = ItemIds.indexOf(itemid);
            if ($(obj).is(":checked"))
            {
                ItemIds.push(itemid);
            }
            else
            {
                if (index >= 0)
                    ItemIds.splice(index, 1);
            }
            console.log(ItemIds);
        }

        function GetMenueLinkedItems(menuId)
        {
            $.ajax({
                url: "/Data.aspx?Action=GetMenueLinkedItems&Data1=" + menuId, cache: false, success: function (data)
                {
                    $("#lblDataTable").html(data);
                }
            });
        }

        function LinkItemWithMenu(itemID, ItemName)
        {
            if (confirm("Confirm to link " + ItemName + " with " + menuText))
            {
                $.post("/Data.aspx?Action=UpdateItemMenuLink&Data1=" + itemID + "&Data2=" + menuId,
                    {
                    }, function (r)
                    {
                        if (r == "ok")
                            location.reload();
                        else
                            alert(r);
                    });
            }
        }

        function LinkItemWithMenuMultiple()
        {
            $.ajax({
                type: "POST",
                url: "/Data.aspx?Action=UpdateItemMenuLinkMultiple&Data1=&Data2=" + menuId,
                data: { ItemIds: ItemIds.toString() },
                cache: false,
                success: function (obj)
                {
                    alert("Done");
                    RemoveAddedItemsFromSearchList();
                    GetMenueLinkedItems(menuId);
                }
            });
        }

        function RemoveAddedItemsFromSearchList()
        {
            $(ItemIds).each(function (index)
            {
                $("#tr" + ItemIds[index]).remove();
            });
            ItemIds.length = 0;
        }

        function DeleteMenuItemLink(ID)
        {
            if (confirm("Sure to delete?"))
            {
                $.ajax({
                    url: "/Data.aspx?Action=DeleteMenuItemLink&Data1=" + ID, cache: false, success: function (data)
                    {
                        if (data == "")
                            location.reload();
                    }
                });
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="panel panel-info">
        <div class="panel-heading">
            <table class="table-condensed" style="width: 40%">
                <tr>
                    <td class="hide">ID:<asp:Label ID="lblID" runat="server" Text="0"></asp:Label></td>
                    <td class="hide">Parent ID:<asp:Label ID="lblParentID" runat="server" Text="0"></asp:Label></td>
                    <td>Name</td>
                    <td>
                        <asp:TextBox ID="txtName" runat="server" Style="width: 300px;"></asp:TextBox></td>
                    <td>
                        <asp:Button ID="btnSave" CssClass="btn btn-sm btn-success" runat="server" Text="Add" OnClick="btnSave_Click" OnClientClick="return Validate();" />
                        <asp:Label ID="lblStatus" runat="server"></asp:Label>
                    </td>
                    <td>
                        <%--<asp:TextBox ID="txtSearch" runat="server"  placeholder="Search Item" autocomplete="off"></asp:TextBox>--%>
                        <asp:TextBox ID="searchItem" class="typeahead" runat="server" placeholder="Search item to link with selected calegory." Style="width: 400px;" onkeyup="ItemSearch(this)"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <table>
        <tr>
            <td style="vertical-align: top;">
                <div id="Node1"></div>
            </td>
            <td style="vertical-align: top;">
                <div id="divItemSearch"></div>
                <asp:Label ID="lblDataTable" runat="server" Text=""></asp:Label></td>
        </tr>
    </table>
</asp:Content>

