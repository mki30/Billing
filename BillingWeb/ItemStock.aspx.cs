using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BillingLib;
using System.Text;

public partial class ItemStock : BasePage
{
    protected new void Page_Load(object sender, EventArgs e)
    {
        base.Page_Load(sender, e);
        int id = Request.QueryString["id"] != null ? Cmn.ToInt(Request.QueryString["id"]) : 0;
        if (id > 0)
        {
            BillingLib.Item i = BillingLib.Item.GetAll(id).First();

            List<BillingLib.Inventory> listInven = BillingLib.Inventory.GetAll();
            List<BillingLib.BillingItem> listBillingItem = BillingLib.BillingItem.GetAll();

            double purchase = listInven.Where(m => m.ItemID == i.ID).Sum(m => m.Quantity);
            double sale = listBillingItem.Where(m => m.PLU == i.PLU).Sum(m => m.Quantity);
            double stock = purchase - sale;
            StringBuilder str = new StringBuilder("<table class='table table-bordered' style='width:50%'><tr><th>Purchase<th>Sale<th>Stock");
            str.Append("<tr><td>" + purchase + "<td>" + sale + "<td>" + stock);
            lblStock.Text = str.Append("</table>").ToString();
        }
    }
}