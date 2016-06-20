using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;
using System.Web;
using System.Net;
using System.Net.Mail;
using Menu = BillingLib.Menu;
using BillingLib;

public partial class Data : BasePage
{
    StringBuilder str = new StringBuilder();
    Boolean AppendError = true;
    string encode = "no";

    protected new void Page_Load(object sender, EventArgs e)
    {
        Action = QueryString("Action");
        string Data1 = Request.QueryString["Data1"] != null ? Request.QueryString["Data1"].ToString() : "";
        string Data2 = Request.QueryString["Data2"] != null ? Request.QueryString["Data2"].ToString() : "";
        string Data3 = Request.QueryString["Data3"] != null ? Request.QueryString["Data3"].ToString() : "";
        string Data4 = Request.QueryString["Data4"] != null ? Request.QueryString["Data4"].ToString() : "";
        string Data5 = Request.QueryString["Data5"] != null ? Request.QueryString["Data5"].ToString() : "";
        string Data6 = Request.QueryString["Data6"] != null ? Request.QueryString["Data6"].ToString() : "";
        string Data7 = Request.QueryString["Data7"] != null ? Request.QueryString["Data7"].ToString() : "";
        string Data8 = Request.QueryString["Data8"] != null ? Request.QueryString["Data8"].ToString() : "";

        string abc = HttpContext.Current.Request.UserAgent;
        string term = Request.QueryString["term"] != null ? Request.QueryString["term"].ToString() : "";
        string query = Request.QueryString["query"] != null ? Request.QueryString["query"].ToString() : "";
        string Error = "";

        encode = GetEncode(this, Action != "Image");
        try
        {
            switch (Action)
            {
                case "":
                    return;//for ping
                case "SignIn": GoogleSignIn(Data1); return;
                case "GetStoreAccess": GetStoreAccess(); return;
                case "SetStore": SetLoginStore(Cmn.ToInt(Data1)); return;
                case "WebData":
                    AppendError = false;
                    WebData((WebDataAction)Cmn.ToInt(Data1));
                    return;
                case "Image":
                    Response.ContentType = "image/jpeg"; // for JPEG file
                    string dir = "Item_Images";
                    if (Data2 == "Brand")
                        dir = "Brand_Images";
                    string path = @"C:\Billing\\" + dir + "\\" + Data1 + ".jpg";
                    Response.WriteFile(File.Exists(path) ? path : Server.MapPath(@"~/images/images.jpg"));
                    AppendError = false;
                    return;
                case "GetSocietyList": GetSocietyList(); return;
                case "SaveOrder": SaveOrder(Cmn.ToInt(Data1), Data2, Data3, Data4, Data5, Data6, Cmn.ToInt(Data7)==1?true:false); return;
                case "TraceOrder": TraceOrder(Cmn.ToInt(Data1), Data2); return;

            }

            if (!SessionState.LogInDone)
            {
                str.Append("Not logged in click here to <a href='/login.aspx'>login</a>.");
                return;
            }

            switch (Action)
            {
                case "GetInventoryHTML": GetInventoryHTML(Cmn.ToInt(Data1)); break;
                case "GetProjectName": GetProjectName(Cmn.ToInt(Data1)); break;
                case "GetProjectJSON": GetProjectJSON(query); break;
                //case "GetDatabase": ZipFiles.DownloadZipToBrowser(new List<string>() { Server.MapPath("~/data/billing.sdf") }, this, "Billingdb"); break;
                case "UpdateFieldValue": UpdateFieldValue(Data1, Data2, Data3, Data4); break;
                case "ItemMultilLink": ItemMultilLink(Cmn.ToInt(Data1), Cmn.ToInt(Data2), Data3); break;
                case "GetStock": str.Append(GetStock()); break;
                case "UpdateSingleField": UpdateSingleField(Data1, Cmn.ToInt(Data2), Data3, Data4); break;
                case "UpdateDeliveryBoy": UpdateDeliveryBoy(Cmn.ToInt(Data1), Data2); break;
                case "ChangeVendor": ChangeVendor(Cmn.ToInt(Data1), Cmn.ToInt(Data2)); break;
                case "SaveProject": SaveProject(); break;
                case "GetProject": GetProject(Cmn.ToInt(Data1)); break;
                case "GetItemByPLUandCreate": GetItemByPLUandCreate(Data1, Cmn.ToInt(Data2)); break;
                case "AssignDelieveryMan": AssignDelieveryMan(Cmn.ToInt(Data1), Cmn.ToInt(Data2), Data3); break;
                case "GetDeliveryManList": GetDeliveryManList(Cmn.ToInt(Data1)); break;

                case "AutolinkApartmentWithCustomer": AutolinkApartmentWithCustomer(Cmn.ToInt(Data1), Data2, Data3, Cmn.ToInt(Data4)); break;
                case "UpdateLastPurchase": UpdateLastPurchase(Data1, Cmn.ToInt(Data2)); break;
                case "LinkCustomerWithApartment": LinkCustomerWithApartment(Cmn.ToInt(Data1), Cmn.ToInt(Data2), Cmn.ToInt(Data3)); break;
                case "GetCustomerByMobile": GetCustomerByMobile(Data1); break;
                case "GetProjectApartments": GetProjectApartments(Cmn.ToInt(Data1)); break;
                //case "UpdateManualClosingStock": UpdateManualClosingStock(Cmn.ToInt(Data1)); break;
                //case "ShowStockTakingDetailList": ShowStockTakingDetailList(Cmn.ToInt(Data1)); break;
                case "AddManualClosingStock":
                    {
                        AppendError = false;
                        NameValueCollection nvc = Request.Form;
                        int ItemID = Cmn.ToInt(nvc["ItemID"]);
                        double Quantity = Cmn.ToDbl(nvc["Quantity"]);
                        DateTime dtMonth = Cmn.MonthFirst(Cmn.ToDate(nvc["Date"]));

                        if (DateTime.Today == Cmn.MonthLast(DateTime.Today))  //If today is last day
                        {
                            AddManualClosingStock(ItemID, dtMonth, Quantity, false); //this month closing
                            AddManualClosingStock(ItemID, dtMonth.AddMonths(1), Quantity, true);//next month opening
                        }
                        else
                        {
                            AddManualOpeningStock(ItemID, dtMonth, Quantity); //this month opening
                        }
                    }
                    break;
                case "DeletePurchasePaymentLink": DeletePurchasePaymentLink(Cmn.ToInt(Data1)); break;
                case "GetPurchaseList":
                    AppendError = false;
                    str.Append(BillingLib.Purchase.GetHTMLList(SessionState.Company, Cmn.ToDate(Data1), Cmn.ToDate(Data2), Cmn.ToInt(Data3), SessionState.StoreID, false, Cmn.ToInt(Data4))); break;
                case "UpdatePurchasePaymentLink":
                    AppendError = false;
                    UpdatePurchasePaymentLink(Cmn.ToInt(Data1), Cmn.ToInt(Data2), Cmn.ToInt(Data3), Cmn.ToDbl(Data4));
                    break;
                case "GetPurchasePaymentLink":
                    AppendError = false;
                    GetPurchasePaymentLink(Cmn.ToInt(Data1));
                    break;

                case "UpdateBill": UpdateBill(); break;
                case "GetMonthlyTotal": GetMonthlyTotal(Cmn.ToDate(Data1), (AmountType)Cmn.ToInt(Data2), Cmn.ToInt(Data3)); break;
                case "SaveMonthlyTotal": SaveMonthlyTotal(Cmn.ToInt(Data1)); break;
                case "GetSalary": GetSalary(Cmn.ToInt(Data1), Cmn.ToDate(Data2)); break;
                case "SaveSalary": SaveSalary(); break;
                case "UpdateItemPrice": UpdateItemPrice(Cmn.ToInt(Data1), Cmn.ToInt(Data2)); break;
                case "WriteCSVFile": WriteCSVFile(); break;
                case "SaveCustomer": SaveCustomer(); break;
                case "GetCustomer": GetCustomer(Cmn.ToInt(Data1)); break;
                case "DeleteAttendance": DeleteAttendance(Cmn.ToInt(Data1)); break;
                case "ChangeOrderStatus": ChangeOrderStatus(Cmn.ToInt(Data1), Cmn.ToInt(Data2), Data3); break;
                //case "TraceOrder": TraceOrder(Cmn.ToInt(Data1), Data2); break;
                case "OrderAssign": AssignOrder(Cmn.ToInt(Data1), Cmn.ToInt(Data2)); break;
                //case "SaveOrder": SaveOrder(Cmn.ToInt(Data1), Data2, Data3, Data4, Data5, Data6); break;
                case "SaveOrderItem": SaveOrderItem(); break;
                case "GetItems": GetItems(Data1, Data2); break;
                case "GetMenueLinkedItems": GetMenueLinkedItems(Cmn.ToInt(Data1)); break;
                case "DeleteMenuItemLink": DeleteMenuItemLink(Cmn.ToInt(Data1)); break;
                case "ItemCSV":
                    AppendError = false;
                    Response.ContentType = "application/CSV";
                    Response.AddHeader("Content-Disposition", "attachment;filename=ItemList.csv");
                    str.Append(ItemCSV());
                    break;

                case "GetExpense": AppendError = false; GetExpense(Cmn.ToInt(Data1)); break;
                case "GetInventoryTransferList": AppendError = false;
                    GetInventoryTransferList(Cmn.ToDate(Data1), Cmn.ToDate(Data2), Cmn.ToInt(Data3), Cmn.ToInt(Data4), Cmn.ToInt(Data5), (RecordType)Cmn.ToInt(Data6));
                    break;
                case "GetInventoryList": AppendError = false;
                    GetInventoryList(Cmn.ToInt(Data1), (RecordType)Cmn.ToInt(Data2), Cmn.ToDate(Data3), Cmn.ToDate(Data4), Cmn.ToInt(Data5), Cmn.ToInt(Data6));
                    break;

                case "GetInventoryListFull": AppendError = false;
                    GetInventoryListFull(Cmn.ToDate(Data1), Cmn.ToDate(Data2), Cmn.ToInt(Data3));
                    break;

                case "GetPayment": AppendError = false; GetPayment(Cmn.ToInt(Data1)); break;
                case "SavePayment": SavePayment(); break;
                case "SaveExpense": SaveExpense(); break;
                case "SavePurchase": SavePurchase(); break;
                case "SaveAttendance": SaveAttendance(Cmn.ToInt(Data1), Data2 == "0"); break;
                case "SaveAttendance2": SaveAttendance2(); break;
                case "GetAttendance": AppendError = false; GetAttendance(Cmn.ToInt(Data1)); break;
                case "RemoveStore": RemoveStore(Cmn.ToInt(Data1), Cmn.ToInt(Data2)); break;
                case "AssignTerminal": AssignTerminal(Cmn.ToInt(Data1), Cmn.ToInt(Data2)); break;
                case "AssignStore": AssignStore(Cmn.ToInt(Data1), Cmn.ToInt(Data2)); break;
                case "GetItemJSON": GetItemJSON(query); break;
                case "GetItemJSON2": GetItemJSON2(query); break;
                case "GetItemJSONSkipLinked": GetItemJSONSkipLinked(query, Cmn.ToInt(Data1)); break;
                case "SaveInventory": SaveInventory(); AppendError = false; break;
                case "SaveReturn": SaveReturn((RecordType)Cmn.ToInt(Data1)); AppendError = false; break;
                case "GetInventory": GetInventory(Cmn.ToInt(Data1)); AppendError = false; break;
                case "GetInventoryByItem": GetInventoryByItem(Cmn.ToInt(Data1), Cmn.ToInt(Data2), (RecordType)Cmn.ToInt(Data3)); break;

                case "UpdatePaymentMode": UpdatePaymentMode(Cmn.ToInt(Data1), Cmn.ToDbl(Data2), Cmn.ToDbl(Data3)); break;
                case "GetItem": GetItem(Data1); break;
                case "GetItemForList": GetItemForList(Cmn.ToInt(Data1)); break;
                case "GetSerchedItem": GetSerchedItem(Data1); break;

                case "GetSerchedItem2": GetSerchedItem(Data1, (RecordType)Cmn.ToInt(Data2), Cmn.ToInt(Data3), Cmn.ToInt(Data4)); break;
                case "GetStoreList": GetStoreList(); break;
                case "GetTerminalList": GetTerminalList(Data1); break;
                case "GetCustomerList": GetCustomer(Data1); break;
                //case "Image":
                //    Response.ContentType = "image/jpeg"; // for JPEG file
                //    string dir = "Item_Images";
                //    if (Data2 == "Brand")
                //        dir = "Brand_Images";
                //    string path = @"C:\Billing\\" + dir + "\\" + Data1 + ".jpg";
                //    Response.WriteFile(File.Exists(path) ? path : Server.MapPath(@"~/images/images.jpg"));
                //    AppendError = false;
                //    return;
                case "FileUpload": FileUpload(Data1); break;
                case "GetItemList":
                    {
                        List<BillingLib.Item> list = BillingLib.Item.GetAll();
                        AppendError = false;
                        WriteResponse(this, new JavaScriptSerializer().Serialize(list), encode);
                    }
                    break;
                case "GetItemByPLU":
                    {
                        AppendError = false;
                        BillingLib.Item item = BillingLib.Item.GetByPLU(Data1);
                        if (item != null)
                        {
                            str.Append(item.ID);
                        }
                    }
                    break;
                case "GetBillingList": GetBillingList(Cmn.ToDate(Data1), Cmn.ToDate(Data2), Data3, Cmn.ToInt(Data4), Cmn.ToInt(Data5), Cmn.ToInt(Data6), Cmn.ToInt(Data7), Data8); break;
                case "GetBillDetail": GetBillDetail(Cmn.ToInt(Data1)); break;
                case "Image_Upload":
                    {
                        string folder = FolderCheck("~/image/");
                        foreach (string f in Request.Files.AllKeys)
                        {
                            HttpPostedFile file = Request.Files[f];
                            string filePath = Path.GetFullPath(file.FileName);
                            file.SaveAs(Server.MapPath(folder + "/") + Data1 + ".jpg");
                            if (File.Exists(Server.MapPath(folder + "/") + Data1 + ".jpg"))
                                str.Append(file.FileName);
                        }
                    }
                    break;
                case "UpdateItemMenuLink":
                    {
                        BillingLib.ItemMenuLink mIl = BillingLib.ItemMenuLink.GetByItemIDAndMenuID(Cmn.ToInt(Data1), Cmn.ToInt(Data2));
                        if (mIl == null)
                        {
                            mIl = new BillingLib.ItemMenuLink();
                        }
                        mIl.ItemID = Cmn.ToInt(Data1);
                        mIl.MenuID = Cmn.ToInt(Data2);

                        if (mIl.Save() == "")
                        {
                            str.Append("ok");
                        }
                        else
                            str.Append("Error");
                        AppendError = false;
                    }
                    break;
                case "UpdateItemMenuLinkMultiple":
                    {
                        string[] ItemIds = GetFormString("ItemIds").Split(',');
                        foreach (string ItemID in ItemIds)
                        {
                            BillingLib.ItemMenuLink mIl = BillingLib.ItemMenuLink.GetByItemIDAndMenuID(Cmn.ToInt(ItemID), Cmn.ToInt(Data2));
                            if (mIl == null)
                            {
                                mIl = new BillingLib.ItemMenuLink();
                            }
                            mIl.ItemID = Cmn.ToInt(ItemID);
                            mIl.MenuID = Cmn.ToInt(Data2);
                            mIl.Save();
                        }
                        AppendError = false;
                    }
                    break;
                case "MENU_ACTION":
                    {
                        string Name = GetFormString("New_Name");
                        if (Name.Contains("("))
                            Name = Name.Split('(')[0];

                        int MT = Cmn.ToInt(Data3), menuid = Cmn.ToInt(Data2);
                        switch (Data1)
                        {
                            case "1"://rename
                                {
                                    BillingLib.Menu m = BillingLib.Menu.GetAll(menuid).FirstOrDefault();
                                    if (m != null)
                                    {
                                        m.Name = Name;
                                        m.UrlName = Cmn.GenerateSlug(Name);
                                        m.Save();
                                    }
                                }
                                break;
                            case "2"://create
                                {
                                    //get menu with id to create child nodes
                                    {
                                        BillingLib.Menu m = new BillingLib.Menu();
                                        m.ParentID = Cmn.ToInt(Data2);
                                        m.Name = Name;
                                        m.UrlName = Cmn.GenerateSlug(Name);
                                        m.Save();
                                    }
                                }
                                break;
                            case "3"://delete
                                {
                                }
                                break;
                            case "4"://change paent or move to another node
                                {
                                    //int ID = GetFormInt("ID"), TOID = GetFormInt("TOID");
                                } break;
                        }
                        str.Append("ok");
                        AppendError = false;
                    }
                    break;
                case "GET_CHILDREN":
                    {
                        GET_CHILDREN(Cmn.ToInt(Data1), Cmn.ToInt(Data2));
                        AppendError = false;
                    } break;
            }
        }
        catch (Exception ex)
        {
            AppendError = true;
            Error += ex.Message + "," + ex.StackTrace;
            Error = ex.Message + ex.StackTrace + "-" + Error;
        }
        finally
        {

            WriteResponse(this, ((AppendError ? Error + "~" : "") + str.ToString()), encode);
        }
    }

