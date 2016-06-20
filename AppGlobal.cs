using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BillingLib;
using System.IO;
using System.Windows.Forms;

namespace BillingSoft
{
    public class AppGlobal
    {
        public static Terminal thisTerminal = new Terminal();
        public static Store thisStore = new Store();
        public static Company thisCompany = new Company();
        public static Employee thisEmployee = new Employee();
        //public static List<BillingLib.Menu> thisMenuList=new List<BillingLib.Menu>();

        public static void Load()
        {
            Dictionary<string, string> Config = new Dictionary<string, string>();
            Config.Add("SERVER", "http://meatmart.biz/");
            Config.Add("TERMINALID", "0");
            Config.Add("CITY", "");

            string FileName = @"C:\Billing\Config.txt";
            if (File.Exists(FileName))
            {
                string[] Lines = File.ReadAllLines(FileName);
                foreach (string L in Lines)
                {
                    string[] F = L.Split('=');
                    if (F.Length != 2)
                        continue;

                    if (Config.ContainsKey(F[0]))
                        Config[F[0]] = F[1];
                }
            }
            
            string Str = "";
            foreach (KeyValuePair<string, string> kvp in Config)
            {
                Str += kvp.Key + "=" + kvp.Value + Environment.NewLine;
            }

            File.WriteAllText(FileName, Str);

            Global.WebServer = Config["SERVER"];
            Global.City = Config["CITY"];
            AppGlobal.thisTerminal.ID = Cmn.ToInt(Config["TERMINALID"]);

            Terminal temp = Terminal.GetByID(Cmn.ToInt(Config["TERMINALID"]));
            if (temp != null)
            {
                AppGlobal.thisTerminal=temp;
                AppGlobal.thisStore = Store.GetByID(AppGlobal.thisTerminal.StoreID);
                if (AppGlobal.thisStore != null)
                {
                    Company comp=Company.GetByID(AppGlobal.thisStore.CompanyID);
                    if(comp!=null)
                        AppGlobal.thisCompany = comp;
                }
            }
            else
                MessageBox.Show("Terminal Information not found");
        }
    }
}
