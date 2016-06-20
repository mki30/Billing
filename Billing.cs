using BillingLib;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using System.Net;

namespace BillingSoft
{
    public partial class frmBilling : Form
    {
        Billing Bill = new Billing();
        public Boolean IsOnline = false;
        public Boolean IsCategorySelected = false;
        List<Item> itemByCategory = new List<Item>();

        enum SearchMode
        {
            PLU,
            BillNumber,
            Qty,
            ItemRemove,
            ItemSelect,
            Price,
            PaymentCash,
            PaymentCard,
            Discount,
            DiscountOnItem,
            DiscountOnBill,
        }

        SearchMode searchMode = SearchMode.PLU;
        DateTime LastTerminalPingToServer = DateTime.Now.AddMinutes(-10);

        Boolean FirstLoad = true;

        public frmBilling()
        {
            InitializeComponent();
        }

        private void txtItemSearch_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Alt && e.KeyCode == Keys.M)
            {
                txtMobile.Focus();
            }
            if (e.KeyValue == 13)
                ProcessEnterKey();
            else
                ProcessSearch();
        }

        void ProcessSearch()
        {
            switch (searchMode)
            {
                case SearchMode.PLU: ShowPLUSearchResult(txtItemSearch.Text); break;
                case SearchMode.PaymentCash: Bill.PaidCash = Cmn.ToDbl(txtItemSearch.Text); break;
                case SearchMode.PaymentCard: Bill.PaidCard = Cmn.ToDbl(txtItemSearch.Text); break;
            }
            UpdateBill();
        }

        void ShowPLUSearchResult(string txt)
        {
            Boolean IsNumeric = Regex.IsMatch(txt, @"\d");

            txt = txt.ToLower();
            listViewSearchResult.View = View.Details;
            listViewSearchResult.GridLines = true;
            listViewSearchResult.FullRowSelect = true;

            //Add column header
            listViewSearchResult.Columns.Clear();
            listViewSearchResult.Columns.Add("PLU");
            listViewSearchResult.Columns.Add("Name");
            listViewSearchResult.Columns.Add("MRP");
            listViewSearchResult.Items.Clear();

            if (txt.Length < 2)
                return;

            List<Item> NewItemList = new List<Item>();
            if (IsCategorySelected)
                NewItemList = itemByCategory;
            else
                NewItemList = (IsNumeric ? AppGlobal.thisCompany.ItemList.Where(m => m.PLU.StartsWith(txt)) : AppGlobal.thisCompany.ItemList.Where(m => m.NameLower.Contains(txt))).Take(24).ToList();
            foreach (Item i in NewItemList)
            {
                //Add items in the listview
                string[] arr = new string[4];
                ListViewItem itm;

                //Add first item
                arr[0] = i.PLU;
                arr[1] = i.Name;
                arr[2] = i.MRP.ToString();
                itm = new ListViewItem(arr);
                listViewSearchResult.Items.Add(itm);

            }

            listViewSearchResult.Columns[0].AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
            listViewSearchResult.Columns[1].AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
            listViewSearchResult.Columns[2].AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
        }

        private void btnBillSearch_Click(object sender, EventArgs e)
        {
            UpdateSearchMode(SearchMode.BillNumber);
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            NewBill(true);
        }

        void NewBill(Boolean ShowAlert = false)
        {
            if (ShowAlert == false || MessageBox.Show("Clear the current bill ?", "Clear Bill", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
            {
                Bill = new Billing();
                btnFinish.BackColor = Color.White;
                UpdateSearchMode(SearchMode.PLU);
                UpdateBill();
            }

        }

        void UpdateSearchMode(SearchMode mode)
        {
            searchMode = mode;
            UpdateButtonNumbers();
            switch (mode)
            {
                case SearchMode.Discount: lblInfo.Text = "Select Discount On "; UpdateDiscountType(); break;
                case SearchMode.DiscountOnItem: lblInfo.Text = "Enter Discount On Item"; break;
                case SearchMode.DiscountOnBill: lblInfo.Text = "Enter Discount On Bill"; break;

                case SearchMode.PaymentCard: lblInfo.Text = "Take Card Payment"; UpdateCardType(); break;
                case SearchMode.PaymentCash: lblInfo.Text = "Enter Cash Received From Customer"; UpdateButtonNumbers(false); break;
                case SearchMode.Price: lblInfo.Text = "Enter Item Price"; break;
                case SearchMode.Qty: lblInfo.Text = "Enter Item Quantity"; break;
                case SearchMode.ItemSelect: lblInfo.Text = "Enter Item Number To Select"; break;
                case SearchMode.PLU: lblInfo.Text = "Enter Next Item PLU"; break;
                case SearchMode.BillNumber: lblInfo.Text = "Enter Bill Number"; break;
                case SearchMode.ItemRemove: lblInfo.Text = "Enter Item Number in bill"; break;
            }
            txtItemSearch.Focus();
            txtItemSearch.Text = "";

            if (mode == SearchMode.PaymentCard)
                txtItemSearch.Text = (Bill.TotalAmount - Bill.PaidCash).ToString();

            UpdateBill();
        }

        void UpdateDiscountType()
        {
            for (int i = 1; i < 10; i++)
            {
                Control[] bt = Controls.Find("btn" + i.ToString(), true);
                (bt[0] as Button).Text = "";
            }

            for (int i = 1; i <= 3; i++)
            {
                Control[] bt = Controls.Find("btn" + i.ToString(), true);
                switch (i)
                {
                    case 1: (bt[0] as Button).Text = "Item"; break;
                    case 2: (bt[0] as Button).Text = "Bill"; break;
                }
            }
        }

        void UpdateCardType()
        {
            for (int i = 1; i < 10; i++)
            {
                Control[] bt = Controls.Find("btn" + i.ToString(), true);
                (bt[0] as Button).Text = "";
            }

            for (int i = 1; i <= 3; i++)
            {
                Control[] bt = Controls.Find("btn" + i.ToString(), true);
                (bt[0] as Button).Text = ((CardType)i).ToString();
            }
        }

        void UpdateButtonNumbers(Boolean Numbers = true)
        {
            int[] Cur = { 1000, 500, 100, 50, 20, 10, 5, 2, 1 };
            for (int i = 1; i < 10; i++)
            {
                Control[] bt = Controls.Find("btn" + i.ToString(), true);
                (bt[0] as Button).Text = (Numbers ? i : Cur[i - 1]).ToString();
            }

        }

        void UpdateBill(Graphics gr = null)
        {
            if (AppGlobal.thisTerminal == null || AppGlobal.thisStore == null || AppGlobal.thisCompany == null)
                return;

            Boolean IsPictureBox = false;
            //Bitmap bmp=null;

            if (gr == null)
            {
                IsPictureBox = true;
                gr = pictBill.CreateGraphics();
                //bmp = new Bitmap(80, pictBill.Height);
                //gr = Graphics.FromImage(bmp);
                lblTotalAmount.Text = "0";
            }

            if (Bill != null)
            {
                Customer c = Bill.customer;
                txtMobile.Text = c.Mobile;
                txtCustomer.Text = c.Name;
                txtHouseNumber.Text = c.HouseNumber;
                txtAddress.Text = c.Address;
                txtArea.Text = c.Area;
                txtCity.Text = c.City;
                txtPincode.Text = c.PIN.ToString();
                btnCOD.BackColor = Bill.Delivery ? Color.Green : Color.White;

                if (IsPictureBox)
                    lblTotalAmount.Text = Bill.TotalAmount.ToString("0");

                gr.PageUnit = GraphicsUnit.Millimeter;
                int Width = 72;//80 mm

                gr.Clear(Color.White);

                float Y = 0, X = 1;
                FontInfo.FontSize = 12;
                Y += BillPrint.PrintTxt(gr, "PURE PROTEIN PVT LTD", X, Y, Width, TextAlign.Center, true).Height;
                FontInfo.FontSize = 9;

                //string[] strLine = { thisCompany.TIN "TIN-09888829877C", "F1, ATS One Hamlet, Sec 104, Noida", "0120-7183559, 9999251914, 9999281915", "http://pureprotein.co.in", "Retail Invoice" + (Bill.Duplicate ? " - Duplicate Bill" : "") + (Bill.Delivery ? " - Delivery" : "") };
                string[] strLine = { "TIN-" + AppGlobal.thisCompany.TIN, AppGlobal.thisStore.Address1, AppGlobal.thisStore.Phone, AppGlobal.thisStore.Address2, "http://pureprotein.co.in", "Retail Invoice" + (Bill.Duplicate ? " - Duplicate Bill" : "") + (Bill.Delivery ? " - Delivery" : "") };

                for (int i = 0; i < strLine.Length; i++)
                    Y += BillPrint.PrintTxt(gr, strLine[i], X, Y, Width, TextAlign.Center, i == 0).Height;

                Y += 1;
                BillPrint.Line(gr, 0, Y, Width + 2);
                Y += 1;

                if (txtMobile.Text.Length > 0)
                {
                    Y += BillPrint.PrintTxt(gr, ("Customer : " + Bill.customer.Name + ", " + Bill.customer.Mobile).ToUpper(), X, Y, Width, TextAlign.Left, true).Height;
                    Y += BillPrint.PrintTxt(gr, (Bill.customer.HouseNumber + ", " + Bill.customer.Address + ", " + Bill.customer.Area + ", " + Bill.customer.City + (Bill.customer.PIN != 0 ? ", " + Bill.customer.PIN : "")).ToUpper(), X, Y, Width, TextAlign.Left, true).Height;
                    Y += 1;
                    BillPrint.Line(gr, 0, Y, Width + 2);

                }

                Y += 1;
                BillPrint.PrintTxt(gr, "Bill No:" + AppGlobal.thisTerminal.ID.ToString("000") + "-" + Bill.BillNo.ToString("00000"), X, Y);
                Y += BillPrint.PrintTxt(gr, Bill.BillDate.ToString("dd-MMM-yyyy HH:mm"), (int)(Width * .5), Y, (int)(Width * .5), TextAlign.Right).Height + 1;


                int ctr = 0;

                BillPrint.Line(gr, 0, Y, Width + 2);
                Y += 1;

                int[] Cols = { 1, 4, 35, 48, 59 };

                BillPrint.PrintTxt(gr, "#", Cols[0], Y);
                BillPrint.PrintTxt(gr, "Description", Cols[1], Y);
                BillPrint.PrintTxt(gr, "Qty", Cols[2], Y, 14, TextAlign.Right);
                BillPrint.PrintTxt(gr, "Rate", Cols[3], Y, 10, TextAlign.Right);
                Y += BillPrint.PrintTxt(gr, "Amt.", Cols[4], Y, Width - Cols[4], TextAlign.Right).Height + 1;
                BillPrint.Line(gr, 0, Y, Width + 2);
                Y += 1;

                foreach (BillingItem i in Bill.ItemList)
                {
                    ctr++;
                    X = 0;

                    int TxtW = 38;// i.Quantity == (int)i.Quantity ? 51 : 46;

                    string ItemName = (ctr > 9 ? "  " : "") + Cmn.ProperCase(i.Name) + (i.BillItemDiscount > 0 ? " @ " + i.BillItemDiscount + "% Disc." : "");

                    float H = BillPrint.TextSize(gr, ItemName, TxtW).Height; // to get row height

                    if (i == Bill.SelectedItem && IsPictureBox)
                        BillPrint.Box(gr, 0, Y, Width, H);

                    BillPrint.PrintTxt(gr, ctr + ".", Cols[0], Y);
                    BillPrint.PrintTxt(gr, ItemName, Cols[1], Y, TxtW);
                    BillPrint.PrintTxt(gr, i.Quantity == (int)i.Quantity ? ((int)i.Quantity).ToString() : i.Quantity.ToString("0.000"), Cols[2], Y, 14, TextAlign.Right);


                    BillPrint.PrintTxt(gr, i.MRP.ToString("0"), Cols[3], Y, 10, TextAlign.Right);

                    BillPrint.PrintTxt(gr, i.Amount.ToString("0.00") + (i.DiscountAmount > 0 ? " " + i.DiscountAmount.ToString("0.00") : ""), Cols[4], Y, Width - Cols[4], TextAlign.Right);

                    Y += H + 2;
                }
                BillPrint.Line(gr, 0, Y, Width + 2);

                Y += 1;
                BillPrint.PrintTxt(gr, "Totals", X, Y);
                BillPrint.PrintTxt(gr, Bill.TotalQuantity.ToString("0.000"), Cols[2], Y, 14, TextAlign.Right, true);
                Y += BillPrint.PrintTxt(gr, Bill.TotalAmountNoRoundOff.ToString("0.00"), Cols[4] - 10, Y, Width - Cols[4] + 10, TextAlign.Right).Height;

                if (Bill.TotalDiscount > 0)
                {
                    Y += BillPrint.PrintTxt(gr, "Discount   " + Bill.TotalDiscount, Width * .5f, Y, Width * .5f, TextAlign.Right, true).Height;
                }
                //BillPrint.PrintTxt(gr, "Vat 5%:0000", X, Y);
                Y += BillPrint.PrintTxt(gr, "Net Amt (R/O)   " + Bill.TotalAmount, Width * .5f, Y, Width * .5f, TextAlign.Right, true).Height;
                //BillPrint.PrintTxt(gr, "Vat 14%:0000", X, Y);

                double Cash = Bill.PaidCash == 0 && Bill.PaidCard == 0 ? Bill.TotalAmount : Bill.PaidCash;

                Y += BillPrint.PrintTxt(gr, "Cash Payment " + Cash, Width * .3f, Y, Width * .7f, TextAlign.Right, true).Height + 1;

                if (Bill.PaidCard != 0)
                    Y += BillPrint.PrintTxt(gr, Bill.cardType + " Card Payment " + Bill.PaidCard, Width * .3f, Y, Width * .7f, TextAlign.Right, true).Height + 1;

                double Change = Bill.TotalAmount - Cash - Bill.PaidCard;
                lblChange.Text = Change < 0 ? Change.ToString("0") : "";

                if (Change < 0)
                    Y += BillPrint.PrintTxt(gr, "Refund " + Change, Width * .3f, Y, Width * .7f, TextAlign.Right, true).Height + 1;

                BillPrint.Line(gr, 0, Y, Width + 2);

                Y += BillPrint.PrintTxt(gr, "*PRICES ARE INCLUSIVE OF VAT*", X, Y).Height;
                Y += BillPrint.PrintTxt(gr, "No refund.Only Exchange", X, Y).Height;
                Y += BillPrint.PrintTxt(gr, "*FREE HOME DELIVERY*  *HAVE A NICE DAY*", X, Y).Height;
                Y += BillPrint.PrintTxt(gr, "Cashier : " + AppGlobal.thisEmployee.Name.Split(' ')[0], X, Y).Height;
            }

            if (IsPictureBox)
            {
                //if(pictBill.Image !=null)
                //    pictBill.Image.Dispose();

                //pictBill.Image = bmp;
            }
        }

        private void frmBilling_Load(object sender, EventArgs e)
        {
            UpdateSearchMode(SearchMode.PLU);
            Text = "Pure Protien Ver :" + Application.ProductVersion.ToString();
        }

        void CheckIsOnline()
        {
            try
            {
                WebClient wc = new WebClient();
                wc.DownloadStringCompleted += wc_DownloadStringCompleted;
                wc.DownloadStringAsync(new Uri(Global.WebServer + "Data.aspx"));
            }
            catch
            {

            }
        }

        void wc_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error == null && e.Result == "~")
            {
                IsOnline = true;
                UpdateData();
            }
            //else
            //   MessageBox.Show(e.Error.Message);
        }


        void SendRecordsToServer(Boolean IsFurtureTable = false)
        {
            List<Billing> list = Billing.GetAll(IsFurtureTable, DateTime.Now.AddMonths(-1), DateTime.Now);
            foreach (Billing b in list.OrderBy(m => m.BillNo))
            {
                Billing bill = Billing.GetByID(b.ID, 0, 0, 0, IsFurtureTable);
                bill.ID = 0;
                foreach (BillingItem i in bill.ItemList)
                {
                    i.ID = 0; //  to create a new record on the server
                }

                //update the local bill on the server
                bill = (Billing)WebData.Data(WebDataAction.SetBill, bill);
                if (bill.ID != 0)
                    b.Delete(IsFurtureTable);
            }
        }
        void UpdateData()
        {
            if (!IsOnline)
                return;

            (new Thread(() =>
            {
                try
                {
                    SendRecordsToServer(); //Normal Bill
                    SendRecordsToServer(true); //future bill

                    // save only when local data does not exists
                    // the machine need to moved to new terminal then delete the local database and do a fresh install
                    Terminal term = Terminal.GetByID(AppGlobal.thisTerminal.ID);
                    if (term == null)
                    {
                        term = (Terminal)WebData.Data(WebDataAction.GetTerminal, AppGlobal.thisTerminal);
                        AppGlobal.thisTerminal = term;
                        AppGlobal.thisTerminal.Save();
                    }
                    else
                    {
                        term.LastAccess = DateTime.Now;
                        term.Save();
                        WebData.Data(WebDataAction.SetTerminal, term);
                    }

                    if (FirstLoad)
                    {
                        FirstLoad = false;

                        if (term != null)
                        {
                            Store store = (Store)WebData.Data(WebDataAction.GetStore, new Store() { ID = term.StoreID });
                            if (store != null)
                            {
                                store.Save();
                                AppGlobal.thisStore = store;

                                Company comp = (Company)WebData.Data(WebDataAction.GetCompany, new Company() { ID = store.CompanyID });
                                if (comp != null)
                                {
                                    comp.Save();
                                    AppGlobal.thisCompany = comp;
                                    AppGlobal.thisCompany.LoadGlobal();
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    File.AppendAllText(@"C:\Billing\Error.txt", DateTime.Now + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace);
                }

            })).Start();

        }

        private void btnItemRemove_Click(object sender, EventArgs e)
        {
            if (Bill.SelectedItem != null)
            {
                Bill.ItemList.Remove(Bill.SelectedItem);

                if (Bill.ItemList.Count > 0)
                    Bill.SelectedItem = Bill.ItemList[Bill.ItemList.Count - 1];

                UpdateSearchMode(SearchMode.PLU);
            }
        }

        private void btnEnter_Click(object sender, EventArgs e)
        {
            ProcessEnterKey();
        }

        void ProcessEnterKey()
        {

            if (Bill.Finished)
            {
                MessageBox.Show("Bill finished, No new item can not be added!");
                return;
            }

            var confirmResult = DialogResult.No;
            switch (searchMode)
            {
                case SearchMode.PaymentCash:
                    Bill.PaidCash = Cmn.ToInt(txtItemSearch.Text);
                    UpdateSearchMode(SearchMode.PLU);
                    break;
                case SearchMode.PaymentCard:
                    Bill.PaidCard = Cmn.ToInt(txtItemSearch.Text);
                    UpdateSearchMode(SearchMode.PLU);
                    break;
                case SearchMode.Price:
                    if (Bill.SelectedItem != null)
                        Bill.SelectedItem.MRP = Cmn.ToDbl(txtItemSearch.Text);
                    UpdateSearchMode(SearchMode.PLU);
                    break;

                case SearchMode.Qty:
                    if (Bill.SelectedItem != null)
                        if (Cmn.ToDbl(txtItemSearch.Text) > 10)
                        {
                            confirmResult = MessageBox.Show("Are you sure qunatity is " + txtItemSearch.Text, "Confirm", MessageBoxButtons.YesNo);
                            if (confirmResult == DialogResult.No)
                                break;
                        }
                    Bill.SelectedItem.Quantity = Cmn.ToDbl(txtItemSearch.Text);
                    UpdateSearchMode(SearchMode.PLU);
                    break;
                case SearchMode.ItemSelect:
                    int Index = Cmn.ToInt(txtItemSearch.Text) - 1;
                    if (Index >= 0 && Index < Bill.ItemList.Count)
                    {
                        Bill.SelectedItem = Bill.ItemList[Index];
                        UpdateBill();
                    }
                    break;

                case SearchMode.DiscountOnItem:
                    if (Bill.SelectedItem != null)
                    {
                        Bill.SelectedItem.BillItemDiscount = Cmn.ToDbl(txtItemSearch.Text);
                        UpdateSearchMode(SearchMode.PLU);
                    }
                    else
                        MessageBox.Show("No item found in the bill");
                    break;

                case SearchMode.DiscountOnBill:

                    foreach (BillingItem bi in Bill.ItemList)
                    {
                        bi.BillItemDiscount = Cmn.ToDbl(txtItemSearch.Text);
                    }
                    UpdateSearchMode(SearchMode.PLU);
                    break;

                case SearchMode.PLU:
                    {
                        string PLU = txtItemSearch.Text;
                        Item i = AppGlobal.thisCompany.GetItem(PLU);

                        if (i != null && !string.IsNullOrWhiteSpace(i.PLU))
                        {
                            BillingItem item = new BillingItem()
                                {
                                    PLU = i.PLU,
                                    ItemID = i.ID,
                                    Cost = i.Cost,
                                    Name = i.Name,
                                    TaxRate = i.TaxRate,
                                    Quantity = i.Quantity,
                                    MRP = i.MRP,
                                };
                            Bill.ItemList.Add(item);
                            Bill.SelectedItem = item;
                        }
                        else
                            MessageBox.Show("Item not found");

                        UpdateSearchMode(SearchMode.PLU);
                    }
                    break;
                case SearchMode.BillNumber:


                    try
                    {
                        Billing tempBill = new Billing() { BillNo = Cmn.ToInt(txtItemSearch.Text), store = AppGlobal.thisStore, TerminalID = AppGlobal.thisTerminal.ID };
                        if (IsOnline)
                        {
                            tempBill = (Billing)WebData.Data(WebDataAction.GetBill, tempBill);
                            if (tempBill != null)
                            {
                                if (tempBill.Error.Length > 0)
                                {
                                    MessageBox.Show(tempBill.Error);
                                }
                                else
                                {
                                    Bill = tempBill;
                                    Bill.BillDate = Bill.BillDate.AddHours(5.5);
                                    tempBill.Finished = true;
                                    tempBill.Duplicate = true;
                                    UpdateBill();
                                }
                            }

                            UpdateSearchMode(SearchMode.PLU);
                        }
                        else
                        {
                            MessageBox.Show("Can not show bill, system not online");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    break;
                case SearchMode.ItemRemove:
                    if (Bill != null)
                    {
                        int i = Cmn.ToInt(txtItemSearch.Text);
                        if (i > 0 && i <= Bill.ItemList.Count)
                            Bill.ItemList.RemoveAt(i - 1);
                        UpdateSearchMode(SearchMode.PLU);
                    }
                    break;
            }
        }

        private void btnItemAdd_Click(object sender, EventArgs e)
        {
            UpdateSearchMode(SearchMode.Discount);
        }

        private void btnPay_Click(object sender, EventArgs e)
        {
            UpdateSearchMode(SearchMode.PaymentCash);
        }


        private void listViewSearchResult_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewSearchResult.SelectedItems.Count > 0)
                txtItemSearch.Text = listViewSearchResult.SelectedItems[0].SubItems[0].Text;

            if (IsCategorySelected)
            {
                int MenueID = Cmn.ToInt(listViewSearchResult.SelectedItems[0].SubItems[1].Text);
                List<ItemMenuLink> imlList = ItemMenuLink.GetByMenuID(MenueID);

                itemByCategory.Clear();
                foreach (ItemMenuLink iml in imlList)
                {
                    Item item = AppGlobal.thisCompany.ItemList.FirstOrDefault(m => m.ID == iml.ItemID);
                    if (item != null)
                        itemByCategory.Add(item);
                }
                ShowPLUSearchResult("CATAGORY");
                IsCategorySelected = false;
            }
        }

        private void btnNumber_Click(object sender, EventArgs e)
        {
            if (searchMode == SearchMode.PaymentCash)
            {
                txtItemSearch.Text = (Cmn.ToInt((sender as Button).Text) + Cmn.ToInt(txtItemSearch.Text)).ToString();
            }
            else if (searchMode == SearchMode.PaymentCard)
            {
                txtItemSearch.Text = txtItemSearch.Text;
                Bill.cardType = (CardType)Cmn.ToInt((sender as Button).Tag);
            }
            else if (searchMode == SearchMode.Discount)
            {
                switch ((sender as Button).Tag.ToString())
                {
                    case "2": UpdateSearchMode(SearchMode.DiscountOnBill); break;
                    default:
                        UpdateSearchMode(SearchMode.DiscountOnItem); break;
                }
            }
            else
            {
                if ((sender as Button).Text == "-")
                    txtItemSearch.Text = (sender as Button).Text + txtItemSearch.Text;
                else
                    txtItemSearch.Text += (sender as Button).Text;
            }

            ProcessSearch();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                PrintDocument pd = new PrintDocument();

                //pd.DefaultPageSettings.PaperSize = new PaperSize("My Paper", 80, 240);
                pd.PrintPage += new PrintPageEventHandler
                   (this.pd_PrintPage);
                pd.Print();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // The PrintPage event is raised for each page to be printed. 
        private void pd_PrintPage(object sender, PrintPageEventArgs ev)
        {
            UpdateBill(ev.Graphics);
            ev.HasMorePages = false;
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            UpdateSearchMode(SearchMode.ItemSelect);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            UpdateSearchMode(SearchMode.Qty);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            UpdateSearchMode(SearchMode.Price);
        }

        private void txtMobile_KeyUp(object sender, KeyEventArgs e)
        {
            lblMobile.Text = "&Mob.\n" + txtMobile.Text.Length;
            if (e.KeyCode == Keys.Return)
                GetCustomer(txtMobile.Text, "", "");

        }

        void GetCustomer(string MobileNo, string HouseNo, string Name)
        {

            try
            {
                List<Customer> listCust = new List<Customer>();
                if (MobileNo != "")
                    listCust = new Customer() { Mobile = txtMobile.Text }.Find();
                if (Name != "")
                    listCust = new Customer { Name = txtCustomer.Text }.Find();
                if (HouseNo != "")
                    listCust = new Customer { HouseNumber = txtHouseNumber.Text }.Find();

                if (IsOnline)
                {
                    if (txtMobile.Text != "" && listCust.Count == 0)
                        listCust = (List<Customer>)WebData.Data(WebDataAction.GetCustomer, new Customer() { Mobile = txtMobile.Text });
                    UpdateSearchMode(SearchMode.PLU);
                }

                if (listCust.Count > 0)
                    Bill.customer = listCust[0];

                UpdateBill();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void txt_Enter(object sender, EventArgs e)
        {
            TextBox txt = sender as TextBox;
            txt.BackColor = Color.Pink;

            switch (txt.Name)
            {
                case "txtMobile": lblInfo.Text = "Enter Customer Mobile Number + Enter to Search"; break;
                case "txtCustomer": lblInfo.Text = "Enter Customer Name"; break;
                case "txtHouseNumber": lblInfo.Text = "Enter House number"; break;
                case "txtAddress": lblInfo.Text = "Enter Address"; break;
                case "txtArea": lblInfo.Text = "Enter Area"; break;
                case "txtCity": lblInfo.Text = "Enter City"; break;
                case "txtPincode": lblInfo.Text = "Enter Pincode"; break;
            }
            UpdateBill();
        }

        private void txt_Leave(object sender, EventArgs e)
        {
            TextBox txt = sender as TextBox;
            txt.BackColor = Color.White;
            txt.Text = Cmn.ProperCase(txt.Text);

            Customer c = Bill.customer;
            switch (txt.Name)
            {
                case "txtMobile": c.Mobile = txt.Text; break;
                case "txtCustomer": c.Name = txt.Text; break;
                case "txtHouseNumber": c.HouseNumber = txt.Text; break;
                case "txtAddress": c.Address = txt.Text; break;
                case "txtArea": c.Area = txt.Text; break;
                case "txtCity": c.City = txt.Text; break;
                case "txtPincode":
                    c.PIN = Cmn.ToInt(txt.Text);
                    UpdateSearchMode(SearchMode.PLU);
                    break;
            }
            UpdateBill();
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            toolStripStatusTime.Text = DateTime.Now.ToString("dd-MMM-yyyy HH.mm.ss");
            toolStripStatusServer.Text = "Server : " + Global.WebServer;

            if (AppGlobal.thisTerminal != null)
                toolStripStatusTerminal.Text = "Terminal : " + AppGlobal.thisTerminal.ID + "-" + AppGlobal.thisTerminal.Name
                + ", Company : " + AppGlobal.thisTerminal.CompanyID + "-" + AppGlobal.thisTerminal.Name
                + ", Store : " + AppGlobal.thisTerminal.StoreID + "-" + AppGlobal.thisTerminal.Name
                + ", BillNo : " + AppGlobal.thisTerminal.BillNo;
            else
                toolStripStatusTerminal.Text = " Terminal Information not found ";

            toolStripStatusOnline.Text = IsOnline ? "Online" : "Offline";
            toolStripStatusOnline.ForeColor = IsOnline ? Color.Green : Color.Red;
            btnPrint.Enabled = Bill.Finished;
            btnFinish.Enabled = !Bill.Finished;

            if ((DateTime.Now - LastTerminalPingToServer).TotalMinutes > 5)
            {
                LastTerminalPingToServer = DateTime.Now;
                CheckIsOnline();
            }
        }

        private void btnFinish_Click(object sender, EventArgs e)
        {
            btnFinish.Enabled = false;
            if (Bill.ItemList.Count > 0 && Bill.Finished == false)
            {
                // to get the current bill number, more than 2 instances can be run on the same machine to do parallel billing
                // get the latest bill number use it by incrementing it and save it back so that It can be used by another application on the terminal
                AppGlobal.thisTerminal = Terminal.GetByID(AppGlobal.thisTerminal.ID);

                //Bill.AmounPaidByCustomer = Bill.TotalAmount;
                string FinYear = Cmn.FinStartDate.ToString("yy") + Cmn.FinEndDate.ToString("yy");
                if (AppGlobal.thisTerminal.BillNo.ToString("0").Substring(0, 4) != FinYear)
                    AppGlobal.thisTerminal.BillNo = Cmn.ToInt(FinYear + "000000");

                Bill.BillNo = ++AppGlobal.thisTerminal.BillNo;
                AppGlobal.thisTerminal.Save();

                Bill.company.ID = AppGlobal.thisTerminal.CompanyID;
                Bill.TerminalID = AppGlobal.thisTerminal.ID;
                Bill.store.ID = AppGlobal.thisTerminal.StoreID;

                Bill.Save();
                if (Bill.Error != "OK")
                {
                    MessageBox.Show(Bill.Error);
                }

                Bill.Finished = true;
                btnFinish.BackColor = Color.Green;
                btnFutureOrder.BackColor = Color.LightBlue;

                UpdateSearchMode(SearchMode.PLU);
                UpdateBill();
                btnPrint_Click(sender, e);
                NewBill();
                CheckIsOnline();
            }
            else
            {
                UpdateSearchMode(SearchMode.PLU);
                MessageBox.Show("No Items found!");
            }

            btnFinish.Enabled = true;
        }

        private void btnCOD_Click(object sender, EventArgs e)
        {
            ShowDeliverySelection();
            if (Bill != null)
            {
                Bill.Delivery = !Bill.Delivery;
                UpdateBill();
            }
        }

        private void btnPayCard_Click(object sender, EventArgs e)
        {
            UpdateSearchMode(SearchMode.PaymentCard);
        }

        private void btnData_Click(object sender, EventArgs e)
        {
            btnData.Enabled = false;

            if (IsOnline)
            {
                try
                {
                    List<Item> list = (List<Item>)WebData.Data(WebDataAction.GetItems, null);
                    if (list != null)
                    {
                        Item.UpdateAll(list);
                        AppGlobal.thisCompany.ItemList = list;
                        MessageBox.Show(list.Count + " Items Imported");
                    }
                    else
                    {
                        MessageBox.Show("Error importing items");
                    }

                    List<ItemStorePrice> listStorePrice = (List<ItemStorePrice>)WebData.Data(WebDataAction.GetItemStorePrice, null);
                    if (listStorePrice != null)
                    {
                        ItemStorePrice.UpdateAll(listStorePrice);
                        AppGlobal.thisCompany.LoadItemStorePricing(AppGlobal.thisStore.ID);

                    }
                    else
                        MessageBox.Show("Error importing items store pricing");


                    List<BillingLib.Menu> listMenu = (List<BillingLib.Menu>)WebData.Data(WebDataAction.GetMenues, null);
                    if (listMenu != null)
                    {
                        BillingLib.Menu.UpdateAll(listMenu);
                        AppGlobal.thisCompany.listMenu = BillingLib.Menu.GetAll(0, true);
                    }
                    else
                        MessageBox.Show("Error importing menu category");

                    List<ItemMenuLink> listItemMenuLink = (List<ItemMenuLink>)WebData.Data(WebDataAction.GetItemMenueLink, null);
                    if (listItemMenuLink != null)
                    {
                        ItemMenuLink.UpdateAll(listItemMenuLink);
                    }
                    else
                        MessageBox.Show("Error importing item menu links");


                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
                MessageBox.Show("System offline, can not update!");

            UpdateSearchMode(SearchMode.PLU);
            btnData.Enabled = true;
        }

        private void txtHouseNumber_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
                GetCustomer("", txtHouseNumber.Text, "");
        }

        private void txtCustomer_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
                GetCustomer("", "", txtCustomer.Text);
        }

        private void panel1_Scroll(object sender, ScrollEventArgs e)
        {
            UpdateBill();
        }

        private void txtAddress_Click(object sender, EventArgs e)
        {
            ShowSocietySelection();
        }

        void ShowSocietySelection()
        {
            using (Society frm = new Society())
            {
                frm.ShowDialog();
                if (frm.SelectedSociety != null)
                {
                    Customer c = Bill.customer;
                    c.Address = txtAddress.Text = frm.SelectedSociety.ProjectName;
                    c.PIN = frm.SelectedSociety.Pin;
                    txtPincode.Text = c.PIN.ToString();
                    c.Area = txtArea.Text = frm.SelectedSociety.Area;
                    c.City = txtCity.Text = frm.SelectedSociety.City;
                    c.ProjectID = frm.SelectedSociety.ID;
                    UpdateSearchMode(SearchMode.PLU);
                }
            }
        }

        private void txtAddress_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
                ShowSocietySelection();
        }

        void ShowDeliverySelection()
        {
            using (Delivery frm = new Delivery())
            {
                if (Bill.ItemList.Count > 0)
                {
                    frm.ShowDialog();
                    if (frm.deliveryMan != "")
                    {
                        Bill.DeliveryBy = frm.deliveryMan;
                    }
                }
                else
                    MessageBox.Show("No item found in the bill");
            }
        }

        private void btnItemCalegory_Click(object sender, EventArgs e)
        {
            IsCategorySelected = true;

            listViewSearchResult.Items.Clear();
            listViewSearchResult.View = View.Details;
            listViewSearchResult.GridLines = true;
            listViewSearchResult.FullRowSelect = true;

            //Add column header
            listViewSearchResult.Columns.Clear();
            listViewSearchResult.Columns.Add("Category");

            List<BillingLib.Menu> menuList = AppGlobal.thisCompany.listMenu;
            foreach (BillingLib.Menu i in menuList)
            {
                //Add items in the listview
                string[] arr = new string[4];
                ListViewItem itm;

                //Add first item
                arr[0] = i.Name;
                arr[1] = i.ID.ToString();
                itm = new ListViewItem(arr);
                listViewSearchResult.Items.Add(itm);
            }
            listViewSearchResult.Columns[0].AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
        }

        private void btnFutureOrder_Click(object sender, EventArgs e)
        {
            using (FutureOrder frm = new FutureOrder())
            {
                if (Bill.customer.Mobile == "" || Bill.customer.Name == "")
                    MessageBox.Show("Please fill customer detail first");
                else
                {
                    if (Bill.ItemList.Count > 0)
                    {
                        frm.ShowDialog();
                        DateTime dt = frm.futureOrderDate;
                        Bill.DeliveryTime = dt;
                        Bill.orderStatus = OrderStatus.Pending;
                        btnFutureOrder.BackColor = System.Drawing.Color.Pink;
                    }
                    else
                        MessageBox.Show("No item found in the bill");
                }
            }
        }

        private void toolStripStatusServer_Click(object sender, EventArgs e)
        {

        }
    }
}