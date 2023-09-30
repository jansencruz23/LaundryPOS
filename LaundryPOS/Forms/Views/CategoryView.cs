using LaundryPOS.Contracts;
using LaundryPOS.Delegates;
using LaundryPOS.Models;
using LaundryPOS.Services;
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
    public partial class CategoryView : UserControl
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ThemeManager _themeManager;
        private readonly ChangeAdminViewDelegate _changeAdminView;
        private Category _category;

        public CategoryView(IUnitOfWork unitOfWork,
            ThemeManager themeManager,
            ChangeAdminViewDelegate changeAdminView)
        {
            _unitOfWork = unitOfWork;
            _themeManager = themeManager;
            _changeAdminView = changeAdminView;

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
                    var imagePath = rowData?.PicPath;
                    e.Value = !string.IsNullOrEmpty(imagePath)
                        ? Image.FromFile(imagePath)
                        : null;
                }
            };
        }

        private void HideUnwantedColumns()
        {
            categoryTable.Columns[nameof(Category.CategoryId)].Visible = false;
            categoryTable.Columns[nameof(Category.PicPath)].Visible = false;
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            var category = new Category
            {
                Name = txtName.Text,
                PicPath = txtPath.Text
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
                    await _unitOfWork.CategoryRepo.Delete(_category.CategoryId);
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
                var categoryId = (selectedRow.DataBoundItem as Category)?.CategoryId;
                _category = await _unitOfWork.CategoryRepo
                    .GetByID(categoryId.Value);

                txtName.Text = _category.Name;
                txtPath.Text = _category.PicPath;
                imgIcon.Image = Image.FromFile(
                    !string.IsNullOrWhiteSpace(_category.PicPath)
                        ? _category.PicPath
                        : "./default.png");

                btnDelete.Enabled = true;
                btnUpdate.Enabled = true;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            btnSave.Enabled = true;
            btnAdd.Enabled = false;
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
                    _category.PicPath = txtPath.Text;

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
    }
}