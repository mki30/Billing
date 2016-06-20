using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingLib
{
    public class Customer
    {
        public int ID;
        public int StoreID;
        public string Name = "";
        public string Address = "";
        public string HouseNumber = "";
        public string Area = "";
        public string City = "";
        public int PIN;
        public string Mobile = "";
        public int Points;
        public int PreviousPoints;
        public DateTime Birthday = Cmn.MinDate;
        public DateTime Anniverary = Cmn.MinDate;

        public int ProjectID;
        public int ApartmentID;
        public DateTime LastSaleDate = Cmn.MinDate;
        public DateTime LastAdvertizedDate = Cmn.MinDate;
        public double TotalAmountSale;

        public string BirthdayString
        {
            get
            {
                return Birthday.ToString("%d-MMM-yy") == "1-Jan-00" ? "" : Birthday.ToString("%d-MMM-yy");
            }
        }

        public string AnniveraryString
        {
            get
            {
                return Anniverary.ToString("%d-MMM-yy") == "1-Jan-00" ? "" : Anniverary.ToString("%d-MMM-yy");
            }
        }

        public string LastAdvertizedDateString
        {
            get
            {
                return LastAdvertizedDate.ToString("%d-MMM-yy") == "1-Jan-00" ? "" : LastAdvertizedDate.ToString("%d-MMM-yy");
            }
        }

        public Customer()
        {
        }

        static Customer GetRecord(IDataReader dr)
        {
            return new Customer()
            {
                ID = Cmn.ToInt(dr["ID"]),
                StoreID = Cmn.ToInt(dr["StoreID"]),
                Name = dr["Name"].ToString(),
                Address = dr["Address"].ToString(),
                Area = dr["Area"].ToString(),
                City = dr["City"].ToString(),
                PIN = Cmn.ToInt(dr["PIN"]),
                Mobile = dr["Mobile"].ToString(),
                HouseNumber = dr["HouseNumber"].ToString(),
                Birthday = Cmn.ToDate(dr["Birthday"]),
                Anniverary = Cmn.ToDate(dr["Anniverary"]),
                ProjectID = Cmn.ToInt(dr["ProjectID"]),
                ApartmentID = Cmn.ToInt(dr["ApartmentID"]),
                LastSaleDate = Cmn.ToDate(dr["LastSaleDate"]),
                LastAdvertizedDate = Cmn.ToDate(dr["LastAdvertizedDate"]),
                TotalAmountSale = Cmn.ToDbl(dr["TotalAmountSale"])
            };
        }

        public static List<Customer> GetAll(int ID = 0, int StoreID = 0)
        {
            List<Customer> list = new List<Customer>();
            DatabaseCE db = new DatabaseCE();
            try
            {
                string Error = "";
                IDataReader dr = db.GetDataReader("select * from Customer where 1=1" + (ID == 0 ? "" : " and ID=" + ID)
                + (StoreID == 0 ? "" : " and StoreID=" + StoreID + " or Storeid is null"), ref Error);
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

        public static List<Customer> Search(string term)
        {
            List<Customer> list = new List<Customer>();
            DatabaseCE db = new DatabaseCE();
            try
            {
                string Error = "";
                IDataReader dr = db.GetDataReader("select * from Customer  where Name like=" + "%" + term, ref Error);
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

            Customer C = Customer.GetByID(ID);
            Dictionary<string, object> data = new Dictionary<string, object>();
            data.Add("ID", ID);
            data.Add("StoreID", StoreID);
            data.Add("Name", Name);
            data.Add("Address", Address);
            data.Add("City", City);
            data.Add("PIN", PIN);
            data.Add("Mobile", Mobile);
            data.Add("Area", Area);
            data.Add("Points", Points);
            data.Add("PreviousPoints", PreviousPoints);
            data.Add("HouseNumber", HouseNumber);
            data.Add("Birthday", Birthday);
            data.Add("Anniverary", Anniverary);
            data.Add("ProjectID", ProjectID);
            data.Add("ApartmentID", ApartmentID);
            data.Add("LastSaleDate", LastSaleDate);
            data.Add("LastAdvertizedDate", LastAdvertizedDate);
            data.Add("TotalAmountSale", TotalAmountSale);

            if (data["Name"].ToString() != "" || data["Mobile"].ToString() != "")
            {
                TableID.Save("Customer", data);
                ID = Cmn.ToInt(data["ID"]);
            }
        }

        public void Save(Billing bill)
        {
            List<Customer> listCust = new Customer() { Mobile = bill.customer.Mobile }.Find();
            if (listCust.Count > 0)
            {
                Customer cust = listCust[0];
                bill.customer.ID = cust.ID;
                bill.customer.ProjectID = cust.ProjectID;
                bill.customer.ApartmentID = cust.ApartmentID;
            }
            bill.customer.StoreID = bill.store.ID;
            bill.customer.LastSaleDate = DateTime.Now;
            bill.customer.TotalAmountSale += bill.TotalAmount;
            bill.customer.Save();
        }

        public List<Customer> Find()
        {
            List<Customer> list = new List<Customer>();
            DatabaseCE db = new DatabaseCE();
            try
            {
                string Error = "";

                IDataReader dr = db.GetDataReader("select * from Customer where 1=1 " +
                    (Mobile.Length > 0 ? "and Mobile='" + Mobile + "'" : "")
                    + (HouseNumber.Length > 0 ? "and HouseNumber='" + HouseNumber + "'" : "")
                    + (Name.Length > 0 ? "and Name like '" + Name + "%'" : "")

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

        public static Customer GetByID(int id)
        {
            string vError = string.Empty;
            DatabaseCE db = new DatabaseCE();
            try
            {
                string Error = "";
                IDataReader dr = db.GetDataReader("select * from Customer where ID=" + id, ref Error);
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

        public Customer Delete()
        {
            DatabaseCE db = new DatabaseCE();
            try
            {
                db.RunQuery("Delete from Customer where id=" + ID);
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

        public static List<Customer> GetByProjectAndApartment(int ProjectID, int ApartmentID)
        {
            List<Customer> list = new List<Customer>();
            DatabaseCE db = new DatabaseCE();
            try
            {
                string Error = "";
                IDataReader dr = db.GetDataReader("select * from Customer  where ProjectID =" + ProjectID
                     + (ApartmentID != 0 ? " and ApartmentID=" + ApartmentID : "")
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
    }
}