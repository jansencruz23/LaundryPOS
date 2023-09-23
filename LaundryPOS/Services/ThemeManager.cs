using Guna.UI2.WinForms;
using LaundryPOS.Contracts;
using LaundryPOS.Models;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaundryPOS.Services
{
    public class ThemeManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private const int FIRST_INDEX = 1;

        public ThemeManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task ApplyThemeToButton(Guna2Button button)
        {
            var appSettings = await _unitOfWork.AppSettingsRepo.GetByID(FIRST_INDEX);

            if (appSettings != null)
            {
                var themeColor = ColorTranslator.FromHtml(appSettings.Theme);
                button.FillColor = themeColor;

                var brightness = themeColor.GetBrightness();
                var foreColor = brightness < 0.5 ? Color.White : Color.Black;

                button.ForeColor = foreColor;
            }
            else
            {
                button.FillColor = Color.White;
                button.ForeColor = Color.Black;
            }
        }
    }
}