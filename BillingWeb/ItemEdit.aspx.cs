using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ItemEdit : BasePage
{
    protected new void Page_Load(object sender, EventArgs e)
    {
        base.Page_Load(sender, e);

        Title = "Items";
        int id = Request.QueryString["id"] != null ? Cmn.ToInt(Request.QueryString["id"]) : 0;

        if (!IsPostBack)
            ShowData(id);
    }

    private void ShowData(int id)
    {
        FillTaxDropdown();
        Common.FillBrandDropdown(ddBrand);
        Common.FillUnitDropdown(ddRateType);
        Common.FillUnitDropdown(ddWeightType);
        Common.FillMenu(ddCategory);

        if (id != 0)
        {
            BillingLib.Item item = BillingLib.Item.GetAll(id).First();
            if (item != null)
            {
                lblID.Text = item.ID.ToString();
                txtName.Text = item.Name;
                txtWeight.Text = item.Weight.ToString();
                ddRateType.SelectedValue = ((int)item.RateType).ToString();
                ddWeightType.SelectedValue = ((int)item.WeightType).ToString();
                txtPluCode.Text = item.PLU;
                ddTaxRate.SelectedValue = item.TaxRateID.ToString();

                ddBrand.SelectedValue = item.BrandID.ToString();
                txtCost.Text = item.Cost.ToString();
                txtMRP.Text = item.MRP.ToString();
                txtReOrderLevel.Text = item.ReOrderLevel.ToString();
                txtCostFranchise.Text = item.CostFranchise.ToString();
                chkItemHidden.Checked = item.IsHidden;
                ddToAdjust.SelectedValue = item.ToAdjust.ToString();

                List<BillingLib.ItemMenuLink> iml = BillingLib.ItemMenuLink.GetByItemID(item.ID);
                if (iml.Count > 0)
                {
                    ddCategory.SelectedValue = iml.First().MenuID.ToString();
                }

                
                lblStore.Text = " Store Pricing : " + BillingLib.Store.GetByID(SessionState.StoreID).Name;
                BillingLib.ItemStorePrice ISP = BillingLib.ItemStorePrice.GetByItemID(Cmn.ToInt(lblID.Text), SessionState.StoreID);
                if (ISP != null)
                {
                    txtRateStore.Text = ISP.Rate.ToString();
                    txtMRPStore.Text = ISP.MRP.ToString();
                }
                lblLink.Text = "&nbsp;<a href='#' class='linkGroup' onclick='OpenlinkDilauge(" + id + ");'>link</a>";
            }
            ltImageSearch.Text = "<a href='https://www.google.co.in/search?as_st=y&tbm=isch&hl=en&as_q=" + (item.BrandID != 0 ? ddBrand.SelectedItem.Text + " " : "") + item.Name + "&as_epq=&as_oq=&as_eq=&cr=&as_sitesearch=&safe=images&tbs=iar:s' target='_blank'>Google Image</a>";
            ltImage.Text = "<img height=150px' src='/Data.aspx?Action=Image&Data1=" + id + "&t=" + DateTime.Now.Ticks + "' /><br />" +
            "<span id='lblImageUpload'><form action='/data.aspx?Action=FileUpload&Data1=" + id + "' class='dropzone' id='my-dropzone'></form></span>";

            WriteClientScript("try{parent.EditLoaded(" + id + ")}catch(e){}");
        }
    }
    private void FillTaxDropdown()
    {
        ddTaxRate.Items.Clear();
        ddTaxRate.Items.Add(new ListItem("-Select-", "-1"));
        foreach (BillingLib.Tax t in Global.listTaxRate.Values)
        {
            ddTaxRate.Items.Add(new ListItem(t.Rate.ToString(), t.ID.ToString()));
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        BillingLib.Item item = BillingLib.Item.GetByID(Cmn.ToInt(lblID.Text));

        item.ID = Cmn.ToInt(lblID.Text);
        item.PLU = txtPluCode.Text.Trim();
        item.Name = txtName.Text.Trim().ToUpper();

        item.Cost = Cmn.ToDbl(txtCost.Text);
        item.CostFranchise = Cmn.ToDbl(txtCostFranchise.Text);
        item.RateType = (UnitType)Cmn.ToInt(ddRateType.Text);

        item.Weight = Cmn.ToDbl(txtWeight.Text);
        item.WeightType = (UnitType)Cmn.ToInt(ddWeightType.Text);

        item.MRP = Cmn.ToDbl(txtMRP.Text);
        item.TaxRateID = Cmn.ToInt(ddTaxRate.SelectedValue);
        item.TaxRate = Cmn.ToDbl(ddTaxRate.SelectedItem.Text);
        item.BrandID = Cmn.ToInt(ddBrand.SelectedValue);
        item.ReOrderLevel = Cmn.ToInt(txtReOrderLevel.Text);

        item.IsHidden = chkItemHidden.Checked;
        item.ToAdjust = Cmn.ToInt(ddToAdjust.SelectedValue);
        item.Save();
        SessionState.Company.UpdateItem(item);
        BillingLib.ItemMenuLink mIl = new BillingLib.ItemMenuLink()
        {
            ItemID = item.ID,
            MenuID = Cmn.ToInt(ddCategory.SelectedValue)
        };
        mIl.Link();

        Common.ResetForm(this.Form);
        WriteClientScript("try{parent.EditLoaded(" + item.ID + ")}catch(e){}");
        lblID.Text = "0";
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        BillingLib.ItemStorePrice ISP = BillingLib.ItemStorePrice.GetByItemID(Cmn.ToInt(lblID.Text), SessionState.StoreID);
        if (ISP == null)
            ISP = new BillingLib.ItemStorePrice();

        ISP.ItemID = Cmn.ToInt(lblID.Text);
        ISP.StoreID = SessionState.StoreID;
        ISP.Rate = Cmn.ToDbl(txtRateStore.Text);
        ISP.MRP = Cmn.ToDbl(txtMRPStore.Text);
        ISP.Save();
        WriteClientScript("try{parent.EditLoaded(" + lblID.Text + ")}catch(e){}");
    }
}