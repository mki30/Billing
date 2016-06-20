using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingLib
{
    public class TerminalAccess
    {
        public int TerminalID { get; set; }
        public int UserID { get; set; }

        static TerminalAccess GetRecord(IDataReader dr)
        {
            return new TerminalAccess()
            {
                TerminalID = Cmn.ToInt(dr["TerminalID"]),
                UserID = Cmn.ToInt(dr["UserID"]),
            };
        }

        public static List<TerminalAccess> GetAll()
        {
            List<TerminalAccess> list = new List<TerminalAccess>();
            DatabaseCE db = new DatabaseCE();
            try
            {
                string Error = "";

                IDataReader dr = db.GetDataReader("select * from TerminalAccess", ref Error);

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
            DatabaseCE db = new DatabaseCE();
            try
            {
                Dictionary<string, object> data = new Dictionary<string, object>();
                data.Add("TerminalID", TerminalID);
                data.Add("UserID", UserID);
                TableID.Save("TerminalAccess", data, new[] { "TerminalID", "UserID" }, db);
            }
            catch
            {
            }
            finally
            {
                db.Close();
            }
        }

        public static TerminalAccess GetByUseridAndTerminalid(int userID, int terminalID)
        {
            DatabaseCE db = new DatabaseCE();
            try
            {
                string Error = "";
                IDataReader dr = db.GetDataReader("select * from TerminalAccess where TerminalID=" + terminalID + " and UserID=" + userID, ref Error);
                while (dr.Read())
                {
                    return TerminalAccess.GetRecord(dr);
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

        public static List<TerminalAccess> GetByUserID(int UserID)
        {
            List<TerminalAccess> list = new List<TerminalAccess>();
            DatabaseCE db = new DatabaseCE();
            try
            {
                string Error = "";
                IDataReader dr = db.GetDataReader("select * from TerminalAccess where UserID=" + UserID, ref Error);
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
