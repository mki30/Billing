using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Invoice : BasePage
{
    protected new void Page_Load(object sender, EventArgs e)
    {
        base.Page_Load(sender, e);
        int ID = Cmn.ToInt(QueryString("id"));
        CurrentNav = "navInvoice";
        if (!IsPostBack)
        {
            Common.FillVendorDropdown(ddClient,true);
            ddClient.SelectedItem.Text ="--Client--";
            if (ID != 0)
                ShowData(ID);
            ShowInvoiceList();
        }
    }

    private void ShowData(int ID)
    {
        BillingLib.ClientInvoice CI = BillingLib.ClientInvoice.GetByID(ID);
        if (CI != null)
        {
            ddClient.SelectedValue = CI.ClientID.ToString();
            txtBillNo.Text=CI.BillNo.ToString();
            txtPONo.Text=CI.PONo.ToString();
            txtTransport.Text= CI.Transport;
            txtVehicleNo.Text = CI.VehicleNo;
            txtStation.Text=CI.Station;
            txtInvoiceNo.Text = CI.InvoiceNo;
            txtDate.Text = CI.CreatedDate.ToString("dd-MMM-yyyy");
            txtBookNo.Text = CI.BookNo;
            lblID.Text = CI.ID.ToString();

            lblBillNo.Text = CI.BillNo.ToString();
            btnGenerateInvoice.Visible = true;
        }
    }

    private void ShowInvoiceList()
    {
        List<BillingLib.ClientInvoice> cil = BillingLib.ClientInvoice.GetInvoiceLIst().OrderByDescending(m=>m.ID).ToList();
        StringBuilder str = new StringBuilder("<table class='table table-striped table-bordered' style='width:50%;'><tr><th>#<th>Invoice No<th>Book No<th>Date<th>Client<th>Bill No<th>PO No<th>Transport<th>Vehicle No<th>Station<th>Invoice");
        int ctr = 1;
        foreach (BillingLib.ClientInvoice cl in cil)
        {
            BillingLib.Vendor client=SessionState.Company.vendorList.FirstOrDefault(m=>m.ID==cl.ClientID);
            str.Append("<tr><td>"+ ctr++ +"<td>"+cl.InvoiceNo+"<td>"+cl.BookNo+"<td>"+cl.CreatedDate.ToString("%d-MMM-yy")+"<td>" + (client == null ? "" : client.Name) + "<td><a href='Invoice.aspx?id=" + cl.ID + "'>" + cl.BillNo + "</a><td>" + cl.PONo + "<td>" + cl.Transport + "<td>" + cl.VehicleNo + "<td>"
                + cl.Station+"<th><a href='ItemPrint.aspx?billno=" + cl.BillNo+"' target='_blank'>invoice</a>");
        }
        lblInviceList.Text = str.Append("</table>").ToString();
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        BillingLib.ClientInvoice CI = BillingLib.ClientInvoice.GetByID(Cmn.ToInt(lblID.Text));
        if (CI == null)
            CI = new BillingLib.ClientInvoice();
        try
        {
            CI.ClientID = Cmn.ToInt(ddClient.SelectedValue);
            CI.BillNo = Cmn.ToInt(txtBillNo.Text);
            CI.PONo = Cmn.ToInt(txtPONo.Text);
            CI.Transport = txtTransport.Text;
            CI.VehicleNo = txtVehicleNo.Text;
            CI.Station = txtStation.Text;
            CI.InvoiceNo = txtInvoiceNo.Text;
            CI.CreatedDate = Cmn.ToDate(txtDate.Text);
            CI.BookNo = txtBookNo.Text;
            CI.Save();
            lblID.Text = CI.ID.ToString();
            lblBillNo.Text = CI.BillNo.ToString();
            btnGenerateInvoice.Visible = true;
            ShowInvoiceList();
        }
        catch
        {
            lblMsg.Text = "Error";
        }
     }
    protected void btnGenerateInvoice_Click(object sender, EventArgs e)
    {
        if(lblBillNo.Text!="")
        Response.Redirect("ItemPrint.aspx?billno=" + lblBillNo.Text);
    }
}