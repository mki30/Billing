using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlServerCe;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BillingLib;

public partial class Tax : BasePage
{
    protected new void Page_Load(object sender, EventArgs e)
    {
        CurrentNav = "navTax";
        base.Page_Load(sender, e);
        Title = "Tax";
        if (SessionState.IsAdmin == false)
            Response.Redirect("/Login.aspx");
        int id = Request.QueryString["id"] != null ? Cmn.ToInt(Request.QueryString["id"]) : 0;
        if (!IsPostBack)
        {
            ShowDataTable(id);
            ShowData(id);
        }
    }
    void ShowData(int id)
    {
        if (id != 0)
        {
            BillingLib.Tax tax = BillingLib.Tax.GetByID(id);
            if (tax != null)
            {
                lblID.Text = tax.ID.ToString();
                txtRate.Text = tax.Rate.ToString();
            }
        }
        ShowDataTable(id);
    }
    private void ShowDataTable(int id)
    {
        string str = "<table class='table table-condensed table-striped table-bordered' style='width:50%'><tr><th>ID<th>Rate";
        foreach (BillingLib.Tax t in BillingLib.Tax.GetAll().Values)
        {
            str += "<tr  " + (t.ID == id ? "style='background-color:#FDFDA6;'" : "") + "><td>" + t.ID + "<td><a href='/Tax.aspx?id=" + t.ID + "'>" + t.Rate.ToString() + "</a>";
        }
        str += "</table>";
        lblDataTable.Text = str;
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        BillingLib.Tax t = BillingLib.Tax.GetByID(Cmn.ToInt(lblID.Text));
        if (t == null) t = new BillingLib.Tax();
        t.ID = Cmn.ToInt(lblID.Text);
        t.Rate = Cmn.ToDbl(txtRate.Text);
        t.Save();
        lblID.Text = t.ID.ToString();
        Response.Redirect(Request.CurrentExecutionFilePath);
    }
}