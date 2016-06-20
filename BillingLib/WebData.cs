using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace BillingLib
{
    public enum WebDataAction
    {
        SetBill,
        GetBill,
        SetCustomer,
        GetCustomer,
        SetItem,
        GetItems,
        GetItemStorePrice,
        GetTerminal,
        GetStore,
        GetCompany,
        GetEmployee,
        GetSociety,
        SetTerminal,
        GetDeliveryMen,
        GetMenues,
        GetItemMenueLink,
    }

    public class WebData
    {
        public static object Data(WebDataAction action, object obj=null)
        {
            using (WebClient wc = new WebClient())
            {
                NameValueCollection nvc = new NameValueCollection();
                nvc.Add("data", new JavaScriptSerializer().Serialize(obj));
                var response = wc.UploadValues(Global.WebServer + "data.aspx?Action=WebData&Data1=" + (int)action, nvc);
                string ret = System.Text.Encoding.UTF8.GetString(response);

                switch (action)
                {
                    case WebDataAction.SetBill:
                    case WebDataAction.GetBill: 
                        return (Billing)new JavaScriptSerializer().Deserialize<Billing>(ret);
                    case WebDataAction.GetCustomer: return (List<Customer>)new JavaScriptSerializer().Deserialize<List<Customer>>(ret);
                    case WebDataAction.GetItems: return (List<Item>)new JavaScriptSerializer().Deserialize<List<Item>>(ret);
                    case WebDataAction.GetItemStorePrice: return (List<ItemStorePrice>)new JavaScriptSerializer().Deserialize<List<ItemStorePrice>>(ret);
                    case WebDataAction.GetTerminal: return (Terminal)new JavaScriptSerializer().Deserialize<Terminal>(ret);
                    case WebDataAction.GetStore: return (Store)new JavaScriptSerializer().Deserialize<Store>(ret);
                    case WebDataAction.GetCompany: return (Company)new JavaScriptSerializer().Deserialize<Company>(ret);
                    case WebDataAction.GetEmployee: return (Employee)new JavaScriptSerializer().Deserialize<Employee>(ret);
                    case WebDataAction.GetSociety: return (List<Project>)new JavaScriptSerializer().Deserialize<List<Project>>(ret);
                    case WebDataAction.SetTerminal: return (Terminal)new JavaScriptSerializer().Deserialize<Terminal>(ret);
                    case WebDataAction.GetDeliveryMen: return (List<Employee>)new JavaScriptSerializer().Deserialize<List<Employee>>(ret);
                    case WebDataAction.GetItemMenueLink:return (List<ItemMenuLink>)new JavaScriptSerializer().Deserialize<List<ItemMenuLink>>(ret);
                    case WebDataAction.GetMenues:return (List<Menu>)new JavaScriptSerializer().Deserialize<List<Menu>>(ret);

                }
            }

            return null;
        }
    }
}
