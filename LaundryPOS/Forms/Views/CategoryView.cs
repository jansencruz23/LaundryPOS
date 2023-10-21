using LaundryPOS.Contracts;
using LaundryPOS.Delegates;
using LaundryPOS.Models;
using LaundryPOS.Helpers;
using Microsoft.IdentityModel.Tokens;
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
    public partial class CategoryView : BaseCategoryView
    {
        private Category _category;
        private const string CATEGORY_FOLDER = "Categories";

        public CategoryView(IUnitOfWork unitOfWork,
            ThemeManager themeManager,
            ChangeAdminViewDelegate changeAdminView)
            : base (unitOfWork, themeManager, changeAdminView)
        {
            InitializeComponent();
        }

        private async void CategoryView_Load(object sender, EventArgs e)
        {
            await DisplayCategory();
            await ApplyTheme();
        }

        public async Task DisplayCategory()
        {
            var categories = await _unitOfWork.CategoryRepo
                .Get();

            categoryTable.DataSource = categories;
            ConfigureImageColumn();
            HandleImageColumnFormatting();
            HideUnwantedColumns();
        }

        private void ConfigureImageColumn()
        {
            if (categoryTable.Columns["Image"] == null)
            {
                var imageColumn = new DataGridViewImageColumn
                {
                    HeaderText = "Image",
                    Name = "Image",
                    ImageLayout = DataGridViewImageCellLayout.Zoom
                };
                categoryTable.Columns.Add(imageColumn);
                categoryTable.Columns["Image"].DisplayIndex = 0;
            }
        }

        private void HandleImageColumnFormatting()
        {
            categoryTable.CellFormatting += (sender, e) =>
            {
                if (e.ColumnIndex == categoryTable.Columns["Image"].Index && e.RowIndex >= 0)
                {
                    var rowData = categoryTable.Rows[e.RowIndex].DataBoundItem as Category;
                    var imagePath = rowData?.Image;
                    e.Value = !string.IsNullOrEmpty(imagePath)
                        ? Image.FromFile(imagePath)
                        : null;
                }
            };
        }

        private void HideUnwantedColumns()
        {
            categoryTable.Columns[nameof(Category.Id)].Visible = false;
            categoryTable.Columns[nameof(Category.Image)].Visible = false;
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text) ||
                string.IsNullOrWhiteSpace(txtPath.Text))
            {
                MessageBox.Show("Invalid item. Please fill up all of the fields including the image.");
                return;
            }

            var category = new Category
            {
                Name = txtName.Text,
                Image = GetImagePath(
                    SaveToImages(txtPath.Text, CATEGORY_FOLDER), 
                    CATEGORY_FOLDER)
            };

            _unitOfWork.CategoryRepo.Insert(category);
            await _unitOfWork.SaveAsync();

            btnAdd.Enabled = true;
            ClearText();
        }

        private void btnFile_Click(object sender, EventArgs e)
        {
            using var file = new OpenFileDialog();
            file.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp;*.ico|All Files|*.*";
            file.FilterIndex = 1;
            file.RestoreDirectory = true;

            if (file.ShowDialog() == DialogResult.OK)
            {
                txtPath.Text = file.FileName;
                imgIcon.Image = Image.FromFile(file.FileName);
            }
        }

        private async Task ApplyTheme()
        {
            await _themeManager.ApplyThemeToButton(btnFile);
            await _themeManager.ApplyThemeToButton(btnCategory);
            await _themeManager.ApplyLighterThemeToDataGridView(categoryTable);
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            if (_category != null)
            {
                DialogResult result = MessageBox.Show("Are you sure you want to delete this employee?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    await _unitOfWork.CategoryRepo.Delete(_category.Id);
                    await _unitOfWork.SaveAsync();

                    MessageBox.Show("Category deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearText();
                }
            }
        }

        private async void categoryTable_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var selectedRow = categoryTable.Rows[e.RowIndex];
                var categoryId = (selectedRow.DataBoundItem as Category)?.Id;
                _category = await _unitOfWork.CategoryRepo
                    .GetByID(categoryId.Value);

                txtName.Text = _category.Name;
                txtPath.Text = _category.Image;
                imgIcon.Image = GetImage(_category);


                btnDelete.Enabled = true;
                btnUpdate.Enabled = true;
                btnSave.Enabled = false;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            ClearText();

            btnSave.Enabled = true;
            btnUpdate.Enabled = false;
            btnDelete.Enabled = false;
        }

        private async void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (_category != null)
                {
                    _category.Name = txtName.Text;
                    _category.Image = GetImagePath(
                        SaveToImages(txtPath.Text, CATEGORY_FOLDER),
                        CATEGORY_FOLDER);

                    _unitOfWork.CategoryRepo.Update(_category);
                    await _unitOfWork.SaveAsync();

                    MessageBox.Show("Category updated successfully");
                    await RefreshData();
                    ClearText();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occured " + ex.Message);
            }
        }

        private async Task RefreshData()
        {
            categoryTable.DataSource = null;
            await DisplayCategory();
        }

        private void ClearText()
        {
            imgIcon.Image = null;
            txtName.Text = string.Empty;
            txtPath.Text = string.Empty;
        }

        private void btnItem_Click(object sender, EventArgs e)
        {
            ChangeAdminView(CreateView<ItemView>());
        }

        private void btnEmployee_Click(object sender, EventArgs e)
        {
            ChangeAdminView(CreateView<EmployeeView>());
        }

        private void btnTransaction_Click(object sender, EventArgs e)
        {
            ChangeAdminView(CreateView<TransactionView>());
        }

        private void btnAdminProfile_Click(object sender, EventArgs e)
        {
            ChangeAdminView(CreateView<AdminProfileView>());
        }

        private async void btnPrint_Click(object sender, EventArgs e)
        {
            var printer = new DGVPrinter();
            var businessName = await GetBusinessName();

            printer.Title = businessName;
            printer.SubTitle = string.Format("Item/Service Category List", printer.SubTitleColor = Color.Black, printer);
            printer.SubTitleSpacing = 15;
            printer.SubTitleFormatFlags = StringFormatFlags.LineLimit | StringFormatFlags.NoClip;
            printer.PageNumbers = true;
            printer.PageNumberInHeader = false;
            printer.PorportionalColumns = true;
            printer.RowHeight = DGVPrinter.RowHeightSetting.CellHeight;
            printer.HeaderCellAlignment = StringAlignment.Near;
            printer.Footer = businessName;
            printer.FooterSpacing = 15;
            printer.PrintDataGridView(categoryTable);
        }

        private async Task<string> GetBusinessName() =>
            (await _unitOfWork.AppSettingsRepo.GetByID(1)).Name;

        private void btnLogout_Click(object sender, EventArgs e)
        {
            DialogResult confirm = MessageBox.Show("Are you sure you want to log out?",
                "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm == DialogResult.Yes)
            {
                RestartApplication();
            }
        }

        private void RestartApplication()
        {
            string appPath = Application.ExecutablePath;
            System.Diagnostics.Process.Start(appPath);
            Application.Exit();
        }
    }
}