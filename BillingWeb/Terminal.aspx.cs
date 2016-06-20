using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlServerCe;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BillingLib;

public partial class Terminal : BasePage
{
    protected new void Page_Load(object sender, EventArgs e)
    {
        base.Page_Load(sender, e);
        CurrentNav = "navTerminal";
        if (SessionState.IsAdmin == false)
            Response.Redirect("Login.aspx");
        Title = "Terminal";
        int id = Request.QueryString["id"] != null ? Cmn.ToInt(Request.QueryString["id"]) : 0;
        
        if (!IsPostBack)
        {
            lblStoreID.Text = QueryString("storeID");
            ShowData(id);
            ShowDataTable();
        }
    }
    void ShowData(int id)
    {
        BillingLib.Store S;
        Global.StoreList.TryGetValue(Cmn.ToInt(lblStoreID.Text), out S);
        if (S != null)
            lblStore.Text = S.Name;

        if (id > 0)
        {
            BillingLib.Terminal T = BillingLib.Terminal.GetByID(id);
            if (T != null)
            {
                lblID.Text = T.ID.ToString();
                lblStoreID.Text = T.StoreID.ToString();
                //
                Global.StoreList.TryGetValue(Cmn.ToInt(lblStoreID.Text), out S);


                if (S != null)
                    lblStore.Text = S.Name;
                
                txtName.Text = T.Name;
                txtPrefix.Text = T.Prefix.ToString();
                txtBillNo.Text = T.BillNo.ToString();
                chkIsVertual.Checked=(T.IsVirtual==1?true:false);
            }
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (lblStoreID.Text == "0") return;

        BillingLib.Terminal t = BillingLib.Terminal.GetByID(Cmn.ToInt(lblID.Text));
        if (t == null)
            t = new BillingLib.Terminal();

        t.ID = Cmn.ToInt(lblID.Text);
        t.CompanyID = SessionState.CompanyID;
        t.StoreID = Cmn.ToInt(lblStoreID.Text);
        t.BillNo = Cmn.ToInt(txtBillNo.Text);
        t.Prefix = Cmn.ToInt(txtPrefix.Text);
        t.Name = txtName.Text;
        t.IsVirtual = (chkIsVertual.Checked==true?1:0);
        t.Save();

        lblID.Text = t.ID.ToString();
        ShowDataTable();
    }

    private void ShowDataTable()
    {
        List<BillingLib.Terminal> list = BillingLib.Terminal.GetByCompanyAndStore(SessionState.CompanyID, Cmn.ToInt(lblStoreID.Text));
        String str = "<table table class='table-condensed table-striped table-bordered'><tr><th>ID<th>Company<th>Store<th>Term. Name<th>Bill No<th>Prefix<th>Last Access<th>User<th>S.Version";
        foreach (BillingLib.Terminal T in list)
        {
            BillingLib.Company comp = null;
            Global.listCompany.TryGetValue(T.CompanyID, out comp);
            BillingLib.Store store = BillingLib.Store.GetByID(T.StoreID);
            str += "<tr><td>" + T.ID.ToString() + "<td>" + (comp == null ? "" : comp.Name) + "<td>" + (store == null ? "" : store.Name) + "<td><a href='/Terminal.aspx?id=" + T.ID + "'>" + T.Name + "</a><td>" + T.BillNo
                + "<td>" + T.Prefix + "<td>" + (T.LastAccess==Cmn.MinDate?"":Cmn.ToDate(T.LastAccess).ToString("dd-MMM-yy hh:mm")) + "<td>" + T.User + "<td>" + T.SoftwareVersion;
        }
        str += "</table>";
        lblDataTable.Text = str;
    }
}