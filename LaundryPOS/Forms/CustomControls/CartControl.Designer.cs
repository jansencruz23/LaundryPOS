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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CartControl));
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            this.btnQuantity = new Guna.UI2.WinForms.Guna2CircleButton();
            this.btnRemove = new Guna.UI2.WinForms.Guna2CircleButton();
            this.lblName = new System.Windows.Forms.Label();
            this.lblPrice = new System.Windows.Forms.Label();
            this.lblSubTotal = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnQuantity
            // 
            this.btnQuantity.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnQuantity.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnQuantity.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnQuantity.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnQuantity.Font = new System.Drawing.Font("Poppins", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnQuantity.ForeColor = System.Drawing.Color.White;
            this.btnQuantity.Location = new System.Drawing.Point(5, 6);
            this.btnQuantity.Name = "btnQuantity";
            this.btnQuantity.ShadowDecoration.CustomizableEdges = customizableEdges1;
            this.btnQuantity.ShadowDecoration.Mode = Guna.UI2.WinForms.Enums.ShadowMode.Circle;
            this.btnQuantity.Size = new System.Drawing.Size(58, 59);
            this.btnQuantity.TabIndex = 0;
            this.btnQuantity.Text = "Q";
            // 
            // btnRemove
            // 
            this.btnRemove.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnRemove.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnRemove.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnRemove.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnRemove.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(237)))), ((int)(((byte)(59)))), ((int)(((byte)(73)))));
            this.btnRemove.Font = new System.Drawing.Font("Poppins", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnRemove.ForeColor = System.Drawing.Color.White;
            this.btnRemove.Image = ((System.Drawing.Image)(resources.GetObject("btnRemove.Image")));
            this.btnRemove.ImageOffset = new System.Drawing.Point(0, 10);
            this.btnRemove.ImageSize = new System.Drawing.Size(10, 15);
            this.btnRemove.Location = new System.Drawing.Point(218, 27);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.ShadowDecoration.CustomizableEdges = customizableEdges2;
            this.btnRemove.ShadowDecoration.Mode = Guna.UI2.WinForms.Enums.ShadowMode.Circle;
            this.btnRemove.Size = new System.Drawing.Size(23, 22);
            this.btnRemove.TabIndex = 1;
            this.btnRemove.Text = "-";
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Font = new System.Drawing.Font("Poppins", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblName.Location = new System.Drawing.Point(69, 17);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(49, 22);
            this.lblName.TabIndex = 2;
            this.lblName.Text = "label1";
            // 
            // lblPrice
            // 
            this.lblPrice.AutoSize = true;
            this.lblPrice.Font = new System.Drawing.Font("Poppins", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblPrice.Location = new System.Drawing.Point(69, 37);
            this.lblPrice.Name = "lblPrice";
            this.lblPrice.Size = new System.Drawing.Size(42, 19);
            this.lblPrice.TabIndex = 3;
            this.lblPrice.Text = "label2";
            // 
            // lblSubTotal
            // 
            this.lblSubTotal.AutoSize = true;
            this.lblSubTotal.Font = new System.Drawing.Font("Poppins", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblSubTotal.Location = new System.Drawing.Point(146, 29);
            this.lblSubTotal.Name = "lblSubTotal";
            this.lblSubTotal.Size = new System.Drawing.Size(52, 22);
            this.lblSubTotal.TabIndex = 4;
            this.lblSubTotal.Text = "label3";
            // 
            // CartControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblSubTotal);
            this.Controls.Add(this.lblPrice);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.btnRemove);
            this.Controls.Add(this.btnQuantity);
            this.Name = "CartControl";
            this.Size = new System.Drawing.Size(255, 72);
            this.Load += new System.EventHandler(this.CartControl_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Guna.UI2.WinForms.Guna2CircleButton btnQuantity;
        private Guna.UI2.WinForms.Guna2CircleButton btnRemove;
        private Label lblName;
        private Label lblPrice;
        private Label lblSubTotal;
    }
}
