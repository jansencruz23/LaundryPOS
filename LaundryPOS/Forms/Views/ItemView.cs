using LaundryPOS.Contracts;
using LaundryPOS.Models;
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
    public partial class ItemView : UserControl
    {
        private readonly IUnitOfWork _unitOfWork;

        public ItemView(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            InitializeComponent();
            InitializeAsync();
            ConfigureImageColumn();
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            var service = new Item();
            service.Name = txtName.Text;
            service.PicPath = txtPath.Text;

            if (!double.TryParse(txtPrice.Text, out double price))
            {
                MessageBox.Show("Invalid Price");
            }
            else
            {
                service.Price = price;
                await CreateService(service);
                await RefreshData();
                ClearText();
            }
        }

        public async Task CreateService(Item service)
        {
            _unitOfWork.ServiceRepo.Insert(service);
            await _unitOfWork.SaveAsync();
        }

        private async void InitializeAsync()
        {
            await DisplayServices();
        }

        private async Task DisplayServices()
        {
            var serviceList = await _unitOfWork.ServiceRepo
                .Get(orderBy: s => s.OrderByDescending(s => s.ItemId));
            dgvService.DataSource = serviceList;

            HideUnwantedColumns();
            HandleImageColumnFormatting();
        }

        private void HideUnwantedColumns()
        {
            dgvService.Columns[nameof(Item.ItemId)].Visible = false;
            dgvService.Columns[nameof(Item.PicPath)].Visible = false;
        }

        private void ConfigureImageColumn()
        {
            var imageColumn = new DataGridViewImageColumn
            {
                HeaderText = "Image",
                Name = "Image",
                ImageLayout = DataGridViewImageCellLayout.Zoom
            };

            dgvService.Columns.Add(imageColumn);
            dgvService.Columns["Image"].DisplayIndex = 0;
        }

        private void HandleImageColumnFormatting()
        {
            dgvService.CellFormatting += (sender, e) =>
            {
                if (e.ColumnIndex == dgvService.Columns["Image"].Index && e.RowIndex >= 0)
                {
                    var rowData = dgvService.Rows[e.RowIndex].DataBoundItem as Item;
                    var imagePath = rowData?.PicPath;
                    e.Value = !string.IsNullOrEmpty(imagePath) 
                        ? Image.FromFile(imagePath) 
                        : null;
                }
            };
        }

        private async Task RefreshData()
        {
            dgvService.DataSource = null;
            await DisplayServices();
        }

        private void btnEmployee_Click(object sender, EventArgs e)
        {
            ((AdminForm)ParentForm!).DisplayEmployeeView();
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

        private void ClearText()
        {
            txtName.Text = string.Empty;
            txtPrice.Text = string.Empty;
            txtPath.Text = string.Empty;
        }
    }
}