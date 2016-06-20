using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BillingLib;
using System.Text;

public partial class Inventory : BasePage
{
    int purchaseId = 0;
    protected new void Page_Load(object sender, EventArgs e)
    {
        CurrentNav = "navPurchase";
        base.Page_Load(sender, e);

        btnDelete.Attributes.Add("style", "display:none");
        Common.FillVendorDropdown(ddVendor);
        //if (SessionState.IsAdminRRA)
        //    btnDeletePurchase.Visible = true;

        int InventoryID = Request.QueryString["id"] != null ? Cmn.ToInt(Request.QueryString["id"]) : 0;
        purchaseId = Request.QueryString["purchaseId"] != null ? Cmn.ToInt(Request.QueryString["purchaseId"]) : 0;

        Title = (purchaseId == 0 ? "New" : "Edit") + " Purchase";
        if (VendorID != 0)
            Title = (SessionState.Company.vendorList.FirstOrDefault(m => m.ID == VendorID).Name);

        if (!IsPostBack)
        {
            txtDate.Text = DateTime.Today.ToString("%d-MMM-yyyy");

            //linkNewPurchase.HRef = "/Inventory.aspx?vendorid=" + VendorID;
            Common.FillTaxDropDown(ddTax);
            Common.FillUnitDropdown(ddUnitType);
            Common.FillUnitDropdown(ddCostUnitType);
            hdPurchaseID.Value = purchaseId.ToString();

            BillingLib.Purchase pur = BillingLib.Purchase.GetByID(purchaseId);
            if (pur != null)
            {
                Title = (SessionState.Company.vendorList.FirstOrDefault(m => m.ID == pur.VendorID).Name) + " Purchase " + pur.InvoiceNo;
            }
            //lblDataTable.Text = BillingLib.Inventory.GetHTML(SessionState.Company, purchaseId, InventoryID);
            ShowPurchase(purchaseId, VendorID);
        }
    }

    void ShowPurchase(int id, int vendorId)
    {
        BillingLib.Vendor v = SessionState.Company.vendorList.FirstOrDefault(m => m.ID == vendorId);
        if (v != null)
        {
            lblVendor.Text = v.Name;
            hdVendorID.Value = v.ID.ToString();
        }
        if (id == 0)
        {
            //inventoryform.Visible = false;
            return;
        }
        txtDate.Text = DateTime.Now.ToString("%d-MMM-yyyy");
        if (id > 0)
        {
            BillingLib.Purchase P = BillingLib.Purchase.GetAll(id).First();
            if (P != null)
            {
                ltPurchaseID.Text = P.ID.ToString();
                hdIDPurchase.Value = P.ID.ToString();
                txtDate.Text = Cmn.ToDate(P.PurchaseDate).ToString("%d-MMM-yyyy");
                v = SessionState.Company.vendorList.FirstOrDefault(m => m.ID == P.VendorID);
                if (v != null)
                {
                    lblVendor.Text = "<a href='#' onclick='ShowVendorChangeForm();'>" + v.Name + "</a>";
                    hdVendorID.Value = v.ID.ToString();
                    //linkNewPurchase.HRef = "/Inventory.aspx?vendorid=" + v.ID;
                }
                txtInvoiceNo.Text = P.InvoiceNo;
                hdRcNo.Value = txtRCNo.Text = P.RCNo;
                hdDatePurchase.Value = P.PurchaseDate.ToString("dd-MMM-yyyy");
                lblBillAmount.Text = Cmn.toCurrency( P.Amount);
                chkIncludingTax.Checked = P.ItemIncludesTax == 1;
                chkDelete.Checked = P.IsDelete == 1;
                chkForm38.Checked = P.IsForm38 == 1;
                chkIsNonUpPurchase.Checked = P.IsNonUP == 1;
                txtCST.Text = P.CST.ToString();

            }
        }
    }
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        BillingLib.Inventory i = BillingLib.Inventory.GetByID(Cmn.ToInt(hdID.Value));
        if (i != null)
        {
            i.Delete();
            Purchase P = Purchase.GetByID(i.PurchaseID);
            if (P != null)
                P.UpdateTotal(); //Update PurchaseTotal and ItemCount in purchase in case of delete an inventory item
            lblStatus.Text = "Deleted";
            Response.Redirect("/Inventory.aspx?purchaseId=" + i.PurchaseID);
        }
    }
    //protected void btnDeletePurchase_Click(object sender, EventArgs e)
    //{
    //    BillingLib.Purchase P = BillingLib.Purchase.GetByID(Cmn.ToInt(hdIDPurchase.Value));
    //    if (P != null)
    //        if (P.DeletePurchase() == "")
    //            Response.Redirect("Purchase.aspx?FilterDateFrom=" + P.PurchaseDate.ToString("dd-MMM-yyyy") + "&FilterDateTo=" + P.PurchaseDate.ToString("dd-MMM-yyyy") + "&ReportType=0&VendorID=" + P.VendorID + "");
    //}
}