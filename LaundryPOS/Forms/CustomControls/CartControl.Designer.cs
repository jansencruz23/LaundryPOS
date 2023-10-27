namespace LaundryPOS.Forms.Views
{
    partial class CartControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges7 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges8 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CartControl));
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            this.lblName = new System.Windows.Forms.Label();
            this.lblCategory = new System.Windows.Forms.Label();
            this.lblSubTotal = new System.Windows.Forms.Label();
            this.btnQuantity = new Guna.UI2.WinForms.Guna2Button();
            this.imgPic = new Guna.UI2.WinForms.Guna2PictureBox();
            this.guna2Panel1 = new Guna.UI2.WinForms.Guna2Panel();
            this.btnRemove = new Guna.UI2.WinForms.Guna2Button();
            ((System.ComponentModel.ISupportInitialize)(this.imgPic)).BeginInit();
            this.guna2Panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblName
            // 
            this.lblName.Font = new System.Drawing.Font("Helvetica", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(35)))), ((int)(((byte)(36)))));
            this.lblName.Location = new System.Drawing.Point(106, 12);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(129, 18);
            this.lblName.TabIndex = 2;
            this.lblName.Text = "Jollibee";
            // 
            // lblCategory
            // 
            this.lblCategory.Font = new System.Drawing.Font("Helvetica", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblCategory.ForeColor = System.Drawing.Color.Gray;
            this.lblCategory.Location = new System.Drawing.Point(107, 32);
            this.lblCategory.Name = "lblCategory";
            this.lblCategory.Size = new System.Drawing.Size(164, 19);
            this.lblCategory.TabIndex = 3;
            this.lblCategory.Text = "Meal";
            // 
            // lblSubTotal
            // 
            this.lblSubTotal.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblSubTotal.Font = new System.Drawing.Font("Helvetica", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblSubTotal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(35)))), ((int)(((byte)(36)))));
            this.lblSubTotal.Location = new System.Drawing.Point(107, 64);
            this.lblSubTotal.Name = "lblSubTotal";
            this.lblSubTotal.Size = new System.Drawing.Size(82, 22);
            this.lblSubTotal.TabIndex = 4;
            this.lblSubTotal.Text = "P203.23";
            // 
            // btnQuantity
            // 
            this.btnQuantity.BorderRadius = 5;
            this.btnQuantity.CustomizableEdges = customizableEdges1;
            this.btnQuantity.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnQuantity.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnQuantity.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnQuantity.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnQuantity.Font = new System.Drawing.Font("Helvetica", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnQuantity.ForeColor = System.Drawing.Color.White;
            this.btnQuantity.Location = new System.Drawing.Point(267, 64);
            this.btnQuantity.Name = "btnQuantity";
            this.btnQuantity.ShadowDecoration.CustomizableEdges = customizableEdges2;
            this.btnQuantity.Size = new System.Drawing.Size(61, 25);
            this.btnQuantity.TabIndex = 5;
            this.btnQuantity.Text = "1";
            this.btnQuantity.TextOffset = new System.Drawing.Point(2, 0);
            this.btnQuantity.Click += new System.EventHandler(this.btnQuantity_Click);
            // 
            // imgPic
            // 
            this.imgPic.BackColor = System.Drawing.Color.Transparent;
            this.imgPic.BorderRadius = 4;
            this.imgPic.CustomizableEdges = customizableEdges3;
            this.imgPic.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.imgPic.ImageRotate = 0F;
            this.imgPic.Location = new System.Drawing.Point(10, 9);
            this.imgPic.Name = "imgPic";
            this.imgPic.ShadowDecoration.CustomizableEdges = customizableEdges4;
            this.imgPic.Size = new System.Drawing.Size(80, 80);
            this.imgPic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.imgPic.TabIndex = 6;
            this.imgPic.TabStop = false;
            // 
            // guna2Panel1
            // 
            this.guna2Panel1.BackColor = System.Drawing.Color.Transparent;
            this.guna2Panel1.BorderRadius = 7;
            this.guna2Panel1.Controls.Add(this.btnRemove);
            this.guna2Panel1.Controls.Add(this.imgPic);
            this.guna2Panel1.Controls.Add(this.btnQuantity);
            this.guna2Panel1.Controls.Add(this.lblName);
            this.guna2Panel1.Controls.Add(this.lblCategory);
            this.guna2Panel1.Controls.Add(this.lblSubTotal);
            this.guna2Panel1.CustomizableEdges = customizableEdges7;
            this.guna2Panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.guna2Panel1.FillColor = System.Drawing.Color.White;
            this.guna2Panel1.Location = new System.Drawing.Point(5, 5);
            this.guna2Panel1.Name = "guna2Panel1";
            this.guna2Panel1.ShadowDecoration.BorderRadius = 7;
            this.guna2Panel1.ShadowDecoration.Color = System.Drawing.Color.DarkGray;
            this.guna2Panel1.ShadowDecoration.CustomizableEdges = customizableEdges8;
            this.guna2Panel1.ShadowDecoration.Depth = 10;
            this.guna2Panel1.ShadowDecoration.Enabled = true;
            this.guna2Panel1.Size = new System.Drawing.Size(338, 100);
            this.guna2Panel1.TabIndex = 8;
            // 
            // btnRemove
            // 
            this.btnRemove.BorderRadius = 5;
            this.btnRemove.CustomizableEdges = customizableEdges5;
            this.btnRemove.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnRemove.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnRemove.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnRemove.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnRemove.FillColor = System.Drawing.Color.Transparent;
            this.btnRemove.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnRemove.ForeColor = System.Drawing.Color.White;
            this.btnRemove.Image = ((System.Drawing.Image)(resources.GetObject("btnRemove.Image")));
            this.btnRemove.ImageSize = new System.Drawing.Size(15, 15);
            this.btnRemove.Location = new System.Drawing.Point(293, 17);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.ShadowDecoration.CustomizableEdges = customizableEdges6;
            this.btnRemove.Size = new System.Drawing.Size(33, 33);
            this.btnRemove.TabIndex = 7;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // CartControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.guna2Panel1);
            this.Margin = new System.Windows.Forms.Padding(3, 3, 3, 6);
            this.Name = "CartControl";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.Size = new System.Drawing.Size(348, 110);
            this.Load += new System.EventHandler(this.CartControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.imgPic)).EndInit();
            this.guna2Panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private Label lblName;
        private Label lblCategory;
        private Label lblSubTotal;
        private Guna.UI2.WinForms.Guna2Button btnQuantity;
        private Guna.UI2.WinForms.Guna2PictureBox imgPic;
        private Guna.UI2.WinForms.Guna2Panel guna2Panel1;
        private Guna.UI2.WinForms.Guna2Button btnRemove;
    }
}
