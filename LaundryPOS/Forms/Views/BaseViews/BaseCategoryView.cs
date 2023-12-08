using LaundryPOS.Contracts;
using LaundryPOS.Delegates;
using LaundryPOS.Models;

namespace LaundryPOS.Forms.Views
{
    public class BaseCategoryView : BaseImageViewControl<Category>
    {
        public BaseCategoryView(IUnitOfWork unitOfWork,
            IStyleManager styleManager,
            ChangeAdminViewDelegate changeAdminView)
            : base(unitOfWork, styleManager, changeAdminView)
        {

        }

        public BaseCategoryView() { }
    }
}