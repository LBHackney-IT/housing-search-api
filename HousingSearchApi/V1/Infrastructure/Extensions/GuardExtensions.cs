namespace HousingSearchApi.V1.Infrastructure
{
    /// <summary>
    /// Guard that will help to avoid some common exceptions
    /// </summary>
    public static class GuardExtensions
    {
        /// <summary>
        /// Checks if parentText contains innerText. Returns null if parentText is null or empty
        /// </summary>
        /// <returns></returns>
        public static bool SafeContains(this string parentText, string innerText)
            => !string.IsNullOrEmpty(parentText) && parentText.Contains(innerText);
    }
}
