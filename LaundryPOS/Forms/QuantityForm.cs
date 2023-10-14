using Guna.UI2.WinForms;
using LaundryPOS.Forms.Views;
using LaundryPOS.Models;
using LaundryPOS.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LaundryPOS.Forms
{
    public partial class QuantityForm : Form
    {
        private readonly ThemeManager _themeManager;
        public int Quantity { get; set; }

        public QuantityForm(ThemeManager themeManager)
        {
            _themeManager = themeManager;
            InitializeComponent();
        }

        private async void QuantityForm_Load(object sender, EventArgs e)
        {
            InitializeQuantity();
            txtQuantity.Focus();
            await ApplyTheme();
        }

        private async Task ApplyTheme()
        {
            await _themeManager.ApplyLighterThemeToPanel(panelBg, 1f);
            await _themeManager.ApplyOutlineThemeToButton(btnClose);
            await _themeManager.ApplyOutlineThemeToButton(btnMinus);
            await _themeManager.ApplyOutlineThemeToButton(btnAdd);
            await _themeManager.ApplyOutlineThemeToButton(btnEnter);
            await _themeManager.ApplyThemeToButton(btn2);
            await _themeManager.ApplyThemeToButton(btn3);
            await _themeManager.ApplyThemeToButton(btn5);
            await _themeManager.ApplyThemeToButton(btn10);
        }

        private void InitializeQuantity()
        {
            txtQuantity.Text = Quantity <= 0 
                ? "1" 
                : Quantity.ToString();
        }

        private void btn_Click(object sender, EventArgs e)
        {
            if (sender is Guna2Button button)
            {
                Quantity = int.Parse(button.Text);
                Close();
            }
        }

        private void btnMinus_Click(object sender, EventArgs e)
        {
            int.TryParse(txtQuantity.Text, out int quantity);
            txtQuantity.Text = (--quantity).ToString();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            int.TryParse(txtQuantity.Text, out int quantity);
            txtQuantity.Text = (++quantity).ToString();
        }

        private void btnEnter_Click(object sender, EventArgs e)
        {
            Quantity = int.Parse(txtQuantity.Text);
            Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Quantity = 0;
            Close();
        }

        private void QuantityForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return ||
                e.KeyCode == Keys.Space)
            {
                btnEnter.PerformClick();
            }
        }
    }
}