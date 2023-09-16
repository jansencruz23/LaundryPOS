namespace LaundryPOS.Forms
{
    partial class AdminForm
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
            this.servicePanel = new Guna.UI2.WinForms.Guna2ShadowPanel();
            this.guna2ShadowPanel1 = new Guna.UI2.WinForms.Guna2ShadowPanel();
            this.SuspendLayout();
            // 
            // servicePanel
            // 
            this.servicePanel.BackColor = System.Drawing.Color.Transparent;
            this.servicePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.servicePanel.FillColor = System.Drawing.Color.White;
            this.servicePanel.Location = new System.Drawing.Point(0, 0);
            this.servicePanel.Margin = new System.Windows.Forms.Padding(30, 3, 3, 3);
            this.servicePanel.Name = "servicePanel";
            this.servicePanel.ShadowColor = System.Drawing.Color.Black;
            this.servicePanel.ShadowDepth = 0;
            this.servicePanel.Size = new System.Drawing.Size(800, 502);
            this.servicePanel.TabIndex = 0;
            // 
            // guna2ShadowPanel1
            // 
            this.guna2ShadowPanel1.BackColor = System.Drawing.Color.Transparent;
            this.guna2ShadowPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.guna2ShadowPanel1.FillColor = System.Drawing.Color.White;
            this.guna2ShadowPanel1.Location = new System.Drawing.Point(0, 0);
            this.guna2ShadowPanel1.Name = "guna2ShadowPanel1";
            this.guna2ShadowPanel1.ShadowColor = System.Drawing.Color.Black;
            this.guna2ShadowPanel1.ShadowDepth = 20;
            this.guna2ShadowPanel1.Size = new System.Drawing.Size(800, 54);
            this.guna2ShadowPanel1.TabIndex = 1;
            // 
            // AdminForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 502);
            this.Controls.Add(this.guna2ShadowPanel1);
            this.Controls.Add(this.servicePanel);
            this.Name = "AdminForm";
            this.Text = "AdminForm";
            this.ResumeLayout(false);

        }

        #endregion

        private Guna.UI2.WinForms.Guna2ShadowPanel servicePanel;
        private Guna.UI2.WinForms.Guna2ShadowPanel guna2ShadowPanel1;
    }
}