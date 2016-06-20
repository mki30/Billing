using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BillingLib;
using System.Text;

public partial class Report : BasePage
{
    protected new void Page_Load(object sender, EventArgs e)
    {
        CurrentNav = "navDailySale";
        if (!SessionState.IsAdmin)
            Response.Redirect("/Login.aspx");

        Title = "Report";
        base.Page_Load(sender, e);

        ddStore.Visible = false;
        Label masterlbl = (Label)Master.FindControl("lblContentText");

        if (!IsPostBack)
        {
            Common.FillBrandDropdown(ddBrand);
            string Brand = QueryString("brandid");
            if (Brand != "") ddBrand.SelectedValue = Brand;
            Common.FillTaxDropDown(ddTax);
            if (QueryString("tax") != "") ddTax.SelectedValue = QueryString("tax");
            Common.FillVendorDropdown(ddVendor);
            string VendorID = QueryString("vendorid");
            if (VendorID != "") ddVendor.SelectedValue = VendorID;
            Common.FillEmployeeDropdown(ddEmployee, SessionState.StoreID);
            string EmployeeID = QueryString("employeeid");
            if (EmployeeID != "") ddEmployee.SelectedValue = EmployeeID;
            Common.FillStoreDropdown(ddStore, true);

            ddStore.SelectedValue = SessionState.IsAdmin ? StoreID.ToString() : SessionState.StoreID.ToString();
            ltDateFilter.Text = GetDateFilterLinks(Cmn.ToInt(QueryString("type")));
            ShowReport();
        }
    }

    void ShowReport()
    {
        switch (reportType)
        {
            case ReportType.DailySale:
                ShowDailySale();
                Title = "Daily Sale";
                CurrentNav = "navDailySale";

                ddStore.Visible = true;
                break;
            case ReportType.ItemSale:
                Title = "Item Sale";
                ddBrand.Visible = true;
                ddTax.Visible = true;
                string TaxTable = "", TaxTableAdjusted = "";
                double AdjustableCostVat = 0, AdjustablePriceVat = 0;
                string tabAdjusted = ShowItemSale(FilterDateFrom, FilterDateTo, Cmn.ToDbl(ddTax.SelectedValue), ItemID, ref  TaxTableAdjusted, ref AdjustableCostVat, ref AdjustablePriceVat, true);
                if (SessionState.IsAdminRRA)
                    ShowItemSale(FilterDateFrom, FilterDateTo, Cmn.ToDbl(ddTax.SelectedValue), ItemID, ref TaxTable, ref AdjustableCostVat, ref AdjustablePriceVat, false);
                txtAmountToReduce.Text = (AdjustablePriceVat - AdjustableCostVat).ToString("0");
                txtPriveVatReducable.Text = AdjustablePriceVat.ToString("0");
                ltData.Text = "<table><tr><td>" + TaxTableAdjusted + "<td>" + TaxTable + "</table>" + tabAdjusted;
                CurrentNav = "navItemSale";
                break;
            case ReportType.ItemPurchased:
                ddTax.Visible = true;
                ddVendor.Visible = true;
                ddBrand.Visible = true;
                Title = "Item Purchased";
                CurrentNav = "navItemPurchased";
                ShowPurchase();
                break;
            case ReportType.ItemSalePurchase:
                ddTax.Visible = false;
                ddVendor.Visible = false;
                Title = "Item Sale Purchased";
                CurrentNav = "navItemSalePurchased";
                ShowSalePurchase();
                break;
            case ReportType.Attendance: ShowAttendance(); Title = "Attendance"; CurrentNav = "navAttendance"; ddEmployee.Visible = true; break;
            case ReportType.PriceMismach: ShowPriceMismach(); Title = "Price Mismach"; CurrentNav = "navPriceMismach"; break;
            default: ShowDailySale(); break;
        }
    }

