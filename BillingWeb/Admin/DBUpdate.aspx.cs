using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using System.Text;
using System.Data.SqlServerCe;
using BillingLib;
using System.Net;

public partial class Admin_DBUpdate : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //DBCheck.UpdateDB("Billing");
    }
    protected void btnReadCsv_Click(object sender, EventArgs e)
    {
        string vError = string.Empty;
        DatabaseCE db = new DatabaseCE();

        string[] Data = File.ReadAllLines(Server.MapPath(@"~\ItemList.csv"));
        foreach (string s in Data)
        {
            string str = s;
            string[] Fields = s.Split(',');
            try
            {
                BillingLib.Item i = BillingLib.Item.GetByPLU(Fields[0].Trim());
                if (i == null)
                {
                    Dictionary<string, object> data = new Dictionary<string, object>();
                    data.Add("ID", 0);
                    data.Add("PLUCode", Fields[0].Trim());
                    data.Add("Name", Fields[1].Trim());
                    data.Add("MRP", Fields[2]);
                    data.Add("Cost", Fields[2]);
                    TableID.Save("Item", data, db);
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }
        db.Close();
    }

    //protected void btnImportPurchase_Click(object sender, EventArgs e)
    //{
    //    string vError = string.Empty;
    //    DatabaseCE db = new DatabaseCE();
    //    string[] Data = File.ReadAllLines(Server.MapPath(@"~\Purchase2.csv"));

    //    string vendorID = Data[8].Split(',')[2];
    //    string vendorName = Data[8].Split(',')[3];
    //    BillingLib.Vendor v = BillingLib.Vendor.GetByID(Cmn.ToInt(vendorID));
    //    if (v == null)
    //    {
    //        v = new BillingLib.Vendor();
    //        v.Name = vendorName;
    //        v.Save();
    //    }

    //    BillingLib.Purchase P = new BillingLib.Purchase();
    //    P.PurchaseDate = DateTime.Now;
    //    P.VendorID = v.ID;
    //    P.Amount = 0;
    //    P.Save();

    //    int ctr = 0;
    //    foreach (string s in Data)
    //    {
    //        if (ctr <= 13 || s.Contains("Grand Total"))
    //        {
    //            ctr++;
    //            continue;
    //        }
    //        string[] Fields = s.Split(',');
    //        try
    //        {
    //            string plu = Fields[4];
    //            BillingLib.Item I;
    //            Global.ItemListByPLU.TryGetValue(plu, out I);

    //            BillingLib.Inventory i = new BillingLib.Inventory();
    //            i.PurchaseID = P.ID;
    //            i.ItemID = (I != null ? I.ID : 0);
    //            i.Quantity = Cmn.ToInt(Fields[6].Split('.')[0]);
    //            i.TotalAmount = Cmn.ToInt(Fields[7].Split('.')[0]);
    //            i.Save();

    //            if (Fields[8] != "")
    //            {
    //                BillingLib.Inventory i2 = new BillingLib.Inventory();
    //                i2.PurchaseID = P.ID;
    //                i2.ItemID = (I != null ? I.ID : 0);
    //                i2.Quantity = -Cmn.ToInt(Fields[8].Split('.')[0]);
    //                i2.TotalAmount = -Cmn.ToInt(Fields[9].Split('.')[0]);
    //                i2.Save();
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            Response.Write(ex.Message);
    //        }
    //        ctr++;
    //    }
    //    db.Close();
    //}

    protected void btnResetGlobal_Click(object sender, EventArgs e)
    {
        Global.LoadGlobal();
    }

    protected void btnUpdatePLU_Click(object sender, EventArgs e)
    {
        List<Item> list = Item.GetAll();
        foreach (Item i in list)
        {
            if (i.PLU.Length < 4)
            {
                i.PLU = i.PLU.PadLeft(4, '0');
                i.Save();
            }
        }
    }
    //protected void btnUpdateSNo_Click(object sender, EventArgs e)
    //{
    //    List<Inventory> list = Inventory.GetByPurchaseID(Cmn.ToInt(txtPurchaseID.Text)).OrderBy(m => m.ID).ToList();
    //    int ctr = 0;
    //    foreach (Inventory i in list)
    //    {
    //        i.SNo = ++ctr;
    //        i.Save();
    //    }
    //}
    //protected void btnUpdateItenmID_Click(object sender, EventArgs e)
    //{
    //    List<BillingItem> bilist = BillingItem.GetAll();
    //    List<Item> ilist = Item.GetAll();
    //    DatabaseCE db = new DatabaseCE();
    //    try
    //    {
    //        foreach (BillingItem bi in bilist)
    //        {
    //            List<Item> lst = ilist.Where(m => m.PLU == bi.PLU).ToList();
    //            if (lst.Count > 0)
    //            {
    //                Item i = ilist.Where(m => m.PLU == bi.PLU).First();
    //                if (bi.ItemID == 0)
    //                {
    //                    bi.ItemID = i.ID;
    //                    bi.SaveBillingItem(db);
    //                }
    //            }
    //        }
    //    }
    //    catch { }
    //    finally { db.Close(); }
    //}

    //protected void btnUpdateBilling_Click(object sender, EventArgs e)
    //{
    //    //    DatabaseCE db = new DatabaseCE();

    //    //    try
    //    //    {
    //    //        string error = "";
    //    //        IDataReader dr = db.GetDataReader("Select * from Billing", ref error);
    //    //        while (dr.Read())
    //    //        {
    //    //            int id = Cmn.ToInt(dr["ID"]);
    //    //            int paidBy = Cmn.ToInt(dr["PaidBy"]);
    //    //            db.RunQuery("update Billing set PaidCash=" + (paidBy == 0 ? dr["PaidAmount"].ToString() : "0") + ",PaidCard=" + (paidBy != 0 ? dr["PaidAmount"].ToString() : "0") + " where id="+ id);

    //    //        }
    //    //    }
    //    //    finally
    //    //    {
    //    //        db.Close();
    //    //    }
    //}

    //protected void btnUpdateItemNamesInInventory_Click(object sender, EventArgs e)
    //{
    //    List<BillingLib.Inventory> il = BillingLib.Inventory.GetAll();
    //    foreach (BillingLib.Inventory i in il)
    //    {
    //        BillingLib.Item item = Global.ItemListByID.Values.Where(m => m.ID == i.ItemID).FirstOrDefault();
    //        if (item != null)
    //        {
    //            if (i.Name == "")
    //            {
    //                i.Name = item.Name;
    //                i.PLU = item.PLU;
    //            }
    //            i.Save();
    //        }
    //    }
    //}
    //protected void btnUpdateInventory_Click(object sender, EventArgs e)
    //{
    //    List<BillingLib.Purchase> PL = BillingLib.Purchase.GetAll().Where(m => m.ItemIncludesTax == 1).ToList();
    //    List<BillingLib.Inventory> IL = BillingLib.Inventory.GetAll();
    //    foreach (BillingLib.Purchase p in PL)
    //    {
    //        List<BillingLib.Inventory> IL2 = IL.Where(m => m.PurchaseID == p.ID).ToList();
    //        foreach (BillingLib.Inventory i in IL2)
    //        {
    //            i.ItemIncludesTax = p.ItemIncludesTax;
    //            i.Save();
    //        }
    //    }
    //}

    //protected void btnUpdateBillAmount_Click(object sender, EventArgs e)
    //{
    //    List<Billing> listBill = Billing.GetAll(new DateTime(2015, 9, 10), DateTime.Now).OrderBy(m => m.BillDate).ToList();
    //    List<BillingItem> listitems = BillingItem.GetAll();

    //    StringBuilder str = new StringBuilder("<table>");
    //    int ctr = 0;
    //    foreach (Billing b in listBill)
    //    {
    //        double itemtotal = listitems.Where(m => m.BillingID == b.ID).Sum(m => m.Amount);

    //        if (b.PaidCard == 0 && itemtotal != 0 && Math.Round(itemtotal, MidpointRounding.AwayFromZero) != Math.Round(b.PaidCard + b.PaidCash, MidpointRounding.AwayFromZero))
    //        {
    //            ctr++;
    //            str.Append("<tr><td>" + ctr + "<td>" + b.ID + "<td>" + b.BillNo + "<td>"
    //                + b.BillDate.ToString("dd-MMM") + "<td>"
    //                + b.PaidCash + "<td>"
    //                + b.PaidCard + "<td>"
    //                + itemtotal);

    //            b.PaidCash = Math.Round(itemtotal, MidpointRounding.AwayFromZero);
    //            b.Save();
    //        }
    //    }
    //    str.Append("</table>");
    //    ltData.Text = str.ToString();
    //}

    //protected void btnUpdateDateInInventory_Click(object sender, EventArgs e)
    //{
    //    List<Purchase> listPurchase = Purchase.GetAll();
    //    List<Inventory> listitems = Inventory.GetAll();

    //    DatabaseCE db = new DatabaseCE();
    //    try
    //    {
    //        foreach (Inventory i in listitems)
    //        {
    //            BillingLib.Purchase p = listPurchase.Where(m => m.ID == i.PurchaseID).FirstOrDefault();
    //            if (p!=null)
    //                db.RunQuery("Update Inventory set RcNo='" + p.RCNo + "', Date='" + p.PurchaseDate.ToString("dd-MMM-yyyy") + "',StoreID="+p.StoreID+" where id=" + i.ID);

    //        }
    //    }
    //    finally
    //    {
    //        db.Close();
    //    }

    //}
    //protected void btnUpdateStoreID_Click(object sender, EventArgs e)
    //{
    //    DatabaseCE db = new DatabaseCE();
    //    try
    //    {
    //        db.RunQuery("Update EmployeeAttendance set StoreID=3");
    //        db.RunQuery("Update PurchasePayment set StoreID=3");
    //    }
    //    finally
    //    {
    //        db.Close();
    //    }
    //}
    //protected void btnUpdateDateInReturn_Click(object sender, EventArgs e)
    //{
    //    List<Inventory> listitems = Inventory.GetAll();

    //    DatabaseCE db = new DatabaseCE();
    //    try
    //    {
    //        foreach (Inventory i in listitems)
    //        {
    //            if(i.RecordType!=0)
    //                db.RunQuery("Update Inventory set Date='" + i.LUDate.ToString("dd-MMM-yyyy") + "' where id="+i.ID+"");
    //        }
    //    }
    //    finally
    //    {
    //        db.Close();
    //    }
    //}
    //protected void btnUpdateBrandCompany_Click(object sender, EventArgs e)
    //{
    //    List<Brand> list = Brand.GetAll();
    //    DatabaseCE db = new DatabaseCE();
    //    try
    //    {
    //        foreach (Brand b in list)
    //        {
    //            db.RunQuery("Update Item set CompanyID=1;");
    //            db.RunQuery("Update Customer set CompanyID=1;");
    //            db.RunQuery("Update Menu set CompanyID=1;");
    //            db.RunQuery("Update Brand set CompanyID=1;");
    //            db.RunQuery("Update PurchasePayment set CompanyID=1;");
    //            db.RunQuery("Update Bank set CompanyID=1;");
    //        }
    //    }
    //    finally
    //    {
    //        db.Close();
    //    }
    //}

    protected void btnUpdateBillingItem_Click1(object sender, EventArgs e)
    {
        List<BillingItem> listitems = BillingItem.GetAll();
        List<Item> items = Item.GetAll();

        DatabaseCE db = new DatabaseCE();
        try
        {
            foreach (BillingItem b in listitems)
            {
                Item i = items.Where(m => m.ID == b.ItemID).FirstOrDefault();
                if (i != null)
                {
                    db.RunQuery("Update BillingItem set TaxPerc=" + i.TaxRate + " where id=" + b.ID);
                }
            }
        }
        finally
        {
            db.Close();
        }
    }

    //protected void btnUpdateInventoryStores_Click(object sender, EventArgs e)
    //{
    //    List<BillingLib.Purchase> purchaseList = BillingLib.Purchase.GetAll();
    //    List<Inventory> inventoryList = Inventory.GetAll();

    //    DatabaseCE db = new DatabaseCE();
    //    try
    //    {
    //        foreach (Inventory i in inventoryList)
    //        {
    //            Purchase p = purchaseList.Where(m => m.ID == i.PurchaseID).FirstOrDefault();
    //            if (p != null)
    //            {
    //                db.RunQuery("Update Inventory set StoreID=" + p.StoreID + " where id=" + i.ID);
    //            }
    //        }
    //    }
    //    finally
    //    {
    //        db.Close();
    //    }
    //}

    protected void btnCombineCustomers_Click(object sender, EventArgs e)
    {
        DatabaseCE db = new DatabaseCE();
        List<Customer> list = Customer.GetAll();
        Dictionary<string, Customer> custList = new Dictionary<string, Customer>();

        StringBuilder str = new StringBuilder("<table><tr><th>ID<th>Mobile<th>Name<th>Address");
        int ctr = 0;
        try
        {
            foreach (Customer cust in list)
            {
                if (cust.Mobile != "")
                {
                    Customer existingCustomer = new Customer();
                    custList.TryGetValue(cust.Mobile, out existingCustomer);
                    if (existingCustomer == null)
                    {
                        custList.Add(cust.Mobile, cust);
                    }
                    else
                    {
                        DeleteCustomer(cust, db);
                        str.Append("<tr><td>" + cust.ID + "<td>" + cust.Mobile + "<td>" + cust.Name + "<td>" + cust.Address + " " + cust.Area + " " + cust.City);
                        ctr++;
                    }
                }
                else
                {
                    DeleteCustomer(cust, db);
                    str.Append("<tr><td>" + cust.ID + "<td>" + cust.Mobile + "<td>" + cust.Name + "<td>" + cust.Address + " " + cust.Area + " " + cust.City);
                    ctr++;
                }

            }
        }
        catch { }
        finally
        {
            db.Close();
        }

        StringBuilder finalList = new StringBuilder("<table><tr><th>ID<th>Mobile<th>Name<th>Address");
        int ctr2 = 0;
        foreach (KeyValuePair<string, Customer> kvp in custList)
        {
            finalList.Append("<tr><td>" + kvp.Value.ID + "<td>" + kvp.Value.Mobile + "<td>" + kvp.Value.Name + "<td>" + kvp.Value.Address + " " + kvp.Value.Area + " " + kvp.Value.City);
            ctr2++;
        }
        ltData.Text = "<h3>Final Customers(" + ctr2 + ")</h3><br/>" + finalList.ToString() + "</Table><h3>Deleted(" + ctr + ")</h3><br/>" + str.ToString() + "</Table>";
    }

    private void DeleteCustomer(Customer cust, DatabaseCE db)
    {
        try
        {
            db.RunQuery("Delete from Customer where id=" + cust.ID);
        }
        catch
        {
        }
    }

    protected void btnUpdateItemStock_Click(object sender, EventArgs e)
    {
        // Stock.GetCurrentStock(3, new DateTime(2015, 8, 1), new DateTime(2015, 8, 31), true);
        //Stock.GetCurrentStock(3, new DateTime(2015, 9, 1), new DateTime(2015, 9, 30), true);
        //Stock.GetCurrentStock(3, new DateTime(2015, 10, 1), new DateTime(2015, 10, 31), true);
        //Stock.GetCurrentStock(3, new DateTime(2015,12,1), new DateTime(2015,12,31), true);
    }

    protected void btnUpdateTaxRates_Click(object sender, EventArgs e)
    {
        DatabaseCE db = new DatabaseCE();
        List<Item> list = Item.GetAll();
        try
        {
            foreach (Item i in list)
            {
                BillingLib.Tax t;
                Global.listTaxRate.TryGetValue(i.TaxRateID, out t);
                if (t != null)
                    db.RunQuery("Update Item set TaxRate=" + t.Rate + " where id=" + i.ID);
                else
                    db.RunQuery("Update Item set TaxRate=0 where id=" + i.ID);
            }
        }
        catch
        {
        }
        finally
        {
            db.Close();
        }
    }
    protected void btnImportCategoryCSV_Click(object sender, EventArgs e)
    {
        string vError = string.Empty;
        DatabaseCE db = new DatabaseCE();

        string[] Data = File.ReadAllLines(Server.MapPath(@"~\ProductList.csv"));
        foreach (string s in Data)
        {
            string str = s;
            string[] Fields = s.Split(',');
            try
            {
                BillingLib.Menu m = BillingLib.Menu.GetByID(Cmn.ToInt(Fields[0].Trim()));
                if (m == null)
                {
                    Dictionary<string, object> data = new Dictionary<string, object>();
                    data.Add("ID", Fields[0].Trim());
                    data.Add("ParentID", Fields[1].Trim());
                    if (Fields[1] == "0")
                        data.Add("Name", Cmn.ProperCase(Fields[2].Trim()));
                    else
                        data.Add("Name", Cmn.ProperCase(Fields[3].Trim()));
                    TableID.Save("Menu", data, db);
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }
        db.Close();
    }

    protected void btmUpdateUrlName_Click(object sender, EventArgs e)
    {
        List<BillingLib.Menu> ml = BillingLib.Menu.GetAll();
        foreach (BillingLib.Menu m in ml)
        {
            m.UrlName = Cmn.GenerateSlug(m.Name);
            m.Save();
        }
    }
    protected void WriteCSVFile_Click(object sender, EventArgs e)
    {
        string Data = File.ReadAllText(Server.MapPath(@"~\berger.txt"));
        string temp = "";
        temp += Data.Replace("~", "\n").Replace("^", ",");
        string Filename = HttpContext.Current.Server.MapPath(@"~\berger.csv");
        File.AppendAllText(Filename, temp);
    }
    protected void btnUpdatePaymentAndDue_Click(object sender, EventArgs e)
    {
        List<Vendor> list = Vendor.GetAll();
        string Error = "";
        DatabaseCE db = new DatabaseCE();
        try
        {
            foreach (Vendor v in list)
            {
                double amountDue = Purchase.GetDueAmountByDate(DateTime.Today.AddYears(-10), DateTime.Today, 0, v.ID);
                double Paid = Cmn.ToDbl(db.GetFieldValue("select sum(Cash+Cheque) from PurchasePayment where VendorID=" + v.ID, ref Error));
                db.RunQuery("update Vendor set AmountPaid=" + Paid + ",AmountDue=" + (amountDue - Paid) + " where ID=" + v.ID);
            }
        }
        finally
        {
            db.Close();
        }
    }
    protected void btnDeleteSalaryTable_Click(object sender, EventArgs e)
    {
        DatabaseCE db = new DatabaseCE();
        try
        {
            db.RunQuery("delete from salary");
        }
        finally
        {
            db.Close();
        }
    }

    protected void btnDeletemonthlyTotal_Click(object sender, EventArgs e)
    {
        DatabaseCE db = new DatabaseCE();
        try
        {
            db.RunQuery("delete from MonthlyTotal");
        }
        finally
        {
            db.Close();
        }
    }
    protected void btnUpdateBillingItemDate(object sender, EventArgs e)
    {
        DatabaseCE db = new DatabaseCE();

        string Error = "";
        try
        {
            IDataReader dr = db.GetDataReader("select * from Billing", ref Error);
            while (dr.Read())
            {
                db.RunQuery("update BillingItem set SaleDate='" + Cmn.ToDate(dr["BillDate"]).ToString("dd-MMM-yyyy HH:mm") + "' , BillNo=" + dr["BillNo"] + ",StoreID=" + dr["StoreID"] + " where billingID=" + dr["ID"]);
            }
        }
        catch (Exception ex)
        {
            Error = ex.Message;
        }
        finally
        {
            db.Close();
        }
    }
    protected void btnUpdaeBillingItemCost_Click(object sender, EventArgs e)
    {
        DatabaseCE db = new DatabaseCE();

        string Error = "";
        try
        {
            IDataReader dr = db.GetDataReader("select * from BillingItem", ref Error);
            while (dr.Read())
            {
                if (Cmn.ToDbl(dr["Cost"]) == 0)
                {
                    Item I = SessionState.Company.ItemList.FirstOrDefault(m => m.ID == Cmn.ToInt(dr["ItemID"]));
                    if (I != null)
                        db.RunQuery("update billingitem set cost=" + I.Cost + " where ID=" + dr["ID"]);
                }
            }
        }
        catch (Exception ex)
        {
            Error = ex.Message;
        }
        finally
        {
            db.Close();
        }

    }

    protected void btnBillAmountDiffer_Click(object sender, EventArgs e)
    {
        DatabaseCE db = new DatabaseCE();
        try
        {
            List<Billing> BL = BillingLib.Billing.GetAll(false,DateTime.Now.AddMonths(-1), DateTime.Now, "", 0, 0, 0, false).OrderByDescending(m => m.BillDate).ToList();
            List<BillingItem> BLItem = BillingLib.BillingItem.GetAll(DateTime.Now.AddMonths(-1), DateTime.Now);

            string str = "<table border='1'><tr><td>BillNo<td>Bill Date<td>Bill Amount<td>BillItem Amount<td>Deff<td>Store";
            foreach (Billing bill in BL)
            {
                double BillAmount = bill.PaidCash + bill.PaidCard;
                double BillItemAmount = BLItem.Where(m => m.BillingID == bill.ID).Sum(m => m.Amount);
                if (BillAmount != BillItemAmount && Math.Abs(BillAmount - BillItemAmount) > 1)
                {
                    str += "<tr><td>" + bill.BillNo + "<td>" + bill.BillDate.ToString("dd-MMM-yy") + "<td>" + BillAmount + "<td>" + BillItemAmount + "<td>" + (BillAmount - BillItemAmount).ToString("0.00") + "<td>" + bill.store.ID;
                }
            }
            str += "</table>";
            ltData.Text = str;
        }
        finally
        {
            db.Close();
        }
    }
    protected void btnDeleteRawBillingItems_Click(object sender, EventArgs e)
    {
        //DatabaseCE db = new DatabaseCE();
        //try
        //{
        //   db.RunQuery("Delete from BillingItem WHERE Code = '' AND Quantity=0 AND SaleDate >= '27-Nov-2015' AND SaleDate < '29-Nov-2015'");
        //}
        //finally
        //{
        //    db.Close();
        //}
    }
    protected void btnDeleteSalaryRecords_Click(object sender, EventArgs e)
    {
        DatabaseCE db = new DatabaseCE();
        try
        {
            db.RunQuery("DELETE FROM Salary  WHERE EmployeeID = 4 AND StoreID = 3");
        }
        finally
        {
            db.Close();
        }
    }

    protected void btnFillDummyData_Click(object sender, EventArgs e)
    {
        //BillingLib.Project P = new Project();
        //P.ProjectName = "ATS One Hamlet";
        //P.Address = "ATS One Hamlet, Sector 104, Noida";
        //P.Area = "Sector 104";
        //P.City = "Noida";
        //P.State = "UP";
        //P.Pin = 203245;
        //P.ID = 1849;
        //P.Save();

        WebClient wc = new WebClient();
        string data = wc.DownloadString("http://propertymap.info/data.aspx?Action=GetApartmentListJSON&Data1=1849");
        string[] Lines = data.Split('~');
        List<BillingLib.Project> PL = BillingLib.Project.GetAll();
        DatabaseCE db = new DatabaseCE();
        try
        {
            foreach (string L in Lines)
            {
                if (L == "") return;
                string[] F = L.Split('^');
                int AptID = Cmn.ToInt(F[0]);
                int ProjectID = PL.Where(m => m.ProjectName == "Ats One Hamlet").Select(m => m.ID).FirstOrDefault(); //Cmn.ToInt(F[1]);
                string ApartmentNo = F[2];
                db.RunQuery("insert into Apartment values(" + AptID + "," + ProjectID + "," + Cmn.ToInt(ApartmentNo) + ",0,'','','','','" + DateTime.Now.ToString("dd-MMM-yyyy HH:mm") + "','')");
            }
        }
        finally
        {
            db.Close();
        }
    }

    protected void btnUpdateLastsaleDate_Click(object sender, EventArgs e)
    {
        List<BillingLib.Customer> CL = BillingLib.Customer.GetAll();
        List<BillingLib.Billing> BL = BillingLib.Billing.GetAll(false,DateTime.Now.AddYears(-1), DateTime.Now);
        DatabaseCE db = new DatabaseCE();
        try
        {
            foreach (BillingLib.Customer C in CL)
            {
                List<BillingLib.Billing> billlist = BL.Where(m => m.customer.Mobile == C.Mobile).ToList();
                if (billlist.Count > 0)
                {
                    DateTime LastBillDate = billlist.Max(m => m.BillDate);
                    db.RunQuery("Update customer set LastSaleDate=" + LastBillDate + " where id=" + C.ID + ")");
                }
            }
        }
        finally
        {
            db.Close();
        }
    }

    protected void btnLinkCustomerAndApartment_Click(object sender, EventArgs e)
    {
        List<BillingLib.Customer> CL = BillingLib.Customer.GetAll();
        List<BillingLib.Apartment> AP = BillingLib.Apartment.GetAll();
        foreach (BillingLib.Customer C in CL)
        {
            Apartment A = AP.FirstOrDefault(m => m.FloorNo == Cmn.ToInt(C.Mobile));
            if (A != null)
            {
                C.ApartmentID = A.ID;
                C.ProjectID = A.ProjectID;
                C.Save();
            }
        }
    }

    protected void btnUpdateEmployeeColumn_Click(object sender, EventArgs e)
    {
        DatabaseCE db = new DatabaseCE();
        try
        {
            //db.RunQuery("ALTER TABLE [Terminal] Drop COLUMN [Addrerss]");
            db.RunQuery("ALTER TABLE [Employee] Drop COLUMN [Designation]");
            db.RunQuery("ALTER TABLE [Employee] ADD [Designation] int");
        }
        finally
        {
            db.Close();
        }
    }

    protected void btnUpdateStoreInCustomer_Click(object sender, EventArgs e)
    {
        List<BillingLib.Customer> CL = BillingLib.Customer.GetAll();
        //List<BillingLib.Billing> BL = BillingLib.Billing.GetAll(DateTime.Now.AddYears(-1), DateTime.Now);

        DatabaseCE db = new DatabaseCE();
        string Error = "";
        try
        {
            foreach (BillingLib.Customer C in CL)
            {
                //BillingLib.Billing B = BL.Where(m => m.customer.ID == C.ID).FirstOrDefault();
                //if (B != null)
                //{
                //    C.StoreID = B.customer.StoreID;
                //    //C.Save();
                //}
                IDataReader dr = db.GetDataReader("Select * from Billing where CustomerMobile='" + C.Mobile + "'", ref Error);
                while (dr.Read())
                {
                    int StoreID = Cmn.ToInt(dr["StoreID"]);
                    C.StoreID = StoreID;
                    C.Save();
                    break;
                }
            }
        }
        finally
        {
            db.Close();
        }
    }
    protected void btnUpateProjectsFromTextFile_Click(object sender, EventArgs e)
    {
        string[] lines = File.ReadAllLines(Server.MapPath(@"~\test.txt"));
        List<BillingLib.Project> PL = new List<BillingLib.Project>();
        foreach (string line in lines)
        {
            try
            {
                string[] l = line.Split('^');
                BillingLib.Project P = new Project();
                P.ID = 0;
                P.ProjectName = l[1].Trim();
                P.Address = l[2].Trim();
                P.Area = l[3].Trim();
                P.City = l[4].Trim();
                P.State = l[5].Trim();
                P.Pin = Cmn.ToInt(l[6]);
                PL.Add(P);
            }
            catch { }
        }
        BillingLib.Project.Save(PL);
    }

    protected void btnDeleteRawItems_Click(object sender, EventArgs e)
    {
        DatabaseCE db = new DatabaseCE();
        try
        {
            db.RunQuery("Delete from Item where (id=717 OR id=1672)");
        }
        finally
        {
            db.Close();
        }

    }
    protected void btnRemoveDuplicate_Click(object sender, EventArgs e)
    {
        ShowDuplicate(true);
    }

    void ShowDuplicate(Boolean Delete = false)
    {
        List<Billing> list = Billing.GetAll(false,DateTime.Today, DateTime.Today, "", 0, 0, 0, false);

        var newlist = list.GroupBy(m => new { m.BillNo, m.store.ID, m.TerminalID, m.PaidCard, m.PaidCash }).Select(n => new { Billno = n.Key.BillNo, StoreID = n.Key.ID, TerminalID = n.Key.TerminalID, Count = n.Count() });

        List<int> BillList = new List<int>();
        string str = "<table>";


        foreach (var i in newlist)
        {
            if (i.Count == 1)
                continue;

            if (!BillList.Contains(i.Billno))
            {
                BillList.Add(i.Billno);
            }


            str += "<tr><td>" + i.Billno + "<td>" + i.StoreID + "<td>" + i.TerminalID + "<td>count " + i.Count;
        }

        if (Delete)
        {
            DatabaseCE db = new DatabaseCE();
            foreach (int bn in BillList)
            {
                List<Billing> b = Billing.GetByBillNo(bn);
                for (int i = 1; i < b.Count; i++)
                {
                    db.RunQuery("delete from Billing where id=" + b[i].ID);
                    db.RunQuery("delete from BillingItem where Billingid=" + b[i].ID);
                }
            }

            db.Close();

        }

        Response.Write(str + "</table>");
    }

    protected void btnShowDuplicate_Click(object sender, EventArgs e)
    {
        ShowDuplicate();
    }
    protected void btnUpdateAllVendorDue_Click(object sender, EventArgs e)
    {
        foreach (Vendor v in SessionState.Company.vendorList)
            v.UpdateAmountDue();
    }
    
    protected void UpdateRemainingVendorIDsInentory_Click(object sender, EventArgs e)
    {
        List<Purchase> PL = Purchase.GetByDate(new DateTime(2015, 6, 1), new DateTime(2015, 9, 30));
        int ctr = 0;
        foreach (Purchase P in PL)
        {
            List<Inventory> IL = Inventory.GetByPurchaseID(P.ID);
            foreach (Inventory i in IL)
            {
                if (i.VendorID == 0 && i.RecordType==RecordType.Purchase)
                {
                    i.VendorID = P.VendorID;
                    i.Save();
                    ctr++;
                }
            }
        }
        Response.Write(ctr);
    }
}
