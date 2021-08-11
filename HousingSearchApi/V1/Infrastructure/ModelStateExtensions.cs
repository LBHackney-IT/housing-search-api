using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Linq;

namespace HousingSearchApi.V1.Infrastructure
{
    public static class ModelStateExtensions
    {
        public static string GetErrorMessage(this ModelStateDictionary modelState)
        {
            return string.Join(" ", modelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)));
        }
    }
}
