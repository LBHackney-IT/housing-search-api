using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HousingSearchApi.V2.Domain.DTOs;

public class SearchParametersDto
{
    [BindNever]
    public IDictionary<string, string[]> Filters { get; set; } = new Dictionary<string, string[]>();

    [FromQuery(Name = "searchText")]
    public string SearchText { get; set; } = string.Empty;

    [FromQuery(Name = "pageSize")]
    public int PageSize { get; set; } = 120;

    [FromQuery(Name = "page")]
    public int Page { get; set; } = 1;
}


