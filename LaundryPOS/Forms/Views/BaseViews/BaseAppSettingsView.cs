using LaundryPOS.Contracts;
using LaundryPOS.Delegates;
using LaundryPOS.Models;

namespace LaundryPOS.Forms.Views.BaseViews
{
    public partial class BaseAppSettingsView : BaseImageViewControl<AppSettings>
    {
        public BaseAppSettingsView(IUnitOfWork unitOfWork,
            IStyleManager styleManager,
            ChangeAdminViewDelegate changeAdminView)
            : base(unitOfWork, styleManager, changeAdminView)
        {

        }

        public BaseAppSettingsView() { }
    }
}