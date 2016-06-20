using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlServerCe;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BillingLib;
using System.Text;

public partial class Company : BasePage
{
    int id;
    protected new void Page_Load(object sender, EventArgs e)
    {
        CurrentNav = "navPurchase";
        base.Page_Load(sender, e);
        Title = "ReturnReport";
        id = Request.QueryString["id"] != null ? Cmn.ToInt(Request.QueryString["id"]) : 0;
        
        if (!IsPostBack)
        {
            Common.FillVendorDropdown(ddVendor);
            ddVendor.SelectedValue = VendorID.ToString();
            ShowData(id);
            ShowDataTable();
        }
        ltDateFilter.Text = GetDateFilterLinks(0);
    }
    void ShowData(int id)
    {
        if (id > 0)
        {
            BillingLib.Purchase P = BillingLib.Purchase.GetAll(id).First();
            if (P != null)
            {
                lblID.Text = P.ID.ToString();
                ddVendor.SelectedValue = P.VendorID.ToString();
            }
        }
    }

    private void ShowDataTable()
    {
        List<BillingLib.Purchase> list = BillingLib.Purchase.GetByStore(FilterDateFrom, FilterDateTo, SessionState.StoreID, VendorID).OrderByDescending(m => m.PurchaseDate).ToList();
        
        StringBuilder str = new StringBuilder("<table class='table-striped table-hover table-bordered' style='width:100%'><tr><th>SNo<th>Date<th>Vendor<th>Store<th class='alnright'>Amount<th class='alnright'>Invoice No.<th>RC No.<th>Items");
        double TotalPurchase = 0;
        int srNo = 0;
        foreach (BillingLib.Purchase P in list)
        {
            BillingLib.Vendor vendor = SessionState.Company.vendorList.FirstOrDefault(m => m.ID == P.VendorID); 
            BillingLib.Store store = null;

            Global.StoreList.TryGetValue(P.StoreID, out store);
            str.Append("<tr  " + (P.ID == id ? "style='background-color:#FDFDA6;'" : "") + " href=\"/Inventory.aspx?purchaseId=" + P.ID + "\"><td>" + (++srNo) + "<td>" + P.PurchaseDate.ToString("%d-MMM-yy")
                + "<td><a href='/Inventory.aspx?purchaseId=" + P.ID + "' target='_blank'>" + (vendor != null ? vendor.Name : "-") + "<td>" + (store != null ? store.Name : "-")
                + "<td class='alnright'>" + P.Amount.ToString("0")
                + "<td class='alnright'>" + P.InvoiceNo
                + "<td>" + P.RCNo
                + "<td>" + (P.ItemCount != 0 ? P.ItemCount.ToString() : "")
                );
            TotalPurchase += P.Amount;
        }
        str.Append("<tr><td colspan='4'><th class='alnright'>" + TotalPurchase.ToString("0") + "<td colspan='4'>");
        str.Append("</table>");
        lblDataTable.Text = str.ToString();
    }
    protected void ddVendor_SelectedIndexChanged(object sender, EventArgs e)
    {
        newLink.HRef = "?vendorid=" + ddVendor.SelectedValue;
        Response.Redirect("/Purchase.aspx?vendorid=" + ddVendor.SelectedValue);
        ShowDataTable();
    }
}