using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.UI;
using System.Text;
using System.IO;

/// <summary>
/// Summary description for Common
/// </summary>
public class Common
{
    public Common()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    
    public static void FillDilvbetyBoyDropDown(DropDownList ddDeliveryBoy)
    {
        ddDeliveryBoy.Items.Clear();
        ddDeliveryBoy.Items.Add(new ListItem("-Deliver By-", "0"));
        List<BillingLib.Employee> empList = BillingLib.Employee.GetDelievetyMenbyStore(SessionState.StoreID).Where(m=>m.IsDelete!=1).ToList();
        foreach (BillingLib.Employee e in empList)
        {
            ddDeliveryBoy.Items.Add(new ListItem(e.Name.ToString(), e.Name.ToString()));
        }
    }
    
    public static void FillExpenseType(DropDownList ddExpense)
    {
        ddExpense.Items.Clear();
        ddExpense.Items.Add(new ListItem("-Expense Type-", "0"));
        foreach (int Value in Enum.GetValues(typeof(ExpenseCategory)))
        {
            string Display = Enum.GetName(typeof(ExpenseCategory), Value);
            ddExpense.Items.Add(new ListItem(Display, Value.ToString()));
        }
    }

    public static void FillCompanyDropdown(DropDownList ddCompany)
    {
        ddCompany.Items.Clear();
        List<BillingLib.Company> list = BillingLib.Company.GetAll();
        foreach (BillingLib.Company c in list)
        {
            ddCompany.Items.Add(new ListItem(c.Name, c.ID.ToString()));
        }
        ddCompany.SelectedValue = SessionState.CompanyID.ToString();
    }

    public static void FillStoreDropdown(DropDownList ddStore, bool addSelect = false, bool skipSelected = false)
    {
        ddStore.Items.Clear();
        List<BillingLib.Store> list = BillingLib.Store.GetByCompanyID(SessionState.CompanyID);

        if (SessionState.IsAdmin || SessionState.IsAdminRRA)
        {
            if (addSelect)
                ddStore.Items.Add(new ListItem("All Stores", "0"));

            foreach (BillingLib.Store c in list)
            {
                ddStore.Items.Add(new ListItem(c.Name, c.ID.ToString()));
            }
            if (SessionState.StoreID != -1)
                ddStore.SelectedValue = SessionState.StoreID.ToString();
        }
        else
        {
            foreach (BillingLib.Store c in list.Where(m => m.ID == SessionState.StoreID))
            {
                ddStore.Items.Add(new ListItem(c.Name, c.ID.ToString()));
            }
        }

        if (skipSelected)  //Skip store selected in session
        {
            ddStore.Items.Clear();
            foreach (BillingLib.Store c in list)
            {
                ddStore.Items.Add(new ListItem(c.Name, c.ID.ToString()));
            }
            BillingLib.Store s = list.FirstOrDefault(m => m.ID == SessionState.StoreID);
            if (s != null)
                ddStore.Items.Remove(new ListItem(s.Name, s.ID.ToString()));
        }
    }

    public static void FillVendorDropdown(DropDownList ddVendor, bool IsClient = false)
    {
        ddVendor.Items.Clear();
        ddVendor.Items.Add(new ListItem("-Vendor-", ""));
        List<BillingLib.Vendor> list = BillingLib.Vendor.GetByCompanyID();
        if (IsClient)
            list = list.Where(m => m.IsClient == true).ToList();
        foreach (BillingLib.Vendor v in list)
        {
            ddVendor.Items.Add(new ListItem(v.Name, v.ID.ToString()));
        }

    }
    
    public static void FillBank(DropDownList ddBank)
    {
        ddBank.Items.Clear();
        foreach (BillingLib.Bank v in BillingLib.Bank.GetAll())
        {
            ddBank.Items.Add(new ListItem(v.Name, v.ID.ToString()));
        }
    }

    public static void FillTerminal(DropDownList ddTerminal, int companyId, int storeId = 0)
    {
        ddTerminal.Items.Clear();
        ddTerminal.Items.Add(new ListItem("-Terminal-", ""));
        if (companyId != -1)
        {
            List<BillingLib.Terminal> list = BillingLib.Terminal.GetByCompanyAndStore(companyId, storeId);
            foreach (BillingLib.Terminal t in list)
            {
                ddTerminal.Items.Add(new ListItem(t.Name, t.ID.ToString()));
            }
        }
    }

