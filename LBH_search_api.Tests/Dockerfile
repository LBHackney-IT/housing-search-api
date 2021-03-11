FROM mcr.microsoft.com/dotnet/core/sdk:3.1

# disable microsoft telematry
ENV DOTNET_CLI_TELEMETRY_OPTOUT='true'

WORKDIR /app

# Copy csproj and restore as distinct layers
COPY ./LBH_search_api.sln ./
COPY ./LBH_search_api/LBH_search_api.csproj ./LBH_search_api/
COPY ./LBH_search_api.Tests/LBH_search_api.Tests.csproj ./LBH_search_api.Tests/

RUN dotnet restore ./LBH_search_api/LBH_search_api.csproj
RUN dotnet restore ./LBH_search_api.Tests/LBH_search_api.Tests.csproj

# Copy everything else and build
COPY . .

RUN dotnet build -c debug -o out LBH_search_api.Tests/LBH_search_api.Tests.csproj

CMD dotnet test
