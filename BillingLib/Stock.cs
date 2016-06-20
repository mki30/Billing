using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.IO;

namespace BillingLib
{
    public class Stock
    {
        public int ItemID;
        public DateTime StockMonth = Cmn.MinDate;
        public double OpeningStock;
        public double Purchase;
        public double Sale;
        public double TransferIn;
        public double TransferOut;
        public double Return;
        public double Damage;
        public int StoreID;
        public double ClosingStock;
        public double AutoOpeningStock;
        public string ItemName = "";
        public string PLU = "";
        public DateTime LastUpdated = Cmn.MinDate;


        public double StockCount
        {
            get
            {
                return (OpeningStock != 0 ? OpeningStock : AutoOpeningStock) + Purchase - Return - Damage + TransferIn - TransferOut - Sale;
            }
        }

        public double AutoClosingStock;

        public static Stock GetRecord(IDataReader dr)
        {
            return new Stock()
            {
                ItemID = Cmn.ToInt(dr["ItemID"]),
                StockMonth = Cmn.ToDate(dr["StockMonth"]),
                Purchase = Cmn.ToDbl(dr["Purchase"]),
                Sale = Cmn.ToDbl(dr["Sale"]),
                TransferIn = Cmn.ToDbl(dr["TransferIn"]),
                TransferOut = Cmn.ToDbl(dr["TransferOut"]),
                Return = Cmn.ToDbl(dr["Return"]),
                Damage = Cmn.ToDbl(dr["Damage"]),
                StoreID = Cmn.ToInt(dr["StoreID"]),
                OpeningStock = Cmn.ToDbl(dr["OpeningStock"]),
                ClosingStock = Cmn.ToDbl(dr["ClosingStock"]),
                AutoOpeningStock = Cmn.ToDbl(dr["AutoOpeningStock"]),
                AutoClosingStock = Cmn.ToDbl(dr["AutoClosingStock"]),
                LastUpdated = Cmn.ToDate(dr["LUDate"]),
            };
        }

        public void Save(DatabaseCE db_ = null)
        {

            DatabaseCE db = db_ == null ? new DatabaseCE() : db_;

            try
            {
                string vError = string.Empty;
                Dictionary<string, object> data = new Dictionary<string, object>();
                data.Add("ItemID", ItemID);
                data.Add("StockMonth", StockMonth);
                data.Add("Purchase", Purchase);
                data.Add("Sale", Sale);
                data.Add("TransferIn", TransferIn);
                data.Add("TransferOut", TransferOut);
                data.Add("Return", Return);
                data.Add("Damage", Damage);
                data.Add("StoreID", StoreID);
                data.Add("OpeningStock", OpeningStock);
                data.Add("ClosingStock", ClosingStock);
                data.Add("AutoOpeningStock", AutoOpeningStock);
                data.Add("AutoClosingStock", AutoClosingStock);
                TableID.Save("Stock", data, new string[] { "ItemID", "StockMonth", "StoreID" }, db);
            }
            finally
            {
                if (db_ == null)
                    db.Close();
            }
        }

        public static void Save(List<Stock> list)
        {
            string vError = string.Empty;
            DatabaseCE db = new DatabaseCE();
            try
            {
                foreach (Stock s in list)
                    s.Save(db);
            }
            catch
            {
            }
            finally
            {
                db.Close();
            }
        }


