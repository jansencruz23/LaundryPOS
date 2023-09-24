using Guna.UI2.WinForms;
using LaundryPOS.Contracts;
using LaundryPOS.Models;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Caching.Memory;
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
        private readonly IMemoryCache _memoryCache;
        private const int FIRST_INDEX = 1;
        private const string THEME_SETTINGS_CACHE_KEY = "ThemeSettings";

        public ThemeManager(IUnitOfWork unitOfWork,
            IMemoryCache memoryCache)
        {
            _unitOfWork = unitOfWork;
            _memoryCache = memoryCache;
        }

        public async Task ApplyThemeToButton(Guna2Button button)
        {
            if (!_memoryCache.TryGetValue(THEME_SETTINGS_CACHE_KEY, out AppSettings appSettings))
            {
                appSettings = await _unitOfWork.AppSettingsRepo.GetByID(FIRST_INDEX);

                if (appSettings != null)
                {
                    _memoryCache.Set(THEME_SETTINGS_CACHE_KEY, appSettings, TimeSpan.FromHours(8));
                }
            }

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

        public async Task ApplyLighterThemeToPanel(Guna2Panel panel, float brightnessFactor = 0.8f)
        {
            if (!_memoryCache.TryGetValue(THEME_SETTINGS_CACHE_KEY, out AppSettings appSettings))
            {
                appSettings = await _unitOfWork.AppSettingsRepo.GetByID(FIRST_INDEX);

                if (appSettings != null)
                {
                    _memoryCache.Set(THEME_SETTINGS_CACHE_KEY, appSettings, TimeSpan.FromHours(8));
                }
            }

            if (appSettings != null)
            {
                var themeColor = ColorTranslator.FromHtml(appSettings.Theme);

                // Adjust the brightness of the theme color
                var adjustedColor = Color.FromArgb(
                    (int)(themeColor.R * brightnessFactor),
                    (int)(themeColor.G * brightnessFactor),
                    (int)(themeColor.B * brightnessFactor)
                );

                panel.FillColor = adjustedColor;

                var brightness = adjustedColor.GetBrightness();
                var foreColor = brightness < 0.5 ? Color.White : Color.Black;

                panel.ForeColor = foreColor;
            }
            else
            {
                panel.FillColor = Color.White;
                panel.ForeColor = Color.Black;
            }
        }
    }
}