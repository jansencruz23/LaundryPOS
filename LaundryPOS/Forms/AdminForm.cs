using LaundryPOS.Contracts;
using LaundryPOS.Delegates;
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
        private ChangeAdminViewDelegate changeAdminViewDelegate;

        public AdminForm(IUnitOfWork unitOfWorK,
            ThemeManager themeManager)
        {
            _unitOfWork = unitOfWorK;
            _themeManager = themeManager;
            changeAdminViewDelegate = new ChangeAdminViewDelegate(ChangePanelContent);

            InitializeComponent();
            viewPanel.Controls.Add(new ItemView(_unitOfWork, themeManager, changeAdminViewDelegate));
        }

        private void ChangePanelContent(UserControl newContent)
        {
            viewPanel.Controls.Clear();
            viewPanel.Controls.Add(newContent);
        }
    }
}