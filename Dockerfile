FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["INTEGRACION_SANJUAN.csproj", "./"]
RUN dotnet restore "./INTEGRACION_SANJUAN.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "INTEGRACION_SANJUAN.csproj" -c Release -o /app/build
RUN dotnet publish "INTEGRACION_SANJUAN.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "INTEGRACION_SANJUAN.dll"]
