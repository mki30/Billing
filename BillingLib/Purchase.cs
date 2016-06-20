using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingLib
{
    public class Purchase
    {
        public int ID { get; set; }
        public int StoreID { get; set; }
        public DateTime PurchaseDate { get; set; }
        public int VendorID { get; set; }
        public double Amount { get; set; }
        public string InvoiceNo { get; set; }
        public string RCNo { get; set; }
        public int ItemIncludesTax { get; set; }
        public double ItemCount { get; set; }
        public double PaymentAmount { get; set; }
        public double CST { get; set; }
        public int IsReturn { get; set; }
        public int IsDelete { get; set; }
        public int IsForm38 { get; set; }
        public int IsNonUP { get; set; }
        

        static Purchase GetRecord(IDataReader dr)
        {
            return new Purchase()
            {
                ID = Cmn.ToInt(dr["ID"]),
                StoreID = Cmn.ToInt(dr["StoreID"]),
                PurchaseDate = Cmn.ToDate(dr["PurchaseDate"]),
                VendorID = Cmn.ToInt(dr["VendorID"]),
                Amount = Cmn.ToDbl(dr["Amount"]),
                InvoiceNo = dr["InvoiceNo"].ToString(),
                RCNo = dr["RCNo"].ToString(),
                ItemIncludesTax = Cmn.ToInt(dr["ItemIncludesTax"]),
                ItemCount = Cmn.ToDbl(dr["ItemCount"]),
                PaymentAmount = Cmn.ToDbl(dr["PaymentAmount"]),
                CST = Cmn.ToDbl(dr["CST"]),
                IsReturn = Cmn.ToInt(dr["IsReturn"]),
                IsDelete = Cmn.ToInt(dr["IsDelete"]),
                IsForm38 = Cmn.ToInt(dr["IsForm38"]),
                IsNonUP = Cmn.ToInt(dr["IsNonUP"])
            };
        }

        public static List<Purchase> GetAll(int ID = 0)
        {
            List<Purchase> list = new List<Purchase>();
            DatabaseCE db = new DatabaseCE();
            try
            {
                string Error = "";

                IDataReader dr = db.GetDataReader("select * from Purchase " + (ID == 0 ? "" : " where ID=" + ID), ref Error);

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

        public static double GetDueAmountByDate(DateTime dateFrom, DateTime dateTo, int storeId = 0, int vendorID = 0)
        {
            string vError = string.Empty;
            DatabaseCE db = new DatabaseCE();
            try
            {
                string Error = "";
                double total = Cmn.ToDbl(db.GetFieldValue("SELECT SUM(Amount) FROM Purchase WHERE 1=1"
                           + (vendorID == 0 ? "" : " and VendorID=" + vendorID)
                           + (storeId == 0 ? "" : " and StoreID=" + storeId)
                           + " and PurchaseDate>='" + dateFrom.ToString("dd-MMM-yyyy") + "'"
                           + " and PurchaseDate<'" + dateTo.ToString("dd-MMM-yyyy") + "'"
                           , ref Error));
                return total;
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

        public static List<Purchase> GetByStore(DateTime dateFrom, DateTime dateTo, int storeId = 0, int vendorID = 0, int ret = 0, string escapeIDs = "")
        {
            List<Purchase> list = new List<Purchase>();
            DatabaseCE db = new DatabaseCE();
            try
            {
                string Error = "";
                IDataReader dr = db.GetDataReader("select * from Purchase where 1=1 "
                    + (storeId == 0 ? "" : " and StoreID=" + storeId)
                    + (vendorID == 0 ? "" : " and VendorID=" + vendorID)
                    + (ret == 0 ? "" : " and IsReturn=1")
                    + " and PurchaseDate>='" + dateFrom.ToString("dd-MMM-yyyy") + "'"
                    + " and PurchaseDate<='" + dateTo.ToString("dd-MMM-yyyy") + "'"
                    + (escapeIDs == "" ? "" : " and ID NOT IN(" + escapeIDs + ")")
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

        public static List<Purchase> GetByBillingID(int BillingID, DatabaseCE db)
        {
            List<Purchase> list = new List<Purchase>();
            try
            {
                string Error = "";
                IDataReader dr = db.GetDataReader("select * from Item where BillingID" + BillingID, ref Error);
                while (dr.Read())
                {
                    list.Add(GetRecord(dr));
                }
            }
            catch
            {

            }
            return list;
        }

        public static Purchase GetByID(int id)
        {
            string vError = string.Empty;
            DatabaseCE db = new DatabaseCE();
            try
            {
                string Error = "";
                IDataReader dr = db.GetDataReader("select * from Purchase where ID=" + id, ref Error);
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

        public static void UpdateTotal(int PurchaseID)
        {
            new Purchase() { ID = PurchaseID }.UpdateTotal();
        }


        public Purchase UpdateTotal()
        {
            string vError = string.Empty;
            DatabaseCE db = new DatabaseCE();
            try
            {
                string Error = "";
                ItemCount = 0;
                double total = Cmn.ToDbl(db.GetFieldValue("SELECT SUM(TotalAmount) FROM Inventory WHERE PurchaseID = " + ID, ref Error));
                ItemCount = db.GetCount("SELECT COUNT(*)  from Inventory WHERE purchaseID=" + ID, ref Error);
                db.RunQuery("Update Purchase set Amount=" + total + ",ItemCount=" + ItemCount + " where ID=" + ID);
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

        public void Save()
        {
            string vError = string.Empty;
            DatabaseCE db = new DatabaseCE();
            try
            {
                Dictionary<string, object> data = new Dictionary<string, object>();
                data.Add("ID", ID);
                data.Add("StoreID", StoreID);
                data.Add("PurchaseDate", PurchaseDate);
                data.Add("VendorID", VendorID);
                data.Add("Amount", Amount);
                data.Add("InvoiceNo", InvoiceNo);
                data.Add("RCNo", RCNo);
                data.Add("ItemIncludesTax", ItemIncludesTax);
                data.Add("ItemCount", ItemCount);
                data.Add("PaymentAmount", PaymentAmount);
                data.Add("CST", CST.ToString());
                data.Add("IsReturn", IsReturn);
                data.Add("IsDelete", IsDelete);
                data.Add("IsForm38", IsForm38);
                data.Add("IsNonUP", IsNonUP);
                TableID.Save("Purchase", data, db);
                ID = Cmn.ToInt(data["ID"]);
                if (Cmn.ToInt(RCNo) == 0)
                {
                    data["RCNo"] = ID.ToString();
                    TableID.Save("Purchase", data, db);
                }

                UpdateTotal();
            }
            catch
            {
            }
            finally
            {
                db.Close();
            }
        }

        public static List<Purchase> GetByDate(DateTime FromDate, DateTime ToDate, int VendorID = 0)
        {

            DatabaseCE db = new DatabaseCE();
            List<Purchase> list = new List<Purchase>();
            string Error = "";
            try
            {
                IDataReader dr = db.GetDataReader("select * from Purchase where PurchaseDate>='" + FromDate.ToString("dd-MMM-yyyy")
                    + "' and PurchaseDate <= '" + ToDate.ToString("dd-MMM-yyyy") + "' " + (VendorID != 0 ? " and VendorID=" + VendorID + "" : "") + " order by PurchaseDate", ref Error);
                while (dr.Read())
                {
                    list.Add(GetRecord(dr));
                }
            }
            catch (Exception ex)
            {
                Error = ex.Message;
            }
            finally
            {
                db.Close();
            }
            return list;
        }

        public static List<Purchase> GetByVendorID(int vendorID)
        {
            DatabaseCE db = new DatabaseCE();
            List<Purchase> list = new List<Purchase>();
            try
            {
                string Error = "";

                IDataReader dr = db.GetDataReader("select * from Purchase where VendorID=" + vendorID, ref Error);

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

        public string DeletePurchase()
        {
            string Error = "";
            DatabaseCE db = new DatabaseCE();
            try
            {
                int Count = db.RunQuery("Delete from purchase where id=" + ID);
                if (Count != 0)
                    Count = db.RunQuery("Delete from Inventory where purchaseid=" + ID);
            }
            catch
            {
            }
            finally
            {
                db.Close();
            }
            return Error;
        }

        class Record
        {
            public int purchaseID;
            public DateTime SortDate;
            public string row;
            public double Amount;
            public double CumulativeTotal;
            public int Sequence;
        }

        public static string GetHTMLList(Company Comp, DateTime DateFrom, DateTime DateTo, int VendorID, int StoreID, bool ShowPaymentRows = true, int PaymentID = 0)
        {
            if (DateFrom == Cmn.MinDate)
                DateFrom = DateTime.Now.AddYears(-1);//Cmn.FinStartDate;

            if (DateTo == Cmn.MinDate)
                DateTo = DateTime.Now;//Cmn.FinEndDate;

            string escapeIds = "";  //remove purchase already linked with bill
            if (!ShowPaymentRows)
            {
                List<PurchasePaymentLink> list = PurchasePaymentLink.GetByPayment(PaymentID);
                foreach (PurchasePaymentLink ppl in list)
                    escapeIds += ppl.PurchaseID + ",";
            }

            List<BillingLib.Purchase> purchaseList = BillingLib.Purchase.GetByStore(DateFrom, DateTo, StoreID, VendorID, 0, escapeIds.TrimEnd(',')).OrderBy(m => m.StoreID).ThenByDescending(m => m.PurchaseDate).ToList();
            List<Record> recordList = new List<Record>();
            double purchaseTotal = 0;
            foreach (BillingLib.Purchase P in purchaseList)
            {
                BillingLib.Vendor vendor = Comp.vendorList.FirstOrDefault(m => m.ID == P.VendorID);
                BillingLib.Store store = null;
                Global.StoreList.TryGetValue(P.StoreID, out store);

                Record rec = new Record();
                rec.SortDate = P.PurchaseDate;
                rec.Amount = P.Amount;
                rec.purchaseID = P.ID;
                rec.row = "<td>" + P.PurchaseDate.ToString("%d-MMM-yy")
                    + "<td><a href='#' onclick='return PurchaseSelect(" + P.ID + ")'>" + (vendor != null ? vendor.Name : "-") + "<td>" + (store != null ? store.Name : "-")
                    //+ "<td " + (AmounPaid == Math.Round(P.Amount, 0) ? "" : "style='color:red;'") + ">" + (AmounPaid == Math.Round(P.Amount, 0) ? "(Paid) " : "(Not Paid) ") + P.Amount.ToString("0")
                    + "<td>" + Cmn.toCurrency(P.Amount)
                    + "<td><span class='label label-info' title='Invoice No'>" + P.InvoiceNo + "</span><span title='RC No' class='label label-warning'>" + P.RCNo + "</span>"
                    + "<td>" + (P.ItemCount != 0 ? P.ItemCount.ToString() : "")
                    + "<td>" + (P.CST > 0 ? P.CST.ToString() : "");
                recordList.Add(rec);
                purchaseTotal += P.Amount;
            }

            List<BillingLib.PurchasePayment> listPayment = BillingLib.PurchasePayment.GetbyVendorID(VendorID, DateFrom, DateTo, StoreID).Where(m => m.IsDelete != 1).ToList();
            //Get all purchase payment links in this critetia
            string paymentIds = "";
            foreach (PurchasePayment pp in listPayment)
                paymentIds += pp.ID.ToString() + ",";
            List<PurchasePaymentLink> pplList = PurchasePaymentLink.GetByPaymentIDs(paymentIds.TrimEnd(','));

            //Get all Purchases for all above purchase payment links
            string purchaseIds = "";
            foreach (PurchasePaymentLink ppl in pplList)
                purchaseIds += ppl.PurchaseID + ",";
            List<Purchase> purchaselist2 = Purchase.GetInIds(purchaseIds.TrimEnd(','));


            if (ShowPaymentRows)  //Add payment rows to show in payment
            {
                foreach (BillingLib.PurchasePayment P in listPayment)
                {
                    //Retrive invoice no and rc no.
                    List<BillingLib.PurchasePaymentLink> pplListTemp = pplList.Where(m => m.PaymentID == P.ID).ToList();
                    List<BillingLib.Purchase> purchaselistTemp = new List<BillingLib.Purchase>();
                    foreach (PurchasePaymentLink ppl in pplListTemp)
                    {
                        Purchase p = purchaselist2.FirstOrDefault(m => m.ID == ppl.PurchaseID);
                        if (p != null) purchaselistTemp.Add(p);
                    }
                    string invoiveNoAndBillDate = ""; 
                    foreach (BillingLib.Purchase p in purchaselistTemp.OrderByDescending(m => m.PurchaseDate))
                    {
                        invoiveNoAndBillDate += "<a href='/Inventory.aspx?purchaseid=" + p.ID + "' target='_blank'><span class='label label-info' title='Invoice No'>" + p.InvoiceNo + "</span><span class='label label-warning' title='Rc No'>" + p.RCNo + "</span>/<span class='label label-success' title='Payment Date'>" + p.PurchaseDate.ToString("%d-MMM-yy") + "</span><br/>";
                        
                    }
                    //

                    Record rec = new Record();
                    rec.SortDate = P.PaymentDate;
                    rec.Amount = P.Cash + P.Cheque;

                    BillingLib.Bank bank = Global.BankList.FirstOrDefault(m => m.ID == P.BankID);
                    rec.row = "<td>" + P.PaymentDate.ToString("dd-MMM-yy") + "<td style='text-align:center;'>Payment" + (P.Cheque != 0 ? "-by cheque (" + P.ChequeNo + ")/" + (bank != null ? bank.Name : "") + "" : "") + "<td><td>" + (P.Cash + P.Cheque) + "<td>" + invoiveNoAndBillDate + "<td><td>";
                    recordList.Add(rec);
                }
            }

            double Total = 0;
            int Sequence = 0;
            foreach (Record r in recordList.OrderBy(m => m.SortDate))
            {
                if (r.purchaseID != 0)
                    Total += r.Amount;
                else
                    Total -= r.Amount;
                r.CumulativeTotal = Total;
                r.Sequence = ++Sequence;
            }

            StringBuilder str = new StringBuilder();
            int ctr = 0;
            foreach (Record r in recordList.OrderByDescending(m => m.Sequence))
            {
                str.Append("<tr " + (r.purchaseID == 0 ? "style='background-color: #E8EAB6;'" : "") + "><td title=" + r.purchaseID + ">" + ++ctr + r.row + "<td>" + Cmn.toCurrency(r.CumulativeTotal));
            }
            return "" + (ShowPaymentRows ? "<a href='#' onclick='DownloadFile(\"Purchase.csv\")'>Download&nbsp;<i class='glyphicon glyphicon-download-alt'></i></a>" : "") + "<table class='datatable table-condensed table-striped table-bordered csvTable' style='width:100%;background:white;'><tr><th>SNo<th>Date<th>Vendor<th>Store<th class='alnright'>Amount<th class='alnright'>Invoice No/RC No<th>Items<th>CST<th>Balance<tr><td><td><td><td><b>Total Purchase<b/><td><b>" + Cmn.toCurrency(purchaseTotal) + "</b><th><th><th><th>"
                + str + (!ShowPaymentRows ? "</table> <span style='color:red'>To show paid payments please connect the payments to purchases</span>" : "");
        }

        public static double GetPurchaseSum(DateTime FromDate, DateTime ToDate, int StoreID)
        {
            string Error = "";
            DatabaseCE db = new DatabaseCE();
            try
            {
                Object amt = db.ExecuteScalar("select SUM(Amount) from Purchase where PurchaseDate>='" + FromDate.ToString("dd-MMM-yyyy")
                    + "' and PurchaseDate < '" + ToDate.AddDays(1).ToString("dd-MMM-yyyy") + "' AND StoreID =" + StoreID + "", ref Error);
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
            double Amt = Math.Round(BillingLib.Purchase.GetPurchaseSum(FromDate, FromDate.AddMonths(1).AddDays(-1), StoreID));
            MonthlyTotal mPur = MonthlyTotal.GetByDateTypeAndStore(FromDate, AmountType.Purchase, StoreID);
            if (mPur == null)
                mPur = new MonthlyTotal() { AmountType = AmountType.Purchase, Amount = Amt, TotalDate = FromDate, StoreID = StoreID };
            else
                mPur.Amount = Amt;
            if (Amt != 0)
                mPur.Save();
        }

        internal static List<Purchase> GetInIds(string Ids)
        {
            List<Purchase> list = new List<Purchase>();
            if (Ids == "") return list;
            DatabaseCE db = new DatabaseCE();
            try
            {
                string Error = "";
                IDataReader dr = db.GetDataReader("select * from Purchase where ID IN (" + Ids + ")", ref Error);
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
    }
}



