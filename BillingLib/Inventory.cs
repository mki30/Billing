using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingLib
{
    public class Inventory
    {
        public int ID;

        public int VendorID;
        public int PurchaseID;
        public int ItemID;
        public double Quantity;
        public double Cost;
        public UnitType CostUnitType;
        public double MRP;
        public double TotalAmount;
        public double Tax;
        public double Discount;
        public int Unit;
        public UnitType UnitType;
        public DateTime Expiry;
        public string ExpiryString { get { return Expiry==Cmn.MinDate?"": Expiry.ToString("dd-MMM-yyyy"); } }
        public int SNo;
        public string PLU = "";
        public string Name = "";
        public int ItemIncludesTax;
        public string RcNo = "";
        public DateTime Date;
        public int StoreID;
        public int IsDelete;
        public DateTime LUDate;
        public string InvoiceNo = "";
        public int FromStoreID;


        public string DateString
        {
            get
            {
                DateTime dt = Cmn.ToDate(Date);
                return dt == Cmn.MinDate ? "" : dt.ToString("dd-MMM-yyyy");
            }
        }

        public string NameLower
        {
            get
            {
                return Name.ToLower();
            }
        }
        public RecordType RecordType = RecordType.Purchase;
        //public string Name
        //{
        //    get
        //    {
        //        Item i;
        //        if (Global.ItemListByID.TryGetValue(ItemID, out i))
        //            return i.Name;
        //        return "";
        //    }
        //}

        public Inventory()
        {

        }

        public Inventory(Inventory obj)
        {
            this.ItemID = obj.ItemID;
            this.Name = obj.Name;
            this.Tax = obj.Tax;
            this.Cost = obj.Cost;
            this.PLU = obj.PLU;
            this.MRP = obj.MRP;
            this.Date = obj.Date;
            this.VendorID = obj.VendorID;

        }
        static Inventory GetRecord(IDataReader dr)
        {
            return new Inventory()
            {
                ID = Cmn.ToInt(dr["ID"]),
                SNo = Cmn.ToInt(dr["SNo"]),
                PurchaseID = Cmn.ToInt(dr["PurchaseID"]),
                VendorID = Cmn.ToInt(dr["VendorID"]),
                ItemID = Cmn.ToInt(dr["ItemID"]),
                Quantity = Cmn.ToDbl(dr["Quantity"]),
                Cost = Cmn.ToDbl(dr["Cost"]),
                MRP = Cmn.ToDbl(dr["MRP"]),
                TotalAmount = Cmn.ToDbl(dr["TotalAmount"]),
                Tax = Cmn.ToDbl(dr["Tax"]),
                Discount = Cmn.ToDbl(dr["Discount"]),
                Unit = Cmn.ToInt(dr["Unit"]),
                UnitType = (UnitType)Cmn.ToInt(dr["UnitType"]),
                CostUnitType = (UnitType)Cmn.ToInt(dr["CostUnitType"]),
                Expiry = Cmn.ToDate(dr["Expiry"]),
                PLU = dr["PLUCode"].ToString(),
                Name = dr["Name"].ToString(),
                RecordType = (RecordType)Cmn.ToInt(dr["RecordType"]),
                ItemIncludesTax = Cmn.ToInt(dr["ItemIncludesTax"]),
                RcNo = dr["RcNo"].ToString(),
                Date = Cmn.ToDate(dr["Date"]),
                LUDate = Cmn.ToDate(dr["LUDate"]),
                StoreID = Cmn.ToInt(dr["StoreID"]),
                IsDelete = Cmn.ToInt(dr["IsDelete"]),
                FromStoreID = Cmn.ToInt(dr["FromStoreID"]),
                InvoiceNo = dr["InvoiceNo"].ToString(),


            };
        }

        public void Save()
        {
            DatabaseCE db = new DatabaseCE();
            try
            {
                Dictionary<string, object> data = new Dictionary<string, object>();

                data.Add("ID", ID);
                data.Add("SNo", SNo);
                data.Add("PurchaseID", PurchaseID);
                data.Add("VendorID", VendorID);
                data.Add("ItemID", ItemID);
                data.Add("Quantity", Quantity);
                data.Add("Cost", Cost);
                data.Add("MRP", MRP);
                data.Add("TotalAmount", TotalAmount);
                data.Add("Tax", Tax);
                data.Add("Discount", Discount);
                data.Add("Unit", Unit);
                data.Add("UnitType", (int)UnitType);
                data.Add("CostUnitType", (int)CostUnitType);
                data.Add("Expiry", Expiry);
                data.Add("PLUCode", PLU);
                data.Add("Name", Name);
                data.Add("RecordType", (int)RecordType);
                data.Add("ItemIncludesTax", ItemIncludesTax);
                data.Add("RcNo", RcNo);
                data.Add("Date", Date);
                data.Add("StoreID", StoreID);
                data.Add("IsDelete", IsDelete);
                data.Add("FromStoreID", FromStoreID);
                data.Add("InvoiceNo", InvoiceNo);

                TableID.Save("Inventory", data, db);
                ID = Cmn.ToInt(data["ID"]);

                string vError = "";

                double count = 0;
                count = Cmn.ToDbl(db.ExecuteScalar("Select Count(*) from Inventory where purchaseID=" + data["PurchaseID"] + "", ref vError));
                db.RunQuery("Update Purchase set ItemCount=" + count + " where id=" + data["PurchaseID"] + "");
                Purchase.UpdateTotal(PurchaseID);
                Purchase.UpdateMonthlyTotal(Date, StoreID);
            }
            catch
            {
            }
            finally
            {
                db.Close();
            }
        }

        public static void UpdateInventoryData(Purchase P)
        {
            DatabaseCE db = new DatabaseCE();
            try
            {
                db.RunQuery("update inventory set [Date]='" + P.PurchaseDate.ToString("dd-MMM-yyyy")
                    + "', ItemIncludesTax = " + P.ItemIncludesTax
                    + ",RcNo = '" + P.RCNo
                    + "',VendorID = " + P.VendorID
                    + ",InvoiceNo = '" + P.InvoiceNo + "' where PurchaseID=" + P.ID);
            }
            finally
            {
                db.Close();
            }
        }
        
        public string GetItemHTMLRow(Company comp, int ReportForStoreID, Purchase P, RecordType ReportType, Boolean HightLight = false)
        {
            Item item = comp.GetItem(ItemID);
            TotalAmount = Quantity * Cost;

            double AfterDiscount = Quantity * Cost * (1 - Discount / 100);
            double ta = P != null && P.ItemIncludesTax == 1 ? TotalAmount / Tax / 100 : TotalAmount * Tax / 100;
            double cost = AfterDiscount + (P != null && P.ItemIncludesTax == 0 ? ta : 0);

            double MRP = this.MRP * Quantity;
            double Margin = (this.MRP - Cost) / Cost * 100;

            string InventoryType = RecordType.ToString();

            if (RecordType == RecordType.Transfer)
            {
                if (ReportType == RecordType.TransferIn && this.StoreID == ReportForStoreID)
                    InventoryType += " In";

                if (ReportType == RecordType.TransferOut && this.FromStoreID == ReportForStoreID)
                    InventoryType += "  Out";
            }

            Vendor ven = comp.vendorList.FirstOrDefault(m => m.ID == VendorID);
            string VendorName = ven != null ? ven.Name : "";

            return "<tr id='tr" + ID + "' style='" + (IsDelete == 1 ? "text-decoration: line-through;" : "") + (HightLight ? " class='highlight'" : "") + "'><td title='" + ID + "'>" + SNo
                    + "<td>" + (item != null ? item.PLU : "")
                    + "<td>" + (ReportType == RecordType.TransferIn ? (item != null ? item.Name : "<No Name>") : "<a href='#' onclick='return ShowEdit(" + ID + "," + (int)RecordType + ")'>" + (item != null ? item.Name : "<No Name>") + "<a/>")
                    + "<td>" + VendorName
                    + "<td class='alnright'>"
                    + (Unit != 0 ? Unit.ToString() : "") + (UnitType != UnitType.none ? UnitType.ToString() : "")
                    + "<td class='alnright' >" + Quantity
                    + "<td class='alnright'>" + Cost.ToString("0.00") + (CostUnitType != UnitType.none ? "/" + CostUnitType : "")
                    + "<td class='alnright'>" + (this.MRP != 0 ? this.MRP.ToString("0") : "")
                    + "<td class='alnright'>" + (Discount != 0 ? Discount.ToString() : "")
                    + "<td class='alnright'>" + (Tax != 0 ? (int)Tax == Tax ? Tax.ToString() : Tax.ToString("0.00") : "")
                    + "<td class='alnright' title='" + TotalAmount.ToString("0.00") + "'>" + AfterDiscount.ToString("0.00")
                    + "<td class='alnright'>" + ta.ToString("0.00")
                    + "<td class='alnright'>" + Margin.ToString("0")
                    + "<td class='alnright' style='white-space:nowrap'>" + Date.ToString("%d-MMM-yy")
                    + "<td class='alnright'>" + RcNo
                    + "<td>" + InventoryType
                //+ "<td class='alnright'>" + 
                    ;
        }

        public static string GetHTML(Company comp, int VendorID, int StoreID, RecordType recordType, DateTime fromDate, DateTime toDate, string RcNo = "", int FromStoreID = 0, int ItemID = 0)
        {
            if ((toDate - fromDate).TotalDays > 31)
                return "Date range can not be more than a month";

            List<Inventory> list = BillingLib.Inventory.GetInventory(VendorID, StoreID, recordType, fromDate, toDate, ItemID, 0, FromStoreID);
            return GetInventoryHTML(comp, list, StoreID, recordType, 0, null, recordType);
        }

        public static string GetHTML(Company comp, int purchaseID, int inventoryID = 0, int StoreID = 0)
        {
            BillingLib.Purchase P = Purchase.GetByID(purchaseID);
            if (P == null)
                return "";

            return GetInventoryHTML(comp, BillingLib.Inventory.GetByPurchaseID(purchaseID), StoreID, RecordType.Purchase, inventoryID, P, RecordType.Purchase);
        }

        public static string GetInventoryHTML(Company comp, List<Inventory> list, int ReportForStoreID, RecordType RecordType, int InventoryID = 0, Purchase P = null, RecordType ReportType = RecordType.None)
        {
            StringBuilder str = new StringBuilder();

            list = (RecordType == RecordType.Purchase ? list.OrderBy(m => m.SNo) : list.OrderByDescending(m => m.Date)).ToList();
            if (RecordType == RecordType.Purchase)
                list = list.Where(m => m.RecordType == RecordType).ToList();

            double totalRate = 0.0f;
            double totalAfterDiscountRate = 0.0f;
            double totalMRP = 0.0f;
            double totalTax = 0.0f;
            double TotalQty = 0;

            int MaxSno = 0;
            int count = 0;

            int ctr = 0;
            foreach (BillingLib.Inventory i in list)
            {
                if (RecordType != RecordType.Purchase)
                    i.SNo = ++ctr;

                i.TotalAmount = i.Quantity * i.Cost;
                double AfterDiscount = i.Quantity * i.Cost * (1 - i.Discount / 100);
                double ta = 0;
                double cost = 0;

                if (P != null)
                {
                    //ta = P.ItemIncludesTax == 1 ? i.TotalAmount / i.Tax / 100 : i.TotalAmount * i.Tax / 100;
                    cost = AfterDiscount + (P.ItemIncludesTax == 0 ? ta : 0);
                }
                ta = i.ItemIncludesTax == 1 ? i.TotalAmount / i.Tax / 100 : i.TotalAmount * i.Tax / 100;

                double MRP = i.MRP * i.Quantity;
                double Margin = (MRP - cost) / MRP * 100;
                TotalQty += i.Quantity;
                str.Append(i.GetItemHTMLRow(comp, ReportForStoreID, P, ReportType, InventoryID == i.ID));
                totalAfterDiscountRate += AfterDiscount;
                totalRate += i.TotalAmount;
                totalMRP += i.MRP * i.Quantity;
                totalTax += ta;

                MaxSno = i.SNo;
                count++;
            }

            double PurchaseCST = P != null ? P.CST : 0;
            double cst = 0;
            double totalAfterTax = (totalAfterDiscountRate + totalTax);
            cst = ((totalAfterDiscountRate * Cmn.ToDbl(PurchaseCST) / 100));
            str.Append("<tr style='font-weight:bold'><td>" + count
                + "<td><td><td class='alnright'>Total<td><td class='alnright' >" + TotalQty
                + "<td><td class='alnright'>" + totalMRP
                + "<td><td>"
                + "<td class='alnright' title='" + totalRate.ToString("0") + "'>" + totalAfterDiscountRate.ToString("0")
                + "<td class='alnright'>" + totalTax.ToString("0") + "<td><td><td>"
                + "<tr style='font-weight:bold'><td colspan='10' class='alnright'>After Tax<td class='alnright' >" + Cmn.toCurrency(totalAfterTax)
                + "<td colspan='2'>"
                + "<tr><th colspan='10' class='alnright'> Total After " + PurchaseCST + "% CST " + cst.ToString("0.0") + "<th class='alnright' id='tdTotalAfterTax'>" + Cmn.toCurrency(totalAfterTax + cst) + "<td colspan='2'>"
                + "<td><td>"
                + "</table>");

            str.Append("<div style='display:none' id='divMaxSno'>" + (++MaxSno) + "</div><a href='#' id='linkNewPurchase' class='btn btn-sm btn-success' onclick='return ShowEdit(0)'>Add New Record</a><br/>");

            if (P != null)
                str.Append((P.ItemIncludesTax == 1 ? "Item Rate Includes Tax" : "Item Rate Excludes Tax"));


            if (RecordType == RecordType.Purchase)  //Update purchase amount on show- to be fixed
            {
                DatabaseCE db = new DatabaseCE();
                try
                {
                    db.RunQuery("update purchase set amount=" + (totalAfterTax + cst) + " where ID=" + P.ID);
                }
                catch
                {
                }
                finally
                {
                    db.Close();
                }
            }

            string csvName = "Download.csv";
            switch ((int)ReportType)
            {
                case 1:
                    csvName = "Return.csv";
                    break;
                case 2:
                    csvName = "Damage.csv";
                    break;
                case 3:
                    csvName = "TranferIn.csv";
                    break;
                case 4:
                    csvName = "TranferOut.csv";
                    break;
            }
            return "<a href='#' onclick='DownloadFile(\"" + csvName + "\")'>&nbsp;<i class='glyphicon glyphicon-download-alt' style='font-size:11px;'></i>Download</a><table class='table table-condensed table-bordered table-striped table-hover csvTable' id='itemDataTable'><tr><th>#<th>PLU<th>Name<th>Vendor<th title='Weight' class='alnright'>Wgt<th class='alnright'>Qty<th class='alnright'>Rate<th class='alnright'>MRP"
                + "<th class='alnright'>Disc.<th class='alnright'>Tax<th title='Amount After Discount' class='alnright'>Amount<th class='alnright'>Tax<th class='alnright' title='Margin'>Mar.%<th class='alnright'>Date<th class='alnright'>RCNo" + str.ToString();
        }

        public static int GetMaxSNo(int PurchaseID, RecordType recordType = RecordType.Purchase)
        {
            List<Inventory> list = new List<Inventory>();
            DatabaseCE db = new DatabaseCE();
            try
            {
                string Error = "";
                return db.GetMax("Inventory", "SNo", "PurchaseID=" + PurchaseID + " and RecordType=" + (int)recordType + "", ref Error);
            }
            finally
            {
                db.Close();
            }
        }

        public static List<Inventory> GetAll(int ID = 0, RecordType recordType = RecordType.None)
        {
            List<Inventory> list = new List<Inventory>();
            DatabaseCE db = new DatabaseCE();
            try
            {
                string Error = "";
                IDataReader dr = db.GetDataReader("select * from Inventory where 1=1" + (ID == 0 ? "" : " and ID=" + ID) + (recordType != RecordType.None ? " and RecordType=" + (int)recordType : ""), ref Error);
                while (dr.Read())
                {
                    list.Add(GetRecord(dr));
                }
            }
            finally
            {
                db.Close();
            }
            return list;
        }

        public static List<Inventory> Get(DateTime fromDate, DateTime toDate, int FromStoreID, int ToStoreID, int ItemID = 0, RecordType recordType = RecordType.None)
        {
            List<Inventory> list = new List<Inventory>();
            DatabaseCE db = new DatabaseCE();
            try
            {
                string query = "select * from Inventory where 1=1 "
                    + (ToStoreID != 0 ? " and StoreID=" + ToStoreID : "")
                    + (FromStoreID != 0 ? " and FromStoreID=" + FromStoreID : "")
                    + " and [Date]>='" + fromDate.ToString("dd-MMM-yyyy") + "'"
                    + " and [Date] < '" + toDate.AddDays(1).ToString("dd-MMM-yyyy") + "'"
                    + (ItemID != 0 ? " and ItemID=" + ItemID : "")
                    + (recordType != RecordType.None ? " and RecordType=" + (int)recordType : "")
                    + " and (IsDelete<>1 or IsDelete is null)";

                string Error = "";
                IDataReader dr = db.GetDataReader(query,
                    ref Error);
                while (dr.Read())
                {
                    list.Add(GetRecord(dr));
                }
            }
            finally
            {
                db.Close();
            }
            return list;
        }

        public static List<Inventory> Get(int StoreID, DateTime fromDate, DateTime toDate, int ItemID = 0)
        {
            List<Inventory> list = new List<Inventory>();
            DatabaseCE db = new DatabaseCE();
            try
            {
                string query = "select * from Inventory where (StoreID=" + StoreID + " or FromStoreID=" + StoreID + ")"
                    + " and [Date]>='" + fromDate.ToString("dd-MMM-yyyy") + "'"
                    + " and [Date] < '" + toDate.AddDays(1).ToString("dd-MMM-yyyy") + "'"
                    + (ItemID != 0 ? " and ItemID=" + ItemID : "")
                    + " and (IsDelete<>1 or IsDelete is null)";

                string Error = "";
                IDataReader dr = db.GetDataReader(query,
                    ref Error);
                while (dr.Read())
                {
                    list.Add(GetRecord(dr));
                }
            }
            finally
            {
                db.Close();
            }
            return list;
        }

        public static List<Inventory> GetInventory(int StoreID, int ItemID, DateTime fromDate, DateTime toDate)
        {
            List<Inventory> list = new List<Inventory>();
            DatabaseCE db = new DatabaseCE();
            try
            {
                string query = "select * from Inventory where (StoreID=" + StoreID + " or FromStoreID =" + StoreID + ")"
                    + (ItemID != 0 ? " and ItemID=" + ItemID : "")
                    + " and [Date]>='" + fromDate.ToString("dd-MMM-yyyy") + "'"
                    + " and [Date] < '" + toDate.AddDays(1).ToString("dd-MMM-yyyy") + "'"
                    + " and (IsDelete<>1 or IsDelete is null) order by [Date],ID desc";

                string Error = "";
                IDataReader dr = db.GetDataReader(query, ref Error);
                while (dr.Read())
                {
                    list.Add(GetRecord(dr));
                }
            }
            finally
            {
                db.Close();
            }
            return list;
        }

        public static List<Inventory> GetInventory(int VendorID, int StoreID, RecordType recordType, DateTime fromDate, DateTime toDate, int ItemID, double Tax, int FromStoreID = 0, string ItemIds = "")
        {
            List<Inventory> list = new List<Inventory>();
            DatabaseCE db = new DatabaseCE();
            try
            {
                string query = "select * from Inventory where 1=1 "
                    + (StoreID != 0 ? "and StoreID=" + StoreID : "")
                    + (recordType != RecordType.None ? " and RecordType=" + (int)recordType : "")
                    + (VendorID == 0 ? "" : " and VendorID=" + VendorID)
                    + " and [Date]>='" + fromDate.ToString("dd-MMM-yyyy") + "'"
                    + " and [Date] < '" + toDate.AddDays(1).ToString("dd-MMM-yyyy") + "'"
                    + (ItemID != 0 ? " and ItemID=" + ItemID : "") + ""
                    + (FromStoreID != 0 ? " and FromStoreID=" + FromStoreID : "") + ""
                    + (Tax != 0 ? " and Tax=" + Tax : "")
                    + (ItemIds != "" ? " and ItemID IN(" + ItemIds + ")" : "")
                    + " and (IsDelete<>1 or IsDelete is null) order by [Date],ID desc";

                string Error = "";
                IDataReader dr = db.GetDataReader(query, ref Error);
                while (dr.Read())
                {
                    list.Add(GetRecord(dr));
                }
            }
            finally
            {
                db.Close();
            }
            return list;
        }

        public static List<Inventory> GetByPurchaseID(int purchaseID)
        {
            List<Inventory> list = new List<Inventory>();
            DatabaseCE db = new DatabaseCE();
            try
            {
                string Error = "";
                IDataReader dr = db.GetDataReader("select * from Inventory where PurchaseID=" + purchaseID, ref Error);
                while (dr.Read())
                {
                    list.Add(GetRecord(dr));
                }
            }
            finally
            {
                db.Close();
            }
            return list;
        }


        public static List<Inventory> GetByItemID(int ItemID)
        {
            List<Inventory> list = new List<Inventory>();
            DatabaseCE db = new DatabaseCE();
            try
            {
                string Error = "";
                IDataReader dr = db.GetDataReader("select * from Inventory where ItemID=" + ItemID, ref Error);
                while (dr.Read())
                {
                    list.Add(GetRecord(dr));
                }
            }
            finally
            {
                db.Close();
            }
            return list;
        }

        public static Inventory GetByID(int id)
        {
            DatabaseCE db = new DatabaseCE();
            try
            {
                string Error = "";
                IDataReader dr = db.GetDataReader("select * from Inventory where id=" + id, ref Error);
                while (dr.Read())
                {
                    Inventory item = Inventory.GetRecord(dr);
                    return item;
                }
            }
            catch
            {

            }
            finally
            {
                db.Close();
            }
            return null;
        }

        public Inventory Delete()
        {
            DatabaseCE db = new DatabaseCE();
            try
            {
                db.RunQuery("Delete from Inventory where id=" + ID);
            }
            catch
            {
            }
            finally
            {
                db.Close();
            }
            return this;
        }

        //public static List<Object> GetByItemID(int vendorID)
        //{
        //    List<Object> list = new List<Object>();
        //    DatabaseCE db = new DatabaseCE();
        //    try
        //    {
        //        string Error = "";
        //        string s = "SELECT Customers.CustomerName, Orders.OrderID FROM Customers INNER JOIN Orders ON Customers.CustomerID=Orders.CustomerID ;";
        //        IDataReader dr = db.GetDataReader("select * from Inventory where ItemID=" + ItemID, ref Error);
        //        while (dr.Read())
        //        {
        //        }
        //    }
        //    finally
        //    {
        //        db.Close();
        //    }
        //    return list;
        //}

        public static void DeleteByPurchaseID(int purchaseID, bool delete)
        {
            DatabaseCE db = new DatabaseCE();
            try
            {
                db.RunQuery("Update Inventory set isdelete=" + (delete ? 1 : 0) + " where purchaseid=" + purchaseID + " and RecordType=" + (int)RecordType.Purchase);
            }
            catch
            {
            }
            finally
            {
                db.Close();
            }
        }
    }
}
