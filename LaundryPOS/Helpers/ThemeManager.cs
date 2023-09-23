using Guna.UI2.WinForms;
using LaundryPOS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaundryPOS.Helpers
{
    public class ThemeManager
    {
        private readonly AppSettings _appSettings;
        public ThemeManager(AppSettings appSettings)
        {
            _appSettings = appSettings;
        }

        public AppSettings GetCurrentTheme() => _appSettings;

        //public void ApplyThemeToButton(Guna2Button)
    }
}