using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using BillingLib;

public partial class Vendor : BasePage
{
    int id;
    protected new void Page_Load(object sender, EventArgs e)
    {
        base.Page_Load(sender, e);
        CurrentNav = "navVendor";
        Title = "Vendor";
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
            BillingLib.Vendor V = BillingLib.Vendor.GetByID(id);
            if (V != null)
            {
                lblID.Text = V.ID.ToString();
                txtName.Text = Cmn.ProperCase(V.Name);
                txtAddress.Text = Cmn.ProperCase(V.Address);
                txtArea.Text = Cmn.ProperCase(V.Area);
                txtCity.Text = Cmn.ProperCase(V.City);
                txtPin.Text = V.PIN.ToString();
                txtMobile.Text = V.Mobile;
                txtTIN.Text = V.TIN;
                chkIsClient.Checked = V.IsClient;
                ddState.SelectedValue = V.State.ToString();
            }
        }
    }

    private void ShowDataTable()
    {
        StringBuilder str = new StringBuilder("<table class='table table-condensed table-striped table-bordered'><tr><th>SNo<th>Name<th>Mobile<th>Address<th>TIN");
        int sNo = 0;
        foreach (BillingLib.Vendor v in BillingLib.Vendor.GetAll())
        {
            str.Append("<tr " + (v.ID == id ? "style='background-color:#FDFDA6;'" : "") + "><td>" + (++sNo) + "<td><a href='/Vendor.aspx?id=" + v.ID + "'>" + v.Name + "</a><td>" + v.Mobile+"<td>" + v.Address + ", " + v.Area + ", " + v.City + ", " + v.PIN+"<td>"+v.TIN);
        }
        lblDataTable.Text = str.Append("</table>").ToString();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        BillingLib.Vendor v = BillingLib.Vendor.GetByID(Cmn.ToInt(lblID.Text));
        if (v == null) v = new BillingLib.Vendor();
        v.ID=Cmn.ToInt(lblID.Text);
        v.Name = txtName.Text;
        v.Address=txtAddress.Text;
        v.Area=txtArea.Text;
        v.City=txtCity.Text;
        v.PIN=Cmn.ToInt(txtPin.Text);
        v.Mobile=txtMobile.Text;
        v.TIN = txtTIN.Text;
        v.IsClient = chkIsClient.Checked;
        v.State = Cmn.ToInt(ddState.SelectedValue);
        v.Save();

        BillingLib.Vendor oldVendor=SessionState.Company.vendorList.FirstOrDefault(m=>m.ID==v.ID);
        if(oldVendor!=null)
            SessionState.Company.vendorList.Remove(oldVendor);

        SessionState.Company.vendorList.Add(v);
        Response.Redirect("/Vendor.aspx");
        lblID.Text = v.ID.ToString();
    }
}