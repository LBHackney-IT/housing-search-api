# Contributing to the Housing Search API

This document covers local setup, build and run, testing, how to add a new search endpoint, and the
development/deployment workflow for the Housing Search API. For an overview of the service, its endpoints and
dependencies, see the [README](README.md).

## Prerequisites

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

## Build and run

```sh
git clone git@github.com:LBHackney-IT/housing-search-api.git
cd housing-search-api
make serve     # builds and starts the API plus Elasticsearch and Kibana (port 3000)
make build     # build only
make shell     # bash inside the API container
```

`make serve` starts the API on <http://localhost:3000> with the compose-provided Elasticsearch
(`docker.elastic.co/elasticsearch/elasticsearch:7.9.3`, port 9200) and Kibana (port 5601). Swagger UI is
available at `/swagger`; the health check is at `/api/v1/healthcheck/ping`.

The application reads its search endpoint from `ElasticSearchDomainUrl`/`ELASTICSEARCH_DOMAIN_URL`
(`http://test-elasticsearch:9200` in the compose stack, `http://localhost:9200` when unset).

## Testing

```sh
make test       # V1 suite: docker compose build/run housing-search-api-test
make test-v2    # V2 suite: docker compose build/run housing-search-api-test-v2
```

Both targets build the test image ([`HousingSearchApi.Tests/Dockerfile`](HousingSearchApi.Tests/Dockerfile))
and run xUnit inside it against the compose-provided Elasticsearch instance. `make test` filters to
`HousingSearchApi.Tests.V1`; the V2 suite is a separate target (and a separate CI job). The test image also
runs the SonarCloud scanner when `SONAR_TOKEN` is supplied.

Notes:

- The test project references `Docker.DotNet` but does not use it — no Docker socket is required.
  Elasticsearch is reached over HTTP using `ELASTICSEARCH_DOMAIN_URL`.
- `Microsoft.EntityFrameworkCore.SqlServer` in the test project is unused.

### Agreed testing approach

- xUnit as the primary framework, `TestStack.BDDfy` for the E2E stories, FluentAssertions and Moq.
- Tests should be independent of each other.
- E2E tests run against a real (containerised) Elasticsearch instance.
- Unit and E2E tests run in CI.

## Implementing a new search endpoint

### Create the controller endpoint

Create a new controller exposing the search endpoint. Refer to the existing ones and to
[Swagger Hub](https://app.swaggerhub.com/apis/Hackney/housingSearchApi/1.0.0) for the request/response
format. Most searches share the same `HousingSearchRequest` base; the response varies with what is requested.

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

## Development workflow

- **Formatting** — code must pass `dotnet-format --check`; run `make lint` (or `dotnet format`) to fix
  formatting locally before pushing. CI fails on formatting drift.
- **Branches and review** — work on a feature branch and open a pull request; changes are reviewed and
  approved before merging.
- **Static analysis** — SonarCloud (`sonar-project.properties`) runs as part of the build-and-test jobs.
- **Infrastructure changes** — environment configuration lives under [`terraform/`](terraform) (one folder
  per environment) and is applied by CI.

### CI/CD and deployment

CircleCI ([`.circleci/config.yml`](.circleci/config.yml)) gates deployment on a code-formatting check
(`dotnet-format --check`) and the **V1 and V2** Docker test suites, then deploys infrastructure with
**Terraform** and the Lambda with the **Serverless Framework**.

**Stages and triggers:**

- **Feature branches** (anything except `master` and `release`) — formatting check and both test suites only.
- **`master`** — formatting and both suites, then Terraform plan/compliance/apply to development and
  deployment to **development**.
- **`release`** — both suites, then Terraform and deployment to **staging**. Production follows only after the
  `permit-production-terraform-release` and `permit-production-release` manual approvals.
- **`release`** — the separate `deploy-terraform-pre-production` and `deploy-code-pre-production` workflows
  deploy **pre-production**, each behind its own manual approval.

**Packaging and runtime:**

- [`HousingSearchApi/build.sh`](HousingSearchApi/build.sh) installs `Amazon.Lambda.Tools` and runs
  `dotnet lambda package --configuration release --framework net10.0 --output-package ./bin/release/net10.0/housing-search-api.zip`.
  (`build.cmd` is the Windows equivalent for local use.)
- The deploy job runs `sls deploy --stage <stage> --conceal`.
- [`HousingSearchApi/serverless.yml`](HousingSearchApi/serverless.yml) declares `runtime: dotnet10`,
  consumes that artifact, and exposes `ANY /{proxy+}` plus `GET /swagger/{proxy+}` behind the request
  authorizer, with the `Platform_APIs_Web_ACL` WAF attached.

[docker-download]: https://www.docker.com/products/docker-desktop
