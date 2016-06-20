using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingLib
{
    public class Project
    {
        public int ID;
        public string ProjectName { get; set; }
        public string ProjectNameLower = "";
        public string Address = "";
        public string Area = "";
        public string City = "";
        public string State = "";
        public int Pin;
        public int Rank;

        static Project GetRecord(IDataReader dr)
        {
            return new Project()
            {
                ID = Cmn.ToInt(dr["ID"]),
                ProjectName = dr["ProjectName"].ToString(),
                ProjectNameLower = dr["ProjectName"].ToString().ToLower(),
                Address = dr["Address"].ToString(),
                Area = dr["Area"].ToString(),
                City = dr["City"].ToString(),
                State = dr["State"].ToString(),
                Pin = Cmn.ToInt(dr["Pin"]),
            };
        }

        public static List<BillingLib.Project> GetAll(string City = "")
        {
            List<BillingLib.Project> list = new List<BillingLib.Project>();
            DatabaseCE db = new DatabaseCE();
            try
            {
                string Error = "";
                IDataReader dr = db.GetDataReader("select * from Project where 1=1 " + (City.Length > 0 ? " and City='" + City + "'" : "") + " order by ProjectName", ref Error);
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


        public static List<BillingLib.Project> GetAllFromPropertyMap()
        {
            List<BillingLib.Project> list = new List<BillingLib.Project>();
            DatabaseCE db = new DatabaseCE();
            try
            {
                string Error = "";
                IDataReader dr = db.GetDataReader("select * from Project order by ProjectName", ref Error);
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

        public static Project Get(int ID)
        {
            DatabaseCE db = new DatabaseCE();
            try
            {
                string Error = "";
                IDataReader dr = db.GetDataReader("select * from Project where ID=" + ID, ref Error);

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

        public static void Save(List<Project> list)
        {
            string vError = string.Empty;
            DatabaseCE db = new DatabaseCE();
            try
            {
                foreach (Project p in list)
                {
                    p.Save(db);
                }
            }
            finally
            {
                db.Close();
            }
        }

        public void Save(DatabaseCE db_ = null)
        {
            string vError = string.Empty;
            DatabaseCE db = db_ != null ? db_ : new DatabaseCE();
            try
            {
                Dictionary<string, object> data = new Dictionary<string, object>();
                data.Add("ID", ID);
                data.Add("ProjectName", ProjectName);
                data.Add("Address", Address);
                data.Add("Area", Area);
                data.Add("City", City);
                data.Add("State", State);
                data.Add("Pin", Pin);
                TableID.Save("Project", data);
                ID = Cmn.ToInt(data["ID"]);
            }
            catch
            {
            }
            finally
            {
                if (db_ == null)
                    db.Close();
            }
        }

        public static Project GetByID(int ID)
        {
            string vError = string.Empty;
            DatabaseCE db = new DatabaseCE();
            try
            {
                string Error = "";
                IDataReader dr = db.GetDataReader("select * from Project where ID=" + ID, ref Error);
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
    }
}
