using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlServerCe;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BillingLib;
using System.Text;

public partial class Menu : BasePage
{
    protected new void Page_Load(object sender, EventArgs e)
    {
        CurrentNav = "navMenu";
        base.Page_Load(sender, e);
        Title = "Menu";
        if (SessionState.IsAdmin == false)
            Response.Redirect("/Login.aspx");

        int id = Request.QueryString["id"] != null ? Cmn.ToInt(Request.QueryString["id"]) : 0;
        int parentID = Request.QueryString["parentid"] != null ? Cmn.ToInt(Request.QueryString["parentid"]) : 0;
        lblParentID.Text = parentID.ToString();
        if (!IsPostBack)
        {
            ShowData(id);
        }
    }
    void ShowData(int id)
    {
        if (id != 0)
        {
            BillingLib.Menu menu = BillingLib.Menu.GetAll(id).First();
            if (menu != null)
            {
                lblID.Text = menu.ID.ToString();
                lblParentID.Text = menu.ParentID.ToString();
                txtName.Text = menu.Name.ToString();
            }
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        BillingLib.Menu m = BillingLib.Menu.GetByID(Cmn.ToInt(lblID.Text));
        if (m == null) m = new BillingLib.Menu();
        m.ID = Cmn.ToInt(lblID.Text);
        m.ParentID = Cmn.ToInt(lblParentID.Text);
        m.Name = txtName.Text;
        m.UrlName = Cmn.GenerateSlug(m.Name);
        m.Save();
        txtName.Text = "";
    }
    protected void btnNew_Click(object sender, EventArgs e)
    {
        Response.Redirect("/Menu.aspx");
    }

    //private void ShowDataTable()
    //{
    //    List<BillingLib.Item> list = SessionState.Company.ItemList.OrderBy(m => m.Name).ToList();
    //    List<BillingLib.ItemMenuLink> listMenu = BillingLib.ItemMenuLink.GetAll();
    //    List<BillingLib.ItemStorePrice> listStorePrice = BillingLib.ItemStorePrice.GetAll(0,SessionState.StoreID);

    //    StringBuilder str = new StringBuilder("<table  id='DataTable' class='table-striped table-hover table-bordered' style='width:100%'><tr><th>#<th>PLU<th>Name<th>Tax<th>Cost<th title='Weight'>Wt.<th>MRP<th>Brand<th>Menu<th>Stock");
    //    int ctr = 0;
    //    foreach (Item i in list)
    //    {
    //        if (listMenu.FirstOrDefault(m => m.ItemID == i.ID) == null)
    //            continue;
    //        ctr++;
    //        double stock = 0;

    //        string menuItems = "";
    //        foreach (BillingLib.ItemMenuLink m in listMenu.Where(m => m.ItemID == i.ID))
    //        {
    //            BillingLib.Menu menu = SessionState.Company.listMenu.FirstOrDefault(n => n.ID == m.MenuID);
    //            if (menu != null)
    //            {
    //                menuItems += (menu.Parent != null ? menu.Parent.Name : "") + "-" + menu.Name + "<a href='#' onclick='DeleteMenuItemLink("+m.ID+")'> X</a><br/>";
    //            }
    //        }

    //        string thisStoreCost = "", thisStoreMRP = "";
    //        BillingLib.ItemStorePrice ISP = listStorePrice.FirstOrDefault(m => m.ItemID == i.ID && m.StoreID == SessionState.StoreID);
    //        if (ISP != null)
    //        {
    //            //i.Cost = ISP.Rate;
    //            //i.MRP = ISP.MRP;
    //            thisStoreCost = ISP.Rate.ToString();
    //            thisStoreMRP = ISP.MRP.ToString();
    //        }

    //        BillingLib.Brand Brand = SessionState.Company.BrandList.FirstOrDefault(m => m.ID == i.BrandID);

    //        str.Append("<tr id='tr" + i.ID + "'><td>" + ctr
    //            + "<td title='" + i.ID + "'>" + i.PLU
    //            + "<td><a href='#' onclick='LinkItemWithMenu(" + i.ID + ",\"" + i.Name + "\")'>"
    //            + i.Name + "</a><td>"
    //            + (i.TaxRate == 0 ? "" : i.TaxRate.ToString())
    //            + "<td>" + i.Cost + (i.RateType != UnitType.none ? "/" + i.RateType : "") + "<br/>" + (thisStoreCost != "" ? thisStoreCost + (i.RateType != UnitType.none ? "/" + i.RateType : "") : "")
    //            + "<td>" + (i.Weight == 0 ? "" : i.Weight.ToString()) + (i.WeightType != UnitType.none ? i.WeightType.ToString() : "")
    //            + "<td>" + i.MRP + "<br/>" + thisStoreMRP
    //            + "<td>" + (Brand != null ? Brand.Name : "")
    //            + "<td> " + menuItems +
    //            "<td><a href='/Stock.aspx?id=" + i.ID + "' target='_blank'>" + stock);
    //    }
    //    lblDataTable.Text = str.Append("</table>").ToString();
    //}
}