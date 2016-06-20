using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class Contols_ReturnControl : System.Web.UI.UserControl
{
    public RecordType recordType = RecordType.Purchase;


    protected void Page_Load(object sender, EventArgs e)
    {
        txtDate.Text = Cmn.ToDate(DateTime.Now).ToString("dd-MMM-yyyy");
        Common.FillStoreDropdown(ddToStore, false, true);
        ltStore.Visible = ddToStore.Visible = recordType == RecordType.TransferOut;
        //hiddenIDs.Attributes.Add(""=SessionState.Debug;   
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        BillingLib.Inventory i = BillingLib.Inventory.GetByID(Cmn.ToInt(hdID.Text));
        if (i != null)
        {
            i.Delete();
            lblStatus.Text = "Deleted";
            Response.Redirect(Request.RawUrl);
        }
    }
}

