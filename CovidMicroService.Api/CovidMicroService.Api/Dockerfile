
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-nanoserver-1903 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-nanoserver-1903 AS build
WORKDIR /src
COPY ["CovidMicroService.Api/CovidMicroService.Api.csproj", "CovidMicroService.Api/"]
RUN dotnet restore "CovidMicroService.Api/CovidMicroService.Api.csproj"
COPY . .
WORKDIR "/src/CovidMicroService.Api"
RUN dotnet build "CovidMicroService.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CovidMicroService.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CovidMicroService.Api.dll"]