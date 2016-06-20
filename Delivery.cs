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

    public partial class Delivery : Form
    {
        public string deliveryMan = "";
        List<Employee> listDeliveryBoy = null;
        public Delivery()
        {
            InitializeComponent();
        }

        private void Delivery_Load(object sender, EventArgs e)
        {
            listDeliveryBoy = BillingLib.Employee.GetDelievetyMenbyStore(AppGlobal.thisStore.ID);//(List<Employee>)WebData.Data(WebDataAction.GetDeliveryMen, AppGlobal.thisStore);
            LoadList(listDeliveryBoy);
        }
        
        void LoadList(List<Employee> list)
        {
            listDeliveryBoy = list;
            int ctr = 1;
            foreach (Employee E in listDeliveryBoy)
            {
                Button newButton = new Button();
                newButton.Location = new Point(8, newButton.Bottom + (ctr * 60));
                newButton.Text = E.Name;
                newButton.Width = 270;
                newButton.Height = 50;
                newButton.Font = new Font("Arial", 14);
                newButton.BackColor = Color.Wheat;
                this.Controls.Add(newButton);
                ctr++;
                newButton.Click += new EventHandler(this.btnDeliveryClick_Click);
            }
        }

        private void btnDeliveryClick_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            deliveryMan = b.Text;
            Hide();
        }
        private void btnCancle_Click(object sender, EventArgs e)
        {
            Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                Employee.DeleteDeliveryMen();
                List<Employee> list = (List<Employee>)WebData.Data(WebDataAction.GetDeliveryMen, AppGlobal.thisStore);
                Employee.Save(list);
                LoadList(list);
                MessageBox.Show("Done");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
