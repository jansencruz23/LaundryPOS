using LaundryPOS.Contracts;
using LaundryPOS.Models;
using LaundryPOS.Managers;
using Guna.UI2.WinForms;
using LaundryPOS.Services;

namespace LaundryPOS.Forms
{
    public partial class LoginForm : Form
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStyleManager _styleManager;
        private readonly ISalesService _salesService;
        private string _title;

        public LoginForm(IUnitOfWork unitOfWork,
            IStyleManager styleManager,
            ISalesService salesService)
        {
            _unitOfWork = unitOfWork;
            _styleManager = styleManager;
            _salesService = salesService;

            InitializeComponent();
            StyleFonts();
            Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
        }

        private async void LoginForm_Load(object sender, EventArgs e)
        {
            await ApplyTheme();
            await DisplayAppInfo();
        }

        private async Task DisplayAppInfo()
        {
            var appData = await _unitOfWork.AppSettingsRepo.GetFirstAppSettings();
            _title = appData?.Name;
            lblName.Text = _title;
        }

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                var username = txtUsername.Text;
                var password = txtPassword.Text;

                var employee = await _unitOfWork.EmployeeRepo.GetEmployeeByUsername(username);

                if (IsValidUser(employee, password, out string userRole))
                {
                    Hide();

                    Form form = userRole == "admin"
                        ? new AdminForm(_unitOfWork, _styleManager, _salesService, _title)
                        : new MainForm(_unitOfWork, _styleManager, employee);
                    form.FormClosed += (s, args) => Close();
                    form.Show();
                }
                else
                {
                    MessageDialog.Show(this, "Invalid username or password.", "Log in failed",
                        MessageDialogButtons.OK, MessageDialogIcon.Error, MessageDialogStyle.Light);
                }

                txtPassword.Clear();
            }
            catch (Exception ex)
            {
                MessageDialog.Show(this, ex.Message, "Error occurred",
                    MessageDialogButtons.OK, MessageDialogIcon.Error, MessageDialogStyle.Light);
            }
        }

        private bool IsValidUser(Employee employee, string password, out string userRole)
        {
            userRole = null!;

            if (employee?.ValidatePassword(password) == true)
            {
                userRole = employee.UserRole;
                return true;
            }

            return false;
        }

        private async Task ApplyTheme()
        {
            await _styleManager.Theme.ApplyThemeToButton(btnLogin, true);
            await _styleManager.Theme.ApplyThemeToPanel(panelDrag);
            await _styleManager.Theme.ApplyFocusedThemeToTextBox(txtUsername);
            await _styleManager.Theme.ApplyFocusedThemeToTextBox(txtPassword);
        }

        private void cbShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            txtPassword.PasswordChar = cbShowPassword.Checked
                ? '\0'
                : '•';
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void StyleFonts()
        {
            lblName.Font = lblLogin.Font = _styleManager.Font.Montserrat(21.75f);
            lblDescription.Font = txtUsername.Font = txtUsername.Font
                = _styleManager.Font.Helvetica(11.25f);
            cbShowPassword.Font = _styleManager.Font.Helvetica(8.25f);
            btnLogin.Font = _styleManager.Font.HelveticaBold(12);
        }
    }
}