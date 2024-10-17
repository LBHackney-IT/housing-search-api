FROM mcr.microsoft.com/dotnet/sdk:6.0

# disable microsoft telematry
ENV DOTNET_CLI_TELEMETRY_OPTOUT='true'

ARG LBHPACKAGESTOKEN
ENV LBHPACKAGESTOKEN=$LBHPACKAGESTOKEN

ARG ELASTICSEARCH_DOMAIN_URL
ENV ELASTICSEARCH_DOMAIN_URL=$ELASTICSEARCH_DOMAIN_URL

ARG SONAR_TOKEN
ENV SONAR_TOKEN=$SONAR_TOKEN

WORKDIR /app


RUN if [ -n "$SONAR_TOKEN" ]; then \
    apt-get update && apt-get install -y openjdk-17-jdk; \
    dotnet tool install --global dotnet-sonarscanner; \
    export PATH="$PATH:/root/.dotnet/tools"; \
    dotnet sonarscanner begin /k:"LBHackney-IT_housing-search-api" /o:"lbhackney-it" /d:sonar.host.url=https://sonarcloud.io /d:sonar.login="${SONAR_TOKEN}"; \
    fi

# Copy csproj and nuget config and restore as distinct layers
COPY ./HousingSearchApi.sln ./
COPY ./HousingSearchApi.Tests/HousingSearchApi.Tests.csproj ./HousingSearchApi.Tests/
COPY /NuGet.Config /root/.nuget/NuGet/NuGet.Config

RUN dotnet restore ./HousingSearchApi.Tests/HousingSearchApi.Tests.csproj

# Copy everything else and build
COPY . .

RUN dotnet build -c Release -o out HousingSearchApi/HousingSearchApi.csproj
RUN dotnet build -c debug -o out HousingSearchApi.Tests/HousingSearchApi.Tests.csproj

RUN if [ -n "$SONAR_TOKEN" ]; then \
    dotnet sonarscanner end /d:sonar.login="${SONAR_TOKEN}"; \
    fi
