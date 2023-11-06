using Guna.UI2.WinForms;
using LaundryPOS.Contracts;
using LaundryPOS.Delegates;
using LaundryPOS.Helpers;
using LaundryPOS.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

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

        protected async Task PrintTable(Guna2DataGridView table, string subtitle)
        {
            var printer = new DGVPrinter();
            var businessName = await GetBusinessName();

            printer.Title = businessName;
            printer.SubTitle = string.Format(subtitle, printer.SubTitleColor = Color.Black, printer);
            printer.SubTitleSpacing = 15;
            printer.SubTitleFormatFlags = StringFormatFlags.LineLimit | StringFormatFlags.NoClip;
            printer.PageNumbers = true;
            printer.PageNumberInHeader = false;
            printer.PorportionalColumns = true;
            printer.RowHeight = DGVPrinter.RowHeightSetting.CellHeight;
            printer.HeaderCellAlignment = StringAlignment.Near;
            printer.Footer = businessName;
            printer.FooterSpacing = 15;
            printer.PrintDataGridView(table);
        }

        protected void ConfigureImageColumn(Guna2DataGridView table)
        {
            if (table.Columns["Icon"] == null)
            {
                var imageColumn = new DataGridViewImageColumn
                {
                    HeaderText = "Icon",
                    Name = "Icon",
                    ImageLayout = DataGridViewImageCellLayout.Zoom
                };

                table.Columns.Add(imageColumn);
                table.Columns["Icon"].DisplayIndex = 0;
                table.Columns["Icon"].Width = 80;
                table.Columns["Icon"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            }
        }

        protected void HandleImageColumnFormatting(Guna2DataGridView table)
        {
            try
            {
                table.CellFormatting += (sender, e) =>
                {
                    if (e.ColumnIndex == table.Columns["Icon"].Index && e.RowIndex >= 0)
                    {
                        var rowData = table.Rows[e.RowIndex].DataBoundItem as T;
                        e.Value = GetImage(rowData);
                    }
                };
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occured " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}