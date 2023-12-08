using LaundryPOS.Contracts;
using LaundryPOS.Delegates;
using LaundryPOS.Forms.Views;
using LaundryPOS.Forms.Views.AdminViews;
using LaundryPOS.Models;

namespace LaundryPOS.Forms
{
    public partial class AdminForm : Form
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStyleManager _styleManager;
        private readonly ISalesService _salesService;
        private readonly ChangeAdminViewDelegate changeAdminViewDelegate;
        private readonly AppSettings _appSettings;

        public AdminForm(IUnitOfWork unitOfWorK,
            IStyleManager styleManager,
            ISalesService salesService, 
            AppSettings appSettings)
        {
            _unitOfWork = unitOfWorK;
            _styleManager = styleManager;
            _salesService = salesService;
            _appSettings = appSettings;
            changeAdminViewDelegate = new ChangeAdminViewDelegate(ChangePanelContent);

            InitializeComponent();
            ShowAppInfo();
            ShowDashboardView();
        }

        private void ShowAppInfo()
        {
            lblTitle.Text = _appSettings.Name;
            imgPic.Image = GetLogo();
        }

        private void ShowDashboardView()
        {
            var itemView = new DashboardView(_unitOfWork, _styleManager, _salesService, changeAdminViewDelegate);
            viewPanel.Controls.Add(itemView);
            itemView.Dock = DockStyle.Fill;
        }

        private void ChangePanelContent(UserControl newContent, bool isDashboard)
        {
            if (isDashboard)
            {
                newContent = new DashboardView(_unitOfWork, _styleManager, _salesService, changeAdminViewDelegate);
            }

            viewPanel.Controls.Clear();
            viewPanel.Controls.Add(newContent);
            newContent.Dock = DockStyle.Fill;
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            var settingsView = new AppSettingsView(_unitOfWork, _styleManager, changeAdminViewDelegate);
            ChangePanelContent(settingsView, false);
        }

        private Image GetLogo()
        {
            try
            {
                return _appSettings.Image != null
                ? Image.FromFile(_appSettings.Image)
                : Image.FromFile("./default.png");
            }
            catch
            {
                return Image.FromFile("./default.png");
            }
        }
    }
}