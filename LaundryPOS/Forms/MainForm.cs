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

        private DateTime loadingStartTime;

        public MainForm(IUnitOfWork unitOfWork,
            IStyleManager styleManager,
            Employee employee)
        {
            _unitOfWork = unitOfWork;
            _styleManager = styleManager;
            _employee = employee;

            InitializeComponent();
            ShowLoadingForm();
            DisplayEmployeeInfo();
            ShowMainView();
        }

        private void ShowMainView()
        {
            var mainView = new MainView(_unitOfWork, _styleManager, _employee);
            panelView.Controls.Add(mainView);
            mainView.Dock = DockStyle.Fill;
        }

        private void ShowUnpaidView()
        {
            var mainView = new MainView(_unitOfWork, _styleManager, _employee);
            panelView.Controls.Add(mainView);
            mainView.Dock = DockStyle.Fill;
        }

        private void ShowLoadingForm()
        {
            if (_loadingForm != null && !_loadingForm.IsDisposed)
            {
                return;
            }

            loadingStartTime = DateTime.Now;
            _loadingForm = new LoadingForm();
            _loadingForm.Show();
            _loadingForm.Refresh();

            Application.Idle += OnLoaded;
        }

        private void OnLoaded(object sender, EventArgs e)
        {
            Application.Idle -= OnLoaded;
            _loadingForm.Close();
            Opacity = 100;

            TimeSpan elapsedTime = DateTime.Now - loadingStartTime;
            MessageBox.Show("Time elapsed: " + elapsedTime.ToString());
        }
      
        private void DisplayEmployeeInfo()
        {
            imgPic.Image = GetImage(_employee);
        }

        private Image GetImage(Employee t)
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

        private void btnPending_Click(object sender, EventArgs e)
        {
            //Hide();
            //var form = new UnpaidForm(_unitOfWork, _styleManager,
            //    _employee, _title);
            //form.FormClosed += (s, args) => Close();
            //form.Show();
        }

        private void btnProfile_Click(object sender, EventArgs e)
        {
            var form = new ProfileForm(_unitOfWork, _styleManager, _employee);
            form.ShowDialog();
        }
      
        private void btnLogout_Click(object sender, EventArgs e)
        {
            var confirmResult = MessageBox.Show("Are you sure you want to log out?", "Confirm Logout",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

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
    }
}