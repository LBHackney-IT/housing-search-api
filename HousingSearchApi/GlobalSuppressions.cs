// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "<Pending>", Scope = "member", Target = "~M:HousingSearchApi.V1.Controllers.GetPersonListController.GetPersonList(HousingSearchApi.V1.Boundary.Requests.GetPersonListRequest)~System.Threading.Tasks.Task{Microsoft.AspNetCore.Mvc.IActionResult}")]
[assembly: SuppressMessage("Globalization", "CA1303:Do not pass literals as localized parameters", Justification = "<Pending>", Scope = "member", Target = "~M:HousingSearchApi.V1.Interfaces.SearchPersonElasticSearchHelper.Search(HousingSearchApi.V1.Boundary.Requests.GetPersonListRequest)~System.Threading.Tasks.Task{Nest.ISearchResponse{HousingSearchApi.V1.Domain.ElasticSearch.QueryablePerson}}")]
[assembly: SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope", Justification = "<Pending>", Scope = "member", Target = "~M:HousingSearchApi.Startup.ConfigureElasticsearch(Microsoft.Extensions.DependencyInjection.IServiceCollection)")]
[assembly: SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "<Pending>", Scope = "member", Target = "~M:HousingSearchApi.V1.Controllers.GetTenureListController.GetPersonList(HousingSearchApi.V1.Boundary.Requests.GetTenureListRequest)~System.Threading.Tasks.Task{Microsoft.AspNetCore.Mvc.IActionResult}")]
