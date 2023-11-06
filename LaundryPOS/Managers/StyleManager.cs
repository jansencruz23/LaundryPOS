using LaundryPOS.Contracts;
using Microsoft.Extensions.Caching.Memory;

namespace LaundryPOS.Managers
{
    public class StyleManager : IStyleManager
    {
        private ThemeManager _themeManager;
        private FontManager _fontManager;
        private IUnitOfWork _unitOfWork;
        private IMemoryCache _cache;

        public StyleManager(ThemeManager themeManager,
            FontManager fontManager, IUnitOfWork unitOfWork,
            IMemoryCache cache)
        {
            _themeManager = themeManager;
            _fontManager = fontManager;
            _unitOfWork = unitOfWork;
            _cache = cache;
        }

        public ThemeManager Theme =>
            _themeManager ??= new ThemeManager(_unitOfWork, _cache, Font);

        public FontManager Font =>
            _fontManager ??= new FontManager();
    }
}