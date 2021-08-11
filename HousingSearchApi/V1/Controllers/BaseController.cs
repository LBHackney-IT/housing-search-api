using Hackney.Core.Middleware;
using HousingSearchApi.V1.Boundary.Responses;
using HousingSearchApi.V1.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
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
            if (HttpContext.Request.Headers[HeaderConstants.CorrelationId].Count == 0)
                throw new KeyNotFoundException("Request is missing a correlationId");

            return HttpContext.Request.Headers[HeaderConstants.CorrelationId];
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
            catch (ArgumentNullException ex)
            {
                return BadRequest(new BaseErrorResponse(ex.Message, HttpStatusCode.BadRequest));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new BaseErrorResponse(ex.Message, HttpStatusCode.BadRequest));
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(new BaseErrorResponse(ex.Message, HttpStatusCode.BadRequest));
            }
            catch (Exception ex)
            {
                return StatusCode((int) HttpStatusCode.InternalServerError, new BaseErrorResponse(ex.GetFullMessage()));
            }
        }
    }
}
