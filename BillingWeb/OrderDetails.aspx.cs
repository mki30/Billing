using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BillingLib;
using System.Text;

public partial class OrderDetails : BasePage
{
    protected new void Page_Load(object sender, EventArgs e)
    {
        base.Page_Load(sender, e);
        Title = "Order Details";
        int id = Request.QueryString["id"] != null ? Cmn.ToInt(Request.QueryString["id"]) : 0;
        string mobile = Request.QueryString["mobile"] != null ? Request.QueryString["mobile"] : "";
        if (!IsPostBack)
        {
            ShowData(mobile);
        }
    }

    private void ShowData(string mobile)
    {
        List<BillingLib.Billing> list = BillingLib.Billing.GetByMobile(mobile);
        StringBuilder str =new StringBuilder("<table class='table table-condensed table-striped table-bordered' style='width:50%'><tr><th>ID<th>Bill No<th>Bill Date<th>Total Amount");
        foreach (BillingLib.Billing b in list)
        {
            str.Append("<tr onclick='ShowData(" + b.ID + ")'><td>" + b.ID +"<td>"+b.BillNo+"<td>" + b.BillDate.ToString("%d-MM-yyyy HH:mm")+"<td>"+b.TotalAmount);
        }
        lblDataTable.Text = str.Append("</table>").ToString();
    }
}