using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingLib
{
    public class PurchasePaymentLink
    {
        public int ID;
        public int VendorID;
        public int PurchaseID;
        public int PaymentID;
        public DateTime PaymentDate = Cmn.MinDate;
        public double Amount;
        public int IsDelete { get; set; }

        static PurchasePaymentLink GetRecord(IDataReader dr)
        {
            return new PurchasePaymentLink()
            {
                ID = Cmn.ToInt(dr["ID"]),
                PurchaseID = Cmn.ToInt(dr["PurchaseID"]),
                VendorID = Cmn.ToInt(dr["VendorID"]),
                PaymentID = Cmn.ToInt(dr["PaymentID"]),
                PaymentDate = Cmn.ToDate(dr["PaymentDate"]),
                Amount = Cmn.ToDbl(dr["Amount"]),
                IsDelete = Cmn.ToInt(dr["IsDelete"])
            };
        }

        public static List<PurchasePaymentLink> GetByPayment(int PaymentID)
        {
            List<PurchasePaymentLink> list = new List<PurchasePaymentLink>();
            DatabaseCE db = new DatabaseCE();
            try
            {
                string Error = "";

                IDataReader dr = db.GetDataReader("select * from PurchasePaymentLink where PaymentID=" + PaymentID, ref Error);

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

        public static List<PurchasePaymentLink> GetByVendor(int VendorID)
        {
            List<PurchasePaymentLink> list = new List<PurchasePaymentLink>();
            DatabaseCE db = new DatabaseCE();
            try
            {
                string Error = "";

                IDataReader dr = db.GetDataReader("select * from PurchasePaymentLink where VendorID=" + VendorID, ref Error);

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
                data.Add("ID", ID);
                data.Add("PurchaseID", PurchaseID);
                data.Add("VendorID", VendorID);

                data.Add("PaymentID", PaymentID);
                data.Add("PaymentDate", PaymentDate);
                data.Add("Amount", Amount);
                data.Add("IsDelete", IsDelete);
                TableID.Save("PurchasePaymentLink", data, db);
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

        public static PurchasePaymentLink GetByID(int ID)
        {
            DatabaseCE db = new DatabaseCE();
            try
            {
                string Error = "";
                IDataReader dr = db.GetDataReader("select * from PurchasePaymentLink where ID='" + ID + "'", ref Error);
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

        public PurchasePaymentLink Delete()
        {
            DatabaseCE db = new DatabaseCE();
            try
            {
                db.RunQuery("Delete from PurchasePaymentLink where id=" + ID);
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

        public static int MarkDelete(int PaymentID,int IsDelete)
        {
            DatabaseCE db = new DatabaseCE();
            try
            {
                return db.RunQuery("Update PurchasePaymentLink set Isdelete=" + IsDelete+" where paymentid="+PaymentID);
            }
            catch
            {
            }
            finally
            {
                db.Close();
            }
            return 0;
        }

        internal static List<PurchasePaymentLink> GetByVendorIDandDate(int VendorID, DateTime DateFrom, DateTime DateTo)
        {
            List<PurchasePaymentLink> list = new List<PurchasePaymentLink>();
            DatabaseCE db = new DatabaseCE();
            try
            {
                string Error = "";
                IDataReader dr = db.GetDataReader("select * from PurchasePaymentLink where 1=1 "
                    + (VendorID != 0 ? " and VendorID=" + VendorID : "")
                    + " and PaymentDate>='" + DateFrom.ToString("dd-MMM-yyyy") + "'"
                    + " and PaymentDate<='" + DateTo.ToString("dd-MMM-yyyy") + "'"
                    + " order by PaymentDate desc", ref Error);
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

        internal static List<PurchasePaymentLink> GetByPaymentIDs(string paymentIds)
        {
            List<PurchasePaymentLink> list = new List<PurchasePaymentLink>();
            if (paymentIds == "") return list;
            DatabaseCE db = new DatabaseCE();
            try
            {
                string Error = "";
                IDataReader dr = db.GetDataReader("select * from PurchasePaymentLink where PaymentID IN ("+paymentIds+")", ref Error);
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
    }
}