    private void ShowSalePurchase()
    {
        if (ItemID == 0)
        {
            ltData.Text = "<h3>Please select an item for the report<span class='h4 pull-right'>Item Sale Purchased</span></h3><script>$('#txtItemSearch').focus()</script>";
            return;
        }

        Item item = SessionState.Company.ItemList.FirstOrDefault(m => m.ID == ItemID);
        if (item == null)
        {
            ltData.Text = "Item not found";
            return;
        }

        List<BillingLib.BillingItem> billingItem = BillingLib.BillingItem.GetAll(FilterDateFrom, FilterDateTo, 0, ItemID, SessionState.StoreID);
        List<BillingLib.Inventory> listInventory = BillingLib.Inventory.GetInventory(SessionState.StoreID, ItemID, FilterDateFrom, FilterDateTo);

        List<BillingItemSalePurchaseReport> itemList = new List<BillingItemSalePurchaseReport>();
        foreach (BillingItem bi in billingItem)
        {
            itemList.Add(new BillingItemSalePurchaseReport()
            {
                RecordType = RecordType.Sale,
                item = new Item() { Quantity = bi.Quantity, Cost = bi.Cost, MRP = bi.MRP },
                date = bi.SaleDate
            });
        }

        foreach (BillingLib.Inventory bi in listInventory)
        {
            itemList.Add(new BillingItemSalePurchaseReport()
            {
                RecordType = bi.RecordType,
                item = new Item() { Quantity = bi.Quantity, Cost = bi.Cost, MRP = bi.MRP },
                date = bi.Date,
                StoreID = bi.StoreID,
                //FromstoreID = bi.FromStoreID
            });
        }

        //first loop
        double Stock = 0;
        foreach (BillingItemSalePurchaseReport i in itemList.OrderBy(m => m.date))
        {
            switch (i.RecordType)
            {
                case RecordType.StockManualEntry:
                    Stock = i.item.Quantity;
                    break;
                case RecordType.Purchase:
                    Stock += i.item.Quantity;
                    break;
                case RecordType.Transfer:
                    if (i.StoreID == SessionState.StoreID)
                        Stock += i.item.Quantity;
                    else
                        Stock -= i.item.Quantity;
                    break;
                case RecordType.Damage:
                case RecordType.Return:
                    Stock -= i.item.Quantity;
                    break;
                default:
                    Stock -= i.item.Quantity; break;
            }
            i.Stock = Stock;
        }

        double marginTotal = 0, purchaseTotal = 0, saleTotal = 0;

        StringBuilder strHead = new StringBuilder("<h3>" + item.Name + "-" + item.PLU + "</h3><table class='table table-condensed table-bordered table-striped table-hover datatable'><tr><th>#<th>Type<th>Date<th>Cost<th>MRP<th>Qty<th>Purchase<th>Sale<th>Margin<th>Stock");
        StringBuilder str = new StringBuilder();
        int ctr = 0;
        foreach (BillingItemSalePurchaseReport i in itemList.OrderByDescending(m => m.date))
        {
            string clr = "";
            string inOutText = "";
            double margin = 0;
            double sale = 0;
            double purchase = 0;
            switch (i.RecordType)
            {
                case RecordType.StockManualEntry: clr = "red"; break;
                case RecordType.Purchase: clr = "MEDIUMSEAGREEN"; purchaseTotal += purchase = (i.item.Cost * i.item.Quantity); break;
                case RecordType.Transfer: if (i.StoreID == SessionState.StoreID)
                    { clr = "GOLD"; inOutText = "In"; }
                    else
                    { clr = "CORNFLOWERBLUE"; inOutText = "Out"; }; break;
                case RecordType.Damage: clr = "TOMATO"; break;
                case RecordType.Return: clr = "HOTPINK"; break;
                case RecordType.Sale: margin = (i.item.MRP * i.item.Quantity - i.item.Cost * i.item.Quantity); marginTotal += margin; saleTotal += sale = i.item.MRP * i.item.Quantity; break;
            }
            str.Append("<tr style='background:" + clr + "'><td>" + ++ctr + "<td>" + i.RecordType + " " + inOutText + "<td>" + i.date.ToString("%d-MMM-yy HH.mm") + "<td>" + i.item.Cost + "<td>"
                + i.item.MRP + "<td>" + i.item.Quantity + "<td>" + (purchase == 0 ? "" : purchase.ToString("0.0")) + "<td>" + (sale == 0 ? "" : sale.ToString("0.0")) + "<td>"
                + (margin != 0 ? margin.ToString("0.00") : "") + "<td>" + i.Stock.ToString("0.000"));
        }
        strHead.Append("<tr><th><th><th><th><th><th><th>" + purchaseTotal.ToString("0") + "<th>" + saleTotal.ToString("0") + "<th>" + marginTotal.ToString("0") + "<th>");
        ltData.Text = strHead.Append(str + "</table>").ToString();
    }
    private void ShowAttendance()
    {
        string empid = QueryString("EmployeeID");
        if (empid != "")
            ddEmployee.SelectedValue = empid;
        List<EmployeeAttendance> list = EmployeeAttendance.GetAll(FilterDateFrom, FilterDateTo, Cmn.ToInt(ddEmployee.SelectedValue), SessionState.StoreID);

        StringBuilder str = new StringBuilder("<table class='table table-condensed table-bordered table-striped table-hover'><tr><th  style='width:120px'>Date<th>In<th>Out");
        foreach (EmployeeAttendance ea in list)
        {
            str.Append("<tr><td>" + ea.Date.ToString("%d-MMM-yyyy") + "<td>" + ea.DateIn.ToString("hh:mm tt") + "<td>" + ea.DateOut.ToString("hh:mm tt"));
        }
        str.Append("</table>");
        ltData.Text = str.ToString();
    }

