using System.Text.RegularExpressions;

namespace HousingSearchApi.V1.Helper
{
    public static class PostCodeHelpers
    {
        private const string EmbeddedPostcodeExpression = @"\b[A-Z]{1,2}[0-9]{1,2}\s*[0-9][A-Z]{2}\b";

        private static readonly Regex _embeddedPostcode =
            new Regex(EmbeddedPostcodeExpression, RegexOptions.IgnoreCase | RegexOptions.Compiled);

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

        public static string NormalizeEmbeddedPostcode(string searchText)
        {
            return _embeddedPostcode.Replace(searchText, m => NormalizePostcode(m.ToString()));
        }

        public static bool SearchTextIsValidPostCode(string searchText)
        {
            if (searchText == null) return false;

            var trimmed = searchText.Replace(" ", "");

            if (trimmed.Length > 7) return false;

            const string pattern = @"^[A-Z]{1,2}[0-9]{1,2} ?[0-9][A-Z]{2}$";
            RegexOptions options = RegexOptions.IgnoreCase;

            return Regex.Match(trimmed, pattern, options).Success;
        }
    }
}
