<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ItemPrint.aspx.cs" Inherits="ItemPrint" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Invoice Print</title>
    <style type="text/css">
        body
        {
        }
        .tdtopalign
        {
            vertical-align: top;
        }

        .bold
        {
            font-weight: bold;
        }

        .headingtext
        {
            font-size: 18px;
            font-weight: bold;
        }

        table.gridtable
        {
            font-family: verdana,arial,sans-serif;
            font-size: 11px;
            color: #333333;
            border-width: 1px;
            border-color: #666666;
            border-collapse: collapse;
        }

            table.gridtable th
            {
                padding: 4px;
                background-color: #dedede;
                border: 1px solid #666666;
            }

            table.gridtable td
            {
                padding: 4px;
                background-color: #ffffff;
                border: 1px solid #666666;
            }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table style="border: 1px solid black;">
                <tr>
                    <td>
                        <table style="width: 800px;">
                            <tr>
                                <td class="tdtopalign bold">TIN:09265701019
                                    <br />
                                    CST No:ND5097146&nbsp;Dt.08.08.99</td>
                                <td class="tdtopalign bold" style="text-align: center;"><span>TAX INVOICE</span>
                                    <br />
                                    <span class="headingtext">PURE PROTEIN PVT LTD</span>
                                    <br />
                                    F-1 ATS ONE HAMLET,SEC-104,NOIDA
                                    <br />
                                    FSSAI LICENCE NO-121405000136
                                    <br />
                                    Tel:0120-433361
                                    <br />
                                    Email:pureprotein@gmail.com
                                </td>
                                <td class="tdtopalign">
                                    <i>Original Copy</i>
                                    <br />
                                    Pre authenticated
                                    <br />
                                    for PURE PROTEIN PVT LTD
                                    <br />
                                    <br />
                                    <br />
                                    Authorised Signatory
                                </td>
                            </tr>
                        </table>
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td>
                        <table style="width: 800px;">
                            <tr>
                                <td class="tdtopalign">
                                    <span class="bold">M/S. <asp:Label ID="lblClientName" runat="server" Text=""></asp:Label></span>
                                    <br />
                                    <asp:Label ID="lblAddress" runat="server" Text=""></asp:Label>
                                    <br />
                                    Party Phone: <asp:Label ID="lblClientPhone" runat="server" Text=""></asp:Label>
                                    <br />
                                    Party TIN:<asp:Label ID="lblClientTIN" runat="server" Text=""></asp:Label>
                                </td>
                                <td class="tdtopalign"><span class="bold">Invoice No:<asp:Label ID="lblInvoiceNo" runat="server" Text=""></asp:Label>&nbsp;&nbsp;&nbsp;Book No:<asp:Label ID="lblBookNo" runat="server" Text=""></asp:Label></span>
                                    <br />
                                    <span class="bold">Dated:<asp:Label ID="lblDate" runat="server" Text="Label"></asp:Label></span>
                                    <hr />
                                    P.O. NO:<asp:Label ID="lblPoNo" runat="server" Text=""></asp:Label>
                                    <br />
                                    Transport:<asp:Label ID="lblTransport" runat="server" Text=""></asp:Label>
                                    <br />
                                    Vehicle No:<asp:Label ID="lblVehicleNo" runat="server" Text=""></asp:Label>
                                    <br />
                                    Station:<asp:Label ID="lblStation" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Label ID="lblItemTable" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table style="width: 100%;">
                            <tr>
                                <td><strong>Terms & Conditions</strong><br />
                                    <ol style="padding-left: 1em;">
                                        <li>Goods once sold will not be taken back or exchanged.
                                        </li>
                                        <li>Interest @18% p.a. will be charged on over due bills.
                                        </li>

                                        <li>Any descrepancy must be intimated within a week from the date of receipt of goods.
                                        </li>
                                        <li>Goods forwarded according to our term& conditions&at buyers risk.
                                        </li>
                                    </ol>
                                </td>
                                <td>for PURE PROTEIN PVT LTD
                                    <br />
                                    <br />
                                    <br />
                                    Authorised signatory
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
