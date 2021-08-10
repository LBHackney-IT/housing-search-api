using System;

namespace HousingSearchApi.V1.Infrastructure
{
    /// <summary>
    /// Class which provide extensions for exception
    /// </summary>
    public static class ExceptionExtensions
    {
        /// <summary>
        /// This extension is used for get full message from exception by collecting all messages from inner exceptions with ';' separator
        /// </summary>
        public static string GetFullMessage(this Exception ex)
        {
            if (ex == null)
            {
                return "Exception message is empty";
            }

            return ex.Message + "; " + ex.InnerException?.GetFullMessage();
        }
    }
}
