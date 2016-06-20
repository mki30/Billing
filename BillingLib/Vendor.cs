using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingLib
{
    public class Vendor
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Area { get; set; }
        public string City { get; set; }
        public int PIN { get; set; }
        public string Mobile { get; set; }
        public string TIN { get; set; }
        public double AmountDue { get; set; }
        public double AmountPaid { get; set; }
        public bool IsClient = false;
        public int State { get; set; }


        static Vendor GetRecord(IDataReader dr)
        {
            return new Vendor()
            {
                ID = Cmn.ToInt(dr["ID"]),
                Name = dr["Name"].ToString(),
                Address = dr["Address"].ToString(),
                Area = dr["Area"].ToString(),
                City = dr["City"].ToString(),
                PIN = Cmn.ToInt(dr["PIN"]),
                Mobile = dr["Mobile"].ToString(),
                TIN = dr["TIN"].ToString(),
                AmountDue = Cmn.ToDbl(dr["AmountDue"]),
                AmountPaid = Cmn.ToDbl(dr["AmountPaid"]),
                IsClient = Cmn.ToInt(dr["IsClient"]) == 1 ? true : false,
                State = Cmn.ToInt(dr["State"])
            };
        }

        public Vendor UpdateAmountDue()
        {
            string vError = string.Empty;
            DatabaseCE db = new DatabaseCE();
            try
            {
                string Error = "";
                foreach (Store s in Global.StoreList.Values)
                {
                    double Purchase = Cmn.ToDbl(db.GetFieldValue("SELECT SUM(Amount) FROM Purchase WHERE VendorID = " + ID + " and StoreID=" + s.ID, ref Error));
                    double Payment = Cmn.ToDbl(db.GetFieldValue("select sum(Cash+Cheque) from PurchasePayment where VendorID=" + ID + " and StoreID=" + s.ID + " and (Isdelete <> 1 or Isdelete IS null)", ref Error));

                    new VendorDue()
                    {
                        VendorID = ID,
                        StoreID = s.ID,
                        FinancialYear = 2015,
                        Purchase = Purchase,
                        Payment = Payment
                    }.Save();

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

        public static List<Vendor> GetAll(int ID = 0)
        {
            List<Vendor> list = new List<Vendor>();
            DatabaseCE db = new DatabaseCE();
            try
            {
                string Error = "";

                IDataReader dr = db.GetDataReader("select * from Vendor where 1=1" + (ID == 0 ? "" : " and ID=" + ID)
                    + " order by Name", ref Error);

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

        public static List<Vendor> GetByCompanyID()
        {
            List<Vendor> list = new List<Vendor>();
            DatabaseCE db = new DatabaseCE();
            try
            {
                string Error = "";

                IDataReader dr = db.GetDataReader("select * from Vendor order by Name", ref Error);

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
        public static Vendor GetByID(int id)
        {
            DatabaseCE db = new DatabaseCE();
            try
            {
                string Error = "";

                IDataReader dr = db.GetDataReader("select * from Vendor where ID=" + id, ref Error);

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

        public void Save()
        {
            string vError = string.Empty;
            DatabaseCE db = new DatabaseCE();
            try
            {
                Dictionary<string, object> data = new Dictionary<string, object>();
                data.Add("ID", ID);
                data.Add("Name", Name);
                data.Add("Address", Address);
                data.Add("Area", Area);
                data.Add("City", City);
                data.Add("PIN", PIN);
                data.Add("Mobile", Mobile);
                data.Add("TIN", TIN);
                data.Add("AmountDue", AmountDue);
                data.Add("AmountPaid", AmountPaid);
                data.Add("IsClient", IsClient == true ? 1 : 0);
                data.Add("State", State);
                TableID.Save("Vendor", data);
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


