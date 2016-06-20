using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingLib
{
    public class EmployeeAttendance
    {
        public int ID ;
        public int EmployeeID ;
        public string Name ="";
        public DateTime Date =DateTime.Today;
        public DateTime DateIn = Cmn.MinDate;
        public DateTime DateOut = Cmn.MinDate;
        public int LeaveType ;
        public int StoreID ;

        public string DateString
        {
            get
            {
                return Date.ToString("dd-MMM-yyyy");
            }
        }
        public string DateInString
        {
            get
            {
                return DateIn.ToString("dd-MMM-yyyy HH:mm");
            }
        }
        public string DateOutString
        {
            get
            {
                return DateOut.ToString("dd-MMM-yyyy HH:mm");
            }
        }

        static EmployeeAttendance GetRecord(IDataReader dr)
        {
            return new EmployeeAttendance()
            {
                ID = Cmn.ToInt(dr["ID"]),
                EmployeeID = Cmn.ToInt(dr["EmployeeID"]),
                Name = dr["Name"].ToString(),
                Date = Cmn.ToDate(dr["Date"]),
                DateIn = Cmn.ToDate(dr["DateIn"]),
                DateOut = Cmn.ToDate(dr["DateOut"]),
                LeaveType = Cmn.ToInt(dr["LeaveType"]),
                StoreID = Cmn.ToInt(dr["StoreID"]),
            };
        }

        public static List<EmployeeAttendance> GetAll(DateTime fromDate, DateTime toDate, int empID = 0, int StoreID = 0)
        {
            List<EmployeeAttendance> list = new List<EmployeeAttendance>();
            DatabaseCE db = new DatabaseCE();
            try
            {
                string Error = "";
                IDataReader dr = db.GetDataReader("select * from EmployeeAttendance where"
                    + (empID != 0 ? " EmployeeID=" + empID + " and" : "")
                    + " Date>='" + fromDate.ToString("dd-MMM-yyyy")
                    + "' and Date<='" + toDate.ToString("dd-MMM-yyyy") + "'"
                    + (StoreID != 0 ? " and StoreID=" + StoreID + "" : "")
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

        public static List<EmployeeAttendance> GetAll(int ID = 0)
        {
            List<EmployeeAttendance> list = new List<EmployeeAttendance>();
            DatabaseCE db = new DatabaseCE();
            try
            {
                string Error = "";

                IDataReader dr = db.GetDataReader("select * from EmployeeAttendance " + (ID == 0 ? "" : " where ID=" + ID), ref Error);

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


        public static EmployeeAttendance GetByID(int id)
        {
            //return GetAll(id).First();
            DatabaseCE db = new DatabaseCE();
            try
            {
                string Error = "";
                IDataReader dr = db.GetDataReader("select * from EmployeeAttendance where ID='" + id + "'", ref Error);
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

        public void Save()
        {
            string vError = string.Empty;
            DatabaseCE db = new DatabaseCE();
            try
            {
                Dictionary<string, object> data = new Dictionary<string, object>();
                data.Add("ID", ID);
                data.Add("EmployeeID", EmployeeID);
                data.Add("Name", Name);
                data.Add("Date", Date);
                data.Add("DateIn", DateIn);
                data.Add("DateOut", DateOut);
                data.Add("LeaveType", LeaveType);
                data.Add("StoreID", StoreID);
                string Error = TableID.Save("EmployeeAttendance", data, db);
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

        public EmployeeAttendance Delete()
        {
            DatabaseCE db = new DatabaseCE();
            try
            {
                db.RunQuery("Delete from EmployeeAttendance where id=" + ID);
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
    }
}
