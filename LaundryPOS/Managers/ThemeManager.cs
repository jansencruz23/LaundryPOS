using Guna.UI2.WinForms;
using LaundryPOS.Contracts;
using LaundryPOS.Models;
using Microsoft.Extensions.Caching.Memory;

namespace LaundryPOS.Managers
{
    public class ThemeManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMemoryCache _memoryCache;
        private const string THEME_SETTINGS_CACHE_KEY = "ThemeSettings";
        private readonly FontManager _fontManager;

        public ThemeManager(IUnitOfWork unitOfWork,
            IMemoryCache memoryCache,
            FontManager fontManager)
        {
            _unitOfWork = unitOfWork;
            _memoryCache = memoryCache;
            _fontManager = fontManager;
        }

        private async Task<AppSettings> GetAppSettings()
        {
            if (!_memoryCache.TryGetValue(THEME_SETTINGS_CACHE_KEY, out AppSettings appSettings))
            {
                appSettings = await _unitOfWork.AppSettingsRepo.GetFirstAppSettings();

                if (appSettings != null)
                {
                    _memoryCache.Set(THEME_SETTINGS_CACHE_KEY, appSettings, TimeSpan.FromHours(8));
                }
            }

            return appSettings;
        }

        public async Task ApplyThemeToButton(Guna2Button button, bool hasShadow = false)
        {
            var appSettings = await GetAppSettings();

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

        public async Task ApplyFocusedThemeToTextBox(Guna2TextBox textbox)
        {
            var appSettings = await GetAppSettings();

            if (appSettings != null)
            {
                var themeColor = ColorTranslator.FromHtml(appSettings.Theme);
                textbox.FocusedState.BorderColor = themeColor;
                textbox.HoverState.BorderColor = themeColor;
                textbox.BorderThickness = 1;
            }
            else
            {
                textbox.FillColor = Color.White;
                textbox.ForeColor = Color.Black;
            }
        }

        public async Task ApplyThemeToLabel(Label label)
        {
            var appSettings = await GetAppSettings();

            if (appSettings != null)
            {
                var themeColor = ColorTranslator.FromHtml(appSettings.Theme);
                label.ForeColor = themeColor;
            }
            else
            {
                label.ForeColor = Color.Black;
            }
        }

        public async Task ApplyThemeToPanel(Guna2Panel panel, float brightnessFactor = 1f, bool changeFontColor = false)
        {
            var appSettings = await GetAppSettings();

            if (appSettings != null)
            {
                var themeColor = ColorTranslator.FromHtml(appSettings.Theme);

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
            var appSettings = await GetAppSettings();

            if (appSettings != null)
            {
                var themeColor = ColorTranslator.FromHtml(appSettings.Theme);

                var adjustedColor = Color.FromArgb(
                    (int)(themeColor.R * brightnessFactor),
                    (int)(themeColor.G * brightnessFactor),
                    (int)(themeColor.B * brightnessFactor)
                );

                dataGridView.ColumnHeadersDefaultCellStyle.BackColor = themeColor;
                dataGridView.ColumnHeadersDefaultCellStyle.SelectionBackColor = adjustedColor;

                var brightness = adjustedColor.GetBrightness();
                var foreColor = brightness < 0.5 ? Color.White : Color.Black;

                dataGridView.ColumnHeadersDefaultCellStyle.Font = _fontManager.HelveticaBold(11.25f);
                dataGridView.ColumnHeadersHeight = 30;
                dataGridView.ColumnHeadersDefaultCellStyle.ForeColor = foreColor;

                if (changeFont)
                {
                    dataGridView.DefaultCellStyle.Font = _fontManager.Helvetica(11.25f);
                    dataGridView.AlternatingRowsDefaultCellStyle.Font = _fontManager.Helvetica(11.25f);
                }

                dataGridView.RowTemplate.Height = 35;
                dataGridView.AlternatingRowsDefaultCellStyle.BackColor = Color.WhiteSmoke;
            }
            else
            {
                dataGridView.ColumnHeadersDefaultCellStyle.BackColor = Color.White;
                dataGridView.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
                dataGridView.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 11f, FontStyle.Bold);
                dataGridView.DefaultCellStyle.Font = new Font("Arial", 11f, FontStyle.Regular);
                dataGridView.AlternatingRowsDefaultCellStyle.BackColor = Color.WhiteSmoke;
            }
        }
        public async Task ApplyOutlineThemeToButton(Guna2Button button, int borderThickness = 1)
        {
            var appSettings = await GetAppSettings();

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