    private void GetInventoryHTML(int PurchaseID)
    {
        AppendError = false;
        str.Append(BillingLib.Inventory.GetHTML(SessionState.Company, PurchaseID));
    }

    private void SetLoginStore(int StoreID)
    {
        AppendError = false;
        SessionState.StoreID = StoreID;
        str.Append("OK");
    }

    private void GoogleSignIn(string Impersonate)
    {
        AppendError = false;
        //https://developers.google.com/identity/protocols/OpenIDConnect#server-flow
        //IP address should also be tracked to avoid multiple login and duplicate logins
        List<BillingLib.Store> sa = new List<BillingLib.Store>();
        NameValueCollection nvc = Request.Form;
        try
        {
            using (WebClient w = new WebClient())
            {
                string data = w.DownloadString("https://www.googleapis.com/oauth2/v3/tokeninfo?id_token=" + nvc["token"]);
                Dictionary<string, object> singInData = (Dictionary<string, object>)new JavaScriptSerializer().DeserializeObject(data);

                Employee u = new Employee();

                u.IPAddress = Request.UserHostAddress;
                foreach (string key in singInData.Keys)
                {
                    switch (key)
                    {
                        case "email": u.Email = singInData[key].ToString(); break;
                        case "name": u.Name = singInData[key].ToString(); break;
                        case "picture": SessionState.UserImage = singInData[key].ToString(); break;
                    }
                }

                if (u.Email == "vimalkmail@gmail.com" || u.Email == "psu.singh@gmail.com")
                {
                    if (Impersonate != "")
                    {
                        u = Employee.GetByEmail(Impersonate);
                    }
                    else
                    {
                        SessionState.IsAdminRRA = true;
                        SessionState.IsAdmin = true;
                        //u.IsAdmin = 1;
                        //u.IsRRAAdmin = 1;
                        u.CompanyID = 1;
                        u.StoreID = 3;
                        str.Append("Admin");
                    }
                }
                else
                    u = Employee.GetByEmail(u.Email);

                if (u != null)
                {
                    if (!Global.Users.ContainsKey(u.Email))
                        Global.Users.Add(u.Email, u);
                    Global.Users[u.Email] = u;
                    HttpCookie c = new HttpCookie("email", u.Email);
                    Response.Cookies.Add(c);

                    SessionState.LogInDone = true;
                    if (!SessionState.IsAdminRRA)
                    {
                        SessionState.IsAdmin = u.IsAdmin == 1 ? true : false;
                    }
                    SessionState.CompanyID = u.CompanyID;
                    SessionState.UserEmailID = u.Email;
                    SessionState.UserID = u.ID;
                    SessionState.StoreID = u.StoreID;

                    str.Append("OK");
                    try
                    {
                        CheckDailyTask();
                    }
                    catch (Exception ex)
                    {
                    }
                }
                else
                {
                    str.Append("Account Not found");
                }
            }
        }
        catch
        {
            str.Append("Authentication server not accessible");
        }
    }

    void CheckDailyTask()
    {
        Dictionary<string, DateTime> Tasks = new Dictionary<string, DateTime>();
        Dictionary<string, DateTime> NewTasks = new Dictionary<string, DateTime>();
        Tasks.Add("Stock Update", Cmn.MinDate);
        Tasks.Add("Budget Update", Cmn.MinDate);

        //Write task log file if not exist
        string FileName = Server.MapPath(@"~\tasklog.txt");
        if (File.Exists(FileName))
        {
            string[] Lines = File.ReadAllLines(FileName);
            foreach (string L in Lines)
            {
                string[] F = L.Split('=');
                if (Tasks.ContainsKey(F[0]))
                    Tasks[F[0]] = Cmn.ToDate(F[1]);
            }
        }

        //if today is first day or last update was 7 days old
        foreach (KeyValuePair<string, DateTime> kvp in Tasks)
        {
            double DaysOld = (DateTime.Now - kvp.Value).TotalDays;
            Boolean Update = DaysOld > 7 || (DateTime.Now.Day == 1 && DaysOld > 7);
            if (Update)
            {
                NewTasks.Add(kvp.Key, DateTime.Now);
                switch (kvp.Key)
                {
                    case "Stock Update":
                        BillingLib.Stock.GetCurrentStock(SessionState.Company, SessionState.StoreID, new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(-1), true);
                        break;
                    case "Budget Update":
                        BillingLib.MonthlyTotal.UpdateMonthlyValues(DateTime.Now, SessionState.StoreID);
                        break;
                }
            }
        }

        if (NewTasks.Count > 0)
        {
            string str = "";
            foreach (KeyValuePair<string, DateTime> kvp in NewTasks)
            {
                str += kvp.Key + "=" + kvp.Value.ToString("dd-MMM-yyyy HH:mm") + Environment.NewLine;
            }

            File.WriteAllText(FileName, str);
        }
    }

    private void GetProjectName(int ProjectID)
    {
        AppendError = false;
        BillingLib.Project P = BillingLib.Project.GetByID(ProjectID);
        if (P != null)
            str.Append(P.ProjectName);
    }

    private void UpdateFieldValue(string fieldName, string fieldValue, string table, string PrimaryKeys)
    {
        AppendError = false;
        if (fieldValue == "" || PrimaryKeys == "")
        {
            str.Append("Error");
            return;
        }
        DatabaseCE db = new DatabaseCE();
        try
        {
            db.RunQuery("update " + table + " set " + fieldName + "='" + fieldValue + "' where id IN (" + PrimaryKeys + ")");
            str.Append("Saved");
        }
        catch
        {
            str.Append("Error");
        }
        finally
        {
            db.Close();
        }
    }

    private void ItemMultilLink(int BrandID, int CategoryID, string ItemIDsToLink)
    {
        AppendError = false;
        string[] Items = ItemIDsToLink.Split(',');
        foreach (string id in Items)
        {
            try
            {
                BillingLib.Item i = BillingLib.Item.GetByID(Cmn.ToInt(id));
                if (i != null)
                {
                    if (BrandID != 0)
                    {
                        i.BrandID = BrandID;
                        i.Save();
                    }
                    if (CategoryID != 0)
                    {
                        BillingLib.ItemMenuLink mIl = new BillingLib.ItemMenuLink()
                        {
                            ItemID = i.ID,
                            MenuID = CategoryID,
                        };
                        mIl.Link();
                    }
                }
            }
            catch
            {
                str.Append("Error!");
            }
        }
        str.Append("Saved");
    }

