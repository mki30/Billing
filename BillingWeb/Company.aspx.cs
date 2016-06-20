using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlServerCe;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BillingLib;
using System.Configuration;

public partial class Company : BasePage
{
    protected new void Page_Load(object sender, EventArgs e)
    {
        base.Page_Load(sender, e);
        if (SessionState.IsAdmin == false)
            Response.Redirect("Login.aspx");
        CurrentNav = "navCompany";

        Title = "Company";
        int id = Request.QueryString["id"] != null ? Cmn.ToInt(Request.QueryString["id"]) : 0;
        if (!IsPostBack)
        {
            ShowDataTable();
            ShowData(id);
        }
    }
    void ShowData(int id)
    {
        BillingLib.Company C = BillingLib.Company.GetByID(id);
        if (C != null)
        {
            lblID.Text = C.ID.ToString();
            txtName.Text = C.Name;
            txtTIN.Text = C.TIN;
            txtTAN.Text = C.TAN;
            txtPAN.Text = C.PAN;
            txtCst.Text = C.CST;
            txtCIN.Text = C.CIN;
            txtAddress.Text = C.Address.ToString();
        }
        ShowDataTable();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        BillingLib.Company C = new BillingLib.Company();
        C.ID = Cmn.ToInt(lblID.Text);
        C.Name = Cmn.ProperCase(txtName.Text);
        C.TIN = txtTIN.Text;
        C.TAN = txtTAN.Text;
        C.PAN = txtPAN.Text;
        C.CST = txtCst.Text;
        C.CIN = txtCIN.Text;
        C.Address = txtAddress.Text;
        C.Save();
        lblID.Text = C.ID.ToString();
        ShowDataTable();
    }

    private void ShowDataTable()
    {
        List<BillingLib.Company> list = BillingLib.Company.GetAll();
        string str = "<table Class='table table-condensed table-striped table-bordered'><tr><th>ID<th>Name<th>TIN<th>TAN<th>PAN<th>CST<th>Address";
        foreach (BillingLib.Company C in list)
        {
            str += "<tr><td>" + C.ID + "<td><a href='/Company.aspx?id=" + C.ID + "'>" + C.Name + "</a><td>" + C.TIN + "<td>" + C.TAN + "<td>" + C.PAN + "<td>" + C.CST + "<td>" + C.Address;
        }
        str += "</table>";
        lblDataTable.Text = str;
    }
}