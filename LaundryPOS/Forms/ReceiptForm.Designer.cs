namespace LaundryPOS.Forms
{
    partial class ReceiptForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReceiptForm));
            this.reportViewer = new Microsoft.Reporting.WinForms.ReportViewer();
            this.panel1 = new Guna.UI2.WinForms.Guna2Panel();
            this.SuspendLayout();
            // 
            // reportViewer
            // 
            this.reportViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.reportViewer.DocumentMapWidth = 81;
            this.reportViewer.Location = new System.Drawing.Point(0, 0);
            this.reportViewer.Name = "ReportViewer";
            this.reportViewer.Padding = new System.Windows.Forms.Padding(10);
            this.reportViewer.ServerReport.BearerToken = null;
            this.reportViewer.Size = new System.Drawing.Size(383, 450);
            this.reportViewer.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.CustomizableEdges = customizableEdges1;
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(10);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(10);
            this.panel1.ShadowDecoration.CustomizableEdges = customizableEdges2;
            this.panel1.Size = new System.Drawing.Size(383, 450);
            this.panel1.TabIndex = 1;
            // 
            // ReceiptForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(383, 450);
            this.Controls.Add(this.reportViewer);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ReceiptForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Print Receipt";
            this.ResumeLayout(false);

        }

        #endregion

        private Microsoft.Reporting.WinForms.ReportViewer reportViewer;
        private Guna.UI2.WinForms.Guna2Panel panel1;
    }
}