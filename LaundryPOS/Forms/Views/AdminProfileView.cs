using LaundryPOS.Contracts;
using LaundryPOS.Delegates;
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

namespace LaundryPOS.Forms.Views
{
    public partial class AdminProfileView : UserControl
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ThemeManager _themeManager;
        private readonly ChangeAdminViewDelegate _changeAdminView;

        public AdminProfileView(IUnitOfWork unitOfWork,
            ThemeManager themeManager,
            ChangeAdminViewDelegate changeAdminView)
        {
            _unitOfWork = unitOfWork;
            _themeManager = themeManager;
            _changeAdminView = changeAdminView;

            InitializeComponent();
        }

        private async void AdminProfileView_Load(object sender, EventArgs e)
        {
            await ApplyTheme();
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            var admin = await _unitOfWork.EmployeeRepo.GetAdmin();

            if (admin == null)
            {
                MessageBox.Show("Admin is empty");
                return;
            }

            if (!admin.ValidatePassword(txtOldPassword.Text))
            {
                MessageBox.Show("Old admin password is incorrect");
                return;
            }

            if (!txtNewPassword.Text.Equals(txtConfirmPassword.Text))
            {
                MessageBox.Show("Passwords do not match");
                return;
            }

            admin.Username = txtUsername.Text;
            admin.SetPassword(txtNewPassword.Text);

            _unitOfWork.EmployeeRepo.Update(admin);
            await _unitOfWork.SaveAsync();
        }

        private async Task ApplyTheme()
        {
            await _themeManager.ApplyThemeToButton(btnAdminProfile);
            await _themeManager.ApplyThemeToButton(btnSave);
        }

        private void ChangeAdminView<T>(Func<IUnitOfWork, ThemeManager, ChangeAdminViewDelegate, T> createViewFunc)
            where T : UserControl
        {
            Dispose();
            var view = createViewFunc(_unitOfWork, _themeManager, _changeAdminView);
            _changeAdminView?.Invoke(view);
        }

        private void btnItem_Click(object sender, EventArgs e)
        {
            ChangeAdminView((_unitOfWork, _themeManager, _changeAdminView)
            => new ItemView(_unitOfWork, _themeManager, _changeAdminView));
        }

        private void btnCategory_Click(object sender, EventArgs e)
        {
            ChangeAdminView((_unitOfWork, _themeManager, _changeAdminView)
            => new CategoryView(_unitOfWork, _themeManager, _changeAdminView));
        }

        private void btnEmployee_Click(object sender, EventArgs e)
        {
            ChangeAdminView((_unitOfWork, _themeManager, _changeAdminView)
            => new EmployeeView(_unitOfWork, _themeManager, _changeAdminView));
        }

        private void btnTransaction_Click(object sender, EventArgs e)
        {
            ChangeAdminView((_unitOfWork, _themeManager, _changeAdminView)
            => new TransactionView(_unitOfWork, _themeManager, _changeAdminView));
        }
    }
}