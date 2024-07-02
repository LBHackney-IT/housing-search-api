using System.Text.RegularExpressions;

namespace HousingSearchApi.V1.Helper
{
    public static class PostCodeHelpers
    {
        public static string NormalizePostcode(string postcode)
        {
            //removes space in middle spaces
            postcode = postcode.Replace(" ", "");

            postcode = postcode.ToUpper();

            return postcode.Length switch
            {
                5 => postcode.Insert(2, " "),
                6 => postcode.Insert(3, " "),
                7 => postcode.Insert(4, " "),
                _ => postcode,
            };
        }

        public static bool SearchTextIsValidPostCode(string searchText)
        {
            if (searchText == null) return false;

            var trimmed = searchText.Replace(" ", "");

            if (trimmed.Length > 7) return false;

            const string pattern = @"^(([a-z][0-9]{1,2})|([a-z][a-hj-y][0-9]{1,2})|([a-z][0-9][a-z])|([a-z][a-hj-y][0-9]?[a-z]))[0-9][a-z]{2}$";

            RegexOptions options = RegexOptions.IgnoreCase;

            return Regex.Match(trimmed, pattern, options).Success;
        }

    }
}
