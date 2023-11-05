using LaundryPOS.Contracts;
using LaundryPOS.Models;
using LaundryPOS.Helpers;

namespace LaundryPOS.Forms
{
    public partial class LoginForm : Form
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ThemeManager _themeManager;
        private string _title;

        public LoginForm(IUnitOfWork unitOfWork,
            ThemeManager themeManager)
        {
            _unitOfWork = unitOfWork;
            _themeManager = themeManager;

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
                        ? new AdminForm(_unitOfWork, _themeManager, _title)
                        : new MainForm(_unitOfWork, _themeManager, employee);
                    form.FormClosed += (s, args) => Close();
                    form.Show();
                }
                else
                {
                    MessageBox.Show("Invalid username or password.", "Log in failed",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                txtPassword.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error occurred",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            await _themeManager.ApplyThemeToButton(btnLogin, true);
            await _themeManager.ApplyThemeToPanel(panelDrag);
            await _themeManager.ApplyFocusedThemeToTextBox(txtUsername);
            await _themeManager.ApplyFocusedThemeToTextBox(txtPassword);
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
            lblName.Font = lblLogin.Font = _themeManager.Montserrat(21.75f);
            lblDescription.Font = txtUsername.Font = txtUsername.Font
                = _themeManager.Helvetica(11.25f);
            cbShowPassword.Font = _themeManager.Helvetica(8.25f);
            btnLogin.Font = _themeManager.HelveticaBold(12);
        }
    }
}