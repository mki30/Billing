using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingLib
{
    public class ItemStorePrice
    {
        public int ID { get; set; }
        public int ItemID { get; set; }
        public int StoreID { get; set; }
        public double Rate { get; set; }
        public double MRP { get; set; }
        
        static ItemStorePrice GetRecord(IDataReader dr)
        {
            return new ItemStorePrice()
            {
                ID = Cmn.ToInt(dr["ID"]),
                ItemID = Cmn.ToInt(dr["ItemID"]),
                StoreID = Cmn.ToInt(dr["StoreID"]),
                Rate = Cmn.ToDbl(dr["Rate"]),
                MRP = Cmn.ToDbl(dr["MRP"])
            };
        }
        
        public void Save(DatabaseCE _db = null)
        {
            DatabaseCE db = _db == null ? new DatabaseCE() : _db;
            
            try
            {
                Dictionary<string, object> data2 = new Dictionary<string, object>();
                data2.Add("ID", ID);
                data2.Add("ItemID", ItemID);
                data2.Add("StoreID", StoreID);
                data2.Add("Rate", Rate);
                data2.Add("MRP", MRP);
                TableID.Save("ItemStorePrice", data2, db);
                ID = Cmn.ToInt(data2["ID"]);
            }
            finally
            {
                if (_db == null)
                    db.Close();
            }
        }

        public static void UpdateAll(List<ItemStorePrice> list)
        {
            DatabaseCE db = new DatabaseCE();
            try
            {
                db.RunQuery("delete from ItemStorePrice");
                foreach (ItemStorePrice i in list)
                    i.Save(db);
            }
            finally
            {
                db.Close();
            }
        }

        public static ItemStorePrice GetByID(int ID)
        {
            return GetAll(ID).First();
        }
        public static List<ItemStorePrice> GetAll(int ID = 0,int StoreID=0)
        {
            List<ItemStorePrice> list = new List<ItemStorePrice>();
            DatabaseCE db = new DatabaseCE();
            try
            {
                string Error = "";

                IDataReader dr = db.GetDataReader("select * from ItemStorePrice " + (ID == 0 ? "" : " where ID=" + ID 
                    + (StoreID != 0 ? " and StoreID=" + StoreID : "")),ref Error);
                
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

        public static List<ItemStorePrice> GetByStoreID(int StoreID)
        {
            List<ItemStorePrice> list = new List<ItemStorePrice>();
            DatabaseCE db = new DatabaseCE();
            try
            {
                string Error = "";
                IDataReader dr = db.GetDataReader("select * from ItemStorePrice  where StoreID=" + StoreID, ref Error);
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

        public static ItemStorePrice GetByItemID(int ItemID,int StoreID)
        {
            DatabaseCE db = new DatabaseCE();
            try
            {
                string Error = "";
                IDataReader dr = db.GetDataReader("select * from ItemStorePrice where ItemID=" + ItemID + " and storeID="+StoreID+"", ref Error);
                while (dr.Read())
                {
                    ItemStorePrice item = ItemStorePrice.GetRecord(dr);
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
        
        //public static ItemStorePrice GetByItemIDandStoreID(int itemID, int storeID=0)
        //{
        //    DatabaseCE db = new DatabaseCE();
        //    try
        //    {
        //        string Error = "";
        //        IDataReader dr = db.GetDataReader("select * from ItemStorePrice where ItemID=" + itemID + " and StoreID=" + storeID, ref Error);
        //        while (dr.Read())
        //        {
        //            ItemStorePrice item = ItemStorePrice.GetRecord(dr);
        //            return item;
        //        }
        //    }
        //    catch
        //    {
        //    }
        //    finally
        //    {
        //        db.Close();
        //    }
        //    return null;
        //}
    }
}