    private string GetStock()
    {
        AppendError = false;

        DateTime startDate = new DateTime(FilterDateFrom.Year, FilterDateFrom.Month, 1);
        DateTime endDate = new DateTime(startDate.Year, startDate.Month, 1).AddMonths(1).AddDays(-1);
        Boolean UpdateStock = QueryString("updatestock") == "1";

        List<BillingLib.Stock> listStock = BillingLib.Stock.GetCurrentStock(SessionState.Company, SessionState.StoreID, startDate, UpdateStock);
        string header = "<table id='DataTable' class='datatable table-condensed table-striped table-bordered'><tr><th>#<th>Item<th>PLU<th style='white-space:nowrap'>"
            + startDate.ToString("%d-MMM-yy") + "<th>Purchase<th>Return<th>Damage<th title='Transfer in'>Tran.In<th title='Transfer Out'>Tran.Out<th>Sale<th>Stock<th>Cost<th>MRP<th title='Manual Stock' style='white-space:nowrap'>" + endDate.ToString("%d-MMM-yy");

        List<BillingLib.Item> itemList = BillingLib.Item.GetAll(0, BrandID);
        int ctr = 0;
        double totalCost = 0, totalMRP = 0;
        StringBuilder str = new StringBuilder();
        foreach (BillingLib.Item i in itemList.OrderBy(m => m.Name))
        {
            ctr++;
            BillingLib.Stock s = listStock.FirstOrDefault(m => m.ItemID == i.ID);
            if (s == null)
                str.Append("<tr><td>" + ctr + "<td style='text-align:left;'><a href='#' onclick='return GetStock(" + i.ID + "," + i.PLU + ",\"" + i.Name + "\",0,0,\"" + i.WeightTypeString + "\");' title='ID=" + i.ID + "'>" + i.Name + "</a><td>" + i.PLU + "<td><td><td><td><td><td><td><td><td><td><td></tr>");
            else
            {
                str.Append("<tr><td>" + ctr + "<td  style='text-align:left;'><a href='#' onclick='return GetStock(" + i.ID + "," + i.PLU + ",\"" + i.Name + "\"," + s.OpeningStock + "," + s.ClosingStock + ",\"" + i.WeightTypeString + "\");' title='ID=" + i.ID + "'>"
                + (i.Name != "" ? i.Name : "--") + "</a><td>"
                + i.PLU + "<td>"
                + (s.OpeningStock != 0 ? s.OpeningStock.ToString("0.0") : "") + "<td>"
                + "<a target='_blabk' href='/Report.aspx?reporttype=3&filterdatefrom=" + startDate.ToString("dd-MMM-yyyy") + "&filterdateto=" + endDate.ToString("dd-MMM-yyyy") + "&itemid=" + s.ItemID + "'>" + (s.Purchase != 0 ? s.Purchase.ToString("0.0") : "") + "</a><td>"
                + "<a target='_blank' href='/Purchase.aspx?reporttype=1&filterdatefrom=" + startDate.ToString("dd-MMM-yyyy") + "&filterdateto=" + endDate.ToString("dd-MMM-yyyy") + "&itemid=" + s.ItemID + "'>" + (s.Return != 0 ? s.Return.ToString("0.0") : "") + "</a><td>"
                + "<a target='_blank' href='/Purchase.aspx?reporttype=2&filterdatefrom=" + startDate.ToString("dd-MMM-yyyy") + "&filterdateto=" + endDate.ToString("dd-MMM-yyyy") + "&itemid=" + s.ItemID + "'>" + (s.Damage != 0 ? s.Damage.ToString("0.0") : "") + "</a><td>"
                + "<a target='_blank' href='/Purchase.aspx?reporttype=3&filterdatefrom=" + startDate.ToString("dd-MMM-yyyy") + "&filterdateto=" + endDate.ToString("dd-MMM-yyyy") + "&itemid=" + s.ItemID + "'>" + (s.TransferIn != 0 ? s.TransferIn.ToString("0.0") : "") + "</a><td>"
                + "<a target='_blank' href='/Purchase.aspx?reporttype=4&filterdatefrom=" + startDate.ToString("dd-MMM-yyyy") + "&filterdateto=" + endDate.ToString("dd-MMM-yyyy") + "&itemid=" + s.ItemID + "'>" + (s.TransferOut != 0 ? s.TransferOut.ToString("0.0") : "") + "</a><td>"
                + "<a target='_blank' href='/Report.aspx?reporttype=2&filterdatefrom=" + startDate.ToString("dd-MMM-yyyy") + "&filterdateto=" + endDate.ToString("dd-MMM-yyyy") + "&itemid=" + s.ItemID + "'>" + (s.Sale != 0 ? s.Sale.ToString("0.0") : "") + "</a>"
                + "<td " + (s.StockCount < 0 ? "style='background-color:#FBB7A8;'>" : ">") + s.StockCount.ToString("0.000")
                + "<td>" + (s.StockCount > 0 ? (i.Cost * s.StockCount).ToString("0.0") : "")
                + "<td>" + (s.StockCount > 0 ? (i.MRP * s.StockCount).ToString("0.0") : "")
                + "<td id='tdQty" + s.ItemID + "'>" + (s.ClosingStock != 0 ? s.ClosingStock.ToString() : "")
                + "</tr>");
                totalCost += (s.StockCount > 0 ? (i.Cost * s.StockCount) : 0);
                totalMRP += (s.StockCount > 0 ? (i.MRP * s.StockCount) : 0);
            }
        }
        header += "<tr><th><th>TOTAL<th><th><th><th><th><th><th><th><th><th>" + totalCost.ToString("0") + "<th>" + totalMRP.ToString("0") + "<th>";
        header += str.ToString() + "</tablle>";
        return header;
    }

    private void UpdateSingleField(string table, int primaryKey, string field, string fieldValue)
    {
        AppendError = false;
        DatabaseCE db = new DatabaseCE();
        try
        {
            db.RunQuery("update " + table + " set " + field + "='" + fieldValue + "' where id=" + primaryKey);
            str.Append("Saved");
        }
        catch
        {
            str.Append("Error");
        }
        finally
        {
            db.Close();
        }
    }

    private void UpdateDeliveryBoy(int billID, string deliveryPerson)
    {
        AppendError = false;
        BillingLib.Billing bill = BillingLib.Billing.GetByID(billID);
        if (bill != null)
        {
            bill.DeliveryBy = deliveryPerson;
            if (deliveryPerson != "")
                bill.Delivery = true;
            else
                bill.Delivery = false;
            bill.Save();
            str.Append(bill.DeliveryBy);
        }
        str.Append("");
    }

    private void ChangeVendor(int PurchaseID, int newVendorID)
    {
        BillingLib.Purchase P = BillingLib.Purchase.GetByID(PurchaseID);
        if (P != null)
        {
            P.VendorID = newVendorID;
            P.Save();
            DatabaseCE db = new DatabaseCE();
            try
            {
                db.RunQuery("Update Inventory set vendorid=" + newVendorID + " where purchaseid=" + PurchaseID + " and recordtype=" + (int)RecordType.Purchase + "");
            }
            finally
            {
                db.Close();
            }
        }
    }

    private void GetProject(int ID)
    {
        AppendError = false;
        BillingLib.Project P = BillingLib.Project.Get(ID);
        if (P != null)
        {
            WriteResponse(this, new JavaScriptSerializer().Serialize(P), encode, true);
        }
    }

    private void SaveProject()
    {
        NameValueCollection nvc = Request.Form;
        int ID = Cmn.ToInt(nvc["ID"]);
        Project P = BillingLib.Project.Get(ID);
        if (P == null)
            P = new BillingLib.Project();
        P.ProjectName = nvc["ProjectName"];
        P.Address = nvc["Address"];
        P.Area = nvc["Area"];
        P.City = nvc["City"];
        P.State = nvc["State"];
        P.Pin = Cmn.ToInt(nvc["PIN"]);
        P.Save();
    }

    private void GetItemByPLUandCreate(string PLUtoLink, int ExistingItemID)
    {
        AppendError = false;
        BillingLib.Item item = BillingLib.Item.GetByPLU(PLUtoLink);
        if (item == null) // create a new item
        {
            item = BillingLib.Item.GetByID(ExistingItemID);
            item.ID = 0;
            item.PLU = PLUtoLink;
            item.Save();
        }

        BillingLib.Item.Link(ExistingItemID, item.ID);
        str.Append(item.ID.ToString());
    }

    private void AssignDelieveryMan(int OrderID, int deliveryPersonID, string deliveryPersonName)
    {
        //BillingLib.Order O = BillingLib.Order.GetByID(OrderID);
        //if (O != null)
        //{
        //    O.DeliveryPersonID = deliveryPersonID;
        //    if (deliveryPersonID != 0)
        //        O.DeliveryPersonName = deliveryPersonName;
        //    else
        //        O.DeliveryPersonName = "";
        //    O.Save();
        //}
        BillingLib.Billing bill = BillingLib.Billing.GetByID(OrderID, 0, 0, 0, true);
        if (bill != null)
        {
            bill.DeliveryBy = deliveryPersonName;
            bill.Save();
        }
    }

    private void GetDeliveryManList(int StoreID)
    {
        AppendError = false;
        List<BillingLib.Employee> empList = BillingLib.Employee.GetDelievetyMenbyStore(StoreID);
        WriteResponse(this, new JavaScriptSerializer().Serialize(empList), encode);
    }

    private void GetSocietyList()
    {
        AppendError = false;
        List<BillingLib.Project> list = BillingLib.Project.GetAll();
        WriteResponse(this, new JavaScriptSerializer().Serialize(list), encode);
    }

    private void AutolinkApartmentWithCustomer(int ProjectID, string MobileNo, string HouseNo, int CustomerID)
    {
        AppendError = false;
        List<BillingLib.Apartment> AL = BillingLib.Apartment.GetByProjectID(ProjectID);
        BillingLib.Apartment A = AL.FirstOrDefault(m => m.FloorNo == Cmn.ToInt(MobileNo) || m.FloorNo == Cmn.ToInt(HouseNo));
        if (A != null)
        {
            BillingLib.Customer C = BillingLib.Customer.GetByID(CustomerID);
            C.ProjectID = ProjectID;
            C.ApartmentID = A.ID;
            C.Save();
            str.Append("Linked with Aprtment No - " + A.FloorNo);
        }
        else
            str.Append("Not Found");
    }

    private void UpdateLastPurchase(string mobile, int CustID)
    {
        DateTime LastBillDate = BillingLib.Billing.GetByMobile(mobile).Max(m => m.BillDate);
        BillingLib.Customer C = BillingLib.Customer.GetByID(CustID);
        if (C != null)
        {
            C.LastSaleDate = LastBillDate;
            C.Save();
        }
    }

    private void LinkCustomerWithApartment(int CistomerID, int ApartmentID, int ProjectID)
    {
        AppendError = false;
        BillingLib.Customer C = BillingLib.Customer.GetByID(CistomerID);
        if (C != null)
        {
            C.ProjectID = ProjectID;
            C.ApartmentID = ApartmentID;
            C.Save();
        }
    }

    private void GetProjectApartments(int ProjectID)
    {
        AppendError = false;
        List<BillingLib.Apartment> AL = BillingLib.Apartment.GetByProjectID(ProjectID);
        if (AL != null)
        {
            WriteResponse(this, new JavaScriptSerializer().Serialize(AL), encode, true);
        }
    }

    //private void UpdateManualClosingStock(int ID)
    //{
    //    AppendError = false;
    //    BillingLib.StockTakingDetail std = BillingLib.StockTakingDetail.GetByID(ID);
    //    if (std != null)
    //    {
    //        BillingLib.Item i = SessionState.Company.ItemList.FirstOrDefault(m => m.ID == std.ItemID);
    //        var obj = new { StoreTakingDetail = std, Item = i };
    //        WriteResponse(this, new JavaScriptSerializer().Serialize(obj), encode, true);
    //    }
    //}

    //private void ShowStockTakingDetailList(int StockID)
    //{
    //    AppendError = false;
    //    List<BillingLib.StockTakingDetail> list = BillingLib.StockTakingDetail.GetAll();
    //    if (list.Count == 0)
    //        return;
    //    str.Append("<table  class='table table-condensed table-striped table-bordered'><th style='width:15px;'>#<th>Item ID<th>Quantity<th>Date/Time");
    //    int ctr = 0;
    //    foreach (BillingLib.StockTakingDetail std in list)
    //    {
    //        str.Append("<tr><td>" + ++ctr + "<td title=" + std.ID + "><a href='#' onclick='EditStockDetail(" + std.ID + ")'>" + SessionState.Company.ItemList.FirstOrDefault(m => m.ID == std.ItemID).Name + "</a><td>" + std.Quantity + "<td>" + std.AddDate.ToString("dd-MMM-yy HH:mm"));
    //    }
    //    str.Append("</table>");
    //}

    private BillingLib.Stock AddManualOpeningStock(int ItemID, DateTime StockMonth, double Qty)
    {
        BillingLib.Stock STD = BillingLib.Stock.Get(ItemID, StockMonth, SessionState.StoreID);
        if (STD == null)
            STD = new BillingLib.Stock();

        STD.OpeningStock = Qty - STD.Purchase + STD.Return + STD.Damage - STD.TransferIn + STD.TransferOut + STD.Sale;
        STD.ItemID = ItemID;
        STD.StockMonth = StockMonth;
        STD.StoreID = SessionState.StoreID;
        try
        {
            STD.Save();
        }
        catch
        {
        }
        return STD;
    }

    private BillingLib.Stock AddManualClosingStock(int ItemID, DateTime StockMonth, double Qty, Boolean IsOpening)
    {
        BillingLib.Stock STD = BillingLib.Stock.Get(ItemID, StockMonth, SessionState.StoreID);
        if (STD == null)
            STD = new BillingLib.Stock();

        STD.ItemID = ItemID;
        if (IsOpening)
            STD.OpeningStock = Qty;
        else
            STD.ClosingStock = Qty;

        STD.StockMonth = StockMonth;
        STD.StoreID = SessionState.StoreID;
        STD.Save();
        return STD;
    }

