FROM mcr.microsoft.com/dotnet/core/sdk:3.1

# disable microsoft telematry
ENV DOTNET_CLI_TELEMETRY_OPTOUT='true'

WORKDIR /app

# Copy csproj and restore as distinct layers
COPY ./SearchApi.sln ./
COPY ./SearchApi/SearchApi.csproj ./SearchApi/
COPY ./SearchApi.Tests/SearchApi.Tests.csproj ./SearchApi.Tests/

RUN dotnet restore ./SearchApi/SearchApi.csproj
RUN dotnet restore ./SearchApi.Tests/SearchApi.Tests.csproj

# Copy everything else and build
COPY . .

RUN dotnet build -c debug -o out SearchApi.Tests/SearchApi.Tests.csproj

CMD dotnet test
