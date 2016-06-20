using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BillingLib;
using System.IO;

namespace BillingSoft
{
    public partial class frmLogin : Form
    {
        string path = @"C:\Billing\";
        string lastlogin = @"C:\Billing\LastLogin.txt";

        public frmLogin()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            btnLogin.Enabled = false;

            if (txtEmaild.Text.Length == 0)
            {
                MessageBox.Show("Please enter emaid id");
                txtEmaild.Focus();
                return;
            }

            if (txtPassword.Text.Length == 0)
            {
                MessageBox.Show("Please enter password");
                txtPassword.Focus();
                return;
            }

            AppGlobal.thisEmployee = Employee.GetByEmailandPassword((txtEmaild.Text), txtPassword.Text); // check in local database

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            File.WriteAllText(lastlogin, txtEmaild.Text);

            btnLogin.Enabled = true;

            if (AppGlobal.thisEmployee == null)
            {
                AppGlobal.thisEmployee = new Employee() { Email = (txtEmaild.Text), Password = txtPassword.Text };
                AppGlobal.thisEmployee = (Employee)WebData.Data(WebDataAction.GetEmployee, AppGlobal.thisEmployee);
                if (AppGlobal.thisEmployee != null)
                {
                    AppGlobal.thisEmployee.Save();
                    StoreAccess.DeleteAll(AppGlobal.thisEmployee.ID);
                }
                else
                {
                    MessageBox.Show("User not found !");
                    return;
                }
            }

            ddStore.Items.Clear();
            foreach (Store s in AppGlobal.thisEmployee.storeList)
            {
                new StoreAccess() { UserID = AppGlobal.thisEmployee.ID, StoreID = s.ID }.Save();
                s.Save();
                ddStore.Items.Add(s);
                ddStore.SelectedIndex = 0;
                ddStore.DisplayMember = "Name";
                btnStart.Text = "Start";
            }

            if (ddStore.Items.Count == 1)
                btnStart_Click(null, EventArgs.Empty);
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            //if (AppGlobal.thisTerminal.StoreID != Cmn.ToInt(ddStore.SelectedValue))
            //{
            //    MessageBox.Show("Not have permission to access this Store!");
            //    return;
            //}
            if (btnStart.Text == "Reset")
            {
                Employee.DeleteEmp((txtEmaild.Text).Trim());
                MessageBox.Show("Employee reset done");
            }
            else
            {
                if (ddStore.SelectedIndex != -1)
                {
                    Hide();
                    Terminal term = Terminal.GetByID(AppGlobal.thisTerminal.ID);
                    if (term != null)
                    {
                        term.User = txtEmaild.Text.ToLower();
                        term.LastAccess = DateTime.Now;
                        term.SoftwareVersion = Application.ProductVersion.ToString();
                        term.Save();
                    }
                    new frmBilling().ShowDialog();
                    Close();
                }
                else
                {
                    MessageBox.Show("Please select store");
                }
            }
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            DatabaseCE.DBPath = @"Data Source=c:\\Billing\\BillingDesktop.sdf;";
            DBCheck.UpdateDB("BillingDesktop");
            AppGlobal.Load();
            AppGlobal.thisCompany.LoadGlobal();
            AppGlobal.thisCompany.LoadItemStorePricing(AppGlobal.thisStore.ID);

            Text = "Login-" + Global.WebServer + " " + Application.ProductVersion.ToString();

            if (File.Exists(lastlogin))
            {
                txtEmaild.Text = File.ReadAllText(lastlogin);
                this.ActiveControl = txtPassword;
            }
        }

        private void txtPassword_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                btnLogin_Click(null, EventArgs.Empty);
            }
        }
    }
}