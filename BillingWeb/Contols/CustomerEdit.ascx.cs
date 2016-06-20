using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Contols_CostomerEdit : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ddCustomerStore.Items.Add(new ListItem("--Select--", "0"));
        foreach (KeyValuePair<int, BillingLib.Store> kvp in Global.StoreList)
        {
            ddCustomerStore.Items.Add(new ListItem(kvp.Value.Name, kvp.Value.ID.ToString()));
        }
        List<BillingLib.Project> PL = BillingLib.Project.GetAll();
    }
}