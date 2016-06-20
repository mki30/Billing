using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingLib
{
    public class Apartment
    {
        public int ID {get; set;}
        public int ProjectID {get; set;}
        public int FloorNo {get; set; }
        public int Status {get; set; }
        public string ResidentName {get; set;}
        public string ResidentPhone {get; set;}
        public string OwnerName {get; set; }
        public string OwnerPhone {get; set;}
        
        static Apartment GetRecord(IDataReader dr)
        {
            return new Apartment()
            {
                ID = Cmn.ToInt(dr["ID"]),
                ProjectID = Cmn.ToInt(dr["ProjectID"]),
                FloorNo = Cmn.ToInt(dr["FlatNo"]),
                Status = Cmn.ToInt(dr["Status"]),
                ResidentName = dr["ResidentName"].ToString(),
                ResidentPhone = dr["ResidentPhone"].ToString(),
                OwnerName = dr["OwnerName"].ToString(),
                OwnerPhone = dr["OwnerPhone"].ToString(),
            };
        }

        public static List<Apartment> GetAll()
        {
            List<Apartment> list = new List<Apartment>();
            DatabaseCE db = new DatabaseCE();
            try
            {
                string Error = "";
                IDataReader dr = db.GetDataReader("select * from Apartment", ref Error);
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
        
        public static Apartment GetByID(int ID)
        {
            DatabaseCE db = new DatabaseCE();
            try
            {
                string Error = "";
                IDataReader dr = db.GetDataReader("select * from Apartment where ID=" + ID, ref Error);
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

        public static List<Apartment> GetByProjectID(int ProjectID)
        {
            List<Apartment> list = new List<Apartment>();
            DatabaseCE db = new DatabaseCE();
            try
            {
                string Error = "";
                IDataReader dr = db.GetDataReader("select * from Apartment where ProjectID=" + ProjectID, ref Error);
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
            string vError = string.Empty;
            DatabaseCE db = new DatabaseCE();
            try
            {
                Dictionary<string, object> data = new Dictionary<string, object>();
                data.Add("ID", ID);
                data.Add("ProjectID", ProjectID);
                data.Add("FloorNo", FloorNo);
                data.Add("Status", Status);
                data.Add("ResidentName", ResidentName);
                data.Add("Facing", ResidentPhone);
                data.Add("OwnerName", OwnerName);
                data.Add("OwnerPhone", OwnerPhone);
                TableID.Save("Apartment", data);
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
