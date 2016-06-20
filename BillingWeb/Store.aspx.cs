using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Store :BasePage
{
    protected new void Page_Load(object sender, EventArgs e)
    {
        base.Page_Load(sender, e);
        Title = "Store";
        CurrentNav = "navStore";
        if (SessionState.IsAdmin == false)
            Response.Redirect("/Login.aspx");
        
        int id = Request.QueryString["id"] != null ? Cmn.ToInt(Request.QueryString["id"]) : 0;
        if (!IsPostBack)
        {
            lblCompanyID.Text = QueryString("companyid");
            ShowData(id);
            ShowDataTable();
        }
    }
    void ShowData(int id)
    {
        BillingLib.Company C;
        Global.listCompany.TryGetValue(Cmn.ToInt(lblCompanyID.Text), out C);
        if (C != null)
            lblCompany.Text = C.Name;
        
        if (id > 0)
        {
            BillingLib.Store S = BillingLib.Store.GetByID(id);
            if (S != null)
            {
                lblID.Text = S.ID.ToString();
                Global.listCompany.TryGetValue(S.CompanyID, out C);
                if(C != null)
                {
                    lblCompanyID.Text = C.ID.ToString();
                    lblCompany.Text = C.Name;
                }
                txtName.Text = S.Name;
                txtAddress1.Text = S.Address1;
                txtAddress2.Text = S.Address2;
                txtPhone.Text = S.Phone;
                chkIsFranchise.Checked = S.IsFranchise;
                ShowDataTable();
            }
            //ShowDataTable();
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (lblCompanyID.Text == "0") return;
        BillingLib.Store s = BillingLib.Store.GetByID(Cmn.ToInt(lblID.Text));
        if (s == null) s = new BillingLib.Store();
        s.ID = Cmn.ToInt(lblID.Text);
        s.Name = Cmn.ProperCase(txtName.Text);
        s.CompanyID = Cmn.ToInt(lblCompanyID.Text); 
        s.Address1 = txtAddress1.Text;
        s.Address2 = txtAddress2.Text;
        s.Phone = txtPhone.Text;
        s.IsFranchise = chkIsFranchise.Checked;
        s.Save();
        lblID.Text = s.ID.ToString();
        ShowDataTable();
    }
    private void ShowDataTable()
    {
        List<BillingLib.Store> list = BillingLib.Store.GetByCompayID(Cmn.ToInt(lblCompanyID.Text));
        String str = "<table table class='table-condensed table-striped table-bordered'><tr><th>ID<th>Company<th>Name<th>Addrerss1<th>Addrerss2<th>Phone";
        foreach (BillingLib.Store T in list)
        {
            BillingLib.Company comp = null;
            Global.listCompany.TryGetValue(T.CompanyID, out comp);
            str += "<tr><td>" + T.ID.ToString() + "<td>" + (comp == null ? "" : comp.Name) + "<td><a href='/Store.aspx?id=" + T.ID + "'>" + T.Name + "</a><td>" + T.Address1 + "<td>" + T.Address2 + "<td>" + T.Phone;
        }
        str += "</table>";
        lblDataTable.Text = str;
    }
    protected void ButAddNew_Click(object sender, EventArgs e)
    {
        Response.Redirect("/Store.aspx");
    }
    protected void ddCompany_SelectedIndexChanged(object sender, EventArgs e)
    {
        ShowDataTable();
    }
}