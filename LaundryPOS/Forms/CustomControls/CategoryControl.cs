using LaundryPOS.CustomEventArgs;
using LaundryPOS.Forms.Views;
using LaundryPOS.Helpers;
using LaundryPOS.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LaundryPOS.Forms.CustomControls
{
    public partial class CategoryControl : UserControl
    {
        public event EventHandler<CategoryEventArgs> CategoryClicked;
        private readonly Category _category;
        private readonly ThemeManager _themeManager;

        public CategoryControl(Category category, 
            ThemeManager themeManager)
        {
            _category = category;
            _themeManager = themeManager;

            InitializeComponent();
            InitializeCategory();
            WireAllControls(this);
        }

        private void InitializeCategory()
        {
            lblName.Text = _category.Name;
            imgIcon.Image = GetImage();
        }

        private Image GetImage()
        {
            try
            {
                return _category.Image != null
                ? Image.FromFile(_category.Image)
                : Image.FromFile("./default.png");
            }
            catch
            {
                return Image.FromFile("./default.png");
            }
        }

        private void WireAllControls(Control control)
        {
            foreach (Control ctrl in control.Controls)
            {
                ctrl.Click += Control_Click!;
                if (ctrl.HasChildren)
                {
                    WireAllControls(ctrl);
                }
            }
        }

        private void Control_Click(object sender, EventArgs e)
        {
            InvokeOnClick(this, EventArgs.Empty);
            CategoryClicked?.Invoke(this,
                new CategoryEventArgs(_category));

            panelBg.FillColor = Color.FromArgb(48,48,48);
            lblName.ForeColor = Color.White;
        }

        public void ChangeToDefault()
        {
            panelBg.FillColor = Color.FromArgb(248, 248, 248);
            lblName.ForeColor = Color.FromArgb(24,24,24);
        }
    }
}