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
    public partial class Society : Form
    {
        public Project SelectedSociety = null;
        List<Project> listProjects = null;
        string FileName = @"C:\billing\TopList.txt";

        public Society()
        {
            InitializeComponent();
        }

        private void Society_Load(object sender, EventArgs e)
        {
            LoadList(Project.GetAll());
        }

        private void txtSearch_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
                SelectProject();
            else
            {
                string txt = txtSearch.Text.ToLower();
                List<Project> list = listProjects.Where(m => m.ProjectNameLower.StartsWith(txt)).ToList();
                if (list.Count == 0)
                    list = listProjects;

                lstSociety.DataSource = list;
            }
        }

        private void lstSociety_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        void LoadList(List<Project> list)
        {
            listProjects = list;
            if (File.Exists(FileName))
            {
                string[] SocityList = File.ReadAllLines(FileName);
                int Rank = 99999;
                foreach (string s in SocityList)
                {
                    Project p = listProjects.FirstOrDefault(m => m.ProjectName == s);
                    if (p != null)
                    {
                        p.Rank = Rank;
                        Rank--;
                    }
                }
            }
            listProjects = listProjects.OrderByDescending(m => m.Rank).ToList();
            lstSociety.DataSource = listProjects;
            lstSociety.DisplayMember = "ProjectName";
            txtSearch.Focus();

        }
        private void btnUpdateListFromServer_Click(object sender, EventArgs e)
        {
            try
            {
                List<Project> list = (List<Project>)WebData.Data(WebDataAction.GetSociety);
                Project.Save(list);
                LoadList(list);

                MessageBox.Show("Done");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void SelectProject()
        {
            SelectedSociety = (Project)lstSociety.SelectedItem;
            Hide();
        }

        private void lstSociety_Click(object sender, EventArgs e)
        {
            SelectProject();
        }

        private void lstSociety_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
                SelectProject();
        }

        private void btnTopList_Click(object sender, EventArgs e)
        {
            
            if(!File.Exists(FileName))
                File.WriteAllText(FileName,"");

            Help.ShowHelp(this,FileName);
        }
    }
}
