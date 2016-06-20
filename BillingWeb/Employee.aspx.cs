using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Employee : BasePage
{
    int EmployeeID;
    protected new void Page_Load(object sender, EventArgs e)
    {
        base.Page_Load(sender, e);
        CurrentNav = "navEmployee";
        Title = "Employee";
        if (SessionState.IsAdmin == false)
            Response.Redirect("/Login.aspx");
        EmployeeID = Request.QueryString["id"] != null ? Cmn.ToInt(Request.QueryString["id"]) : 0;
        int AddNew = Request.QueryString["new"] != null ? Cmn.ToInt(Request.QueryString["new"]) : 0;
        if (!IsPostBack)
        {
            ddDesignation.Items.Add(new ListItem("--Select--", ""));
            foreach (int Value in Enum.GetValues(typeof(Designation)))
            {
                string Display = Enum.GetName(typeof(Designation), Value);
                ddDesignation.Items.Add(new ListItem(Display, Value.ToString()));
            }
            Common.FillStoreDropdown(ddStore);
            ShowDataTable();
            if(AddNew==1)
                WriteClientScript("$(document).ready(function (){OpenPopup2();});");
            else
            ShowData(EmployeeID);
            Common.FillStoreDropdown(ddStoreEmp, true);
            ddStoreEmp.SelectedIndex = -1;
        }
    }
    private void ShowData(int id)
    {
        if (id != 0)
        {
            BillingLib.Employee E = BillingLib.Employee.GetByID(id);
            if (E != null)
            {
                lblID.Text = E.ID.ToString();
                txtName.Text = E.Name;
                txtAddress.Text = E.Address;
                txtDOJ.Text = E.DOJ.ToString("%d-MMM-yy");
                txtDOB.Text = E.DOB.ToString("%d-MMM-yy");
                txtMobile.Text = E.Mobile;
                txtEmail.Text = E.Email.ToLower();
                txtPassword.Text = E.Password;
                ddStore.SelectedValue = E.StoreID.ToString();
                chkAdmin.Checked = E.IsAdmin == 1 ? true : false;
                ChkDelete.Checked = E.IsDelete == 1 ? true : false;
                ddDesignation.SelectedValue = ((int)E.Designation).ToString();
                WriteClientScript("$(document).ready(function (){OpenPopup2();});");
            }
        }
    }
    private void ShowDataTable()
    {
        StringBuilder str = new StringBuilder("<div style='float:left;width:45%;'><a href='/Employee.aspx?new=1' class='btn btn-xs btn-success'>+New</a></div><div style='text-align:center;font-weight:bold;display:inline;float:left;'>" + ddStore.SelectedItem.Text + "</div><table class='datatable table-bordered' style='font-size:13px'><tr><th>#<th>Store<th>Name<th>DOJ<th>DOB<th>Age<th>Mobile<th>Email<th>Password<th>Address<th>Access");
        int ctr=0;
        foreach (BillingLib.Employee e in BillingLib.Employee.GetAll(0, SessionState.StoreID).Values.OrderBy(m=>m.IsDelete))
        {
            BillingLib.Store S = null;
            Global.StoreList.TryGetValue(e.StoreID, out S);
            str.Append("<tr style='" + (e.IsDelete == 1 ? "text-decoration: line-through;" : "") + " " + (e.ID == EmployeeID ? "background-color:#FDFDA6;" : "") + "'><td>" + ++ctr + "<td>" + (S == null ? "" : S.Name)
                + "<td><a href='/Employee.aspx?id=" + e.ID + "'>" + e.Name + "</a><td style='white-space:nowrap'>"
                + e.DOJ.ToString("%d-MMM-yy") + "<td style='white-space:nowrap'>"
                + e.DOB.ToString("%d-MMM-yy") + "<td>"
                + (DateTime.Today.Year - e.DOB.Year) + "<td>"
                + e.Mobile + "<td>" + e.Email +"<td>" +  (SessionState.IsAdmin ?  e.Password : "") + "<td>"
                + e.Address
                + "<td>" + GetStores(e.ID));
        }
        lblDataTable.Text = str.Append("</table>").ToString();
    }

    private string GetStores(int userId)
    {
        List<BillingLib.Store> SAL = BillingLib.StoreAccess.GetByUserID(userId);
        string storelList = "";
        if (SAL.Count <= 0)
            storelList += "<a href='#' onclick='assignStore(" + userId + ",0)'>+Add</a><br/>";
        foreach (BillingLib.Store SA in SAL)
        {
            storelList += "<a href='#' onclick='assignStore(" + userId + "," + SA.ID + ")'>" + SA.Name + "</a>, ";
        }
        return storelList;
    }

    private string GetTerminals(int userId)
    {
        List<BillingLib.TerminalAccess> TAL = BillingLib.TerminalAccess.GetByUserID(userId);
        string terminalList = "";
        foreach (BillingLib.TerminalAccess TA in TAL)
        {
            BillingLib.Terminal T = null;
            Global.TerminalList.TryGetValue(TA.TerminalID, out T);
            if (T != null)
                terminalList += "<a href='#' onclick='assignTerminal(" + userId + "," + T.ID + ")'>" + T.Name + "</a><a href='#' onclick='RemoveStore(" + userId + "," + T.ID + ")'>(-)</a><br/>";
        }
        return terminalList;
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        BillingLib.Employee emp = BillingLib.Employee.GetByID(Cmn.ToInt(lblID.Text));
        if (emp == null) emp = new BillingLib.Employee();
        emp.ID = Cmn.ToInt(lblID.Text);
        emp.CompanyID = SessionState.CompanyID;
        emp.Name = txtName.Text;
        emp.DOB = Cmn.ToDate(txtDOB.Text);
        emp.DOJ = Cmn.ToDate(txtDOJ.Text);
        emp.Mobile = txtMobile.Text;
        emp.Email = txtEmail.Text.ToLower();
        emp.Address = txtAddress.Text;
        emp.Password = txtPassword.Text.ToLower();
        emp.StoreID = SessionState.StoreID;
        emp.IsAdmin = chkAdmin.Checked ? 1 : 0;
        emp.IsDelete = ChkDelete.Checked ? 1 : 0;
        emp.StoreID = Cmn.ToInt(ddStore.SelectedValue);
        emp.Designation = (Designation)Cmn.ToInt(ddDesignation.SelectedValue);
        emp.Save();
        lblID.Text = emp.ID.ToString();
        Response.Redirect("/Employee.aspx");
    }
}