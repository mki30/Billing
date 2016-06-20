using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using BillingLib;

public enum ExpenseCategory
{
    Petrol=1,
    KichenExpense=2,
    StaffTea=3,
    Stationary=4
}

public enum Designation
{
    Manager=1, 
    Cashier=2, 
    Salesman=3,
    Delivery=4
}

public enum AmountType
{
    Purchase = 1,
    Sale = 2,
    DailyExpense = 3,
    Salary = 4,
    Rent = 5,
    Electricity = 6,
    Loan = 7,
    OtherExpense = 8,
    Margin = 9,
    Payment=10
}

public enum DeliveryStatus
{
    Requested = 1,
    Assigned = 2,
    Confirm = 3,
    GettingReady = 4,
    Ready = 5,
    OutForDelivery = 6,
    Delivered = 7
}

public enum UnitType
{
    none = 0,
    gm = 1,
    kg = 2,
    pc = 3,
    dozen = 4,
    ml = 5,
    lt = 6,
}

public enum LeaveType
{
    none = 0,
    CL = 1,
    PL = 2,
}

public enum ReportType
{
    none = 0,
    DailySale = 1,
    ItemSale = 2,
    ItemPurchased = 3,
    BrandWiseSale = 4,
    ItemWiseSale = 5,
    Stock = 6,
    Expense = 7,
    Attendance = 8,
    PriceMismach = 9,
    ItemSalePurchase=10
}

public enum RecordType
{
    None = -1,
    Purchase = 0,
    Return = 1,
    Damage = 2,
    TransferIn = 3,
    TransferOut = 4,
    Transfer = 5,
    Sale = 6,
    StockManualEntry = 7,
}

public enum TransferType
{
    None = 0,
    TransferIn = 1,
    TransferOut = 2,
}

public class Global
{
    public static Dictionary<int, Tax> listTaxRate = new Dictionary<int, Tax>();
    public static Dictionary<int, Company> listCompany = new Dictionary<int, Company>();
    public static Dictionary<int, Employee> EmployeeList = new Dictionary<int, Employee>();
    public static Dictionary<int, Store> StoreList = new Dictionary<int, Store>();
    public static Dictionary<int, Terminal> TerminalList = new Dictionary<int, Terminal>();
    public static List<Project> ProjectList = new List<Project>();
    public static List<Bank> BankList = new List<Bank>();

    public static Dictionary<string, Employee> Users = new Dictionary<string, Employee>();

    public static string WebServer = "http://meatmart.biz/";
    public static string City = "";


    public Global()
    {
    }


    public static Employee GetEmployee(int ID)
    {
        Employee e;
        if (EmployeeList.TryGetValue(ID, out e))
            return e;

        return null;
    }

    public static Employee GetEmployee(string EmailID, string Password)
    {
        EmailID = EmailID.ToLower();
        Password = Password.ToLower();
        return EmployeeList.Values.FirstOrDefault(m => m.Email == EmailID && m.Password == Password);
    }


    public static void LoadGlobal()
    {
        StoreList = Store.GetAll();
        TerminalList = Terminal.GetAll();
        EmployeeList = Employee.GetAll();
        listTaxRate = Tax.GetAll();
        ProjectList = Project.GetAll();
        BankList = Bank.GetAll();
        foreach (Company c in Company.GetAll())
        {
            listCompany.Add(c.ID, c);
            c.LoadGlobal();
        }
    }

    public static string GetStoreName(int StoreID)
    {
        Store s = Global.StoreList.Values.FirstOrDefault(m => m.ID == StoreID);
        return s == null ? "" : s.Name;
    }
    
    public static string GetText(object obj)
    {
        if (obj is ExpenseCategory)
        {
            switch ((ExpenseCategory)obj)
            {
                case ExpenseCategory.StaffTea: return "Staff Tea";
                case ExpenseCategory.KichenExpense: return "Kichen Expense";
            }
        }
        return "";
    }

}