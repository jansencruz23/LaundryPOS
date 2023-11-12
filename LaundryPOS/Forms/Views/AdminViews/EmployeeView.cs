using LaundryPOS.Contracts;
using LaundryPOS.Delegates;
using LaundryPOS.Models;
using LaundryPOS.Forms.Views.BaseViews;
using Guna.UI2.WinForms;
using System.Text.RegularExpressions;
using LaundryPOS.Managers;

namespace LaundryPOS.Forms.Views
{
    public partial class EmployeeView : BaseEmployeeView
    {
        private Employee _employee;
        private LoadingForm _loadingForm;

        public EmployeeView(IUnitOfWork unitOfWork,
            IStyleManager styleManager,
            ChangeAdminViewDelegate changeAdminView)
            : base(unitOfWork, styleManager, changeAdminView)
        {
            InitializeComponent();
            ShowLoadingForm();
            StyleFonts();
        }

        private void ShowLoadingForm()
        {
            if (_loadingForm != null && !_loadingForm.IsDisposed)
            {
                return;
            }

            panelCover.Dock = DockStyle.Fill;
            _loadingForm = new LoadingForm();
            _loadingForm.Show();
            _loadingForm.Refresh();

            Application.Idle += OnLoaded;
        }

        private void OnLoaded(object sender, EventArgs e)
        {
            Application.Idle -= OnLoaded;
            _loadingForm.Close();
            panelCover.Dock = DockStyle.None;
            panelCover.Size = new Size(1, 1);
        }

        private async void EmployeeView_Load(object sender, EventArgs e)
        {
            await ApplyTheme();
            await DisplayEmployees();
        }

        private async void btnRegister_Click(object sender, EventArgs e)
        {
            new RegisterForm(_unitOfWork, _styleManager).ShowDialog();
            await RefreshData();
        }

        private async Task DisplayEmployees(string query = "")
        {
            var employeeList = await _unitOfWork.EmployeeRepo
                .Get(filter: e => e.IsActive
                    && (e.LastName.Contains(query)
                    || e.FirstName.Contains(query)));
            employeeTable.DataSource = employeeList;

            HideUnwantedColumns();
            ConfigureImageColumn(employeeTable);
            HandleImageColumnFormatting(employeeTable);
        }

        private void HideUnwantedColumns()
        {
            employeeTable.Columns[nameof(Employee.Id)].Visible = false;
            employeeTable.Columns[nameof(Employee.Image)].Visible = false;
            employeeTable.Columns[nameof(Employee.HashedPassword)].Visible = false;
            employeeTable.Columns[nameof(Employee.Salt)].Visible = false;
            employeeTable.Columns[nameof(Employee.IsActive)].Visible = false;
        }

        private async Task RefreshData(string query = "")
        {
            employeeTable.DataSource = null;
            await DisplayEmployees(query);
        }

        private async Task ApplyTheme()
        {
            await _styleManager.Theme.ApplyThemeToButton(btnEmployee);
            await _styleManager.Theme.ApplyThemeToButton(btnSearch);
            await _styleManager.Theme.ApplyLighterThemeToDataGridView(employeeTable, 1f, true);
        }

        private void ChangeAdminView<T>(Func<IUnitOfWork, IStyleManager, ChangeAdminViewDelegate, T> createViewFunc)
            where T : UserControl
        {
            Dispose();
            var view = createViewFunc(_unitOfWork, _styleManager, _changeAdminView);
            _changeAdminView?.Invoke(view);
        }

        private void btnCategory_Click(object sender, EventArgs e)
        {
            ChangeAdminView(CreateView<CategoryView>());
        }

        private void btnItem_Click(object sender, EventArgs e)
        {
            ChangeAdminView(CreateView<ItemView>());
        }

        private void btnTransaction_Click(object sender, EventArgs e)
        {
            ChangeAdminView(CreateView<TransactionView>());
        }

        private void btnAdminProfile_Click(object sender, EventArgs e)
        {
            ChangeAdminView(CreateView<AdminProfileView>());
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            if (_employee != null)
            {
                DialogResult result = MessageBox.Show("Are you sure you want to delete this employee?", 
                    "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    _employee.IsActive = false;
                    _unitOfWork.EmployeeRepo.Update(_employee);
                    await _unitOfWork.SaveAsync();

                    MessageBox.Show("Employee deleted successfully.", "Employee Deleted Successfully", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    await RefreshData();
                    ClearText();
                    btnDelete.Enabled = false;
                }
            }
        }

        private void ClearText()
        {
            imgPic.Image = default;
            txtAge.Text = string.Empty;
            txtBirthday.Text = string.Empty;
            txtName.Text = string.Empty;
            txtUsername.Text = string.Empty;
        }

        private async void employeeTable_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var selectedRow = employeeTable.Rows[e.RowIndex];
                var employeeId = (selectedRow.DataBoundItem as Employee)?.Id;
                _employee = await _unitOfWork.EmployeeRepo
                    .GetByID(employeeId.Value);

                txtName.Text = _employee.FullName;
                txtUsername.Text = _employee.Username;
                txtAge.Text = _employee.Age.ToString();
                txtBirthday.Text = _employee.BirthDate.ToShortDateString();
                imgPic.Image = GetImage(_employee);

                btnDelete.Enabled = true;
            }
        }

        private async void btnPrint_Click(object sender, EventArgs e)
        {
            await PrintTable(employeeTable, "Item/Service Employee List");
        }


        private void btnLogout_Click(object sender, EventArgs e)
        {
            ConfirmLogout();
        }

        private async void btnSearch_Click(object sender, EventArgs e)
        {
            await RefreshData(txtSearch.Text);
        }

        private void StyleFonts()
        {
            _styleManager.Font.StyleFontsButton(11.25f, btnItem, btnCategory, btnEmployee, btnEmployee,
                btnAdminProfile, btnRegister, btnDelete, btnPrint, btnLogout, btnSearch);
            _styleManager.Font.StyleFontsLabel(18f, true, lblDetails, lblList);
            _styleManager.Font.StyleFontsLabel(11.25f, false, lblName, lblAge, lblUsername, lblBirthday);
            _styleManager.Font.StyleFontsTextBox(11.25f, txtName, txtSearch);
        }
    }
}