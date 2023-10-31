using LaundryPOS.Contracts;
using LaundryPOS.Delegates;
using LaundryPOS.Forms.Views;
using LaundryPOS.Helpers;

namespace LaundryPOS.Forms
{
    public partial class AdminForm : Form
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ThemeManager _themeManager;
        private ChangeAdminViewDelegate changeAdminViewDelegate;
        private readonly string _title;

        public AdminForm(IUnitOfWork unitOfWorK,
            ThemeManager themeManager, string title)
        {
            _unitOfWork = unitOfWorK;
            _themeManager = themeManager;
            _title = title;
            changeAdminViewDelegate = new ChangeAdminViewDelegate(ChangePanelContent);

            InitializeComponent();
            lblTitle.Text = _title;

            var itemView = new ItemView(_unitOfWork, themeManager, changeAdminViewDelegate);
            viewPanel.Controls.Add(itemView);
            itemView.Dock = DockStyle.Fill;
        }

        private void ChangePanelContent(UserControl newContent)
        {
            viewPanel.Controls.Clear();
            viewPanel.Controls.Add(newContent);
            newContent.Dock = DockStyle.Fill;
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            var settingsView = new AppSettingsView(_unitOfWork, _themeManager, changeAdminViewDelegate);
            ChangePanelContent(settingsView);
        }
    }
}