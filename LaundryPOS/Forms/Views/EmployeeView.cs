using LaundryPOS.Contracts;
using LaundryPOS.Delegates;
using LaundryPOS.Models;
using LaundryPOS.Services;
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

namespace LaundryPOS.Forms.Views
{
    public partial class EmployeeView : UserControl
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ThemeManager _themeManager;
        private readonly ChangeAdminViewDelegate _changeAdminView;

        public EmployeeView(IUnitOfWork unitOfWork,
            ThemeManager themeManager,
            ChangeAdminViewDelegate changeAdminView)
        {
            _unitOfWork = unitOfWork;
            _themeManager = themeManager;
            _changeAdminView = changeAdminView;

            InitializeComponent();
            InitializeAsync();
        }

        private async void EmployeeView_Load(object sender, EventArgs e)
        {
            await ApplyTheme();
        }

        private async void btnRegister_Click(object sender, EventArgs e)
        {
            new RegisterForm(_unitOfWork).ShowDialog();
            await RefreshData();
        }

        private async void InitializeAsync()
        {
            await DisplayServices();
        }

        private async Task DisplayServices()
        {
            var employeeList = await _unitOfWork.EmployeeRepo.Get();
            itemTable.DataSource = employeeList;

            HideUnwantedColumns();
            ConfigureImageColumn();
            HandleImageColumnFormatting();
        }

        private void HideUnwantedColumns()
        {
            itemTable.Columns[nameof(Employee.EmployeeId)].Visible = false;
            itemTable.Columns[nameof(Employee.PicPath)].Visible = false;
            itemTable.Columns[nameof(Employee.HashedPassword)].Visible = false;
            itemTable.Columns[nameof(Employee.Salt)].Visible = false;
            itemTable.Columns[nameof(Employee.IsActive)].Visible = false;
        }

        private void ConfigureImageColumn()
        {
            var imageColumn = new DataGridViewImageColumn
            {
                HeaderText = "Image",
                Name = "Image",
                ImageLayout = DataGridViewImageCellLayout.Zoom
            };
            itemTable.Columns.Add(imageColumn);
            itemTable.Columns["Image"].DisplayIndex = 0;
        }

        private void HandleImageColumnFormatting()
        {
            itemTable.CellFormatting += (sender, e) =>
            {
                if (e.ColumnIndex == itemTable.Columns["Image"].Index && e.RowIndex >= 0)
                {
                    var rowData = itemTable.Rows[e.RowIndex].DataBoundItem as Item;
                    var imagePath = rowData?.PicPath;
                    e.Value = !string.IsNullOrEmpty(imagePath)
                        ? Image.FromFile(imagePath)
                        : null;
                }
            };
        }

        private async Task RefreshData()
        {
            itemTable.DataSource = null;
            await DisplayServices();
        }

        private async Task ApplyTheme()
        {
            await _themeManager.ApplyThemeToButton(btnEmployee);
            await _themeManager.ApplyLighterThemeToDataGridView(itemTable);
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
            ChangeAdminView((_unitOfWork, _themeManager, _changeAdminView)
            => new CategoryView(_unitOfWork, _themeManager, _changeAdminView));
        }

        private void btnItem_Click(object sender, EventArgs e)
        {
            ChangeAdminView((_unitOfWork, _themeManager, _changeAdminView)
            => new ItemView(_unitOfWork, _themeManager, _changeAdminView));
        }

        private void btnTransaction_Click(object sender, EventArgs e)
        {
            ChangeAdminView((_unitOfWork, _themeManager, _changeAdminView)
            => new TransactionView(_unitOfWork, _themeManager, _changeAdminView));
        }

        private void btnAdminProfile_Click(object sender, EventArgs e)
        {
            ChangeAdminView((_unitOfWork, _themeManager, _changeAdminView)
            => new AdminProfileView(_unitOfWork, _themeManager, _changeAdminView));
        }
    }
}