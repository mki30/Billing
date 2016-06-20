using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingLib
{
    public class Terminal
    {
        public int ID;
        public int IsVirtual;
        
        public int BillNo;
        public int CompanyID;
        public int Prefix;
        public string Name="";
        public int StoreID;
        public string Address="";
        public Boolean IsOnline=false;
        public DateTime LastAccess = Cmn.MinDate;
        public string User ="";
        public string SoftwareVersion ="";


        static Terminal GetRecord(IDataReader dr)
        {
            return new Terminal()
            {
                ID = Cmn.ToInt(dr["ID"]),
                IsVirtual = Cmn.ToInt(dr["IsVirtual"]),
                CompanyID = Cmn.ToInt(dr["CompanyID"]),
                BillNo = Cmn.ToInt(dr["BillNo"]),
                Prefix = Cmn.ToInt(dr["Prefix"]),
                Name = dr["Name"].ToString(),
                StoreID = Cmn.ToInt(dr["StoreID"]),
                Address = dr["Address"].ToString(),
                LastAccess = Cmn.ToDate(dr["LastAccess"]),
                User = dr["User"].ToString(),
                SoftwareVersion = dr["SoftwareVersion"].ToString()
            };
        }

        public static Dictionary<int, Terminal> GetAll(int ID = 0)
        {
            Dictionary<int, Terminal> list = new Dictionary<int, Terminal>();
            DatabaseCE db = new DatabaseCE();
            try
            {
                string Error = "";
                IDataReader dr = db.GetDataReader("select * from Terminal " + (ID == 0 ? "" : " where ID=" + ID), ref Error);
                while (dr.Read())
                {
                    Terminal t = GetRecord(dr);
                    list.Add(t.ID, t);
                }
            }
            finally
            {
                db.Close();
            }
            return list;
        }

        public Terminal Save(Boolean SentFromClient = false)
        {
            DatabaseCE db = new DatabaseCE();
            try
            {
                Dictionary<string, object> data = new Dictionary<string, object>();
                data.Add("ID", ID);
                if (!SentFromClient)
                {
                    data.Add("CompanyID", CompanyID);
                    data.Add("Prefix", Prefix);
                    data.Add("Name", Name);
                    data.Add("StoreID", StoreID);
                    data.Add("Address", Address);
                }
                data.Add("IsVirtual", IsVirtual);
                data.Add("BillNo", BillNo);
                data.Add("LastAccess", LastAccess);
                data.Add("User", User);
                data.Add("SoftwareVersion", SoftwareVersion);
                TableID.Save("Terminal", data, db);
                ID = Cmn.ToInt(data["ID"]);
            }
            finally
            {
                db.Close();
            }

            return this;

        }

        public static Terminal GetByID(int ID)
        {
            DatabaseCE db = new DatabaseCE();
            try
            {
                string Error = "";
                IDataReader dr = db.GetDataReader("select * from Terminal where ID=" + ID, ref Error);
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

        public static List<Terminal> GetByStoreID(int StoreID, Boolean IsVirtual = false)
        {
            List<Terminal> list = new List<Terminal>();
            DatabaseCE db = new DatabaseCE();
            try
            {
                string Error = "";
                IDataReader dr = db.GetDataReader("select * from Terminal where StoreID=" + StoreID + (IsVirtual ? " and IsVirtual=1" : ""), ref Error);
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

        public static List<Terminal> GetByCompanyAndStore(int companId, int storeId = 0)
        {
            List<Terminal> list = new List<Terminal>();
            DatabaseCE db = new DatabaseCE();
            try
            {
                string Error = "";
                IDataReader dr = db.GetDataReader("select * from Terminal where CompanyID=" + companId + (storeId == 0 ? "" : " and StoreID=" + storeId), ref Error);
                while (dr.Read())
                {
                    list.Add(GetRecord(dr));
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
    }
}
