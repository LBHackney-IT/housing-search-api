// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Design", "CA1001:Types that own disposable fields should be disposable", Justification = "<Pending>", Scope = "type", Target = "~T:HousingSearchApi.Tests.V1.Controllers.GetPersonListControllerTests")]
[assembly: SuppressMessage("Design", "CA1001:Types that own disposable fields should be disposable", Justification = "<Pending>", Scope = "type", Target = "~T:HousingSearchApi.Tests.IntegrationTests")]
[assembly: SuppressMessage("Design", "CA1063:Implement IDisposable Correctly", Justification = "<Pending>", Scope = "type", Target = "~T:HousingSearchApi.Tests.V1.Helper.PersonsFixture")]
[assembly: SuppressMessage("Usage", "CA2211:Non-constant fields should not be visible", Justification = "<Pending>", Scope = "member", Target = "~F:HousingSearchApi.Tests.V1.Helper.PersonDataHelper.ALPHABET")]
[assembly: SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope", Justification = "<Pending>", Scope = "member", Target = "~M:HousingSearchApi.Tests.ElasticsearchTests.SetupElasticsearchConnection~Nest.ElasticClient")]
[assembly: SuppressMessage("Usage", "CA1816:Dispose methods should call SuppressFinalize", Justification = "<Pending>", Scope = "member", Target = "~M:HousingSearchApi.Tests.V1.Helper.PersonsFixture.Dispose")]
[assembly: SuppressMessage("Usage", "CA2211:Non-constant fields should not be visible", Justification = "<Pending>", Scope = "member", Target = "~F:HousingSearchApi.Tests.V1.E2ETests.Fixtures.PersonsFixture.Alphabet")]
[assembly: SuppressMessage("Usage", "CA2211:Non-constant fields should not be visible", Justification = "<Pending>", Scope = "member", Target = "~F:HousingSearchApi.Tests.V1.E2ETests.Fixtures.AssetFixture.Alphabet")]
[assembly: SuppressMessage("Usage", "CA2211:Non-constant fields should not be visible", Justification = "<Pending>", Scope = "member", Target = "~F:HousingSearchApi.Tests.V1.E2ETests.Fixtures.TenureFixture.Alphabet")]
