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
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            this.btnViewUnpaid = new Guna.UI2.WinForms.Guna2Button();
            this.itemsPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.btnPayLater = new Guna.UI2.WinForms.Guna2Button();
            this.cartPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.btnPayNow = new Guna.UI2.WinForms.Guna2Button();
            this.lblTotal = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnViewUnpaid
            // 
            this.btnViewUnpaid.CustomizableEdges = customizableEdges1;
            this.btnViewUnpaid.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnViewUnpaid.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnViewUnpaid.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnViewUnpaid.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnViewUnpaid.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnViewUnpaid.ForeColor = System.Drawing.Color.White;
            this.btnViewUnpaid.Location = new System.Drawing.Point(794, 37);
            this.btnViewUnpaid.Name = "btnViewUnpaid";
            this.btnViewUnpaid.ShadowDecoration.CustomizableEdges = customizableEdges2;
            this.btnViewUnpaid.Size = new System.Drawing.Size(75, 45);
            this.btnViewUnpaid.TabIndex = 11;
            this.btnViewUnpaid.Text = "VIEW UNPAID";
            this.btnViewUnpaid.Click += new System.EventHandler(this.btnViewUnpaid_Click);
            // 
            // itemsPanel
            // 
            this.itemsPanel.AutoScroll = true;
            this.itemsPanel.Location = new System.Drawing.Point(56, 37);
            this.itemsPanel.Name = "itemsPanel";
            this.itemsPanel.Size = new System.Drawing.Size(526, 412);
            this.itemsPanel.TabIndex = 6;
            // 
            // btnPayLater
            // 
            this.btnPayLater.CustomizableEdges = customizableEdges3;
            this.btnPayLater.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnPayLater.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnPayLater.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnPayLater.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnPayLater.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnPayLater.ForeColor = System.Drawing.Color.White;
            this.btnPayLater.Location = new System.Drawing.Point(604, 404);
            this.btnPayLater.Name = "btnPayLater";
            this.btnPayLater.ShadowDecoration.CustomizableEdges = customizableEdges4;
            this.btnPayLater.Size = new System.Drawing.Size(94, 45);
            this.btnPayLater.TabIndex = 10;
            this.btnPayLater.Text = "PAY LATER";
            this.btnPayLater.Click += new System.EventHandler(this.btnPayLater_Click);
            // 
            // cartPanel
            // 
            this.cartPanel.AutoScroll = true;
            this.cartPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.cartPanel.Location = new System.Drawing.Point(588, 37);
            this.cartPanel.Name = "cartPanel";
            this.cartPanel.Size = new System.Drawing.Size(200, 327);
            this.cartPanel.TabIndex = 7;
            this.cartPanel.WrapContents = false;
            // 
            // btnPayNow
            // 
            this.btnPayNow.CustomizableEdges = customizableEdges5;
            this.btnPayNow.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnPayNow.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnPayNow.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnPayNow.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnPayNow.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnPayNow.ForeColor = System.Drawing.Color.White;
            this.btnPayNow.Location = new System.Drawing.Point(704, 404);
            this.btnPayNow.Name = "btnPayNow";
            this.btnPayNow.ShadowDecoration.CustomizableEdges = customizableEdges6;
            this.btnPayNow.Size = new System.Drawing.Size(84, 45);
            this.btnPayNow.TabIndex = 9;
            this.btnPayNow.Text = "PAY NOW";
            this.btnPayNow.Click += new System.EventHandler(this.btnPayNow_Click);
            // 
            // lblTotal
            // 
            this.lblTotal.AutoSize = true;
            this.lblTotal.Location = new System.Drawing.Point(704, 382);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(38, 15);
            this.lblTotal.TabIndex = 8;
            this.lblTotal.Text = "label1";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(924, 487);
            this.Controls.Add(this.btnViewUnpaid);
            this.Controls.Add(this.itemsPanel);
            this.Controls.Add(this.btnPayLater);
            this.Controls.Add(this.cartPanel);
            this.Controls.Add(this.btnPayNow);
            this.Controls.Add(this.lblTotal);
            this.Name = "MainForm";
            this.RightToLeftLayout = true;
            this.Text = "MainForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Guna.UI2.WinForms.Guna2Button btnViewUnpaid;
        private FlowLayoutPanel itemsPanel;
        private Guna.UI2.WinForms.Guna2Button btnPayLater;
        private FlowLayoutPanel cartPanel;
        private Guna.UI2.WinForms.Guna2Button btnPayNow;
        private Label lblTotal;
    }
}