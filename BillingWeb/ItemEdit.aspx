<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ItemEdit.aspx.cs" Inherits="ItemEdit" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Item Edit</title>
    <link href="BootStrap/css/bootstrap.min.css" rel="stylesheet" />
    <script src="js/jquery-1.11.3.min.js"></script>
    <script src="js/dropzone.js"></script>
    <style type="text/css">
        .dropzone
        {
            border: 2px dashed #0087F7;
            -moz-border-radius: 5px;
            -webkit-border-radius: 5px;
            border-radius: 5px;
            background: white;
            min-height: 100px;
            -moz-box-sizing: border-box;
            -webkit-box-sizing: border-box;
            box-sizing: border-box;
            cursor: pointer;
        }

        input[type='button'], input[type='submit']
        {
            cursor: pointer;
        }

        input:focus
        {
            background-color: pink;
        }

        select:focus
        {
            background-color: pink;
        }

        select
        {
            padding: 3px;
        }

        a
        {
            color: #1479FB;
        }
    </style>

    <script>
        $(document).ready(function ()
        {
            $("#txtPluCode").blur(function ()
            {
                if ($(lblID).text() == "0")
                {
                    $.ajax({
                        url: "/Data.aspx?Action=GetItem&Data1=" + $(this).val(), cache: false, success: function (data)
                        {
                            var obj = $.parseJSON(data);
                            if (obj)
                                window.location.href = '/ItemEdit.aspx?id=' + obj.ID;
                        }
                    });
                }
            }
        )
        });
        $(function ()
        {
            $("input").click(function ()
            {
                $(this).select();
            });

        });

        function OpenlinkDilauge(ItemID)
        {
            var plu = prompt("Please enter PLU", "");
            if (plu != null)
            {
                $.ajax({
                    url: "/Data.aspx?Action=GetItemByPLUandCreate&Data1=" + plu + "&Data2=" + ItemID, cache: false, success: function (data)
                    {
                        location.href = "/itemedit.aspx?id=" + data;
                    }
                });
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server" autocomplete="off">
        <table style="width: 100%">
            <tr>
                <td class="hide">
                    <asp:Label ID="lblID" runat="server" Text="0"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>PLU</td>
                <td>
                    <asp:TextBox ID="txtPluCode" runat="server" MaxLength="15" required="required" placeholder="PLU" Width="100%"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>Name</td>
                <td>
                    <asp:TextBox ID="txtName" runat="server" MaxLength="50" required="required" Width="100%" placeholder="Item Name"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>Tax</td>
                <td>
                    <asp:DropDownList ID="ddTaxRate" runat="server" required="required" Width="100"></asp:DropDownList>
                </td>
            </tr>

            <tr>
                <td>Cost</td>
                <td>
                    <table>
                        <tr>
                            <td>
                                <asp:TextBox ID="txtCost" runat="server" required="required" Width="100" placeholder="Rate"></asp:TextBox>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddRateType" runat="server"></asp:DropDownList>
                            </td>
                        </tr>
                    </table>

                </td>
            </tr>
            <tr>
                <td>Franch Rate</td>
                <td>
                    <asp:TextBox ID="txtCostFranchise" runat="server" required="required" Width="100" placeholder="Franchise Rate"></asp:TextBox></td>
            </tr>
            <tr>
                <td>Weight</td>
                <td>
                    <table>
                        <tr>
                            <td>
                                <asp:TextBox ID="txtWeight" runat="server" placeholder="Weight" Width="100"></asp:TextBox>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddWeightType" runat="server"></asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>MRP</td>
                <td>
                    <asp:TextBox ID="txtMRP" runat="server" required="required" placeholder="MRP" Width="100"></asp:TextBox></td>
            </tr>
            <tr>
                <td>Brand</td>
                <td>
                    <asp:DropDownList ID="ddBrand" runat="server" required="required" Width="100%"></asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>Category</td>
                <td>
                    <asp:DropDownList ID="ddCategory" runat="server" Width="100%">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>Reorder</td>
                <td>
                    <asp:TextBox ID="txtReOrderLevel" runat="server" placeholder="Reorder Level" Width="100"></asp:TextBox><asp:Label ID="lblLink" runat="server" Text=""></asp:Label>&nbsp;<asp:CheckBox ID="chkItemHidden" runat="server" />Inactive</td>
            </tr>
            <tr>
                <td></td>
                <td>
                    <asp:DropDownList ID="ddToAdjust" runat="server">
                        <asp:ListItem Value="">-Select-</asp:ListItem>
                        <asp:ListItem Value="1">Adjust</asp:ListItem>
                        <asp:ListItem Value="2">Bill</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td></td>
                <td>
                    <br />
                    <asp:Button ID="btnSave" CssClass="btn btn-sm btn-primary" runat="server" Text="Save" OnClick="btnSave_Click" />
                    <a href="ItemEdit.aspx" class="btn btn-sm btn-success">New </a>
                    <br />
                    <br />
                    <asp:Label ID="lblStatus" runat="server"></asp:Label>
                </td>
            </tr>
            <tr style="background-color: #E0DFDC;">
                <td colspan="2">
                    <asp:Label ID="lblStore" runat="server" Text=""></asp:Label></td>
            </tr>
            <tr>
                <td>Rate</td>
                <td>
                    <asp:TextBox ID="txtRateStore" runat="server" placeholder="Rate" Width="100"></asp:TextBox></td>
            </tr>
            <tr>
                <td>MRP</td>
                <td>
                    <asp:TextBox ID="txtMRPStore" runat="server" placeholder="MRP" Width="100"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td></td>
                <td>
                    <asp:Button ID="btnItemStoreSave" runat="server" Text="Save" CssClass="btn btn-sm btn-warning" OnClick="Button1_Click" />
                </td>
            </tr>
        </table>
    </form>
    <br />
    <asp:Literal ID="ltImageSearch" runat="server"></asp:Literal><br />
    <asp:Literal ID="ltImage" runat="server"></asp:Literal>
</body>
</html>
