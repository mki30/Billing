using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingLib
{
    public class ReportDailySale
    {
        public int ID { get; set; }
        public DateTime Date { get; set; }
        public int StoreID { get; set; }
        public int TerminalID { get; set; }
        public double Amount { get; set; }
        public double AmountCard { get; set; }
        public double Margin { get; set; }
        public double Vat { get; set; }
        public DateTime LUDate = Cmn.MinDate;

        static ReportDailySale GetRecord(IDataReader dr)
        {
            return new ReportDailySale()
            {
                ID = Cmn.ToInt(dr["ID"]),
                Date = Cmn.ToDate(dr["Date"]),
                LUDate = Cmn.ToDate(dr["LUDate"]),
                StoreID = Cmn.ToInt(dr["StoreID"]),
                TerminalID = Cmn.ToInt(dr["TerminalID"]),
                Amount = Cmn.ToDbl(dr["Amount"]),
                AmountCard = Cmn.ToDbl(dr["AmountCard"]),
                Margin = Cmn.ToDbl(dr["Margin"]),
                Vat = Cmn.ToDbl(dr["Vat"]),
            };
        }

        public static List<ReportDailySale> GetAll(int ID = 0)
        {
            List<ReportDailySale> list = new List<ReportDailySale>();
            DatabaseCE db = new DatabaseCE();
            try
            {
                string Error = "";

                IDataReader dr = db.GetDataReader("select * from ReportDailySale " + (ID == 0 ? "" : " where ID=" + ID), ref Error);

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

        public static List<ReportDailySale> GetAll(int StoreID, int TemerminalID, DateTime DateFrom, DateTime DateTo)
        {
            List<ReportDailySale> list = new List<ReportDailySale>();
            DatabaseCE db = new DatabaseCE();
            try
            {
                string Error = "";

                IDataReader dr = db.GetDataReader("select * from ReportDailySale where 1=1" + (StoreID == 0 ? "" : " and StoreID=" + StoreID)
                    + (TemerminalID == 0 ? "" : " and TemerminalID=" + TemerminalID)
                    + " and [Date]>='" + DateFrom.ToString("dd-MMM-yyyy") + "'"
                    + " and [Date]<'" + DateTo.AddDays(1).ToString("dd-MMM-yyyy") + "'"
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

        public ReportDailySale Save()
        {
            string vError = string.Empty;
            DatabaseCE db = new DatabaseCE();
            try
            {
                Dictionary<string, object> data = new Dictionary<string, object>();
                data.Add("ID", ID);
                data.Add("Date", Date);
                data.Add("StoreID", StoreID);
                data.Add("TerminalID", TerminalID);
                data.Add("Amount", Amount);
                data.Add("AmountCard", AmountCard);
                data.Add("Margin", Margin);
                data.Add("Vat", Vat);
                TableID.Save("ReportDailySale", data, db);
                ID = Cmn.ToInt(data["ID"]);
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
