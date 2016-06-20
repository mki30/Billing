using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Text;
using System.Collections.Specialized;
using System.Drawing;
using System.Threading;
using System.Globalization;
using System.IO.Compression;
using System.IO;

public class BasePage : System.Web.UI.Page
{
    public string Action = "";
    public string Data1 = "";
    public string Data2 = "";
    public string Data3 = "";
    public Boolean CheckLogin = true;
    public string CurrentNav = "";
    public DateTime FilterDateFrom = DateTime.Today.AddDays(-6);
    public DateTime FilterDateTo = DateTime.Today;
    public ReportType reportType;
    public int VendorID;
    public int StoreID;
    public int FromStore;
    public int ToStore;
    public string SortOn;
    public int ItemID;
    public int EmpID;
    public int BrandID;
    public string Tax = "";
    public int futureorder;
    public int expensetype;
    public Boolean UpdateAll = false;
    public string ThePageScript = "";
    public int DefaultFilterDays = 1;
    public int deliveryType = 0;
    public string deliveryBoy = "";

    public BasePage()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    
    public string FilterHeader
    {
        get
        {
            if (FilterDateFrom == FilterDateTo)
                return "On " + FilterDateFrom.ToString("%d-MMM-yy") + " ";
            else
                return "Between " + FilterDateFrom.ToString("%d-MMM-yy") + " and " + FilterDateTo.ToString("%d-MMM-yy") + " ";
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        WriteClientScript("deliveryboy='" + (deliveryBoy == "" ? "" : deliveryBoy) + "';deliverytype=" + deliveryType + ";expensetype=" + expensetype + ";brandid=" + BrandID + ";tax=" + (Tax == "" ? "''" : Tax) + ";vendorid=" + VendorID + ";ItemID=" + ItemID + ";TodayDate='" + DateTime.Today.ToString("%d-MMM-yyyy") + "';CurrentNav='" + CurrentNav + "';var FilterDateFrom='" + FilterDateFrom.ToString("%d-MMM-yyyy")
            + "',FilterDateTo='" + FilterDateTo.ToString("%d-MMM-yyyy") + "',ReportType=" + (int)reportType + ";futureorder=" + futureorder + ThePageScript);
    }

    public Boolean CheckDateRange(ITextControl txtCtl, int Days = 31)
    {
        Boolean flag = (FilterDateFrom - FilterDateTo).TotalDays > Days;
        txtCtl.Text = flag ? "Please select date range less than or equal to " + Days : "";
        return flag;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (CheckLogin)
        {
            //string host = HttpContext.Current.Request.Url.Host.ToLower();
            //if (host != "localhost")
            //{
            if (!SessionState.LogInDone)
                Response.Redirect("/login.aspx");
            //}
            //else
            //SessionState.LogInDone = true;
        }

        if (SessionState.IsAdminRRA)
        {
            if (QueryString("StoreID") != "")
                SessionState.StoreID = Cmn.ToInt(QueryString("StoreID"));

            if (QueryString("CompanyID") != "")
                SessionState.CompanyID = Cmn.ToInt(QueryString("CompanyID"));
        }

        FilterDateFrom = Cmn.ToDate(QueryString("filterdatefrom", DateTime.Today.AddDays(1 - DefaultFilterDays).ToString("%d-MMM-yyyy")));
        FilterDateTo = Cmn.ToDate(QueryString("filterdateto", DateTime.Today.ToString("%d-MMM-yyyy")));
        if (FilterDateTo == Cmn.MinDate)
            FilterDateTo = FilterDateFrom;

        VendorID = Cmn.ToInt(QueryString("vendorid"));
        FromStore = Cmn.ToInt(QueryString("FromStore"));
        ToStore = Cmn.ToInt(QueryString("ToStore"));
        ItemID = Cmn.ToInt(QueryString("ItemID"));
        EmpID = Cmn.ToInt(QueryString("employeeid"));
        BrandID = Cmn.ToInt(QueryString("brandid"));
        Tax = QueryString("tax");
        futureorder = Cmn.ToInt(QueryString("futureorder"));
        expensetype = Cmn.ToInt(QueryString("expensetype"));
        SortOn = QueryString("sorton");
        UpdateAll = QueryString("updateall") != "";
        if (SessionState.IsAdminRRA || SessionState.IsAdmin)
            StoreID = Cmn.ToInt(QueryString("storeid"));
        else
            StoreID = SessionState.StoreID;
        reportType = (ReportType)Cmn.ToInt(QueryString("reportType"));
        deliveryType = Cmn.ToInt(QueryString("deliverytype"));
        deliveryBoy = QueryString("deliveryboy");

        SessionState.Debug = Request.QueryString["Debug"] != null;
        Action = RouteData.Values["Action"] != null ? RouteData.Values["Action"].ToString() : "";
        Data1 = RouteData.Values["Data1"] != null ? RouteData.Values["Data1"].ToString() : "";
        Data2 = RouteData.Values["Data2"] != null ? RouteData.Values["Data2"].ToString() : "";
        Data3 = RouteData.Values["Data3"] != null ? RouteData.Values["Data3"].ToString() : "";

        if (!string.IsNullOrEmpty(Request.Headers["Accept-Encoding"]) && !SessionState.Debug)
        {
            string enc = Request.Headers["Accept-Encoding"].ToUpperInvariant();

            if (enc.Contains("GZIP") || enc.Contains("*"))//preferred: gzip or wildcard 
            {
                Response.AppendHeader("Content-encoding", "gzip");
                Response.Filter = new GZipStream(Response.Filter, CompressionMode.Compress);
            }
            else if (enc.Contains("DEFLATE"))//deflate 
            {
                Response.AppendHeader("Content-encoding", "deflate");
                Response.Filter = new DeflateStream(Response.Filter, CompressionMode.Compress);
            }
        }
    }

    public static string FolderCheck(string Path)
    {
        bool IsExists = Directory.Exists(HttpContext.Current.Server.MapPath(Path));
        if (!IsExists)
            Directory.CreateDirectory(HttpContext.Current.Server.MapPath(Path));
        return Path;
    }

    public string GetDateFilterLinks(int type, Boolean ShowOnlyMonths = false)
    {
        DateTime dt = DateTime.Today;
        List<object[]> dtsList = new List<object[]>();

        if (ShowOnlyMonths == false)
        {
            dtsList.Add(new object[] { dt.AddDays(-1).ToString("%d-MMM"), dt.AddDays(-1), dt.AddDays(-1) });//yesterday
            dtsList.Add(new object[] { "Today", dt, dt });//today
            dtsList.Add(new object[] { "7 Days", dt.AddDays(-6), dt }); //last 7 Days
            dtsList.Add(new object[] { "30 Days", dt.AddDays(-30), dt }); //last 30 Days
        }

        for (int i = 0; i < 12; i++)
        {
            dtsList.Add(new object[] { dt.ToString("MMM yy"), FirstDayOfMonth(dt), LastDayOfMonth(dt) }); //months
            dt = dt.AddMonths(-1);
        }

        dt = DateTime.Today;
        if (dt.Month < 4)
            dt = dt.AddYears(-1);

        dtsList.Add(new object[] { dt.ToString("yyyy") + "-" + dt.AddYears(1).ToString("yy"), new DateTime(dt.Year, 4, 1), new DateTime(dt.Year + 1, 3, 31) }); //This financial year
        dtsList.Add(new object[] { dt.AddYears(-1).ToString("yyyy") + "-" + dt.ToString("yy"), new DateTime(dt.AddYears(-1).Year, 4, 1), new DateTime(dt.Year, 3, 31) }); //Last Financial Year

        string link = "<td><select id='filterMonth' onchange='location.href=$(this).val()'>";

        foreach (var item in dtsList)
        {
            DateTime dtf = Cmn.ToDate(item[1]);
            DateTime dtt = Cmn.ToDate(item[2]);

            Boolean DateFilterFound = FilterDateFrom.Date == dtf.Date && FilterDateTo.Date == dtt.Date;

            link += "<option data-startdate='" + dtf.ToString("%d-MMM-yyyy") + "' data-enddate='" + dtt.ToString("%d-MMM-yyyy") + "' " + (DateFilterFound ? "selected='selected'" : "")
                + " value='?reporttype=" + (int)reportType + "&filterdatefrom=" + dtf.ToString("%d-MMM-yyyy") + "&filterdateto=" + dtt.ToString("%d-MMM-yyyy")
                + (VendorID != 0 ? "&vendorid=" + VendorID : "")
                + (StoreID != 0 ? "&storeid=" + StoreID : "")
                + (EmpID != 0 ? "&employeeid=" + EmpID : "")
                + (ItemID != 0 ? "&itemid=" + ItemID : "")
                + (BrandID != 0 ? "&brandid=" + BrandID : "")
                + (Tax != "" ? "&tax=" + Tax : "")
                + (expensetype != 0 ? "&expensetype=" + expensetype : "")
                + "'>" + item[0] + "</a>";
        }

        link += "</select>";

        return "<table><tr><td><input id='txtItemSearch' type='text' onkeyup='ItemSearchJSON(this)' placeholder='Item Filter'/>" +
            (ShowOnlyMonths ? "" :
            "<td style='width: 10px;'>From<td style='width: 90px;'><input type='text' value='" + FilterDateFrom.ToString("%d-MMM-yyyy") + "' id='txtFrom' class='datepicker' style='width:90px;'>" +
            "<td style='width: 10px;'>To<td style='width: 90px;'><input type='text' value='" + FilterDateTo.ToString("%d-MMM-yyyy") + "' id='txtTo' class='datepicker' style='width:90px;'>"
            ) +
            "<td><a onclick=\"location.href='?reportType=" + (int)reportType + "&filterdatefrom=' + $('#txtFrom').val()+'&filterdateto=' + $('#txtTo').val()+'&vendorid=' + $('#ddVendor').val()+'&employeeid=' + $('#ddEmployee').val() \" class='btn btn-sm btn-warning'>Show</a>"
            + link + "</table>";
    }

    public static void LogError(Exception ex, string Message, string fileName = "")   //
    {
        string Folder = HttpContext.Current.Server.MapPath(@"~\Error\");
        if (!Directory.Exists(Folder))
            Directory.CreateDirectory(Folder);
        string Filename = HttpContext.Current.Server.MapPath(@"~\Error\" + (!string.IsNullOrWhiteSpace(fileName) ? fileName : DateTime.Now.ToString("dd-MMM-yyyy").Replace('-', '_')) + ".txt");

        string Error = DateTime.Now.ToString() + Environment.NewLine;

        if (!string.IsNullOrEmpty(Message))
            Error += Message + Environment.NewLine;

        if (ex != null)
        {
            Error += ex.Message + Environment.NewLine;
            if (string.IsNullOrWhiteSpace(fileName))
                Error += ex.StackTrace + Environment.NewLine;
        }
        try
        {
            File.AppendAllText(Filename, Error);
        }
        catch { }
    }

    public static string GetImageFileName(string ID, string path, string fileextention = "")
    {
        string fileName;
        for (int i = 1; ; i++)
        {
            fileName = HttpContext.Current.Server.MapPath(path + ID + "_" + i) + (fileextention == "" ? ".jpg" : fileextention);
            if (!File.Exists(fileName))
                break;
        }
        return Path.GetFileName(fileName);
    }

    public static string GetFormString(string FieldName)
    {
        NameValueCollection nvc = HttpContext.Current.Request.Form;
        if (nvc[FieldName] != null)
            return nvc[FieldName];

        return "";
    }

    public static void LogException(string ModuleName, string FunctionName, Exception ex)
    {
        if (HttpContext.Current.Application["ERROR_IDS"] == null)
        {
            HttpContext.Current.Application["ERROR_IDS"] = new Dictionary<string, int>();
            HttpContext.Current.Application["ERROR_LIST"] = new Dictionary<int, string>();
        }

        Dictionary<string, int> ErrorLog = (Dictionary<string, int>)HttpContext.Current.Application["ERROR_IDS"];
        if (!ErrorLog.ContainsKey(ModuleName + "_" + FunctionName))
        {
            ErrorLog.Add(ModuleName + "_" + FunctionName, ErrorLog.Count + 1);
        }

        //get the id of the error
        int i = ErrorLog[ModuleName + "_" + FunctionName];
        Dictionary<int, string> ErrorList = (Dictionary<int, string>)HttpContext.Current.Application["ERROR_LIST"];

        if (!ErrorList.ContainsKey(i))
            ErrorList.Add(i, "");

        ErrorList[i] = ModuleName + ":" + FunctionName + ":" + ex.Message + ":" + DateTime.Now.ToString();
    }

    public static void DeleteExistingFile(string folder, string id)
    {
        string[] files = Directory.GetFiles(HttpContext.Current.Server.MapPath(folder), "" + id + "*");
        if (files.Length > 0)
        {
            foreach (string s in files)
            {
                try
                {
                    string fileame = s.Split('\\').Last();
                    FileInfo fi = new FileInfo(HttpContext.Current.Server.MapPath(folder + "\\" + fileame + ""));
                    if (fi != null)
                        fi.Delete();
                }
                catch { };
            }
        }
    }

    public static void GetAllClientID(Control parent, ref string strCtl)
    {
        foreach (Control ctl in parent.Controls)
        {
            //if (ctl.GetType().ToString().Equals("System.Web.UI.WebControls.TextBox"))
            if (ctl.ID != null)
                strCtl += "var " + ctl.ID + "=\"#" + ctl.ClientID + "\";\n";

            try
            {
                if (ctl.Controls.Count > 0)
                    GetAllClientID(ctl, ref strCtl);
            }
            catch (Exception Ex)
            {
                string str = Ex.Message;
            }
        }
    }

    public static void ClearTextBoxes(Control parent)   //reset fields in form
    {
        foreach (Control ctl in parent.Controls)
        {
            if (ctl.GetType().ToString().Equals("System.Web.UI.WebControls.TextBox"))
                ((TextBox)ctl).Text = "";

            if (ctl.GetType().ToString().Equals("System.Web.UI.WebControls.CheckBox"))
                ((CheckBox)ctl).Checked = false;

            if (ctl.GetType().ToString().Equals("System.Web.UI.WebControls.DropDownList"))
                ((DropDownList)ctl).SelectedIndex = -1;

            if (ctl.Controls.Count > 0)
                ClearTextBoxes(ctl);
        }
    }

    public static void Download(FileInfo file, Page page) //download file from server directory
    {
        Stream s = File.OpenRead(file.FullName);
        Byte[] buffer = new Byte[s.Length];
        try { s.Read(buffer, 0, (Int32)s.Length); }
        finally { s.Close(); }
        page.Response.ClearHeaders();
        page.Response.ClearContent();
        page.Response.ContentType = "application/octet-stream";
        page.Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);
        page.Response.BinaryWrite(buffer);
        page.Response.End();
    }

    public void WriteClientScript(string Client_Script)
    {
        ClientScriptManager cs = this.ClientScript;
        string csname1 = "S1";
        if (!cs.IsClientScriptBlockRegistered(this.GetType(), csname1))
        {
            StringBuilder cstext2 = new StringBuilder();
            cstext2.Append("<script language='javascript' type='text/javascript'> \n");
            cstext2.Append(Client_Script);
            cstext2.Append("</script>");
            cs.RegisterClientScriptBlock(this.GetType(), csname1, cstext2.ToString(), false);
        }
    }

    public void WriteResponse(Page p, string Message, string Compression = "", Boolean isJson = false)
    {
        try
        {
            if (isJson)
                p.Response.ContentType = "text/json";
            if (Compression == "")
                Compression = GetEncode(p);
            if (Compression == "no" || string.IsNullOrEmpty(Compression))
                p.Response.Write(Message);
            else
                p.Response.BinaryWrite(GetCompressed(Message, Compression));
        }
        catch
        {
        }
    }

    public static byte[] GetCompressed(string str, string CompressionType = "gzip")      //Compreess Data 
    {
        byte[] buffer = System.Text.Encoding.ASCII.GetBytes(str);
        MemoryStream ms = new MemoryStream();

        switch (CompressionType)
        {
            case "gzip":
                {
                    GZipStream gz = new GZipStream(ms, CompressionMode.Compress, true);
                    gz.Write(buffer, 0, buffer.Length);
                    gz.Close();
                }
                break;
            case "deflate":
                {
                    DeflateStream dz = new DeflateStream(ms, CompressionMode.Compress);
                    dz.Write(buffer, 0, buffer.Length);
                    dz.Close();
                }
                break;
        }
        byte[] compressedData = (byte[])ms.ToArray();
        return compressedData;
    }

    public static string GetEncode(Page pg, Boolean SetHeader = true)
    {
        string encodings = pg.Request.Headers.Get("Accept-Encoding");
        string encode = "no";

        if (encodings != null)
        {
            encodings = encodings.ToLower();
            if (encodings.Contains("gzip"))
            {
                if (SetHeader)
                    pg.Response.AppendHeader("Content-Encoding", "gzip");
                encode = "gzip";
            }
            else if (encodings.Contains("deflate"))
            {
                if (SetHeader)
                    pg.Response.AppendHeader("Content-Encoding", "deflate");
                encode = "deflate";
            }
        }
        return encode;
    }

    public string QueryString(string Key, string Default = "")
    {
        return Request.QueryString[Key] != null ? Request.QueryString[Key].ToString() : Default;
    }
    public DateTime FirstDayOfMonth(DateTime dateTime)
    {
        return new DateTime(dateTime.Year, dateTime.Month, 1);
    }
    public DateTime LastDayOfMonth(DateTime dateTime)
    {
        return new DateTime(dateTime.Year, dateTime.Month, DateTime.DaysInMonth(dateTime.Year, dateTime.Month));
    }
}

public class SessionState
{
    public static Boolean LogInDone
    {
        get
        {
            if (HttpContext.Current.Session["LOGINDONE"] == null)
                return false;
            else
                return HttpContext.Current.Session["LOGINDONE"].ToString() == "1" ? true : false;
        }
        set
        {
            HttpContext.Current.Session["LOGINDONE"] = value ? "1" : "0";
        }
    }

    public static Boolean IsAdminRRA
    {
        get
        {
            if (HttpContext.Current.Session["IsAdminRRA"] == null)
                return false;
            else
                return HttpContext.Current.Session["IsAdminRRA"].ToString() == "1" ? true : false;
        }
        set
        {
            HttpContext.Current.Session["IsAdminRRA"] = value ? "1" : "0";
        }
    }

    public static Boolean IsAdmin
    {
        get
        {
            if (HttpContext.Current.Session["ISADMIN"] == null)
                return false;
            else
                return HttpContext.Current.Session["ISADMIN"].ToString() == "1" ? true : false;
        }
        set
        {
            HttpContext.Current.Session["ISADMIN"] = value ? "1" : "0";
        }
    }

    public static string UserImage
    {
        get
        {
            if (HttpContext.Current.Session["UserImage"] == null)
                return "";
            else
                return HttpContext.Current.Session["UserImage"].ToString();
        }
        set
        {
            HttpContext.Current.Session["UserImage"] = value;
        }
    }

    public static string UserEmailID
    {
        get
        {
            if (HttpContext.Current.Session["UserEmailID"] == null)
                return "";
            else
                return HttpContext.Current.Session["UserEmailID"].ToString();
        }
        set
        {
            HttpContext.Current.Session["UserEmailID"] = value;
        }
    }

    public static string CompanyName
    {
        get
        {
            if (HttpContext.Current.Session["CompanyName"] == null)
                return "Pure Protien";
            else
                return HttpContext.Current.Session["CompanyName"].ToString();
        }
        set
        {
            HttpContext.Current.Session["CompanyName"] = value;
        }
    }

    public static int UserID
    {
        get
        {
            if (HttpContext.Current.Session["UserID"] == null)
                return -1;
            else
                return Cmn.ToInt(HttpContext.Current.Session["UserID"]);
        }
        set
        {
            HttpContext.Current.Session["UserID"] = value;
        }
    }

    public static int CompanyID
    {
        get
        {
            if (HttpContext.Current.Session["CompanyID"] == null)
                return 1;
            else
                return Cmn.ToInt(HttpContext.Current.Session["CompanyID"]);
        }
        set
        {
            HttpContext.Current.Session["CompanyID"] = value;
        }
    }

    public static BillingLib.Company Company
    {
        get
        {
            BillingLib.Company c;
            if (Global.listCompany.TryGetValue(CompanyID, out c))
                return c;
            return null;
        }
    }

    public static int StoreID
    {
        get
        {
            if (HttpContext.Current.Session["StoreID"] == null)
                return 3;
            else
                return Cmn.ToInt(HttpContext.Current.Session["StoreID"]);
        }
        set
        {
            HttpContext.Current.Session["StoreID"] = value;
        }
    }

    public static Boolean Debug
    {
        get
        {
            if (HttpContext.Current.Session["Debug"] == null)
                return false;
            else
                return Convert.ToBoolean(HttpContext.Current.Session["Debug"]);
        }
        set
        {
            HttpContext.Current.Session["Debug"] = value;
        }
    }
}

public class Orderitem
{
    public int OrderID { get; set; }
    public string Status { get; set; }
}

