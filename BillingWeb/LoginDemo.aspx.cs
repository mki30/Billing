using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class LoginDemo : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString["logout"] != null)
            {
                if (Request.Cookies["email"] != null && Global.Users.ContainsKey(Request.Cookies["email"].Value))
                {
                    Global.Users.Remove(Request.Cookies["email"].Value);
                    ResetSession();
                }
                Common.WriteClientScript(this, "DoSignOut=true;");
            }
        }
    }

    private void ResetSession()
    {
        SessionState.LogInDone = false;
        SessionState.IsAdmin = false;
        SessionState.IsAdminRRA = false;
        SessionState.UserEmailID = "";
        SessionState.CompanyID = 1;
        SessionState.StoreID = 3;
    }
    protected void btnGo_Click(object sender, EventArgs e)
    {
        SessionState.StoreID = Cmn.ToInt(ddStore.SelectedValue);
        Response.Redirect("/Billing.aspx");
    }
}