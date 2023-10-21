using LaundryPOS.Contracts;
using LaundryPOS.Delegates;
using LaundryPOS.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LaundryPOS.Forms.Views
{
    public partial class BaseViewControl : UserControl
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly ThemeManager _themeManager;
        protected readonly ChangeAdminViewDelegate _changeAdminView;

        public BaseViewControl(IUnitOfWork unitOfWork,
            ThemeManager themeManager,
            ChangeAdminViewDelegate changeAdminView)
        {
            _unitOfWork = unitOfWork;
            _themeManager = themeManager;
            _changeAdminView = changeAdminView;
        }

        public BaseViewControl()
        {
        }

        protected void ChangeAdminView<T>(Func<IUnitOfWork, ThemeManager, ChangeAdminViewDelegate, T> createViewFunc)
            where T : UserControl
        {
            Dispose();
            var view = createViewFunc(_unitOfWork, _themeManager, _changeAdminView);
            _changeAdminView?.Invoke(view);
        }

        protected Func<IUnitOfWork, ThemeManager, ChangeAdminViewDelegate, T> CreateView<T>()
            where T : UserControl
                => (_unitOfWork, _themeManager, _changeAdminView)
                    => Activator.CreateInstance(
                        typeof(T), _unitOfWork, _themeManager, _changeAdminView) as T;

        protected void ConfirmLogout()
        {
            DialogResult confirm = MessageBox.Show("Are you sure you want to log out?",
                "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm == DialogResult.Yes)
            {
                RestartApplication();
            }
        }

        protected void RestartApplication()
        {
            string appPath = Application.ExecutablePath;
            System.Diagnostics.Process.Start(appPath);
            Application.Exit();
        }
    }
}