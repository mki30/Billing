using BillingLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Payment : BasePage
{
    protected new void Page_Load(object sender, EventArgs e)
    {
        base.Page_Load(sender, e);
        Title = "Payment";
        CurrentNav = "navPayment";

        int ID = Request.QueryString["ID"] != null ? Cmn.ToInt(Request.QueryString["ID"]) : 0;
        string store = Request.QueryString["store"] != null ? Request.QueryString["store"] : "";
        if (!IsPostBack)
        {
            txtDate.Text = DateTime.Now.ToString("%d-MMM-yyyy");
            Common.FillVendorDropdown(ddVendor);
            Common.FillBank(ddBank);
            Common.FillStoreDropdown(ddPaymentStore);

            ddSelectStore.Items.Add(new ListItem("All Stores Payments", "0"));
            foreach (BillingLib.Store c in Global.StoreList.Values.Where(m => m.CompanyID == SessionState.Company.ID))
            {
                ddSelectStore.Items.Add(new ListItem(c.Name, c.ID.ToString()));
            }
            if (store == "")
                ddSelectStore.SelectedValue = SessionState.StoreID.ToString();
            else
                ddSelectStore.SelectedValue = store;

            ddVendor.SelectedValue = VendorID.ToString();
            ShowPaymentList();
            ShowDueList(VendorID);
        }
        ltFilters.Text = GetDateFilterLinks(0);
        if (SessionState.IsAdmin)
            ddPaymentStore.Visible = true;
    }
    void ShowPaymentList()
    {
        lblPaymentList.Text = "<strong>Payment " + FilterHeader + "</strong><br/>" + BillingLib.PurchasePayment.GetHtmlByVendorID(VendorID, FilterDateFrom, FilterDateTo, Cmn.ToInt(ddSelectStore.SelectedValue), SessionState.Company);
    }
    void ShowDueList(int VendorID)
    {
        BillingLib.Vendor Vendor = BillingLib.Vendor.GetByID(VendorID);
        if (Vendor != null)
            Vendor.UpdateAmountDue();


        List<BillingLib.VendorDue> list = BillingLib.VendorDue.GetByStore(Cmn.ToInt(ddSelectStore.SelectedValue), Cmn.FinStartDate.Year);
        StringBuilder str = new StringBuilder();
        double TotalPurchase = 0, TotalPayment = 0, TotalDue = 0, TotalOverPayment = 0;
        

        List<dynamic> tempVendorList = new List<dynamic>();

        foreach (BillingLib.Vendor vendor in SessionState.Company.vendorList.OrderBy(m => m.Name))
        {
            if (VendorID != 0 && VendorID != vendor.ID) continue;

            List<BillingLib.VendorDue> VL = list.Where(m => m.VendorID == vendor.ID).GroupBy(m => m.VendorID).Select(
                vg => new BillingLib.VendorDue()
                {
                    Purchase = vg.Sum(k => k.Purchase),
                    Payment = vg.Sum(k => k.Payment),
                }
                ).ToList();

            foreach (BillingLib.VendorDue V in VL)
            {
                if (V == null || (V.Purchase == 0 && V.Payment == 0))
                    continue;

                double dueAmt = Math.Round( V.Purchase - V.Payment);

                string Link = "/Purchase.aspx?reporttype=0&filterdatefrom=" + Cmn.FinStartDate.ToString("%d-MMM-yyyy") + "&filterdateto=" + Cmn.FinEndDate.ToString("%d-MMM-yyyy") + "&vendorid=" + vendor.ID+ "&storeid=" + SessionState.StoreID;
                string row="<td style='text-align:left'><a href='" + Link + "' target='_blank'>" + vendor.Name + "</a><td>"
                    + Cmn.toCurrency(V.Purchase) + "<td>"
                    + Cmn.toCurrency(V.Payment)
                    + (dueAmt > 0 ? "<td style='background-color:#FFC8C8;'>" + Cmn.toCurrency(dueAmt).ToString() : "<td>")
                    + (dueAmt < 0 ? "<td style='background-color:#FFC8C8;'>" + Cmn.toCurrency(dueAmt).ToString() : "<td>");

                if (dueAmt > 0)
                    TotalDue += dueAmt;
                else
                    TotalOverPayment += dueAmt;

                TotalPurchase += V.Purchase;
                TotalPayment += V.Payment;

                tempVendorList.Add(new { 
                    ID=vendor.ID,
                    Name = vendor.Name,
                    Purchase=V.Purchase,
                    Payment=V.Payment,
                    Due=dueAmt,
                    HTMLRow=row
                });
            }

        }
        List<dynamic> SortedList = tempVendorList.OrderBy(m => m.Name).ToList();
        switch (SortOn)
        {
            case "purcahse": SortedList = tempVendorList.OrderByDescending(m => m.Purchase).ToList(); break;
            case "payment": SortedList = tempVendorList.OrderByDescending(m => m.Payment).ToList(); break;
            case "due": SortedList = tempVendorList.OrderByDescending(m => m.Due).ToList(); break;
        }

        int ctr = 0;
        foreach (var ob in SortedList)
        {
            str.Append("<tr><td>" + ++ctr + ob.HTMLRow);
        }
        string link2 = "<th class='alnright'><a href='?filterdatefrom=" + FilterDateFrom.ToString("dd-MMM-yyyy") + "&filterdateto=" + FilterDateTo.ToString("dd-MMM-yyyy") + "&vendorid=" + VendorID + "&store=" + ddSelectStore.SelectedValue;
        lblDueList.Text = "<strong>Due Amount for year " +(Cmn.ToFinancialYear(FilterDateFrom)-1)+"-"+ Cmn.ToFinancialYear(FilterDateFrom).ToString().Substring(2,2) + "</strong><br/>"
            + "<table  class='datatable table table-condensed table-bordered'><tr><th>#" 
            + link2 +"&sorton=name'>Vendor</a>" 
            + link2 +"&sorton=purchase'>Purchase</a>" 
            + link2 + "&sorton=payment'>Payment</a>"
            + link2 +"&sorton=due'>Due</a>"
            +"<th class='alnright'>Over Payment"
            + "<tr><td><td><th class='alnright'>" + Cmn.toCurrency(TotalPurchase) + "<th class='alnright'>" + Cmn.toCurrency(TotalPayment)
            + "<th class='alnright'>" + Cmn.toCurrency(TotalDue)
            + "<th class='alnright'>" + Cmn.toCurrency(TotalOverPayment)
            + str
            + "</table><span style='color:red;'>To show paid payments please connect the payments to purchases</span>";
    }
}