using System;
using System.Threading.Tasks;
using HousingSearchApi.V1.Domain;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Nest;

namespace HousingSearchApi.V1.Infrastructure
{
    public class CorrelationMiddleware
    {
        private readonly RequestDelegate _next;

        public CorrelationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Headers[Constants.CorrelationId].Count == 0)
            {
                context.Request.Headers[Constants.CorrelationId] = Guid.NewGuid().ToString();
            }

            if (_next != null)
                await _next(context).ConfigureAwait(false);
        }
    }

    public static class CorrelationMiddlewareExtensions
    {
        public static IApplicationBuilder UseCorrelation(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CorrelationMiddleware>();
        }
    }

    public interface IPersonListSortFactory
    {
        IPersonListSort Create(GetPersonListRequest request);
    }

    public class PersonListSortFactory : IPersonListSortFactory
    {
        public IPersonListSort Create(GetPersonListRequest request)
        {
            throw new NotImplementedException();
        }
    }

    public interface IPersonListSort
    {
        SortDescriptor<QueryablePerson> Get();
    }

    public class LastNameAtoZ : IPersonListSort
    {
        public SortDescriptor<QueryablePerson> Get()
        {
            throw new NotImplementedException();
        }
    }

    public class LastNameZtoA : IPersonListSort
    {
        public SortDescriptor<QueryablePerson> Get()
        {
            throw new NotImplementedException();
        }
    }

    public class StreetNameAtoZ : IPersonListSort
    {
        public SortDescriptor<QueryablePerson> Get()
        {
            throw new NotImplementedException();
        }
    }

    public class StreetNameZtoA : IPersonListSort
    {
        public SortDescriptor<QueryablePerson> Get()
        {
            throw new NotImplementedException();
        }
    }
}
