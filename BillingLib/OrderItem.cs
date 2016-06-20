using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingLib
{
    public class OrderItem
    {
        public int ID;
        public int ItemID;
        public double Quantity;
        public double Price;
        public int OrderID;
        public string ItemName { get; set; }

        static OrderItem GetRecord(IDataReader dr)
        {
            return new OrderItem()
            {
                ID = Cmn.ToInt(dr["ID"]),
                ItemID = Cmn.ToInt(dr["ItemID"]),
                OrderID = Cmn.ToInt(dr["OrderID"]),
                ItemName = dr["ItemName"].ToString(),
                Quantity = Cmn.ToDbl(dr["Quantity"]),
                Price = Cmn.ToDbl(dr["Price"]),
            };
        }

        public static List<OrderItem> GetAll()
        {
            List<OrderItem> list = new List<OrderItem>();
            DatabaseCE db = new DatabaseCE();
            try
            {
                string Error = "";

                IDataReader dr = db.GetDataReader("select * from OrderItem", ref Error);

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

        public static OrderItem GetByID(int ID)
        {
            DatabaseCE db = new DatabaseCE();
            try
            {
                string Error = "";
                IDataReader dr = db.GetDataReader("select * from OrderItem where ID=" + ID, ref Error);
                while (dr.Read())
                {
                    return GetRecord(dr);
                }
            }
            finally
            {
                db.Close();
            }
            return null;
        }

        public static List<OrderItem> GetByOrderID(int OrderID)
        {
            List<OrderItem> list = new List<OrderItem>();
            DatabaseCE db = new DatabaseCE();
            try
            {
                string Error = "";

                IDataReader dr = db.GetDataReader("select * from OrderItem where OrderID="+OrderID, ref Error);

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

        public void Save()
        {
            string vError = string.Empty;
            DatabaseCE db = new DatabaseCE();
            try
            {
                Dictionary<string, object> data = new Dictionary<string, object>();
                data.Add("ID", ID);
                data.Add("ItemID", ItemID);
                data.Add("OrderID", OrderID);
                data.Add("ItemName", ItemName);
                data.Add("Quantity", Quantity);
                data.Add("Price", Price);
                TableID.Save("OrderItem", data);
                ID = Cmn.ToInt(data["ID"]);
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

