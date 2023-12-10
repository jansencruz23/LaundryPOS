namespace LaundryPOS.Helpers
{
    public static class ImageSaveHelper
    {
        public static string GetImagePath(string imagePath, string folder)
           => Path.Combine("Icons", "Images", folder, Path.GetFileName(imagePath));

        public static string SaveToImages(string imagePath, string folder)
        {
            string uniqueName = $"{Guid.NewGuid().ToString()[^8..]}_" +
                $"{Path.GetFileName(imagePath)[Math.Max(0, Path.GetFileName(imagePath).Length - 10)..]}";
            string destinationPath = Path.Combine(Application.StartupPath, "Icons", "Images", folder, uniqueName);
            Directory.CreateDirectory(Path.GetDirectoryName(destinationPath));
            File.Copy(imagePath, destinationPath);

            return uniqueName;
        }
    }
}