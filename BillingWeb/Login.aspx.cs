using System;
using System.Collections.Generic;
using System.IO;
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
            string script = string.Empty;
            if (Request.QueryString["as"] != null)
            {
                script = ";Impersonate='" + Request.QueryString["as"] + "';";
            }
            if (Request.QueryString["logout"] != null)
            {
                if (Request.Cookies["email"] != null && Global.Users.ContainsKey(Request.Cookies["email"].Value))
                {
                    Global.Users.Remove(Request.Cookies["email"].Value);
                    ResetSession();
                }
                script += "DoSignOut=true;";
            }
            if (script != string.Empty)
                Common.WriteClientScript(this, script);
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
}