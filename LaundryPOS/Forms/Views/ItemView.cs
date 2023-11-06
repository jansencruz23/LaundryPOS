using LaundryPOS.Contracts;
using LaundryPOS.Delegates;
using LaundryPOS.Models;
using LaundryPOS.Models.ViewModels;
using LaundryPOS.Helpers;
using System.Data;
using static LaundryPOS.Helpers.DGVPrinter;
using LaundryPOS.Forms.Views.BaseViews;
using System.Text.RegularExpressions;

namespace LaundryPOS.Forms.Views
{
    public partial class ItemView : BaseItemView
    {
        private LoadingForm _loadingForm;
        private Item _item;
        private IEnumerable<Category> _categories;
        private const string ITEMS_FOLDER = "Items";

        public ItemView(IUnitOfWork unitOfWork,
            ThemeManager themeManager,
            ChangeAdminViewDelegate changeAdminView)
            : base(unitOfWork, themeManager, changeAdminView)
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


        private async void ItemView_Load(object sender, EventArgs e)
        {
            await DisplayItems();
            await InitializeCategory();
            await ApplyTheme();
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs())
            {
                MessageBox.Show("Invalid item. Please make sure fields are valid.", 
                    "Item Registration Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var item = CreateItemFromInputs();
            await CreateItem(item);
            await RefreshData();
            ClearText();
            DisableFields();
            btnSave.Enabled = false;
        }

        private bool ValidateInputs()
        {
            bool isValid = true;

            isValid &= ValidateField(!IsValidName(txtName), lblNameValidation);
            isValid &= ValidateField(cbCategory.SelectedItem == null, lblCategoryValidation);
            isValid &= ValidateField(!IsValidDecimalInput(txtPrice.Text, out _), lblPriceValidation);
            isValid &= ValidateField(!IsValidDecimalInput(txtStock.Text, out _), lblStockValidation);
            isValid &= ValidateField(string.IsNullOrWhiteSpace(txtPath.Text), lblIconValidation);

            return isValid;
        }

        private Item CreateItemFromInputs()
        {
            var selectedCategory = (dynamic)cbCategory.SelectedItem;
            var price = decimal.Parse(txtPrice.Text);
            var stock = int.Parse(txtStock.Text);
            var imagePath = SaveToImages(txtPath.Text, ITEMS_FOLDER);

            return new Item
            {
                Name = txtName.Text,
                Image = GetImagePath(imagePath, ITEMS_FOLDER),
                CategoryId = selectedCategory.Value,
                Price = price,
                Stock = stock
            };
        }

        public async Task CreateItem(Item service)
        {
            _unitOfWork.ItemRepo.Insert(service);
            await _unitOfWork.SaveAsync();
        }

        private async Task InitializeCategory()
        {
            _categories = await _unitOfWork.CategoryRepo.Get();
            cbCategory.DataSource = _categories.Select(c => new
            {
                Text = c.Name,
                Value = c.Id
            })
            .ToList();

            cbCategory.DisplayMember = "Text";
            cbCategory.ValueMember = "Value";
        }

        private async Task DisplayItems(string query = "")
        {
            var itemList = await _unitOfWork.ItemRepo
                .Get(orderBy: s => s.OrderByDescending(s => s.Id),
                includeProperties: "Category",
                filter: i => i.Name.Contains(query));

            itemTable.DataSource = itemList;

            if (itemTable.Columns.Contains("Category"))
            {
                var categoryNameColumn = itemTable.Columns["Category"];
                categoryNameColumn.DisplayIndex = 3;
            }

            HideUnwantedColumns();
            ConfigureImageColumn(itemTable);
            HandleImageColumnFormatting(itemTable);
        }

        private void HideUnwantedColumns()
        {
            itemTable.Columns[nameof(Item.Id)].Visible = false;
            itemTable.Columns[nameof(Item.Image)].Visible = false;
            itemTable.Columns[nameof(Item.CategoryId)].Visible = false;
        }

        private async Task RefreshData(string query = "")
        {
            itemTable.DataSource = null;
            await DisplayItems(query);
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
            txtName.Clear();
            txtPrice.Clear();
            txtPath.Clear();
            txtStock.Clear();
            imgIcon.Image = null;
        }

        private async Task ApplyTheme()
        {
            await _themeManager.ApplyThemeToButton(btnFile);
            await _themeManager.ApplyThemeToButton(btnItem);
            await _themeManager.ApplyThemeToButton(btnSearch);
            await _themeManager.ApplyLighterThemeToDataGridView(itemTable, 1f, true);
            await _themeManager.ApplyFocusedThemeToTextBox(txtName);
            await _themeManager.ApplyFocusedThemeToTextBox(txtPrice);
            await _themeManager.ApplyFocusedThemeToTextBox(txtSearch);
            await _themeManager.ApplyFocusedThemeToTextBox(txtStock);
        }

        private void itemTable_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            var columnName = itemTable.Columns[e.ColumnIndex].Name;

            if (columnName == "Category" &&
                itemTable.Rows[e.RowIndex].DataBoundItem is Item rowData)
            {
                e.Value = rowData.Category != null
                    ? rowData.Category.Name
                    : string.Empty;
                e.FormattingApplied = true;
            }
        }

