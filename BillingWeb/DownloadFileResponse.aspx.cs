using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class DownloadFileResponse : BasePage
{
    new protected void Page_Load(object sender, EventArgs e)
    {
        base.Page_Load(sender, e);

        if (SessionState.LogInDone)
            ZipFiles.DownloadZipToBrowser(new List<string>() { "c:\\Billing\\Billing.sdf" }, this, "Billing");
    }
}