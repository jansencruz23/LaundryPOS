using LaundryPOS.Contracts;
using LaundryPOS.Delegates;
using LaundryPOS.Managers;
using LaundryPOS.Models;

namespace LaundryPOS.Forms.Views.BaseViews
{
    public class BaseEmployeeView : BaseImageViewControl<Employee>
    {
        public BaseEmployeeView(IUnitOfWork unitOfWork,
            IStyleManager styleManager,
            ChangeAdminViewDelegate changeAdminView)
            : base (unitOfWork, styleManager, changeAdminView)
        {

        }

        public BaseEmployeeView() {}
    }
}