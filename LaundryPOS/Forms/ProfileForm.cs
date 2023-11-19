using Guna.UI2.WinForms;
using LaundryPOS.Contracts;
using LaundryPOS.Models;

namespace LaundryPOS.Forms
{
    public partial class ProfileForm : Form
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStyleManager _styleManager;
        private readonly Employee _employee;

        public ProfileForm(IUnitOfWork unitOfWork,
            IStyleManager styleManager, Employee employee)
        {
            _unitOfWork = unitOfWork;
            _styleManager = styleManager;
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
            await _styleManager.Theme.ApplyThemeToButton(btnEdit);
            await _styleManager.Theme.ApplyOutlineThemeToButton(btnClose, 1);
            await _styleManager.Theme.ApplyThemeToPanel(bgPanel, 1f);
            await _styleManager.Theme.ApplyFocusedThemeToTextBox(txtName);
            await _styleManager.Theme.ApplyFocusedThemeToTextBox(txtUsername);
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            Opacity = 0;
            var form = new UpdateProfileForm(_unitOfWork, _styleManager, _employee);
            form.FormClosed += (s, args) => Close();
            form.ShowDialog();
            Opacity = 100;
        }

        private void btnLogOut_Click(object sender, EventArgs e)
        {
            var confirmResult = MessageDialog.Show(this, "Are you sure you want to log out?", "Confirm Logout",
                MessageDialogButtons.YesNo, MessageDialogIcon.Question, MessageDialogStyle.Light);

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