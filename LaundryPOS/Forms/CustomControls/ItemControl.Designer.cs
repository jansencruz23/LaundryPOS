namespace LaundryPOS.Forms.Views
{
    partial class ItemControl
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
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            this.guna2Panel1 = new Guna.UI2.WinForms.Guna2Panel();
            this.lblCategory = new System.Windows.Forms.Label();
            this.lblStock = new System.Windows.Forms.Label();
            this.imgIcon = new Guna.UI2.WinForms.Guna2PictureBox();
            this.lblPrice = new System.Windows.Forms.Label();
            this.lblName = new System.Windows.Forms.Label();
            this.guna2Panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imgIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // guna2Panel1
            // 
            this.guna2Panel1.BackColor = System.Drawing.Color.Transparent;
            this.guna2Panel1.BorderRadius = 10;
            this.guna2Panel1.Controls.Add(this.lblCategory);
            this.guna2Panel1.Controls.Add(this.lblStock);
            this.guna2Panel1.Controls.Add(this.imgIcon);
            this.guna2Panel1.Controls.Add(this.lblPrice);
            this.guna2Panel1.Controls.Add(this.lblName);
            this.guna2Panel1.CustomizableEdges = customizableEdges3;
            this.guna2Panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.guna2Panel1.FillColor = System.Drawing.Color.White;
            this.guna2Panel1.Location = new System.Drawing.Point(0, 0);
            this.guna2Panel1.Name = "guna2Panel1";
            this.guna2Panel1.ShadowDecoration.CustomizableEdges = customizableEdges4;
            this.guna2Panel1.Size = new System.Drawing.Size(195, 143);
            this.guna2Panel1.TabIndex = 0;
            // 
            // lblCategory
            // 
            this.lblCategory.AutoSize = true;
            this.lblCategory.Font = new System.Drawing.Font("Poppins", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblCategory.Location = new System.Drawing.Point(13, 34);
            this.lblCategory.Name = "lblCategory";
            this.lblCategory.Size = new System.Drawing.Size(46, 22);
            this.lblCategory.TabIndex = 4;
            this.lblCategory.Text = "label2";
            // 
            // lblStock
            // 
            this.lblStock.AutoSize = true;
            this.lblStock.Font = new System.Drawing.Font("Poppins", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblStock.Location = new System.Drawing.Point(121, 13);
            this.lblStock.Name = "lblStock";
            this.lblStock.Size = new System.Drawing.Size(43, 22);
            this.lblStock.TabIndex = 3;
            this.lblStock.Text = "label1";
            // 
            // imgIcon
            // 
            this.imgIcon.BackColor = System.Drawing.Color.Transparent;
            this.imgIcon.CustomizableEdges = customizableEdges1;
            this.imgIcon.FillColor = System.Drawing.Color.Transparent;
            this.imgIcon.ImageRotate = 0F;
            this.imgIcon.Location = new System.Drawing.Point(90, 42);
            this.imgIcon.Name = "imgIcon";
            this.imgIcon.ShadowDecoration.CustomizableEdges = customizableEdges2;
            this.imgIcon.Size = new System.Drawing.Size(83, 84);
            this.imgIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.imgIcon.TabIndex = 0;
            this.imgIcon.TabStop = false;
            // 
            // lblPrice
            // 
            this.lblPrice.AutoSize = true;
            this.lblPrice.Font = new System.Drawing.Font("Poppins", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblPrice.Location = new System.Drawing.Point(13, 103);
            this.lblPrice.Name = "lblPrice";
            this.lblPrice.Size = new System.Drawing.Size(55, 23);
            this.lblPrice.TabIndex = 2;
            this.lblPrice.Text = "label2";
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Font = new System.Drawing.Font("Poppins", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblName.Location = new System.Drawing.Point(13, 13);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(52, 23);
            this.lblName.TabIndex = 1;
            this.lblName.Text = "label1";
            // 
            // ItemControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.guna2Panel1);
            this.Name = "ItemControl";
            this.Size = new System.Drawing.Size(195, 143);
            this.guna2Panel1.ResumeLayout(false);
            this.guna2Panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imgIcon)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Guna.UI2.WinForms.Guna2Panel guna2Panel1;
        private Guna.UI2.WinForms.Guna2PictureBox imgIcon;
        private Label lblPrice;
        private Label lblName;
        private Label lblStock;
        private Label lblCategory;
    }
}
