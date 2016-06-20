using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingLib
{
    public class VendorDue
    {
        public int VendorID;
        public int StoreID;
        public double Purchase;
        public double Payment;
        public int FinancialYear;

        static VendorDue GetRecord(IDataReader dr)
        {
            return new VendorDue()
            {
                VendorID = Cmn.ToInt(dr["VendorID"]),
                StoreID = Cmn.ToInt(dr["StoreID"]),
                Purchase = Cmn.ToDbl(dr["Purchase"]),
                Payment = Cmn.ToDbl(dr["Payment"]),
                FinancialYear = Cmn.ToInt(dr["FinancialYear"])
            };
        }

        public static List<VendorDue> GetByStore(int StoreID, int FinancialYear)
        {
            List<VendorDue> list = new List<VendorDue>();
            DatabaseCE db = new DatabaseCE();
            try
            {
                string Error = "";

                IDataReader dr = db.GetDataReader("select * from VendorDue where 1=1"
                    + (StoreID == 0 ? "" : " and StoreID=" + StoreID)
                    + " and FinancialYear=" + FinancialYear, ref Error);

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

        public static List<VendorDue> GetByVendor(int VendorID)
        {
            List<VendorDue> list = new List<VendorDue>();
            DatabaseCE db = new DatabaseCE();
            try
            {
                string Error = "";

                IDataReader dr = db.GetDataReader("select * from VendorDue where VendorID=" + VendorID, ref Error);

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
                data.Add("VendorID", VendorID);
                data.Add("StoreID", StoreID);
                data.Add("Purchase", Purchase);
                data.Add("Payment", Payment);
                data.Add("FinancialYear", FinancialYear);
                TableID.Save("VendorDue", data, new string[] { "VendorID", "StoreID", "FinancialYear" }, db);
            }
            catch
            {
            }
            finally
            {
                db.Close();
            }
        }
        
        public VendorDue Delete()
        {
            DatabaseCE db = new DatabaseCE();
            try
            {
                db.RunQuery("Delete from VendorDue where VendorID=" + VendorID + " and StoreID=" + StoreID + " and FinancialYear=" + FinancialYear);
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
    }
}