    private void DeletePurchasePaymentLink(int PurchasePaymentLinkID)
    {
        AppendError = false;
        PurchasePaymentLink pl = PurchasePaymentLink.GetByID(PurchasePaymentLinkID);
        if (pl != null)
            pl.Delete();
    }

    void UpdatePurchasePaymentLink(int PurchasePaymentLinkID, int PaymentID, int PurchaseID, double Amount)
    {
        //double PayAmount = BillingLib.PurchasePaymentLink.GetByPayment(PaymentID).Sum(m=>m.Amount);
        PurchasePaymentLink pl = PurchasePaymentLink.GetByID(PurchasePaymentLinkID);
        if (pl == null)
        {
            BillingLib.Purchase purch = BillingLib.Purchase.GetByID(PurchaseID);
            if (purch != null)
            {
                BillingLib.PurchasePayment payment = BillingLib.PurchasePayment.GetByID(PaymentID);
                if (payment != null)
                {
                    pl = new PurchasePaymentLink();
                    pl.PurchaseID = PurchaseID;
                    pl.PaymentID = PaymentID;
                    pl.VendorID = payment.VendorID;
                    pl.PaymentDate = payment.PaymentDate;
                    Amount = Math.Round(purch.Amount, 0);
                }
            }
        }
        pl.Amount = Amount;
        pl.Save();
    }

    void GetPurchasePaymentLink(int PaymentID)
    {
        List<PurchasePaymentLink> list = PurchasePaymentLink.GetByPayment(PaymentID);

        str.Append("<table class='table table-condensed'><tr><th>Purch Date<th>Invoice No<th>RC No<th>Amount Paid<td><td>");
        double Total = 0;
        foreach (PurchasePaymentLink pl in list)
        {
            BillingLib.Purchase purch = BillingLib.Purchase.GetByID(pl.PurchaseID);
            str.Append("<tr><td>" + purch.PurchaseDate.ToString("%d-MMM-yy") + "<td>"
            + purch.InvoiceNo + "<td>"
            + purch.RCNo + "<td>"
            + pl.Amount.ToString("0") + "<td><a href='#' onclick='UpdatePurchaseLinkAmount(" + pl.ID + ")'>Update</a><td><a href='#' onclick='DeletePurchasePaymentLink(" + pl.ID + ")'>Delete</a>");
            Total += pl.Amount;
        }
        str.Append("<tr><th colspan='3'>Total<th>" + Total.ToString("0") + "<td><td>");
        str.Append("</table>");
    }

    private void UpdateBill()
    {
        int ID = Cmn.ToInt(GetFormString("ID"));
        double Quantity = Cmn.ToDbl(GetFormString("Quantity"));
        double Rate = Cmn.ToDbl(GetFormString("Rate"));
        double Cost = Cmn.ToDbl(GetFormString("Cost"));
        AppendError = false;

        BillingLib.BillingItem bi = BillingLib.BillingItem.GetByBillingItemID(ID);
        if (bi != null)
        {
            BillingLib.Billing Bill = BillingLib.Billing.GetByID(bi.BillingID);
            if (Bill != null)
            {
                BillingLib.BillingItem thisItem = Bill.ItemList.FirstOrDefault(m => m.ID == ID);
                if (thisItem != null)
                {
                    thisItem.Quantity = Quantity;
                    thisItem.MRP = Rate;
                    thisItem.Cost = Cost;
                    if (Bill.PaidCash != 0)
                        Bill.PaidCash = Bill.TotalAmount;
                    else
                        Bill.PaidCard = Bill.TotalAmount;

                    Bill.Save();
                    WriteResponse(this, new JavaScriptSerializer().Serialize(Bill.ItemList.FirstOrDefault(m => m.ID == ID)), encode, true);
                    return;
                }
            }
        }
        WriteResponse(this, "Error object found", encode, true);
    }

    private void SaveMonthlyTotal(int StoreID)
    {
        NameValueCollection nvc = Request.Form;
        AmountType AmountType = (AmountType)Cmn.ToInt(nvc["AmountType"]);
        DateTime dt = Cmn.ToDate(nvc["TotalDateString"]);
        dt = new DateTime(dt.Year, dt.Month, 1);

        for (int i = 0; i < 12; i++)
        {
            MonthlyTotal MT = BillingLib.MonthlyTotal.GetByDateTypeAndStore(dt, AmountType, StoreID);
            if (MT == null)
                MT = new MonthlyTotal();
            MT.AmountType = (AmountType)AmountType;
            //MT.Amount = Cmn.ToDbl(nvc["txtAmount"]);
            MT.AmountManual = Cmn.ToDbl(nvc["AmountManual"]);
            MT.StoreID = StoreID;
            MT.TotalDate = dt;
            MT.Save();

            if (nvc["chkUpdateAllMonths"] == "0")
                break;
            dt = dt.AddMonths(1);
            if (dt.Year > MT.TotalDate.Year && dt.Month > 3)
                break;
        }

        switch (AmountType)
        {
            case AmountType.Sale:
                BillingLib.Billing.UpdateMonthlyTotal(dt, StoreID);
                break;
            case AmountType.Purchase:
                BillingLib.Purchase.UpdateMonthlyTotal(dt, StoreID);
                break;
            case AmountType.DailyExpense:
                BillingLib.ExpenseLog.UpdateMonthlyTotal(dt, StoreID);
                break;
            case AmountType.Payment:
                BillingLib.PurchasePayment.UpdateMonthlyTotal(dt, StoreID);
                break;
        }
    }

    private void GetMonthlyTotal(DateTime TotalDate, AmountType amountType, int StoreID)
    {
        AppendError = false;
        MonthlyTotal MT = BillingLib.MonthlyTotal.GetByDateTypeAndStore(TotalDate, amountType, StoreID);
        WriteResponse(this, new JavaScriptSerializer().Serialize(MT), encode, true);
    }

    private void GetSalary(int EmpID, DateTime SalaryDate)
    {
        AppendError = false;
        Salary S = Salary.GetByEmpidAndSalDate(EmpID, SalaryDate);
        if (S != null)
            WriteResponse(this, new JavaScriptSerializer().Serialize(S), encode, true);
    }

    private void SaveSalary()
    {
        NameValueCollection nvc = Request.Form;
        DateTime dtStart = Cmn.ToDate(nvc["txtSalaryDate"]);
        DateTime dtEnd = dtStart.Month <= 3 ? new DateTime(dtStart.Year, 3, 1) : new DateTime(dtStart.Year + 1, 3, 1);
        DateTime dt = new DateTime(dtStart.Year, dtStart.Month, 1);

        for (int i = 0; i < 12; i++)
        {
            int salaryID = Cmn.ToInt(nvc["txtSalaryID"]);
            int EmplpyeeID = Cmn.ToInt(nvc["txtEnployeeID"]);
            int StoreID = Cmn.ToInt(nvc["ctl00$ContentPlaceHolder1$ddStoreSelect"]);

            Salary S = null;
            //if (salaryID != 0)
            //    S = Salary.GetByID(salaryID);
            //else
            //    S = Salary.GetByEmpidAndSalDate(EmplpyeeID, dt);

            if (nvc["chkUpdateRemaining"] == null)
            {
                if (salaryID != 0)
                    S = Salary.GetByID(salaryID);
            }
            else
                S = Salary.GetByEmpidAndSalDate(EmplpyeeID, dt);
            if (S == null)
                S = new Salary();
            S.EmployeeID = Cmn.ToInt(nvc["txtEnployeeID"]);
            S.EmployeeName = nvc["txtEnployeeName"];
            S.SalaryDate = dt;
            S.Basic = Cmn.ToDbl(nvc["txtBasic"]);
            S.HRA = Cmn.ToDbl(nvc["txtHRA"]);
            S.TA = Cmn.ToDbl(nvc["txtTA"]);
            S.Medical = Cmn.ToDbl(nvc["txtMedical"]);
            S.Advance = Cmn.ToDbl(nvc["txtAdvance"]);
            S.TDS = Cmn.ToDbl(nvc["txtTDS"]);
            S.Loan = Cmn.ToDbl(nvc["txtLoan"]);
            S.StoreID = StoreID;
            S.Save();

            if (nvc["chkUpdateRemaining"] == null)
                break;
            dt = dt.AddMonths(1);

            if (dt > dtEnd)
                break;
        }

    }

    void GetCustomer(int ID)
    {
        AppendError = false;
        BillingLib.Customer i = BillingLib.Customer.GetByID(ID);
        WriteResponse(this, new JavaScriptSerializer().Serialize(i), encode, true);
    }

    private void SaveCustomer()
    {
        NameValueCollection nvc = Request.Form;
        BillingLib.Customer C = BillingLib.Customer.GetByID(Cmn.ToInt(nvc["txtID"]));
        if (C == null)
            C = new BillingLib.Customer();
        C.Name = nvc["txtName"];
        C.Mobile = nvc["txtMobile"];
        C.HouseNumber = nvc["txtHouseNo"];
        C.Address = nvc["txtAddress"];
        C.Area = nvc["txtArea"];
        C.City = nvc["txtCity"];
        C.PIN = Cmn.ToInt(nvc["txtPin"]);
        C.Birthday = Cmn.ToDate(nvc["txtBirthday"]);
        C.Anniverary = Cmn.ToDate(nvc["txtAnniverary"]);
        C.ProjectID = Cmn.ToInt(nvc["txtProjectID"]);
        C.ApartmentID = Cmn.ToInt(nvc["ddProjectApartment"]);
        C.LastAdvertizedDate = Cmn.ToDate(nvc["txtLatAdvertized"]);
        C.StoreID = Cmn.ToInt(nvc["ddCustomerStore"]);
        C.Save();
    }

    private void UpdateItemPrice(int itemID, int newMRP)
    {
        BillingLib.Item item = BillingLib.Item.GetByID(itemID);
        if (item != null)
        {
            item.MRP = newMRP;
            item.Save();
            SessionState.Company.UpdateItem(item);
        }
    }

    private void DeleteAttendance(int ID)
    {
        AppendError = false;
        BillingLib.EmployeeAttendance EmpAtt = BillingLib.EmployeeAttendance.GetByID(ID);
        if (EmpAtt != null)
        {
            EmpAtt.Delete();
        }
    }

    private void WriteCSVFile()
    {
        StringBuilder str = new StringBuilder();
        Dictionary<string, string> DealerDetails = new Dictionary<string, string>();
        string FileName = Server.MapPath(@"~\Sony.csv");
        if (File.Exists(FileName))
        {
            try
            {
                string[] Lines = File.ReadAllLines(FileName);
                foreach (string s in Lines)
                {
                    string[] F = s.Split(',');
                    DealerDetails.Add(F[0] + F[1], s);
                }
            }
            catch (Exception ex)
            {
                string s = ex.ToString();
            }
        }

        string[] DealerLines = GetFormString("data").Replace("\r\n", "").Split('~'); //a.Split('~');
        foreach (String s in DealerLines)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(s))
                    continue;
                string[] F = s.Split('^');
                string key = F[0] + F[1];

