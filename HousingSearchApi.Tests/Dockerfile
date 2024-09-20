FROM mcr.microsoft.com/dotnet/sdk:6.0

# disable microsoft telematry
ENV DOTNET_CLI_TELEMETRY_OPTOUT='true'

ARG LBHPACKAGESTOKEN
ENV LBHPACKAGESTOKEN=$LBHPACKAGESTOKEN

WORKDIR /app

# Copy csproj and nuget config and restore as distinct layers
COPY ./HousingSearchApi.sln ./
COPY ./HousingSearchApi.Tests/HousingSearchApi.Tests.csproj ./HousingSearchApi.Tests/
COPY /NuGet.Config /root/.nuget/NuGet/NuGet.Config

RUN dotnet restore ./HousingSearchApi.Tests/HousingSearchApi.Tests.csproj

# Copy everything else and build
COPY . .

RUN dotnet build -c Release -o out HousingSearchApi/HousingSearchApi.csproj
RUN dotnet build -c debug -o out HousingSearchApi.Tests/HousingSearchApi.Tests.csproj

CMD dotnet test --filter HousingSearchApi.Tests.V1
