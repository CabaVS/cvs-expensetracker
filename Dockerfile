FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR "/src"
COPY ["Directory.Packages.props", "."]
COPY ["Directory.Build.props", "."]
COPY ["src/CabaVS.ExpenseTracker.API/CabaVS.ExpenseTracker.API.csproj", "src/CabaVS.ExpenseTracker.API/"]
RUN dotnet restore "./src/CabaVS.ExpenseTracker.API/CabaVS.ExpenseTracker.API.csproj"
COPY . .
WORKDIR "/src/src/CabaVS.ExpenseTracker.API"
RUN dotnet build "./CabaVS.ExpenseTracker.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./CabaVS.ExpenseTracker.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish

FROM base AS final
WORKDIR "/app"
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CabaVS.ExpenseTracker.API.dll"]