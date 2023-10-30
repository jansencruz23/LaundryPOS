namespace LaundryPOS.Forms.CustomControls
{
    partial class CategoryControl
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
            this.imgIcon = new Guna.UI2.WinForms.Guna2PictureBox();
            this.lblName = new System.Windows.Forms.Label();
            this.panelBg = new Guna.UI2.WinForms.Guna2Panel();
            ((System.ComponentModel.ISupportInitialize)(this.imgIcon)).BeginInit();
            this.panelBg.SuspendLayout();
            this.SuspendLayout();
            // 
            // imgIcon
            // 
            this.imgIcon.BorderRadius = 2;
            this.imgIcon.CustomizableEdges = customizableEdges1;
            this.imgIcon.Dock = System.Windows.Forms.DockStyle.Left;
            this.imgIcon.FillColor = System.Drawing.Color.DimGray;
            this.imgIcon.ImageRotate = 0F;
            this.imgIcon.Location = new System.Drawing.Point(20, 5);
            this.imgIcon.Margin = new System.Windows.Forms.Padding(0);
            this.imgIcon.Name = "imgIcon";
            this.imgIcon.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.imgIcon.ShadowDecoration.CustomizableEdges = customizableEdges2;
            this.imgIcon.Size = new System.Drawing.Size(25, 25);
            this.imgIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.imgIcon.TabIndex = 0;
            this.imgIcon.TabStop = false;
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblName.Font = new System.Drawing.Font("Helvetica", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(24)))), ((int)(((byte)(24)))));
            this.lblName.Location = new System.Drawing.Point(45, 5);
            this.lblName.Name = "lblName";
            this.lblName.Padding = new System.Windows.Forms.Padding(4, 4, 10, 4);
            this.lblName.Size = new System.Drawing.Size(54, 25);
            this.lblName.TabIndex = 1;
            this.lblName.Text = "Meal";
            // 
            // panelBg
            // 
            this.panelBg.AutoSize = true;
            this.panelBg.BackColor = System.Drawing.Color.Transparent;
            this.panelBg.BorderRadius = 5;
            this.panelBg.Controls.Add(this.lblName);
            this.panelBg.Controls.Add(this.imgIcon);
            this.panelBg.Cursor = System.Windows.Forms.Cursors.Hand;
            this.panelBg.CustomizableEdges = customizableEdges3;
            this.panelBg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBg.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.panelBg.Location = new System.Drawing.Point(0, 0);
            this.panelBg.Margin = new System.Windows.Forms.Padding(3, 3, 10, 3);
            this.panelBg.Name = "panelBg";
            this.panelBg.Padding = new System.Windows.Forms.Padding(20, 5, 8, 5);
            this.panelBg.ShadowDecoration.BorderRadius = 5;
            this.panelBg.ShadowDecoration.Color = System.Drawing.Color.Silver;
            this.panelBg.ShadowDecoration.CustomizableEdges = customizableEdges4;
            this.panelBg.ShadowDecoration.Depth = 5;
            this.panelBg.ShadowDecoration.Enabled = true;
            this.panelBg.Size = new System.Drawing.Size(107, 35);
            this.panelBg.TabIndex = 2;
            // 
            // CategoryControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.panelBg);
            this.Name = "CategoryControl";
            this.Size = new System.Drawing.Size(107, 35);
            ((System.ComponentModel.ISupportInitialize)(this.imgIcon)).EndInit();
            this.panelBg.ResumeLayout(false);
            this.panelBg.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Guna.UI2.WinForms.Guna2PictureBox imgIcon;
        private Label lblName;
        private Guna.UI2.WinForms.Guna2Panel panelBg;
    }
}
