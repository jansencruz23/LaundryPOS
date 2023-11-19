using LaundryPOS.Contracts;
using LaundryPOS.Delegates;
using LaundryPOS.Models;
using LaundryPOS.Helpers;
using Guna.UI2.WinForms;

namespace LaundryPOS.Forms.Views
{
    public partial class CategoryView : BaseCategoryView
    {
        private LoadingForm _loadingForm;
        private Category _category;
        private const string CATEGORY_FOLDER = "Categories";

        public CategoryView(IUnitOfWork unitOfWork,
            IStyleManager styleManager,
            ChangeAdminViewDelegate changeAdminView)
            : base (unitOfWork, styleManager, changeAdminView)
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

        private async void CategoryView_Load(object sender, EventArgs e)
        {
            await DisplayCategory();
            await ApplyTheme();
        }

        public async Task DisplayCategory(string query = "")
        {
            var categories = await _unitOfWork.CategoryRepo 
                .Get(filter: c => c.Name.Contains(query));

            categoryTable.DataSource = categories;

            HideUnwantedColumns();
            ConfigureImageColumn(categoryTable);
            HandleImageColumnFormatting(categoryTable);
        }

        private void HideUnwantedColumns()
        {
            categoryTable.Columns[nameof(Category.Id)].Visible = false;
            categoryTable.Columns[nameof(Category.Image)].Visible = false;
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs())
            {
                MessageDialog.Show(ParentForm, "Invalid category. Please make sure all fields are valid.",
                    "Item Registration Failed", MessageDialogButtons.OK, MessageDialogIcon.Error,
                    MessageDialogStyle.Light);
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
            await RefreshData();
            ClearText();

            btnAdd.Enabled = true;
            txtName.Enabled = false;
            btnFile.Enabled = false;
            btnSave.Enabled = false;

            MessageDialog.Show(ParentForm, "Category added successfully!", "Category Created Successfully",
                MessageDialogButtons.OK, MessageDialogIcon.Information, MessageDialogStyle.Light);
        }

        private bool ValidateInputs()
        {
            var isValid = true;
            var isNameValid = RegexValidator.IsValidItemName(txtName.Text);

            isValid &= ValidateField(!isNameValid, lblNameValidation);
            isValid &= ValidateField(string.IsNullOrWhiteSpace(txtPath.Text), lblIconValidation);

            return isValid;
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
            await _styleManager.Theme.ApplyThemeToButton(btnFile);
            await _styleManager.Theme.ApplyThemeToButton(btnCategory);
            await _styleManager.Theme.ApplyThemeToButton(btnSearch);
            await _styleManager.Theme.ApplyLighterThemeToDataGridView(categoryTable, 1f, true);
            await _styleManager.Theme.ApplyFocusedThemeToTextBox(txtName);
            await _styleManager.Theme.ApplyFocusedThemeToTextBox(txtSearch);
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            if (_category != null)
            {
                DialogResult result = MessageDialog.Show(ParentForm, "Are you sure you want to delete this employee?", 
                    "Confirmation", MessageDialogButtons.YesNo, MessageDialogIcon.Question, MessageDialogStyle.Light);

                if (result == DialogResult.Yes)
                {
                    var items = await _unitOfWork.ItemRepo
                        .Get(filter: i => i.CategoryId == _category.Id);

                    foreach (var item in items)
                    {
                        item.CategoryId = null;
                        item.Category = null;

                        _unitOfWork.ItemRepo.MarkAsModified(item);
                    }

                    await _unitOfWork.SaveAsync();
                    await _unitOfWork.CategoryRepo.Delete(_category.Id);
                    await _unitOfWork.SaveAsync();

                    MessageDialog.Show(ParentForm, "Category deleted successfully.", "Success",
                        MessageDialogButtons.OK, MessageDialogIcon.Information, MessageDialogStyle.Light);
                    ClearText();
                    await RefreshData();

                    btnFile.Enabled = false;
                    txtName.Enabled = false;
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

                txtName.Enabled = true;
                btnFile.Enabled = true;
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
            btnFile.Enabled = true;
            txtName.Enabled = true;
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

                    MessageDialog.Show(ParentForm, "Category updated successfully", "Category Updated Successfully", 
                        MessageDialogButtons.OK, MessageDialogIcon.Information, MessageDialogStyle.Light);
                    await RefreshData();
                    ClearText();

                    btnFile.Enabled = false;
                    txtName.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageDialog.Show(ParentForm, "An error occured " + ex.Message, "Category Update Failed",
                    MessageDialogButtons.OK, MessageDialogIcon.Error, MessageDialogStyle.Light);
            }
        }

        private async Task RefreshData(string query = "")
        {
            categoryTable.DataSource = null;
            await DisplayCategory(query);
        }

        private void ClearText()
        {
            imgIcon.Image = null;
            txtName.Clear();
            txtPath.Clear();
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
            await PrintTable(categoryTable, "Item/Service Category List");
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
                btnAdminProfile, btnAdd, btnSave, btnUpdate, btnDelete,
                btnPrint, btnLogout, btnFile, btnSearch);
            _styleManager.Font.StyleFontsLabel(18f, true, lblDetails, lblList);
            _styleManager.Font.StyleFontsLabel(11.25f, false, lblName);
            _styleManager.Font.StyleFontsTextBox(11.25f, txtName, txtSearch);
            _styleManager.Font.StyleFontsLabel(8.25f, false, lblNameValidation, lblIconValidation);
        }

        private void txtName_Click(object sender, EventArgs e)
        {
            HideValidationError(lblIconValidation, lblNameValidation);
        }
    }
}