    private void ShowDailySale()
    {
        int StoreID = Cmn.ToInt(ddStore.SelectedValue);
        List<BillingLib.ExpenseLog> expList = BillingLib.ExpenseLog.GetByDate(FilterDateFrom, FilterDateTo, StoreID);
        List<ReportDailySale> listDailySale = ReportDailySale.GetAll(0, 0, FilterDateFrom, FilterDateTo);
        Dictionary<DateTime, ReportDailySale> dataList = new Dictionary<DateTime, ReportDailySale>();
        List<BillingLib.Billing> listTotalBilling = BillingLib.Billing.GetAll(false, FilterDateFrom, FilterDateTo, "", SessionState.CompanyID, 0, 0, false);

        DateTime fromDate = FilterDateFrom;
        DateTime toDate = FilterDateTo;
        double MaxAmount = 0, MaxCash = 0, MaxCard = 0;
        int totalDays = (toDate - fromDate).Days + 1;

        for (DateTime dt = fromDate; dt <= toDate; dt += TimeSpan.FromDays(1))
        {
            ReportDailySale ds = listDailySale.FirstOrDefault(m => m.Date == dt);

            if (dt >= DateTime.Today.AddDays(-2) || ds == null || fromDate.Date == toDate.Date || UpdateAll) // always update records for the last 2 days  or report is for one day only
            {
                //List<BillingLib.Billing> listBilling = BillingLib.Billing.GetAll(dt, dt, "", SessionState.CompanyID); 

                List<BillingLib.Billing> listBilling = listTotalBilling.Where(m => m.BillDate >= dt && m.BillDate < dt.AddDays(1)).ToList();

                foreach (BillingLib.Store store in Global.StoreList.Values.Where(m => m.CompanyID == SessionState.CompanyID))
                {
                    ds = listDailySale.FirstOrDefault(m => m.StoreID == store.ID && m.Date == dt);
                    if (ds == null)
                        ds = new ReportDailySale()
                        {
                            StoreID = store.ID,
                            Date = dt,
                        };

                    ds.Amount = listBilling.Where(m => m.store.ID == store.ID).Sum(m => m.PaidCash);
                    ds.AmountCard = listBilling.Where(m => m.store.ID == store.ID).Sum(m => m.PaidCard);

                    List<BillingLib.BillingItem> BIList = BillingLib.BillingItem.GetAll(FilterDateFrom, FilterDateTo, "", store.ID).Where(m => m.SaleDate >= dt && m.SaleDate < dt.AddDays(1)).ToList();
                    //double Cost = BIList.Sum(m => (m.Cost * m.Quantity) - m.DiscountAmount);
                    //ds.Margin = Math.Round(ds.Amount + ds.AmountCard - Cost);

                    double marg = 0, vat = 0;
                    foreach (BillingLib.BillingItem bi in BIList)
                    {
                        double cost = bi.Cost * bi.Quantity;
                        double saleMRP = bi.MRP * bi.Quantity;
                        double saleAmount = bi.MRP * bi.Quantity - bi.DiscountAmount;
                        double salePrice = saleAmount / (1 + bi.TaxRate / 100);
                        double saleVat = bi.TaxRate != 0 ? saleAmount - salePrice : 0;

                        double margin = saleAmount - saleVat - cost;
                        marg += margin;
                        vat += saleVat;
                    }

                    ds.Margin = marg;
                    ds.Vat = vat;
                    if (ds.Amount + ds.AmountCard != 0)
                        ds.Save();

                    // add or update the record in the list
                    ReportDailySale d = listDailySale.FirstOrDefault(m => m.Date.Date == dt.Date && m.StoreID == store.ID);
                    if (d != null)
                        listDailySale.Remove(d);

                    listDailySale.Add(ds);
                }
            }

            List<ReportDailySale> dsList = listDailySale.Where(m => m.Date.Date == dt.Date && (StoreID == 0 || m.StoreID == StoreID)).ToList();
            if (dsList.Count > 0)
            {
                double AmtCash = dsList.Sum(m => m.Amount);
                double AmtCard = dsList.Sum(m => m.AmountCard);
                double AmtMargin = dsList.Sum(m => m.Margin);
                double AmtVAT = dsList.Sum(m => m.Vat);

                if (AmtCash > MaxCash)
                    MaxCash = AmtCash;

                if (AmtCard > MaxCard)
                    MaxCard = AmtCard;

                if (AmtCash + AmtCard > MaxAmount)
                    MaxAmount = AmtCash + AmtCard;

                dataList.Add(dt, new ReportDailySale() { Date = dt, Amount = AmtCash, AmountCard = AmtCard, Margin = AmtMargin, Vat = AmtVAT });
            }
        }

        string header = "<div class='text-center'><a href='#' onclick='DownloadFile(\"DailySale.csv\")' class='pull-left'>Download&nbsp;<i class='glyphicon glyphicon-download-alt'></i></a><h4>" + ddStore.SelectedItem.Text + " Daily Sale</h4></div>" +
        "<table class='table table-condensed table-bordered table-striped table-hover csvTable' style='white-space:nowrap'>"
        + "<tr><th colspan='7' style='display:none'>Daily sale from:" + FilterDateFrom.ToString("%d-MMM-yy") + " to:" + FilterDateTo.ToString("%d-MMM-yy")
        + "<tr><th  style='width:120px'>Date<th style='width:50%'><th class='alnright'>Cash<th class='alnright'>Card<th class='alnright'>Total<th class='alnright' Title='Expense'>Exp.<th class='alnright'>Balance<th class='alnright' Title='MRP-SaleVat-Cost'>Margin<th class='alnright'>Sale VAT";
        double monthlySum = 0, monthlySumCash = 0, monthlySumCard = 0, monthlyExpense = 0, monthlyBalance = 0, vatTotal = 0, marginTotal = 0, netmarginTotal = 0;
        MaxAmount *= 1.2;
        StringBuilder str = new StringBuilder();

        foreach (KeyValuePair<DateTime, ReportDailySale> kvp in dataList.OrderByDescending(m => m.Key))
        {
            double dailyExpense = expList.Where(m => m.Date == kvp.Key).Sum(m => m.Amount);
            monthlySum += kvp.Value.Amount + kvp.Value.AmountCard;
            monthlySumCash += kvp.Value.Amount;
            monthlySumCard += kvp.Value.AmountCard;
            monthlyExpense += dailyExpense;
            double totalDailySale = kvp.Value.Amount + kvp.Value.AmountCard;
            double dailyBalance = totalDailySale - dailyExpense;
            monthlyBalance += dailyBalance;
            marginTotal += kvp.Value.Margin;

            double netMargin = (kvp.Value.Margin - dailyExpense);
            netMargin = netMargin > 0 ? netMargin : 0;
            netmarginTotal += netMargin;
            vatTotal += kvp.Value.Vat;

            str.Append("<tr><td>" + kvp.Key.ToString("dd-M-yy ddd")
                + "<td><div class='progress-bar progress-bar-info' style='width:" + (int)(kvp.Value.Amount / MaxAmount * 100) + "%' title='" + kvp.Value.Amount + "'>&nbsp;</div>"
                + "<div class='progress-bar progress-bar-alert' style='width:" + (int)(kvp.Value.AmountCard / MaxCash * 100) + "%' title='" + kvp.Value.AmountCard + "'>&nbsp;</div>"
                + "<td class='alnright'>" + Cmn.toCurrency(kvp.Value.Amount)
                + "<td class='alnright'>" + Cmn.toCurrency(kvp.Value.AmountCard)
                + "<td class='alnright'>" + Cmn.toCurrency(kvp.Value.Amount + kvp.Value.AmountCard)
                + "<td class='alnright'><a target='_blank' href='/ExpenseLog.aspx?reporttype=0&filterdatefrom=" + FilterDateFrom.ToString("%d-MMM-yyyy") + "&filterdateto=" + FilterDateTo.ToString("%d-MMM-yyyy") + "'>" + Cmn.toCurrency(dailyExpense) + "</a></td>"
                + "<td class='alnright'>" + Cmn.toCurrency(dailyBalance)
                + "<td class='alnright'>" + Cmn.toCurrency(kvp.Value.Margin)
                + "<td class='alnright'>" + Cmn.toCurrency(kvp.Value.Vat)
                + "</tr>"
                );
        }
        header += "<tr><th><th>TOTAL</br>AVG<th class='alnright'>" + Cmn.toCurrency(monthlySumCash) + "<br/>" + Cmn.toCurrency(monthlySumCash / totalDays)
            + "<th class='alnright'>" + Cmn.toCurrency(monthlySumCard) + "<br/>" + Cmn.toCurrency(monthlySumCard / totalDays)
            + "<th class='alnright'>" + Cmn.toCurrency(monthlySum) + "<br/>" + Cmn.toCurrency(monthlySum / totalDays)
            + "<th class='alnright'>" + Cmn.toCurrency(monthlyExpense) + "<br/>" + Cmn.toCurrency(monthlyExpense / totalDays)
            + "<th class='alnright'>" + Cmn.toCurrency(monthlyBalance) + "<br/>" + Cmn.toCurrency(monthlyBalance / totalDays)
            + "<th class='alnright'>" + Cmn.toCurrency(marginTotal) + "<br/>" + Cmn.toCurrency(marginTotal / totalDays)
            + "<th class='alnright'>" + Cmn.toCurrency(vatTotal) + "<br/>" + Cmn.toCurrency(vatTotal / totalDays);

        ltData.Text = header + str.ToString() + "</table><script>$('#txtItemSearch').hide()</script>";
    }

