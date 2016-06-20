namespace BillingSoft
{
    partial class frmBilling
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmBilling));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictBill = new System.Windows.Forms.PictureBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.txtItemSearch = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.lblTotalAmount = new System.Windows.Forms.Label();
            this.btnPayCard = new System.Windows.Forms.Button();
            this.btnPayCash = new System.Windows.Forms.Button();
            this.btnZero = new System.Windows.Forms.Button();
            this.btnDot = new System.Windows.Forms.Button();
            this.btn9 = new System.Windows.Forms.Button();
            this.btn8 = new System.Windows.Forms.Button();
            this.btn7 = new System.Windows.Forms.Button();
            this.btn6 = new System.Windows.Forms.Button();
            this.btn5 = new System.Windows.Forms.Button();
            this.btn4 = new System.Windows.Forms.Button();
            this.btn3 = new System.Windows.Forms.Button();
            this.btn2 = new System.Windows.Forms.Button();
            this.btn1 = new System.Windows.Forms.Button();
            this.btnItemAdd = new System.Windows.Forms.Button();
            this.btnBillSearch = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnMinus = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnQuantity = new System.Windows.Forms.Button();
            this.btnPrice = new System.Windows.Forms.Button();
            this.btnEnter = new System.Windows.Forms.Button();
            this.btnSelect = new System.Windows.Forms.Button();
            this.btnItemRemove = new System.Windows.Forms.Button();
            this.btnCOD = new System.Windows.Forms.Button();
            this.btnFinish = new System.Windows.Forms.Button();
            this.lblChange = new System.Windows.Forms.Label();
            this.btnData = new System.Windows.Forms.Button();
            this.lblInfo = new System.Windows.Forms.Label();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.label4 = new System.Windows.Forms.Label();
            this.txtPincode = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtArea = new System.Windows.Forms.TextBox();
            this.txtMobile = new System.Windows.Forms.TextBox();
            this.txtAddress = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.lblMobile = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblAddress = new System.Windows.Forms.Label();
            this.txtCustomer = new System.Windows.Forms.TextBox();
            this.txtCity = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtHouseNumber = new System.Windows.Forms.TextBox();
            this.txtPoints = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.listViewSearchResult = new System.Windows.Forms.ListView();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.btnFutureOrder = new System.Windows.Forms.Button();
            this.btnItemCalegory = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusTime = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusServer = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusTerminal = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusOnline = new System.Windows.Forms.ToolStripStatusLabel();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictBill)).BeginInit();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 315F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 500F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel4, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.listViewSearchResult, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel5, 2, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 161F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1426, 701);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.pictBill);
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.tableLayoutPanel1.SetRowSpan(this.panel1, 2);
            this.panel1.Size = new System.Drawing.Size(309, 695);
            this.panel1.TabIndex = 6;
            this.panel1.Scroll += new System.Windows.Forms.ScrollEventHandler(this.panel1_Scroll);
            // 
            // pictBill
            // 
            this.pictBill.BackColor = System.Drawing.Color.White;
            this.pictBill.Location = new System.Drawing.Point(0, 3);
            this.pictBill.Name = "pictBill";
            this.pictBill.Size = new System.Drawing.Size(289, 3500);
            this.pictBill.TabIndex = 0;
            this.pictBill.TabStop = false;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel2.AutoSize = true;
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.txtItemSearch, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel3, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.lblInfo, 0, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(318, 164);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 3;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(494, 534);
            this.tableLayoutPanel2.TabIndex = 1;
            // 
            // txtItemSearch
            // 
            this.txtItemSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtItemSearch.BackColor = System.Drawing.Color.White;
            this.txtItemSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtItemSearch.Location = new System.Drawing.Point(3, 28);
            this.txtItemSearch.Name = "txtItemSearch";
            this.txtItemSearch.Size = new System.Drawing.Size(488, 53);
            this.txtItemSearch.TabIndex = 0;
            this.txtItemSearch.Enter += new System.EventHandler(this.txt_Enter);
            this.txtItemSearch.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtItemSearch_KeyUp);
            this.txtItemSearch.Leave += new System.EventHandler(this.txt_Leave);
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel3.ColumnCount = 4;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel3.Controls.Add(this.lblTotalAmount, 3, 6);
            this.tableLayoutPanel3.Controls.Add(this.btnPayCard, 1, 5);
            this.tableLayoutPanel3.Controls.Add(this.btnPayCash, 0, 5);
            this.tableLayoutPanel3.Controls.Add(this.btnZero, 1, 3);
            this.tableLayoutPanel3.Controls.Add(this.btnDot, 0, 3);
            this.tableLayoutPanel3.Controls.Add(this.btn9, 2, 2);
            this.tableLayoutPanel3.Controls.Add(this.btn8, 1, 2);
            this.tableLayoutPanel3.Controls.Add(this.btn7, 0, 2);
            this.tableLayoutPanel3.Controls.Add(this.btn6, 2, 1);
            this.tableLayoutPanel3.Controls.Add(this.btn5, 1, 1);
            this.tableLayoutPanel3.Controls.Add(this.btn4, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.btn3, 2, 0);
            this.tableLayoutPanel3.Controls.Add(this.btn2, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.btn1, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.btnItemAdd, 3, 0);
            this.tableLayoutPanel3.Controls.Add(this.btnBillSearch, 0, 4);
            this.tableLayoutPanel3.Controls.Add(this.btnClear, 1, 4);
            this.tableLayoutPanel3.Controls.Add(this.btnMinus, 2, 3);
            this.tableLayoutPanel3.Controls.Add(this.btnPrint, 2, 4);
            this.tableLayoutPanel3.Controls.Add(this.btnQuantity, 3, 1);
            this.tableLayoutPanel3.Controls.Add(this.btnPrice, 3, 2);
            this.tableLayoutPanel3.Controls.Add(this.btnEnter, 3, 3);
            this.tableLayoutPanel3.Controls.Add(this.btnSelect, 3, 4);
            this.tableLayoutPanel3.Controls.Add(this.btnItemRemove, 3, 5);
            this.tableLayoutPanel3.Controls.Add(this.btnCOD, 2, 5);
            this.tableLayoutPanel3.Controls.Add(this.btnFinish, 0, 6);
            this.tableLayoutPanel3.Controls.Add(this.lblChange, 2, 6);
            this.tableLayoutPanel3.Controls.Add(this.btnData, 1, 6);
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 87);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 7;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28572F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28572F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28572F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28572F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28572F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28572F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28572F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(488, 444);
            this.tableLayoutPanel3.TabIndex = 1;
            // 
            // lblTotalAmount
            // 
            this.lblTotalAmount.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblTotalAmount.AutoSize = true;
            this.lblTotalAmount.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalAmount.ForeColor = System.Drawing.Color.Blue;
            this.lblTotalAmount.Location = new System.Drawing.Point(396, 392);
            this.lblTotalAmount.Name = "lblTotalAmount";
            this.lblTotalAmount.Size = new System.Drawing.Size(89, 37);
            this.lblTotalAmount.TabIndex = 18;
            this.lblTotalAmount.Text = "8888";
            // 
            // btnPayCard
            // 
            this.btnPayCard.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPayCard.BackColor = System.Drawing.Color.DodgerBlue;
            this.btnPayCard.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPayCard.Location = new System.Drawing.Point(125, 318);
            this.btnPayCard.Name = "btnPayCard";
            this.btnPayCard.Size = new System.Drawing.Size(116, 57);
            this.btnPayCard.TabIndex = 17;
            this.btnPayCard.TabStop = false;
            this.btnPayCard.Tag = "1";
            this.btnPayCard.Text = "CA&RD";
            this.toolTip1.SetToolTip(this.btnPayCard, "Card Payment");
            this.btnPayCard.UseVisualStyleBackColor = false;
            this.btnPayCard.Click += new System.EventHandler(this.btnPayCard_Click);
            // 
            // btnPayCash
            // 
            this.btnPayCash.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPayCash.BackColor = System.Drawing.Color.DodgerBlue;
            this.btnPayCash.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPayCash.Location = new System.Drawing.Point(3, 318);
            this.btnPayCash.Name = "btnPayCash";
            this.btnPayCash.Size = new System.Drawing.Size(116, 57);
            this.btnPayCash.TabIndex = 17;
            this.btnPayCash.TabStop = false;
            this.btnPayCash.Tag = "0";
            this.btnPayCash.Text = "&CASH";
            this.toolTip1.SetToolTip(this.btnPayCash, "Cash payment");
            this.btnPayCash.UseVisualStyleBackColor = false;
            this.btnPayCash.Click += new System.EventHandler(this.btnPay_Click);
            // 
            // btnZero
            // 
            this.btnZero.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnZero.BackColor = System.Drawing.Color.LightBlue;
            this.btnZero.Font = new System.Drawing.Font("Microsoft Sans Serif", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnZero.Location = new System.Drawing.Point(125, 192);
            this.btnZero.Name = "btnZero";
            this.btnZero.Size = new System.Drawing.Size(116, 57);
            this.btnZero.TabIndex = 13;
            this.btnZero.TabStop = false;
            this.btnZero.Tag = "0";
            this.btnZero.Text = "0";
            this.btnZero.UseVisualStyleBackColor = false;
            this.btnZero.Click += new System.EventHandler(this.btnNumber_Click);
            // 
            // btnDot
            // 
            this.btnDot.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDot.BackColor = System.Drawing.Color.LightBlue;
            this.btnDot.Font = new System.Drawing.Font("Microsoft Sans Serif", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDot.Location = new System.Drawing.Point(3, 192);
            this.btnDot.Name = "btnDot";
            this.btnDot.Size = new System.Drawing.Size(116, 57);
            this.btnDot.TabIndex = 12;
            this.btnDot.TabStop = false;
            this.btnDot.Tag = ".";
            this.btnDot.Text = ".";
            this.btnDot.UseVisualStyleBackColor = false;
            this.btnDot.Click += new System.EventHandler(this.btnNumber_Click);
            // 
            // btn9
            // 
            this.btn9.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btn9.BackColor = System.Drawing.Color.LightBlue;
            this.btn9.Font = new System.Drawing.Font("Microsoft Sans Serif", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn9.Location = new System.Drawing.Point(247, 129);
            this.btn9.Name = "btn9";
            this.btn9.Size = new System.Drawing.Size(116, 57);
            this.btn9.TabIndex = 10;
            this.btn9.TabStop = false;
            this.btn9.Tag = "9";
            this.btn9.Text = "9";
            this.btn9.UseVisualStyleBackColor = false;
            this.btn9.Click += new System.EventHandler(this.btnNumber_Click);
            // 
            // btn8
            // 
            this.btn8.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btn8.BackColor = System.Drawing.Color.LightBlue;
            this.btn8.Font = new System.Drawing.Font("Microsoft Sans Serif", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn8.Location = new System.Drawing.Point(125, 129);
            this.btn8.Name = "btn8";
            this.btn8.Size = new System.Drawing.Size(116, 57);
            this.btn8.TabIndex = 9;
            this.btn8.TabStop = false;
            this.btn8.Tag = "8";
            this.btn8.Text = "8";
            this.btn8.UseVisualStyleBackColor = false;
            this.btn8.Click += new System.EventHandler(this.btnNumber_Click);
            // 
            // btn7
            // 
            this.btn7.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btn7.BackColor = System.Drawing.Color.LightBlue;
            this.btn7.Font = new System.Drawing.Font("Microsoft Sans Serif", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn7.Location = new System.Drawing.Point(3, 129);
            this.btn7.Name = "btn7";
            this.btn7.Size = new System.Drawing.Size(116, 57);
            this.btn7.TabIndex = 8;
            this.btn7.TabStop = false;
            this.btn7.Tag = "7";
            this.btn7.Text = "7";
            this.btn7.UseVisualStyleBackColor = false;
            this.btn7.Click += new System.EventHandler(this.btnNumber_Click);
            // 
            // btn6
            // 
            this.btn6.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btn6.BackColor = System.Drawing.Color.LightBlue;
            this.btn6.Font = new System.Drawing.Font("Microsoft Sans Serif", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn6.Location = new System.Drawing.Point(247, 66);
            this.btn6.Name = "btn6";
            this.btn6.Size = new System.Drawing.Size(116, 57);
            this.btn6.TabIndex = 6;
            this.btn6.TabStop = false;
            this.btn6.Tag = "6";
            this.btn6.Text = "6";
            this.btn6.UseVisualStyleBackColor = false;
            this.btn6.Click += new System.EventHandler(this.btnNumber_Click);
            // 
            // btn5
            // 
            this.btn5.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btn5.BackColor = System.Drawing.Color.LightBlue;
            this.btn5.Font = new System.Drawing.Font("Microsoft Sans Serif", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn5.Location = new System.Drawing.Point(125, 66);
            this.btn5.Name = "btn5";
            this.btn5.Size = new System.Drawing.Size(116, 57);
            this.btn5.TabIndex = 5;
            this.btn5.TabStop = false;
            this.btn5.Tag = "5";
            this.btn5.Text = "5";
            this.btn5.UseVisualStyleBackColor = false;
            this.btn5.Click += new System.EventHandler(this.btnNumber_Click);
            // 
            // btn4
            // 
            this.btn4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btn4.BackColor = System.Drawing.Color.LightBlue;
            this.btn4.Font = new System.Drawing.Font("Microsoft Sans Serif", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn4.Location = new System.Drawing.Point(3, 66);
            this.btn4.Name = "btn4";
            this.btn4.Size = new System.Drawing.Size(116, 57);
            this.btn4.TabIndex = 4;
            this.btn4.TabStop = false;
            this.btn4.Tag = "4";
            this.btn4.Text = "4";
            this.btn4.UseVisualStyleBackColor = false;
            this.btn4.Click += new System.EventHandler(this.btnNumber_Click);
            // 
            // btn3
            // 
            this.btn3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btn3.BackColor = System.Drawing.Color.LightBlue;
            this.btn3.Font = new System.Drawing.Font("Microsoft Sans Serif", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn3.Location = new System.Drawing.Point(247, 3);
            this.btn3.Name = "btn3";
            this.btn3.Size = new System.Drawing.Size(116, 57);
            this.btn3.TabIndex = 2;
            this.btn3.TabStop = false;
            this.btn3.Tag = "3";
            this.btn3.Text = "3";
            this.btn3.UseVisualStyleBackColor = false;
            this.btn3.Click += new System.EventHandler(this.btnNumber_Click);
            // 
            // btn2
            // 
            this.btn2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btn2.BackColor = System.Drawing.Color.LightBlue;
            this.btn2.Font = new System.Drawing.Font("Microsoft Sans Serif", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn2.Location = new System.Drawing.Point(125, 3);
            this.btn2.Name = "btn2";
            this.btn2.Size = new System.Drawing.Size(116, 57);
            this.btn2.TabIndex = 1;
            this.btn2.TabStop = false;
            this.btn2.Tag = "2";
            this.btn2.Text = "2";
            this.btn2.UseVisualStyleBackColor = false;
            this.btn2.Click += new System.EventHandler(this.btnNumber_Click);
            // 
            // btn1
            // 
            this.btn1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btn1.BackColor = System.Drawing.Color.LightBlue;
            this.btn1.Font = new System.Drawing.Font("Microsoft Sans Serif", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn1.Location = new System.Drawing.Point(3, 3);
            this.btn1.Name = "btn1";
            this.btn1.Size = new System.Drawing.Size(116, 57);
            this.btn1.TabIndex = 0;
            this.btn1.TabStop = false;
            this.btn1.Tag = "1";
            this.btn1.Text = "1";
            this.btn1.UseVisualStyleBackColor = false;
            this.btn1.Click += new System.EventHandler(this.btnNumber_Click);
            // 
            // btnItemAdd
            // 
            this.btnItemAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnItemAdd.BackColor = System.Drawing.Color.DarkTurquoise;
            this.btnItemAdd.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnItemAdd.Location = new System.Drawing.Point(369, 3);
            this.btnItemAdd.Name = "btnItemAdd";
            this.btnItemAdd.Size = new System.Drawing.Size(116, 57);
            this.btnItemAdd.TabIndex = 19;
            this.btnItemAdd.TabStop = false;
            this.btnItemAdd.Text = "Disc&ount";
            this.toolTip1.SetToolTip(this.btnItemAdd, "Add Discount on Item or Full Bill, to remove discount apply zero percent");
            this.btnItemAdd.UseVisualStyleBackColor = false;
            this.btnItemAdd.Click += new System.EventHandler(this.btnItemAdd_Click);
            // 
            // btnBillSearch
            // 
            this.btnBillSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBillSearch.BackColor = System.Drawing.Color.LightYellow;
            this.btnBillSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBillSearch.Location = new System.Drawing.Point(3, 255);
            this.btnBillSearch.Name = "btnBillSearch";
            this.btnBillSearch.Size = new System.Drawing.Size(116, 57);
            this.btnBillSearch.TabIndex = 18;
            this.btnBillSearch.TabStop = false;
            this.btnBillSearch.Text = "&Bill Search";
            this.btnBillSearch.UseVisualStyleBackColor = false;
            this.btnBillSearch.Click += new System.EventHandler(this.btnBillSearch_Click);
            // 
            // btnClear
            // 
            this.btnClear.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClear.BackColor = System.Drawing.Color.Pink;
            this.btnClear.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClear.Location = new System.Drawing.Point(125, 255);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(116, 57);
            this.btnClear.TabIndex = 20;
            this.btnClear.TabStop = false;
            this.btnClear.Text = "&NEW";
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnMinus
            // 
            this.btnMinus.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMinus.BackColor = System.Drawing.Color.LightBlue;
            this.btnMinus.Font = new System.Drawing.Font("Microsoft Sans Serif", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMinus.Location = new System.Drawing.Point(247, 192);
            this.btnMinus.Name = "btnMinus";
            this.btnMinus.Size = new System.Drawing.Size(116, 57);
            this.btnMinus.TabIndex = 12;
            this.btnMinus.TabStop = false;
            this.btnMinus.Tag = "-";
            this.btnMinus.Text = "-";
            this.btnMinus.UseVisualStyleBackColor = false;
            this.btnMinus.Click += new System.EventHandler(this.btnNumber_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.btnPrint.BackColor = System.Drawing.Color.White;
            this.btnPrint.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPrint.Location = new System.Drawing.Point(247, 255);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(116, 57);
            this.btnPrint.TabIndex = 17;
            this.btnPrint.TabStop = false;
            this.btnPrint.Text = "PRIN&T";
            this.btnPrint.UseVisualStyleBackColor = false;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // btnQuantity
            // 
            this.btnQuantity.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnQuantity.BackColor = System.Drawing.Color.Chocolate;
            this.btnQuantity.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnQuantity.Location = new System.Drawing.Point(369, 66);
            this.btnQuantity.Name = "btnQuantity";
            this.btnQuantity.Size = new System.Drawing.Size(116, 57);
            this.btnQuantity.TabIndex = 19;
            this.btnQuantity.TabStop = false;
            this.btnQuantity.Text = "&QTY";
            this.toolTip1.SetToolTip(this.btnQuantity, "Change quantity of an item");
            this.btnQuantity.UseVisualStyleBackColor = false;
            this.btnQuantity.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnPrice
            // 
            this.btnPrice.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPrice.BackColor = System.Drawing.Color.Khaki;
            this.btnPrice.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPrice.Location = new System.Drawing.Point(369, 129);
            this.btnPrice.Name = "btnPrice";
            this.btnPrice.Size = new System.Drawing.Size(116, 57);
            this.btnPrice.TabIndex = 19;
            this.btnPrice.TabStop = false;
            this.btnPrice.Text = "&PRICE";
            this.toolTip1.SetToolTip(this.btnPrice, "Change  price of an item");
            this.btnPrice.UseVisualStyleBackColor = false;
            this.btnPrice.Click += new System.EventHandler(this.button4_Click);
            // 
            // btnEnter
            // 
            this.btnEnter.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEnter.BackColor = System.Drawing.Color.GreenYellow;
            this.btnEnter.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEnter.Location = new System.Drawing.Point(369, 192);
            this.btnEnter.Name = "btnEnter";
            this.btnEnter.Size = new System.Drawing.Size(116, 57);
            this.btnEnter.TabIndex = 15;
            this.btnEnter.TabStop = false;
            this.btnEnter.Text = "&ENTER";
            this.btnEnter.UseVisualStyleBackColor = false;
            this.btnEnter.Click += new System.EventHandler(this.btnEnter_Click);
            // 
            // btnSelect
            // 
            this.btnSelect.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelect.BackColor = System.Drawing.Color.Yellow;
            this.btnSelect.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSelect.Location = new System.Drawing.Point(369, 255);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(116, 57);
            this.btnSelect.TabIndex = 17;
            this.btnSelect.TabStop = false;
            this.btnSelect.Text = "&Select";
            this.toolTip1.SetToolTip(this.btnSelect, "Click then enter the item number to select");
            this.btnSelect.UseVisualStyleBackColor = false;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // btnItemRemove
            // 
            this.btnItemRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnItemRemove.BackColor = System.Drawing.Color.DarkTurquoise;
            this.btnItemRemove.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnItemRemove.Location = new System.Drawing.Point(369, 318);
            this.btnItemRemove.Name = "btnItemRemove";
            this.btnItemRemove.Size = new System.Drawing.Size(116, 57);
            this.btnItemRemove.TabIndex = 19;
            this.btnItemRemove.TabStop = false;
            this.btnItemRemove.Text = "ITEM &-";
            this.toolTip1.SetToolTip(this.btnItemRemove, "Remove the selected item");
            this.btnItemRemove.UseVisualStyleBackColor = false;
            this.btnItemRemove.Click += new System.EventHandler(this.btnItemRemove_Click);
            // 
            // btnCOD
            // 
            this.btnCOD.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCOD.BackColor = System.Drawing.Color.White;
            this.btnCOD.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCOD.Location = new System.Drawing.Point(247, 318);
            this.btnCOD.Name = "btnCOD";
            this.btnCOD.Size = new System.Drawing.Size(116, 57);
            this.btnCOD.TabIndex = 17;
            this.btnCOD.TabStop = false;
            this.btnCOD.Tag = "1";
            this.btnCOD.Text = "&Deliver";
            this.toolTip1.SetToolTip(this.btnCOD, "Cash on delivery");
            this.btnCOD.UseVisualStyleBackColor = false;
            this.btnCOD.Click += new System.EventHandler(this.btnCOD_Click);
            // 
            // btnFinish
            // 
            this.btnFinish.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFinish.BackColor = System.Drawing.Color.White;
            this.btnFinish.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFinish.Location = new System.Drawing.Point(3, 381);
            this.btnFinish.Name = "btnFinish";
            this.btnFinish.Size = new System.Drawing.Size(116, 60);
            this.btnFinish.TabIndex = 17;
            this.btnFinish.TabStop = false;
            this.btnFinish.Tag = "1";
            this.btnFinish.Text = "&FINISH";
            this.btnFinish.UseVisualStyleBackColor = false;
            this.btnFinish.Click += new System.EventHandler(this.btnFinish_Click);
            // 
            // lblChange
            // 
            this.lblChange.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblChange.AutoSize = true;
            this.lblChange.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblChange.ForeColor = System.Drawing.Color.Red;
            this.lblChange.Location = new System.Drawing.Point(274, 392);
            this.lblChange.Name = "lblChange";
            this.lblChange.Size = new System.Drawing.Size(89, 37);
            this.lblChange.TabIndex = 18;
            this.lblChange.Text = "8888";
            // 
            // btnData
            // 
            this.btnData.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.btnData.BackColor = System.Drawing.Color.White;
            this.btnData.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnData.Location = new System.Drawing.Point(125, 381);
            this.btnData.Name = "btnData";
            this.btnData.Size = new System.Drawing.Size(116, 60);
            this.btnData.TabIndex = 17;
            this.btnData.TabStop = false;
            this.btnData.Text = "Get Server Items";
            this.toolTip1.SetToolTip(this.btnData, "Download items from the server");
            this.btnData.UseVisualStyleBackColor = false;
            this.btnData.Click += new System.EventHandler(this.btnData_Click);
            // 
            // lblInfo
            // 
            this.lblInfo.AutoSize = true;
            this.lblInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInfo.Location = new System.Drawing.Point(3, 0);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(44, 25);
            this.lblInfo.TabIndex = 2;
            this.lblInfo.Text = "Info";
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel4.BackColor = System.Drawing.SystemColors.Control;
            this.tableLayoutPanel4.ColumnCount = 4;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 19.62617F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 80.37383F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 44F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 236F));
            this.tableLayoutPanel4.Controls.Add(this.label4, 2, 3);
            this.tableLayoutPanel4.Controls.Add(this.txtPincode, 3, 3);
            this.tableLayoutPanel4.Controls.Add(this.label5, 2, 2);
            this.tableLayoutPanel4.Controls.Add(this.txtArea, 3, 2);
            this.tableLayoutPanel4.Controls.Add(this.txtMobile, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.txtAddress, 1, 2);
            this.tableLayoutPanel4.Controls.Add(this.label7, 0, 3);
            this.tableLayoutPanel4.Controls.Add(this.lblMobile, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel4.Controls.Add(this.lblAddress, 0, 2);
            this.tableLayoutPanel4.Controls.Add(this.txtCustomer, 1, 1);
            this.tableLayoutPanel4.Controls.Add(this.txtCity, 1, 3);
            this.tableLayoutPanel4.Controls.Add(this.label6, 2, 1);
            this.tableLayoutPanel4.Controls.Add(this.txtHouseNumber, 3, 1);
            this.tableLayoutPanel4.Controls.Add(this.txtPoints, 3, 0);
            this.tableLayoutPanel4.Controls.Add(this.label8, 2, 0);
            this.tableLayoutPanel4.Location = new System.Drawing.Point(318, 3);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 4;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.Size = new System.Drawing.Size(494, 155);
            this.tableLayoutPanel4.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(217, 129);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(22, 13);
            this.label4.TabIndex = 14;
            this.label4.Text = "Pin";
            // 
            // txtPincode
            // 
            this.txtPincode.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPincode.BackColor = System.Drawing.Color.White;
            this.txtPincode.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPincode.Location = new System.Drawing.Point(261, 119);
            this.txtPincode.Name = "txtPincode";
            this.txtPincode.Size = new System.Drawing.Size(230, 30);
            this.txtPincode.TabIndex = 6;
            this.txtPincode.Text = "000000";
            this.txtPincode.Enter += new System.EventHandler(this.txt_Enter);
            this.txtPincode.Leave += new System.EventHandler(this.txt_Leave);
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(217, 91);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(29, 13);
            this.label5.TabIndex = 15;
            this.label5.Text = "Area";
            // 
            // txtArea
            // 
            this.txtArea.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtArea.BackColor = System.Drawing.Color.White;
            this.txtArea.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtArea.Location = new System.Drawing.Point(261, 83);
            this.txtArea.Name = "txtArea";
            this.txtArea.Size = new System.Drawing.Size(230, 30);
            this.txtArea.TabIndex = 4;
            this.txtArea.Enter += new System.EventHandler(this.txt_Enter);
            this.txtArea.Leave += new System.EventHandler(this.txt_Leave);
            // 
            // txtMobile
            // 
            this.txtMobile.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMobile.BackColor = System.Drawing.Color.White;
            this.txtMobile.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMobile.Location = new System.Drawing.Point(45, 3);
            this.txtMobile.MaxLength = 12;
            this.txtMobile.Name = "txtMobile";
            this.txtMobile.Size = new System.Drawing.Size(166, 38);
            this.txtMobile.TabIndex = 0;
            this.txtMobile.Enter += new System.EventHandler(this.txt_Enter);
            this.txtMobile.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtMobile_KeyUp);
            this.txtMobile.Leave += new System.EventHandler(this.txt_Leave);
            // 
            // txtAddress
            // 
            this.txtAddress.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtAddress.BackColor = System.Drawing.Color.White;
            this.txtAddress.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAddress.Location = new System.Drawing.Point(45, 83);
            this.txtAddress.Name = "txtAddress";
            this.txtAddress.Size = new System.Drawing.Size(166, 30);
            this.txtAddress.TabIndex = 3;
            this.txtAddress.Click += new System.EventHandler(this.txtAddress_Click);
            this.txtAddress.Enter += new System.EventHandler(this.txt_Enter);
            this.txtAddress.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtAddress_KeyUp);
            this.txtAddress.Leave += new System.EventHandler(this.txt_Leave);
            // 
            // label7
            // 
            this.label7.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(3, 129);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(24, 13);
            this.label7.TabIndex = 17;
            this.label7.Text = "City";
            // 
            // lblMobile
            // 
            this.lblMobile.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblMobile.AutoSize = true;
            this.lblMobile.Location = new System.Drawing.Point(3, 15);
            this.lblMobile.Name = "lblMobile";
            this.lblMobile.Size = new System.Drawing.Size(31, 13);
            this.lblMobile.TabIndex = 15;
            this.lblMobile.Text = "&Mob.";
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 55);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 14;
            this.label2.Text = "Name";
            // 
            // lblAddress
            // 
            this.lblAddress.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblAddress.AutoSize = true;
            this.lblAddress.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblAddress.ForeColor = System.Drawing.Color.Black;
            this.lblAddress.Location = new System.Drawing.Point(3, 91);
            this.lblAddress.Name = "lblAddress";
            this.lblAddress.Size = new System.Drawing.Size(29, 13);
            this.lblAddress.TabIndex = 13;
            this.lblAddress.Text = "Add.";
            // 
            // txtCustomer
            // 
            this.txtCustomer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCustomer.BackColor = System.Drawing.Color.White;
            this.txtCustomer.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCustomer.Location = new System.Drawing.Point(45, 47);
            this.txtCustomer.Name = "txtCustomer";
            this.txtCustomer.Size = new System.Drawing.Size(166, 30);
            this.txtCustomer.TabIndex = 2;
            this.txtCustomer.Enter += new System.EventHandler(this.txt_Enter);
            this.txtCustomer.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtCustomer_KeyUp);
            this.txtCustomer.Leave += new System.EventHandler(this.txt_Leave);
            // 
            // txtCity
            // 
            this.txtCity.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCity.BackColor = System.Drawing.Color.White;
            this.txtCity.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCity.Location = new System.Drawing.Point(45, 119);
            this.txtCity.Name = "txtCity";
            this.txtCity.Size = new System.Drawing.Size(166, 30);
            this.txtCity.TabIndex = 5;
            this.txtCity.Enter += new System.EventHandler(this.txt_Enter);
            this.txtCity.Leave += new System.EventHandler(this.txt_Leave);
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(217, 49);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(38, 26);
            this.label6.TabIndex = 15;
            this.label6.Text = "&House No";
            // 
            // txtHouseNumber
            // 
            this.txtHouseNumber.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtHouseNumber.BackColor = System.Drawing.Color.White;
            this.txtHouseNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtHouseNumber.Location = new System.Drawing.Point(261, 47);
            this.txtHouseNumber.Name = "txtHouseNumber";
            this.txtHouseNumber.Size = new System.Drawing.Size(230, 30);
            this.txtHouseNumber.TabIndex = 2;
            this.txtHouseNumber.Enter += new System.EventHandler(this.txt_Enter);
            this.txtHouseNumber.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtHouseNumber_KeyUp);
            this.txtHouseNumber.Leave += new System.EventHandler(this.txt_Leave);
            // 
            // txtPoints
            // 
            this.txtPoints.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPoints.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPoints.Location = new System.Drawing.Point(261, 3);
            this.txtPoints.Name = "txtPoints";
            this.txtPoints.Size = new System.Drawing.Size(230, 38);
            this.txtPoints.TabIndex = 11;
            this.txtPoints.TabStop = false;
            this.txtPoints.Text = "0";
            this.txtPoints.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtMobile_KeyUp);
            // 
            // label8
            // 
            this.label8.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(217, 15);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(36, 13);
            this.label8.TabIndex = 15;
            this.label8.Text = "&Points";
            // 
            // listViewSearchResult
            // 
            this.listViewSearchResult.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewSearchResult.BackColor = System.Drawing.Color.White;
            this.listViewSearchResult.Font = new System.Drawing.Font("Arial Narrow", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listViewSearchResult.Location = new System.Drawing.Point(818, 164);
            this.listViewSearchResult.Name = "listViewSearchResult";
            this.listViewSearchResult.Size = new System.Drawing.Size(605, 534);
            this.listViewSearchResult.TabIndex = 5;
            this.listViewSearchResult.TabStop = false;
            this.listViewSearchResult.UseCompatibleStateImageBehavior = false;
            this.listViewSearchResult.SelectedIndexChanged += new System.EventHandler(this.listViewSearchResult_SelectedIndexChanged);
            this.listViewSearchResult.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtItemSearch_KeyUp);
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.ColumnCount = 3;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel5.Controls.Add(this.btnFutureOrder, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.btnItemCalegory, 2, 0);
            this.tableLayoutPanel5.Location = new System.Drawing.Point(818, 3);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 3;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(605, 155);
            this.tableLayoutPanel5.TabIndex = 7;
            // 
            // btnFutureOrder
            // 
            this.btnFutureOrder.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFutureOrder.BackColor = System.Drawing.Color.LightBlue;
            this.btnFutureOrder.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFutureOrder.Location = new System.Drawing.Point(3, 3);
            this.btnFutureOrder.Name = "btnFutureOrder";
            this.btnFutureOrder.Size = new System.Drawing.Size(195, 45);
            this.btnFutureOrder.TabIndex = 2;
            this.btnFutureOrder.TabStop = false;
            this.btnFutureOrder.Tag = "2";
            this.btnFutureOrder.Text = "Now";
            this.btnFutureOrder.UseVisualStyleBackColor = false;
            this.btnFutureOrder.Click += new System.EventHandler(this.btnFutureOrder_Click);
            // 
            // btnItemCalegory
            // 
            this.btnItemCalegory.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnItemCalegory.BackColor = System.Drawing.Color.LightBlue;
            this.btnItemCalegory.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnItemCalegory.Location = new System.Drawing.Point(405, 3);
            this.btnItemCalegory.Name = "btnItemCalegory";
            this.btnItemCalegory.Size = new System.Drawing.Size(197, 45);
            this.btnItemCalegory.TabIndex = 3;
            this.btnItemCalegory.TabStop = false;
            this.btnItemCalegory.Tag = "2";
            this.btnItemCalegory.Text = "Category";
            this.btnItemCalegory.UseVisualStyleBackColor = false;
            this.btnItemCalegory.Click += new System.EventHandler(this.btnItemCalegory_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.BackColor = System.Drawing.SystemColors.Control;
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusTime,
            this.toolStripStatusServer,
            this.toolStripStatusTerminal,
            this.toolStripStatusOnline});
            this.statusStrip1.Location = new System.Drawing.Point(0, 707);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.statusStrip1.Size = new System.Drawing.Size(1426, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusTime
            // 
            this.toolStripStatusTime.Name = "toolStripStatusTime";
            this.toolStripStatusTime.Size = new System.Drawing.Size(49, 17);
            this.toolStripStatusTime.Text = "00.00.00";
            // 
            // toolStripStatusServer
            // 
            this.toolStripStatusServer.Name = "toolStripStatusServer";
            this.toolStripStatusServer.Size = new System.Drawing.Size(116, 17);
            this.toolStripStatusServer.Text = "toolStripStatusServer";
            this.toolStripStatusServer.Click += new System.EventHandler(this.toolStripStatusServer_Click);
            // 
            // toolStripStatusTerminal
            // 
            this.toolStripStatusTerminal.Name = "toolStripStatusTerminal";
            this.toolStripStatusTerminal.Size = new System.Drawing.Size(54, 17);
            this.toolStripStatusTerminal.Text = "Terminal";
            // 
            // toolStripStatusOnline
            // 
            this.toolStripStatusOnline.BackColor = System.Drawing.Color.Red;
            this.toolStripStatusOnline.ForeColor = System.Drawing.Color.Red;
            this.toolStripStatusOnline.Name = "toolStripStatusOnline";
            this.toolStripStatusOnline.Size = new System.Drawing.Size(42, 17);
            this.toolStripStatusOnline.Text = "Online";
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 500;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // frmBilling
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1426, 729);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmBilling";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Billing";
            this.Load += new System.EventHandler(this.frmBilling_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtItemSearch_KeyUp);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictBill)).EndInit();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.tableLayoutPanel5.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TextBox txtItemSearch;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Button btnEnter;
        private System.Windows.Forms.Button btnZero;
        private System.Windows.Forms.Button btnDot;
        private System.Windows.Forms.Button btn9;
        private System.Windows.Forms.Button btn8;
        private System.Windows.Forms.Button btn7;
        private System.Windows.Forms.Button btn6;
        private System.Windows.Forms.Button btn5;
        private System.Windows.Forms.Button btn4;
        private System.Windows.Forms.Button btn3;
        private System.Windows.Forms.Button btn2;
        private System.Windows.Forms.Button btn1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.TextBox txtCustomer;
        private System.Windows.Forms.TextBox txtMobile;
        private System.Windows.Forms.TextBox txtAddress;
        private System.Windows.Forms.Label lblAddress;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblMobile;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtPincode;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtArea;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtCity;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.Button btnPayCash;
        private System.Windows.Forms.Button btnBillSearch;
        private System.Windows.Forms.Button btnItemAdd;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnItemRemove;
        private System.Windows.Forms.Button btnQuantity;
        private System.Windows.Forms.Button btnMinus;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.Button btnPrice;
        private System.Windows.Forms.Button btnPayCard;
        private System.Windows.Forms.Label lblTotalAmount;
        private System.Windows.Forms.Button btnCOD;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtHouseNumber;
        private System.Windows.Forms.TextBox txtPoints;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.Button btnFinish;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusTime;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label lblChange;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusServer;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusTerminal;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusOnline;
        private System.Windows.Forms.Button btnData;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pictBill;
        private System.Windows.Forms.ListView listViewSearchResult;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.Button btnFutureOrder;
        private System.Windows.Forms.Button btnItemCalegory;
    }
}