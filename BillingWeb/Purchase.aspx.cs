using System;

public partial class Company : BasePage
{
    int id;
    protected new void Page_Load(object sender, EventArgs e)
    {
        base.Page_Load(sender, e);
        Title = "Purchase";
        id = Request.QueryString["id"] != null ? Cmn.ToInt(Request.QueryString["id"]) : 0;

        int a = (int)reportType;
        if ((int)reportType == 0 || (int)reportType == 1 || (int)reportType == 2)
        {
            Common.FillVendorDropdown(ddVendor);
            if (ddVendor.Items.Count > 0)
                ddVendor.SelectedValue = VendorID.ToString();
        }

        CurrentNav = "navPurchase";
        RecordType recordType = (RecordType)reportType;
        ReturnControl1.recordType = recordType;
        switch (recordType)
        {
            case RecordType.Return:
                newLink.InnerHtml = "<i class='glyphicon glyphicon-plus'></i> Return";
                if (VendorID != 0)
                    newLink.Attributes.Add("onclick", "ShowDialog(true,'Return')");
                else
                    newLink.Attributes.Add("onclick", "alert('Please select vendor')");
                
                    CurrentNav = "navReturn";
                Title = "Return";
                ThePageScript = ";var CurrentNav='" + CurrentNav + "',RecordType=" + (int)RecordType.Return + ";";
                ddVendor.Visible = true;
                break;
            case RecordType.Damage:
                newLink.InnerHtml = "<i class='glyphicon glyphicon-plus'></i> Damage";
                newLink.Attributes.Add("onclick", "ShowDialog(true,'Damage')");
                CurrentNav = "navDamage";
                Title = "Damage";
                ThePageScript = ";var CurrentNav='" + CurrentNav + "',RecordType=" + (int)RecordType.Damage + ";";
                ddVendor.Visible = false;
                break;
            case RecordType.TransferIn:
                newLink.Visible = false;
                CurrentNav = "navIn";
                Title = "Transfer In";
                ddVendor.Visible = false;
                ThePageScript = ";var CurrentNav='" + CurrentNav + "',RecordType=" + (int)RecordType.TransferIn + ",Store=" + SessionState.StoreID + ";";
                break;
            case RecordType.TransferOut:
                newLink.InnerHtml = "<i class='glyphicon glyphicon-plus'></i> Trans Out";
                newLink.Attributes.Add("onclick", "ShowDialog(true,'Transfer Out')");
                CurrentNav = "navOut";
                Title = "Transfer Out";
                ddVendor.Visible = false;
                ThePageScript = ";var CurrentNav='" + CurrentNav + "',RecordType=" + (int)RecordType.TransferOut + ",Store=" + SessionState.StoreID + ";";
                break;
            default:
                if (VendorID != 0)
                {
                    newLink.Target = "_blank";
                    newLink.HRef = "/Inventory.aspx?VendorID=" + VendorID;
                }
                else
                {
                    newLink.Attributes.Add("onclick", "alert('Please select vendor')");
                }
                lblDataTable.InnerHtml = "<strong>Purchase " + FilterHeader + "</strong>" + BillingLib.Purchase.GetHTMLList(SessionState.Company, FilterDateFrom, FilterDateTo, VendorID, SessionState.StoreID, true);
                break;
        }
        ltDateFilter.Text = GetDateFilterLinks(0);
    }
}