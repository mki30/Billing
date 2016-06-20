using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace BillingLib
{
    public enum PaymentMode
    {
        Cash = 0,
        Card = 1,
        COD = 2
    }

    public enum CardType
    {
        None = 0,
        Visa = 1,
        Master = 2,
        American = 3
    }

    public enum OrderStatus
    {
        None=0,
        Pending=1,
        Cleared=2,
    }

    public class Billing
    {
        public int ID;
        public int BillNo;
        public DateTime BillDate = DateTime.Now;
        public DateTime DeliveryTime = Cmn.MinDate;
        public string DeliveryTimeString
        {
            get
            {
                return DeliveryTime.ToString("%d-MMM-yyyy HH:mm") == "1-Jan-00" ? "" : DeliveryTime.ToString("%d-MMM-yyyy HH:mm");
            }
        }
        public double PaidCash { get; set; }
        public double PaidCard { get; set; }
        public CardType cardType = CardType.Visa;

        public string Code = "";
        public string TIN = "";
        public int TerminalID;
        public string CST = "";

        public double VAT1 { get; set; }
        public double VAT2 { get; set; }
        public string DeliveryBy = "";
        public int PaymentRecieved;
        public string Remarks = "";
        public OrderStatus orderStatus = OrderStatus.None;

        public BillingItem SelectedItem = null;
        public Boolean Finished = false;
        public Boolean Duplicate = false;
        public Boolean Delivery = false;

        public Customer customer = new Customer();
        public Company company = new Company();
        public Store store = new Store();
        public List<BillingItem> ItemList = new List<BillingItem>();
        public string Error = "";

        public Billing()
        {
            this.company.Name = "PURE PROTEIN PVT LTD";
        }

        public Billing(int ComanyID, int StoreID, int TerminalID, int BillNo)
        {
            this.company.ID = ComanyID;
            this.store.ID = StoreID;
            this.TerminalID = TerminalID;
            this.BillNo = BillNo;
        }

        public double TotalAmountNoRoundOff
        {
            get
            {
                double total = 0;
                foreach (BillingItem i in ItemList)
                {
                    total += i.Amount;
                }
                return total;
            }
        }

        public double TotalAmount
        {
            get
            {
                return Math.Round(TotalAmountNoRoundOff, MidpointRounding.AwayFromZero);
            }
        }

        public double TotalDiscount
        {
            get
            {
                double total = 0;
                foreach (BillingItem i in ItemList)
                {
                    total += i.DiscountAmount;
                }
                return total;
            }
        }

        public double TotalQuantity
        {
            get
            {
                double total = 0;
                foreach (BillingItem i in ItemList)
                {
                    total += i.Quantity;
                }
                return total;
            }
        }

        static Billing GetRecord(IDataReader dr, DatabaseCE db = null, bool GetBillingItems = true,bool IsfutureTable=false)
        {
            Billing bill = new Billing()
            {
                ID = Cmn.ToInt(dr["ID"]),
                company = new Company() { ID = Cmn.ToInt(dr["CompanyID"]) },
                BillDate = Cmn.ToDate(dr["BillDate"]),
                BillNo = Cmn.ToInt(dr["BillNo"]),
                TerminalID = Cmn.ToInt(dr["TerminalID"]),
                store = new Store() { ID = Cmn.ToInt(dr["StoreID"]) },
                PaidCash = Cmn.ToDbl(dr["PaidCash"]),
                PaidCard = Cmn.ToDbl(dr["PaidCard"]),
                Delivery = Cmn.ToInt(dr["Delivery"]) == 1,
                DeliveryBy = dr["DeliveryBy"].ToString(),
                PaymentRecieved = Cmn.ToInt(dr["PaymentRecieved"]),
                DeliveryTime=Cmn.ToDate(dr["DeliveryTime"]),
                orderStatus=(OrderStatus)Cmn.ToInt(dr["OrderStatus"]),
                
                customer = new Customer()
                {
                    //ID=Cmn.ToInt(dr["ID"]),
                    Mobile = dr["CustomerMobile"].ToString(),
                    HouseNumber = dr["CustomerHouseNumber"].ToString(),
                    Name = dr["CustomerName"].ToString(),
                    Address = dr["CustomerAddress"].ToString(),
                    Area = dr["CustomerArea"].ToString(),
                    City = dr["CustomerCity"].ToString(),
                    PIN = Cmn.ToInt(dr["CustomerPIN"])
                },
            };
            if (db != null && GetBillingItems)
                bill.ItemList = BillingItem.GetByBillingID(Cmn.ToInt(dr["ID"]), db,IsfutureTable);
            return bill;
        }

        public Billing Save()
        {
            if (PaidCash == 0 && PaidCard == 0)
                PaidCash = TotalAmount;

            if (PaidCash > TotalAmount)
                PaidCash = TotalAmount;

            string vError = string.Empty;
            DatabaseCE db = new DatabaseCE();
            try
            {
                Dictionary<string, object> data = new Dictionary<string, object>();
                data.Add("ID", ID);
                data.Add("BillNo", BillNo);
                data.Add("BillDate", BillDate);
                data.Add("CompanyID", company.ID);
                data.Add("StoreID", store.ID);
                data.Add("TerminalID", TerminalID);

                data.Add("CustomerID", customer.ID);
                data.Add("CustomerName", Cmn.ProperCase(customer.Name));
                data.Add("CustomerHouseNumber", Cmn.ProperCase(customer.HouseNumber));
                data.Add("CustomerAddress", Cmn.ProperCase(customer.Address));
                data.Add("CustomerArea", Cmn.ProperCase(customer.Area));
                data.Add("CustomerCity", Cmn.ProperCase(customer.City));
                data.Add("CustomerPIN", customer.PIN);
                data.Add("CustomerMobile", customer.Mobile);


                data.Add("Delivery", Delivery ? 1 : 0);
                data.Add("DeliveryBy", DeliveryBy);
                data.Add("DeliveryTime", DeliveryTime);
                data.Add("PaymentRecieved", PaymentRecieved);

                data.Add("PaidCash", PaidCash);
                data.Add("PaidCard", PaidCard);
                data.Add("CardType", (int)cardType);

                data.Add("ItemCount", ItemList.Count);
                data.Add("VAT1", VAT1);
                data.Add("VAT2", VAT2);

                data.Add("OrderStatus", (int)orderStatus);

                Boolean futureOrder =orderStatus == OrderStatus.Pending;
                Error = TableID.Save(futureOrder ?"FutureBilling":"Billing", data, db);
                
                ID = Cmn.ToInt(data["ID"]);

                db.RunQuery("update terminal set billno=" + BillNo + " where ID=" + TerminalID);
                db.RunQuery("delete from Billingitem where BillingID=" + ID);
                
                foreach (BillingItem i in ItemList)
                {
                    i.BillingID = ID;
                    i.BillNo = BillNo;
                    i.SaleDate = BillDate;
                    i.StoreID = store.ID;
                    i.SaveBillingItem(db, futureOrder);
                }
                if (data["CustomerMobile"].ToString() != "")
                {
                    customer.Save(this);
                    db.RunQuery("update billing set customerid=" + customer.ID + " where id=" + ID);
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
            Error = "OK";

            return this;
        }

        public static Billing GetByID(int ID, int BillNo = 0, int StoreID = 0, int TerminalID = 0, Boolean IsFutureBill = false)
        {
            DatabaseCE db = new DatabaseCE();
            try
            {
                string Error = "";
                IDataReader dr = db.GetDataReader("select * from " + (IsFutureBill ? "FutureBilling" : "Billing") 
                    + " where " + (BillNo > 0 ? " BillNo=" + BillNo+" and StoreID="+StoreID+" and TerminalID="+TerminalID : " ID=" + ID), ref Error);
                while (dr.Read())
                {
                    Billing bill = Billing.GetRecord(dr);
                    bill.ItemList = BillingItem.GetByBillingID(bill.ID, db,IsFutureBill);
                    return bill;
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

        public static Billing GetByBillNo(int BillNo, int StoreID, int TerminalID=0)
        {
            DatabaseCE db = new DatabaseCE();
            string Error = "";
            try
            {
                IDataReader dr = db.GetDataReader("select * from Billing where billno=" + BillNo + " and StoreID=" + StoreID 
                    + (TerminalID!=0?" and TerminalID=" + TerminalID:""), ref Error);
                while (dr.Read())
                {
                    return GetRecord(dr, db, false);
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
            return null;
        }

        public static List<Billing> GetByBillNo(int BillNo,bool IncludeItems=false)
        {
            DatabaseCE db = new DatabaseCE();
            List<Billing> list = new List<Billing>();
            string Error = "";
            try
            {
                IDataReader dr = db.GetDataReader("select * from Billing where billno=" + BillNo, ref Error);
                while (dr.Read())
                {
                    list.Add(GetRecord(dr, db, IncludeItems));
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
        
        public static List<Billing> GetAll(Boolean IsFutureTable, DateTime DateFrom, DateTime DateTo, string mobile = "", int companyID = 0, int storeID = 0, int terminalID = 0, bool GetBillingItems = true, int delivery = 0, string deliveryboy="")
        {
            DatabaseCE db = new DatabaseCE();
            List<Billing> list = new List<Billing>();
            string Error = "";
            try
            {
                IDataReader dr = db.GetDataReader("select * from " + (IsFutureTable ? "FutureBilling" : "Billing") + " where BillDate>='"
                    + DateFrom.ToString("dd-MMM-yyyy") + "' and BillDate<'" + DateTo.AddDays(1).Date.ToString("dd-MMM-yyyy")
                    + "'" + (mobile != "" ? " and CustomerMobile=" + mobile : "") + ""
                    + (companyID != 0 ? " and CompanyID=" + companyID : "")
                    + (storeID != 0 ? " and StoreID=" + storeID : "")
                    + (terminalID != 0 ? " and TerminalID=" + terminalID : "")
                    + (delivery != 0 ? " and "+(delivery==1?"delivery=" + delivery:"delivery<>1") : "")
                    + (deliveryboy != "" ? " and DeliveryBy='" + deliveryboy+"'" :"")
                    , ref Error);
                while (dr.Read())
                {
                    list.Add(GetRecord(dr, db, GetBillingItems, IsFutureTable));
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
        
        public static List<Billing> GetByDate(DateTime FromDate, DateTime ToDate,Boolean IsFutureOrder=false)
        {
            DatabaseCE db = new DatabaseCE();
            List<Billing> list = new List<Billing>();
            string Error = "";
            try
            {
                IDataReader dr = db.GetDataReader("select * from " + (IsFutureOrder ? "FutureBilling" : "Billing") + " where BillDate>='" + FromDate.ToString("dd-MMM-yyyy")
                    + "' and BillDate < '" + ToDate.AddDays(1).ToString("dd-MMM-yyyy") + "' order by BillDate", ref Error);
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

        public static List<Billing> GetByMobile(string mobileNo)
        {
            DatabaseCE db = new DatabaseCE();
            List<Billing> list = new List<Billing>();
            string Error = "";
            try
            {
                IDataReader dr = db.GetDataReader("select * from Billing where CustomerMobile='" + mobileNo + "'", ref Error);
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
        
        public Billing Delete(Boolean IsFutureBill=false)
        {
            DatabaseCE db = new DatabaseCE();
            try
            {
                db.RunQuery("Delete from " + (IsFutureBill ? "FutureBilling" : "Billing") + " where ID=" + ID);
                db.RunQuery("Delete from " + (IsFutureBill ? "FutureBillingItem" : "BillingItem") + " where BillingID=" + ID);
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

        public static double GetBillSum(DateTime FromDate, DateTime ToDate, int StoreID)
        {
            string Error = "";
            DatabaseCE db = new DatabaseCE();
            try
            {
                Object amt = db.ExecuteScalar("select SUM(PaidCash+PaidCard) from Billing where BillDate>='" + FromDate.ToString("dd-MMM-yyyy")
                    + "' and BillDate < '" + ToDate.AddDays(1).ToString("dd-MMM-yyyy") + "' AND StoreID =" + StoreID + "", ref Error);
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
            double billSum = Math.Round(BillingLib.Billing.GetBillSum(FromDate, FromDate.AddMonths(1).AddDays(-1), StoreID), 0);
            MonthlyTotal mMonth = MonthlyTotal.GetByDateTypeAndStore(FromDate, AmountType.Sale, StoreID);
            if (mMonth == null)
                mMonth = new MonthlyTotal() { AmountType = AmountType.Sale, Amount = billSum, TotalDate = FromDate, StoreID = StoreID };
            mMonth.Amount = billSum;
            mMonth.Save();
        }
    }
}