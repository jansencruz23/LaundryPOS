using LaundryPOS.Contracts;
using LaundryPOS.Delegates;
using LaundryPOS.Helpers;
using LaundryPOS.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LaundryPOS.Forms.Views
{
    public partial class BaseImageViewControl<T> : UserControl
        where T : ImageEntity
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly ThemeManager _themeManager;
        protected readonly ChangeAdminViewDelegate _changeAdminView;

        public BaseImageViewControl(IUnitOfWork unitOfWork,
            ThemeManager themeManager,
            ChangeAdminViewDelegate changeAdminView)
        {
            _unitOfWork = unitOfWork;
            _themeManager = themeManager;
            _changeAdminView = changeAdminView;
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
    }
}