                if (!DealerDetails.ContainsKey(key))
                    DealerDetails.Add(key, s.Replace('^', ','));
                else
                {
                    string value = "";
                    DealerDetails.TryGetValue(key, out value);
                    string cat = "";
                    if (value != "")
                    {
                        string[] valArr = value.Split(',');
                        cat = valArr[10] + "$" + F[10];
                        string result = string.Join("$", cat.Split('$').Distinct().ToArray()).ToString();
                        DealerDetails[key] = string.Join(",", valArr.Take(valArr.Length - 1)) + "," + result;
                    }
                }
            }
            catch
            {
            }
        }
        StringBuilder ctr = new StringBuilder();
        foreach (KeyValuePair<string, string> kvp in DealerDetails)
        {
            ctr.Append(kvp.Value + Environment.NewLine);
        }
        File.WriteAllText(FileName, ctr.ToString());
    }

    private void SaveOrderItem()
    {
        AppendError = false;
        string data = GetFormString("data");
        OrderItem list = new JavaScriptSerializer().Deserialize<OrderItem>(data);
        BillingLib.OrderItem oi = BillingLib.OrderItem.GetByID(list.ID);
        if (oi == null)
            oi = new BillingLib.OrderItem();
        oi.ID = list.ID;
        oi.OrderID = list.OrderID;
        oi.ItemID = list.ItemID;
        oi.Quantity = list.Quantity;
        BillingLib.Item i = SessionState.Company.ItemList.FirstOrDefault(m => m.ID == oi.ItemID);
        if (i != null)
        {
            oi.ItemName = i.Name;
            oi.Price = (i.MRP * oi.Quantity);
        }
        oi.Save();
        WriteResponse(this, new JavaScriptSerializer().Serialize(oi), encode);
    }

    private void ChangeOrderStatus(int OrderID, int Status, string prevNext)
    {
        AppendError = false;
        BillingLib.Order O = BillingLib.Order.GetByID(OrderID);
        try
        {
            if (prevNext == "")
            {
                O.delStatus = (DeliveryStatus)Status;
            }
            else
            {
                if (prevNext == "prev")
                {
                    int min = Enum.GetValues(typeof(DeliveryStatus)).Cast<int>().Min();
                    int status = Status - 1;
                    if (status >= min)
                        O.delStatus = (DeliveryStatus)(status);
                }
                else
                {
                    int max = Enum.GetValues(typeof(DeliveryStatus)).Cast<int>().Max();
                    int status = Status + 1;
                    if (status <= max)
                        O.delStatus = (DeliveryStatus)(status);
                }
            }
            O.Save();
            str.Append(O.delStatus + "-" + (int)(DeliveryStatus)O.delStatus);
        }
        catch (Exception ex)
        {
            str.Append(ex.Message.ToString());
        }
    }

    private void TraceOrder(int BillID, string MobileNumber)
    {
        //    AppendError = false;
        //    BillingLib.Order ol = BillingLib.Order.GetByID(OrderID, MobileNumber);
        //    if (ol == null)
        //    {
        //        ol = new BillingLib.Order() { Message = "Order not found" };
        //    }

        //    WriteResponse(this, new JavaScriptSerializer().Serialize(ol), encode);
        AppendError = false;
        BillingLib.Billing ol = BillingLib.Billing.GetByID(BillID, 0, 0, 0, true);
        //if (ol == null)
        //{
        //    ol = new BillingLib.Billing() { Message = "Order not found" };
        //}
        WriteResponse(this, new JavaScriptSerializer().Serialize(ol), encode);
    }

    private void AssignOrder(int OrderID, int StoreID)
    {
        AppendError = false;
        BillingLib.Order O = BillingLib.Order.GetByID(OrderID);
        try
        {
            O.StoreID = StoreID;
            O.delStatus = DeliveryStatus.Assigned;
            O.Save();
        }
        catch (Exception ex)
        {
            str.Append(ex.Message.ToString());
        }
    }

    private void SaveOrder(int ID = 0, string Mobile = "", string Address = "", string CustomerName = "", string SocietyInfo = "", string deliveryDateTime = "",bool IsEditMode=false)
    {
        AppendError = false;
        
        if (IsEditMode)
        {
            NameValueCollection nvc = Request.Form;
            int OrderID = Cmn.ToInt(nvc["orderID"]);
            string MobileNo = nvc["mobile"];
            string Name = nvc["customerName"];
            string CustomerAddress = nvc["address"];
            int SocietyID = Cmn.ToInt(nvc["society"]);
            int ApartmentID = Cmn.ToInt(nvc["apartment"]);
            DateTime deliveryTime = Cmn.ToDate(nvc["deliverydateTime"]);
            int OrderItemID = Cmn.ToInt(nvc["orderItemID"]);
            int ItemID = Cmn.ToInt(nvc["itemID"]);
            int Quantity = Cmn.ToInt(nvc["quantity"]);

            string data = GetFormString("data");
            BillingLib.Billing b = BillingLib.Billing.GetByID(OrderID, 0, 0, 0, true);
            if (b == null)
            {
                b = new BillingLib.Billing();
                b.orderStatus = OrderStatus.Pending;
            }
            else
            {
                b.customer.Name = Name;
                b.customer.Mobile = MobileNo;
                b.customer.Address = CustomerAddress;
                b.customer.ProjectID = SocietyID;
                b.customer.ApartmentID = ApartmentID;
                b.DeliveryTime = deliveryTime;

                BillingLib.Item i = i = BillingLib.Item.GetByID(ItemID);
                if (OrderItemID == 0)
                {
                    i.Quantity = Quantity;
                    b.ItemList.Add(new BillingItem()
                    {
                        BillingID = b.ID,
                        ItemID = i.ID,
                        Name = i.Name,
                        Quantity = Quantity,
                        Cost = i.Cost,
                        MRP = i.MRP,
                        TaxRate = i.TaxRate,
                        TaxRateID = i.TaxRateID,
                        PLU = i.PLU,
                        SaleDate = b.DeliveryTime,
                    });
                }
                else
                {
                    BillingLib.BillingItem bi = b.ItemList.FirstOrDefault(m => m.ID == OrderItemID);
                    bi.BillingID = b.ID;
                    bi.ItemID = i.ID;
                    bi.Name = i.Name;
                    bi.Quantity = Quantity;
                    bi.Cost = Cmn.ToDbl(i.Cost);
                    bi.MRP = Cmn.ToDbl(i.MRP);
                    bi.TaxRate = Cmn.ToDbl(i.TaxRate);
                    bi.TaxRateID = i.TaxRateID;
                    bi.PLU = i.PLU;
                    bi.SaleDate = b.DeliveryTime;
                }
            }
            b.Save();
        }

        else  //Front end request
        {
            BillingLib.Billing b = BillingLib.Billing.GetByID(ID, 0, 0, 0, true);
            try
            {
                if (b == null)
                {
                    b = new Billing();
                    b.orderStatus = OrderStatus.Pending;
                }
                b.customer.Name = CustomerName;
                b.customer.Mobile = Mobile;
                b.customer.Address = Address;
                if (SocietyInfo != "")
                {
                    string[] societyInfo = SocietyInfo.Split('^');
                    b.customer.ProjectID = Cmn.ToInt(societyInfo[0]);
                    b.customer.ApartmentID = Cmn.ToInt(societyInfo[1]);
                }
                b.DeliveryTime = Cmn.ToDate(deliveryDateTime);
                //b.Save();
                
                string data = GetFormString("data");
                List<OrderItem> list = new List<OrderItem>();
                if (data != "")
                    list = new JavaScriptSerializer().Deserialize<List<OrderItem>>(data);
                foreach (BillingLib.OrderItem oi in list)
                {
                    BillingLib.Item i = i = Item.GetByID(oi.ItemID);
                    BillingLib.BillingItem bi = new BillingLib.BillingItem();
                    bi.BillingID = b.ID;
                    bi.ItemID = oi.ItemID;
                    bi.Name = i.Name;
                    bi.Cost = Cmn.ToDbl(i.Cost);
                    bi.MRP = Cmn.ToDbl(i.MRP);
                    bi.TaxRate = Cmn.ToDbl(i.TaxRate);
                    bi.TaxRateID = i.TaxRateID;
                    bi.PLU = i.PLU;
                    bi.Quantity = oi.Quantity;
                    bi.SaleDate = b.DeliveryTime;
                    //bi.Save();
                    b.ItemList.Add(bi);
                }
                b.Save();
                str.Append(b.ID);
            }
            catch
            {
                str.Append("error");
            }
        }

        //List<OrderItem> list = new List<OrderItem>();
        //if (data != "")
        //    list = new JavaScriptSerializer().Deserialize<List<OrderItem>>(data);
        //try
        //{
        //    BillingLib.Order O = BillingLib.Order.GetByID(ID);
        //    if (O == null)
        //        O = new BillingLib.Order();
        //    O.Name = CustomerName;
        //    O.Mobile = Mobile;
        //    O.Address = Address;
        //    if (SocietyInfo != "")
        //    {
        //        string[] societyInfo = SocietyInfo.Split('^');
        //        O.ProjectID = Cmn.ToInt(societyInfo[0]);
        //        O.ApartmentNo = societyInfo[1];
        //    }
        //    if (deliveryDateTime != "")
        //        O.DeliveryDateTime = Cmn.ToDate(deliveryDateTime);
        //    O.Save();
        //    foreach (OrderItem obj in list)
        //    {
        //        BillingLib.OrderItem oi = BillingLib.OrderItem.GetByID(obj.ID);
        //        if (oi == null)
        //            oi = new BillingLib.OrderItem();
        //        oi.ID = obj.ID;
        //        oi.OrderID = O.ID;
        //        oi.ItemID = obj.ItemID;
        //        oi.Quantity = obj.Quantity;
        //        BillingLib.Item i = SessionState.Company.ItemList.FirstOrDefault(m => m.ID == oi.ItemID);
        //        if (i != null)
        //        {
        //            oi.ItemName = i.Name;
        //            oi.Price = i.MRP;// * oi.Quantity);
        //        }
        //        oi.Save();
        //    }
        //    str.Append(O.ID);
        //}
        //catch
        //{
        //    str.Append("error");
        //}


    }

    private void GetItems(string type, string ItemName)
    {
        AppendError = false;
        //string PlaceHolder = "";
        //GetBase64String(Server.MapPath(@"~/images/images.jpg"));
        str.Append(Item.GetItemsHTML(SessionState.Company, type, ItemName));
    }

    private void GetMenueLinkedItems(int MenuID)
    {
        AppendError = false;

        List<BillingLib.Item> list = SessionState.Company.ItemList.OrderBy(m => m.Name).ToList();
        //List<BillingLib.ItemMenuLink> listMenu = BillingLib.ItemMenuLink.GetByMenuID(MenuID);
        List<BillingLib.ItemMenuLink> listMenu = BillingLib.ItemMenuLink.GetAll();
        //List<BillingLib.ItemStorePrice> listStorePrice = BillingLib.ItemStorePrice.GetAll(0, SessionState.StoreID);

        StringBuilder sb = new StringBuilder("<table  id='DataTable' class='table-striped table-hover table-bordered' style='width:100%'><tr><th>#<th>PLU<th>Name<th>Tax<th>Cost<th title='Weight'>Wt.<th>MRP<th>Brand<th>Menu<th>Stock");
        int ctr = 0;
        foreach (Item i in list)
        {
            if (listMenu.FirstOrDefault(m => m.ItemID == i.ID && m.MenuID == MenuID) == null)
                continue;
            ctr++;
            double stock = 0;

            string menuItems = "";
            foreach (BillingLib.ItemMenuLink m in listMenu.Where(m => m.ItemID == i.ID))
            {
                BillingLib.Menu menu = SessionState.Company.listMenu.FirstOrDefault(n => n.ID == m.MenuID);
                if (menu != null)
                {
                    menuItems += (menu.Parent != null ? menu.Parent.Name : "") + "-" + menu.Name + "<a href='#' onclick='DeleteMenuItemLink(" + m.ID + ")'> X</a><br/>";
                }
            }

            string thisStoreCost = "", thisStoreMRP = "";
            //BillingLib.ItemStorePrice ISP = listStorePrice.FirstOrDefault(m => m.ItemID == i.ID && m.StoreID == SessionState.StoreID);
            //if (ISP != null)
            //{
            //    //i.Cost = ISP.Rate;
            //    //i.MRP = ISP.MRP;
            //    thisStoreCost = ISP.Rate.ToString();
            //    thisStoreMRP = ISP.MRP.ToString();
            //}
            BillingLib.Brand Brand = SessionState.Company.BrandList.FirstOrDefault(m => m.ID == i.BrandID);

            sb.Append("<tr id='tr" + i.ID + "'><td>" + ctr
                + "<td title='" + i.ID + "'>" + i.PLU
                + "<td><a href='#' onclick='LinkItemWithMenu(" + i.ID + ",\"" + i.Name + "\")'>"
                + i.Name + "</a><td>"
                + (i.TaxRate == 0 ? "" : i.TaxRate.ToString())
                + "<td>" + i.Cost + (i.RateType != UnitType.none ? "/" + i.RateType : "") + "<br/>" + (thisStoreCost != "" ? thisStoreCost + (i.RateType != UnitType.none ? "/" + i.RateType : "") : "")
                + "<td>" + (i.Weight == 0 ? "" : i.Weight.ToString()) + (i.WeightType != UnitType.none ? i.WeightType.ToString() : "")
                + "<td>" + i.MRP + "<br/>" + thisStoreMRP
                + "<td>" + (Brand != null ? Brand.Name : "")
                + "<td> " + menuItems +
                "<td><a href='/Stock.aspx?id=" + i.ID + "' target='_blank'>" + stock);
        }
        sb.Append("</table>").ToString();
        str.Append(sb);
    }

    private void DeleteMenuItemLink(int ID)
    {
        AppendError = false;
        BillingLib.ItemMenuLink mIl = BillingLib.ItemMenuLink.GetByID(ID);
        if (mIl != null)
        {
            mIl.Delete();
            //SessionState.Company.ItemMenuLinkList.Remove(mIl);
        }
    }

    string ItemCSV()
    {
        StringBuilder str = new StringBuilder("#,PLU,Name,Tax,Cost,Cost Type,Weight,Weight Type,MRP,Brand" + Environment.NewLine);
        int ctr = 0;
        foreach (Item i in SessionState.Company.ItemList.OrderBy(m => m.Name))
        {
            BillingLib.Brand b = SessionState.Company.BrandList.FirstOrDefault(m => m.ID == i.BrandID);
            str.Append(++ctr + ",#" + i.PLU + " ," + i.Name + "," + i.TaxRate + "," + i.Cost + "," + i.RateType + "," + i.Weight + "," + i.WeightType + "," + i.MRP + "," + (b != null ? b.Name : "") + Environment.NewLine);
        }
        return str.ToString();
    }


    void GetInventoryTransferList(DateTime FilterDateFrom, DateTime FilterDateTo, int FromStoreID, int ToStoreID, int ItemID, RecordType ReportType)
    {
        List<BillingLib.Inventory> list = BillingLib.Inventory.Get(FilterDateFrom, FilterDateTo, FromStoreID, ToStoreID, ItemID, RecordType.Transfer);
        str.Append(BillingLib.Inventory.GetInventoryHTML(SessionState.Company, list, SessionState.StoreID, RecordType.Transfer, 0, null, ReportType));
    }

    void GetInventoryList(int VendorID, RecordType recordType, DateTime FilterDateFrom, DateTime FilterDateTo, int FromStoreID, int ItemID)
    {
        int StoreID = SessionState.StoreID;
        str.Append(BillingLib.Inventory.GetHTML(SessionState.Company, VendorID, StoreID, recordType, FilterDateFrom, FilterDateTo, "", 0, ItemID));
    }

    void GetInventoryListFull(DateTime FilterDateFrom, DateTime FilterDateTo, int ItemID)
    {
        List<BillingLib.Inventory> list = BillingLib.Inventory.Get(SessionState.StoreID, FilterDateFrom, FilterDateTo, ItemID);
        str.Append(BillingLib.Inventory.GetInventoryHTML(SessionState.Company, list, SessionState.StoreID, RecordType.None));
    }

    private void SavePayment()
    {
        NameValueCollection nvc = Request.Form;
        int id = Cmn.ToInt(nvc["txtID"]);
        BillingLib.PurchasePayment paym = BillingLib.PurchasePayment.GetByID(id);

        double AmountPaid = 0;
        if (paym == null)
            paym = new BillingLib.PurchasePayment();
        else
            AmountPaid = (paym.Cash + paym.Cheque);

        paym.PurchaseID = Cmn.ToInt(nvc["txtPurchaseID"]);
        paym.VendorID = Cmn.ToInt(nvc["txtVendorID"]);
        if (nvc["txtCash"] != null)
            paym.Cash = Cmn.ToDbl(nvc["txtCash"]);
        if (nvc["txtCheque"] != null)
            paym.Cheque = Cmn.ToDbl(nvc["txtCheque"]);
        if (nvc["txtChequeNo"] != null)
            paym.ChequeNo = nvc["txtChequeNo"];

        paym.BankID = Cmn.ToInt(nvc["ddBank"]);
        paym.PaymentDate = Cmn.ToDate(nvc["txtDate"]);

        if (nvc["ddPaymentStore"] != null)
            paym.StoreID = Cmn.ToInt(nvc["ddPaymentStore"]);
        else
            paym.StoreID = SessionState.StoreID;
        paym.LedgerNo = nvc["txtLedgerNo"];
        paym.IsDelete = nvc["chkDelete"] != null ? 1 : 0;
        paym.Save();
        PurchasePaymentLink.MarkDelete(paym.ID, paym.IsDelete);

        double totalAmt = paym.Cash + paym.Cheque;
        BillingLib.Vendor V = BillingLib.Vendor.GetByID(paym.VendorID);
        if (V != null)
        {
            V.AmountPaid = (id == 0 ? (V.AmountPaid + totalAmt) : ((V.AmountPaid - AmountPaid) + totalAmt));
            V.Save();

            if (paym.IsDelete == 1)
                V.UpdateAmountDue();
        }
    }

    private void GetPayment(int id)
    {
        BillingLib.PurchasePayment paym = BillingLib.PurchasePayment.GetByID(id);
        WriteResponse(this, new JavaScriptSerializer().Serialize(paym), encode, true);
    }

    private void SaveExpense()
    {
        NameValueCollection nvc = Request.Form;
        int id = Cmn.ToInt(nvc["txtID"]);
        BillingLib.ExpenseLog exp = BillingLib.ExpenseLog.GetByID(id);
        if (exp == null)
        {
            exp = new BillingLib.ExpenseLog();
        }
        exp.Name = nvc["txtName"];
        exp.TerminalID = Cmn.ToInt(nvc["ddTerminal"]);
        exp.Amount = Cmn.ToDbl(nvc["txtAmount"]);
        exp.Date = Cmn.ToDate(nvc["txtDate"]);
        exp.StoreID = SessionState.StoreID;
        exp.expenseType = (ExpenseCategory)Cmn.ToInt(nvc["ddExpenseType"]);
        exp.Save();
    }

    private void GetExpense(int id)
    {
        BillingLib.ExpenseLog exp = BillingLib.ExpenseLog.GetByID(id);
        WriteResponse(this, new JavaScriptSerializer().Serialize(exp), encode, true);
    }

    private void SaveAttendance2()
    {
        NameValueCollection nvc = Request.Form;
        int id = Cmn.ToInt(nvc["txtID"]);
        int empID = Cmn.ToInt(nvc["txtEmployeeID"]);
        BillingLib.EmployeeAttendance att = BillingLib.EmployeeAttendance.GetByID(id);
        if (att == null)
        {
            att = new BillingLib.EmployeeAttendance();
            BillingLib.Employee e = null;
            Global.EmployeeList.TryGetValue(empID, out e);
            if (e != null)
                att.Name = e.Name;
        }
        att.Date = Cmn.ToDate(nvc["txtDate"]);
        att.DateIn = Cmn.ToDate(nvc["txtDate"] + " " + nvc["txtIn"]);
        att.DateOut = Cmn.ToDate(nvc["txtDate"] + " " + nvc["txtOut"]);
        att.EmployeeID = Cmn.ToInt(nvc["txtEmployeeID"]);
        att.LeaveType = Cmn.ToInt(nvc["ddLeaveType"]);
        att.StoreID = SessionState.StoreID;
        att.Save();
    }

    private void GetAttendance(int id)
    {
        BillingLib.EmployeeAttendance att = BillingLib.EmployeeAttendance.GetByID(id);
        WriteResponse(this, new JavaScriptSerializer().Serialize(att), encode, true);
    }

    void SaveAttendance(int EmpID, Boolean In = true)
    {
        BillingLib.Employee Emp;
        if (Global.EmployeeList.TryGetValue(EmpID, out Emp))
        {
            EmployeeAttendance empAt = EmployeeAttendance.GetAll(DateTime.Today, DateTime.Today, EmpID, SessionState.StoreID).FirstOrDefault();
            if (empAt == null)
            {
                empAt = new BillingLib.EmployeeAttendance();
                empAt.Date = DateTime.Today;
                empAt.Name = Cmn.ProperCase(Emp.Name);
                empAt.EmployeeID = EmpID;
                empAt.StoreID = SessionState.StoreID;
            }

            if (In)
                empAt.DateIn = DateTime.Now;
            else
                empAt.DateOut = DateTime.Now;
            empAt.Save();
        }
    }

    private void RemoveStore(int userID, int storeID)
    {
        StoreAccess SA = StoreAccess.GetByUseridAndStoreid(userID, storeID);
        if (SA != null)
        {
            SA.Delete();
        }
        AppendError = false;
    }

    private void AssignStore(int userID, int storeID)
    {
        if (storeID != 0)
        {
            StoreAccess SA = new StoreAccess()
            {
                StoreID = storeID,
                UserID = userID
            };

            SA.Save();
        }
        AppendError = false;
    }

    private void AssignTerminal(int userID, int terminalID)
    {
        if (terminalID != 0)
        {
            TerminalAccess TA = TerminalAccess.GetByUseridAndTerminalid(userID, terminalID);
            if (TA == null)
                TA = new TerminalAccess();
            TA.TerminalID = terminalID;
            TA.UserID = userID;
            TA.Save();
        }
        AppendError = false;
    }

    private void SaveReturn(RecordType recordType)
    {
        NameValueCollection nvc = Request.Form;
        BillingLib.Inventory i = null;
        int ID = Cmn.ToInt(nvc["ID"]);
        int InventoryID = Cmn.ToInt(nvc["InventoryID"]);
        int VendorID = Cmn.ToInt(nvc["VendorID"]);
        int ToStore = Cmn.ToInt(nvc["ToStore"]);
        int ItemID = Cmn.ToInt(nvc["ItemID"]);

        if (ID == 0)
        {
            i = BillingLib.Inventory.GetByID(InventoryID);
            i.ID = 0;
            i.VendorID = VendorID;
            i.RecordType = recordType;

            if (recordType == RecordType.TransferOut)   //Transfert Out
            {
                i.StoreID = ToStore;
                i.FromStoreID = SessionState.StoreID;
                i.RecordType = RecordType.Transfer;

                BillingLib.Item item = SessionState.Company.ItemList.FirstOrDefault(m => m.ID == i.ItemID);
                BillingLib.Store s;
                if (item != null && Global.StoreList.TryGetValue(i.StoreID, out s))
                {
                    if (s.IsFranchise && item.CostFranchise != 0)
                        i.Cost = item.CostFranchise;
                }
            }
            //else if (recordType == RecordType.TransferIn)  //Transfer In
            //{
            //    i.StoreID = SessionState.StoreID;
            //    i.FromStoreID = FromToStore;
            //    i.RecordType = RecordType.Transfer;

            //    BillingLib.Item item = SessionState.Company.ItemList.FirstOrDefault(m => m.ID == i.ItemID);
            //    BillingLib.Store s;
            //    if (item != null && Global.StoreList.TryGetValue(i.StoreID, out s))
            //    {
            //        if (s.IsFranchise && item.CostFranchise != 0)
            //            i.Cost = item.CostFranchise;
            //    }
            //}
            else
                i.StoreID = SessionState.StoreID;     //StoreID except Transfer-In/Out case
        }
        else
        {
            i = BillingLib.Inventory.GetByID(ID);
        }

        i.Quantity = Cmn.ToDbl(nvc["Quantity"]);
        i.Date = Cmn.ToDate(nvc["TransactionDate"]);
        i.Cost = Cmn.ToDbl(nvc["Cost"]);
        i.MRP = Cmn.ToDbl(nvc["MRP"]);
        i.RcNo = nvc["RcNo"];

        var grossAmount = i.Quantity * i.Cost;
        var Amt = grossAmount - ((grossAmount * i.Discount) / 100);
        i.TotalAmount = Cmn.ToDbl(Amt);

        i.Save();
        AppendError = false;
        Purchase P = Purchase.GetByID(i.PurchaseID);
        var obj = new { id = i.ID, html = i.GetItemHTMLRow(SessionState.Company, SessionState.StoreID, P, RecordType.Return) };
        WriteResponse(this, new JavaScriptSerializer().Serialize(obj), encode, true);

    }

    private void GetItemJSON(string query)
    {
        AppendError = false;
        List<Item> list = new List<Item>();
        if (query.Length > 1)
        {
            query = query.ToLower();
            list = SessionState.Company.ItemList.Where(m => m.PLU.StartsWith(query)).Take(10).ToList();
            list.AddRange(SessionState.Company.ItemList.Where(m => m.NameLower.StartsWith(query) && !list.Contains(m)).ToList());
            list.AddRange(SessionState.Company.ItemList.Where(m => m.NameLower.Contains(query) && !list.Contains(m)).Take(10).ToList());
        }
        WriteResponse(this, new JavaScriptSerializer().Serialize(list.Select(m => new { id = m.ID, plu = m.PLU, name = m.Name })), encode, true);
    }
    private void GetProjectJSON(string query)
    {
        AppendError = false;
        List<Project> list = new List<Project>();
        if (query.Length > 1)
        {
            query = query.ToLower();
            list = SessionState.Company.listProject.Where(m => m.ProjectNameLower.StartsWith(query)).Take(10).ToList();
            list.AddRange(SessionState.Company.listProject.Where(m => m.ProjectNameLower.Contains(query) && !list.Contains(m)).Take(10).ToList());
        }
        WriteResponse(this, new JavaScriptSerializer().Serialize(list.Select(m => new { id = m.ID, name = m.ProjectName })), encode, true);
    }

    private void GetItemJSON2(string query)
    {
        AppendError = false;
        List<Item> list = new List<Item>();
        if (query.Length > 1)
        {
            query = query.ToLower();
            list = SessionState.Company.ItemList.Where(m => m.PLU.StartsWith(query)).Take(10).ToList();
            list.AddRange(SessionState.Company.ItemList.Where(m => m.NameLower.StartsWith(query) && !list.Contains(m)).ToList());
            list.AddRange(SessionState.Company.ItemList.Where(m => m.NameLower.Contains(query) && !list.Contains(m)).Take(10).ToList());
        }
        WriteResponse(this, new JavaScriptSerializer().Serialize(list.Select(m => new { id = m.ID, plu = m.PLU, name = m.Name, cost = m.Cost, mrp = m.MRP, quantity = m.Quantity, tax = m.TaxRate })), encode, true);
    }

    private void GetItemJSONSkipLinked(string query, int MenuID)
    {
        AppendError = false;
        List<ItemMenuLink> itemMenuLinkList = ItemMenuLink.GetByMenuID(MenuID);
        List<Item> list = new List<Item>();
        if (query.Length > 1)
        {
            query = query.ToLower();
            list = SessionState.Company.ItemList.Where(m => m.PLU.StartsWith(query)).Take(10).ToList();
            list.AddRange(SessionState.Company.ItemList.Where(m => m.NameLower.Contains(query) && !list.Contains(m)).Take(10).ToList());

            List<Item> itemToremovelist = new List<Item>();
            foreach (Item i in list)
            {
                ItemMenuLink iml = itemMenuLinkList.FirstOrDefault(m => m.ItemID == i.ID);
                if (iml != null)
                    itemToremovelist.Add(i);
            }
            foreach (Item i in itemToremovelist)
            {
                list.Remove(i);
            }
        }
        WriteResponse(this, new JavaScriptSerializer().Serialize(list.Select(m => new { id = m.ID, plu = m.PLU, name = m.Name })), encode, true);
    }

    void GetInventory(int ID)
    {
        BillingLib.Inventory i = BillingLib.Inventory.GetByID(ID);
        WriteResponse(this, new JavaScriptSerializer().Serialize(i), encode, true);
    }

    private void SavePurchase()
    {
        NameValueCollection nvc = Request.Form;
        BillingLib.Purchase p = BillingLib.Purchase.GetByID(Cmn.ToInt(nvc["hdIDPurchase"]));
        if (p == null)
        {
            p = new BillingLib.Purchase();
            p.StoreID = SessionState.StoreID;
        }

        p.PurchaseDate = Cmn.ToDate(nvc["txtDate"]);

        p.VendorID = Cmn.ToInt(nvc["hdVendorID"]);
        if (nvc.Get("chkIncludingTax") != null)
            p.ItemIncludesTax = 1;
        else
            p.ItemIncludesTax = 0;

        if (nvc.Get("chkForm38") != null)
            p.IsForm38 = 1;
        else
            p.IsForm38 = 0;

        if (nvc.Get("chkIsNonUpPurchase") != null)
            p.IsNonUP = 1;
        else
            p.IsNonUP = 0;

        if (nvc.Get("chkDelete") != null)
        {
            p.IsDelete = 1;
            BillingLib.Inventory.DeleteByPurchaseID(p.ID, true);
        }
        else
        {
            p.IsDelete = 0;
            BillingLib.Inventory.DeleteByPurchaseID(p.ID, false);
        }

        p.InvoiceNo = nvc["txtInvoiceNo"];
        p.RCNo = nvc["txtRCNo"];
        p.CST = Cmn.ToDbl(nvc["txtCST"]);
        p.Save();

        BillingLib.Inventory.UpdateInventoryData(p);
        AppendError = false;
        str.Append(p.ID);
    }

    private void SaveInventory()
    {
        NameValueCollection nvc = Request.Form;
        int SNo = Cmn.ToInt(nvc["txtSNo"]);
        BillingLib.Inventory i = BillingLib.Inventory.GetByID(Cmn.ToInt(nvc["hdID"]));
        if (i == null)
        {
            i = new BillingLib.Inventory();
        }

        i.ID = Cmn.ToInt(nvc["hdID"]);
        i.PurchaseID = Cmn.ToInt(nvc["hdPurchaseID"]);

        Purchase P = Purchase.GetByID(i.PurchaseID);
        i.ItemIncludesTax = P.ItemIncludesTax;
        i.RcNo = P.RCNo;
        i.Date = P.PurchaseDate;
        i.VendorID = P.VendorID;
        i.InvoiceNo = P.InvoiceNo;


        i.SNo = SNo == 0 ? BillingLib.Inventory.GetMaxSNo(i.PurchaseID) + 1 : SNo;

        i.ItemID = Cmn.ToInt(nvc["hidItemID"]);
        i.Quantity = Cmn.ToDbl(nvc["txtQuantity"]);
        i.Cost = Cmn.ToDbl(nvc["txtCost"]);
        i.MRP = Cmn.ToDbl(nvc["txtMRP"]);
        i.TotalAmount = Cmn.ToDbl(nvc["HiddenTotalAmount"]);
        i.Tax = Cmn.ToDbl(nvc["ddTax"]);
        i.Discount = Cmn.ToDbl(nvc["txtDiscount"]);
        i.Unit = Cmn.ToInt(nvc["txtUnit"]);
        i.UnitType = (UnitType)Cmn.ToInt(nvc["ddUnitType"]);
        i.CostUnitType = (UnitType)Cmn.ToInt(nvc["ddCostUnitType"]);
        i.Expiry = Cmn.ToDate(nvc["txtExpiry"]);
        i.PLU = nvc["txtPLU"];
        i.Name = nvc["hdItemName"];

        i.StoreID = SessionState.StoreID;
        i.Save();

        //P.UpdateTotal();
        BillingLib.Inventory.GetHTML(SessionState.Company, i.PurchaseID);//should be fixed used temporarly
        BillingLib.Vendor V = BillingLib.Vendor.GetByID(i.VendorID);
        V.UpdateAmountDue();

        AppendError = false;
        var obj = new { id = i.ID, html = i.GetItemHTMLRow(SessionState.Company, SessionState.StoreID, P, RecordType.None) };
        WriteResponse(this, new JavaScriptSerializer().Serialize(obj), encode, true);
    }

    private void UpdatePaymentMode(int billingID, double Cash, double Card)
    {
        AppendError = false;
        BillingLib.Billing b = BillingLib.Billing.GetByID(billingID);
        DatabaseCE db = new DatabaseCE();
        double billAmount = BillingItem.GetByBillingID(billingID, db).Sum(m => m.MRP * m.Quantity - m.DiscountAmount);
        if (b != null)
        {
            if (Cash + Card != Math.Round(billAmount))
            {
                str.Append("Please check, amounts are not matching with total!");
                return;
            }
            b.PaidCash = Cash;
            b.PaidCard = Card;
            b.Save();
            str.Append("ok");
        }
    }

    private void GetItem(string PLU)
    {
        BillingLib.Item item = BillingLib.Item.GetByPLU(PLU);
        AppendError = false;
        WriteResponse(this, new JavaScriptSerializer().Serialize(item), encode);
    }

    void GetItemForList(int ItemID)
    {
        AppendError = false;
        BillingLib.Item i = BillingLib.Item.GetByID(ItemID);
        WriteResponse(this, new JavaScriptSerializer().Serialize(i), encode);
    }

    void GetInventoryByItem(int ItemID, int VendorID = 0, RecordType recordType = RecordType.None)
    {
        AppendError = false;
        List<BillingLib.Inventory> iList = BillingLib.Inventory.GetInventory(VendorID, 0, RecordType.Purchase, DateTime.Today.AddYears(-1), DateTime.Today, ItemID, 0).OrderByDescending(m => m.Date).ToList();
        if (recordType == RecordType.TransferIn || recordType == RecordType.TransferOut)
        {
            foreach (BillingLib.Inventory i in iList)
            {
                BillingLib.Item item = SessionState.Company.ItemList.FirstOrDefault(m => m.ID == i.ItemID);
                BillingLib.Store s;
                if (item != null && Global.StoreList.TryGetValue(SessionState.StoreID, out s))
                {
                    if (s.IsFranchise && item.CostFranchise != 0)
                        i.Cost = item.CostFranchise;
                }
            }
        }
        WriteResponse(this, new JavaScriptSerializer().Serialize(iList.Take(20)), encode);
    }

    void GetSerchedItem(string term, RecordType recordtype = RecordType.Purchase, int vendorID = 0, int itemID = 0)
    {
        AppendError = false;
        term = term.ToLower();

        List<Item> list = new List<Item>();
        list = SessionState.Company.ItemList.Where(m => m.PLU.StartsWith(term)).Take(20).ToList();
        if (Cmn.ToInt(term) == 0)
        {
            list.AddRange(SessionState.Company.ItemList.Where(m => m.NameLower.StartsWith(term) && !list.Contains(m)).Take(10).ToList());
            list.AddRange(SessionState.Company.ItemList.Where(m => m.NameLower.Contains(term) && !list.Contains(m)).Take(10).ToList());
        }
        WriteResponse(this, new JavaScriptSerializer().Serialize(list), encode);
    }

    private void GetTerminalList(string StoreID)
    {
        AppendError = false;
        List<BillingLib.Terminal> list = new List<BillingLib.Terminal>();
        list = BillingLib.Terminal.GetByStoreID(SessionState.StoreID);
        WriteResponse(this, new JavaScriptSerializer().Serialize(list), encode);
    }

    private void GetStoreList()
    {
        AppendError = false;
        List<BillingLib.Store> list = BillingLib.Store.GetByCompanyID(SessionState.CompanyID);

        WriteResponse(this, new JavaScriptSerializer().Serialize(list), encode);
    }

    private void GetStoreAccess()
    {
        AppendError = false;
        List<BillingLib.Store> sa = BillingLib.StoreAccess.GetByUserID(SessionState.UserID);
        WriteResponse(this, new JavaScriptSerializer().Serialize(sa), encode);
    }

    private void GetBillDetail(int billID)
    {
        BillingLib.Billing bill = BillingLib.Billing.GetByID(billID);
        string billDescription = "<span style='background-color:#F1D198;padding:5px;'>Bill No: " + bill.BillNo + " Bill Date: " + bill.BillDate.ToString("%d-MMM-yy HH:mm") + " Customer Name: " + bill.customer.Name + "</span>";
        StringBuilder sb = new StringBuilder("<table class='table table-condensed table-striped table-bordered' style='background-color:white;'><tr><th>ID<th>Code<th>ItemName<th>Cost<th>Sale Rate<th>Quantity<th>Discount<th>Tax<th class='alnright'>Total");
        DatabaseCE db = new DatabaseCE();
        List<BillingLib.BillingItem> billingList = BillingLib.BillingItem.GetByBillingID(bill.ID, db);

        int ctr = 0;
        foreach (BillingLib.BillingItem bi in billingList)
        {
            double discountAmount = 0;
            discountAmount = ((bi.Quantity * bi.MRP) - bi.Amount);
            sb.Append("<tr><td title=" + bi.ID + ">" + ++ctr
            + "<td>" + bi.PLU
            + "<td>" + bi.Name
            + "<td><input type='text' value='" + bi.Cost + "' id='txtCost" + bi.ID + "' style='width:60px;' " + (SessionState.IsAdminRRA ? "" : "disabled='disabled'") + ">"
            + "<td><input type='text' value='" + bi.MRP + "' id='txtMRP" + bi.ID + "' style='width:60px;' " + (SessionState.IsAdminRRA ? "" : "disabled='disabled'") + ">"
            + "<td><input type='text' value='" + bi.Quantity + "' id='txtQuantity" + bi.ID + "' style='width:60px;' " + (SessionState.IsAdminRRA ? "" : "disabled='disabled'") + ">"
            + "<td>" + (bi.BillItemDiscount > 0 ? "(" + discountAmount.ToString("0") + ") " + bi.BillItemDiscount + "%" : "-")
            + "<td>" + bi.TaxRate
            + "<td id='tdAmount" + bi.ID + "' class='alnright'>" + bi.Amount);

            if (SessionState.IsAdminRRA)
                sb.Append("<td><a href='#' onclick='UpdateBill(" + bi.ID + ")' class='btn btn-success btn-sm'>Update</a>");
        }
        sb.Append("<tr><th class='alnright' colspan='8'>TOTAL<th class='alnright'>" + bill.TotalAmount.ToString("0"));
        sb.Append("</table>");

        List<BillingLib.Employee> EL = BillingLib.Employee.GetDelievetyMenbyStore(SessionState.StoreID);
        sb.Append("Delivery by <select  id='ddDeliverBy' onchange='UpdateDeliveryBoy(" + billID + ",this)'><option value=''>-None-</option>");
        foreach (BillingLib.Employee e in EL)
        {
            sb.Append("<option value='" + e.Name + "' " + (e.Name == bill.DeliveryBy ? "selected" : "") + ">" + e.Name + "</option>");
        }
        sb.Append("</select>");
        sb.Append("&nbsp;&nbsp;<input type='checkbox' value='" + bill.PaymentRecieved + "' " + (bill.PaymentRecieved == 0 ? "" : "checked") + " onchange='UpdateDeliveryPayment(" + billID + ",this)'>&nbsp;Cash recieved.");

        AppendError = false;
        str.Append((billDescription + sb).ToString());
    }

    private void GetBillingList(DateTime fromDate, DateTime toDate, string mobile, int terminalID, int billno, int futureorder = 0, int deliverytype = 0, string deliveryboy = "")
    {
        if (toDate == Cmn.MinDate)
            toDate = fromDate;

        AppendError = false;
        List<BillingLib.Billing> list;
        if (billno != 0)
            list = BillingLib.Billing.GetByBillNo(billno);
        else if (fromDate == Cmn.MinDate && toDate == Cmn.MinDate)
            list = BillingLib.Billing.GetByMobile(mobile).OrderByDescending(m => m.BillDate).ToList();
        else
            list = BillingLib.Billing.GetAll(false, fromDate, toDate, mobile, SessionState.CompanyID, SessionState.StoreID, terminalID, false, deliverytype, deliveryboy).OrderByDescending(m => m.ID).ToList();

        if (list.Count <= 0)
        {
            str.Append("No item found for this serch criterion!");
            return;
        }

        string header = "<table class='table table-condensed table-striped table-bordered' style='background:white;'><tr><th>SNo<th>BillNo<th>BillDate<th>Term.<th>Customer<th class='alnright'>Cash<th class='alnright'>Card<th class='alnright' style='width:50px;'>Discount";
        StringBuilder sb = new StringBuilder();
        double TotalCash = 0, TotalCard = 0, TotalDiscount = 0;
        int sNo = 0;
        foreach (BillingLib.Billing bill in list)
        {

            TotalCard += bill.PaidCard;
            TotalCash += bill.PaidCash;
            TotalDiscount += bill.TotalDiscount;

            string pb = "<td class='alnright' " + (bill.Delivery ? " style='background-color:" + (bill.PaymentRecieved == 0 ? "#FDFDA6" : "#D4F3D4") + "' title='Delivery Bill '" : "") + "'><a href='#' onclick='return ShowPaymentMode(";
            sb.Append("<tr id='tr" + bill.ID + "'><td title=" + bill.ID + ">" + (++sNo)
                + "<td title=" + bill.ID + "><a onclick='return GetBillDetail(" + bill.ID + ")' href='#'>"
                + bill.BillNo + "</a><td>"
                + bill.BillDate.ToString("%d-%M-yy HH:mm") + "<td>"
                + bill.TerminalID + "<td>"
                + (bill.customer.Name == "" ? "" : bill.customer.Name + ", ") + (bill.customer.Mobile == "" ? "" : bill.customer.Mobile)
                + (!string.IsNullOrWhiteSpace(bill.customer.HouseNumber) ? "<br/>" + bill.customer.HouseNumber + ", " + bill.customer.Address + ", " + bill.customer.Area + "," + bill.customer.City + "," + bill.customer.PIN : "")
                + (bill.DeliveryBy == "" ? "<span id='spanDeliveryBy" + bill.ID + "'></span>" : "<span style='background-color:" + (bill.PaymentRecieved == 0 ? "#FDFDA6" : "#D4F3D4") + ";padding:2px;' id='spanDeliveryBy" + bill.ID + "'>&nbsp;DeliveryBy:" + bill.DeliveryBy + "</span>")
                + pb + bill.ID + "," + (int)PaymentMode.Cash + "," + bill.PaidCash + "," + bill.PaidCard + ")'>" + (bill.PaidCash != 0 ? bill.PaidCash.ToString() : "") + "</a>"
                + pb + bill.ID + "," + (int)PaymentMode.Card + "," + bill.PaidCash + "," + bill.PaidCard + ")'>" + (bill.PaidCard != 0 ? bill.PaidCard.ToString() : "") + "</a><td class='alnright'>"
                + (bill.TotalDiscount != 0 ? bill.TotalDiscount.ToString("0") : ""));
        }
        header += "<tr><td colspan='4'><th class='alnright'>Total(Cash+Card): " + Cmn.toCurrency(TotalCash + TotalCard) + "<th class='alnright'>" + Cmn.toCurrency(TotalCash) + "<th class='alnright'>" + Cmn.toCurrency(TotalCard) + "<th class='alnright'>" + Cmn.toCurrency(TotalDiscount);
        str.Append(header + sb.ToString() + "</table>");
    }

    void WebData(WebDataAction action)
    {
        NameValueCollection nvc = Request.Form;
        switch (action)
        {
            case WebDataAction.GetBill:
                {
                    BillingLib.Billing bill = (BillingLib.Billing)new JavaScriptSerializer().Deserialize<BillingLib.Billing>(nvc["data"].ToString());
                    bill = BillingLib.Billing.GetByID(0, bill.BillNo, bill.store.ID, bill.TerminalID);
                    WriteResponse(this, new JavaScriptSerializer().Serialize(bill), encode);
                }
                break;
            case WebDataAction.SetBill:
                {
                    BillingLib.Billing bill = (BillingLib.Billing)new JavaScriptSerializer().Deserialize<BillingLib.Billing>(nvc["data"].ToString());
                    bill.BillDate = bill.BillDate.AddHours(5.5);

                    // get the exiting bill
                    BillingLib.Billing billTemp = BillingLib.Billing.GetByBillNo(bill.BillNo, bill.store.ID, bill.TerminalID);
                    if (billTemp == null || billTemp.BillDate.Date != bill.BillDate.Date)
                        bill.Save();
                    else
                        bill = billTemp;

                    WriteResponse(this, new JavaScriptSerializer().Serialize(bill), encode);
                }
                break;
            case WebDataAction.GetCustomer:
                BillingLib.Customer cust = (BillingLib.Customer)new JavaScriptSerializer().Deserialize<BillingLib.Customer>(nvc["data"].ToString());
                WriteResponse(this, new JavaScriptSerializer().Serialize(cust.Find()), encode);
                break;
            case WebDataAction.GetItems:
                WriteResponse(this, new JavaScriptSerializer().Serialize(BillingLib.Item.GetAll()), encode);
                break;
            case WebDataAction.GetItemStorePrice:
                WriteResponse(this, new JavaScriptSerializer().Serialize(ItemStorePrice.GetAll()), encode);
                break;
            case WebDataAction.GetTerminal:
                BillingLib.Terminal term = (BillingLib.Terminal)new JavaScriptSerializer().Deserialize<BillingLib.Terminal>(nvc["data"].ToString());
                WriteResponse(this, new JavaScriptSerializer().Serialize(BillingLib.Terminal.GetByID(term.ID)), encode);
                break;

            case WebDataAction.SetTerminal:
                BillingLib.Terminal termSet = (BillingLib.Terminal)new JavaScriptSerializer().Deserialize<BillingLib.Terminal>(nvc["data"].ToString());
                termSet.LastAccess = termSet.LastAccess.AddHours(5.5);
                WriteResponse(this, new JavaScriptSerializer().Serialize(termSet.Save(true)), encode);
                break;

            case WebDataAction.GetStore:
                BillingLib.Store store = (BillingLib.Store)new JavaScriptSerializer().Deserialize<BillingLib.Store>(nvc["data"].ToString());
                WriteResponse(this, new JavaScriptSerializer().Serialize(BillingLib.Store.GetByID(store.ID)), encode);
                break;

            case WebDataAction.GetCompany:
                BillingLib.Company comp = (BillingLib.Company)new JavaScriptSerializer().Deserialize<BillingLib.Company>(nvc["data"].ToString());
                WriteResponse(this, new JavaScriptSerializer().Serialize(BillingLib.Company.GetByID(comp.ID)), encode);
                break;

            case WebDataAction.GetEmployee:
                BillingLib.Employee emp = (BillingLib.Employee)new JavaScriptSerializer().Deserialize<BillingLib.Employee>(nvc["data"].ToString());
                WriteResponse(this, new JavaScriptSerializer().Serialize(BillingLib.Employee.GetByEmailandPassword(emp.Email, emp.Password)), encode);
                break;

            case WebDataAction.GetSociety:
                List<BillingLib.Project> lst = BillingLib.Project.GetAll();
                WriteResponse(this, new JavaScriptSerializer().Serialize(BillingLib.Project.GetAll()), encode);
                break;

            case WebDataAction.GetDeliveryMen:
                BillingLib.Store st = (BillingLib.Store)new JavaScriptSerializer().Deserialize<BillingLib.Store>(nvc["data"].ToString());
                if (st != null)
                {
                    List<BillingLib.Employee> el = BillingLib.Employee.GetDelievetyMenbyStore(st.ID);
                    WriteResponse(this, new JavaScriptSerializer().Serialize(el), encode);
                }
                break;
            case WebDataAction.GetItemMenueLink:
                {
                    WriteResponse(this, new JavaScriptSerializer().Serialize(BillingLib.ItemMenuLink.GetAll()), encode);
                }
                break;
            case WebDataAction.GetMenues:
                {
                    WriteResponse(this, new JavaScriptSerializer().Serialize(BillingLib.Menu.GetAll()), encode);
                }
                break;
        }
    }

    private void GetCustomerByMobile(string mobile)
    {
        AppendError = false;
        List<BillingLib.Customer> listCust = new BillingLib.Customer() { Mobile = mobile }.Find();
        if (listCust.Count > 0)
        {
            BillingLib.Customer c = listCust[0];
            var list = new
            {
                Name = c.Name,
                Mobile = c.Mobile,
                ProjectID = c.ProjectID,
                ApartmentID = c.ApartmentID,
                ApartmentNo = GetApartmentNo(c.ApartmentID),
                Address = c.Address
            };
            WriteResponse(this, new JavaScriptSerializer().Serialize(list), encode);
        }
    }

    private string GetApartmentNo(int ID)
    {
        BillingLib.Apartment A = BillingLib.Apartment.GetByID(ID);
        if (A != null)
            return A.FloorNo.ToString();
        else
            return "";
    }
    private void GetCustomer(string mobile)
    {
        AppendError = false;
        List<BillingLib.Customer> list = new BillingLib.Customer() { Mobile = mobile }.Find();
        StringBuilder sb = new StringBuilder("<table class='table table-condensed table-striped table-bordered' style='width:50%;background:white;'><tr><th>ID<th>Name<th>Mobile<th>Address<th>Area<th>City<th>PIN");
        if (list.Count > 0)
        {
            BillingLib.Customer c = list[0];
            string FromDate = "", ToDate = "";
            sb.Append("<tr onclick='GetBillingList(\"" + FromDate + "\",\"" + ToDate + "\"," + c.Mobile + ")'><td>" + c.ID + "<td><a href='#'>" + c.Name.ToString() + "</a><td>" + c.Mobile.ToString() + "<td>" + c.Address + "<td>" + c.Area + "<td>" + c.City + "<td>" + c.PIN + "");
            str.Append(sb.ToString());
        }
        else
            str.Append("No record found!");
    }

    void GET_CHILDREN(int ParentID, int MenuType)
    {
        List<BillingLib.Menu> list = new List<BillingLib.Menu>();
        var newList = new List<object>();
        switch (MenuType)
        {
            case 1:
                {
                    list.AddRange(BillingLib.Menu.GetByParentID(ParentID));
                }
                break;
        }
        list = list.OrderBy(m => m.Name).ToList();
        newList = list.Select(a => new
        {
            data = a.Name,
            id = a.ID,
            title = a.Name,
            attr = new { name = a.Name, pid = a.ParentID, id = a.ID, Sequence = 0, menutype = MenuType, title = "ID: " + a.ID },
            state = "closed",
        }).ToList<object>();

        Response.AddHeader("Content-Type", "application/json");
        //Response.Write(new JavaScriptSerializer().Serialize(newList));
        WriteResponse(this, new JavaScriptSerializer().Serialize(newList), encode);
    }

    string FileUpload(string FileID)
    {
        foreach (string s in Request.Files)
        {
            HttpPostedFile file = Request.Files[s];
            int fileSizeInBytes = file.ContentLength;

            string directory = @"C:\Billing\Item_Images\";
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            file.SaveAs((directory + FileID + ".jpg"));
        }
        return "done";
    }
}
