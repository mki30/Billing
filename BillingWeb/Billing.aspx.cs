using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BillingLib;
using System.Text;
using System.Collections.Specialized;
using System.Net;
using System.Web.Script.Serialization;

public partial class Billing : BasePage
{
    protected new void Page_Load(object sender, EventArgs e)
    {
        CurrentNav = "navBilling";
        base.Page_Load(sender, e);
        Title = "Billing";
        Common.FillDilvbetyBoyDropDown(ddDeliveryBoy);
        ddDilivery.SelectedValue = Cmn.ToInt(QueryString("deliverytype")).ToString();
        ddDeliveryBoy.SelectedValue = QueryString("deliveryboy");
        hdStoreID.Value = SessionState.StoreID.ToString();
        txtBilNo.Text = QueryString("billno");
        ltDateFilter.Text = GetDateFilterLinks(0);

    }
}