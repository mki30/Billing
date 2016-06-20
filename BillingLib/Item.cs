using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingLib
{
    public class Item
    {
        public int ID { get; set; }
        public string PLU { get; set; }
        public string Name { get; set; }
        public int TaxRateID { get; set; }
        public double TaxRate { get; set; }
        public double Cost { get; set; }
        public double CostFranchise { get; set; }
        public double MRP { get; set; }
        public double MaxDiscount { get; set; }
        public int BrandID { get; set; }
        public string UrlName { get; set; }
        public UnitType RateType { get; set; }
        public double Weight { get; set; }
        public UnitType WeightType { get; set; }
        public string WeightTypeString { get; set; }
        public int ReOrderLevel { get; set; }
        public double Quantity { get; set; }
        public int GroupID { get; set; }
        public bool IsHidden { get; set; }
        public int ToAdjust { get; set; }

        public string NameLower
        {
            get
            {
                return Name.ToLower();
            }
        }

        public Item()
        {

        }
        public Item(Item i)
        {
            this.ID = i.ID;
            this.PLU = i.PLU;
            this.Name = i.Name;
            this.TaxRateID = i.TaxRateID;
            this.TaxRate = i.TaxRate;
            this.Cost = i.Cost;
            this.CostFranchise = i.CostFranchise;
            this.MRP = i.MRP;
            this.MaxDiscount = i.MaxDiscount;
            this.BrandID = i.BrandID;
            this.UrlName = i.UrlName;
            this.RateType = i.RateType;
            this.Weight = i.Weight;
            this.WeightType = i.WeightType;
            //this.WeightTypeString = ((UnitType)i.WeightType).ToString();
            this.ReOrderLevel = i.ReOrderLevel;
            this.Quantity = i.Quantity;
            this.GroupID = i.GroupID;
            this.IsHidden=i.IsHidden;
            this.ToAdjust = i.ToAdjust;
        }

        static Item GetRecord(IDataReader dr)
        {
            return new Item()
            {
                ID = Cmn.ToInt(dr["ID"]),
                PLU = dr["PLUCode"].ToString(),
                Name = dr["Name"].ToString(),
                TaxRateID = Cmn.ToInt(dr["TaxRateID"]),
                TaxRate = Cmn.ToDbl(dr["TaxRate"]),
                MRP = Cmn.ToInt(dr["MRP"]),
                MaxDiscount = Cmn.ToDbl(dr["MaxDiscount"]),
                Quantity = 1,
                BrandID = Cmn.ToInt(dr["BrandID"]),
                ReOrderLevel = Cmn.ToInt(dr["ReOrderLevel"]),
                Cost = Cmn.ToDbl(dr["Cost"]),
                CostFranchise = Cmn.ToDbl(dr["CostFranchise"]),
                RateType = (UnitType)(Cmn.ToInt(dr["RateType"])),
                Weight = (Cmn.ToDbl(dr["Weight"])),
                WeightType = (UnitType)(Cmn.ToInt(dr["WeightType"])),
                WeightTypeString = ((UnitType)Cmn.ToInt(dr["WeightType"])).ToString(),
                GroupID = Cmn.ToInt(dr["GroupID"]),
                IsHidden=(Cmn.ToInt(dr["IsHidden"])==1?true:false),
                ToAdjust = Cmn.ToInt(dr["ToAdjust"])
            };
        }

        public static void ReLoad(List<Item> list)
        {
            DatabaseCE db = new DatabaseCE();

            try
            {
                foreach (Item i in list)
                {
                    i.Save(db);
                }

            }
            finally
            {
                db.Close();
            }
        }


        public static void UpdateAll(List<Item> list)
        {
            DatabaseCE db = new DatabaseCE();

            try
            {
                db.RunQuery("delete from Item");
                foreach (Item i in list)
                    i.Save(db);
            }
            finally
            {
                db.Close();
            }
        }

        public static int Link(int ItemID1, int ItemID2)
        {
            DatabaseCE db = new DatabaseCE();
            try
            {
                Item item1 = GetByID(ItemID1);
                Item item2 = GetByID(ItemID2);
                if (item1 != null && item2!=null)
                {
                    if (item1.GroupID != 0)
                    {
                        item2.GroupID = item1.GroupID;
                        item2.Save();
                    }
                    else if (item2.GroupID != 0)
                    {
                        item1.GroupID = item2.GroupID;
                        item1.Save();
                    }
                    else
                    {
                        string Error = "";
                        int maxGroupID = db.GetMax("Item", "GroupID", "", ref Error);
                        item1.GroupID = maxGroupID+1;
                        item1.Save();
                        item2.GroupID = maxGroupID + 1;
                        item2.Save();

                    }
                    return item1.GroupID;
                }
            }
            finally
            {
                db.Close();
            }

            return 0;
        }

        public void Save(DatabaseCE _db = null)
        {
            DatabaseCE db = _db == null ? new DatabaseCE() : _db;

            try
            {
                Dictionary<string, object> data2 = new Dictionary<string, object>();
                data2.Add("ID", ID);
                data2.Add("PLUCode", PLU);
                data2.Add("Name", Name);
                data2.Add("MRP", MRP);
                data2.Add("TaxRateID", TaxRateID);
                data2.Add("TaxRate", TaxRate);
                data2.Add("BrandID", BrandID);
                data2.Add("MaxDiscount", MaxDiscount);
                data2.Add("ReOrderLevel", ReOrderLevel);
                data2.Add("Cost", Cost);
                data2.Add("CostFranchise", CostFranchise);
                data2.Add("RateType", (int)RateType);
                data2.Add("Weight", Weight);
                data2.Add("WeightType", (int)WeightType);
                data2.Add("GroupID", GroupID);
                data2.Add("IsHidden",(IsHidden==true?1:0));
                data2.Add("ToAdjust", ToAdjust);
                string Error = TableID.Save("Item", data2, db);
                ID = Cmn.ToInt(data2["ID"]);

                //Global.UpdateItem(this);
            }
            finally
            {
                if (_db == null)
                    db.Close();
            }
        }

        public static Item GetByID(int ID)
        {
            return GetAll(ID).First();
        }
        
        public static List<Item> GetAll(int ID = 0, int BrandID = 0,bool ShowHidden=true,int Adjust=0)
        {
            List<Item> list = new List<Item>();
            DatabaseCE db = new DatabaseCE();
            try
            {
                string Error = "";

                IDataReader dr = db.GetDataReader("select * from Item where 1=1"
                    + (ID == 0 ? "" : " and ID=" + ID)
                    + (BrandID != 0 ? " and BrandID=" + BrandID : "")
                    + (ShowHidden == true ? "" : " and (IsHidden=0 or IsHidden IS NULL)")
                    + (Adjust != 0 ? " and ToAdjust=" + Adjust : "")
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

        public static Item GetByPLU(string PLUCode)
        {
            DatabaseCE db = new DatabaseCE();
            try
            {
                string Error = "";
                IDataReader dr = db.GetDataReader("select * from Item where PLUCode='" + PLUCode + "'", ref Error);
                while (dr.Read())
                {
                    Item item = Item.GetRecord(dr);
                    return item;
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

        //string PlaceHolder = GetBase64String(Server.MapPath(@"~/images/images.jpg"));
        public static string GetItemsHTML(Company comp, string type, string ItemName, int BrandID = 0)
        {
            StringBuilder str = new StringBuilder();
            List<Item> il = new List<Item>();
            Brand b = comp.BrandList.FirstOrDefault(m => m.NameLower == ItemName);
            Menu menu = comp.listMenu.FirstOrDefault(m => m.UrlName == ItemName);
            List<ItemMenuLink> imlList = new List<ItemMenuLink>();
            if (menu != null)
                imlList = ItemMenuLink.GetByMenuID(menu.ID).ToList();
            switch (type)
            {
                case "search":
                    List<Item> ilTemp = new List<Item>();
                    ilTemp.AddRange(comp.ItemList.Where(m => m.NameLower.Contains(ItemName)));
                    if (b != null)
                        ilTemp.AddRange(comp.ItemList.Where(m => m.BrandID == b.ID));
                    foreach (ItemMenuLink iml in imlList)
                        ilTemp.Add(comp.ItemList.FirstOrDefault(m => m.ID == iml.ItemID));
                    il = ilTemp.Distinct().Take(50).ToList();
                    break;
                case "brand":
                    if (b != null)
                        il = comp.ItemList.Where(m => m.BrandID == b.ID).ToList();
                    break;
                case "category":
                    foreach (ItemMenuLink iml in imlList)
                        il.Add(comp.ItemList.FirstOrDefault(m => m.ID == iml.ItemID));
                    break;
            }

            foreach (Item i in il)
            {
                string path = @"C:\Billing\Item_Images\" + i.ID + ".jpg";
                string ImageData = File.Exists(path) ? "data:image/jpg;base64, " + GetBase64String(path) : "/images/images.jpg";

                str.Append(
                "<div class='col-sm-3 itemdiv' id='" + i.ID + "'>"
                + "<table>"
                + "<tr><td><a href='#'><img style='width:80%;' src='" + ImageData + "'></a></td></tr>"
                + "<tr><td><a class='item-name' href='#'>" + i.Name + "</a></td></tr>"
                + "<tr><td><div>" + i.MRP + "&nbsp;<span>/pc</span></div></td></tr>"
                + "<tr style='height:10px'><td>"
                + "<table><tr><td>Qty</td><td><input style='width:60px;' type='number' min='1' max='50' name='quantity' class='form-control' value='1' id='txtQuantity" + i.ID + "'></td>"
                + "<td><a id='addCart" + i.ID + "' class='btn btn-primary btnAddToCart' onclick='AddToCart(" + i.ID + ",\"" + i.Name + "\"," + i.MRP + ")'>Add</a></td>"
                + "<td><a htref='#' id='like" + i.ID + "' class='glyphicon glyphicon-heart gray like' style='font-size:30px;text-decoration:none;' onclick='AddFavouriteItems(" + i.ID + ",\"" + i.Name + "\"," + i.MRP + ")'></a></td></tr></table>"
                + "</td></tr>"
                + "</table>"
                + "</div>");
            }
            return str.ToString();
        }

        static string GetBase64String(string ImagePath)
        {
            using (System.Drawing.Image image = System.Drawing.Image.FromFile(ImagePath))
            {
                using (MemoryStream m = new MemoryStream())
                {
                    image.Save(m, image.RawFormat);
                    byte[] imageBytes = m.ToArray();

                    // Convert byte[] to Base64 String
                    return Convert.ToBase64String(imageBytes);
                }
            }
        }


        public static List<Item> GetAdjustedItems()
        {
            List<Item> list = new List<Item>();
            DatabaseCE db = new DatabaseCE();
            try
            {
                string Error = "";
                IDataReader dr = db.GetDataReader("select * from Item where ToAdjust=1", ref Error);
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