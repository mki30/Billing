using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Bank : BasePage
{
    int id;
    protected new void Page_Load(object sender, EventArgs e)
    {
        CurrentNav = "navBank";
        base.Page_Load(sender, e);
        Title = "Bank";
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
            BillingLib.Bank Bank = BillingLib.Bank.GetAll(id).First();
            if (Bank != null)
            {
                lblID.Text = Bank.ID.ToString();
                txtName.Text = Bank.Name;
                txtAddress.Text = Bank.Address;
                txtAccountNo.Text = Bank.AccountNo;
            }
        }
        ShowDataTable();
    }

    private void ShowDataTable()
    {
        List<BillingLib.Bank> list = BillingLib.Bank.GetAll(0);
        string str = "<table class='table table-condensed table-striped table-bordered'><tr><th>SNo<th>Name<th>Account No<th>Address";
        int sNo = 0;
        foreach (BillingLib.Bank b in list)
        {
            str += "<tr   " + (b.ID == id ? "style='background-color:#FDFDA6;'" : "") + " onclick='ShowData(" + b.ID + ")'><td>" + (++sNo) + "<td><a href='/Bank.aspx?id=" + b.ID + "' title=" + b.ID + ">" + b.Name.ToString() + "</a>";
            str += "<td>" + b.AccountNo + "<td>" + b.Address;
        }
        str += "</table>";
        lblDataTable.Text = str;
    }
    
    protected void btnSave_Click(object sender, EventArgs e)
    {
        BillingLib.Bank b = BillingLib.Bank.GetByName(txtName.Text);

        if (b == null)
            b = new BillingLib.Bank();

        b.ID = Cmn.ToInt(lblID.Text);
        b.Name = Cmn.ProperCase(txtName.Text);
        b.AccountNo = txtAccountNo.Text;
        b.Address = txtAddress.Text;

        b.Save();

        Response.Redirect("/Bank.aspx");
    }
}