    public static void FillTaxDropDown(DropDownList ddTaxRate)
    {
        ddTaxRate.Items.Clear();
        ddTaxRate.Items.Add(new ListItem("-Tax-", "0"));
        foreach (BillingLib.Tax t in Global.listTaxRate.Values)
        {
            ddTaxRate.Items.Add(new ListItem(t.Rate.ToString(), HttpContext.Current.Server.HtmlEncode(t.Rate.ToString())));
        }
    }

    public static void ResetForm(HtmlForm htmlForm)
    {
        foreach (Control ctrl in htmlForm.Controls)
        {
            if (ctrl.GetType() == typeof(TextBox))
            {
                ((TextBox)(ctrl)).Text = string.Empty;
            }
            else if (ctrl.GetType() == typeof(DropDownList))
            {
                ((DropDownList)(ctrl)).SelectedIndex = 0;
            }
        }
    }

    public static void FillUnitDropdown(DropDownList ddUnitType)
    {
        ddUnitType.Items.Clear();
        foreach (UnitType r in Enum.GetValues(typeof(UnitType)))
        {
            ListItem item = new ListItem(Enum.GetName(typeof(UnitType), r), ((int)r).ToString());
            ddUnitType.Items.Add(item);
        }
    }

    public static void FillMenu(DropDownList dd)
    {
        dd.Items.Clear();
        dd.Items.Add(new ListItem("--Select--", ""));
        foreach (BillingLib.Menu m in SessionState.Company.listMenu.Where(m => m.ParentID == 0))
        {
            if (m.ChildMenu.Count == 0)
                dd.Items.Add(new ListItem(m.Name, m.ID.ToString()));
            foreach (BillingLib.Menu n in m.ChildMenu)
            {
                dd.Items.Add(new ListItem(m.Name + "-" + n.Name, n.ID.ToString()));
            }
        }
    }

    public static void FillEmployeeDropdown(DropDownList ddEmployee, int StoreID)
    {
        ddEmployee.Items.Clear();
        ddEmployee.Items.Add(new ListItem("-Select-", ""));
        foreach (BillingLib.Employee e in Global.EmployeeList.Values.Where(m => m.StoreID == StoreID))
        {
            ddEmployee.Items.Add(new ListItem(e.Name.ToString(), e.ID.ToString()));
        }
    }

    public static void FillLeaveTypeDropdown(DropDownList ddLeaveType)
    {
        ddLeaveType.Items.Clear();
        foreach (LeaveType r in Enum.GetValues(typeof(LeaveType)))
        {
            ListItem item = new ListItem(Enum.GetName(typeof(LeaveType), r), ((int)r).ToString());
            ddLeaveType.Items.Add(item);
        }
    }

    public static void FillBrandDropdown(DropDownList ddBrand, string SelectText = "")
    {
        ddBrand.Items.Clear();
        if (SelectText == "")
            SelectText = "-Brand-";
        else
            SelectText = "-All Brands-";
        ddBrand.Items.Add(new ListItem(SelectText, ""));
        foreach (BillingLib.Brand b in SessionState.Company.BrandList)
        {
            ddBrand.Items.Add(new ListItem(b.Name.ToString(), b.ID.ToString()));
        }
    }
    
    public static void FillItemDropDown(DropDownList ddItems)
    {
        ddItems.Items.Clear();
        foreach (BillingLib.Item i in SessionState.Company.ItemList)
        {
            ddItems.Items.Add(new ListItem(i.Name.ToString(), i.ID.ToString()));
        }
    }
    
    public static void WriteClientScript(Page pg, string Client_Script)
    {
        ClientScriptManager cs = pg.ClientScript;
        string csname1 = "S1";
        if (!cs.IsClientScriptBlockRegistered(pg.GetType(), csname1))
        {
            StringBuilder cstext2 = new StringBuilder();
            cstext2.Append("<script language='javascript' type='text/javascript'> \n");
            cstext2.Append(Client_Script);
            cstext2.Append("</script>");
            cs.RegisterClientScriptBlock(pg.GetType(), csname1, cstext2.ToString(), false);
        }
    }

    public static void WriteTextFile(string type, string[] Lines)
    {
        //string[] Lines = File.ReadAllLines(System.Web.HttpContext.Current.Server.MapPath(@"tasklog.txt"));
        switch (type)
        {
            case "stock":
                Lines[0] = "stock:" + Cmn.ToDate(DateTime.Now).ToString();
                break;
            case "budget":
                Lines[1] = "budget:" + Cmn.ToDate(DateTime.Now).ToString();
                break;
        }
        File.WriteAllLines(System.Web.HttpContext.Current.Server.MapPath(@"tasklog.txt"), Lines);
    }
}