    //ShowAdjusted - will include the bills which have been created in the virtual terminal
    private string ShowItemSale(DateTime FilterDateFrom, DateTime FilterDateTo, double Tax, int ItemID, ref string TaxInfo, ref double AdjustableCostVat, ref double AdjustablePriceVat, Boolean ShowAdjusted = false)
    {
        AdjustableCostVat = 0;
        AdjustablePriceVat = 0;
        if (SessionState.IsAdminRRA)
        {
            btnUpdateHiddenIn.Visible = true;
            txtPriveVatReducable.Visible = true;
            txtAmountToReduce.Visible = true;
        }
        Dictionary<double, List<double>> TaxTotals = new Dictionary<double, List<double>>();

        // this list includes all the bills from normal and virtual terminals
        List<BillingLib.Billing> listBills = BillingLib.Billing.GetAll(false, FilterDateFrom, FilterDateTo, "", 0, SessionState.StoreID, 0, false);

        // if actual need to be shown then remove the veitual terminal
        List<BillingLib.Terminal> termList = BillingLib.Terminal.GetByStoreID(SessionState.StoreID, true); //Get virtual terminal
        if (ShowAdjusted == false && termList.Count > 0) // if ShowAdjusted  is false then do not include the vertual terminal billing items in the report
            listBills = listBills.Where(m => m.TerminalID != termList[0].ID).ToList();

        string IDs = "-1";
        foreach (BillingLib.Billing b in listBills)
            IDs += "," + b.ID;

        List<BillingLib.BillingItem> listBillItems = BillingLib.BillingItem.GetAll(IDs, Tax, ItemID);
        if (BrandID != 0) //filter by brand
        {
            var idList = SessionState.Company.ItemList.Where(m => m.BrandID == BrandID && BrandID != 0).Select(m => m.ID).ToList();
            listBillItems = listBillItems.Where(m => idList.Contains(m.ItemID)).ToList();
        }

        // if the report is for not single item then group the item on their cost & MRP to show their transaction details
        List<BillingItemReport> itemList = new List<BillingItemReport>();
        foreach (BillingItem bi in listBillItems)
        {
            if (bi.AdjustedRate == 0)
                bi.AdjustedRate = bi.MRP;

            BillingItemReport bir = null;
            if (ItemID != 0) // if report is for single item only then will show all the transactions of the selected item
            {
                bir = new BillingItemReport() { billItem = bi, CostPerUnit = bi.Cost, MRPPerUnit = bi.MRP, AdjustedMRP = bi.AdjustedRate };
                itemList.Add(bir);
            }
            else
            {
                bir = itemList.FirstOrDefault(m => m.CostPerUnit == bi.Cost && m.MRPPerUnit == bi.MRP);
                if (bir == null)
                {
                    bir = new BillingItemReport() { billItem = bi, CostPerUnit = bi.Cost, MRPPerUnit = bi.MRP, AdjustedMRP = bi.AdjustedRate };
                    itemList.Add(bir);
                }
            }

            bi.UseAdjustedMRP = ShowAdjusted;

            bir.TotalQuantity += bi.Quantity;
            bir.TotalDiscount += bi.DiscountAmount;

            bir.CostPrice += bi.CostPrice;
            bir.CostVat += bi.CostVat;

            bir.SalePrice += bi.SalePrice;
            bir.SaleVat += bi.SaleVat;

            bir.Margin += bi.Margin;

            if (bir.TaxRate < bi.TaxRate)
                bir.TaxRate = bi.TaxRate;

            if (!TaxTotals.ContainsKey(bi.TaxRate))
            {
                TaxTotals.Add(bi.TaxRate, new List<double>());
                for (int i = 0; i < 5; i++)
                    TaxTotals[bi.TaxRate].Add(0);
            }
        }

        string header = "<div class='text-center'><a href='#' onclick='DownloadFile(\"ItemSale.csv\",[1])' class='pull-left'>Download&nbsp;<i class='glyphicon glyphicon-download-alt'></i></a><span class='h4'>Item Sale</span>" +
        "<table id='tblItemSale' class='datatable table-condensed table-striped table-bordered csvTable'>"
        + "<tr><th colspan='19' style='display:'>Item Sale " + GetInfoString()
        + "<tr onclick='selectElementContents(tblItemSale)'><th>SNo<th>PLU" + (ItemID != 0 ? "<th>Bill No" : "")
        + "<th>Date<th>Name<th title='Tax Percentage'>Tax<th title='Quantity'>Qty"
        + "<th>Unit"
        + "<th title='Purchase Cost'>Cost/Unit"
        + "<th>Cost Price"
        + "<th>Cost Vat"
        + "<th title='Cost= Cost Price + Cost Vat'>Cost"
        + "<th>MRP/Unit"
        + "<th>MRP"
        + "<th title='Discount Amount'>Disc."
        + "<th>Sale Price"
        + "<th>Sale Vat"
        + "<th title='Sale Amount = Sale Price + Sale Vat'>Sale Amount"
        + "<th title='Margin = Sale Amount- Sale Vat - Cost'>Margin"
        + "<th title='Margin Percent'>Mar.%";
        StringBuilder str = new StringBuilder();
        int ctr = 0;

        double totalCost = 0;
        double totalCostPrice = 0;
        double totalCostVat = 0;

        double totalMRP = 0;
        double totalDiscount = 0;

        double totalSalePrice = 0;
        double totalSaleVAT = 0;

        double totalSaleAmount = 0;
        double totalMargin = 0;

        List<BillingLib.Item> listItemsToAdjust = BillingLib.Item.GetAdjustedItems();//Get list of item which need adjustment of vat - items marked Adjust=1

        foreach (BillingItemReport BI in itemList.OrderBy(m => m.billItem.Name))
        {
            totalCost += BI.TotalCost;
            totalCostPrice += BI.CostPrice;
            totalCostVat += BI.CostVat;

            totalMRP += BI.TotalSaleAmount;
            totalDiscount += BI.TotalDiscount;
            totalSalePrice += BI.SalePrice;
            totalSaleVAT += BI.SaleVat;
            totalSaleAmount += BI.TotalSaleAmount;
            totalMargin += BI.Margin;

            Item I = SessionState.Company.GetItem(BI.billItem.PLU);
            str.Append("<tr><td>" + (++ctr) + "<td title='ID=" + BI.billItem.ItemID + "'>"
                + BI.billItem.PLU + (ItemID != 0 ? "<td><a href='/Billing.aspx?billno=" + BI.billItem.BillNo + "' target='_blank'>" + BI.billItem.BillNo + "</a>" : "")
                + "<td style='white-space: nowrap'>" + BI.billItem.SaleDate.ToString("%d-MMM-yy HH:mm") + "<td style='text-align:left'>"
                + "<a href='/Report.aspx?reporttype=2&itemid=" + BI.billItem.ItemID + "&filterdatefrom=" + FilterDateFrom.ToString("%d-MMM-yyyy") + "&filterdateto=" + FilterDateTo.ToString("%d-MMM-yyyy") + "&BillID=" + BI.billItem.BillingID + "' target='_blank'>"
                + BI.billItem.Name + "</a>"
                + "<td>" + (BI.TaxRate != 0 ? BI.TaxRate.ToString("0.0") : "")
                + "<td>" + (BI.TotalQuantity == (int)BI.TotalQuantity ? BI.TotalQuantity.ToString() : BI.TotalQuantity.ToString("0.000")) //Qty
                + "<td>" + (I != null ? "" + I.Weight + I.WeightType : "") //Unit
                + "<td>" + BI.CostPerUnit //Cost per units
                + "<td>" + BI.CostPrice.ToString("0.00")
                + "<td>" + (BI.CostVat != 0 ? BI.CostVat.ToString("0.00") : "")
                + "<td><b>" + BI.CostPerUnit.ToString("0.00") //Cost
                + "</b><td>" + BI.AdjustedMRP + (BI.AdjustedMRP != BI.MRPPerUnit ? "/" + Cmn.toCurrency(BI.MRPPerUnit) : "")// MRP per unit
                + "<td>" + Cmn.toCurrency(BI.SalePrice) + (BI.AdjustedMRP != BI.MRPPerUnit ? "/" + Cmn.toCurrency(BI.MRPPerUnit * BI.TotalQuantity) : "")
                + "<td>" + (BI.TotalDiscount != 0 ? BI.TotalDiscount.ToString("0") : "")
                + "<td>" + BI.SalePrice.ToString("0.00")
                + "<td>" + (BI.SaleVat != 0 ? BI.SaleVat.ToString("0.00") : "")
                + "<td><b>" + BI.TotalSaleAmount.ToString("0.00")
                + "</b><td " + (BI.Margin < 0 || BI.CostPerUnit == 0 ? "style='background:pink'" : "") + ">" + BI.Margin.ToString("0.0")
                + "<td> " + (BI.Margin / BI.CostPrice * 100).ToString("0")
                );

            TaxTotals[BI.TaxRate][0] += BI.CostPrice;
            TaxTotals[BI.TaxRate][1] += BI.CostVat;
            TaxTotals[BI.TaxRate][2] += BI.SalePrice;
            TaxTotals[BI.TaxRate][3] += BI.SaleVat;
            TaxTotals[BI.TaxRate][4] += BI.TotalSaleAmount;

            Item i = listItemsToAdjust.FirstOrDefault(m => m.ID == I.ID);
            if (i != null)
            {
                AdjustableCostVat += BI.CostVat;
                AdjustablePriceVat += BI.SaleVat;
            }
        }

        header += "<tr><th><th><th><th>" + (ItemID != 0 ? "<th>" : "")
        + "Total<th><th><th><th>"
        + "<th>" + Cmn.toCurrency(totalCostPrice)
        + "<th>" + Cmn.toCurrency(totalCostVat)
        + "<th>" + Cmn.toCurrency(totalCost)
        + "<th><th>" + Cmn.toCurrency(totalMRP)
        + "<th>" + Cmn.toCurrency(totalDiscount)
        + "<th>" + Cmn.toCurrency(totalSalePrice)
        + "<th>" + Cmn.toCurrency(totalSaleVAT)
        + "<th>" + Cmn.toCurrency(totalSaleAmount)
        + "<th>" + Cmn.toCurrency(totalMargin)
        + "<th class='alnright'>" + Cmn.toCurrency(totalMargin / totalCost * 100);

        TaxInfo = "<table id='tblVatSummary' style='width:auto;text-align:right' class='table table-condensed table-striped table-bordered'><caption>Vat Summary" + (ShowAdjusted ? "" : " Not Adjusted") + "</caption><tr onclick='selectElementContents(tblVatSummary)'>"
        + "<th>VAT Rate"
        + "<th>Cost Price"
        + "<th>Cost Vat"
        + "<th>Sale Price"
        + "<th>Sale Vat"
        + "<th>Sale Amount";

        foreach (KeyValuePair<double, List<double>> kvp in TaxTotals.OrderBy(m => m.Key))
        {
            TaxInfo += "<tr><td>" + kvp.Key + "%";
            for (int i = 0; i < 5; i++)
                TaxInfo += "<td>" + Cmn.toCurrency(kvp.Value[i]);
        }
        TaxInfo += "<tr><th>Total";

        totalCostVat = 0;
        for (int i = 0; i < 5; i++)
        {
            double total = 0;
            foreach (KeyValuePair<double, List<double>> kvp in TaxTotals.OrderBy(m => m.Key))
                total += kvp.Value[i];
            TaxInfo += "<td><b>" + Cmn.toCurrency(total) + "</b>";
            switch (i)
            {
                case 1: totalCostVat = total; break;
                case 3: TaxInfo += "<br/>" + Cmn.toCurrency(total - totalCostVat); break;
            }
        }
        TaxInfo += "</table>";
        if (!ShowAdjusted)
            TaxInfo += "Adjustable Item : Cost Vat =" + Cmn.toCurrency(AdjustableCostVat) + ", Price Vat =" + Cmn.toCurrency(AdjustablePriceVat);

        // if the filter dates are first and last days of the month
        // Save the total sale and total margin for the selected store
        if (FilterDateFrom.Day == 1 && FilterDateTo.Date == new DateTime(FilterDateFrom.Year, FilterDateFrom.Month, 1).AddMonths(1).AddDays(-1).Date)
        {
            MonthlyTotal mMonth = MonthlyTotal.GetByDateTypeAndStore(FilterDateFrom, AmountType.Margin, SessionState.StoreID);
            mMonth.Amount = totalMargin;
            //mMonth.CostVat = totalCostVat;
            //mMonth.PriceVat = totalSaleVAT;
            mMonth.Save();
        }
        return header + str.Append("</table>").ToString();
    }

