using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Order : BasePage
{
    protected new void Page_Load(object sender, EventArgs e)
    {
        CurrentNav = "navOrder";
        base.Page_Load(sender, e);
        Title = "Brand";

        if (!IsPostBack)
        {
            ltDateFilter.Text = GetDateFilterLinks(Cmn.ToInt(QueryString("type")));
            ShowOrderList();

            Common.FillStoreDropdown(dropStore, true);
            List<BillingLib.Project> PL = BillingLib.Project.GetAll();

            ddSociety.Items.Add(new ListItem("-Select-", ""));
            foreach (BillingLib.Project P in PL)
            {
                ddSociety.Items.Add(new ListItem(P.ProjectName, P.ID.ToString()));
            }
            ddSociety.Items.Add(new ListItem("Other", "other"));
        }
    }

    private void ShowOrderList()
    {
        StringBuilder str = new StringBuilder("<table class='table table-condensed table-striped table-bordered'><tr><th>ID<th>Mobile<th>Name<th>Address<th>Society<th>Delivery/Order Date<th>Qnt.<th>Amt.<th>Store<th>Delivery<th>Status");
        List<BillingLib.Billing> list = BillingLib.Billing.GetAll(true, FilterDateFrom, FilterDateTo, "", 0, 0, 0, true);
        foreach (BillingLib.Billing B in list)
        {
            TimeSpan duration = new TimeSpan(DateTime.Now.Ticks - B.BillDate.Ticks);
            BillingLib.Store S;
            Global.StoreList.TryGetValue(B.store.ID, out S);

            string Span = "";
            if (duration.TotalHours > 24)
                Span = duration.Days + " day";
            else if (duration.TotalHours > 1)
                Span = duration.TotalHours.ToString("0") + " hr";
            else
                Span = duration.TotalMinutes.ToString("0") + " m";

            double itemCount = B.ItemList.Count;
            double Amount = B.TotalAmount;

            str.Append("<tr><td>" + B.ID + "<td><a href='#' onclick='GetOrderItem(" + B.ID + ")'>" + B.customer.Mobile
                + "</a><td>" + B.customer.Name + "<td>" + B.customer.Address + "<a href='http://maps.google.com/?q=" + B.customer.Address + "' target='_blank'><span class='glyphicon glyphicon-map-marker'></span</a><td>"
                + "<td style='white-space:nowrap'>" + (B.DeliveryTime == Cmn.MinDate ? "" : B.DeliveryTime.ToString("%d-MMM-yy HH:mm")) + "<br/>"
                + B.BillDate.ToString("%d-MMM-yy HH:mm") + " " + Span + "<td>" + itemCount + "<td>" + Amount + "<td>" + (S != null ? S.Name : "") + "<td>" + B.DeliveryBy
                + "<td><a href='#'  onclick='changeOrderStatus(" + B.ID + ",\"prev\")'><span class='glyphicon glyphicon-chevron-left'></span></a><span id='spanOrderStatusID' style='display:none;'>" + (int)B.orderStatus + "</span><span id='spanOrderStatus'>"
                + B.orderStatus + "</span><a href='#' onclick='changeOrderStatus(" + B.ID + ",\"next\")'><span class='glyphicon glyphicon-chevron-right'></span</a>"
             );
        }
        str.Append("</table>");
        OrderList.InnerHtml = str.ToString();
    }
}