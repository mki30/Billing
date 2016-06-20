using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingLib
{
    public class BillingItemReport
    {
        public BillingItem billItem;

        public double TotalQuantity;

        //Cost
        public double CostPerUnit;
        public double CostPrice;
        public double CostVat;

        public double TotalCost
        {
            get { return CostPrice + CostVat; }
        }

        public double MRPPerUnit;
        public double SalePrice;
        public double SaleVat;
        public double TotalSaleAmount
        {
            get { return SalePrice + SaleVat; }
        }
        
        public double TotalDiscount;
        public double Margin;

        public double TaxRate;
        public double AdjustedMRP;

    }

    public class BillingItemSalePurchaseReport
    {
        public DateTime date;
        public Item item;
        public RecordType RecordType;
        public double Stock;
        public int StoreID;
        public int FromstoreID;
    }

    public class BillingItem : Item
    {
        public int BillingID { get; set; }
        public int ItemID { get; set; }
        public double BillItemDiscount { get; set; }
        public DateTime SaleDate = DateTime.Now;
        public int BillNo { get; set; }
        public int StoreID { get; set; }
        public double AdjustedRate { get; set; }
        public double AdjustedCost { get; set; }
        public Boolean UseAdjustedMRP = false;


        public double TotalCost
        {
            get
            {
                return Cost * Quantity;
            }

        }

        public double CostPrice
        {
            get
            {
                return TotalCost / (1 + TaxRate / 100);
            }
        }

        public double CostVat
        {
            get
            {
                return TotalCost - CostPrice;
            }
        }

        public double SaleAmount
        {
            get
            {
                double newMRP = UseAdjustedMRP ? AdjustedRate : MRP;
                return newMRP * Quantity - DiscountAmount;
            }
        }

        public double SalePrice
        {
            get
            {
                return SaleAmount / (1 + TaxRate / 100);
            }
        }

        public double SaleVat
        {
            get
            {
                return TaxRate != 0 ? SaleAmount - SalePrice : 0; //SaleVat
            }
        }

        public double Margin
        {
            get
            {
                return SaleAmount - SaleVat - TotalCost; //margin
            }
        }

        public double Amount
        {
            get
            {
                return Quantity * MRP * (BillItemDiscount > 0 ? (100 - BillItemDiscount) / 100 : 1);
            }
        }

        public double AmountAdjusted
        {
            get
            {
                return Quantity * AdjustedRate * (BillItemDiscount > 0 ? (100 - BillItemDiscount) / 100 : 1);
            }
        }

        public double DiscountAmount
        {
            get
            {
                return ((Quantity * MRP) - Amount);
            }
        }

        static BillingItem GetRecord(IDataReader dr)
        {
            return new BillingItem()
            {
                ID = Cmn.ToInt(dr["ID"]),
                BillingID = Cmn.ToInt(dr["BillingID"].ToString()),
                PLU = dr["Code"].ToString(),
                Name = dr["ItemName"].ToString(),
                MRP = Cmn.ToDbl(dr["Rate"]),
                Cost = Cmn.ToDbl(dr["Cost"]),
                TaxRate = Cmn.ToDbl(dr["TaxPerc"]),
                Quantity = Cmn.ToDbl(dr["Quantity"]),
                ItemID = Cmn.ToInt(dr["ItemID"]),
                BillItemDiscount = Cmn.ToDbl(dr["BillItemDiscount"]),
                SaleDate = Cmn.ToDate(dr["SaleDate"]),
                BillNo = Cmn.ToInt(dr["BillNo"]),
                StoreID = Cmn.ToInt(dr["StoreID"]),
                AdjustedRate = Cmn.ToDbl(dr["AdjustedRate"]),
                AdjustedCost = Cmn.ToDbl(dr["AdjustedCost"])
            };
        }

        public void SaveBillingItem(DatabaseCE db, bool IsfutureOrder = false)
        {
            Dictionary<string, object> data2 = new Dictionary<string, object>();
            data2.Add("ID", ID);
            data2.Add("BillingID", BillingID);
            data2.Add("Code", PLU);
            data2.Add("ItemName", Name);
            data2.Add("Rate", MRP);
            data2.Add("Cost", Cost);
            data2.Add("Quantity", Quantity);
            data2.Add("BillItemDiscount", BillItemDiscount);
            data2.Add("TaxPerc", TaxRate);
            data2.Add("Total", Amount);
            data2.Add("ItemID", ItemID);
            data2.Add("SaleDate", SaleDate);
            data2.Add("BillNo", BillNo);
            data2.Add("StoreID", StoreID);
            data2.Add("AdjustedRate", AdjustedRate);
            data2.Add("AdjustedCost", AdjustedCost);
            if (IsfutureOrder)
                TableID.Save("FutureBillingItem", data2, db);
            else
                TableID.Save("BillingItem", data2, db);
            ID = Cmn.ToInt(data2["ID"]);
        }

        public static void SaveBillingItem(List<BillingItem> list)
        {
            string vError = string.Empty;
            DatabaseCE db = new DatabaseCE();
            try
            {
                foreach (BillingItem bi in list)
                {
                    bi.SaveBillingItem(db);
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


        public static List<BillingItem> GetByBillingID(int BillingID, DatabaseCE db, Boolean IsFutureTable = false)
        {
            List<BillingItem> list = new List<BillingItem>();
            try
            {
                string Error = "";

                IDataReader dr = db.GetDataReader("select * from " + (IsFutureTable ? "FutureBillingItem" : "BillingItem") + " where BillingID=" + BillingID, ref Error);

                while (dr.Read())
                {
                    list.Add(GetRecord(dr));
                }
            }
            catch (Exception ex)
            {
                string s = ex.Message;
            }
            return list;
        }

        public static List<BillingItem> GetAll(DateTime DateFrom, DateTime DateTo, string ItemIds, int StoreID = 0)
        {
            List<BillingItem> list = new List<BillingItem>();
            DatabaseCE db = new DatabaseCE();
            try
            {
                string Error = "";
                IDataReader dr = db.GetDataReader("select * from BillingItem  where 1=1" + (ItemIds != "" ? "ItemID in (" + ItemIds + ")" : "")
                     + " and SaleDate>='" + DateFrom.ToString("dd-MMM-yyyy") + "' and SaleDate< '" + DateTo.AddDays(1).ToString("dd-MMM-yyyy") + "'"
                    + (StoreID != 0 ? " and StoreID=" + StoreID + "" : ""), ref Error);
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

        public static List<BillingItem> GetAll(string BillingIDs, double Tax = 0, int ItemID = 0)
        {
            List<BillingItem> list = new List<BillingItem>();
            DatabaseCE db = new DatabaseCE();
            try
            {
                string Error = "";
                IDataReader dr = db.GetDataReader("select * from BillingItem where BillingID in (" + BillingIDs + ")"
                    + (Tax != 0 ? " and TaxPerc=" + Tax + "" : "")
                    + (ItemID != 0 ? " and ItemID=" + ItemID + "" : "")
                    , ref Error);
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

        public static List<BillingItem> MakeRateAndAdjustedSame(string BillingIDs)
        {
            List<BillingItem> list = new List<BillingItem>();
            DatabaseCE db = new DatabaseCE();
            try
            {
                db.RunQuery("update BillingItem set AdjustedRate=Rate where BillingID in (" + BillingIDs + ")");
            }
            finally
            {
                db.Close();
            }
            return list;
        }

        public static List<BillingItem> GetAll(string BillingIDs, string ItemIDs)
        {
            List<BillingItem> list = new List<BillingItem>();
            DatabaseCE db = new DatabaseCE();
            try
            {
                string Error = "";
                IDataReader dr = db.GetDataReader("select * from BillingItem where BillingID in (" + BillingIDs + ")"
                    + " and ItemID in (" + ItemIDs + ")"
                    , ref Error);
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

        public static List<BillingItem> GetAll()
        {
            List<BillingItem> list = new List<BillingItem>();
            DatabaseCE db = new DatabaseCE();
            try
            {
                string Error = "";
                IDataReader dr = db.GetDataReader("select * from BillingItem", ref Error);
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

        public static BillingItem GetByBillingItemID(int id)
        {
            string vError = string.Empty;
            DatabaseCE db = new DatabaseCE();
            try
            {
                string Error = "";
                IDataReader dr = db.GetDataReader("select * from BillingItem where ID=" + id, ref Error);
                while (dr.Read())
                {
                    return GetRecord(dr);
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

        public static List<BillingItem> GetAll(DateTime DateFrom, DateTime DateTo, double Tax = 0, int ItemID = 0, int StoreID = 0)
        {
            List<BillingItem> list = new List<BillingItem>();
            DatabaseCE db = new DatabaseCE();
            try
            {
                string Error = "";
                IDataReader dr = db.GetDataReader("select * from BillingItem where 1=1 " + (Tax != 0 ? " and TaxPerc=" + Tax + "" : "")
                    + (ItemID != 0 ? " and ItemID=" + ItemID + "" : "")
                    + " and SaleDate>='" + DateFrom.ToString("dd-MMM-yyyy") + "' and SaleDate< '" + DateTo.AddDays(1).ToString("dd-MMM-yyyy") + "'"
                    + (StoreID != 0 ? " and StoreID=" + StoreID + "" : "")
                    , ref Error);
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


        public static BillingItem GetFirstBillByItemID(int ItemID)
        {
            string vError = string.Empty;
            DatabaseCE db = new DatabaseCE();
            try
            {
                string Error = "";
                IDataReader dr = db.GetDataReader("select TOP(1) * from BillingItem where ItemID=" + ItemID, ref Error);
                while (dr.Read())
                {
                    return GetRecord(dr);
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
    }
}
