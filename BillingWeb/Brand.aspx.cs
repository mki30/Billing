using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Brand : BasePage
{
    int id;
    protected new void Page_Load(object sender, EventArgs e)
    {
        CurrentNav = "navBrand";
        base.Page_Load(sender, e);
        Title = "Brand";
        if (SessionState.IsAdmin == false)
            Response.Redirect("/Login.aspx");
        id = Request.QueryString["id"] != null ? Cmn.ToInt(Request.QueryString["id"]) : 0;
        if (!IsPostBack)
        {
            ShowDataTable();
            ShowData(id);
        }
    }
    
    private void ShowData(int id)
    {
        if (id != 0)
        {
            BillingLib.Brand brand = BillingLib.Brand.GetByID(id);
            if (brand != null)
            {
                hdID.Value= brand.ID.ToString();
                txtName.Text = brand.Name.ToString();
                imgBrand.Src = "/Data.aspx?Action=Image&Data1=" + id + "&Data2=Brand";
            }
        }
        ShowDataTable();
    }

    private void ShowDataTable()
    {
        List<BillingLib.Brand> list = BillingLib.Brand.GetAll();
        string str = "<table class='table table-condensed table-striped table-bordered'><tr><th>SNo<th>Name";
        int sNo = 0;
        foreach (BillingLib.Brand b in list)
        {
            str += "<tr   " + (b.ID == id ? "style='background-color:#FDFDA6;'" : "") + " onclick='ShowData(" + b.ID + ")'><td>" + (++sNo) + "<td><a href='/Brand.aspx?id=" + b.ID + "' title=" + b.ID + ">" + b.Name.ToString() + "</a>";
        }
        str += "</table>";
        lblDataTable.Text = str;
    }
    
    protected void btnSave_Click(object sender, EventArgs e)
    {
        BillingLib.Brand b = BillingLib.Brand.GetByID(Cmn.ToInt(hdID.Value));
        if (b == null)
            b = new BillingLib.Brand();
        b.ID = Cmn.ToInt(hdID.Value);
        b.Name = Cmn.ProperCase(txtName.Text);
        b.Save();
        SessionState.Company.UpdateBrand(b);

        if (FileUpload1.HasFile)
        {
            string directory = @"C:\Billing\Brand_Images\";
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);
            FileUpload1.SaveAs((directory + b.ID + ".jpg"));
        }
        Response.Redirect("/Brand.aspx");
    }
}