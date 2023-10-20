using LaundryPOS.Contracts;
using LaundryPOS.Delegates;
using LaundryPOS.Models;
using LaundryPOS.Models.ViewModels;
using LaundryPOS.Helpers;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static LaundryPOS.Helpers.DGVPrinter;

namespace LaundryPOS.Forms.Views
{
    public partial class ItemView : UserControl
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ThemeManager _themeManager;
        private readonly ChangeAdminViewDelegate _changeAdminView;
        private Item _item;
        private FileInfo _imageFile;

        public ItemView(IUnitOfWork unitOfWork,
            ThemeManager themeManager,
            ChangeAdminViewDelegate changeAdminView)
        {
            _unitOfWork = unitOfWork;
            _themeManager = themeManager;
            _changeAdminView = changeAdminView;

            InitializeComponent();
        }

        private async void ItemView_Load(object sender, EventArgs e)
        {
            await DisplayItems();
            await InitializeCategory();
            await ApplyTheme();
            ConfigureImageColumn();
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs())
            {
                MessageBox.Show("Invalid item. Please fill up all of the fields including the image.");
                return;
            }

            var item = CreateItemFromInputs();
            SaveToImages(txtPath.Text);
            await CreateItem(item);
            await RefreshData();
            ClearText();
        }

        private bool ValidateInputs()
        {
            if (cbCategory.SelectedItem == null ||
                !decimal.TryParse(txtPrice.Text, out _) ||
                !int.TryParse(txtStock.Text, out _) ||
                string.IsNullOrWhiteSpace(txtName.Text) ||
                string.IsNullOrWhiteSpace(txtPath.Text))
            {
                return false;
            }

            return true;
        }

        private Item CreateItemFromInputs()
        {
            var selectedCategory = (dynamic)cbCategory.SelectedItem;
            var price = decimal.Parse(txtPrice.Text);
            var stock = int.Parse(txtStock.Text);
            var imagePath = SaveToImages(txtPath.Text);

            return new Item
            {
                Name = txtName.Text,
                Image = GetImagePath(imagePath),
                CategoryId = selectedCategory.Value,
                Price = price,
                Stock = stock
            };
        }

        private string GetImagePath(string imagePath)
            => Path.Combine("Icons", "Images", "Items", Path.GetFileName(imagePath));

        private string SaveToImages(string imagePath)
        {
            string uniqueName = $"{Guid.NewGuid().ToString()[^8..]}_" +
                $"{Path.GetFileName(imagePath)[Math.Max(0, Path.GetFileName(imagePath).Length - 10)..]}";
            string destinationPath = Path.Combine(Application.StartupPath, "Icons", "Images", "Items", uniqueName);
            Directory.CreateDirectory(Path.GetDirectoryName(destinationPath));
            File.Copy(imagePath, destinationPath);

            return uniqueName;
        }

        public async Task CreateItem(Item service)
        {
            _unitOfWork.ItemRepo.Insert(service);
            await _unitOfWork.SaveAsync();
        }

        private async Task InitializeCategory()
        {
            var categories = await _unitOfWork.CategoryRepo.Get();
            cbCategory.DataSource = categories.Select(c => new
            {
                Text = c.Name,
                Value = c.CategoryId
            })
            .ToList();

            cbCategory.DisplayMember = "Text";
            cbCategory.ValueMember = "Value";
        }

        private async Task DisplayItems()
        {
            var itemList = await _unitOfWork.ItemRepo
                .Get(orderBy: s => s.OrderByDescending(s => s.ItemId),
                includeProperties: "Category");

            itemTable.DataSource = itemList;

            if (itemTable.Columns.Contains("Category"))
            {
                var categoryNameColumn = itemTable.Columns["Category"];
                categoryNameColumn.DisplayIndex = 3;
            }

            HideUnwantedColumns();
            HandleImageColumnFormatting();
        }

        private void HideUnwantedColumns()
        {
            itemTable.Columns[nameof(Item.ItemId)].Visible = false;
            itemTable.Columns[nameof(Item.Image)].Visible = false;
            itemTable.Columns[nameof(Item.CategoryId)].Visible = false;
        }

        private void ConfigureImageColumn()
        {
            if (itemTable.Columns["Image"] == null)
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
        }

        private void HandleImageColumnFormatting()
        {
            try
            {
                itemTable.CellFormatting += (sender, e) =>
                {
                    if (e.ColumnIndex == itemTable.Columns["Image"].Index && e.RowIndex >= 0)
                    {
                        var rowData = itemTable.Rows[e.RowIndex].DataBoundItem as Item;
                        e.Value = GetImage(rowData);
                    }
                };
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occured " + ex.Message);
            }
        }

        private Image GetImage(Item item)
        {
            try
            {
                return item.Image != null
                ? Image.FromFile(item.Image)
                : Image.FromFile("./default.png");
            }
            catch
            {
                return Image.FromFile("./default.png");
            }
        }

        private async Task RefreshData()
        {
            itemTable.DataSource = null;
            await DisplayItems();
        }

        private void btnFile_Click(object sender, EventArgs e)
        {
            using var file = new OpenFileDialog();
            file.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp;*.ico|All Files|*.*";
            file.FilterIndex = 1;
            file.RestoreDirectory = true;

            if (file.ShowDialog() == DialogResult.OK)
            {
                _imageFile = new FileInfo(file.FileName);
                txtPath.Text = file.FileName;
                imgIcon.Image = Image.FromFile(file.FileName);
            }
        }

        private void ClearText()
        {
            txtName.Text = string.Empty;
            txtPrice.Text = string.Empty;
            txtPath.Text = string.Empty;
            txtStock.Text = string.Empty;
            imgIcon.Image = null;
        }

        private async Task ApplyTheme()
        {
            await _themeManager.ApplyThemeToButton(btnFile);
            await _themeManager.ApplyThemeToButton(btnItem);
            await _themeManager.ApplyLighterThemeToDataGridView(itemTable, 0.8f);
        }

        private void itemTable_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            var columnName = itemTable.Columns[e.ColumnIndex].Name;

            if (columnName == "Category" &&
                itemTable.Rows[e.RowIndex].DataBoundItem is Item rowData)
            {
                e.Value = rowData.Category?.Name;
                e.FormattingApplied = true;
            }
        }

        private void ChangeAdminView<T>(Func<IUnitOfWork, ThemeManager, ChangeAdminViewDelegate, T> createViewFunc)
            where T : UserControl
        {
            Dispose();
            var view = createViewFunc(_unitOfWork, _themeManager, _changeAdminView);
            _changeAdminView?.Invoke(view);
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

        private void btnCategory_Click(object sender, EventArgs e)
        {
            ChangeAdminView((_unitOfWork, _themeManager, _changeAdminView)
                => new CategoryView(_unitOfWork, _themeManager, _changeAdminView));
        }

        private void btnAdminProfile_Click(object sender, EventArgs e)
        {
            ChangeAdminView((_unitOfWork, _themeManager, _changeAdminView)
               => new AdminProfileView(_unitOfWork, _themeManager, _changeAdminView));
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            ClearText();
            EnableInputAddControls();
        }

        private void EnableInputAddControls()
        {
            txtName.Enabled = true;
            txtPrice.Enabled = true;
            txtStock.Enabled = true;
            cbCategory.Enabled = true;

            btnFile.Enabled = true;
            btnSave.Enabled = true;
            btnUpdate.Enabled = false;
            btnDelete.Enabled = false;
        }

        private async void itemTable_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var selectedRow = itemTable.Rows[e.RowIndex];
                var itemId = (selectedRow.DataBoundItem as Item)?.ItemId;
                _item = await _unitOfWork.ItemRepo
                    .GetByID(itemId.Value);

                txtName.Text = _item.Name;
                txtPrice.Text = _item.Price.ToString();
                txtStock.Text = _item.Stock.ToString();
                txtPath.Text = _item.Image?.ToString();
                imgIcon.Image = GetImage(_item);

                EnableInputEditControls();
            }
        }

        private void EnableInputEditControls()
        {
            txtName.Enabled = true;
            cbCategory.Enabled = true;
            txtPrice.Enabled = true;
            txtStock.Enabled = true;
            btnFile.Enabled = true;

            btnDelete.Enabled = true;
            btnUpdate.Enabled = true;
        }

        private async void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidateInputs())
                {
                    MessageBox.Show("Invalid item. Please fill up all of the fields including the image.");
                    return;
                }

                if (_item != null)
                {
                    await UpdateItem();
                    MessageBox.Show("Item updated successfully");
                    await RefreshData();
                    ClearText();

                    btnUpdate.Enabled = false;
                    btnDelete.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occured " + ex.Message);
            }
        }

        private async Task UpdateItem()
        {
            _item.Name = txtName.Text;
            _item.Price = decimal.Parse(txtPrice.Text);
            _item.Stock = int.Parse(txtStock.Text);
            _item.Image = GetImagePath(SaveToImages(txtPath.Text));

            _unitOfWork.ItemRepo.Update(_item);
            await _unitOfWork.SaveAsync();
        }

        private async void btnPrint_Click(object sender, EventArgs e)
        {
            var printer = new DGVPrinter();
            var businessName = await GetBusinessName();

            printer.Title = businessName;
            printer.SubTitle = string.Format("Item/Service List", printer.SubTitleColor = Color.Black, printer);
            printer.SubTitleSpacing = 15;
            printer.SubTitleFormatFlags = StringFormatFlags.LineLimit | StringFormatFlags.NoClip;
            printer.PageNumbers = true;
            printer.PageNumberInHeader = false;
            printer.PorportionalColumns = true;
            printer.RowHeight = RowHeightSetting.CellHeight;
            printer.HeaderCellAlignment = StringAlignment.Near;
            printer.Footer = businessName;
            printer.FooterSpacing = 15;
            printer.PrintDataGridView(itemTable);
        }

        private async Task<string> GetBusinessName() =>
            (await _unitOfWork.AppSettingsRepo.GetByID(1)).Name;

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            DialogResult confirm = MessageBox.Show("Are you sure you want to delete this item?",
                "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm == DialogResult.Yes)
            {
                try
                {
                    _unitOfWork.ItemRepo.Delete(_item);
                    await _unitOfWork.SaveAsync();

                    ClearText();
                    ResetButtons();
                    await RefreshData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error occured: {ex.Message}");
                }
            }
        }

        private void ResetButtons()
        {
            btnAdd.Enabled = true;
            btnSave.Enabled = false;
            btnDelete.Enabled = false;
            btnUpdate.Enabled = false;
        }

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