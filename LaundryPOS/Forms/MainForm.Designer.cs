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
            this.itemsPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.cartPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.lblTotal = new System.Windows.Forms.Label();
            this.btnPayNow = new Guna.UI2.WinForms.Guna2GradientButton();
            this.SuspendLayout();
            // 
            // itemsPanel
            // 
            this.itemsPanel.AutoScroll = true;
            this.itemsPanel.Location = new System.Drawing.Point(188, 71);
            this.itemsPanel.Name = "itemsPanel";
            this.itemsPanel.Size = new System.Drawing.Size(526, 412);
            this.itemsPanel.TabIndex = 0;
            // 
            // cartPanel
            // 
            this.cartPanel.AutoScroll = true;
            this.cartPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.cartPanel.Location = new System.Drawing.Point(720, 71);
            this.cartPanel.Name = "cartPanel";
            this.cartPanel.Size = new System.Drawing.Size(200, 327);
            this.cartPanel.TabIndex = 1;
            this.cartPanel.WrapContents = false;
            // 
            // lblTotal
            // 
            this.lblTotal.AutoSize = true;
            this.lblTotal.Location = new System.Drawing.Point(836, 416);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(38, 15);
            this.lblTotal.TabIndex = 2;
            this.lblTotal.Text = "label1";
            // 
            // btnPayNow
            // 
            this.btnPayNow.CustomizableEdges = customizableEdges1;
            this.btnPayNow.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnPayNow.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnPayNow.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnPayNow.DisabledState.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnPayNow.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnPayNow.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnPayNow.ForeColor = System.Drawing.Color.White;
            this.btnPayNow.Location = new System.Drawing.Point(836, 438);
            this.btnPayNow.Name = "btnPayNow";
            this.btnPayNow.ShadowDecoration.CustomizableEdges = customizableEdges2;
            this.btnPayNow.Size = new System.Drawing.Size(84, 45);
            this.btnPayNow.TabIndex = 3;
            this.btnPayNow.Text = "Pay Now";
            this.btnPayNow.Click += new System.EventHandler(this.btnPayNow_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(924, 487);
            this.Controls.Add(this.btnPayNow);
            this.Controls.Add(this.lblTotal);
            this.Controls.Add(this.cartPanel);
            this.Controls.Add(this.itemsPanel);
            this.Name = "MainForm";
            this.RightToLeftLayout = true;
            this.Text = "MainForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private FlowLayoutPanel itemsPanel;
        private FlowLayoutPanel cartPanel;
        private Label lblTotal;
        private Guna.UI2.WinForms.Guna2GradientButton btnPayNow;
    }
}