        private void btnEmployee_Click(object sender, EventArgs e)
        {
            ChangeAdminView(CreateView<EmployeeView>());
        }

        private void btnTransaction_Click(object sender, EventArgs e)
        {
            ChangeAdminView(CreateView<TransactionView>());
        }

        private void btnCategory_Click(object sender, EventArgs e)
        {
            ChangeAdminView(CreateView<CategoryView>());
        }

        private void btnAdminProfile_Click(object sender, EventArgs e)
        {
            ChangeAdminView(CreateView<AdminProfileView>());
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
                var itemId = (selectedRow.DataBoundItem as Item)?.Id;
                _item = await _unitOfWork.ItemRepo
                    .GetByID(itemId.Value);

                txtName.Text = _item.Name;
                txtPrice.Text = _item.Price.ToString();
                txtStock.Text = _item.Stock.ToString();
                txtPath.Text = _item.Image?.ToString();
                cbCategory.SelectedValue = _item.CategoryId != null 
                    ? _item.CategoryId
                    : -1;
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
                    MessageBox.Show("Invalid item. Please fill up all of the fields including the image."
                        , "Item Update Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (_item != null)
                {
                    await UpdateItem();
                    MessageBox.Show("Item updated successfully", "Item Update Successful",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    await RefreshData();
                    ClearText();

                    btnUpdate.Enabled = false;
                    btnDelete.Enabled = false;
                }
        }
            catch (Exception ex)
            {
                MessageBox.Show("An error occured " + ex.Message, "Item Update Failed",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
}

        private async Task UpdateItem()
        {
            var category = _categories.FirstOrDefault(c => c.Id == (int) cbCategory.SelectedValue);
            _item.Name = txtName.Text;
            _item.Category = category;
            _item.Price = decimal.Parse(txtPrice.Text);
            _item.Stock = int.Parse(txtStock.Text);
            _item.Image = GetImagePath(
                SaveToImages(txtPath.Text, ITEMS_FOLDER), 
                ITEMS_FOLDER);

            _unitOfWork.ItemRepo.Update(_item);
            await _unitOfWork.SaveAsync();

            DisableFields();
        }

        private void DisableFields()
        {
            txtName.Enabled = false;
            txtPrice.Enabled = false;
            txtStock.Enabled = false;
            cbCategory.Enabled = false;
        }

        private async void btnPrint_Click(object sender, EventArgs e)
        {
            await PrintTable(itemTable, "Item/Service List");
        }

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
                    MessageBox.Show($"Error occured: {ex.Message}", "Item Delete Failed",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            ConfirmLogout();
        }

        private async void btnSearch_Click(object sender, EventArgs e)
        {
            await RefreshData(txtSearch.Text);
        }

        private void StyleFonts()
        {
            StyleFontsButton(11.25f, btnItem, btnCategory, btnEmployee, btnEmployee,
                btnAdminProfile, btnAdd, btnSave, btnUpdate, btnDelete,
                btnPrint, btnLogout);
            StyleFontsLabel(18f, true, lblDetails, lblList);
            StyleFontsLabel(11.25f, false, lblName, lblCategory, lblPrice, lblStock);
            StyleFontsButton(11.25f, btnFile, btnSearch);
            StyleFontsTextBox(11.25f, txtName, txtPrice, txtSearch, txtStock);
            StyleFontsLabel(8.25f, false, lblNameValidation, lblPriceValidation,
                lblCategoryValidation, lblIconValidation, lblStockValidation);
            cbCategory.Font = _themeManager.Helvetica(11.25f);
        }

        private void textBoxes_Click(object sender, EventArgs e)
        {
            lblNameValidation.Visible =
            lblIconValidation.Visible =
            lblCategoryValidation.Visible =
            lblPriceValidation.Visible =
            lblStockValidation.Visible = false;
        }
    }
}