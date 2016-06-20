using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using BillingLib;
using System.IO;

public partial class Items : BasePage
{
    protected new void Page_Load(object sender, EventArgs e)
    {
        CurrentNav = "navItems";
        base.Page_Load(sender, e);
        Title = "Items - " + SessionState.CompanyName;
        int id = Request.QueryString["id"] != null ? Cmn.ToInt(Request.QueryString["id"]) : 0;
        int brandID = Request.QueryString["brandid"] != null ? Cmn.ToInt(Request.QueryString["brandid"]) : 0;
        int showAll = Request.QueryString["showall"] != null ? Cmn.ToInt(Request.QueryString["showall"]) : 0;
        int adjust = Request.QueryString["adjust"] != null ? Cmn.ToInt(Request.QueryString["adjust"]) : 0;

        if (!IsPostBack)
        {
            if (showAll != 0)
            {
                chkShowAll.Checked = true;
            }
            Common.FillBrandDropdown(ddBrand, "-All Brands-");
            ShowDataTable(showAll,adjust);

            Common.FillBrandDropdown(ddBrandMultilink,"-Select-");
            Common.FillMenu(ddCategoryMultilink);
        }
        ddBrand.SelectedValue = brandID.ToString();
        ddToAdjust.SelectedValue = adjust.ToString();
    }

    private void ShowDataTable(int ShowAll,int Adjust)
    {
        List<BillingLib.Item> list = BillingLib.Item.GetAll(0, BrandID, (ShowAll == 0 ? true : false), Adjust).OrderBy(m => m.Name).ToList();
        List<BillingLib.ItemStorePrice> listStorePrice = BillingLib.ItemStorePrice.GetAll(0, SessionState.StoreID);

        StringBuilder str = new StringBuilder("<table  id='DataTable' class='table-striped table-bordered csvTable' style='width:100%;'><tr><th>#<th><input type='checkbox'  id='chkAll' onclick='SelectAllCheckBox(this)'></span><th>PLU<th>Name<th>Tax<th>Cost<th title='Weight'>Wt.<th>MRP");
        int ctr = 0;
        foreach (Item i in list)
        {
            ctr++;
            string thisStoreCost = "", thisStoreMRP = "";
            BillingLib.ItemStorePrice ISP = listStorePrice.FirstOrDefault(m => m.ItemID == i.ID && m.StoreID == SessionState.StoreID);
            if (ISP != null)
            {
                thisStoreCost = ISP.Rate.ToString();
                thisStoreMRP = ISP.MRP.ToString();
            }
            BillingLib.Brand Brand = SessionState.Company.BrandList.FirstOrDefault(m => m.ID == i.BrandID);
            bool imageexist = File.Exists(@"c:\Billing\Item_Images\" + i.ID + ".jpg");

            str.Append("<tr id='tr" + i.ID + "'><td " + (!imageexist ? "style='background:pink'" : "") + ">" + ctr
                + "<td><input type='checkbox' value=" + i.ID + " id='chk" + i.ID + "' onclick='PushId(this)'>"
                + "<td title='" + i.ID + "'>" + i.PLU
                + "<td><a href='#' onclick='ShowEditItem(" + i.ID + ",this)'>"
                + i.Name + "</a> " + (Brand != null ? Brand.Name : "<span style='color:red;'>No Brand</span>")
                + (i.GroupID != 0 ? "<span style='color:red;'> Grp" + i.GroupID + "</span>" : "")
                + "<td>" + (i.TaxRate == 0 ? "" : i.TaxRate.ToString())
                + "<td>" + i.Cost + (i.RateType != UnitType.none ? "/" + i.RateType : "") + "<br/>" + (thisStoreCost != "" ? thisStoreCost + (i.RateType != UnitType.none ? "/" + i.RateType : "") : "")
                + "<td>" + (i.Weight == 0 ? "" : i.Weight.ToString()) + (i.WeightType != UnitType.none ? i.WeightType.ToString() : "")
                + "<td>" + i.MRP +(thisStoreMRP!="0"?"<br/>"+thisStoreMRP:""));
        }
        lblDataTable.Text = str.Append("</table>").ToString();
    }

    protected void lblUpdateHidden_Click(object sender, EventArgs e)
    {
        List<BillingLib.Inventory> listInventory = BillingLib.Inventory.GetInventory(0, 0, RecordType.Purchase, FilterDateFrom, FilterDateTo, 0, 0);
        DatabaseCE db = new DatabaseCE();
        try
        {
            db.RunQuery("Update item set IsHidden=1");
            db.RunQuery("Update item set IsHidden=0 where id IN(SELECT DISTINCT ItemID FROM BillingItem) ");
            db.RunQuery("Update item set IsHidden=0 where id IN(SELECT DISTINCT ItemID FROM Inventory) ");
            lblDataTable.Text = "done";
        }
        finally
        {
            db.Close();
        }
    }
}