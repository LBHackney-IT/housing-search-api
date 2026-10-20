# Housing Search API

The Housing Search API is the London Borough of Hackney (LBH) **MTFH** service responsible for searching
for **persons, tenures, assets, accounts, transactions and processes**. It is a read-only, Elasticsearch-backed
search API: documents are indexed by the platform's other services and queried here.

## Stack

- **.NET 10** (`net10.0`) — ASP.NET Core Web API hosted on **AWS Lambda** via `Amazon.Lambda.AspNetCoreServer`
  (the managed `dotnet10` runtime).
- **Elasticsearch** as the datastore, accessed with **NEST**.
- **Swashbuckle** for Swagger/OpenAPI, **FluentValidation** for request validation.
- **MVC API versioning** (URL segment reader) with a V1/V2 split.
- **Newtonsoft.Json** as the MVC JSON formatter.
- **AWS X-Ray** for tracing.
- **xUnit** (+ `TestStack.BDDfy` for the E2E stories) and **NUnit** for the few remaining NUnit-style fixtures.

### API definition

Swagger Hub — https://app.swaggerhub.com/apis/Hackney/housingSearchApi/1.0.0

The running service also serves its own Swagger UI at `/swagger` (and `/swagger/{proxy+}` through API Gateway).

## Endpoints

Versioning uses `UrlSegmentApiVersionReader` with `DefaultApiVersion = 1.0` and
`AssumeDefaultVersionWhenUnspecified = true`. Every V1 controller is decorated `[ApiVersion("1")]`,
`[ApiController]`, `[Produces("application/json")]`.

The V1 controllers share a common query base (`HousingSearchRequest`): `searchText`, `pageSize` (default 12),
`page`, `sortBy`, `isDesc`, `issimplequery`, `isFilteredQuery`.

| Verb | Route | Additional query parameters | Response |
| --- | --- | --- | --- |
| GET | `/api/v1/search/persons` | `personType` | `APIResponse<GetPersonListResponse>` — `results.persons[]`, `total` |
| GET | `/api/v1/search/accounts` | — | `APIResponse<GetAccountListResponse>` — `results.accounts[]`, `total` |
| GET | `/api/v1/search/tenures` | `uprn`, `bookingStatus`, `isTemporaryAccommodation` | `APIResponse<GetTenureListResponse>` — `results.tenures[]`, `total` |
| GET | `/api/v1/search/tenures/all` | `uprn`, `bookingStatus`, `isTemporaryAccommodation`, `lastHitId`, `lastHitTenureStartDate` | `APIAllTenureResponse<GetAllTenureListResponse>` — `results[]`, `total`, `lastHitId`, `lastHitTenureStartDate`. Search-after paging; only valid when sorting by `tenureStartDate`. |
| GET | `/api/v1/search/assets` | `assetTypes`, `numberOfBedrooms`, `isActive`, `useCustomSorting` | `APIResponse<GetAssetListResponse>` — `results.assets[]`, `total`. Returns 400 if `useCustomSorting=true` is combined with `page`/`sortBy`. |
| GET | `/api/v1/search/assets/all` | `lastHitId`, `assetStatus`, `numberOfBedSpaces`, `numberOfCots`, `floorNo`, `privateBathroom`, `privateKitchen`, `stepFree`, `contractApprovalStatus`, `contractApprovalStatusReason`, `contractIsActive`, `contractEndReason`, `chargesSubType`, `isTemporaryAccomodation`, `parentAssetId`, `tenureType`, `temporaryAccommodationParentAssetId`, `isPartOfTemporaryAccommodationBlock` | `APIAllResponse<GetAllAssetListResponse>` — `results[]`, `total`, `lastHitId` |
| GET | `/api/v1/search/assetrelationships` | `searchText` (required; blank → 400 `"Request searchtext cannot be blank"`) | `GetAssetRelationshipsResponse` — `childAssets[]` (not wrapped) |
| GET | `/api/v1/search/processes` | `targetType`, `targetId`, `isOpen`, `processNames` | `APIResponse<GetProcessListResponse>` — `results.processes[]`, `total` |
| GET | `/api/v1/search/transactions` | `startDate`, `endDate` | `APIResponse<GetTransactionListResponse>` — `results.total`, `results.transactions[]` |
| GET | `/api/v2/search/{indexName}` | `searchText` (default `""`), `pageSize` (default 120), `page` (default 1) | Unwrapped `{ results: { <indexName>: [ ... ] }, total }`. Each document is a raw JSON object with an injected `confidenceScore`. |
| GET | `/api/v1/healthcheck/ping` | — | Health report written by `HealthCheckResponseWriter` (unversioned; mapped outside MVC). |
| GET | `/swagger` | — | Swagger UI / OpenAPI document, one document per API version. |

