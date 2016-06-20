using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingLib
{
    public class ExpenseLog
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public double Amount { get; set; }
        public DateTime Date { get; set; }
        public int StoreID { get; set; }
        public int TerminalID { get; set; }
        public ExpenseCategory expenseType { get; set; }

        public string DateString
        {
            get
            {
                return Date.ToString("dd-MMM-yyyy");
            }
        }

        static ExpenseLog GetRecord(IDataReader dr)
        {
            return new ExpenseLog()
            {
                ID = Cmn.ToInt(dr["ID"]),
                Name = dr["Name"].ToString(),
                Amount = Cmn.ToDbl(dr["Amount"]),
                Date = Cmn.ToDate(dr["Date"]),
                StoreID = Cmn.ToInt(dr["StoreID"]),
                TerminalID = Cmn.ToInt(dr["TerminalID"]),
                expenseType = (ExpenseCategory)Cmn.ToInt(dr["ExpenseType"]),

            };
        }

        public static List<ExpenseLog> GetAll(int ID = 0)
        {
            List<ExpenseLog> list = new List<ExpenseLog>();
            DatabaseCE db = new DatabaseCE();
            try
            {
                string Error = "";

                IDataReader dr = db.GetDataReader("select * from ExpenseLog " + (ID == 0 ? "" : " where ID=" + ID), ref Error);

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

        public static List<ExpenseLog> GetByDate(DateTime fromDate, DateTime toDate, int StoreID, int ExpenseType = 0)
        {
            List<ExpenseLog> list = new List<ExpenseLog>();
            DatabaseCE db = new DatabaseCE();
            try
            {
                string Error = "";

                IDataReader dr = db.GetDataReader("select * from ExpenseLog where date>='" + fromDate.ToString("dd-MMM-yyyy")
                    + "' and date<='" + toDate.ToString("dd-MMM-yyyy") + "'"
                    + (StoreID != 0 ? " and StoreID=" + StoreID + "" : "")
                    + (ExpenseType != 0 ? " and ExpenseType=" + ExpenseType + "" : "")
                    + " order by date"
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

        public void Save()
        {
            string vError = string.Empty;
            DatabaseCE db = new DatabaseCE();
            try
            {
                Dictionary<string, object> data = new Dictionary<string, object>();
                data.Add("ID", ID);
                data.Add("Name", Name);
                data.Add("Amount", Amount);
                data.Add("Date", Date);
                data.Add("StoreID", StoreID);
                data.Add("TerminalID", TerminalID);
                data.Add("ExpenseType", (int)expenseType);
                TableID.Save("ExpenseLog", data, db);
                ID = Cmn.ToInt(data["ID"]);
                UpdateMonthlyTotal(Date, StoreID);
            }
            catch
            {
            }
            finally
            {
                db.Close();
            }
        }

        public static ExpenseLog GetByID(int id)
        {
            string vError = string.Empty;
            DatabaseCE db = new DatabaseCE();
            try
            {
                string Error = "";
                IDataReader dr = db.GetDataReader("select * from ExpenseLog where ID=" + id, ref Error);
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

        public static string GetExpenseDataTable(Company comp, DateTime dateFrom, DateTime dateTo, int heighlightId = 0, bool isReport = false, int StoreID = 0, int ExpenseType = 0)
        {
            List<BillingLib.ExpenseLog> list = list = BillingLib.ExpenseLog.GetByDate(dateFrom, dateTo, StoreID, ExpenseType);

            double total = 0;
            string table = "<table class='table table-condensed table-striped table-bordered csvTable'><tr><th>SNo<th>Terminal<th>Store<th>Name<th class='alnright'>Amount<th>Date<th>Catag.";
            StringBuilder tr = new StringBuilder();
            int ctr = 0;
            foreach (BillingLib.ExpenseLog e in list)
            {
                BillingLib.Terminal T;
                Global.TerminalList.TryGetValue(e.TerminalID, out T);

                BillingLib.Store S;
                Global.StoreList.TryGetValue(e.StoreID, out S);
                string editLink = "<a href='#' onclick='EditExpense(" + e.ID + ");'>" + (string.IsNullOrWhiteSpace(e.Name) ? "<span style='color:red'>No Name</span>" : e.Name) + "</a>";
                tr.Append("<tr  " + (e.ID == heighlightId ? "style='background-color:#FDFDA6;'" : "") + "><td title=" + e.ID + ">" + (++ctr) + "<td>" + (T != null ? T.Name : "-") + "<td>" + (S != null ? S.Name : "-") + "<td>" + editLink + "<td  class='alnright'>" + Cmn.toCurrency(e.Amount) + "<td style='width:80px;'>" + e.Date.ToString("dd-MMM-yy") + "<td>" + (e.expenseType == 0 ? "" : ((ExpenseCategory)e.expenseType).ToString()));
                total += e.Amount;
            }
            table += "<tr style='font-weight:bold;'><td><td><td><td>Total<td class='alnright'>" + Cmn.toCurrency(total) + "<td><td></tr>" + tr.ToString() + "</table>";
            return table;
        }

        public static double GetExpenseSum(DateTime FromDate, DateTime ToDate, int StoreID)
        {
            string Error = "";
            DatabaseCE db = new DatabaseCE();
            try
            {
                Object amt = db.ExecuteScalar("select SUM(Amount) from ExpenseLog where Date>='" + FromDate.ToString("dd-MMM-yyyy")
                    + "' and Date < '" + ToDate.AddDays(1).ToString("dd-MMM-yyyy") + "' AND StoreID =" + StoreID + "", ref Error);
                return Cmn.ToDbl(amt);
            }
            catch
            {
            }
            finally
            {
                db.Close();
            }
            return 0;
        }

        public static void UpdateMonthlyTotal(DateTime FromDate, int StoreID)
        {
            FromDate = Cmn.ConverToFirstDate(FromDate);
            double expenseSum = Math.Round(BillingLib.ExpenseLog.GetExpenseSum(FromDate, FromDate.AddMonths(1).AddDays(-1), StoreID));
            MonthlyTotal monthlyTotalExp = MonthlyTotal.GetByDateTypeAndStore(FromDate, AmountType.DailyExpense, StoreID);
            if (monthlyTotalExp == null)
                monthlyTotalExp = new MonthlyTotal() { AmountType = AmountType.DailyExpense, Amount = expenseSum, TotalDate = FromDate, StoreID = StoreID };
            monthlyTotalExp.Amount = Math.Round(expenseSum, 0);
            monthlyTotalExp.Save();
        }
    }
}
