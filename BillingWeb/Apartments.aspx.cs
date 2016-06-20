using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

public partial class Apartments : BasePage
{
    protected new void Page_Load(object sender, EventArgs e)
    {
        base.Page_Load(sender, e);
        CurrentNav = "navApartment";
        Title = "Apartment & Customers";

        ShowCustomerList();
        List<BillingLib.Project> PL = BillingLib.Project.GetAll().OrderBy(m => m.City).ThenBy(m => m.ProjectName).ToList();

        string city = QueryString("City");
        foreach (BillingLib.Project P in PL)
        {
            if (ddProjectCity.Items.FindByValue(P.City) == null)
                ddProjectCity.Items.Add(new ListItem((P.City==""?"-None-":P.City), P.City));
        }
        ddProjectCity.SelectedValue = city;

        string projectlist = "<h2>Projects</h2><table class='table table-condensed table-striped table-bordered'>";
        foreach (BillingLib.Project P in PL.Where(m => m.City == city))
        {
            projectlist += "<tr><td title=" + P.ID + "><a href='/Apartments.aspx?ProjectID=" + P.ID + "&City=" + ddProjectCity.SelectedValue + "'>" + P.ProjectName
                + "</a><td>" + P.Area + "<td>" + P.City + "<td>" + (P.Pin == 0 ? "" : P.Pin.ToString()) + "<td><a href='#' onclick='return GetProject(" + P.ID + ")'>Edit</a>";
        }
        lblProjectList.Text = projectlist + "</table>";
    }

    protected void ddProject_SelectedIndexChanged(object sender, EventArgs e)
    {
        ShowCustomerList();
    }

    protected void ShowCustomerList()
    {
        List<BillingLib.Customer> CL = BillingLib.Customer.GetByProjectAndApartment(Cmn.ToInt(QueryString("ProjectID")), 0);
        List<BillingLib.Apartment> AL = BillingLib.Apartment.GetByProjectID(Cmn.ToInt(QueryString("ProjectID")));
        StringBuilder str = new StringBuilder("<h2>Apartments</h2><table class='table table-striped table-hover table-bordered' style='width:100%;'><tr><th>#<th>FlatNo</a><th>Customer Name<th>Mobile<th>Address<th>Last Purchase<th>Last Advt.<th>Total Amt");
        int ctrApt = 0;
        foreach (BillingLib.Apartment A in AL)
        {
            BillingLib.Customer C = CL.FirstOrDefault(m => m.ApartmentID == A.ID);
            if (C != null)
            {
                if (C.Mobile == null)
                    continue;
            }
            else
                continue;

            string cust = "<td><a href='#' onclick='return ShowCustomerLink(" + A.ID + ")' title='Link flat- " + A.FloorNo + " to customer'><i class='glyphicon glyphicon-random'/></a><td><td><td><td><td>";
            cust = "<td><a href='#' onclick='return ShowEdit(" + C.ID + ")'>" + (C.Name == "" ? "--" : C.Name) + "</a><td>" + C.Mobile
                + "<td>" + C.Address + "<td>"
                + (C.LastSaleDate == Cmn.MinDate ? "" : C.LastSaleDate.ToString("%d-MMM-yy"))
                + "<a href='#' title='Update last purchase date' onclick='UpdateLastPurchase(" + C.Mobile + "," + C.ID + ")'>&nbsp;<i class='glyphicon glyphicon-refresh pull-right'/></a><td>"
                + (C.LastAdvertizedDate == Cmn.MinDate ? "" : C.LastAdvertizedDate.ToString("%d-MMM-yy"))
                + "<td>" + C.TotalAmountSale.ToString("0");
            str.Append("<tr><td>" + ++ctrApt + "<td>" + A.FloorNo + cust);
        }
        str.Append("</table>");
        lblCustomerList.Text = str.ToString();
    }
}