Important characteristics:

- **V1** is the typed search API (8 controllers under `V1/Controllers/`), each going through a use case to the
  search gateway and NEST. V1 is what `make test` covers.
- **V2** is a single generic `SearchController` (`V2/Controllers/SearchController.cs`) that takes the index
  name from the URL and returns raw Elasticsearch documents rather than typed DTOs. V2 is covered by
  `make test-v2`.
- All endpoints are read-only and sit behind the API Gateway request authorizer and API key.

## Implementing a new search endpoint

### Create the controller endpoint

Create a new controller exposing the search endpoint. Refer to the existing ones and to
[Swagger Hub](https://app.swaggerhub.com/apis/Hackney/housingSearchApi/1.0.0) for the request/response
format. Most searches share the same `HousingSearchRequest`; the response varies with what is requested.

### Create a new use case

Each new search requires its own search use case. Name it appropriately.

### Create the new search method in the search gateway

Each use case calls the search gateway to perform the search. The gateway is the layer between the use cases
and Elasticsearch. New methods look much like the existing ones — the only difference is the response type
they return.

### Create a new query generator and add it to the query factory

Use [NEST](https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/introduction.html) to build
the query you want to run. Then add the new type to the `QueryFactory` — a generic factory that returns query
generators based on the query search type.

### Create a new sort generator (if necessary)

If you need sorting, create a sort generator and add it to the `SortFactory`. By default, sorting is based
on ranking.

## Data sources and external dependencies

| Dependency | Detail |
| --- | --- |
| Elasticsearch indices | `persons`, `tenures`, `assets`, `accounts`, `transactions`, `processes` (see `V1/Infrastructure/IndexSelector.cs`). |
| Index / mapping definitions | `HousingSearchApi/data/elasticsearch/*Index.json` plus `seed-data.json`. Note there is no `accountsIndex.json`, and the test fixtures read these files from `./data/elasticsearch`. |
| AWS-managed Elasticsearch | Domain `housing-search-api-es`, endpoint published to SSM and injected as `ELASTICSEARCH_DOMAIN_URL`. Development/staging run `7.10`, production/pre-production run `7.8`. |
| Local Elasticsearch | `docker.elastic.co/elasticsearch/elasticsearch:7.9.3` with the `analysis-icu` plugin, plus Kibana 7.9.3 (`docker-compose.yml`). |
| AWS X-Ray | `AWSSDKHandler.RegisterXRayForAllServices()` + `app.UseXRay(...)`; `AWSXRayDaemonWriteAccess` on the Lambda role. |
| AWS SSM Parameter Store | `/housing-tl/{stage}/aspnetcore-environment`; per-stage Elasticsearch endpoint. |
| Postgres / DynamoDB | **Not used.** `database.env`, `database/Makefile` and the DynamoDB settings in `appsettings*.json` are stale template leftovers, and `serverless.yml` carries an unimplemented TODO about a postgres mirror. |

## Setup

### Prerequisites

1. [Docker][docker-download] with the **`docker compose`** v2 plugin.
2. A **`LBHPACKAGESTOKEN`** with `read:packages` access to the `LBHackney-IT` GitHub Packages feed — the
   `Hackney.*` packages are not published to nuget.org:
   ```sh
   export LBHPACKAGESTOKEN=<token>
   ```
3. Optionally copy [`.env.sample`](.env.sample) to `.env` and set `LBHPACKAGESTOKEN` there instead of
   exporting it — `docker compose` reads `.env` for the values referenced in `docker-compose.yml`. The file
   also lists every environment variable the API reads at runtime; those are **not** loaded from `.env`, so
   export them (or add them to the compose service) if you need to override the defaults.

### Build and run

```sh
git clone git@github.com:LBHackney-IT/housing-search-api.git
cd housing-search-api
make serve     # builds and starts the API plus Elasticsearch and Kibana (port 3000)
make build     # build only
make shell     # bash inside the API container
```

The application reads its search endpoint from `ElasticSearchDomainUrl`/`ELASTICSEARCH_DOMAIN_URL`
(`http://test-elasticsearch:9200` in the compose stack).

## Testing

```sh
make test       # V1 suite: docker compose build/run housing-search-api-test
make test-v2    # V2 suite: docker compose build/run housing-search-api-test-v2
```

Both targets build the test image (`HousingSearchApi.Tests/Dockerfile`) and run xUnit inside it against the
compose-provided Elasticsearch instance. `make test` filters to `HousingSearchApi.Tests.V1`; the V2 suite is a
separate target (and a separate CI job).

Notes:

- The test project references `Docker.DotNet` but does not use it — no Docker socket is required. Elasticsearch
  is reached over HTTP using `ELASTICSEARCH_DOMAIN_URL`.
- `Microsoft.EntityFrameworkCore.SqlServer` in the test project is unused.

### Agreed testing approach

- xUnit as the primary framework, `TestStack.BDDfy` for the E2E stories, FluentAssertions and Moq.
- Tests should be independent of each other.
- E2E tests run against a real (containerised) Elasticsearch instance.
- Unit and E2E tests run in CI.

## Deployment

CircleCI (`.circleci/config.yml`) gates deployment on a code-formatting check (`dotnet-format --check`), the
**V1 and V2** Docker test suites (the test image also runs the SonarCloud scanner when `SONAR_TOKEN` is
supplied), then deploys infrastructure with **Terraform** and the Lambda with the **Serverless Framework**.
Changes are reviewed and approved on a branch before merging.

**Stages and triggers:**

- **Feature branches** (anything except `master` and `release`) — formatting check and both test suites only.
- **`master`** — formatting and both suites, then Terraform plan/compliance/apply to development and deployment
  to **development**.
- **`release`** — both suites, then Terraform and deployment to **staging**. Production follows only after the
  `permit-production-terraform-release` and `permit-production-release` manual approvals.
- **`release`** — the separate `deploy-terraform-pre-production` and `deploy-code-pre-production` workflows
  deploy **pre-production**, each behind its own manual approval.

**Packaging and runtime:**

- `build.sh` installs `Amazon.Lambda.Tools` and runs
  `dotnet lambda package --configuration release --framework net10.0 --output-package ./bin/release/net10.0/housing-search-api.zip`.
  (`build.cmd` is the Windows equivalent for local use.)
- The deploy job runs `sls deploy --stage <stage> --conceal`.
- `serverless.yml` declares `runtime: dotnet10`, consumes that artifact, and exposes `ANY /{proxy+}` plus
  `GET /swagger/{proxy+}` behind the request authorizer, with the `Platform_APIs_Web_ACL` WAF attached.

## Contacts

**Housing product team** — [mmh-project-team@hackney.gov.uk](mailto:mmh-project-team@hackney.gov.uk)

[docker-download]: https://www.docker.com/products/docker-desktop