    private double GetMarginTotal(DateTime FilterDateFrom, DateTime FilterDateTo, double Tax, List<Item> listItemsToAdjust)
    {
        List<BillingLib.Billing> billingList = BillingLib.Billing.GetAll(false, FilterDateFrom, FilterDateTo, "", 0, SessionState.StoreID, 0, false).Where(m => m.TerminalID != 4).ToList(); // do not include virtual terminal bills

        string IDs = "-1";
        foreach (BillingLib.Billing b in billingList)
            IDs += "," + b.ID;

        // get all the billing items of the above billing list
        List<BillingLib.BillingItem> billingItemTemp = BillingLib.BillingItem.GetAll(IDs, Tax, ItemID);

        // get all the items for which sales adjustment need to be done.
        List<BillingLib.BillingItem> billingItem = new List<BillingItem>();
        foreach (BillingLib.BillingItem bi in billingItemTemp)
        {
            BillingLib.Item i = listItemsToAdjust.FirstOrDefault(m => m.ID == bi.ItemID);
            if (i != null)
                billingItem.Add(bi);
        }
        double Margin = 0;
        foreach (BillingItem bi in billingItem)
        {
            Margin += bi.MRP * bi.Quantity - bi.Cost * bi.Quantity;
        }
        return Margin;
    }

