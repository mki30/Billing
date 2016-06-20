using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingLib
{
    public class Menu
    {
        public int ID { get; set; }
        public int ParentID { get; set; }
        public string Name { get; set; }
        public string UrlName { get; set; }
        public string NameLower
        {
            get
            {
                return Name.ToLower();
            }
        }
        
        public List<Menu> ChildMenu = new List<Menu>();
        public Menu Parent=null;

        static Menu GetRecord(IDataReader dr)
        {
            return new Menu()
            {
                ID = Cmn.ToInt(dr["ID"]),
                ParentID = Cmn.ToInt(dr["ParentID"]),
                Name = dr["Name"].ToString(),
                UrlName = dr["UrlName"].ToString(),
            };
        }

        public static List<Menu> GetAll(int ID = 0,bool onlyTopLevel=false)
        {
            List<Menu> list = new List<Menu>();
            DatabaseCE db = new DatabaseCE();
            try
            {
                string Error = "";
                IDataReader dr = db.GetDataReader("select * from Menu where 1=1" + (ID == 0 ? "" : " and ID=" + ID)+(onlyTopLevel == true ? "" : " and parentID=0"), ref Error);
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
        
        public static List<Menu> GetByName(string Name)
        {
            List<Menu> list = new List<Menu>();
            DatabaseCE db = new DatabaseCE();
            try
            {
                string Error = "";
                IDataReader dr = db.GetDataReader("select * from Menu where Name=" + Name, ref Error);
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

        public static List<Menu> GetByParentID(int ParentID = 0)
        {
            List<Menu> list = new List<Menu>();
            DatabaseCE db = new DatabaseCE();
            try
            {
                string Error = "";
                IDataReader dr = db.GetDataReader("select * from Menu where ParentID=" + ParentID  , ref Error);
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

        public void Save(DatabaseCE _db = null)
        {
            string vError = string.Empty;
            DatabaseCE db = _db == null ? new DatabaseCE() : _db;
            try
            {
                Dictionary<string, object> data = new Dictionary<string, object>();
                data.Add("ID", ID);
                data.Add("ParentID", ParentID);
                data.Add("Name",Name);
                data.Add("UrlName", UrlName);
                TableID.Save("Menu", data, db);
                ID = Cmn.ToInt(data["ID"]);
            }
            catch
            {
            }
            finally
            {
                if (_db == null)
                db.Close();
            }
        }

        public static Menu GetByID(int id)
        {
            string vError = string.Empty;
            DatabaseCE db = new DatabaseCE();
            try
            {
                string Error = "";
                IDataReader dr = db.GetDataReader("select * from Menu where ID=" + id, ref Error);
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

        public static void UpdateAll(List<Menu> list)
        {
            DatabaseCE db = new DatabaseCE();
            try
            {
                db.RunQuery("delete from Menu");
                foreach (Menu m in list)
                    m.Save(db);
            }
            finally
            {
                db.Close();
            }
        }
    }

}