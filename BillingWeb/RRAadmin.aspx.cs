using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
public partial class RRAadmin : BasePage
{
    protected new void Page_Load(object sender, EventArgs e)
    {
        if (SessionState.IsAdminRRA == false)
            Response.Redirect("/Login.aspx");
        base.Page_Load(sender, e);
        CurrentNav = "navAdmin";

        StringBuilder str = new StringBuilder("<table class='tableborder' style='width:200px;'>");
        foreach (BillingLib.Company c in Global.listCompany.Values)
        {
            str.Append("<tr><td class='alert-danger'><a href='#' onclick='ShowCompany(" + c.ID + ")'>" + c.ID + "-" + c.Name + "</a>");
            str.Append("<td style='padding-left:20px;' class='alert-success'><a href='#' onclick='ShowStore(0," + c.ID + ")'>(+Store)</a>");
            List<BillingLib.Store> storeList = BillingLib.Store.GetByCompanyID(c.ID);
            foreach (BillingLib.Store s in storeList)
            {
                str.Append("<tr><td style='padding-left:20px;' class='alert-success'><a href='#' onclick='ShowStore(" + s.ID + "," + c.ID + ")'>" + s.ID + "-" + s.Name + "</a>");
                str.Append("<td style='padding-left:20px;' class='alert-warning'><a href='#' onclick='ShowTerminal(0," + s.ID + ")'>(+Term)</a>");
                foreach (BillingLib.Terminal t in Global.TerminalList.Values.Where(m => m.StoreID == s.ID))
                {
                    str.Append("<tr><td colspan='2' style='padding-left:40px;' class='alert-warning'><a href='#' onclick='ShowTerminal(" + t.ID + "," + s.ID + ")'>" + t.ID + "-" + t.Name + "</a>");
                }
            }
        }
        str.Append("</table>");
        ltList.Text = str.ToString();
    }
}