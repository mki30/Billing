using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BillingLib
{
    public class Employee
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public DateTime DOJ { get; set; }
        public DateTime DOB { get; set; }
        public string Address { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int CompanyID { get; set; }
        public int StoreID { get; set; }
        public string ID2 { get; set; }
        public int IsAdmin { get; set; }
        public int IsRRAAdmin { get; set; }
        public int IsDelete { get; set; }
        public Designation Designation { get; set; }
        public string IPAddress { get; set; }
        public List<Store> storeList = new List<Store>();

        static Employee GetRecord(IDataReader dr)
        {
            return new Employee()
            {
                ID = Cmn.ToInt(dr["ID"]),
                Name = dr["Name"].ToString(),
                DOJ = Cmn.ToDate(dr["DOJ"]),
                DOB = Cmn.ToDate(dr["DOB"]),
                Address = dr["Address"].ToString(),
                Mobile = dr["Mobile"].ToString(),
                Email = dr["Email"].ToString(),
                Password = dr["Password"].ToString(),
                CompanyID = Cmn.ToInt(dr["CompanyID"]),
                StoreID = Cmn.ToInt(dr["StoreID"]),
                ID2 = (dr["ID2"]).ToString(),
                IsAdmin = Cmn.ToInt(dr["IsAdmin"]),
                //IsRRAAdmin = Cmn.ToInt(dr["IsRRAAdmin"]),
                IsDelete = Cmn.ToInt(dr["IsDelete"]),
                Designation = (Designation)Cmn.ToInt(dr["Designation"]),
            };
        }

        public static Dictionary<int, Employee> GetAll(int ID = 0, int StoreId = 0)
        {
            Dictionary<int, Employee> list = new Dictionary<int, Employee>();
            DatabaseCE db = new DatabaseCE();
            try
            {
                string Error = "";
                IDataReader dr = db.GetDataReader("select * from Employee where 1=1 " + (ID == 0 ? "" : " and ID=" + ID) + (StoreId == 0 ? "" : " and storeid=" + StoreId) + " order by Name", ref Error);
                while (dr.Read())
                {
                    Employee e = GetRecord(dr);
                    list.Add(e.ID, e);
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
                if (string.IsNullOrWhiteSpace(ID2))
                    ID2 = Cmn.RandomString();
                data.Add("ID2", ID2);
                data.Add("Name", Cmn.ProperCase(Name));
                data.Add("DOJ", DOJ);
                data.Add("DOB", DOB);
                data.Add("Address", Address);
                data.Add("Mobile", Mobile);
                data.Add("Email", Email.ToLower());
                data.Add("Password", Password);
                data.Add("CompanyID", CompanyID);
                data.Add("StoreID", StoreID);
                data.Add("IsAdmin", IsAdmin);
                data.Add("IsDelete", IsDelete);
                data.Add("Designation", (int)Designation);
                TableID.Save("Employee", data, db);
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

        public static void Save(List<Employee> list)
        {
            string vError = string.Empty;
            DatabaseCE db = new DatabaseCE();
            try
            {
                foreach (Employee e in list)
                {
                    e.Save();
                }
            }
            finally
            {
                db.Close();
            }
        }

        public static int DeleteDeliveryMen()
        {
            DatabaseCE db = new DatabaseCE();
            try
            {
                return db.RunQuery("Delete from Employee where Designation=" + (int)Designation.Delivery);
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

        public static Employee GetByID(int id)
        {
            string vError = string.Empty;
            DatabaseCE db = new DatabaseCE();
            try
            {
                string Error = "";
                IDataReader dr = db.GetDataReader("select * from Employee where ID=" + id, ref Error);
                while (dr.Read())
                {
                    Employee emp = GetRecord(dr);
                    emp.storeList = StoreAccess.GetByUserID(emp.ID);
                    return emp;
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

        public static Employee GetByEmailandPassword(string email, string password)
        {
            DatabaseCE db = new DatabaseCE();
            try
            {
                string Error = "";
                IDataReader dr = db.GetDataReader("select * from Employee where Email='" + email.ToLower().Trim() + "' and password='" + password + "'", ref Error);
                while (dr.Read())
                {
                    Employee emp = GetRecord(dr);
                    emp.storeList = StoreAccess.GetByUserID(emp.ID);
                    return emp;
                }
            }
            finally
            {
                db.Close();
            }
            return null;
        }
        public static Employee GetByEmail(string email)
        {
            DatabaseCE db = new DatabaseCE();
            try
            {
                string Error = "";
                IDataReader dr = db.GetDataReader("select * from Employee where Email='" + email.ToLower() + "'", ref Error);
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

        public static Employee GetByID2(string ID2)
        {

            DatabaseCE db = new DatabaseCE();
            try
            {
                string Error = "";
                IDataReader dr = db.GetDataReader("select * from Employee where ID2='" + ID2 + "'", ref Error);
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
        
        public static Employee DeleteEmp(string Emailid)
        {
            DatabaseCE db = new DatabaseCE();
            try
            {
                db.RunQuery("delete from Employee where email='" + Emailid.ToLower() + "'");

            }
            finally
            {
                db.Close();
            }
            return null;
        }

        public static List<Employee> GetDelievetyMenbyStore(int StoreID)
        {
            List<Employee> list = new List<Employee>();
            DatabaseCE db = new DatabaseCE();
            try
            {
                string Error = "";
                IDataReader dr = db.GetDataReader("select * from Employee where StoreId=" + StoreID + " and Designation=" + (int)Designation.Delivery, ref Error);
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
