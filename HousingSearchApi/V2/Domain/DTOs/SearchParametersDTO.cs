using Microsoft.AspNetCore.Mvc;

namespace HousingSearchApi.V2.Domain.DTOs;

public class SearchParametersDto
{
    [FromQuery(Name = "searchText")]
    public string SearchText { get; set; } = string.Empty;

    [FromQuery(Name = "pageSize")]
    public int PageSize { get; set; } = 40;

    [FromQuery(Name = "pageNumber")]
    public int PageNumber { get; set; } = 1;

    [FromQuery(Name = "sortField")]
    public string SortField { get; set; } = "_score";

    [FromQuery(Name = "sortOrder")]
    public bool IsDesc { get; set; } = true;
}


