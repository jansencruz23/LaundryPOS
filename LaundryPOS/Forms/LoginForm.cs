using LaundryPOS.Contracts;
using LaundryPOS.Models;
using LaundryPOS.Helpers;

namespace LaundryPOS.Forms
{
    public partial class LoginForm : Form
    {
        private const int APP_DATA_INDEX = 1;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ThemeManager _themeManager;            
        
        public LoginForm(IUnitOfWork unitOfWork,
            ThemeManager themeManager)
        {
            _unitOfWork = unitOfWork;
            _themeManager = themeManager;
            InitializeComponent();
            Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 30, 30));
        }

        private async void LoginForm_Load(object sender, EventArgs e)
        {
            await ApplyTheme();
            await DisplayAppInfo();
        }

        private async Task DisplayAppInfo()
        {
            var appData = await _unitOfWork.AppSettingsRepo.GetByID(APP_DATA_INDEX);
            lblName.Text = appData.Name;
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
                        ? new AdminForm(_unitOfWork, _themeManager)
                        : new MainForm(_unitOfWork, _themeManager, employee);
                    form.FormClosed += (s, args) => Close();
                    form.Show();
                }
                else
                {
                    MessageBox.Show("Invalid username or password.");
                }

                txtPassword.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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
    }
}