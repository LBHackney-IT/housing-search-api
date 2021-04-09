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
            if (string.IsNullOrEmpty(request.SortBy))
                return new DefaultSort();

            switch (request.IsDesc)
            {
                case true:
                    return new LastNameDesc();
                case false:
                    return new LastNameAsc();
            }
        }
    }

    public interface IPersonListSort
    {
        SortDescriptor<QueryablePerson> Get(SortDescriptor<QueryablePerson> descriptor);
    }

    public class DefaultSort : IPersonListSort
    {
        public SortDescriptor<QueryablePerson> Get(SortDescriptor<QueryablePerson> descriptor)
        {
            return descriptor;
        }
    }

    public class LastNameAsc : IPersonListSort
    {
        public SortDescriptor<QueryablePerson> Get(SortDescriptor<QueryablePerson> descriptor)
        {
            return descriptor
                .Ascending(f => f.Surname)
                .Ascending(f => f.Firstname);
        }
    }

    public class LastNameDesc : IPersonListSort
    {
        public SortDescriptor<QueryablePerson> Get(SortDescriptor<QueryablePerson> descriptor)
        {
            return descriptor
                .Descending(f => f.Surname)
                .Descending(f => f.Firstname);
        }
    }
}
