using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BillingLib;
using System.Text;
using System.IO;

public partial class Default : BasePage
{
    string placeHolder = "";
    protected new void Page_Load(object sender, EventArgs e)
    {
        CheckLogin = false;
        base.Page_Load(sender, e);
        placeHolder = GetBase64String(Server.MapPath(@"~/images/images.jpg"));

        List<BillingLib.Brand> list = BillingLib.Brand.GetAll();

        List<string> listBandHTML = new List<string>();
        foreach (BillingLib.Brand b in list)
        {
            int itemCount = SessionState.Company.ItemList.Where(m => m.BrandID == b.ID).Count();
            if (itemCount > 0)
                listBandHTML.Add("<a href='/brand/" + b.Name.ToLower() + "'>" + Cmn.ProperCase(b.Name) + " (" + itemCount + ")</a><br/> ");
        }

        ltBrands.Text = "<div class='col-md-3'>";
        int rows = (int)Math.Ceiling(listBandHTML.Count / 4.0);
        int ctr = 0;
        foreach (string s in listBandHTML)
        {
            ltBrands.Text += s;
            ctr++;

            if (ctr % rows == 0)
                ltBrands.Text += "</div><div class='col-md-3'>";
        }

        ltBrands.Text += "</div>";

        if (Data1 == "")
        {
            Data1 = "brand";
            Data2 = "mccain";
        }
        else if (Data2 == "")
        {
            if (Data1 == "brand")
                Data2 = "mccain";
            if (Data1 == "category")
                Data2 = "chicken";
        }

        StringBuilder str = new StringBuilder();
        str.Clear();

        SessionState.CompanyID = 1;
        BillingLib.Company comp = SessionState.Company;

        ltCategory.Text = "<div class='col-md-3'>";
        int ctrl = 0;
        foreach (BillingLib.Menu m in comp.listMenu.Where(m => m.ParentID == 0))
        {
            ctrl++;
            ltCategory.Text += "<a class='sub-category-link' href='/category/" + Cmn.GenerateSlug(m.Name) + "'>" + m.Name + "</a><br/>";
            if (ctrl % 10 == 0)
                ltCategory.Text += "</div><div class='col-md-3'>";
        }
        ltCategory.Text += "</div>";
        str.Clear();
        str.Append(Item.GetItemsHTML(comp, Data1, Data2));
        ltItems.Text = str.ToString();
    }

    string GetBase64String(string ImagePath)
    {
        using (System.Drawing.Image image = System.Drawing.Image.FromFile(ImagePath))   
        {
            using (MemoryStream m = new MemoryStream())
            {
                image.Save(m, image.RawFormat);
                byte[] imageBytes = m.ToArray();
                return Convert.ToBase64String(imageBytes);
            }
        }
    }
}