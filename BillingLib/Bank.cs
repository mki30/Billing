using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingLib
{
    public class Bank
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string AccountNo { get; set; }
        
        static Bank GetRecord(IDataReader dr)
        {
            return new Bank()
            {
                ID = Cmn.ToInt(dr["ID"]),
                Name = dr["Name"].ToString(),
                Address = dr["Address"].ToString(),
                AccountNo = dr["AccountNo"].ToString(),
            };
        }

        public static List<Bank> GetAll(int ID = 0)
        {
            List<Bank> list = new List<Bank>();
            DatabaseCE db = new DatabaseCE();
            try
            {
                string Error = "";

                IDataReader dr = db.GetDataReader("select * from Bank where 1=1" + (ID == 0 ? "" : " and ID=" + ID) + " order by Name", ref Error);
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

        public static Bank GetByName(string Name)
        {
            Name = Cmn.ProperCase(Name);
            List<Bank> list = new List<Bank>();
            DatabaseCE db = new DatabaseCE();
            try
            {
                string Error = "";

                IDataReader dr = db.GetDataReader("select * from Bank  where Name='" + Name + "'", ref Error);
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
                data.Add("Address", Address);
                data.Add("AccountNo", AccountNo);
                TableID.Save("Bank", data);
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
