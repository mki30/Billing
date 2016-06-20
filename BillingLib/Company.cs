using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BillingLib
{
    public class Company
    {
        public int ID;
        public string Name = "";
        public string DBName
        {
            get
            {
                return Name.Replace(" ", "") + ".sdf";
            }
        }
        public string Address = "";
        public string TIN = "";
        public string TAN = "";
        public string PAN = "";
        public string CST = "";
        public string CIN = "";
        public string ServiceTax = "";

        public List<Bank> BankList = new List<Bank>();
        public List<Brand> BrandList = new List<Brand>();
        public List<Vendor> vendorList = new List<Vendor>();
        public List<Menu> listMenu = new List<Menu>();
        public List<Item> ItemList = new List<Item>();
        public List<Project> listProject = new List<Project>();
        
        public static Company GetRecord(IDataReader dr)
        {
            return new Company()
            {
                ID = Cmn.ToInt(dr["ID"]),
                Name = dr["Name"].ToString(),
                TIN = dr["TIN"].ToString(),
                TAN = dr["TAN"].ToString(),
                PAN = dr["PAN"].ToString(),
                CST = dr["CST"].ToString(),
                CIN = dr["CIN"].ToString(),
                Address = dr["Address"].ToString(),
                ServiceTax = dr["ServiceTax"].ToString()
            };
        }

        public void LoadGlobal()
        {
            ItemList = Item.GetAll();
            vendorList = Vendor.GetAll();
            listMenu = Menu.GetAll();
            BrandList = Brand.GetAll();
            listMenu = Menu.GetAll(0, true);
            listProject = Project.GetAll();
            
            foreach (Menu m in listMenu)
            {
                if (m.ParentID == 0)
                {
                    m.ChildMenu = listMenu.Where(n => n.ParentID == m.ID).ToList();
                    foreach (Menu k in m.ChildMenu)
                        k.Parent = m;
                }
            }
        }

        public Item GetItem(int ItemID)
        {
            return ItemList.FirstOrDefault(m => m.ID == ItemID);
        }

        public Item GetItem(string PLU)
        {
            double Quantity = 1;
            double MRP = 0;
            Boolean flag = false;

            if (PLU.Contains("%"))
            {
                string[] Data = PLU.Split('%');
                PLU = Data[0];
                Quantity = Cmn.ToDbl(Data[1].Substring(0, 5)) / 1000.0;
                double Amount = Cmn.ToDbl(Data[1].Substring(5)) / 100.0;
                MRP = Amount / Quantity;
                flag = true;
            }

            Item i = ItemList.FirstOrDefault(m => m.PLU == PLU);
            if (i != null)
            {
                i = new Item(i);
                if (flag)
                {
                    if (i.RateType == UnitType.pc)
                    {
                        Quantity *= 1000;
                        MRP /= 1000;
                    }

                    i.Quantity = Quantity;
                    i.MRP = MRP;
                }
            }
            else
                i = new Item();
            return i;
        }
        
        public Item UpdateItem(Item item)
        {
            Item i = ItemList.FirstOrDefault(m => m.ID == item.ID);
            if (i != null)
                ItemList.Remove(i);

            ItemList.Add(item);
            return item;
        }

        public Brand UpdateBrand(Brand brand)
        {
            Brand b = BrandList.FirstOrDefault(m => m.ID == brand.ID);
            if (b != null)
                BrandList.Remove(b);
            BrandList.Add(brand);
            return brand;
        }

        public void LoadItemStorePricing(int StoreID)
        {
            List<ItemStorePrice> list = ItemStorePrice.GetByStoreID(StoreID);
            foreach (ItemStorePrice isp in list)
            {
                if (isp.Rate != 0 && isp.MRP != 0)
                {
                    Item i = GetItem(isp.ItemID);
                    if (i != null)
                    {
                        i.Cost = isp.Rate;
                        i.MRP = isp.MRP;
                    }
                }
            }
        }

        public static Company GetByID(int ID)
        {
            string vError = string.Empty;
            DatabaseCE db = new DatabaseCE();
            try
            {
                string Error = "";
                IDataReader dr = db.GetDataReader("select * from Company where ID=" + ID, ref Error);
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

        public static List<Company> GetAll(int ID = 0)
        {
            List<Company> list = new List<Company>();
            DatabaseCE db = new DatabaseCE();
            try
            {
                string Error = "";

                IDataReader dr = db.GetDataReader("select * from Company " + (ID == 0 ? "" : " where ID=" + ID), ref Error);

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
                data.Add("TIN", TIN);
                data.Add("TAN", TAN);
                data.Add("PAN", PAN);
                data.Add("CST", CST);
                data.Add("CIN", CIN);
                data.Add("Address", Address);
                data.Add("ServiceTax", ServiceTax);
                TableID.Save("Company", data, db);
                ID = Cmn.ToInt(data["ID"]);

                if (!Global.listCompany.ContainsKey(ID))
                    Global.listCompany.Add(ID, this);
                else
                    Global.listCompany[ID] = this;
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
