using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;

namespace LaundryPOS.Forms
{
    partial class LoginForm
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
        /// 

        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
        (
            int nLeftRect,
            int nTopRect,
            int nRightRect,
            int nBottomRect,
            int nWidthEllipse,
            int nHeightEllipse
        );

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginForm));
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges19 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges20 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges15 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges16 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges7 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges8 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges11 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges12 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges9 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges10 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges13 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges14 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges17 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges18 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            this.txtPassword = new Guna.UI2.WinForms.Guna2TextBox();
            this.txtUsername = new Guna.UI2.WinForms.Guna2TextBox();
            this.panelDrag = new Guna.UI2.WinForms.Guna2Panel();
            this.guna2Panel1 = new Guna.UI2.WinForms.Guna2Panel();
            this.btnMinimize = new Guna.UI2.WinForms.Guna2Button();
            this.btnClose = new Guna.UI2.WinForms.Guna2Button();
            this.label2 = new System.Windows.Forms.Label();
            this.lblName = new System.Windows.Forms.Label();
            this.panelLogin = new Guna.UI2.WinForms.Guna2Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.cbShowPassword = new Guna.UI2.WinForms.Guna2CheckBox();
            this.btnLogin = new Guna.UI2.WinForms.Guna2Button();
            this.guna2PictureBox1 = new Guna.UI2.WinForms.Guna2PictureBox();
            this.guna2Panel2 = new Guna.UI2.WinForms.Guna2Panel();
            this.dragControl = new Guna.UI2.WinForms.Guna2DragControl(this.components);
            this.panelDrag.SuspendLayout();
            this.guna2Panel1.SuspendLayout();
            this.panelLogin.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.guna2PictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // txtPassword
            // 
            this.txtPassword.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.txtPassword.BackColor = System.Drawing.Color.Transparent;
            this.txtPassword.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(246)))), ((int)(((byte)(248)))));
            this.txtPassword.BorderRadius = 5;
            this.txtPassword.BorderThickness = 0;
            this.txtPassword.CustomizableEdges = customizableEdges1;
            this.txtPassword.DefaultText = "";
            this.txtPassword.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtPassword.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtPassword.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtPassword.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtPassword.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(246)))), ((int)(((byte)(248)))));
            this.txtPassword.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(246)))), ((int)(((byte)(248)))));
            this.txtPassword.Font = new System.Drawing.Font("Helvetica", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtPassword.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.txtPassword.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtPassword.IconLeft = ((System.Drawing.Image)(resources.GetObject("txtPassword.IconLeft")));
            this.txtPassword.IconLeftOffset = new System.Drawing.Point(5, -1);
            this.txtPassword.IconLeftSize = new System.Drawing.Size(15, 15);
            this.txtPassword.Location = new System.Drawing.Point(28, 174);
            this.txtPassword.Margin = new System.Windows.Forms.Padding(3, 9, 3, 9);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '•';
            this.txtPassword.PlaceholderText = "Password";
            this.txtPassword.SelectedText = "";
            this.txtPassword.ShadowDecoration.CustomizableEdges = customizableEdges2;
            this.txtPassword.Size = new System.Drawing.Size(250, 45);
            this.txtPassword.TabIndex = 1;
            this.txtPassword.TextOffset = new System.Drawing.Point(2, 1);
            // 
            // txtUsername
            // 
            this.txtUsername.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.txtUsername.BackColor = System.Drawing.Color.Transparent;
            this.txtUsername.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(246)))), ((int)(((byte)(248)))));
            this.txtUsername.BorderRadius = 5;
            this.txtUsername.BorderThickness = 0;
            this.txtUsername.CustomizableEdges = customizableEdges3;
            this.txtUsername.DefaultText = "";
            this.txtUsername.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(246)))), ((int)(((byte)(248)))));
            this.txtUsername.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtUsername.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtUsername.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtUsername.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(246)))), ((int)(((byte)(248)))));
            this.txtUsername.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(246)))), ((int)(((byte)(248)))));
            this.txtUsername.Font = new System.Drawing.Font("Helvetica", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtUsername.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.txtUsername.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtUsername.IconLeft = ((System.Drawing.Image)(resources.GetObject("txtUsername.IconLeft")));
            this.txtUsername.IconLeftOffset = new System.Drawing.Point(5, -1);
            this.txtUsername.IconLeftSize = new System.Drawing.Size(15, 15);
            this.txtUsername.Location = new System.Drawing.Point(28, 116);
            this.txtUsername.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.PasswordChar = '\0';
            this.txtUsername.PlaceholderText = "Username";
            this.txtUsername.SelectedText = "";
            this.txtUsername.ShadowDecoration.CustomizableEdges = customizableEdges4;
            this.txtUsername.Size = new System.Drawing.Size(250, 45);
            this.txtUsername.TabIndex = 0;
            this.txtUsername.TextOffset = new System.Drawing.Point(2, 1);
            // 
            // panelDrag
            // 
            this.panelDrag.BackColor = System.Drawing.Color.Transparent;
            this.panelDrag.Controls.Add(this.guna2Panel1);
            this.panelDrag.Controls.Add(this.guna2Panel2);
            this.panelDrag.CustomizableEdges = customizableEdges19;
            this.panelDrag.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelDrag.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(244)))), ((int)(((byte)(244)))));
            this.panelDrag.Location = new System.Drawing.Point(0, 0);
            this.panelDrag.Name = "panelDrag";
            this.panelDrag.Padding = new System.Windows.Forms.Padding(30);
            this.panelDrag.ShadowDecoration.CustomizableEdges = customizableEdges20;
            this.panelDrag.Size = new System.Drawing.Size(1080, 672);
            this.panelDrag.TabIndex = 12;
            // 
            // guna2Panel1
            // 
            this.guna2Panel1.BackColor = System.Drawing.Color.Transparent;
            this.guna2Panel1.BorderRadius = 12;
            this.guna2Panel1.Controls.Add(this.btnMinimize);
            this.guna2Panel1.Controls.Add(this.btnClose);
            this.guna2Panel1.Controls.Add(this.label2);
            this.guna2Panel1.Controls.Add(this.lblName);
            this.guna2Panel1.Controls.Add(this.panelLogin);
            this.guna2Panel1.Controls.Add(this.guna2PictureBox1);
            this.guna2Panel1.CustomizableEdges = customizableEdges15;
            this.guna2Panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.guna2Panel1.FillColor = System.Drawing.Color.White;
            this.guna2Panel1.Location = new System.Drawing.Point(30, 30);
            this.guna2Panel1.Name = "guna2Panel1";
            this.guna2Panel1.ShadowDecoration.CustomizableEdges = customizableEdges16;
            this.guna2Panel1.Size = new System.Drawing.Size(1020, 612);
            this.guna2Panel1.TabIndex = 16;
            // 
            // btnMinimize
            // 
            this.btnMinimize.BorderRadius = 15;
            this.btnMinimize.CustomizableEdges = customizableEdges5;
            this.btnMinimize.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnMinimize.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnMinimize.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnMinimize.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnMinimize.FillColor = System.Drawing.Color.Transparent;
            this.btnMinimize.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnMinimize.ForeColor = System.Drawing.Color.White;
            this.btnMinimize.Image = ((System.Drawing.Image)(resources.GetObject("btnMinimize.Image")));
            this.btnMinimize.ImageSize = new System.Drawing.Size(15, 15);
            this.btnMinimize.Location = new System.Drawing.Point(915, 16);
            this.btnMinimize.Name = "btnMinimize";
            this.btnMinimize.ShadowDecoration.CustomizableEdges = customizableEdges6;
            this.btnMinimize.Size = new System.Drawing.Size(39, 39);
            this.btnMinimize.TabIndex = 19;
            this.btnMinimize.Click += new System.EventHandler(this.btnMinimize_Click);
            // 
            // btnClose
            // 
            this.btnClose.BorderRadius = 15;
            this.btnClose.CustomizableEdges = customizableEdges7;
            this.btnClose.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnClose.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnClose.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnClose.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnClose.FillColor = System.Drawing.Color.Transparent;
            this.btnClose.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Image = ((System.Drawing.Image)(resources.GetObject("btnClose.Image")));
            this.btnClose.ImageSize = new System.Drawing.Size(15, 15);
            this.btnClose.Location = new System.Drawing.Point(960, 16);
            this.btnClose.Name = "btnClose";
            this.btnClose.ShadowDecoration.CustomizableEdges = customizableEdges8;
            this.btnClose.Size = new System.Drawing.Size(39, 39);
            this.btnClose.TabIndex = 18;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Helvetica", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(98, 80);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(345, 41);
            this.label2.TabIndex = 17;
            this.label2.Text = "Point-of-sale management system";
            // 
            // lblName
            // 
            this.lblName.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblName.AutoSize = true;
            this.lblName.BackColor = System.Drawing.Color.Transparent;
            this.lblName.Font = new System.Drawing.Font("Montserrat", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblName.ForeColor = System.Drawing.Color.Black;
            this.lblName.Location = new System.Drawing.Point(92, 41);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(215, 40);
            this.lblName.TabIndex = 16;
            this.lblName.Text = "Laundry POS";
            // 
            // panelLogin
            // 
            this.panelLogin.BackColor = System.Drawing.Color.Transparent;
            this.panelLogin.BorderRadius = 5;
            this.panelLogin.Controls.Add(this.txtUsername);
            this.panelLogin.Controls.Add(this.label1);
            this.panelLogin.Controls.Add(this.txtPassword);
            this.panelLogin.Controls.Add(this.cbShowPassword);
            this.panelLogin.Controls.Add(this.btnLogin);
            this.panelLogin.CustomizableEdges = customizableEdges11;
            this.panelLogin.FillColor = System.Drawing.Color.White;
            this.panelLogin.Location = new System.Drawing.Point(94, 146);
            this.panelLogin.Name = "panelLogin";
            this.panelLogin.ShadowDecoration.Color = System.Drawing.Color.DimGray;
            this.panelLogin.ShadowDecoration.CustomizableEdges = customizableEdges12;
            this.panelLogin.ShadowDecoration.Depth = 5;
            this.panelLogin.ShadowDecoration.Enabled = true;
            this.panelLogin.ShadowDecoration.Shadow = new System.Windows.Forms.Padding(4, 4, 12, 12);
            this.panelLogin.Size = new System.Drawing.Size(306, 405);
            this.panelLogin.TabIndex = 14;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Montserrat", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(23, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(130, 41);
            this.label1.TabIndex = 12;
            this.label1.Text = "Log in";
            // 
            // cbShowPassword
            // 
            this.cbShowPassword.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.cbShowPassword.BackColor = System.Drawing.Color.Transparent;
            this.cbShowPassword.CheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cbShowPassword.CheckedState.BorderRadius = 0;
            this.cbShowPassword.CheckedState.BorderThickness = 0;
            this.cbShowPassword.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cbShowPassword.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cbShowPassword.Font = new System.Drawing.Font("Helvetica", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.cbShowPassword.Location = new System.Drawing.Point(178, 231);
            this.cbShowPassword.Name = "cbShowPassword";
            this.cbShowPassword.Size = new System.Drawing.Size(108, 23);
            this.cbShowPassword.TabIndex = 3;
            this.cbShowPassword.Text = "Show Password";
            this.cbShowPassword.UncheckedState.BorderColor = System.Drawing.Color.Black;
            this.cbShowPassword.UncheckedState.BorderRadius = 0;
            this.cbShowPassword.UncheckedState.BorderThickness = 0;
            this.cbShowPassword.UncheckedState.FillColor = System.Drawing.Color.Transparent;
            this.cbShowPassword.UseVisualStyleBackColor = false;
            this.cbShowPassword.CheckedChanged += new System.EventHandler(this.cbShowPassword_CheckedChanged);
            // 
            // btnLogin
            // 
            this.btnLogin.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnLogin.BackColor = System.Drawing.Color.Transparent;
            this.btnLogin.BorderRadius = 5;
            this.btnLogin.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLogin.CustomizableEdges = customizableEdges9;
            this.btnLogin.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnLogin.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnLogin.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnLogin.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnLogin.Font = new System.Drawing.Font("Helvetica", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnLogin.ForeColor = System.Drawing.Color.White;
            this.btnLogin.Location = new System.Drawing.Point(28, 289);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.ShadowDecoration.BorderRadius = 5;
            this.btnLogin.ShadowDecoration.Color = System.Drawing.Color.IndianRed;
            this.btnLogin.ShadowDecoration.CustomizableEdges = customizableEdges10;
            this.btnLogin.ShadowDecoration.Depth = 5;
            this.btnLogin.ShadowDecoration.Enabled = true;
            this.btnLogin.ShadowDecoration.Shadow = new System.Windows.Forms.Padding(2, 2, 10, 10);
            this.btnLogin.Size = new System.Drawing.Size(250, 45);
            this.btnLogin.TabIndex = 2;
            this.btnLogin.Text = "Log in";
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // guna2PictureBox1
            // 
            this.guna2PictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.guna2PictureBox1.CustomizableEdges = customizableEdges13;
            this.guna2PictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("guna2PictureBox1.Image")));
            this.guna2PictureBox1.ImageRotate = 0F;
            this.guna2PictureBox1.Location = new System.Drawing.Point(251, 31);
            this.guna2PictureBox1.Name = "guna2PictureBox1";
            this.guna2PictureBox1.ShadowDecoration.CustomizableEdges = customizableEdges14;
            this.guna2PictureBox1.Size = new System.Drawing.Size(911, 621);
            this.guna2PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.guna2PictureBox1.TabIndex = 15;
            this.guna2PictureBox1.TabStop = false;
            // 
            // guna2Panel2
            // 
            this.guna2Panel2.BackColor = System.Drawing.Color.Transparent;
            this.guna2Panel2.BorderRadius = 15;
            this.guna2Panel2.CustomizableEdges = customizableEdges17;
            this.guna2Panel2.FillColor = System.Drawing.Color.White;
            this.guna2Panel2.Location = new System.Drawing.Point(1056, 338);
            this.guna2Panel2.Name = "guna2Panel2";
            this.guna2Panel2.ShadowDecoration.BorderRadius = 15;
            this.guna2Panel2.ShadowDecoration.Color = System.Drawing.Color.DarkGray;
            this.guna2Panel2.ShadowDecoration.CustomizableEdges = customizableEdges18;
            this.guna2Panel2.ShadowDecoration.Depth = 15;
            this.guna2Panel2.ShadowDecoration.Enabled = true;
            this.guna2Panel2.ShadowDecoration.Shadow = new System.Windows.Forms.Padding(2, 2, 8, 8);
            this.guna2Panel2.Size = new System.Drawing.Size(400, 492);
            this.guna2Panel2.TabIndex = 13;
            this.guna2Panel2.Visible = false;
            // 
            // dragControl
            // 
            this.dragControl.DockIndicatorTransparencyValue = 0.6D;
            this.dragControl.TargetControl = this.panelDrag;
            this.dragControl.UseTransparentDrag = true;
            // 
            // LoginForm
            // 
            this.ClientSize = new System.Drawing.Size(1080, 672);
            this.Controls.Add(this.panelDrag);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "LoginForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Login";
            this.Load += new System.EventHandler(this.LoginForm_Load);
            this.panelDrag.ResumeLayout(false);
            this.guna2Panel1.ResumeLayout(false);
            this.guna2Panel1.PerformLayout();
            this.panelLogin.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.guna2PictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private Guna.UI2.WinForms.Guna2TextBox txtPassword;
        private Guna.UI2.WinForms.Guna2TextBox txtUsername;
        private Guna.UI2.WinForms.Guna2Panel panelDrag;
        private Guna.UI2.WinForms.Guna2Button btnLogin;
        private Label label1;
        private Guna.UI2.WinForms.Guna2Panel guna2Panel2;
        private Guna.UI2.WinForms.Guna2CheckBox cbShowPassword;
        private Guna.UI2.WinForms.Guna2DragControl dragControl;
        private Guna.UI2.WinForms.Guna2Panel panelLogin;
        private Guna.UI2.WinForms.Guna2Panel guna2Panel1;
        private Guna.UI2.WinForms.Guna2PictureBox guna2PictureBox1;
        private Label label2;
        private Label lblName;
        private Guna.UI2.WinForms.Guna2Button btnMinimize;
        private Guna.UI2.WinForms.Guna2Button btnClose;
    }
}