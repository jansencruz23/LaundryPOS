using LaundryPOS.Contracts;
using LaundryPOS.Delegates;
using LaundryPOS.Models;
using LaundryPOS.Helpers;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LaundryPOS.Forms.Views.BaseViews;

namespace LaundryPOS.Forms.Views
{
    public partial class EmployeeView : BaseEmployeeView
    {
        private Employee _employee;
        private LoadingForm _loadingForm;

        public EmployeeView(IUnitOfWork unitOfWork,
            ThemeManager themeManager,
            ChangeAdminViewDelegate changeAdminView)
            : base(unitOfWork, themeManager, changeAdminView)
        {
            InitializeComponent();
            ShowLoadingForm();
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
            new RegisterForm(_unitOfWork, _themeManager).ShowDialog();
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
            ConfigureImageColumn();
            HandleImageColumnFormatting();
        }

        private void HideUnwantedColumns()
        {
            employeeTable.Columns[nameof(Employee.Id)].Visible = false;
            employeeTable.Columns[nameof(Employee.Image)].Visible = false;
            employeeTable.Columns[nameof(Employee.HashedPassword)].Visible = false;
            employeeTable.Columns[nameof(Employee.Salt)].Visible = false;
            employeeTable.Columns[nameof(Employee.IsActive)].Visible = false;
        }

        private void ConfigureImageColumn()
        {
            if (employeeTable.Columns["Image"] == null)
            {
                var imageColumn = new DataGridViewImageColumn
                {
                    HeaderText = "Image",
                    Name = "Image",
                    ImageLayout = DataGridViewImageCellLayout.Zoom
                };
                employeeTable.Columns.Add(imageColumn);
                employeeTable.Columns["Image"].DisplayIndex = 0;
            }
        }

        private void HandleImageColumnFormatting()
        {
            employeeTable.CellFormatting += (sender, e) =>
            {
                if (e.ColumnIndex == employeeTable.Columns["Image"].Index && e.RowIndex >= 0)
                {
                    var rowData = employeeTable.Rows[e.RowIndex].DataBoundItem as Employee;
                    var imagePath = rowData?.Image;
                    e.Value = !string.IsNullOrEmpty(imagePath)
                        ? Image.FromFile(imagePath)
                        : null;
                }
            };
        }

        private async Task RefreshData(string query = "")
        {
            employeeTable.DataSource = null;
            await DisplayEmployees(query);
        }

        private async Task ApplyTheme()
        {
            await _themeManager.ApplyThemeToButton(btnEmployee);
            await _themeManager.ApplyThemeToButton(btnSearch);
            await _themeManager.ApplyLighterThemeToDataGridView(employeeTable, 1f, true);
        }

        private void ChangeAdminView<T>(Func<IUnitOfWork, ThemeManager, ChangeAdminViewDelegate, T> createViewFunc)
            where T : UserControl
        {
            Dispose();
            var view = createViewFunc(_unitOfWork, _themeManager, _changeAdminView);
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
            lblAge.Text = string.Empty;
            lblBirthday.Text = string.Empty;
            lblName.Text = string.Empty;
            lblUsername.Text = string.Empty;
        }

        private async void employeeTable_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var selectedRow = employeeTable.Rows[e.RowIndex];
                var employeeId = (selectedRow.DataBoundItem as Employee)?.Id;
                _employee = await _unitOfWork.EmployeeRepo
                    .GetByID(employeeId.Value);

                lblName.Text = _employee.FullName;
                lblUsername.Text = _employee.Username;
                lblAge.Text = _employee.Age.ToString();
                lblBirthday.Text = _employee.BirthDate.ToShortDateString();
                imgPic.Image = GetImage(_employee);

                btnDelete.Enabled = true;
            }
        }

        private async void btnPrint_Click(object sender, EventArgs e)
        {
            var printer = new DGVPrinter();
            var businessName = await GetBusinessName();

            printer.Title = businessName;
            printer.SubTitle = string.Format("Item/Service Employee List", printer.SubTitleColor = Color.Black, printer);
            printer.SubTitleSpacing = 15;
            printer.SubTitleFormatFlags = StringFormatFlags.LineLimit | StringFormatFlags.NoClip;
            printer.PageNumbers = true;
            printer.PageNumberInHeader = false;
            printer.PorportionalColumns = true;
            printer.RowHeight = DGVPrinter.RowHeightSetting.CellHeight;
            printer.HeaderCellAlignment = StringAlignment.Near;
            printer.Footer = businessName;
            printer.FooterSpacing = 15;
            printer.PrintDataGridView(employeeTable);
        }

        private async Task<string> GetBusinessName() =>
            (await _unitOfWork.AppSettingsRepo.GetByID(1)).Name;

        private void btnLogout_Click(object sender, EventArgs e)
        {
            ConfirmLogout();
        }

        private async void btnSearch_Click(object sender, EventArgs e)
        {
            await RefreshData(txtSearch.Text);
        }
    }
}