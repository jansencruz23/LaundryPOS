using Guna.UI2.WinForms;
using LaundryPOS.Contracts;
using LaundryPOS.Delegates;
using LaundryPOS.Helpers;

namespace LaundryPOS.Forms.Views
{
    public partial class BaseViewControl : UserControl
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IStyleManager _styleManager;
        protected readonly ChangeAdminViewDelegate _changeAdminView;

        public BaseViewControl(IUnitOfWork unitOfWork,
            IStyleManager styleManager,
            ChangeAdminViewDelegate changeAdminView)
        {
            _unitOfWork = unitOfWork;
            _styleManager = styleManager;
            _changeAdminView = changeAdminView;
        }

        public BaseViewControl()
        {
        }

        protected void ChangeAdminView<T>(Func<IUnitOfWork, IStyleManager, ChangeAdminViewDelegate, T> createViewFunc)
            where T : UserControl
        {
            Dispose();
            var view = createViewFunc(_unitOfWork, _styleManager, _changeAdminView);
            _changeAdminView?.Invoke(view);
        }

        protected Func<IUnitOfWork, IStyleManager, ChangeAdminViewDelegate, T> CreateView<T>()
            where T : UserControl
                => (_unitOfWork, _themeManager, _changeAdminView)
                    => Activator.CreateInstance(
                        typeof(T), _unitOfWork, _themeManager, _changeAdminView) as T;

        protected async Task<string> GetBusinessName()
        {
            var appSettings = await _unitOfWork.AppSettingsRepo.GetFirstAppSettings();
            return appSettings.Name;
        }

        protected void ConfirmLogout()
        {
            DialogResult confirm = MessageBox.Show("Are you sure you want to log out?",
                "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm == DialogResult.Yes)
            {
                RestartApplication();
            }
        }

        protected void RestartApplication()
        {
            string appPath = Application.ExecutablePath;
            System.Diagnostics.Process.Start(appPath);
            Application.Exit();
        }

        protected bool ValidateField(bool condition, Label label)
        {
            label.Visible = condition;
            return !condition;
        }

        protected bool IsValidDecimalInput(string input, out decimal value)
        {
            return decimal.TryParse(input, out value) && value > 0 && input.Length <= 6;
        }

        protected void HideValidationError(params Label[] labels)
        {
            Array.ForEach(labels, lbl 
                => lbl.Visible = false);
        }

        protected async Task PrintTable(Guna2DataGridView table, string subtitle)
        {
            var printer = new DGVPrinter();
            var businessName = await GetBusinessName();

            printer.Title = businessName;
            printer.SubTitle = string.Format(subtitle, printer.SubTitleColor = Color.Black, printer);
            printer.SubTitleSpacing = 15;
            printer.SubTitleFormatFlags = StringFormatFlags.LineLimit | StringFormatFlags.NoClip;
            printer.PageNumbers = true;
            printer.PageNumberInHeader = false;
            printer.PorportionalColumns = true;
            printer.RowHeight = DGVPrinter.RowHeightSetting.CellHeight;
            printer.HeaderCellAlignment = StringAlignment.Near;
            printer.Footer = businessName;
            printer.FooterSpacing = 15;
            printer.PrintDataGridView(table);
        }
    }
}