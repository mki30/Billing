using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingLib
{
    public class Order
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Mobile { get; set; }
        public string Address { get; set; }
        public DateTime OrderDate = DateTime.Now;
        public int ProjectID { get; set; }
        public string ProjectOther { get; set; }
        public string ApartmentNo { get; set; }
        public int DeliveryPersonID { get; set; }
        public string DeliveryPersonName { get; set; }
        public DateTime DeliveryDateTime = DateTime.Now;
        public string Message = "";
        public string OrderDateString
        {
            get
            {
                return OrderDate.ToString("%d-MMM-yyyy HH:mm");
            }
        }
        public string DeliveryDateTimeString
        {
            get
            {
                return DeliveryDateTime.ToString("%d-MMM-yyyy HH:mm");
            }
        }

        public string ProjectName="";
        public int StoreID { get; set; }
        public DeliveryStatus delStatus = DeliveryStatus.Requested;
        public string OrderStatus
        {
            get
            {
                return delStatus.ToString();
            }
        }
        public List<OrderItem> OrderItemList = new List<OrderItem>();
        
        static Order GetRecord(IDataReader dr)
        {
            Project p = Global.ProjectList.FirstOrDefault(m => m.ID == Cmn.ToInt(dr["ProjectID"]));

            return new Order()
            {
                ID = Cmn.ToInt(dr["ID"]),
                Name = dr["Name"].ToString(),
                Mobile = dr["Mobile"].ToString(),
                Address = dr["Address"].ToString(),
                ProjectID=Cmn.ToInt(dr["ProjectID"]),
                ProjectName = p!=null?p.ProjectName:"",
                ProjectOther=dr["ProjectOther"].ToString(),
                ApartmentNo = dr["ApartmentNo"].ToString(),
                OrderDate = Cmn.ToDate(dr["OrderDate"]),
                StoreID = Cmn.ToInt(dr["StoreID"]),
                delStatus = (DeliveryStatus)Cmn.ToInt(dr["Status"]),
                DeliveryPersonID = Cmn.ToInt(dr["DeliveryPersonID"]),
                DeliveryPersonName = dr["DeliveryPersonName"].ToString(),
                DeliveryDateTime = Cmn.ToDate(dr["DeliveryDateTime"]),
            };
        }

        public static List<Order> GetAll()
        {
            List<Order> list = new List<Order>();
            DatabaseCE db = new DatabaseCE();
            try
            {
                string Error = "";

                IDataReader dr = db.GetDataReader("select * from OrderOL", ref Error);
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

        public static List<Order> GetByDate(DateTime DateFrom, DateTime DateTo)
        {
            List<Order> list = new List<Order>();
            DatabaseCE db = new DatabaseCE();
            try
            {
                string Error = "";

                IDataReader dr = db.GetDataReader("select * from OrderOL where OrderDate>='"
                    + DateFrom.ToString("dd-MMM-yyyy") + "' and OrderDate<'" + DateTo.AddDays(1).ToString("dd-MMM-yyyy") + "' order by orderdate desc"
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

        public static Order GetByID(int ID, string Mobile = "")
        {
            DatabaseCE db = new DatabaseCE();
            try
            {
                string Error = "";
                IDataReader dr = db.GetDataReader("select * from OrderOL where " + (Mobile.Length == 10 ? " mobile='" + Mobile + "'" : " ID=" + ID) + " order by orderdate desc", ref Error);
                while (dr.Read())
                {
                    Order o = GetRecord(dr);
                    o.OrderItemList = OrderItem.GetByOrderID(o.ID);
                    return o;
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
                data.Add("Name", Name);
                data.Add("Mobile", Mobile);
                data.Add("Address", Address);
                data.Add("ProjectID", ProjectID);
                data.Add("ProjectOther", ProjectOther);
                data.Add("ApartmentNo", ApartmentNo);
                data.Add("OrderDate", OrderDate);
                data.Add("StoreID", StoreID);
                data.Add("Status", (int)delStatus);
                data.Add("DeliveryPersonID", DeliveryPersonID);
                data.Add("DeliveryPersonName", DeliveryPersonName);
                data.Add("DeliveryDateTime", DeliveryDateTime);
                TableID.Save("OrderOL", data);
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
    }
}

