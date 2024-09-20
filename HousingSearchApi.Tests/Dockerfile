FROM mcr.microsoft.com/dotnet/sdk:6.0

# disable microsoft telematry
ENV DOTNET_CLI_TELEMETRY_OPTOUT='true'

ARG LBHPACKAGESTOKEN
ENV LBHPACKAGESTOKEN=$LBHPACKAGESTOKEN

# ARG SONAR_TOKEN
# ENV SONAR_TOKEN=$SONAR_TOKEN

WORKDIR /app

# RUN apt-get update && apt-get install -y openjdk-17-jdk
# RUN dotnet tool install --global dotnet-sonarscanner
# ENV PATH="$PATH:/root/.dotnet/tools"

# RUN dotnet sonarscanner begin /k:"LBHackney-IT_housing-search-api" /o:"lbhackney-it" /d:sonar.host.url=https://sonarcloud.io /d:sonar.login="${SONAR_TOKEN}"

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
# RUN dotnet sonarscanner end /d:sonar.login="${SONAR_TOKEN}"