    private void ShowPurchase()
    {
        string itemIdsSelectedBrand = "";
        List<BillingLib.Item> itemlistSelectedBrand = SessionState.Company.ItemList.Where(m => m.BrandID == BrandID && BrandID != 0).ToList();
        foreach (BillingLib.Item i in itemlistSelectedBrand)
        {
            itemIdsSelectedBrand += i.ID + ",";
        }
        itemIdsSelectedBrand = itemIdsSelectedBrand.TrimEnd(',');

        List<BillingLib.Inventory> listInventory = BillingLib.Inventory.GetInventory(Cmn.ToInt(ddVendor.SelectedValue), SessionState.StoreID, RecordType.Purchase, FilterDateFrom, FilterDateTo, ItemID, Cmn.ToDbl(ddTax.SelectedValue), 0, itemIdsSelectedBrand);
        List<BillingLib.Inventory> itemsList = new List<BillingLib.Inventory>();

        Dictionary<double, double> TaxTotals = new Dictionary<double, double>();

        if (listInventory.Count > 0)
        {
            foreach (BillingLib.Inventory i in listInventory)
            {
                BillingLib.Inventory ii = itemsList.FirstOrDefault(m => m.ItemID == i.ItemID && m.Cost == i.Cost && m.MRP == i.MRP);
                if (ii == null || ItemID != 0)
                {
                    ii = new BillingLib.Inventory(i);
                    itemsList.Add(ii);
                }
                double Amount = ((i.Cost * i.Quantity) - ((i.Cost * i.Quantity) * i.Discount / 100));
                ii.Quantity += i.Quantity;
                ii.TotalAmount += Amount;

                if (!TaxTotals.ContainsKey(i.Tax))
                    TaxTotals.Add(i.Tax, 0);
                TaxTotals[i.Tax] += Amount;
            }
        }
        string TaxInfo = "<table id='tblPurchaseVatSummary' class='table-striped table-bordered' ><tr onclick='selectElementContents(tblPurchaseVatSummary)'><th>Vat Type<th>Sale Amount<th>Vat Amount";
        foreach (KeyValuePair<double, double> kvp in TaxTotals.OrderBy(i => i.Key))
        {
            TaxInfo += "<tr><td>" + (kvp.Key != 0 ? "VAT@ " + kvp.Key + "" : "VAT FREE") + "<td  class='alnright'>" + Cmn.toCurrency(kvp.Value) + "<td  class='alnright'>" + Cmn.toCurrency((kvp.Value * kvp.Key) / 100);
        }
        TaxInfo += "</table>";

        string header = "<div class='text-center'><a href='#' onclick='DownloadFile(\"ItemPurchased.csv\")' class='pull-left'>Download&nbsp;<i class='glyphicon glyphicon-download-alt'></i></a><span class='h4'>Item Purchased</span></div>" +
        "<table id='tblPurchaseReport' class='datatable table-condensed table-striped table-bordered csvTable'>" +
        "<tr><th colspan='12' style='display:'>ITEM PURCHASED " + GetInfoString() +
        "<tr onclick='selectElementContents(tblPurchaseReport)'><th>#<th>PLU<th style='width:70px'>Date<th>Vendor<th>Name<th>Tax<th>Quantity<th>Rate<th>Qty X Rate<th>MRP<th>Margin<th>Mar%";
        StringBuilder str = new StringBuilder();
        int ctr = 0;

        double itemCount = 0, totalCost = 0, totalMRP = 0, totalMargin = 0;

        BillingLib.Inventory Previous = null;
        foreach (BillingLib.Inventory item in itemsList.OrderBy(m => m.Name))
        {
            double margin = (item.MRP * item.Quantity - item.TotalAmount);

            Boolean ShowName = Previous == null || Previous.ItemID != item.ItemID;
            if (ShowName)
            {
                BillingLib.Vendor ven = SessionState.Company.vendorList.FirstOrDefault(m => m.ID == item.VendorID);
                str.Append("<tr><td>" + ++ctr + "<td>"
                   + item.PLU + "<td class='alnright'>"
                   + item.Date.ToString("%d-M-yy") + "<td style='text-align:left'><div style='height:1.1em;overflow:hidden'>"
                   + (ven != null ? ven.Name : "")
                   + "</div><td style='text-align:left'><a onclick='ShowItemDetail(" + item.ItemID + ",0)' style='cursor:pointer;'>" + item.Name + "</a><td class='alnright'>");
            }
            else
            {
                str.Append("<tr><td colspan='3'>");
            }
            str.Append((item.Tax != 0 ? item.Tax.ToString() : "") + "</a><td class='alnright'>"
                + item.Quantity + "<td class='alnright'>"
                + item.Cost.ToString("0") + "<td class='alnright'>"
                + item.TotalAmount.ToString("0") + "<td class='alnright'>"
                + item.MRP.ToString("0") + "<td class='alnright'>"
                + margin.ToString("0")
                + "<td>" + (margin / item.TotalAmount * 100).ToString("0") + "%");

            itemCount += item.Quantity;
            totalCost += item.TotalAmount;
            totalMRP += item.MRP;
            totalMargin += margin;


        }
        header += "<tr><th><th><th><th><th>Total<th><th  class='alnright'>" + itemCount.ToString("0.0") + "<th><th  class='alnright'>" + Cmn.toCurrency(totalCost) + "<th  class='alnright'>" + Cmn.toCurrency(totalMRP) + "<th  class='alnright'>" + Cmn.toCurrency(totalMargin) + "<td>";
        str.Append("</table><br/>");
        ltData.Text = header + str + TaxInfo;
    }

