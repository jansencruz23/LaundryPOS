using LaundryPOS.Contracts;
using LaundryPOS.Delegates;
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

namespace LaundryPOS.Forms.Views
{
    public partial class AdminProfileView : BaseViewControl
    {
        public AdminProfileView(IUnitOfWork unitOfWork,
            ThemeManager themeManager,
            ChangeAdminViewDelegate changeAdminView)
            : base (unitOfWork, themeManager, changeAdminView)
        {
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
                MessageBox.Show("Admin is empty", "Empty Admin",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!admin.ValidatePassword(txtOldPassword.Text))
            {
                MessageBox.Show("Old admin password is incorrect", "Admin Settings Update Failed",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!txtNewPassword.Text.Equals(txtConfirmPassword.Text))
            {
                MessageBox.Show("Passwords do not match", "Admin Settings Update Failed",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void btnItem_Click(object sender, EventArgs e)
        {
            ChangeAdminView(CreateView<ItemView>());
        }

        private void btnCategory_Click(object sender, EventArgs e)
        {
            ChangeAdminView(CreateView<CategoryView>());
        }

        private void btnEmployee_Click(object sender, EventArgs e)
        {
            ChangeAdminView(CreateView<EmployeeView>());
        }

        private void btnTransaction_Click(object sender, EventArgs e)
        {
            ChangeAdminView(CreateView<TransactionView>());
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            ConfirmLogout();
        }
    }
}