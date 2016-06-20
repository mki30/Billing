using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BillingLib;
using System.Text;

public partial class Customer : BasePage
{
    protected new void Page_Load(object sender, EventArgs e)
    {
        CurrentNav = "navCustomer";
        base.Page_Load(sender, e);
        Title = "Customer";
        if (!IsPostBack)
        {
            ShowCustomerList();
            List<BillingLib.Project> PL = BillingLib.Project.GetAll();
        }
    }
    private void ShowCustomerList()
    {
        StringBuilder str = new StringBuilder("<table class='table table-condensed table-striped table-bordered'><tr><th>#<th>Name<th>Mobile<th>House No<th>Address<th>Area<th>City<th>Pin<th>Loyalty Points<th>Apartment<th>AptNo.");
        List<BillingLib.Customer> CL = BillingLib.Customer.GetAll(0, SessionState.StoreID).OrderBy(m => m.Name).ToList();
        List<BillingLib.Project> PL = BillingLib.Project.GetAll();
        List<BillingLib.Apartment> AL = BillingLib.Apartment.GetAll();

        int ctr = 0;
        foreach (BillingLib.Customer C in CL)
        {
            if (C.Mobile == "") continue;
            Project proj = PL.FirstOrDefault(m => m.ID == C.ProjectID);
            Apartment Apt = AL.FirstOrDefault(m => m.ID == C.ApartmentID);
            str.Append("<tr " + (C.StoreID == 0 ? "style='background-color:orange' title='Store not assigned'" : "") + "><td title=" + C.ID + ">" + (++ctr) + "<td><a href='#' onclick='return ShowEdit(" + C.ID + ")'>" + (C.Name == "" ? "No Name" : C.Name)
                + "</a><td><a href='#' onclick='return ShowCuntomerData(\"" + C.Mobile + "\")'>" + C.Mobile + "</a><td>"
                + C.HouseNumber
                + "<td>" + C.Address
                + "<td>" + C.Area
                + "<td>" + C.City
                + "<td>" + (C.PIN != 0 ? C.PIN.ToString() : "")
                + "<td>-<td>" + (proj != null ? proj.ProjectName : "") + "<td>" + (Apt == null ? "" : Apt.FloorNo.ToString()));
        }
        str.Append("</table>");
        customerList.Text = str.ToString();
    }
}