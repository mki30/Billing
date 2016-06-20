using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ItemPrint : BasePage
{
    protected new void Page_Load(object sender, EventArgs e)
    {
        base.Page_Load(sender, e);
        int BillNo = Cmn.ToInt(QueryString("billno"));
        if (BillNo != 0)
        {
            BillingLib.Billing bill = BillingLib.Billing.GetByBillNo(BillNo,true).FirstOrDefault();
            if (bill != null)
            {
                if(bill.ItemList.Count>0)
                    GetItemTable(bill.ItemList, BillNo);
            }
        }
    }
    private void GetItemTable(List<BillingLib.BillingItem> itelList,int BillNo)
    {
        BillingLib.ClientInvoice ci = BillingLib.ClientInvoice.GetByBillNo(BillNo);
        if (ci != null)
        {
            lblPoNo.Text = ci.PONo.ToString();
            lblTransport.Text = ci.Transport;
            lblVehicleNo.Text = ci.VehicleNo;
            lblStation.Text = ci.Station;
            lblInvoiceNo.Text = ci.InvoiceNo;
            lblBookNo.Text = ci.BookNo;
            lblDate.Text = ci.CreatedDate.ToString("%d-MM-yy");

            BillingLib.Vendor v = SessionState.Company.vendorList.FirstOrDefault(m => m.ID == ci.ClientID);
            if (v != null)
            {
                lblClientName.Text = v.Name.ToUpper();
                lblAddress.Text = v.Address;
                lblClientPhone.Text = v.Mobile;
                lblClientTIN.Text = v.TIN;
            }
        }

        StringBuilder str = new StringBuilder("<table style='width: 800px;' class='gridtable'><tr><th>SNo</th><th>Description of Goods</th><th>Qty+Scheme</th><th>Unit</th><th>MRP</th><th>Price</th><th>VAT%</th><th>VAT</th><th>Amount</th></tr>");
        Dictionary<double, double> listVatSum = new Dictionary<double, double>();
        double totalAmount = 0, totalQty = 0;
        int ctr = 1;
        foreach (BillingLib.BillingItem i in itelList)
        {
            double vatAmount = ((i.MRP * i.Quantity) * i.TaxRate) / 100;
            double Amount = (i.MRP * i.Quantity);
            str.Append("<tr><td>" + ctr++ + "</td><td>" + i.Name + "</td><td>" + i.Quantity + "</td><td>Pcs.</td><td>" + i.MRP + "</td><td>" + i.Cost + "</td><td>" + i.TaxRate + "%</td><td>" + vatAmount + "</td><td>" + Math.Round(Amount,2) + "</td></tr>");

            totalQty += i.Quantity;
            totalAmount += Amount;

            if (listVatSum.ContainsKey(i.TaxRate))
                listVatSum[i.TaxRate] += Amount;
            else
                listVatSum.Add(i.TaxRate, Amount);
        }
        str.Append("<tr><td colspan='8'>Sub Total</td><td>" + Math.Round(totalAmount,2) + "</td></tr>");
        
        double roundOffAmount = Math.Round(totalAmount) - Math.Round(totalAmount,2);

        str.Append("<tr><td colspan='8'><i>Add:Roundedoff(+)</i></td><td>" + Math.Round(roundOffAmount,2) + "</td></tr>");
        str.Append("<tr class='bold'><td colspan='2'>Grand Total</td><td colspan='6'>" + totalQty + " &nbsp;Pcs.</td><td>" + Cmn.toCurrency(totalAmount) + "</td></tr>");

        string vatSummary = "Sale";
        double totalSateWithoutVat = 0,totalVat=0;
        foreach (KeyValuePair<double, double> kvp in listVatSum)
        {
            double vat = ((kvp.Value * kvp.Key) / 100);
            vatSummary += "@" + kvp.Key + "%=" + kvp.Value + " VAT=" + vat + " ";
            totalSateWithoutVat += kvp.Value;
            totalVat += vat;
        }
        vatSummary += "Total Sale=" + Math.Round(totalSateWithoutVat,2) + " VAT=" + Math.Round(totalVat,2);

        str.Append("<tr class='bold'><td colspan='9'>" + vatSummary + "<br/>RUPEES " + Cmn.ConvertNumberToWord(Cmn.ToInt(Math.Round(totalAmount))).ToUpper() + "</td></tr>");
        str.Append("</table>");
        lblItemTable.Text = str.ToString();
    }
}