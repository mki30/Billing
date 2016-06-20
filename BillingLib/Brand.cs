using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingLib
{ 
    public class Brand
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public string NameLower
        {
            get
            {
                return Name.ToLower();
            }
        }

        static Brand GetRecord(IDataReader dr)
        {
            return new Brand()
            {
                ID = Cmn.ToInt(dr["ID"]),
                Name = dr["Name"].ToString(),
            };
        }

        public static List<Brand> GetAll()
        {
            List<Brand> list = new List<Brand>();
            DatabaseCE db = new DatabaseCE();
            try
            {
                string Error = "";

                IDataReader dr = db.GetDataReader("select * from Brand order by Name", ref Error);

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

        public static Brand GetByID(int ID)
        {
            DatabaseCE db = new DatabaseCE();
            try
            {
                string Error = "";
                IDataReader dr = db.GetDataReader("select * from Brand where ID=" + ID, ref Error);
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

        public static Brand GetByName(string Name)
        {
            Name = Cmn.ProperCase(Name);
            List<Brand> list = new List<Brand>();
            DatabaseCE db = new DatabaseCE();
            try
            {
                string Error = "";

                IDataReader dr = db.GetDataReader("select * from Brand  where Name='" + Name + "'", ref Error);
                while (dr.Read())
                {
                    list.Add(GetRecord(dr));
                }
            }
            finally
            {
                db.Close();
            }

            return list.FirstOrDefault();
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
                TableID.Save("Brand", data);
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
