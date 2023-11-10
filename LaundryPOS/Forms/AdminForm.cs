using LaundryPOS.Contracts;
using LaundryPOS.Delegates;
using LaundryPOS.Forms.Views;
using LaundryPOS.Managers;

namespace LaundryPOS.Forms
{
    public partial class AdminForm : Form
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStyleManager _styleManager;
        private ChangeAdminViewDelegate changeAdminViewDelegate;
        private readonly string _title;

        public AdminForm(IUnitOfWork unitOfWorK,
            IStyleManager styleManager, string title)
        {
            _unitOfWork = unitOfWorK;
            _styleManager = styleManager;
            _title = title;
            changeAdminViewDelegate = new ChangeAdminViewDelegate(ChangePanelContent);

            InitializeComponent();
            lblTitle.Text = _title;

            ShowItemView();
        }

        private void ShowItemView()
        {
            var itemView = new ItemView(_unitOfWork, _styleManager, changeAdminViewDelegate);
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
            var settingsView = new AppSettingsView(_unitOfWork, _styleManager, changeAdminViewDelegate);
            ChangePanelContent(settingsView);
        }
    }
}