using BillingLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class StockTrackingForm : BasePage
{
    protected new void Page_Load(object sender, EventArgs e)
    {
        CurrentNav = "navStockTaking";
        base.Page_Load(sender, e);

        if (!IsPostBack)
            Common.FillBrandDropdown(ddBrand);
        int brandID = Request.QueryString["brandid"] != null ? Cmn.ToInt(Request.QueryString["brandid"]) : 0;
        ddBrand.SelectedValue = brandID.ToString();
        divStockTakingDetail.Visible = true;
        ltDateFilter.Text = GetDateFilterLinks(Cmn.ToInt(QueryString("type")), true);
        if (QueryString("monthlyreport") != "1")
        {
            ShowStock();
        }
        else
            ShowMonthlyReport();
    }

    private void ShowMonthlyReport()
    {
        List<BillingLib.Stock> listStock = BillingLib.Stock.GetByMonth(Cmn.FinStartDate, Cmn.FinEndDate, SessionState.StoreID);
        StringBuilder str = new StringBuilder("<table class='datatable  table table-condensed table-bordered table-striped'><th>#<th>Item<th>PLU");

        DateTime dt = Cmn.FinStartDate;
        for (int i = 0; i < 12; i++)
        {
            str.Append("<th>" + dt.ToString("dd-MMM-yy"));
            dt = dt.AddMonths(1);
        }

        List<Item> listItem = Item.GetAll();
        int ctr = 0;
        foreach (Item item in listItem.OrderBy(m => m.Name))
        {
            List<BillingLib.Stock> list = listStock.Where(m => m.ItemID == item.ID).ToList();
            str.Append("<tr><td>" + ++ctr + "<td>" + item.Name + "<td>" + item.PLU);
            dt = Cmn.FinStartDate;
            for (int i = 0; i < 12; i++)
            {
                BillingLib.Stock st = list.FirstOrDefault(m => m.StockMonth == dt);
                str.Append("<td>" + (st != null ? st.StockCount.ToString("0.0") : ""));
                dt = dt.AddMonths(1);
            }
        }
        divData.InnerHtml = str.Append("</table>").ToString();
    }

    private string GetNewFormat(double floatNumber)
    {
        if (floatNumber == 0)
            return "";
        double number = floatNumber - Math.Truncate(floatNumber);
        if (number > 0)
            return floatNumber.ToString("0.00");
        else
            return Math.Round(floatNumber).ToString();
    }

    private void ShowStock()
    {
        DateTime startDate = new DateTime(FilterDateFrom.Year, FilterDateFrom.Month, 1);
        DateTime endDate = new DateTime(startDate.Year, startDate.Month, 1).AddMonths(1).AddDays(-1);
        Boolean UpdateStock = QueryString("updatestock") == "1";

        List<BillingLib.Stock> listStock = BillingLib.Stock.GetCurrentStock(SessionState.Company, SessionState.StoreID, startDate, UpdateStock);
        string header = "<div><table id='DataTable' class='datatable csvTable table-condensed table-striped table-bordered'><tr><th>#<th>Item<th>PLU<th style='white-space:nowrap'>"
            + startDate.ToString("%d-MMM-yy") + "<br/>Opening<th title='Purchase'>Purch<th title='Return'>Ret<th title='Damage'>Dmg<th title='Transfer in'>In<th title='Transfer Out'>Out<th>Sale<th>Stock<th>Cost<th>MRP<th title='Manual Stock' style='white-space:nowrap'>" + endDate.ToString("%d-MMM-yy") + "<br/>Closing";

        List<BillingLib.Item> itemList = BillingLib.Item.GetAll(0, Cmn.ToInt(ddBrand.SelectedValue), false);

        int ctr = 0;
        double totalCost = 0, totalMRP = 0;
        StringBuilder str = new StringBuilder();
        foreach (BillingLib.Item i in itemList.OrderBy(m => m.Name))
        {
            ctr++;
            BillingLib.Stock s = listStock.FirstOrDefault(m => m.ItemID == i.ID);
            if (s == null)
                str.Append("<tr><td>" + ctr + "<td style='text-align:left;'><a href='#' onclick='return GetStock(" + i.ID + "," + i.PLU + ",\"" + i.Name + "\",0,0,\"" + i.WeightTypeString + "\");' title='ID=" + i.ID + "'>" + i.Name + "</a><td>" + i.PLU + "<td><td><td><td><td><td><td><td><td><td><td></tr>");
            else
            {
                string filterdate = "&filterdatefrom=" + startDate.ToString("dd-MMM-yyyy") + "&filterdateto=" + endDate.ToString("dd-MMM-yyyy") + "&itemid=" + s.ItemID + "'>";
                string PurRep = "<a target='_blank' href='/Purchase.aspx?reporttype=";
                str.Append("<tr><td>" + ctr + "<td  style='text-align:left;'><a href='#' onclick='return GetStock(" + i.ID + "," + i.PLU + ",\"" + i.Name + "\"," + s.OpeningStock + "," + s.ClosingStock + ",\"" + i.WeightTypeString + "\");' title='ID=" + i.ID + "'>"
                + (i.Name != "" ? i.Name : "--") + "</a><td>"
                + i.PLU
                + "<td title='" + (s.OpeningStock != 0 ? "Auto " + s.AutoOpeningStock : "Manual " + s.OpeningStock) + " (Updated On:" + s.LastUpdated.ToString("dd-MM-yy") + ")"
                + "' class='" + (s.OpeningStock != 0 ? "auto" : "manual") + "'>"  // s.OpeningStock != 0  Auto
                + (s.OpeningStock != 0 ? GetNewFormat(s.OpeningStock) : (s.AutoOpeningStock != 0 ? GetNewFormat(s.AutoOpeningStock) : ""))
                + "<td>" + (s.Purchase != 0 ? "<a target='_blank' href='/Report.aspx?reporttype=3" + filterdate + GetAmt(s.Purchase) + "</a>" : "")
                + "<td>" + (s.Return != 0 ? PurRep + "1" + filterdate + GetAmt(s.Return) + "</a>" : "")
                + "<td>" + (s.Damage != 0 ? PurRep + "2" + filterdate + GetAmt(s.Damage) + "</a>" : "")
                + "<td>" + (s.TransferIn != 0 ? PurRep + "3" + filterdate + GetAmt(s.TransferIn) + "</a>" : "")
                + "<td>" + (s.TransferOut != 0 ? PurRep + "4" + filterdate + GetAmt(s.TransferOut) + "</a>" : "")
                + "<td>" + (s.Sale != 0 ? "<a target='_blank' href='/Report.aspx?reporttype=2" + filterdate + GetAmt(s.Sale) + "</a>" : "")
                + "<td " + (s.StockCount < 0 ? "style='background-color:#FBB7A8;'>" : ">") + GetNewFormat(s.StockCount)
                + "<td>" + (s.StockCount > 0 ? (i.Cost * s.StockCount).ToString("0.0") : "")
                + "<td>" + (s.StockCount > 0 ? (i.MRP * s.StockCount).ToString("0.0") : "")
                + "<td id='tdQty" + s.ItemID + "' title='" + (s.ClosingStock != 0 ? "Auto " + s.AutoClosingStock : "Manual " + s.ClosingStock) + "' style=" + (s.ClosingStock != 0 ? "background-color:#F3F3A6;" : "background-color:#C1F1C1;") + ">" + (s.ClosingStock != 0 ? GetNewFormat(s.ClosingStock) : (s.AutoClosingStock != 0 ? GetNewFormat(s.AutoClosingStock) : ""))
                + "</tr>");

                totalCost += (s.StockCount > 0 ? (i.Cost * s.StockCount) : 0);
                totalMRP += (s.StockCount > 0 ? (i.MRP * s.StockCount) : 0);
            }
        }
        header += "<tr><th><th>TOTAL<th><th><th><th><th><th><th><th><th><th>" + Cmn.toCurrency(totalCost) + "<th>" + Cmn.toCurrency(totalMRP) + "<th>";
        header += str.ToString() + "</tablle></div>";
        divData.InnerHtml = header;
    }

    string GetAmt(double amt)
    {
        return amt != 0 ? amt.ToString("0.0") : "";
    }
}