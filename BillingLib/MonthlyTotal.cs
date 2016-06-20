using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingLib
{
    public class MonthlyTotal
    {
        public int ID { get; set; }
        public AmountType AmountType { get; set; }

        public double Amount { get; set; }
        public double AmountManual { get; set; }
        public double StoreID { get; set; }
        public DateTime TotalDate { get; set; }
        public DateTime UpdateDateManual { get; set; }
        public double CostVat { get; set; }
        public double PriceVat { get; set; }

        public string TotalDateString
        {
            get
            {
                return TotalDate.ToString("dd-MMM-yyyy");
            }
        }

        static MonthlyTotal GetRecord(IDataReader dr)
        {
            return new MonthlyTotal()
            {
                ID = Cmn.ToInt(dr["ID"]),
                AmountType = (AmountType)Cmn.ToInt(dr["AmountType"]),
                Amount = Cmn.ToDbl(dr["Amount"]),
                AmountManual = Cmn.ToDbl(dr["AmountManual"]),
                StoreID = Cmn.ToDbl(dr["StoreID"]),
                TotalDate = Cmn.ToDate(dr["TotalDate"]),
                UpdateDateManual = Cmn.ToDate(dr["UpdateDateManual"]),
                CostVat = Cmn.ToDbl(dr["CostVat"]),
                PriceVat = Cmn.ToDbl(dr["PriceVat"]),
            };
        }

        public static MonthlyTotal GetByID(int id)
        {
            DatabaseCE db = new DatabaseCE();
            try
            {
                string Error = "";

                IDataReader dr = db.GetDataReader("select * from MonthlyTotal where ID=" + id, ref Error);

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
                MonthlyTotal mt = MonthlyTotal.GetByID(ID);
                Dictionary<string, object> data = new Dictionary<string, object>();
                data.Add("ID", ID);
                data.Add("AmountType", (int)AmountType);
                data.Add("Amount", Amount);
                data.Add("AmountManual", AmountManual);
                data.Add("StoreID", StoreID);
                if (ID == 0)
                    data.Add("TotalDate", TotalDate);
                if (mt != null)
                {
                    if (mt.Amount != Amount)
                        data.Add("TotalDate", TotalDate);
                    if (mt.AmountManual != AmountManual)
                        data.Add("UpdateDateManual", UpdateDateManual);
                }
                data.Add("CostVat", CostVat);
                data.Add("PriceVat", PriceVat);
                TableID.Save("MonthlyTotal", data);
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

        public static MonthlyTotal GetByDateTypeAndStore(DateTime TotalDate, AmountType amountType, int StoreID)
        {
            DatabaseCE db = new DatabaseCE();
            try
            {
                string Error = "";
                IDataReader dr = db.GetDataReader("select * from MonthlyTotal where AmountType=" + (int)amountType + " and StoreID=" + StoreID
                    + " and TotalDate='" + TotalDate.ToString("dd-MMM-yyyy") + "' and TotalDate<'" + TotalDate.AddDays(1).Date.ToString("dd-MMM-yyyy") + "'"
                    , ref Error);
                while (dr.Read())
                {
                    return GetRecord(dr);
                }
            }
            finally
            {
                db.Close();
            }
            return new MonthlyTotal() { AmountType = amountType, StoreID = StoreID, TotalDate = TotalDate.Date };
        }


        public static List<MonthlyTotal> GetAll()
        {
            List<MonthlyTotal> list = new List<MonthlyTotal>();
            DatabaseCE db = new DatabaseCE();
            try
            {
                string Error = "";

                IDataReader dr = db.GetDataReader("select * from MonthlyTotal", ref Error);

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

        public static void UpdateMonthlyMargin(DateTime FromDate, int StoreID)
        {
            FromDate = Cmn.ConverToFirstDate(FromDate);
            List<ReportDailySale> listDailySale = ReportDailySale.GetAll(StoreID, 0, FromDate, FromDate.AddMonths(1).AddDays(-1));

            List<BillingLib.BillingItem> BIList = BillingLib.BillingItem.GetAll(FromDate, FromDate.AddMonths(1).AddDays(-1), "", StoreID).ToList();
            double marg = 0;
            foreach (BillingLib.BillingItem bi in BIList)
            {
                marg += bi.Margin;
            }
            MonthlyTotal mPur = MonthlyTotal.GetByDateTypeAndStore(FromDate, AmountType.Margin, StoreID);
            if (mPur == null)
                mPur = new MonthlyTotal() { AmountType = AmountType.Margin, Amount = marg, TotalDate = FromDate, StoreID = StoreID };
            else
                mPur.Amount = marg;
            if (marg != 0)
                mPur.Save();
        }

        public static void UpdateMonthlyValues(DateTime date, int StoreID)
        {
            if (StoreID == 0)
                return;

            DateTime dt = Cmn.ToDate(new DateTime(date.Year, 4, 1));
            for (var i = 0; i < 12; i++)
            {
                //Sale
                BillingLib.Billing.UpdateMonthlyTotal(dt, StoreID);
                //Purchase
                BillingLib.Purchase.UpdateMonthlyTotal(dt, StoreID);
                //Daily Expense
                BillingLib.ExpenseLog.UpdateMonthlyTotal(dt, StoreID);
                //Payment
                BillingLib.PurchasePayment.UpdateMonthlyTotal(dt, StoreID);
                //Margin
                BillingLib.MonthlyTotal.UpdateMonthlyMargin(dt, StoreID);
                dt = dt.AddMonths(1);
            }
        }
    }

}


