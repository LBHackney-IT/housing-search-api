FROM mcr.microsoft.com/dotnet/core/sdk:3.1

ARG LBHPACKAGESTOKEN
ENV LBHPACKAGESTOKEN=%LBHPACKAGESTOKEN%

WORKDIR /app

# Copy csproj and nuget config and restore as distinct layers
COPY ./HousingSearchApi.sln ./
COPY ./HousingSearchApi/HousingSearchApi.csproj ./HousingSearchApi/
COPY /NuGet.Config /root/.nuget/NuGet/NuGet.Config

RUN dotnet restore ./HousingSearchApi/HousingSearchApi.csproj --configfile /root/.nuget/NuGet/NuGet.Config
# Copy everything else and build
COPY . ./

EXPOSE ${PORT:-3000}
