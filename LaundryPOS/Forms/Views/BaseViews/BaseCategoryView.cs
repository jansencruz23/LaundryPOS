using LaundryPOS.Contracts;
using LaundryPOS.Delegates;
using LaundryPOS.Helpers;
using LaundryPOS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaundryPOS.Forms.Views
{
    public class BaseCategoryView : BaseImageViewControl<Category>
    {
        public BaseCategoryView(IUnitOfWork unitOfWork,
            ThemeManager themeManager,
            ChangeAdminViewDelegate changeAdminView)
            : base(unitOfWork, themeManager, changeAdminView)
        {

        }

        public BaseCategoryView() { }
    }
}