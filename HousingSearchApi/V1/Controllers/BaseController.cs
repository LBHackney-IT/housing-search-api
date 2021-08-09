// TODO: 1 Return when last commit
//using Hackney.Core.Middleware;
using HousingSearchApi.V1.Boundary.Responses;
using HousingSearchApi.V1.Boundary.Responses.Metadata;
using HousingSearchApi.V1.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Net;
using System.Threading.Tasks;

namespace HousingSearchApi.V1.Controllers
{
    public class BaseController : Controller
    {
        public BaseController()
        {
            ConfigureJsonSerializer();
        }

        public string GetCorrelationId()
        {
            // TODO: 1 Return when last commit
            //if (HttpContext.Request.Headers[HeaderConstants.CorrelationId].Count == 0)
            //    throw new KeyNotFoundException("Request is missing a correlationId");

            //return HttpContext.Request.Headers[HeaderConstants.CorrelationId];
            return null;
        }

        public static void ConfigureJsonSerializer()
        {
            JsonConvert.DefaultSettings = () =>
            {
                var settings = new JsonSerializerSettings();
                settings.Formatting = Formatting.Indented;
                settings.ContractResolver = new CamelCasePropertyNamesContractResolver();

                settings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                settings.DateFormatHandling = DateFormatHandling.IsoDateFormat;

                return settings;
            };
        }

        protected async Task<IActionResult> UseErrorHandling(Func<Task<IActionResult>> action)
        {
            try
            {
                return await action().ConfigureAwait(false);
            }
            catch(NotFoundException ex)
            {
                return NotFound(new BaseErrorResponse(ex.GetFullMessage(), HttpStatusCode.NotFound));
            }
            // ToDO: add more catches
            catch(Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new BaseErrorResponse(ex.GetFullMessage()));
            }
        }
    }
}
