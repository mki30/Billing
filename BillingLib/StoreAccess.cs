using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingLib
{
    public class StoreAccess
    {
        public int StoreID { get; set; }
        public int UserID { get; set; }

        static StoreAccess GetRecord(IDataReader dr)
        {
            return new StoreAccess()
            {
                StoreID = Cmn.ToInt(dr["StoreID"]),
                UserID = Cmn.ToInt(dr["UserID"]),
            };
        }

        public static List<StoreAccess> GetAll()
        {
            List<StoreAccess> list = new List<StoreAccess>();
            DatabaseCE db = new DatabaseCE();
            try
            {
                string Error = "";
                IDataReader dr = db.GetDataReader("select * from StoreAccess", ref Error);
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
            DatabaseCE db = new DatabaseCE();
            try
            {
                Dictionary<string, object> data = new Dictionary<string, object>();
                data.Add("StoreID", StoreID);
                data.Add("UserID", UserID);
                TableID.Save("StoreAccess", data, new[] { "StoreID", "UserID" }, db);
            }
            catch
            {
            }
            finally
            {
                db.Close();
            }
        }
        public static StoreAccess GetByUseridAndStoreid(int userID, int storeID)
        {
            DatabaseCE db = new DatabaseCE();
            try
            {
                string Error = "";
                IDataReader dr = db.GetDataReader("select * from StoreAccess where StoreID=" + storeID + " and UserID=" + userID, ref Error);
                while (dr.Read())
                {
                    return StoreAccess.GetRecord(dr);
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
        public static List<Store> GetByUserID(int UserID)
        {
            List<Store> list = new List<Store>();
            DatabaseCE db = new DatabaseCE();
            try
            {
                string Error = "";
                IDataReader dr = db.GetDataReader("select * from store where id in ( select Storeid from StoreAccess where UserID=" + UserID + ")", ref Error);

                while (dr.Read())
                {
                    list.Add(Store.GetRecord(dr));
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

        public void Delete()
        {
            DatabaseCE db = new DatabaseCE();
            try
            {
                db.RunQuery("Delete from StoreAccess where UserID=" + UserID + " and StoreID=" + StoreID + "");
            }
            catch
            {

            }
            finally
            {
                db.Close();
            }

        }

        public static void DeleteAll(int UserID)
        {
            DatabaseCE db = new DatabaseCE();
            try
            {
                db.RunQuery("Delete from StoreAccess where UserID=" + UserID);
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
