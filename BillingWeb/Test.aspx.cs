using BillingLib;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class Testaspx : BasePage
{
    List<string> ApartNo = new List<string>();
    new protected void Page_Load(object sender, EventArgs e)
    {

        //    base.Page_Load(sender, e);
        //    if (SessionState.LogInDone)
        //        ZipFiles.DownloadZipToBrowser(new List<string>() { "c:\\Billing\\Billing.sdf" }, this, "Billing");

        //ZipFiles.CreateSample("c:\\Billing\\biling.zip", "", "Billing");

        //Society S;
        //if (Global.ProjectList.TryGetValue(Cmn.ToInt(Data1), out S))
        //{
        //    var newList = S.Select(a => new
        //    {
        //        ID = a.ID,
        //        ProjectName = a.SocietyName,
        //        Address = a.Address,
        //        Area = a.Area,
        //        City = a.City,
        //        State = a.State,
        //        PIN = a.Pin,
        //    }).ToList();
        //    str.Append(new JavaScriptSerializer().Serialize(newList));
        //}
        //AppendError = false;


        //List<BillingLib.Project> PL = BillingLib.Project.GetAllFromPropertyMap();
        //string str1 = "";
        //foreach (BillingLib.Project P in PL)
        //{
        //    str1 += P.ID + "^" + P.ProjectName;
        //}
        
        DatabaseCE db = new DatabaseCE(@"Data Source=c:\\Developement\\PropertyMap\\App_Data\\PropertyList.sdf;");
        string Error = "";
        try
        {
            IDataReader dr = db.GetDataReader("Select ID,SocietyName,Address,Area,City,State,Pin from society where  (CityID = 1) AND (SubCityID = 2) OR  (CityID = 4)", ref Error);
            string str = "";
            while (dr.Read())
            {
                str += dr["ID"].ToString() + "^" + dr["SocietyName"] + "^" + dr["Address"] + "^" + dr["Area"] + "^" + dr["City"] + "^" + dr["State"] + "^" + dr["Pin"];
                str += Environment.NewLine;
            }
            System.IO.File.WriteAllText(Server.MapPath(@"~\test.txt"), str);
        }
        finally
        {
            db.Close();
        }
    }
}