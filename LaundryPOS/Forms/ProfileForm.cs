using LaundryPOS.Contracts;
using LaundryPOS.Models;
using LaundryPOS.Helpers;

namespace LaundryPOS.Forms
{
    public partial class ProfileForm : Form
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ThemeManager _themeManager;
        private readonly Employee _employee;

        public ProfileForm(IUnitOfWork unitOfWork,
            ThemeManager themeManager, Employee employee)
        {
            _unitOfWork = unitOfWork;
            _themeManager = themeManager;
            _employee = employee;

            InitializeComponent();
            Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 40, 40));
        }

        private async void ProfileForm_Load(object sender, EventArgs e)
        {
            await ApplyTheme();
            InitializeProfile();
        }

        private void InitializeProfile()
        {
            txtName.Text = _employee.FullName;
            txtUsername.Text = _employee.Username;
            imgPic.Image = Image.FromFile(!string.IsNullOrWhiteSpace(_employee.Image)
                ? _employee.Image
                : "./default.png");
        }

        private async Task ApplyTheme()
        {
            await _themeManager.ApplyThemeToButton(btnEdit);
            await _themeManager.ApplyOutlineThemeToButton(btnLogOut);
            await _themeManager.ApplyOutlineThemeToButton(btnClose, 1);
            await _themeManager.ApplyThemeToPanel(bgPanel, 1f);
            await _themeManager.ApplyFocusedThemeToTextBox(txtName);
            await _themeManager.ApplyFocusedThemeToTextBox(txtUsername);
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            Opacity = 0;
            var form = new UpdateProfileForm(_unitOfWork, _themeManager, _employee);
            form.FormClosed += (s, args) => Close();
            form.ShowDialog();
            Opacity = 100;
        }

        private void btnLogOut_Click(object sender, EventArgs e)
        {
            var confirmResult = MessageBox.Show("Are you sure you want to log out?", "Confirm Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirmResult == DialogResult.Yes)
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

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}