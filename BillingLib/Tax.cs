using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingLib
{
    public class Tax
    {
        public int ID { get; set; }
        public double Rate { get; set; }

        static Tax GetRecord(IDataReader dr)
        {
            return new Tax()
            {
                ID = Cmn.ToInt(dr["ID"]),
                Rate = Cmn.ToDbl(dr["Rate"]), 
            };
        }

        public static Dictionary<int, Tax> GetAll(int ID = 0)
        {
            Dictionary<int, Tax> list = new Dictionary<int, Tax>();
            DatabaseCE db = new DatabaseCE();
            try
            {
                string Error = "";

                IDataReader dr = db.GetDataReader("select * from TaxRate " + (ID == 0 ? "" : " where ID=" + ID) + " order by Rate", ref Error);

                while (dr.Read())
                {
                    Tax t = GetRecord(dr);
                    list.Add(t.ID,t);
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
                data.Add("Rate", Rate);
                TableID.Save("TaxRate", data, db);
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
        
        public static Tax GetByID(int ID)
        {
            DatabaseCE db = new DatabaseCE();
            try
            {
                string Error = "";

                IDataReader dr = db.GetDataReader("select * from TaxRate where ID=" + ID, ref Error);

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
    }
}