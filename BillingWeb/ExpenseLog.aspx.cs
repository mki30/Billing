using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ExpenseLog : BasePage
{
    int id;
    protected new void Page_Load(object sender, EventArgs e)
    {
        base.Page_Load(sender, e);
        Title = "Expense Log";
        CurrentNav = "navExpenseLog";
        id = Request.QueryString["id"] != null ? Cmn.ToInt(Request.QueryString["id"]) : 0;
        string date = Request.QueryString["filterdatefrom"] != null ? Request.QueryString["filterdatefrom"] : "";
        int dailyExpense = Request.QueryString["dailyexpense"] != null ? Cmn.ToInt(Request.QueryString["dailyexpense"]) : 0;
        if (!IsPostBack)
        {
            txtDate.Text = date == "" ? DateTime.Now.ToString("%d-MMM-yyyy") : date;
            Common.FillTerminal(ddTerminal, SessionState.CompanyID, SessionState.StoreID);
            Common.FillExpenseType(ddExpenseType);
            Common.FillExpenseType(ddExpenseTypeSelect);
            if (dailyExpense > 0)
                ShowDailyExpense();
            else
            ShowDataTable();
        }
        ltFilters.Text = GetDateFilterLinks(0);
        ddExpenseTypeSelect.SelectedValue = expensetype.ToString();
    }
    private void ShowDataTable()
    {
        string csvname = "\"Expense.csv\"";
        lblDataTable.Text = "<a href='#' onclick='DownloadFile(" + csvname + ")'>Download<i class='glyphicon glyphicon-download-alt'></i></a>"+
        "&nbsp;&nbsp;&nbsp;&nbsp;<a class='label label-info' href='/ExpenseLog.aspx?reporttype=0&filterdatefrom=" + FilterDateFrom.ToString("%d-MMM-yyyy") + "&filterdateto=" + FilterDateTo.ToString("%d-MMM-yyyy") + "&dailyexpense=1&expensetype="+expensetype+"'>Daily Expense</a>"
        + BillingLib.ExpenseLog.GetExpenseDataTable(SessionState.Company, FilterDateFrom, FilterDateTo, id, false, SessionState.StoreID,expensetype);
    }

    private void ShowDailyExpense()
    {
        List<BillingLib.ExpenseLog> list = BillingLib.ExpenseLog.GetByDate(FilterDateFrom, FilterDateTo, SessionState.StoreID,expensetype);
        Dictionary<string, double> datewiseList = new Dictionary<string, double>();
        foreach (BillingLib.ExpenseLog el in list)
        {
            double val = 0;
            if (datewiseList.TryGetValue(el.Date.ToString("dd-MM-yyyy"), out val))
                datewiseList[el.Date.ToString("dd-MM-yyyy")] = val + el.Amount;
            else
                datewiseList.Add(el.Date.ToString("dd-MM-yyyy"), el.Amount);

        }
        StringBuilder sb = new StringBuilder("<a href='#' onclick='DownloadFile(\"DailyExpense.csv\")'>Download<i class='glyphicon glyphicon-download-alt'></i></a>&nbsp;&nbsp;&nbsp;&nbsp;<a class='label label-info' href='/ExpenseLog.aspx?reporttype=0&filterdatefrom=" + FilterDateFrom.ToString("%d-MMM-yyyy") + "&filterdateto=" + FilterDateTo.ToString("%d-MMM-yyyy") + "&expensetype="+expensetype+"'>Expense log</a><table class='table-condensed table-striped table-bordered csvTable'><tr><th>Date<th>Amount");
        double Total = 0;
        foreach (KeyValuePair<string, double> entry in datewiseList)
        {
            sb.Append("<tr><td>" + entry.Key + "<td class='alnright'>" + Cmn.toCurrency(entry.Value));
            Total += entry.Value;
        }
        lblDataTable.Text = sb.Append("<tr><th>Total<th class='alnright'>" + Cmn.toCurrency(Total) + "</table>").ToString();
    }
}