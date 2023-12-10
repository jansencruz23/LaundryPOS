using Guna.UI2.WinForms;
using LaundryPOS.Contracts;
using LaundryPOS.Forms.Views;
using LaundryPOS.Models;

namespace LaundryPOS.Forms
{
    public partial class MainForm : Form
    {
        private LoadingForm _loadingForm;

        private readonly IUnitOfWork _unitOfWork;
        private readonly IStyleManager _styleManager;
        private readonly Employee _employee;
        private readonly AppSettings _appSettings;

        private readonly MainView _mainView;
        private readonly PendingView _pendingView;

        public MainForm(IUnitOfWork unitOfWork,
            IStyleManager styleManager,
            Employee employee,
            AppSettings appSettings)
        {
            _unitOfWork = unitOfWork;
            _styleManager = styleManager;
            _employee = employee;
            _appSettings = appSettings;

            _mainView = new MainView(unitOfWork, styleManager, employee);
            _pendingView = new PendingView(unitOfWork, styleManager);

            InitializeComponent();
            DisplayEmployeeInfo();
            DisplayAppInfo();
            ShowMainView();
        }

        private void ShowMainView()
        {
            panelView.Controls.Clear();
            panelView.Controls.Add(_mainView);
            _mainView.ShowLoadingForm();
            _mainView.Dock = DockStyle.Fill;
        }

        private void ShowPendingView()
        {
            panelView.Controls.Clear();
            var pendingView = new PendingView(_unitOfWork, _styleManager);
            panelView.Controls.Add(pendingView);
            pendingView.Dock = DockStyle.Fill;
        }

        private void OnLoaded(object sender, EventArgs e)
        {
            Application.Idle -= OnLoaded;
            _loadingForm.Close();
            Opacity = 100;
        }
      
        private void DisplayEmployeeInfo()
        {
            imgPic.Image = GetImage(_employee);
            Text = _appSettings.Name;
        }

        private void DisplayAppInfo()
        {
            imgLogo.Image = GetImage(_appSettings);
        }

        private Image GetImage(ImageEntity t)
        {
            try
            {
                return t.Image != null
                ? Image.FromFile(t.Image)
                : Image.FromFile("./default.png");
            }
            catch
            {
                return Image.FromFile("./default.png");
            }
        }

        private void btnProfile_Click(object sender, EventArgs e)
        {
            var form = new ProfileForm(_unitOfWork, _styleManager, _employee);
            form.ShowDialog();
            DisplayEmployeeInfo();
        }
      
        private void btnLogout_Click(object sender, EventArgs e)
        {
            var confirmResult = MessageDialog.Show(this, "Are you sure you want to log out?", 
                "Confirm Logout", MessageDialogButtons.YesNo, MessageDialogIcon.Question,
                MessageDialogStyle.Light);

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

        private void btnMain_Click(object sender, EventArgs e)
        {
            UpdateViewButton(btnMain, "./Icons/homepage (1).png", 
                Color.FromArgb(248, 248, 248), 
                Color.FromArgb(48,48,48));
            UpdateViewButton(btnPending, "./Icons/time.png", 
                Color.White, 
                Color.FromArgb(204, 204, 204));
            ShowMainView();
        }

        private void btnPending_Click(object sender, EventArgs e)
        {
            UpdateViewButton(btnPending, "./Icons/timeblack.png", 
                Color.FromArgb(248, 248, 248), 
                Color.FromArgb(48,48,48));
            UpdateViewButton(btnMain, "./Icons/homepagewhte.png", 
                Color.White, 
                Color.FromArgb(204, 204, 204));
            ShowPendingView();
        }

        private void UpdateViewButton(Guna2Button button, string imagePath, 
            Color fillColor, Color foreColor)
        {
            button.Image = Image.FromFile(imagePath);
            button.FillColor = fillColor;
            button.ForeColor = foreColor;
        }
    }
}