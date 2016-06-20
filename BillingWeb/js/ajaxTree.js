var BrandID = 0, MenuType = 1, viewPage = 0;
var ShowData = true;
var reload = true, isEdit = false;
var menuId = 0;
var menuText = "";

function BuildTree(ID, MenuTypes)
{
    return $(ID)
    .bind("before.jstree", function (e, data)
    {
        $("#alog").append(data.func + "<br />");
    })
    .jstree({
        "plugins": ["themes", "localStorage", "json_data", "ui", "crrm", "dnd", "search", "types", "contextmenu"],
        "themes": {
            "theme": "default",
            "dots": true,
            "icons": true
        },
        "checkbox": {
            "keep_selected_style": false,
            "two_state": true
        },
        contextmenu: {
            items:
            {
                "seqUpdate": {
                    "label": "Update Sequence",
                    "action": function (obj, data)
                    {
                        //ShowUpdataSequence(obj.attr("dataid"), obj.attr("id"), obj.attr("sequence"), obj.attr("menutype"));
                    }
                },
                "images":
                {
                    "label": "Edit Menu",
                    "action": function (obj, data)
                    {
                        //$.fancybox({
                        //    'scrolling': 'no',
                        //    'autoSize': false,
                        //    height: $(window).height(),
                        //    'type': 'iframe',
                        //    'href': '../edit/menuInfo.aspx?MenuID=' + obj.attr("id")
                        //});                         
                    }
                },
                "ccp": false
            }
        },
        "json_data": {
            "ajax": {
                "url": "/data.aspx?Action=GET_CHILDREN&Data2=" + MenuType,
                "data": function (n)
                {
                    return {
                        "Data1": n.attr ? n.attr("id").replace("node_", "") : 0,
                        IsLoade: true
                    };
                }, "success": function ()
                {

                }
            }
        },
        "core": { "html_titles": true },

        "cookies": {
            "cookie_options": {
                "path": "/",
                "prefix": MenuType
            }
        },
        "localStorage": {
            "localStorage_options": {
                "prefix": MenuType
            }
        },
    })
    .bind("select_node.jstree", function (e, data)  
    {
        try
        {
            menuId = data.rslt.obj.attr("id"); 
            menuText = data.rslt.obj.attr("name");
            //ShowMenuDetail(data.rslt.obj.attr("id"), MenuType, data.rslt.obj.attr("referenceid"));
            GetMenueLinkedItems(menuId);
        }
        catch (a)
        {
        }
    }).bind("rename.jstree", function (e, data)
    {
        var oldName = data.rslt.old_name.split('(')[0];
        if (oldName.toLowerCase() == data.rslt.new_name.split('(')[0].toLowerCase()) return;

        if (confirm("Confirm rename \"" + data.rslt.old_name.split('(')[0] + "\""))
        {
            $.post(
                "../Data.aspx?Action=MENU_ACTION&Data1=1&Data2=" + data.rslt.obj.attr("id") + "&Data3=" + MenuType,
                {
                    "New_Name": data.rslt.new_name
                }, function (r)
                {
                    if (r !== "ok")
                        $.jstree.rollback(data.rlbk);
                    else
                        data.inst.refresh();
                });
        }
    }).bind("create.jstree", function (e, data)
    {
        $.post("/data.aspx?Action=MENU_ACTION&Data1=2&Data2=" + data.inst._get_parent(data.rslt.obj).attr("id") + "&Data3=" + MenuType,
            {
                "New_Name": data.rslt.name,
            }, function (r)
            {
                //alert(r);
                //if (r !== "ok")
                //    $.jstree.rollback(data.rlbk);
                //else
                    data.inst.refresh();
            });
    }).bind("remove.jstree", function (e, data)
    {
        if (confirm("Confirm Delete \"" + data.rslt.obj.attr("name") + "\"\n Note: if it has child rows then it can't be deleted."))
        {
            $.post("/data.aspx?Action=MENU_ACTION&Data1=3&Data2=" + data.rslt.obj.attr("id") + "&Data3=" + MenuType,
                {
                    "New_Name": data.rslt.name
                }, function (r)
                {
                    if (r !== "no" && r !== "ok")
                    {
                        alert(r);
                        $.jstree.rollback(data.rlbk);
                    }
                    else if (r === "no")
                    {
                        alert("Not able to delete, multiple childs found.");
                        $.jstree.rollback(data.rlbk);
                    } else
                        data.inst.refresh();
                });
        } else
            $.jstree.rollback(data.rlbk);
    }).bind("move_node.jstree", function (event, data)
    {
        var thisID = data.rslt.o.attr("id"), ToID = data.rslt.np.attr("id").replace("Node", "");
        if (ToID == 1 || ToID == 3 || ToID == 13) ToID = 0;
        var thisName = data.rslt.o.attr("name"), thisParent = data.rslt.o.attr("pid"), ToName = data.rslt.np.attr("name");
        if (ToID === 0) ToName = "Home";
        if (thisParent == ToID)
        {
            alert("Already Linked to this Menu");
            return;
        }

        if (confirm("Moving \"" + thisName + "\"" + " TO \"" + ToName + "\""))
        {
            $.post("/data.aspx?Action=MENU_ACTION&Data1=4&Data3=" + MenuType,
                {
                    "ID": thisID,
                    "TOID": ToID
                }, function (r)
                {
                    if (r != "ok")
                    {
                        alert("Not able to change parent, you may try again.");
                        $.jstree.rollback(data.rlbk);
                    } else
                        data.inst.refresh();
                });
        } else
            $.jstree.rollback(data.rlbk);
    });
}

function refreshTree(menuType)
{
    $.jstree._reference($("#Node" + menuType)).refresh(-1);
}