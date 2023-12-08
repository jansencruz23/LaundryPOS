namespace LaundryPOS.Models
{
    public class AppSettings : ImageEntity
    {
        public string Name { get; set; }

        public string Theme { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Address { get; set; }

        public string? Email { get; set; }

        public string? Description { get; set; }
    }
}