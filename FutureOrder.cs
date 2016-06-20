using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BillingSoft
{
    public partial class FutureOrder : Form
    {
        public FutureOrder()
        {
            InitializeComponent();
        }

        private void FutureOrder_Load(object sender, EventArgs e)
        {
            DateTime dt = DateTime.Today;

            int top = 10;
            int left = 10;
            for (int i = 0; i < 7; i++)
            {
                Button newButton = new Button();
                newButton.BackColor = SystemColors.Control;
                newButton.Height = 50;
                newButton.Width = 70;
                newButton.Location = new Point(left, top);
                top += newButton.Height + 2;
                newButton.Text = dt.AddDays(i).ToString("dd-MM-yyyy");
                this.Controls.Add(newButton);
                newButton.Click += new EventHandler(this.btnDate_Click);
                if (i == 0)
                    newButton.PerformClick();
            }

            top = 10;
            left = 100;
            DateTime initialTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 10, 0, 0);
            for (int i = 0; i < 12; i++)
            {
                Button newButton = new Button();
                newButton.BackColor = SystemColors.Control;
                newButton.Height = 50;
                newButton.Width = 70;
                newButton.Location = new Point(left, top);
                top += newButton.Height + 2;
                newButton.Text = initialTime.AddHours(i).ToString("HH:mm tt");
                this.Controls.Add(newButton);
                newButton.Click += new EventHandler(this.btnHors_Click);
                if (i == 0)
                    newButton.PerformClick();
            }

            top = 10;
            left = 180;
            for (int i = 0; i < 3; i++)
            {
                Button newButton = new Button();
                newButton.BackColor = SystemColors.Control;
                newButton.Height = 50;
                newButton.Width = 70;
                newButton.Location = new Point(left, top);
                top += newButton.Height + 2;
                newButton.Text = ((i + 1) * 15).ToString("00");
                this.Controls.Add(newButton);
                newButton.Click += new EventHandler(this.btnMinutes_Click);
                //if (i == 0)
                //    newButton.PerformClick();
            }
        }

        public DateTime futureOrderDate = new DateTime();
        int year = DateTime.Now.Year, month = DateTime.Now.Month, day = DateTime.Now.Day, hour = 0, minute = 0;

        Button lastDateButton = null;
        private void btnDate_Click(object sender, EventArgs e)
        {
            if (lastDateButton != null)
                lastDateButton.BackColor = SystemColors.Control;
            Button b = (Button)sender;
            lastDateButton = b;
            b.BackColor = System.Drawing.Color.Pink;
            year = Cmn.ToInt(b.Text.Split('-')[2]);
            month = Cmn.ToInt(b.Text.Split('-')[1]);
            day = Cmn.ToInt(b.Text.Split('-')[0]);
            futureOrderDate = new DateTime(year, month, day);
        }

        Button lastHourButton = null;
        private void btnHors_Click(object sender, EventArgs e)
        {
            if (lastHourButton != null)
                lastHourButton.BackColor = SystemColors.Control;
            Button b = lastHourButton = (Button)sender;
            b.BackColor = System.Drawing.Color.Pink;
            hour = Cmn.ToInt(b.Text.Split(':')[0]);
            futureOrderDate = new DateTime(year, month, day, hour, minute, 0);
        }

        Button lastMinuteButton = null;
        private void btnMinutes_Click(object sender, EventArgs e)
        {
            if (lastMinuteButton != null)
                lastMinuteButton.BackColor = SystemColors.Control;
            Button b = lastMinuteButton = (Button)sender;
            b.BackColor = System.Drawing.Color.Pink;
            minute = Cmn.ToInt(b.Text);
            futureOrderDate = new DateTime(year, month, day, hour, minute, 0);
            Hide();
        }
    }
}
