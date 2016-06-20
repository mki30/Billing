using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using System.Text;
using BillingLib;

public partial class Budget : BasePage
{
    protected new void Page_Load(object sender, EventArgs e)
    {
        base.Page_Load(sender, e);
        CurrentNav = "navBudget";
        Title = "Budget";

        if (!SessionState.IsAdmin)
            Response.Redirect("/Login.aspx");

        ddStoreBudget.Items.Add(new ListItem("All Stores", "0"));
        foreach (BillingLib.Store s in Global.StoreList.Values.Where(m => m.CompanyID == SessionState.CompanyID))
        {
            ListItem li = new ListItem(s.Name, s.ID.ToString());
            ddStoreBudget.Items.Add(li);
            ddStoreSelect.Items.Add(li);
            if (StoreID == s.ID)
                li.Selected = true;
        }

        DateTime FinancialYear = Cmn.FinStartDate;
        for (int i = FinancialYear.Year; i >= 2015; i--)
        {
            ListItem li = new ListItem(i + "-" + (i + 1).ToString().Substring(2, 2), "filterdatefrom=" + FinancialYear.ToString("%d-MMM-yyyy") + "&filterdateto=" + FinancialYear.AddYears(1).AddDays(-1).ToString("%d-MMM-yyyy"));
            ddYear.Items.Add(li);
            if (FilterDateFrom.Year == i)
                li.Selected = true;

            FinancialYear = FinancialYear.AddYears(-1);
        }
        if (QueryString("update") == "1")
        {
            UpdateMonthlyValues();
        }
        ShowDataTable();
    }

