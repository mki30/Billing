using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingLib
{
    public class PurchasePayment
    {
        public int ID { get; set; }
        public int VendorID { get; set; }
        public int PurchaseID { get; set; }
        public double Cash { get; set; }
        public double Cheque { get; set; }
        public string ChequeNo { get; set; }
        public string BillNo { get; set; }
        public int BankID { get; set; }
        public DateTime BillDate = Cmn.MinDate;
        public DateTime PaymentDate = Cmn.MinDate;
        public int StoreID { get; set; }
        public string LedgerNo { get; set; }
        public int IsDelete { get; set; }

        public string BillDateString
        {
            get
            {
                return BillDate.ToString("dd-MMM-yyyy");
            }
        }
        public string PaymentDateString
        {
            get
            {
                return PaymentDate.ToString("dd-MMM-yyyy");
            }
        }

        static PurchasePayment GetRecord(IDataReader dr)
        {
            return new PurchasePayment()
            {
                ID = Cmn.ToInt(dr["ID"]),
                VendorID = Cmn.ToInt(dr["VendorID"]),
                PurchaseID = Cmn.ToInt(dr["PurchaseID"]),
                Cash = Cmn.ToDbl(dr["Cash"]),
                Cheque = Cmn.ToDbl(dr["Cheque"]),
                ChequeNo = dr["ChequeNo"].ToString(),
                BillNo = dr["BillNo"].ToString(),
                PaymentDate = Cmn.ToDate(dr["PaymentDate"]),
                BillDate = Cmn.ToDate(dr["BillDate"]),
                StoreID = Cmn.ToInt(dr["StoreID"]),
                BankID = Cmn.ToInt(dr["BankID"]),
                LedgerNo = dr["LedgerNo"].ToString(),
                IsDelete = Cmn.ToInt(dr["IsDelete"])
            };
        }

        public static List<PurchasePayment> GetbyVendorID(int VendorID, DateTime DateFrom, DateTime DateTo, int StoreID)
        {
            List<PurchasePayment> list = new List<PurchasePayment>();
            DatabaseCE db = new DatabaseCE();
            try
            {
                string Error = "";
                IDataReader dr = db.GetDataReader("select * from PurchasePayment where 1=1 "
                    + (VendorID != 0 ? " and VendorID=" + VendorID : "")
                    + (StoreID != 0 ? " and StoreID=" + StoreID + "" : "")
                    + " and PaymentDate>='" + DateFrom.ToString("dd-MMM-yyyy") + "'"
                    + " and PaymentDate<='" + DateTo.ToString("dd-MMM-yyyy") + "'"
                    + " order by PaymentDate desc", ref Error);

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

        public static List<PurchasePayment> GetAll(int ID = 0)
        {
            List<PurchasePayment> list = new List<PurchasePayment>();
            DatabaseCE db = new DatabaseCE();
            try
            {
                string Error = "";

                IDataReader dr = db.GetDataReader("select * from PurchasePayment " + (ID == 0 ? "" : " where ID=" + ID), ref Error);

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
                data.Add("ID", ID);
                data.Add("PurchaseID", PurchaseID);
                data.Add("VendorID", VendorID);
                data.Add("Cash", Cash);
                data.Add("Cheque", Cheque);
                data.Add("ChequeNo", ChequeNo);
                data.Add("PaymentDate", PaymentDate);
                data.Add("BillDate", BillDate);
                data.Add("BillNo", BillNo);
                data.Add("BankID", BankID);
                data.Add("StoreID", StoreID);
                data.Add("LedgerNo", LedgerNo);
                data.Add("IsDelete", IsDelete);
                TableID.Save("PurchasePayment", data, db);
                ID = Cmn.ToInt(data["ID"]);

                //string vError = "";
                //double sum = 0;
                //sum = Cmn.ToDbl(db.ExecuteScalar("SELECT Cash + Cheque AS Sum FROM purchasePayment WHERE (PurchaseID = " + data["PurchaseID"] + ") and Isdelete <> 1", ref vError));
                //db.RunQuery("Update Purchase SET PaymentAmount=" + sum + " where id=" + data["PurchaseID"] + "");
            }
            catch
            {
            }
            finally
            {
                db.Close();
            }
        }

        public static PurchasePayment GetByID(int ID)
        {
            DatabaseCE db = new DatabaseCE();
            try
            {
                string Error = "";
                IDataReader dr = db.GetDataReader("select * from PurchasePayment where ID='" + ID + "'", ref Error);
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


        public static List<PurchasePayment> GetByPurchaseID(int purchaseId)
        {
            List<PurchasePayment> list = new List<PurchasePayment>();
            DatabaseCE db = new DatabaseCE();
            try
            {
                string Error = "";
                IDataReader dr = db.GetDataReader("select * from PurchasePayment  where PurchaseID=" + purchaseId, ref Error);
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

        public static string GetHtml(int purchaseId)
        {
            List<PurchasePayment> list = PurchasePayment.GetByPurchaseID(purchaseId);
            String str = "<table class='table table-condensed table-bordered'><tr><th>Date<th>Cash<th>Cheque<th>Cheque No";
            foreach (PurchasePayment pp in list)
            {
                str += "<tr><td><a href='/Payment.aspx?purchaseId=" + pp.PurchaseID + "&id=" + pp.ID + "'>" + Cmn.ToDate(pp.PaymentDate).ToString("dd-MM-yy") + "</a><td>" + pp.Cash + "<td>" + pp.Cheque + "<td>" + pp.ChequeNo;
            }
            return str += "</table>";
        }

        public static string GetHtmlByVendorID(int VendorID, DateTime DateFrom, DateTime DateTo, int StoreID, Company company)
        {
            List<PurchasePayment> list = PurchasePayment.GetbyVendorID(VendorID, DateFrom, DateTo, StoreID);
            List<Bank> listBank = Bank.GetAll();
            string tableHead = "<table id='paymentTable' class='datatable table table-condensed table-bordered'><tr><th>#<th>Vendor<th>Store<th>Payment Date<th class='alnright'>Cash<th class='alnright'>Cheque<th>Cheque No<th>Bank<th>Bill No / Bill Date";
            StringBuilder str = new StringBuilder();
            int ctr = 0;
            float grandTotal = 0;
            float cashTotal = 0;
            float chequeTotal = 0;


            //Get all purchase payment links
            string paymentIds = "";
            foreach (PurchasePayment pp in list)
                paymentIds += pp.ID.ToString() + ",";
            List<PurchasePaymentLink> pplList = PurchasePaymentLink.GetByPaymentIDs(paymentIds.TrimEnd(','));

            //Get all Purchases
            string purchaseIds = "";
            foreach (PurchasePaymentLink ppl in pplList)
                purchaseIds += ppl.PurchaseID + ",";
            List<Purchase> purchaselist = Purchase.GetInIds(purchaseIds.TrimEnd(','));

            foreach (PurchasePayment pp in list)
            {
                List<PurchasePaymentLink> pplListTemp = pplList.Where(m => m.PaymentID == pp.ID).ToList();
                List<Purchase> purchaselistTemp = new List<Purchase>();
                foreach (PurchasePaymentLink ppl in pplListTemp)
                    purchaselistTemp.Add(purchaselist.FirstOrDefault(m => m.ID == ppl.PurchaseID));

                string rcNoAndBillDate = "";
                foreach (Purchase p in purchaselistTemp.OrderByDescending(m => m.PurchaseDate))
                    rcNoAndBillDate += "<a href='/Inventory.aspx?purchaseid=" + p.ID + "' target='_blank'>" + p.RCNo + "/" + p.PurchaseDate.ToString("%d-M-yy") + ", ";


                Vendor vendor = company.vendorList.FirstOrDefault(m => m.ID == pp.VendorID);
                Bank bank = listBank.FirstOrDefault(m => m.ID == pp.BankID);
                Store store;
                Global.StoreList.TryGetValue(pp.StoreID, out store);
                ctr++;
                float total = (float)(pp.Cash + pp.Cheque);

                str.Append("<tr " + (pp.IsDelete == 1 ? "style='text-decoration: line-through;'" : "") + "><td title=" + pp.ID + ">" + ctr + "<td>" + (vendor != null ? vendor.Name : "") + "<td>" + (store != null ? store.Name : "") + "<td><a href='#' onclick='EditPayment(" + pp.ID + ")'>"
                    + Cmn.ToDate(pp.PaymentDate).ToString("%d-MMM-yy") + "</a><td class='alnright'>"
                    + (pp.Cash != 0 ? Cmn.toCurrency(pp.Cash) : "") + "<td class='alnright'>"
                    + (pp.Cheque != 0 ? Cmn.toCurrency(pp.Cheque) : "") + "<td class='alnright'>"
                    + pp.ChequeNo + "<td>"
                    + (bank != null ? bank.Name : "") + "<td>"
                    + rcNoAndBillDate
                   );
                if (pp.IsDelete != 1) //Escape deleted record from calculation.
                {
                    grandTotal += total;
                    cashTotal += (float)pp.Cash;
                    chequeTotal += (float)pp.Cheque;
                }
            }
            tableHead += "<tr style='font-weight:bold;' class='alnright'><td colspan=4>Total<td class='alnright'>" + cashTotal + "<td class='alnright'>" + chequeTotal + "<td><td><td>";
            str.Append("</table>");

            return tableHead + str;
        }

        public static void UpdateMonthlyTotal(DateTime FromDate, int StoreID)
        {
            FromDate = Cmn.ConverToFirstDate(FromDate);
            double paymentSum = Math.Round(BillingLib.PurchasePayment.GetPaymentSum(FromDate, FromDate.AddMonths(1).AddDays(-1), StoreID));
            MonthlyTotal monthlyTotalExp = MonthlyTotal.GetByDateTypeAndStore(FromDate, AmountType.Payment, StoreID);
            if (monthlyTotalExp == null)
                monthlyTotalExp = new MonthlyTotal() { AmountType = AmountType.Payment, Amount = paymentSum, TotalDate = FromDate, StoreID = StoreID };
            monthlyTotalExp.Amount = Math.Round(paymentSum, 0);
            monthlyTotalExp.Save();
        }

        private static double GetPaymentSum(System.DateTime FromDate, System.DateTime ToDate, int StoreID)
        {
            string Error = "";
            DatabaseCE db = new DatabaseCE();
            try
            {
                Object amt = db.ExecuteScalar("select SUM(Cash+Cheque) from PurchasePayment where PaymentDate>='" + FromDate.ToString("dd-MMM-yyyy")
                    + "' and PaymentDate < '" + ToDate.AddDays(1).ToString("dd-MMM-yyyy") + "' AND StoreID =" + StoreID + " and IsDelete <> 1", ref Error);
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

    }
}