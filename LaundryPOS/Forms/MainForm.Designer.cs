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
            this.itemsPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.cartPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.lblTotal = new System.Windows.Forms.Label();
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
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(924, 487);
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
    }
}