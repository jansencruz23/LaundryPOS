using Guna.UI2.WinForms;
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
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

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

        protected async Task<string> GetBusinessName()
        {
            var appSettings = await _unitOfWork.AppSettingsRepo.GetFirstAppSettings();
            return appSettings.Name;
        }

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

        protected void StyleFontsButton(float size, params Guna2Button[] buttons)
        {
            Array.ForEach(buttons, btn 
                => btn.Font = _themeManager.HelveticaBold(size));
        }

        protected void StyleFontsTextBox(float size, params Guna2TextBox[] textbox)
        {
            Array.ForEach(textbox, txt
                => txt.Font = _themeManager.Helvetica(size));
        }

        protected void StyleFontsLabel(float size, bool bold, params Label[] labels)
        {
            if (bold)
            {
                Array.ForEach(labels, lbl
                    => lbl.Font = _themeManager.HelveticaBold(size));
            }
            else
            {
                Array.ForEach(labels, lbl
                    => lbl.Font = _themeManager.Helvetica(size));
            }
        }

        protected bool ValidateField(bool condition, Label label)
        {
            label.Visible = condition;
            return !condition;
        }

        protected bool IsValidDecimalInput(string input, out decimal value)
        {
            return decimal.TryParse(input, out value) && value > 0 && input.Length <= 6;
        }
    }
}