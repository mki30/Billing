using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class DownloadDatabase : BasePage
{
    protected new void Page_Load(object sender, EventArgs e)
    {
    }
    protected void btnDownloadFile_Click(object sender, EventArgs e)
    {
         base.Page_Load(sender, e);
         string host = HttpContext.Current.Request.Url.Host.ToLower();
         if (host == "localhost")
         {
             WebClient wc = new WebClient();
             wc.DownloadFile(new System.Uri("http://localhost:50125/DownloadFileResponse.aspx"), @"C:\Billing\billing.zip");
             try
             {
                 string zipPath = @"C:\Billing\billing.zip";
                 string extractPath = @"c:\Billing\";
                 File.Delete(@"C:\Billing\billing.sdf");
                 try
                 {
                     System.IO.Compression.ZipFile.ExtractToDirectory(zipPath, extractPath);


                 }
                 catch
                 {

                 }
                 
                 File.Delete(zipPath);
             }
             catch 
             {
             }
         }
         else
             Response.Write("Allowed in local machine only!");
    }
}