    private void ShowItemWise()
    {
        List<BillingLib.Billing> listBilling = BillingLib.Billing.GetByDate(FilterDateFrom, FilterDateTo);
        List<BillingLib.BillingItem> billingItem = BillingLib.BillingItem.GetAll();

        Dictionary<int, Item> itemList = new Dictionary<int, Item>();
        foreach (BillingLib.Billing b in listBilling)
        {
            List<BillingLib.BillingItem> list = billingItem.Where(m => m.BillingID == b.ID && m.ItemID == ItemID).ToList();
            foreach (BillingLib.BillingItem bi in list)
            {
                if (!itemList.ContainsKey(bi.ItemID))
                    itemList.Add(bi.ItemID, bi);
                else
                {
                    itemList[bi.ItemID].Quantity += bi.Quantity;
                    itemList[bi.ItemID].MRP += bi.MRP;
                    itemList[bi.ItemID].Cost += bi.Cost;

                }
            }
        }

        StringBuilder str = new StringBuilder("<a href='#' onclick='DownloadFile(\"ItemViseSale.csv\")'>Download&nbsp;<i class='glyphicon glyphicon-download-alt'></i></a><table class='table-condensed table-striped table-bordered csvTable'><tr><th>PLU<th>Name<th>Quantity<th>Cost<th>MRP<th>Margin");
        foreach (KeyValuePair<int, Item> kvp in itemList)
        {
            double margin = kvp.Value.MRP - kvp.Value.Cost;
            str.Append("<tr><td>"
                + kvp.Key + "<td>"
                + "<a href='/MonthlyItemSale.aspx?id=" + kvp.Value.ID + "&fromdate=" + FilterDateFrom.ToString("%d-MMM-yyyy") + "&todate=" + FilterDateTo.ToString("%d-MMM-yyyy") + "' target='_blank'>" + kvp.Value.Name + "</a><td>"
                + kvp.Value.Quantity + "<td class='alnright'>"
                + kvp.Value.Cost.ToString("0.0") + "<td class='alnright'>"
                + kvp.Value.MRP.ToString("0.0") + "<td>"
                + margin);
        }
        ltData.Text = str.Append("</table>").ToString();
    }

    private void ShowPriceMismach()
    {
        List<BillingLib.Inventory> list = BillingLib.Inventory.GetAll(0, RecordType.Purchase).ToList();
        StringBuilder str = new StringBuilder("<div class='text-center'><span class='h4'>Price Mismach</span><div><table class='table table-condensed table-bordered table-striped table-hover'><tr><th>#<th>Item Name<th>Purch. Date<th>Purch. MRP<th>Sale MRP<th>Sale MRP");
        int ctr = 0;
        List<BillingLib.Inventory> invTemp = new List<BillingLib.Inventory>();
        StringBuilder sbLess = new StringBuilder("<tr><th colspan='6' style='text-align:center;'>Sale price less than purchase price");
        StringBuilder sbMore = new StringBuilder("<tr><th colspan='6' style='text-align:center;'>Sale price more than purchase price");
        foreach (Item item in SessionState.Company.ItemList.OrderBy(m => m.Name))
        {
            BillingLib.Inventory inv = list.OrderByDescending(m => m.Date).FirstOrDefault(m => m.ItemID == item.ID);
            if (inv != null && item.MRP != inv.MRP)
            {
                if (invTemp.FirstOrDefault(m => m.ID == inv.ID) == null)
                {
                    string row = "";
                    invTemp.Add(inv);

                    row = "<tr><td>" + ++ctr + "<td style='text-align:left;'><a href='/Inventory.aspx?purchaseId=" + inv.PurchaseID + "&InventoryID=" + inv.ID + "'  target='_blank'>" + inv.Name + "</a><td>" + inv.Date.ToString("dd-MMM-yy") + "<td>"
                        + inv.MRP + "<td " + (item.MRP < inv.MRP ? "style='background:pink'" : "") + " >" + item.MRP
                        + "<td><a href='#' onclick='return UpdateItemPrice(" + item.ID + "," + inv.MRP + ");'>Update</a>";
                    if (item.MRP < inv.MRP)
                        sbLess.Append(row);
                    else
                        sbMore.Append(row);
                }
            }
        }
        ltData.Text = str.ToString() + sbLess + sbMore;

        //Adjusting
    }

