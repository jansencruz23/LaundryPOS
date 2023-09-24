using LaundryPOS.Contracts;
using LaundryPOS.Models;
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
    public partial class CategoryView : UserControl
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ThemeManager _themeManager;

        public CategoryView(IUnitOfWork unitOfWork,
            ThemeManager themeManager)
        {
            _unitOfWork = unitOfWork;
            _themeManager = themeManager;
            InitializeComponent();
            InitializeCategory();
        }

        public async void InitializeCategory()
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
            var imageColumn = new DataGridViewImageColumn
            {
                HeaderText = "Image",
                Name = "Image",
                ImageLayout = DataGridViewImageCellLayout.Zoom
            };
            categoryTable.Columns.Add(imageColumn);
            categoryTable.Columns["Image"].DisplayIndex = 0;
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
    }
}