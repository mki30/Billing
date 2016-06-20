using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingLib
{
    public class Salary
    {
        public int ID;
        public int EmployeeID;
        public string EmployeeName = "";
        public DateTime SalaryDate = Cmn.MinDate;
        public double Basic;
        public double HRA;
        public double TA;
        public double Medical;
        public double Advance;
        public double TDS;
        public double Loan;
        public DateTime PaidOn = Cmn.MinDate;
        public double PaidAmount = 0;
        public int StoreID;

        public string SalaryDateString
        {
            get
            {
                return SalaryDate.ToString("dd-MMM-yyyy");
            }
        }

        public string PaidOnString
        {
            get
            {
                return PaidOn.ToString("dd-MMM-yyyy");
            }
        }

        public double Total
        {
            get
            {
                return Basic + HRA + TA + Medical + Advance - TDS - Loan;
            }
        }

        static Salary GetRecord(IDataReader dr)
        {
            return new Salary()
            {
                ID = Cmn.ToInt(dr["ID"]),
                EmployeeID = Cmn.ToInt(dr["EmployeeID"]),
                EmployeeName = dr["EmployeeName"].ToString(),
                SalaryDate = Cmn.ToDate(dr["SalaryDate"]),
                Basic = Cmn.ToDbl(dr["Basic"]),
                HRA = Cmn.ToDbl(dr["HRA"]),
                TA = Cmn.ToDbl(dr["TA"]),
                Medical = Cmn.ToDbl(dr["Medical"]),
                Advance = Cmn.ToDbl(dr["Advance"]),
                TDS = Cmn.ToDbl(dr["TDS"]),
                Loan = Cmn.ToDbl(dr["Loan"]),
                PaidOn = Cmn.ToDate(dr["PaidOn"]),
                PaidAmount = Cmn.ToDbl(dr["PaidAmount"]),
                StoreID=Cmn.ToInt(dr["StoreID"])
            };
        }


        public static List<Salary> GetAll(DateTime dtFrom,DateTime dtTo)
        {
            List<Salary> list = new List<Salary>();
            DatabaseCE db = new DatabaseCE();
            try
            {
                string Error = "";

                IDataReader dr = db.GetDataReader("select * from Salary where SalaryDate>='" + dtFrom.ToString("dd-MMM-yyyy") + "' and SalaryDate<='" + dtTo.ToString("dd-MMM-yyyy") + "'"  , ref Error);

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

        public static Salary GetByID(int id)
        {
            DatabaseCE db = new DatabaseCE();
            try
            {
                string Error = "";

                IDataReader dr = db.GetDataReader("select * from Salary where ID=" + id, ref Error);

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
                data.Add("EmployeeName", EmployeeName);
                data.Add("SalaryDate", SalaryDate);
                data.Add("Basic", Basic);
                data.Add("HRA", HRA);
                data.Add("TA", TA);
                data.Add("Medical", Medical);
                data.Add("Advance", Advance);
                data.Add("TDS", TDS);
                data.Add("Loan", Loan);
                data.Add("PaidOn", PaidOn);
                data.Add("PaidAmount", PaidAmount);
                data.Add("StoreID", StoreID);
                TableID.Save("Salary", data);
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

        public static Salary GetByEmpidAndSalDate(int EmpID, DateTime SalaryDate)
        {
            DatabaseCE db = new DatabaseCE();
            try
            {
                string Error = "";

                IDataReader dr = db.GetDataReader("select * from Salary where EmployeeID=" + EmpID + " and SalaryDate='" + SalaryDate.ToString("dd-MMM-yyyy") + "' ", ref Error);

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


