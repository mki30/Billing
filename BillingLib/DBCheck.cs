using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlServerCe;
using System.IO;

public class DBCheck
{
    //CheckDatabase
    //CheckTable
    public static void RunSQLFile(DatabaseCE db, string data)
    {
        try
        {
            string[] Commands = data.Split(new string[] { "GO\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            IDbCommand cmd = new SqlCeCommand();
            cmd.Connection = db.myconnection;
            foreach (string s in Commands)
            {
                cmd.CommandText = s;
                cmd.ExecuteNonQuery();
            }
        }
        catch
        {
        }
    }

    public static void UpdateDB(string Name)
    {
        if (!Directory.Exists(@"c:\Billing\"))
            Directory.CreateDirectory(@"c:\Billing\");

        string DBName = @"c:\Billing\" + Name + ".sdf";

        if (!File.Exists(DBName))
            DatabaseCE.CreateDB(DBName, @"Data Source=c:\\Billing\\" + Name + ".sdf;");

        DatabaseCE db = new DatabaseCE();   //Update PropertyMap Database Structute
        try
        {
            int ctr = 0;
            while (DBCheck.UpdateDBStructure(db, ++ctr)) ;
            db.RunQuery("ALTER TABLE  Store ALTER COLUMN Phone nvarchar(100);");
        }
        catch
        {

        }
        finally
        {
            db.Close();
        }
    }

    public static void CheckTable(DatabaseCE db, string TableName, Dictionary<string, string> Fields, string[] PrimaryKeys)
    {
        //fields to be added to all table
        try
        {
            Fields.Add("LUDate", "[datetime]");
            Fields.Add("LUBy", "[nvarchar](50)");
        }
        catch
        {
        }

        //create table
        string SQL = "CREATE TABLE [" + TableName + "] (";
        string PK = " PRIMARY KEY (";
        foreach (string s in PrimaryKeys)
        {
            SQL += " [" + s + "] " + Fields[s] + ",";
            PK += " [" + s + "] " + ",";
        }

        PK = PK.Substring(0, PK.Length - 1) + ") ";
        SQL = SQL + PK + ") ";

        db.RunQuery(SQL);

        //check for fields
        foreach (string s in Fields.Keys)
        {
            db.RunQuery("ALTER TABLE [" + TableName + "] ADD [" + s + "] " + Fields[s]);
        }
    }

    public static Boolean UpdateDBStructure(DatabaseCE db, int Counter)
    {
        Dictionary<string, string> fields = new Dictionary<string, string>();

        switch (Counter)
        {
            case 1://Item
                fields.Add("ID", "[int]");
                fields.Add("PLUCode", "[nvarchar](50)");
                fields.Add("Name", "[nvarchar](50)");
                fields.Add("TaxRateID", "[int]");
                fields.Add("TaxRate", "[float]");
                fields.Add("MRP", "[float]");
                fields.Add("Cost", "[float]");
                fields.Add("CostFranchise", "[float]");
                fields.Add("RateType", "[int]");
                fields.Add("BrandID", "[int]");
                fields.Add("MaxDiscount", "[float]");
                fields.Add("ReOrderLevel", "[int]");
                fields.Add("UrlName", "[nvarchar](300)");
                fields.Add("WeightType", "[int]");
                fields.Add("Weight", "[float]");
                fields.Add("GroupID", "[int]");
                fields.Add("IsHidden", "[int]");
                fields.Add("ToAdjust", "[int]");
                CheckTable(db, "Item", fields, new string[] { "ID" });
                break;
            case 2://Tax Rate
                fields.Add("ID", "[int]");
                fields.Add("Rate", "[float]");
                CheckTable(db, "TaxRate", fields, new string[] { "ID" });
                break;
            case 3://Company
                fields.Add("ID", "[int]");
                fields.Add("Name", "[nvarchar](50)");
                fields.Add("TIN", "[nvarchar](50)");
                fields.Add("TAN", "[nvarchar](20)");
                fields.Add("PAN", "[nvarchar](20)");
                fields.Add("CST", "[nvarchar](50)");
                fields.Add("Address", "[nvarchar](200)");
                fields.Add("ServiceTax", "[nvarchar](20)");
                fields.Add("CIN", "[nvarchar](30)");
                CheckTable(db, "Company", fields, new string[] { "ID" });
                break;
            case 4://Customer
                fields.Add("ID", "[int]");
                fields.Add("StoreID", "[int]");
                fields.Add("Name", "[nvarchar](50)");
                fields.Add("Address", "[nvarchar](50)");
                fields.Add("Area", "[nvarchar](50)");
                fields.Add("City", "[nvarchar](50)");
                fields.Add("PIN", "[int]");
                fields.Add("Mobile", "[nvarchar](12)");
                fields.Add("HouseNumber", "[nvarchar](25)");
                fields.Add("LoyaltyPoints", "[int]");
                fields.Add("Points", "[int]");
                fields.Add("PreviousPoints", "[int]");
                fields.Add("Birthday", "[dateTime]");
                fields.Add("Anniverary", "[dateTime]");
                fields.Add("ProjectID", "[int]");
                fields.Add("ApartmentID", "[int]");
                fields.Add("LastSaleDate", "[dateTime]");
                fields.Add("LastAdvertizedDate", "[dateTime]");
                fields.Add("TotalamountSale", "[float]");
                CheckTable(db, "Customer", fields, new string[] { "ID" });
                break;
            case 5://Billing
                fields.Add("ID", "[int]");
                fields.Add("CompanyID", "[int]");
                fields.Add("StoreID", "[int]");
                fields.Add("TerminalID", "[int]");
                fields.Add("BillNo", "[int]");
                fields.Add("BillDate", "[dateTime]");
                fields.Add("CustomerName", "[nvarchar](50)");
                fields.Add("CustomerAddress", "[nvarchar](50)");
                fields.Add("CustomerArea", "[nvarchar](50)");
                fields.Add("CustomerCity", "[nvarchar](50)");
                fields.Add("CustomerPIN", "[int]");
                fields.Add("CustomerHouseNumber", "[nvarchar](25)");
                fields.Add("CustomerMobile", "[nvarchar](12)");
                fields.Add("Code", "[nvarchar](50)");
                fields.Add("DicountPercentage", "[float]");
                fields.Add("VAT1", "[float]");
                fields.Add("VAT2", "[float]");

                fields.Add("NetAmount", "[float]");
                fields.Add("SentThrough", "[nvarchar](50)");
                fields.Add("Remarks", "[nvarchar](100)");

                fields.Add("PaidCash", "[float]");
                fields.Add("PaidCard", "[float]"); //0-Cash  1-Card
                fields.Add("CardType", "[int]");//Visa-1,Master-2,American-3

                fields.Add("Delivery", "[int]");//

                fields.Add("TransactionID", "[nvarchar](20)");
                fields.Add("DeliveryTime", "[dateTime]");
                fields.Add("ItemCount", "[int]");
                fields.Add("CustomerID", "[int]");
                fields.Add("DeliveryBy", "[nvarchar](100)");
                fields.Add("PaymentRecieved", "[int]"); //Delivery Payment Recieved
                fields.Add("OrderStatus", "[int]"); //1 pending,

                CheckTable(db, "Billing", fields, new string[] { "ID" });
                CheckTable(db, "FutureBilling", fields, new string[] { "ID" });
                break;
            case 6://Purchase
                fields.Add("ID", "[int]");
                fields.Add("PurchaseDate", "[dateTime]");
                fields.Add("VendorID", "[int]");
                fields.Add("Amount", "[float]");
                fields.Add("StoreID", "[int]");
                fields.Add("InvoiceNo", "[nvarchar](20)");
                fields.Add("RCNo", "[nvarchar](20)");
                fields.Add("ItemIncludesTax", "[int]");//0-No,1-Yes
                fields.Add("ItemCount", "[float]");
                fields.Add("PaymentAmount", "[float]");
                fields.Add("CST", "[nvarchar](10)");
                fields.Add("IsReturn", "[int]");
                fields.Add("IsDelete", "[int]");
                fields.Add("IsForm38", "[int]");
                fields.Add("IsNonUP", "[int]"); //0 UP,1 Non UP
                CheckTable(db, "Purchase", fields, new string[] { "ID" });
                break;
            case 7://BillingItem
                fields.Add("ID", "[int]");
                fields.Add("BillingID", "[int]");
                fields.Add("Code", "[nvarchar](20)");
                fields.Add("ItemName", "[nvarchar](50)");
                fields.Add("Rate", "[float]");
                fields.Add("Cost", "[float]");
                fields.Add("Quantity", "[float]");
                fields.Add("BillItemDiscount", "[float]");
                fields.Add("TaxPerc", "[float]");
                fields.Add("Total", "[float]");
                fields.Add("ItemID", "[int]");

                //These fields are copy from Billing table
                fields.Add("SaleDate", "[dateTime]");
                fields.Add("BillNo", "[int]");
                fields.Add("StoreID", "[int]");
                fields.Add("IsHidden", "[int]");
                fields.Add("AdjustedRate", "[float]");
                fields.Add("AdjustedCost", "[float]");
                CheckTable(db, "BillingItem", fields, new string[] { "ID" });
                CheckTable(db, "FutureBillingItem", fields, new string[] { "ID" });
                db.RunQuery("CREATE INDEX Indx_BillingID ON BillingItem (BillingID)");
                break;

            case 8://Vendor
                fields.Add("ID", "[int]");
                fields.Add("Name", "[nvarchar](50)");
                fields.Add("Address", "[nvarchar](50)");
                fields.Add("Area", "[nvarchar](50)");
                fields.Add("City", "[nvarchar](50)");
                fields.Add("PIN", "[int]");
                fields.Add("TIN", "[nvarchar](20)");
                fields.Add("Mobile", "[nvarchar](12)");
                fields.Add("AmountDue", "[float]");
                fields.Add("AmountPaid", "[float]");
                fields.Add("IsClient", "[int]");
                fields.Add("State", "[int]");
                CheckTable(db, "Vendor", fields, new string[] { "ID" });
                break;

            case 9://Inventory
                fields.Add("ID", "[int]");
                fields.Add("PurchaseID", "[int]");
                fields.Add("VendorID", "[int]");
                fields.Add("ItemID", "[int]");
                fields.Add("Quantity", "[float]");
                fields.Add("Cost", "[float]");
                fields.Add("CostUnitType", "[int]");//kg,gm,pc,dozen
                fields.Add("MRP", "[float]");
                fields.Add("Tax", "[float]");
                fields.Add("Discount", "[float]");
                fields.Add("TotalAmount", "[float]");
                fields.Add("Unit", "[int]"); // weight/count
                fields.Add("UnitType", "[int]");//kg,gm,pc,dozen
                fields.Add("Expiry", "[dateTime]");
                fields.Add("SNo", "[int]");
                fields.Add("PLUCode", "[nvarchar](50)");
                fields.Add("RecordType", "[int]");//0-Purchase, 1= Return, 2=Damage
                fields.Add("ItemIncludesTax", "[int]");//0-No,1-Yes
                fields.Add("RcNo", "[nvarchar](20)");
                fields.Add("Date", "[dateTime]");
                fields.Add("StoreID", "[int]");
                fields.Add("IsDelete", "[int]");
                fields.Add("FromStoreID", "[int]");
                fields.Add("InvoiceNo", "[nvarchar](20)");

                CheckTable(db, "Inventory", fields, new string[] { "ID" });
                break;

            case 10://Menu
                fields.Add("ID", "[int]");
                fields.Add("ParentID", "[int]");
                fields.Add("Name", "[nvarchar](100)");
                fields.Add("UrlName", "[nvarchar](300)");
                CheckTable(db, "Menu", fields, new string[] { "ID" });
                break;

            case 11://Item Menu Link
                fields.Add("ID", "[int]");
                fields.Add("ItemID", "[int]");
                fields.Add("MenuID", "[int]");
                fields.Add("Rank", "[int]");
                fields.Add("SerialNo ", "[float]");
                CheckTable(db, "ItemMenuLink", fields, new string[] { "ID" });
                break;

            case 12://Brands
                fields.Add("ID", "[int]");
                fields.Add("Name", "[nvarchar](50)");
                fields.Add("UrlName", "[nvarchar](300)");
                CheckTable(db, "Brand", fields, new string[] { "ID" });
                break;

            case 13://Delivery By 
                fields.Add("ID", "[int]");
                fields.Add("Name", "[nvarchar](50)");
                CheckTable(db, "DeliveryBy", fields, new string[] { "ID" });
                break;

            case 14://Stock
                fields.Add("ItemID", "[int]");
                fields.Add("StockMonth", "[dateTime]");
                fields.Add("OpeningStock", "[float]");
                fields.Add("Purchase", "[float]");
                fields.Add("Sale", "[float]");
                fields.Add("TransferIn", "[float]");
                fields.Add("TransferOut", "[float]");
                fields.Add("Return", "[float]");
                fields.Add("Damage", "[float]");
                fields.Add("StoreID", "[int]");
                fields.Add("ClosingStock", "[float]");
                fields.Add("AutoOpeningStock", "[float]");
                fields.Add("AutoClosingStock", "[float]");
                CheckTable(db, "Stock", fields, new string[] { "ItemID", "StockMonth", "StoreID" });
                break;

            case 15://Terminal
                fields.Add("ID", "[int]");
                fields.Add("CompanyID", "[int]");
                fields.Add("StoreID", "[int]");
                fields.Add("BillNo", "[int]");
                fields.Add("IsVirtual", "[int]");
                fields.Add("Name", "[nvarchar](50)");
                fields.Add("Prefix", "[nvarchar](10)");
                fields.Add("Address", "[nvarchar](100)");
                fields.Add("LastAccess", "[dateTime]");
                fields.Add("User", "[nvarchar](200)");
                fields.Add("SoftwareVersion", "[nvarchar](20)");
                CheckTable(db, "Terminal", fields, new string[] { "ID" });
                break;
            case 16://CompanyUsers
                fields.Add("ID", "[int]");
                fields.Add("Name", "[nvarchar](50)");
                fields.Add("EmailID", "[nvarchar](50)");
                fields.Add("Password", "[nvarchar](20)");
                fields.Add("ID2", "[nvarchar](500)");
                CheckTable(db, "CompanyUser", fields, new string[] { "ID" });
                break;
            case 17://TerminalAccess
                fields.Add("TerminalID", "[int]");
                fields.Add("UserID", "[int]");
                CheckTable(db, "TerminalAccess", fields, new string[] { "TerminalID", "UserID" });
                break;
            case 18://Store
                fields.Add("ID", "[int]");
                fields.Add("CompanyID", "[int]");
                fields.Add("Name", "[nvarchar](50)");
                fields.Add("Address1", "[nvarchar](100)");
                fields.Add("Address2", "[nvarchar](100)");
                fields.Add("Phone", "[nvarchar](100)");
                fields.Add("IsFranchise", "[int]");
                CheckTable(db, "Store", fields, new string[] { "ID" });
                break;
            case 19://Purchase Payment
                fields.Add("ID", "[int]");
                fields.Add("PurchaseID", "[int]");
                fields.Add("BillNo", "[nvarchar](50)");
                fields.Add("BillDate", "[dateTime]");
                fields.Add("VendorID", "[int]");
                fields.Add("Cash", "[float]");
                fields.Add("Cheque", "[float]");
                fields.Add("ChequeNo", "[nvarchar](50)");
                fields.Add("BankID", "[int]");
                fields.Add("StoreID", "[int]");
                fields.Add("PaymentDate", "[dateTime]");
                fields.Add("LedgerNo", "[nvarchar](20)");
                fields.Add("IsDelete", "[int]");

                CheckTable(db, "PurchasePayment ", fields, new string[] { "ID" });
                break;
            case 20://Employee
                fields.Add("ID", "[int]");
                fields.Add("Name", "[nvarchar](50)");
                fields.Add("DOJ", "[dateTime]");
                fields.Add("DOB", "[dateTime]");
                fields.Add("Address", "[nvarchar](100)");
                fields.Add("Mobile", "[nvarchar](15)");
                fields.Add("CompanyID", "[int]");
                fields.Add("StoreID", "[int]");
                fields.Add("Email", "[nvarchar](100)");
                fields.Add("Password", "[nvarchar](20)");
                fields.Add("ID2", "[nvarchar](500)");
                fields.Add("IsAdmin", "[int]"); //0-no,1-Yes
                fields.Add("IsDelete", "[int]");
                fields.Add("Designation", "[int]");
                //fields.Add("IPAddress", "[nvarchar](100)");
                CheckTable(db, "Employee", fields, new string[] { "ID" });
                break;
            case 21://EmployeeAttendance
                fields.Add("ID", "[int]");
                fields.Add("EmployeeID", "[int]");
                fields.Add("Name", "[nvarchar](50)");
                fields.Add("Date", "[dateTime]");
                fields.Add("DateIn", "[dateTime]");
                fields.Add("DateOut", "[dateTime]");
                fields.Add("LeaveType", "[int]");
                fields.Add("StoreID", "[int]");
                CheckTable(db, "EmployeeAttendance", fields, new string[] { "ID" });
                break;
            case 22://Expense Log
                fields.Add("ID", "[int]");
                fields.Add("Name", "[nvarchar](50)");
                fields.Add("Amount", "[float]");
                fields.Add("Date", "[dateTime]");
                fields.Add("StoreID", "[int]");
                fields.Add("TerminalID", "[int]");
                fields.Add("ExpenseType", "[int]");
                CheckTable(db, "ExpenseLog", fields, new string[] { "ID" });
                break;
            case 23://Report Daily Sale
                fields.Add("ID", "[int]");
                fields.Add("Date", "[dateTime]");
                fields.Add("StoreID", "[int]");
                fields.Add("TerminalID", "[int]");
                fields.Add("Amount", "[float]");
                fields.Add("AmountCard", "[float]");
                fields.Add("Margin", "[float]");
                fields.Add("Vat", "[float]");
                CheckTable(db, "ReportDailySale", fields, new string[] { "ID" });
                break;
            case 24: //Item Store Price
                fields.Add("ID", "[int]");
                fields.Add("ItemID", "[int]");
                fields.Add("StoreID", "[int]");
                fields.Add("Rate", "[float]");
                fields.Add("MRP", "[float]");
                CheckTable(db, "ItemStorePrice", fields, new string[] { "ID" });
                break;
            case 25: //Bank
                fields.Add("ID", "[int]");
                fields.Add("Name", "[nvarchar](50)");
                fields.Add("Address", "[nvarchar](200)");
                fields.Add("AccountNo", "[nvarchar](20)");
                CheckTable(db, "Bank", fields, new string[] { "ID" });
                break;
            case 26://StoreAccess
                fields.Add("StoreID", "[int]");
                fields.Add("UserID", "[int]");
                CheckTable(db, "StoreAccess", fields, new string[] { "StoreID", "UserID" });
                break;

            case 27://Order
                fields.Add("ID", "[int]");
                fields.Add("Name", "[nvarchar](100)");
                fields.Add("Mobile", "[nvarchar](15)");
                fields.Add("Address", "[nvarchar](100)");
                fields.Add("OrderDate", "[dateTime]");
                fields.Add("StoreID", "[int]");
                fields.Add("Status", "[int]");
                fields.Add("ProjectID", "[int]");
                fields.Add("ProjectOther", "[nvarchar](100)");
                fields.Add("ApartmentNo", "[nvarchar](20)");
                fields.Add("DeliveryPersonID", "[int]");
                fields.Add("DeliveryPersonName", "[nvarchar](100)");
                fields.Add("DeliveryDateTime", "[dateTime]");
                CheckTable(db, "OrderOL", fields, new string[] { "ID" });
                break;

            case 28://OrderItem
                fields.Add("ID", "[int]");
                fields.Add("OrderID", "[int]");
                fields.Add("ItemID", "[int]");
                fields.Add("ItemName", "[nvarchar](100)");
                fields.Add("Quantity", "[float]");
                fields.Add("Price", "[float]");
                CheckTable(db, "OrderItem", fields, new string[] { "ID" });
                break;

            case 29://Salary
                fields.Add("ID", "[int]");
                fields.Add("EmployeeID", "[int]");
                fields.Add("EmployeeName", "[nvarchar](100)");
                fields.Add("SalaryDate", "[dateTime]");
                fields.Add("Basic", "[float]");
                fields.Add("HRA", "[float]");
                fields.Add("TA", "[float]");
                fields.Add("Medical", "[float]");
                fields.Add("Advance", "[float]");
                fields.Add("TDS", "[float]");
                fields.Add("Loan", "[float]");
                fields.Add("PaidOn", "[dateTime]");
                fields.Add("PaidAmount", "[float]");
                fields.Add("StoreID", "[int]");
                CheckTable(db, "Salary", fields, new string[] { "ID" });
                break;

            case 30://MonthlyTotal
                fields.Add("ID", "[int]");
                fields.Add("AmountType", "[int]");
                fields.Add("Amount", "[float]");
                fields.Add("StoreID", "[int]");
                fields.Add("TotalDate", "[datetime]");
                fields.Add("AmountManual", "[float]");
                fields.Add("UpdateDateAuto", "[datetime]");
                fields.Add("UpdateDateManual", "[datetime]");
                fields.Add("CostVat", "[float]");
                fields.Add("PriceVat", "[float]");
                CheckTable(db, "MonthlyTotal", fields, new string[] { "ID" });
                break;

            case 31://PurchasePaymentLink
                fields.Add("ID", "[int]");
                fields.Add("PurchaseID", "[int]");
                fields.Add("VendorID", "[int]");
                fields.Add("PaymentID", "[int]");
                fields.Add("PaymentDate", "[datetime]");
                fields.Add("Amount", "[float]");
                fields.Add("IsDelete", "[int]");
                CheckTable(db, "PurchasePaymentLink", fields, new string[] { "ID" });
                break;

            case 32://Project
                fields.Add("ID", "[int]");
                fields.Add("ProjectName", "[nvarchar](100)");
                fields.Add("Address", "[nvarchar](255)");
                fields.Add("Area", "[nvarchar](50)");
                fields.Add("City", "[nvarchar](50)");
                fields.Add("State", "[nvarchar](255)");
                fields.Add("PIN", "[int]");
                CheckTable(db, "Project", fields, new string[] { "ID" });
                break;

            case 33://Apartment
                fields.Add("ID", "[int]");
                fields.Add("ProjectID", "[int]");
                fields.Add("FlatNo", "[int]");
                fields.Add("Status", "[int]");  //0 - Vacant , 1- Owner 2- Rented
                fields.Add("ResidentName", "[nvarchar](100)");
                fields.Add("ResidentPhone", "[nvarchar](20)");
                fields.Add("OwnerName", "[nvarchar](100)");
                fields.Add("OwnerPhone", "[nvarchar](20)");
                CheckTable(db, "Apartment", fields, new string[] { "ID" });
                break;

            case 34://ItemLink
                fields.Add("GroupID", "[int]");
                fields.Add("ItemID", "[int]");
                CheckTable(db, "ItemLink", fields, new string[] { "GroupID", "ItemID" });
                break;

            case 35://VendorDue
                fields.Add("VendorID", "[int]");
                fields.Add("StoreID", "[int]");
                fields.Add("Purchase", "[float]");
                fields.Add("Payment", "[float]");
                fields.Add("FinancialYear", "[int]"); // put the start month year if 2015-2016 store 2015
                CheckTable(db, "VendorDue", fields, new string[] { "VendorID", "StoreID", "FinancialYear" });
                break;
            case 36://ClientInvoice
                fields.Add("ID", "[int]");
                fields.Add("ClientID", "[int]");
                fields.Add("BillNo", "[int]");
                fields.Add("PONo", "[int]");
                fields.Add("Transport", "[nvarchar](100)");
                fields.Add("VehicleNo", "[nvarchar](20)");
                fields.Add("Station", "[nvarchar](100)");
                fields.Add("InvoiceNo", "[nvarchar](20)");
                fields.Add("CreatedDate", "[datetime]");
                fields.Add("BookNo", "[nvarchar](20)");
                CheckTable(db, "ClientInvoice", fields, new string[] { "ID" });
                break;
            default:
                return false;
        }
        return true;
    }
}

