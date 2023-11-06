using Guna.UI2.WinForms;
using System.Text.RegularExpressions;

namespace LaundryPOS.Helpers
{
    public class RegexValidator
    {
        private static readonly string ITEM_NAME_PATTERN = @"^[a-zA-Z0-9!&*()-:',.|\s]*[a-zA-Z0-9]+[a-zA-Z0-9!&*()-:',.|\s]*$";
        private static readonly string PERSON_NAME_PATTERN = @"^[a-zA-Z\s\p{L}-]*[.,]*(?:[a-zA-Z\s\p{L}-][.,]*)*$";
        private static readonly string USERNAME_PATTERN = "^[a-zA-Z0-9_]{3,20}$";
        private static readonly string PASSWORD_PATTERN = "^[\\s\\S]{4,20}$";

        public static bool IsValidInput(string input, string pattern)
            => !string.IsNullOrWhiteSpace(input) && Regex.IsMatch(input, pattern);

        public static bool IsValidPersonName(string input)
            => IsValidInput(input, PERSON_NAME_PATTERN);

        public static bool IsValidUsername(string input)
            => IsValidInput(input, USERNAME_PATTERN);

        public static bool IsValidPassword(string input)
        => IsValidInput(input, PASSWORD_PATTERN);

        public static bool IsValidItemName(string input)
            => IsValidInput(input, ITEM_NAME_PATTERN);
    }
}