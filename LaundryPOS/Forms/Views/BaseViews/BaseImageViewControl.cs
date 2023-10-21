using LaundryPOS.Contracts;
using LaundryPOS.Delegates;
using LaundryPOS.Helpers;
using LaundryPOS.Models;

namespace LaundryPOS.Forms.Views
{
    public partial class BaseImageViewControl<T> : BaseViewControl
        where T : ImageEntity
    {
        public BaseImageViewControl(IUnitOfWork unitOfWork,
            ThemeManager themeManager,
            ChangeAdminViewDelegate changeAdminView)
            : base (unitOfWork, themeManager, changeAdminView)
        {
        }

        public BaseImageViewControl() { }

        protected Image GetImage(T t)
        {
            try
            {
                return t.Image != null
                ? Image.FromFile(t.Image)
                : Image.FromFile("./default.png");
            }
            catch
            {
                return Image.FromFile("./default.png");
            }
        }

        protected string SaveToImages(string imagePath, string folderName)
        {
            string uniqueName = $"{Guid.NewGuid().ToString()[^8..]}_" +
                $"{Path.GetFileName(imagePath)[Math.Max(0, Path.GetFileName(imagePath).Length - 10)..]}";
            string destinationPath = Path.Combine(Application.StartupPath, "Icons", "Images", folderName, uniqueName);
            Directory.CreateDirectory(Path.GetDirectoryName(destinationPath));
            File.Copy(imagePath, destinationPath);

            return uniqueName;
        }

        protected string GetImagePath(string imagePath, string folderName)
            => Path.Combine("Icons", "Images", folderName, Path.GetFileName(imagePath));
    }
}