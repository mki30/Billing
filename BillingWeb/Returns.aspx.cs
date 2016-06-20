using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BillingLib;

public partial class Returns1 : BasePage
{
    RecordType mRecordType;
    protected new void Page_Load(object sender, EventArgs e)
    {
        base.Page_Load(sender, e);

        Title = "Inventory Returns";
        btnDelete.Attributes.Add("style", "display:none");

        int VendorID = Cmn.ToInt(QueryString("VendorID"));
        int ID = Cmn.ToInt(QueryString("id"));

        DateTime date = Cmn.ToDate(QueryString("Date"));
        mRecordType = (RecordType)Cmn.ToInt(QueryString("RecordType"));

        CurrentNav = mRecordType == RecordType.Damage ? "navDamage" : "navReturn";

        if (!IsPostBack)
        {
            txtDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            if (date != Cmn.MinDate)
                txtDate.Text = date.ToString("dd-MMM-yyyy");

            BillingLib.Vendor v = SessionState.Company.vendorList.FirstOrDefault(m => m.ID == VendorID);
            if (v != null)
            {
                ltVendor.Text = v.Name + (mRecordType == RecordType.Damage ? " Damage " : " Return ");
                hdVendorID.Text = v.ID.ToString();
            }
            WriteClientScript("var CurrentNav='" + CurrentNav + "', RecordType=" + (int)mRecordType + ",id=" + ID + ";");
        }
    }
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        BillingLib.Inventory i = BillingLib.Inventory.GetByID(Cmn.ToInt(hdID.Text));
        if (i != null)
        {
            i.Delete();
            lblStatus.Text = "Deleted";
            Response.Redirect("/Returns.aspx?vendorid=" + VendorID + "&RecordType=" + (int)mRecordType);
        }
    }
}