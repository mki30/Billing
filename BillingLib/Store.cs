using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingLib
{
    public class Store
    {
        public int ID { get; set; }
        public int CompanyID { get; set; }
        public string Name { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Phone { get; set; }
        public bool IsFranchise { get; set; }
        
        public List<Terminal> TerminalList = new List<Terminal>();

        public static Store GetRecord(IDataReader dr)
        {
            return new Store()
            {
                ID = Cmn.ToInt(dr["ID"]),
                CompanyID = Cmn.ToInt(dr["CompanyID"]),
                Name = dr["Name"].ToString(),
                Address1 = dr["Address1"].ToString(),
                Address2 = dr["Address2"].ToString(),
                Phone = dr["Phone"].ToString(),
                IsFranchise = Cmn.ToInt(dr["IsFranchise"])==1,
            };
        }

        public static Store GetByID(int ID)
        {
            string vError = string.Empty;
            DatabaseCE db = new DatabaseCE();
            try
            {
                string Error = "";
                IDataReader dr = db.GetDataReader("select * from Store where ID=" + ID, ref Error);
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

        public static Dictionary<int, Store> GetAll()
        {
            Dictionary<int, Store> list = new Dictionary<int, Store>();
            DatabaseCE db = new DatabaseCE();
            try
            {
                string Error = "";

                IDataReader dr = db.GetDataReader("select * from Store", ref Error);

                while (dr.Read())
                {
                    Store s = GetRecord(dr);
                    list.Add(s.ID, s);
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
                data.Add("CompanyID", CompanyID);
                data.Add("Name", Name);
                data.Add("Address1", Address1);
                data.Add("Address2", Address2);
                data.Add("Phone", Phone);
                data.Add("IsFranchise", IsFranchise?1:0);
                TableID.Save("Store", data, db);
                ID = Cmn.ToInt(data["ID"]);
            
                //if (!Global.listStore.ContainsKey(ID))
                //    Global.listStore.Add(ID, this);
                //else
                //    Global.listStore[ID] = this;
            }
            catch
            {
            }
            finally
            {
                db.Close();
            }
        }

        public static List<Store> GetByCompanyID(int companyID)
        {
            List<Store> list = new List<Store>();
            DatabaseCE db = new DatabaseCE();
            try
            {
                string Error = "";

                IDataReader dr = db.GetDataReader("select * from Store where CompanyID=" + companyID, ref Error);

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

        public static List<Store> GetByCompayID(int companyID = 0)
        {
            List<Store> list = new List<Store>();
            DatabaseCE db = new DatabaseCE();
            try
            {
                string Error = "";
                IDataReader dr = db.GetDataReader("select * from Store " + (companyID == 0 ? "" : " where companyID=" + companyID), ref Error);
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
