﻿using LaundryPOS.Contracts;
using LaundryPOS.Forms.Views;
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
    public partial class AdminForm : Form
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ThemeManager _themeManager;

        public AdminForm(IUnitOfWork unitOfWorK,
            ThemeManager themeManager)
        {
            _unitOfWork = unitOfWorK;
            _themeManager = themeManager;
            InitializeComponent();
            viewPanel.Controls.Add(new TransactionView(_unitOfWork, themeManager));
        }

        private void ChangePanelContent(UserControl newContent)
        {
            viewPanel.Controls.Clear();
            viewPanel.Controls.Add(newContent);
        }

        public void DisplayEmployeeView()
        {
            ChangePanelContent(new EmployeeView(_unitOfWork));
        }
    }
}