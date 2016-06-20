using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MasterPage : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (SessionState.LogInDone == true)
            {
                lnkLogin.InnerText = "Logout";
                lnkLogin.HRef = "login.aspx?logout=1";
                
                ltUser.Text = "<img src='" + SessionState.UserImage + "' style='height:25px' title='" + SessionState.UserEmailID + "' />";
                
                if (SessionState.IsAdmin == true)
                {
                    if (SessionState.IsAdminRRA == true)
                    {
                        menuAdminRRA.Visible = ddCompany.Visible = ddStore.Visible = true;
                        Common.FillCompanyDropdown(ddCompany);
                    }
                    Common.FillStoreDropdown(ddStore);
                    menuMasters.Visible = menuReport.Visible = true;
                }
                else
                {
                    List<BillingLib.Store> SAL = BillingLib.StoreAccess.GetByUserID(SessionState.UserID);
                    foreach (BillingLib.Store SA in SAL)
                    {
                        ddStore.Items.Add(new ListItem(SA.Name, SA.ID.ToString()));
                    }
                }
                
                //Store                    
                BillingLib.Store s = null;
                Global.StoreList.TryGetValue(SessionState.StoreID, out s);
                BillingLib.Company c = null;
                Global.listCompany.TryGetValue(SessionState.CompanyID, out c);
               
            }
        }
    }

}
