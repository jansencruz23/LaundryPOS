using LaundryPOS.Managers;

namespace LaundryPOS.Contracts
{
    public interface IStyleManager
    {
        ThemeManager Theme { get; }
        FontManager Font { get; }
    }
}