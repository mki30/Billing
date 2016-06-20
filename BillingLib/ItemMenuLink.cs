using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingLib
{
    public class ItemMenuLink
    {
        public int ID { get; set; }
        public int ItemID { get; set; }
        public int MenuID { get; set; }
        public int Rank { get; set; }
        public double SerialNo { get; set; }


        static ItemMenuLink GetRecord(IDataReader dr)
        {
            return new ItemMenuLink()
            {
                ID = Cmn.ToInt(dr["ID"]),
                ItemID = Cmn.ToInt(dr["ItemID"]),
                MenuID = Cmn.ToInt(dr["MenuID"]),
                Rank = Cmn.ToInt(dr["Rank"]),
                //SerialNo = Cmn.ToDbl(dr["SerialNo"])
            };
        }

        public static List<ItemMenuLink> GetAll(int ID = 0)
        {
            List<ItemMenuLink> list = new List<ItemMenuLink>();
            DatabaseCE db = new DatabaseCE();
            try
            {
                string Error = "";
                IDataReader dr = db.GetDataReader("select * from ItemMenuLink " + (ID == 0 ? "" : " where ID=" + ID), ref Error);
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

        public static ItemMenuLink GetByID(int ID)
        {
            DatabaseCE db = new DatabaseCE();
            try
            {
                string Error = "";
                IDataReader dr = db.GetDataReader("select * from ItemMenuLink where ID=" + ID , ref Error);
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

        public static List<ItemMenuLink> GetByMenuID(int menuID)
        {
            List<ItemMenuLink> list = new List<ItemMenuLink>();
            DatabaseCE db = new DatabaseCE();
            try
            {
                string Error = "";
                IDataReader dr = db.GetDataReader("select * from ItemMenuLink where MenuID=" + menuID, ref Error);
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

        public static List<ItemMenuLink> GetByItemID(int itemID)
        {
            List<ItemMenuLink> list = new List<ItemMenuLink>();
            DatabaseCE db = new DatabaseCE();
            try
            {
                string Error = "";
                IDataReader dr = db.GetDataReader("select * from ItemMenuLink where ItemID=" + itemID, ref Error);
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

        public static ItemMenuLink GetByItemIDAndMenuID(int itemID,int MenuID)
        {
            DatabaseCE db = new DatabaseCE();
            try
            {
                string Error = "";
                IDataReader dr = db.GetDataReader("select * from ItemMenuLink where ItemID=" + itemID + " and MenuID=" + MenuID, ref Error);
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

        public string Link()
        {
            string Error = string.Empty;
            DatabaseCE db = new DatabaseCE();
            try
            {
                db.RunQuery("delete from ItemMenuLink where ItemID=" + ItemID);
                Save();
            }
            catch (Exception ex)
            {
                Error += ex.Message;
            }
            finally
            {
                db.Close();
            }
            return Error;

        }

        public string Save(DatabaseCE _db = null)
        {
            string Error = string.Empty;
            DatabaseCE db = _db == null ? new DatabaseCE() : _db;
            try
            {
                Dictionary<string, object> data = new Dictionary<string, object>();
                data.Add("ID", ID);
                data.Add("ItemID", ItemID);
                data.Add("MenuID", MenuID);
                data.Add("Rank", Rank);
                data.Add("SerialNo", SerialNo);
                Error=TableID.Save("ItemMenuLink", data, db);
                ID = Cmn.ToInt(data["ID"]);
            }
            catch(Exception ex)
            {
                Error+=ex.Message;
            }
            finally
            {
                if (_db == null)
                db.Close();
            }
            return Error;
        }

        public ItemMenuLink Delete()
        {
            DatabaseCE db = new DatabaseCE();
            try
            {
                db.RunQuery("Delete from ItemMenuLink where id=" + ID);
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
        
        public static void UpdateAll(List<ItemMenuLink> list)
        {
            DatabaseCE db = new DatabaseCE();
            try
            {
                db.RunQuery("delete from ItemMenuLink");
                foreach (ItemMenuLink i in list)
                    i.Save(db);
            }
            finally
            {
                db.Close();
            }
        }


        //internal static List<Item> GetItemList(int menuID)
        //{
        //    List<Item> itemlist = new List<Item>();
        //    ItemMenuLink.GetByMenuID(menuID);
        //    return itemlist;
        //}
    }

}