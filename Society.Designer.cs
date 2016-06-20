namespace BillingSoft
{
    partial class Society
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Society));
            this.lstSociety = new System.Windows.Forms.ListBox();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.btnUpdateListFromServer = new System.Windows.Forms.Button();
            this.btnTopList = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // lstSociety
            // 
            this.lstSociety.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstSociety.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstSociety.FormattingEnabled = true;
            this.lstSociety.ItemHeight = 20;
            this.lstSociety.Location = new System.Drawing.Point(12, 49);
            this.lstSociety.Name = "lstSociety";
            this.lstSociety.Size = new System.Drawing.Size(290, 504);
            this.lstSociety.TabIndex = 0;
            this.lstSociety.Click += new System.EventHandler(this.lstSociety_Click);
            this.lstSociety.SelectedIndexChanged += new System.EventHandler(this.lstSociety_SelectedIndexChanged);
            this.lstSociety.KeyUp += new System.Windows.Forms.KeyEventHandler(this.lstSociety_KeyUp);
            // 
            // txtSearch
            // 
            this.txtSearch.CharacterCasing = System.Windows.Forms.CharacterCasing.Lower;
            this.txtSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSearch.Location = new System.Drawing.Point(13, 13);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(289, 29);
            this.txtSearch.TabIndex = 0;
            this.txtSearch.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtSearch_KeyUp);
            // 
            // btnUpdateListFromServer
            // 
            this.btnUpdateListFromServer.Location = new System.Drawing.Point(12, 560);
            this.btnUpdateListFromServer.Name = "btnUpdateListFromServer";
            this.btnUpdateListFromServer.Size = new System.Drawing.Size(108, 23);
            this.btnUpdateListFromServer.TabIndex = 2;
            this.btnUpdateListFromServer.TabStop = false;
            this.btnUpdateListFromServer.Text = "Get Server List";
            this.btnUpdateListFromServer.UseVisualStyleBackColor = true;
            this.btnUpdateListFromServer.Click += new System.EventHandler(this.btnUpdateListFromServer_Click);
            // 
            // btnTopList
            // 
            this.btnTopList.Location = new System.Drawing.Point(227, 559);
            this.btnTopList.Name = "btnTopList";
            this.btnTopList.Size = new System.Drawing.Size(75, 23);
            this.btnTopList.TabIndex = 3;
            this.btnTopList.Text = "Top List";
            this.toolTip1.SetToolTip(this.btnTopList, "Enter the names of the societies which should come on the top");
            this.btnTopList.UseVisualStyleBackColor = true;
            this.btnTopList.Click += new System.EventHandler(this.btnTopList_Click);
            // 
            // Society
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(314, 587);
            this.Controls.Add(this.btnTopList);
            this.Controls.Add(this.btnUpdateListFromServer);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.lstSociety);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Society";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Society";
            this.Load += new System.EventHandler(this.Society_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lstSociety;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Button btnUpdateListFromServer;
        private System.Windows.Forms.Button btnTopList;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}