    protected void btnUpdateHiddenIn_Click(object sender, EventArgs e)
    {
        List<BillingLib.Terminal> termList = BillingLib.Terminal.GetByStoreID(SessionState.StoreID, true); //Get virtual terminal
        if (termList.Count == 0)
        {
            Response.Write("Adjusting terminal not found");
            return;
        }
        if (FilterDateFrom.Day != 1 || FilterDateTo.Day != FilterDateFrom.AddMonths(1).AddDays(-1).Day)
        {
            Response.Write("Action can be done only for monthy report");
            return;
        }
        double amountToReduce = Cmn.ToDbl(txtAmountToReduce.Text);
        if (amountToReduce == 0)
        {
            Response.Write("Please enter Vat amount to reduce");
            return;
        }
        double reducePercentage = (amountToReduce / Cmn.ToDbl(txtPriveVatReducable.Text)) * 100;

        List<BillingLib.Item> listItemsToAdjust = BillingLib.Item.GetAdjustedItems(); //Get Item list to adjust
        List<BillingLib.Billing> billingList = BillingLib.Billing.GetAll(false, FilterDateFrom, FilterDateTo, "", 0, SessionState.StoreID, 0, false).Where(m => m.TerminalID != termList[0].ID).ToList(); // do not include virtual terminal bills

        string BillingIDs = "-1", ItemIds = "-1";
        foreach (BillingLib.Billing b in billingList)
            BillingIDs += "," + b.ID;

        foreach (BillingLib.Item i in listItemsToAdjust)
            ItemIds += "," + i.ID;

        //the billing items of the above billing list
        List<BillingLib.BillingItem> billingItem = BillingLib.BillingItem.GetAll(BillingIDs, ItemIds);

        //BillingItem.MakeRateAndAdjustedSame(BillingIDs);
        foreach (BillingItem bi in billingItem)
        {
            double Price = bi.MRP / (1 + bi.TaxRate / 100);
            double PriceVat = bi.MRP - Price;
            double costVAT = (bi.Cost * bi.TaxRate) / 100;
            double vatToReduce = (PriceVat - costVAT) * (reducePercentage / 100);
            double vatAfterReduction = PriceVat - vatToReduce;
            double newPrice = vatAfterReduction / (bi.TaxRate / 100);
            double newMRP = newPrice + vatAfterReduction;
            bi.AdjustedRate = Math.Round(newMRP);
        }
        BillingLib.BillingItem.SaveBillingItem(billingItem);

        int billno = 1;
        //delete all bills and billing item of the virtual terminal between the filter dates
        List<BillingLib.Billing> billVertual = BillingLib.Billing.GetByDate(FilterDateFrom, FilterDateTo).Where(m => m.TerminalID == termList[0].ID).ToList();
        if (billVertual.Count > 0)
        {
            foreach (BillingLib.Billing b in billVertual)
            {
                b.Delete();
            }
        }

        //Bill Item against which amount is adjusted-R CH BROILER.
        BillingLib.BillingItem biItemAdjusted = BillingLib.BillingItem.GetFirstBillByItemID(1);
        foreach (DateTime day in EachDay(FilterDateFrom, FilterDateTo))
        {
            List<BillingLib.BillingItem> billingItemListbyDay = billingItem.Where(m => m.SaleDate >= day && m.SaleDate < day.AddDays(1)).ToList();
            if (billingItemListbyDay.Count == 0)
                continue;

            double ActualTotal = billingItemListbyDay.Sum(m => m.Amount);
            double AdjustedTotal = billingItemListbyDay.Sum(m => m.AmountAdjusted);
            double TotalAdjustedAmount = ActualTotal - AdjustedTotal;
            if (TotalAdjustedAmount != 0)
            {
                BillingLib.Billing bill = new BillingLib.Billing();
                bill.TerminalID = termList[0].ID;
                bill.company = SessionState.Company;
                bill.store = Global.StoreList.Values.FirstOrDefault(m => m.ID == termList[0].StoreID);

                //++billno;
                bill.BillNo = Cmn.ToInt(day.ToString("yyMMdd") + (billno++) + termList[0].ID);
                bill.BillDate = day;
                bill.PaidCash = Math.Round(TotalAdjustedAmount, 2);

                if (biItemAdjusted != null)
                {
                    biItemAdjusted.ID = 0;
                    biItemAdjusted.SaleDate = day;
                    biItemAdjusted.Quantity = Math.Round((TotalAdjustedAmount / biItemAdjusted.MRP), 2);
                    bill.ItemList.Add(biItemAdjusted);
                }
                bill.Save();
            }
        }
        ShowReport();
    }

    public IEnumerable<DateTime> EachDay(DateTime from, DateTime thru)
    {
        for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))
            yield return day;
    }
    private string GetInfoString()
    {
        return FilterDateFrom.ToString("%d-MMM-yy") + " to " + FilterDateTo.ToString("%d-MMM-yy") + (ddBrand.SelectedValue != "" ? " Brand: " + ddBrand.SelectedItem.Text : "")
            + (ddTax.SelectedValue != "" ? " Tax: " + ddTax.SelectedItem.Text : "") + (ddVendor.SelectedValue != "" ? " Vendor : " + ddVendor.SelectedItem.Text : "");
    }
}