        public static Stock Get(int ItemID, DateTime StockMonth, int StoreID)
        {
            string vError = string.Empty;
            DatabaseCE db = new DatabaseCE();
            try
            {
                string Error = "";
                IDataReader dr = db.GetDataReader("select * from Stock where ItemID=" + ItemID + " and StockMonth='" + StockMonth.ToString("dd-MMM-yyyy") + "' and StoreID=" + StoreID, ref Error);
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

        public List<Stock> Get(DateTime StockOpeningDate)
        {
            return new List<Stock>();
        }

        //Send months first date
        public static List<Stock> GetByMonth(DateTime DateFrom, DateTime DateTo, int StoreID)
        {
            string vError = string.Empty;
            List<Stock> list = new List<Stock>();
            DatabaseCE db = new DatabaseCE();
            try
            {
                using (IDataReader dr = db.GetDataReader("select * from Stock where StoreID=" + StoreID
                    + " and StockMonth>='" + DateFrom.ToString("dd-MMM-yyyy") + "' and StockMonth<='" + DateTo.ToString("dd-MMM-yyyy") + "'", ref vError))
                {
                    while (dr.Read())
                    {
                        list.Add(GetRecord(dr));
                    }

                }
            }
            catch
            {
            }
            finally
            {
                db.Close();
            }

            return list;
        }

        public static List<BillingLib.Stock> GetCurrentStock(Company company, int StoreID, DateTime StockMonth, Boolean SaveToDB = false)
        {
            DateTime startDate = Cmn.MonthFirst(StockMonth);
            DateTime endDate = Cmn.MonthLast(StockMonth);

            List<Stock> list = Stock.GetByMonth(startDate, endDate, StoreID);
            foreach (Stock s in list)
            {
                s.Purchase = 0;
                s.Sale = 0;
                s.TransferIn = 0;
                s.TransferOut = 0;
                s.Return = 0;
                s.Damage = 0;
            }

            List<BillingLib.Inventory> listInventoryPurchase = BillingLib.Inventory.Get(StoreID, startDate, endDate);// purchase transfer damage returns
            List<BillingLib.BillingItem> billingItem = BillingLib.BillingItem.GetAll(startDate, endDate, 0, 0, StoreID);// sale

            // update next months opening stock
            DateTime NextMonthOpening = endDate.AddDays(1);
            List<Stock> listNext = Stock.GetByMonth(NextMonthOpening, Cmn.MonthLast(NextMonthOpening), StoreID);

            //updae all items
            foreach (Item item in company.ItemList)
            {
                BillingLib.Stock s = list.FirstOrDefault(m => m.ItemID == item.ID);
                if (s == null)
                {
                    s = new BillingLib.Stock();
                    s.ItemID = item.ID;
                    list.Add(s);
                }

                foreach (BillingLib.Inventory p in listInventoryPurchase.Where(m => m.ItemID == item.ID)) // get purchase transfer damage of the item
                {
                    if (p.RecordType == RecordType.Purchase)
                        s.Purchase += p.Quantity;
                    else if (p.RecordType == RecordType.Transfer && p.StoreID == StoreID)
                        s.TransferIn += p.Quantity;
                    else if (p.RecordType == RecordType.Transfer && p.FromStoreID == StoreID)
                        s.TransferOut += p.Quantity;
                    else if (p.RecordType == RecordType.Return)
                        s.Return += p.Quantity;
                    else if (p.RecordType == RecordType.Damage)
                        s.Damage += p.Quantity;
                }

                foreach (BillingLib.BillingItem b in billingItem.Where(m => m.ItemID == item.ID))
                {
                    s.Sale += b.Quantity;
                }

                s.StoreID = StoreID;
                s.StockMonth = startDate;

                //s.AutoOpeningStock = s.StockCount;
                s.AutoClosingStock = s.StockCount;

                //next month update
                BillingLib.Stock ns = listNext.FirstOrDefault(m => m.ItemID == s.ItemID);
                if (ns == null)
                {
                    ns = new BillingLib.Stock();
                    ns.ItemID = s.ItemID;
                    ns.StoreID = s.StoreID;
                    ns.StockMonth = NextMonthOpening;
                    listNext.Add(ns);
                }

                ns.AutoOpeningStock = (s.ClosingStock != 0 ? (s.ClosingStock < 0 ? 0 : s.ClosingStock) : (s.AutoClosingStock < 0 ? 0 : s.AutoClosingStock));
                //if (ns.OpeningStock < 0)
                //ns.OpeningStock = 0;
            }

            if (SaveToDB)
            {
                Stock.Save(list);
                Stock.Save(listNext);
            }
            return list;
        }
    }
}