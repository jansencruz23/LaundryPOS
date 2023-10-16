﻿using Guna.UI2.WinForms;
using LaundryPOS.Forms.Views;
using LaundryPOS.Models;
using LaundryPOS.Helpers;
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
            FocusTextbox();
            await ApplyTheme();
        }

        private void FocusTextbox()
        {
            BeginInvoke((MethodInvoker)delegate
            {
                txtQuantity.Focus();
                txtQuantity.SelectAll();
            });
        }

        private async Task ApplyTheme()
        {
            await _themeManager.ApplyThemeToPanel(panelBg, 1f);
            await _themeManager.ApplyOutlineThemeToButton(btnClose, 2);
            await _themeManager.ApplyOutlineThemeToButton(btnMinus, 2);
            await _themeManager.ApplyOutlineThemeToButton(btnAdd, 2);
            await _themeManager.ApplyOutlineThemeToButton(btnEnter,2);
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
                DialogResult = DialogResult.OK;
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
            try
            {
                Quantity = int.Parse(txtQuantity.Text);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch
            {
                MessageBox.Show("Please input valid quantity");
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void txtQuantity_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return || e.KeyChar == (char)Keys.Space)
            {
                btnEnter.PerformClick();
            }

            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}