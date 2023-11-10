namespace LaundryPOS.Forms
{
    partial class MainForm
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
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges12 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges13 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges10 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges11 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges7 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges8 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges9 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            this.bgPanel = new Guna.UI2.WinForms.Guna2Panel();
            this.panelView = new Guna.UI2.WinForms.Guna2Panel();
            this.panelLeft = new Guna.UI2.WinForms.Guna2Panel();
            this.btnLogout = new Guna.UI2.WinForms.Guna2Button();
            this.btnPending = new Guna.UI2.WinForms.Guna2Button();
            this.btnMain = new Guna.UI2.WinForms.Guna2Button();
            this.imgPic = new Guna.UI2.WinForms.Guna2CirclePictureBox();
            this.bgPanel.SuspendLayout();
            this.panelLeft.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imgPic)).BeginInit();
            this.SuspendLayout();
            // 
            // bgPanel
            // 
            this.bgPanel.Controls.Add(this.panelView);
            this.bgPanel.Controls.Add(this.panelLeft);
            this.bgPanel.CustomizableEdges = customizableEdges12;
            this.bgPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bgPanel.FillColor = System.Drawing.Color.White;
            this.bgPanel.Location = new System.Drawing.Point(0, 0);
            this.bgPanel.Margin = new System.Windows.Forms.Padding(0);
            this.bgPanel.Name = "bgPanel";
            this.bgPanel.ShadowDecoration.CustomizableEdges = customizableEdges13;
            this.bgPanel.Size = new System.Drawing.Size(1471, 859);
            this.bgPanel.TabIndex = 25;
            // 
            // panelView
            // 
            this.panelView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelView.BackColor = System.Drawing.Color.White;
            this.panelView.CustomizableEdges = customizableEdges1;
            this.panelView.FillColor = System.Drawing.Color.White;
            this.panelView.Location = new System.Drawing.Point(106, 0);
            this.panelView.Name = "panelView";
            this.panelView.ShadowDecoration.CustomizableEdges = customizableEdges2;
            this.panelView.Size = new System.Drawing.Size(1365, 859);
            this.panelView.TabIndex = 26;
            // 
            // panelLeft
            // 
            this.panelLeft.BackColor = System.Drawing.Color.White;
            this.panelLeft.Controls.Add(this.btnLogout);
            this.panelLeft.Controls.Add(this.btnPending);
            this.panelLeft.Controls.Add(this.btnMain);
            this.panelLeft.Controls.Add(this.imgPic);
            this.panelLeft.CustomizableEdges = customizableEdges10;
            this.panelLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelLeft.FillColor = System.Drawing.Color.White;
            this.panelLeft.Location = new System.Drawing.Point(0, 0);
            this.panelLeft.Name = "panelLeft";
            this.panelLeft.ShadowDecoration.BorderRadius = 1;
            this.panelLeft.ShadowDecoration.Color = System.Drawing.Color.DarkGray;
            this.panelLeft.ShadowDecoration.CustomizableEdges = customizableEdges11;
            this.panelLeft.ShadowDecoration.Depth = 10;
            this.panelLeft.ShadowDecoration.Enabled = true;
            this.panelLeft.ShadowDecoration.Shadow = new System.Windows.Forms.Padding(2, 2, 8, 8);
            this.panelLeft.Size = new System.Drawing.Size(100, 859);
            this.panelLeft.TabIndex = 25;
            // 
            // btnLogout
            // 
            this.btnLogout.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnLogout.BackColor = System.Drawing.Color.White;
            this.btnLogout.BorderRadius = 12;
            this.btnLogout.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLogout.CustomizableEdges = customizableEdges3;
            this.btnLogout.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnLogout.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnLogout.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnLogout.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnLogout.FillColor = System.Drawing.Color.Transparent;
            this.btnLogout.Font = new System.Drawing.Font("Helvetica", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnLogout.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.btnLogout.Image = ((System.Drawing.Image)(resources.GetObject("btnLogout.Image")));
            this.btnLogout.ImageOffset = new System.Drawing.Point(11, -7);
            this.btnLogout.ImageSize = new System.Drawing.Size(28, 28);
            this.btnLogout.Location = new System.Drawing.Point(15, 756);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.ShadowDecoration.CustomizableEdges = customizableEdges4;
            this.btnLogout.Size = new System.Drawing.Size(70, 70);
            this.btnLogout.TabIndex = 22;
            this.btnLogout.Text = "Log out";
            this.btnLogout.TextOffset = new System.Drawing.Point(-8, 15);
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);
            // 
            // btnPending
            // 
            this.btnPending.BackColor = System.Drawing.Color.White;
            this.btnPending.BorderRadius = 12;
            this.btnPending.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnPending.CustomizableEdges = customizableEdges5;
            this.btnPending.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnPending.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnPending.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnPending.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnPending.FillColor = System.Drawing.Color.White;
            this.btnPending.Font = new System.Drawing.Font("Helvetica", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnPending.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.btnPending.Image = ((System.Drawing.Image)(resources.GetObject("btnPending.Image")));
            this.btnPending.ImageOffset = new System.Drawing.Point(10, -7);
            this.btnPending.ImageSize = new System.Drawing.Size(30, 30);
            this.btnPending.Location = new System.Drawing.Point(15, 210);
            this.btnPending.Name = "btnPending";
            this.btnPending.ShadowDecoration.CustomizableEdges = customizableEdges6;
            this.btnPending.Size = new System.Drawing.Size(70, 70);
            this.btnPending.TabIndex = 1;
            this.btnPending.Text = "Pending";
            this.btnPending.TextOffset = new System.Drawing.Point(-8, 15);
            this.btnPending.Click += new System.EventHandler(this.btnPending_Click);
            // 
            // btnMain
            // 
            this.btnMain.BackColor = System.Drawing.Color.Transparent;
            this.btnMain.BorderRadius = 12;
            this.btnMain.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnMain.CustomizableEdges = customizableEdges7;
            this.btnMain.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnMain.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnMain.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnMain.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnMain.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.btnMain.Font = new System.Drawing.Font("Helvetica", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnMain.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.btnMain.Image = ((System.Drawing.Image)(resources.GetObject("btnMain.Image")));
            this.btnMain.ImageOffset = new System.Drawing.Point(8, -7);
            this.btnMain.ImageSize = new System.Drawing.Size(30, 30);
            this.btnMain.Location = new System.Drawing.Point(15, 126);
            this.btnMain.Name = "btnMain";
            this.btnMain.ShadowDecoration.CustomizableEdges = customizableEdges8;
            this.btnMain.Size = new System.Drawing.Size(70, 70);
            this.btnMain.TabIndex = 0;
            this.btnMain.Text = "Home";
            this.btnMain.TextOffset = new System.Drawing.Point(-7, 15);
            this.btnMain.Click += new System.EventHandler(this.btnMain_Click);
            // 
            // imgPic
            // 
            this.imgPic.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.imgPic.BackColor = System.Drawing.Color.White;
            this.imgPic.Cursor = System.Windows.Forms.Cursors.Hand;
            this.imgPic.FillColor = System.Drawing.Color.WhiteSmoke;
            this.imgPic.ImageRotate = 0F;
            this.imgPic.Location = new System.Drawing.Point(25, 688);
            this.imgPic.Margin = new System.Windows.Forms.Padding(20, 10, 0, 10);
            this.imgPic.MaximumSize = new System.Drawing.Size(50, 50);
            this.imgPic.MinimumSize = new System.Drawing.Size(50, 50);
            this.imgPic.Name = "imgPic";
            this.imgPic.ShadowDecoration.CustomizableEdges = customizableEdges9;
            this.imgPic.ShadowDecoration.Mode = Guna.UI2.WinForms.Enums.ShadowMode.Circle;
            this.imgPic.Size = new System.Drawing.Size(50, 50);
            this.imgPic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.imgPic.TabIndex = 21;
            this.imgPic.TabStop = false;
            this.imgPic.Click += new System.EventHandler(this.btnProfile_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1471, 859);
            this.Controls.Add(this.bgPanel);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(1020, 600);
            this.Name = "MainForm";
            this.RightToLeftLayout = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MainForm";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.bgPanel.ResumeLayout(false);
            this.panelLeft.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.imgPic)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private Guna.UI2.WinForms.Guna2Panel bgPanel;
        private Guna.UI2.WinForms.Guna2CirclePictureBox imgPic;
        private Guna.UI2.WinForms.Guna2Panel panelLeft;
        private Guna.UI2.WinForms.Guna2Button btnMain;
        private Guna.UI2.WinForms.Guna2Button btnPending;
        private Guna.UI2.WinForms.Guna2Button btnLogout;
        private Guna.UI2.WinForms.Guna2Panel panelView;
    }
}