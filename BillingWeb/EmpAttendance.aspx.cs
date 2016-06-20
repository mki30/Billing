using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class EmpAttendance : BasePage
{
    protected new void Page_Load(object sender, EventArgs e)
    {
        DefaultFilterDays = 1;
        base.Page_Load(sender, e);
        Title = "Employee Attendance";
        CurrentNav = "navEmpAttendance";
        string empId = QueryString("employeeid");


        ltFilters.Text = GetDateFilterLinks(0);
        if (!IsPostBack)
        {
            Common.FillLeaveTypeDropdown(ddLeaveType);
            Common.FillEmployeeDropdown(ddEmployee, Cmn.ToInt(SessionState.StoreID));
            txtDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
        }
        ddEmployee.SelectedValue = empId;
        ShowEmpList();
        ShowDataTable();
    }

    void ShowEmpList()
    {
        List<BillingLib.EmployeeAttendance> listAtt = BillingLib.EmployeeAttendance.GetAll(DateTime.Today, DateTime.Today, 0, SessionState.StoreID);
        StringBuilder str = new StringBuilder();
        
        Boolean RecordsFound = false;
        int ctr = 0;
        foreach (BillingLib.Employee emp in BillingLib.Employee.GetAll(0, SessionState.StoreID).Values)
        {
            if (emp.IsDelete == 1)
                continue;
            BillingLib.EmployeeAttendance att = listAtt.FirstOrDefault(m => m.EmployeeID == emp.ID);

            if (att != null && att.DateIn != Cmn.MinDate && att.DateOut != Cmn.MinDate)
                continue;

            str.Append("<tr><td title=" + emp.ID + ">" + (++ctr) + "<td>" + emp.Name.Split(' ')[0] + "<td>");

            if (att == null || att.DateIn == Cmn.MinDate)
                str.Append("<a class='btn btn-sm btn-success' href='#' onclick=' return SaveAttendance(" + emp.ID + ",0)'>In</a>");

            str.Append("<td>");

            if (att == null || att.DateOut == Cmn.MinDate)
                str.Append("<a class='btn btn-sm btn-warning' href='#' onclick=' return SaveAttendance(" + emp.ID + ",1)'>Out</a>");
            RecordsFound = true;
        }
        if (RecordsFound)
            ltEmployeeList.Text = "<table class='table table-condensed table-striped table-bordered'><tr><th style='width:15px;'>#<th>Employee<th>In<th>Out" + str.Append("</table>").ToString();
    }
    
    private void ShowDataTable()
    {
        if (CheckDateRange(lblDataTable))
            return;
        
        List<BillingLib.EmployeeAttendance> list = BillingLib.EmployeeAttendance.GetAll(FilterDateFrom, FilterDateTo, Cmn.ToInt(ddEmployee.SelectedValue), SessionState.StoreID).OrderByDescending(m => m.Date).ToList();
        StringBuilder str = new StringBuilder("<table class='table table-condensed table-striped table-bordered'><tr><th>ID<th>Name<th>Date<th>In<th>Out<th>Leave");
        foreach (BillingLib.EmployeeAttendance empAtend in list)
        {
            BillingLib.Employee e = null;
            Global.EmployeeList.TryGetValue(empAtend.EmployeeID, out e);

            str.Append("<tr><td>" + empAtend.ID + "<td><a href='#' onclick='EditAttendance(" + empAtend.ID + ")'>" + (e != null ? e.Name : "")
                + "</a><td>" + empAtend.Date.ToString("dd-MMM-yyy")
                + "<td>" + empAtend.DateIn.ToString("HH:mm")
                + "<td>" + empAtend.DateOut.ToString("HH:mm") 
                + "<td>" + (LeaveType)empAtend.LeaveType);
        }
        lblDataTable.Text = str.Append("</table>").ToString();
    }
}
