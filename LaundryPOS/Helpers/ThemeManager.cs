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

namespace LaundryPOS.Helpers
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

        public async Task ApplyThemeToButton(Guna2Button button, bool hasShadow = false)
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

                if (hasShadow)
                    button.ShadowDecoration.Color = themeColor;
            }
            else
            {
                button.FillColor = Color.White;
                button.ForeColor = Color.Black;
            }
        }

        public async Task ApplyThemeToButton(Guna2CircleButton button)
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

        public async Task ApplyThemeToPanel(Guna2Panel panel, float brightnessFactor = 1f, bool changeFontColor = false)
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
                if (changeFontColor)
                {
                    var foreColor = brightness < 0.5 ? Color.White : Color.Black;
                    panel.ForeColor = foreColor;
                }
            }
            else
            {
                panel.FillColor = Color.White;
                panel.ForeColor = Color.Black;
            }
        }

        public async Task ApplyLighterThemeToDataGridView(DataGridView dataGridView, float brightnessFactor = 0.8f, bool changeFont = false)
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

                // Apply the adjusted color to the DataGridView column header background
                dataGridView.ColumnHeadersDefaultCellStyle.BackColor = themeColor;
                dataGridView.ColumnHeadersDefaultCellStyle.SelectionBackColor = adjustedColor;

                var brightness = adjustedColor.GetBrightness();
                var foreColor = brightness < 0.5 ? Color.White : Color.Black;

                // Set the font for column headers
                dataGridView.ColumnHeadersDefaultCellStyle.Font = new Font("Poppins", 10f, FontStyle.Bold);
                dataGridView.ColumnHeadersDefaultCellStyle.ForeColor = foreColor;

                if (changeFont)
                    dataGridView.DefaultCellStyle.Font = new Font("Poppins", 10f, FontStyle.Regular);

                // Set alternating row background color to "whitesmoke"
                dataGridView.AlternatingRowsDefaultCellStyle.BackColor = Color.WhiteSmoke;
            }
            else
            {
                // Apply default colors and fonts if appSettings is not available
                dataGridView.ColumnHeadersDefaultCellStyle.BackColor = Color.White;
                dataGridView.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
                dataGridView.ColumnHeadersDefaultCellStyle.Font = new Font("Poppins", 10f, FontStyle.Bold);
                dataGridView.DefaultCellStyle.Font = new Font("Poppins", 10f, FontStyle.Regular);

                // Set alternating row background color to "whitesmoke"
                dataGridView.AlternatingRowsDefaultCellStyle.BackColor = Color.WhiteSmoke;
            }
        }
        public async Task ApplyOutlineThemeToButton(Guna2Button button, int borderThickness = 1)
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
                button.FillColor = Color.Transparent;
                button.BorderColor = themeColor;
                button.BorderThickness = borderThickness;

                var brightness = themeColor.GetBrightness();
                button.ForeColor = ColorTranslator.FromHtml(appSettings.Theme);
            }
            else
            {
                button.FillColor = Color.White;
                button.ForeColor = Color.Black;
            }
        }
    }
}