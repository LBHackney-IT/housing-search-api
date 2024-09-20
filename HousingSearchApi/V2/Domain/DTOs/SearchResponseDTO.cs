using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace HousingSearchApi.V2.Domain.DTOs;

public class SearchResponseDto
{
    public IReadOnlyCollection<object> Documents;
    public long Total;
}