    void ShowDataTable()
    {
        DateTime dt = new DateTime(FilterDateFrom.Ticks);
        StringBuilder emptableRows=new StringBuilder(); 
        Dictionary<int, BillingLib.Employee> empList = BillingLib.Employee.GetAll();
        int ctr = 0;
        double[] TotalExpense = new double[12];
        List<Salary> listSalary = Salary.GetAll(FilterDateFrom, FilterDateTo);
        foreach (BillingLib.Employee emp in empList.Values)
        {
            Boolean RecordFound = StoreID == 0; // if no store is selected show all the employees

            if (emp.StoreID == StoreID)
                RecordFound = true;

            // if this employee has been given salary for the selected store then show the records even if this employee is not currently working in the selected store
            if (emp.IsDelete == 1 && listSalary.FirstOrDefault(m => m.EmployeeID == emp.ID) == null)
                RecordFound = false;

            if (!RecordFound)
                continue;

            emptableRows.Append("<tr><td>" + ++ctr + "<td class='RowHead' style='text-align:left;" + (emp.IsDelete == 1 ? "text-decoration: line-through;color:red" : "") + "'><a href='Employee.aspx?id=" +emp.ID + "' target='_blank'>" + emp.Name + "</a>");

            dt = new DateTime(FilterDateFrom.Ticks);
            for (int i = 0; i < 12; i++)
            {
                Salary s = listSalary.FirstOrDefault(m => m.EmployeeID == emp.ID && m.SalaryDate == dt);
                double Amt = s != null ? s.Total : 0;

                if (s != null && StoreID != 0 && StoreID != s.StoreID)
                {
                    emptableRows.Append("<td>" + toCurrency(Amt) + "<br/><span style='font-size:9px;'>" + Global.StoreList[s.StoreID].Name + "</span>");
                }
                else
                {
                    emptableRows.Append("<td " + (Amt != 0 ? "class='HasValue'" : "") + "><a href='#' onclick=\"return EditSalary(" + emp.ID + ",'" + dt.ToString("dd-MMM-yyyy") + "','" + emp.Name + "');\" >" + (Amt != 0 ? toCurrency(Amt) : "Edit") + "</a>");// + "-" + (s != null ? s.StoreID : 0));
                    TotalExpense[i] += Amt;
                }
                dt = dt.AddMonths(1);
            }
        }
        
        StringBuilder str = new StringBuilder("<div id='divData'><table class='datatable table-bordered' style='font-size:12px;'><tr><th>");
        StringBuilder emptable = new StringBuilder("<br/><table class='datatable table-bordered' style='font-size:12px;'><tr><th><th>");

        string empSalaryTotal = "";
        double SalaryTotal=0;
        dt = new DateTime(FilterDateFrom.Ticks);
        for (int i = 0; i < 12; i++)
        {
            str.Append("<th style='white-space:nowrap'>" + dt.ToString("MMM-yy"));
            emptable.Append("<th style='white-space:nowrap'>" + dt.ToString("MMM-yy"));
            empSalaryTotal+="<th>"+Cmn.toCurrency(TotalExpense[i]);
            SalaryTotal+=TotalExpense[i];
            dt = dt.AddMonths(1);
        }

        emptable.Append("<tr><th><th>Salary Total " + Cmn.toCurrency( SalaryTotal) + empSalaryTotal + emptableRows);

        List<BillingLib.Purchase> purchaselist = BillingLib.Purchase.GetByStore(dt.AddYears(-1), dt, StoreID);
        List<ReportDailySale> rdsList = ReportDailySale.GetAll(StoreID, 0, dt.AddYears(-1), dt);
        List<MonthlyTotal> MTL = MonthlyTotal.GetAll();
        double[] TotalMargin = new double[12];


        dt = new DateTime(FilterDateFrom.Ticks);
        string RowSalaryTotal = "", RowRent = "", RowElectricity = "", RowPurchase = "", RowTotalExpense = "", RowTotalSale = "", NetExpense = "", RowMargin = "", RowProfitLoss = "", RowMonthlyRequiredSale = "", RowDailyExpense = "", RowOtherExpense = "", RowLoan = "", RowPayment = "";
        for (int i = 0; i < 12; i++)
        {
            string lastDateOfMonth = dt.AddMonths(1).AddDays(-1).ToString("dd-MMM-yyyy");

            RowSalaryTotal += "<td>" + toCurrency(TotalExpense[i]);

            //Rent
            double AmtManual = MTL.Where(m => m.AmountType == AmountType.Rent && m.TotalDate == dt && (m.StoreID == StoreID || StoreID == 0)).Sum(m => m.AmountManual);
            double Amt = AmtManual != 0 ? AmtManual : MTL.Where(m => m.AmountType == AmountType.Rent && m.TotalDate == dt && (m.StoreID == StoreID || StoreID == 0)).Sum(m => m.Amount);
            RowRent += "<td " + (Amt != 0 ? "class='HasValue " + (AmtManual != 0 ? " font-bold" : "") + "'" : "") + ">" + (StoreID != 0 ? "<a href='#' onclick=\"return EditMonthlyTotal('" + dt.ToString("dd-MMM-yyyy") + "'," + (int)AmountType.Rent + "," + StoreID + ");\" >" + (Amt != 0 ? toCurrency(Amt) : "Edit") + "</a>" : toCurrency(Amt));
            TotalExpense[i] += Amt;

            //Electricity
            AmtManual = MTL.Where(m => m.AmountType == AmountType.Electricity && m.TotalDate == dt && (m.StoreID == StoreID || StoreID == 0)).Sum(m => m.AmountManual);
            Amt = AmtManual != 0 ? AmtManual : MTL.Where(m => m.AmountType == AmountType.Electricity && m.TotalDate == dt && (m.StoreID == StoreID || StoreID == 0)).Sum(m => m.Amount);
            RowElectricity += "<td " + (Amt != 0 ? "class='HasValue " + (AmtManual != 0 ? " font-bold" : "") + "'" : "") + ">" + (StoreID != 0 ? "<a href='#' onclick=\"return EditMonthlyTotal('" + dt.ToString("dd-MMM-yyyy") + "'," + (int)AmountType.Electricity + "," + StoreID + ");\" >" + (Amt != 0 ? toCurrency(Amt) : "Edit") + "</a>" : toCurrency(Amt));
            TotalExpense[i] += Amt;

            //Loan
            AmtManual = MTL.Where(m => m.AmountType == AmountType.Loan && m.TotalDate == dt && (m.StoreID == StoreID || StoreID == 0)).Sum(m => m.AmountManual);
            Amt = AmtManual != 0 ? AmtManual : MTL.Where(m => m.AmountType == AmountType.Loan && m.TotalDate == dt && (m.StoreID == StoreID || StoreID == 0)).Sum(m => m.Amount);
            RowLoan += "<td " + (Amt != 0 ? "class='HasValue " + (AmtManual != 0 ? " font-bold" : "") + "'" : "") + ">" + (StoreID != 0 ? "<a href='#' onclick=\"return EditMonthlyTotal('" + dt.ToString("dd-MMM-yyyy") + "'," + (int)AmountType.Loan + "," + StoreID + ");\" >" + (Amt != 0 ? toCurrency(Amt) : "Edit") + "</a>" : toCurrency(Amt));
            TotalExpense[i] += Amt;

            //Daily Expense
            AmtManual = MTL.Where(m => m.AmountType == AmountType.DailyExpense && m.TotalDate == dt && (m.StoreID == StoreID || StoreID == 0)).Sum(m => m.AmountManual);
            Amt = AmtManual != 0 ? AmtManual : MTL.Where(m => m.AmountType == AmountType.DailyExpense && m.TotalDate == dt && (m.StoreID == StoreID || StoreID == 0)).Sum(m => m.Amount);
            RowDailyExpense += "<td " + (Amt != 0 ? "class='HasValue " + (AmtManual != 0 ? " font-bold" : "") + "'" : "") + ">" + (StoreID != 0 ? "<a href='#' onclick=\"return EditMonthlyTotal('" + dt.ToString("dd-MMM-yyyy") + "'," + (int)AmountType.DailyExpense + "," + StoreID + ",'" + lastDateOfMonth + "');\" >" + (Amt != 0 ? toCurrency(Amt) : "Edit") + "</a>" : toCurrency(Amt));
            TotalExpense[i] += Amt;

            //Other Expenses
            AmtManual = MTL.Where(m => m.AmountType == AmountType.OtherExpense && m.TotalDate == dt && (m.StoreID == StoreID || StoreID == 0)).Sum(m => m.AmountManual);
            Amt = AmtManual != 0 ? AmtManual : MTL.Where(m => m.AmountType == AmountType.OtherExpense && m.TotalDate == dt && (m.StoreID == StoreID || StoreID == 0)).Sum(m => m.Amount);
            RowOtherExpense += "<td " + (Amt != 0 ? "class='HasValue " + (AmtManual != 0 ? " font-bold" : "") + "'" : "") + ">" + (StoreID != 0 ? "<a href='#' onclick=\"return EditMonthlyTotal('" + dt.ToString("dd-MMM-yyyy") + "'," + (int)AmountType.OtherExpense + "," + StoreID + ",'" + lastDateOfMonth + "');\" >" + (Amt != 0 ? toCurrency(Amt) : "Edit") + "</a>" : toCurrency(Amt));
            TotalExpense[i] += Amt;

            //Purchase
            AmtManual = MTL.Where(m => m.AmountType == AmountType.Purchase && m.TotalDate == dt && (m.StoreID == StoreID || StoreID == 0)).Sum(m => m.AmountManual);
            Amt = AmtManual != 0 ? AmtManual : MTL.Where(m => m.AmountType == AmountType.Purchase && m.TotalDate == dt && (m.StoreID == StoreID || StoreID == 0)).Sum(m => m.Amount);
            RowPurchase += "<td " + (Amt != 0 ? "class='HasValue " + (AmtManual != 0 ? " font-bold" : "") + "'" : "") + ">" + (StoreID != 0 ? "<a href='#' onclick=\"return EditMonthlyTotal('" + dt.ToString("dd-MMM-yyyy") + "'," + (int)AmountType.Purchase + "," + StoreID + ",'" + lastDateOfMonth + "');\" >" + (Amt != 0 ? toCurrency(Amt) : "Edit") + "</a>" : toCurrency(Amt));
            TotalExpense[i] += Amt;

            //Total Expense
            RowTotalExpense += "<th class='MainRow'>" + toCurrency(TotalExpense[i]);

            //Sale
            AmtManual = MTL.Where(m => m.AmountType == AmountType.Sale && m.TotalDate == dt && (m.StoreID == StoreID || StoreID == 0)).Sum(m => m.AmountManual);
            double TotalSale = Amt = AmtManual != 0 ? AmtManual : MTL.Where(m => m.AmountType == AmountType.Sale && m.TotalDate == dt && (m.StoreID == StoreID || StoreID == 0)).Sum(m => m.Amount);
            RowTotalSale += "<td " + (Amt != 0 ? "class='HasValue " + (AmtManual != 0 ? " font-bold" : "") + "'" : "") + ">" + (StoreID != 0 ? "<a href='#' onclick=\"return EditMonthlyTotal('" + dt.ToString("dd-MMM-yyyy") + "'," + (int)AmountType.Sale + "," + StoreID + ",'" + lastDateOfMonth + "');\" >" + (Amt != 0 ? toCurrency(Amt) : "Edit") + "</a>" : toCurrency(Amt));
            TotalExpense[i] -= TotalSale;

            //Net Expense
            NetExpense += "<th class='MainRow'>" + toCurrency(TotalExpense[i]);

            //Margin
            AmtManual = MTL.Where(m => m.AmountType == AmountType.Margin && m.TotalDate == dt && (m.StoreID == StoreID || StoreID == 0)).Sum(m => m.AmountManual);
            Amt = AmtManual != 0 ? AmtManual : MTL.Where(m => m.AmountType == AmountType.Margin && m.TotalDate == dt && (m.StoreID == StoreID || StoreID == 0)).Sum(m => m.Amount);
            TotalMargin[i] = Amt;

            //Payment
            AmtManual = MTL.Where(m => m.AmountType == AmountType.Payment && m.TotalDate == dt && (m.StoreID == StoreID || StoreID == 0)).Sum(m => m.AmountManual);
            Amt = AmtManual != 0 ? AmtManual : MTL.Where(m => m.AmountType == AmountType.Payment && m.TotalDate == dt && (m.StoreID == StoreID || StoreID == 0)).Sum(m => m.Amount);
            RowPayment += "<td " + (Amt != 0 ? "class='HasValue " + (AmtManual != 0 ? " font-bold" : "") + "'" : "") + ">" + (StoreID != 0 ? "<a href='#' onclick=\"return EditMonthlyTotal('" + dt.ToString("dd-MMM-yyyy") + "'," + (int)AmountType.Payment + "," + StoreID + ",'" + lastDateOfMonth + "');\" >" + (Amt != 0 ? toCurrency(Amt) : "Edit") + "</a>" : toCurrency(Amt));


            RowMargin += "<td>";
            if (TotalSale != 0)
                RowMargin += toCurrency(TotalMargin[i]) + "<br/>" + (Amt / TotalSale * 100).ToString("0") + "%";

            TotalExpense[i] -= TotalMargin[i];
            TotalExpense[i] *= -1;

            //Profit / Loss
            RowProfitLoss += "<th class='MainRow " + (TotalExpense[i] < 0 ? "Negative" : "") + "'>" + toCurrency(TotalExpense[i]);

            //Required Sale
            RowMonthlyRequiredSale += "<th class='MainRow'>" + toCurrency(Math.Abs(TotalExpense[i] / .3));
            dt = dt.AddMonths(1);
        }

        str.Append("<tr><td class='RowHead'>Salary" + RowSalaryTotal);
        str.Append("<tr><td class='RowHead'>Rent" + RowRent);
        str.Append("<tr><td class='RowHead'>Electricity" + RowElectricity);
        str.Append("<tr><td class='RowHead'>Purchase" + RowPurchase);
        str.Append("<tr><td class='RowHead'>Loan" + RowLoan);
        str.Append("<tr><td class='RowHead'>Daily Expense" + RowDailyExpense);
        str.Append("<tr><td class='RowHead'>Other Expense" + RowOtherExpense);
        str.Append("<tr><th class='MainRow RowHead'>Total Expense" + RowTotalExpense);
        str.Append("<tr><th>Sale" + RowTotalSale);
        str.Append("<tr><th class='MainRow RowHead'>Net Expense" + NetExpense);
        str.Append("<tr><th>Margin" + RowMargin);
        str.Append("<tr><th>Payment" + RowPayment);
        str.Append("<tr><th class='MainRow'>Profit & Loss" + RowProfitLoss);
        str.Append("<tr><th class='MainRow'>Monthly Required Sale for 0 loss(30% Margin)" + RowMonthlyRequiredSale);

        ltData.Text = str.Append("</table>").ToString() + emptable.ToString() + "</table></div>";
    }
    private string toCurrency(double Amount)
    {
        System.Globalization.CultureInfo info = System.Globalization.CultureInfo.GetCultureInfo("hi-IN");
        //return Amount.ToString("C2", info); //with Rs sign
        return string.Format(info, "{0:#,#}", Amount); //without rs sign
    }

    void UpdateMonthlyValues()
    {
        if (StoreID == 0)
            return;
        
        DateTime dt = Cmn.ToDate(new DateTime(FilterDateFrom.Year, 4, 1));
        for (var i = 0; i < 12; i++)
        {
            //Sale
            BillingLib.Billing.UpdateMonthlyTotal(dt, StoreID);
            //Purchase
            BillingLib.Purchase.UpdateMonthlyTotal(dt, StoreID);
            //Daily Expense
            BillingLib.ExpenseLog.UpdateMonthlyTotal(dt, StoreID);
            //Payment
            BillingLib.PurchasePayment.UpdateMonthlyTotal(dt, StoreID);
            //Margin
            BillingLib.MonthlyTotal.UpdateMonthlyMargin(dt, StoreID);
            dt = dt.AddMonths(1);
        }
    }
}