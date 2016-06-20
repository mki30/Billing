using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingLib
{
    public class ClientInvoice
    {
        public int ID { get; set; }
        public int ClientID { get; set; }
        public int BillNo { get; set; }
        public int PONo { get; set; }
        public string Transport { get; set; }
        public string VehicleNo { get; set; }
        public string Station { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime CreatedDate = Cmn.MinDate;
        public string BookNo { get; set; }

        public static ClientInvoice GetRecord(IDataReader dr)
        {
            return new ClientInvoice()
            {
                ID = Cmn.ToInt(dr["ID"]),
                ClientID = Cmn.ToInt(dr["ClientID"]),
                BillNo = Cmn.ToInt(dr["BillNo"]),
                PONo = Cmn.ToInt(dr["PONo"]),
                Transport = dr["Transport"].ToString(),
                VehicleNo = dr["VehicleNo"].ToString(),
                Station = dr["Station"].ToString(),
                InvoiceNo = dr["InvoiceNo"].ToString(),
                CreatedDate=Cmn.ToDate(dr["CreatedDate"]),
                BookNo = dr["BookNo"].ToString()
            };
        }

        public static ClientInvoice GetByID(int ID)
        {
            string vError = string.Empty;
            DatabaseCE db = new DatabaseCE();
            try
            {
                string Error = "";
                IDataReader dr = db.GetDataReader("select * from ClientInvoice where ID=" + ID, ref Error);
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

        public void Save()
        {
            string vError = string.Empty;
            DatabaseCE db = new DatabaseCE();
            try
            {
                Dictionary<string, object> data = new Dictionary<string, object>();
                data.Add("ID", ID);
                data.Add("ClientID", ClientID);
                data.Add("BillNo", BillNo);
                data.Add("PONo", PONo);
                data.Add("Transport", Transport);
                data.Add("VehicleNo", VehicleNo);
                data.Add("Station", Station);
                data.Add("InvoiceNo", InvoiceNo);
                data.Add("CreatedDate", CreatedDate);
                data.Add("BookNo", BookNo);
                TableID.Save("ClientInvoice", data, db);
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

        public static List<ClientInvoice> GetInvoiceLIst()
        {
            List<ClientInvoice> list = new List<ClientInvoice>();
            DatabaseCE db = new DatabaseCE();
            try
            {
                string Error = "";

                IDataReader dr = db.GetDataReader("select * from ClientInvoice", ref Error);

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

        public static ClientInvoice GetByBillNo(int BillNo)
        {
            string vError = string.Empty;
            DatabaseCE db = new DatabaseCE();
            try
            {
                string Error = "";
                IDataReader dr = db.GetDataReader("select * from ClientInvoice where BillNo=" + BillNo, ref Error);
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
    }
}
