# ---------- Build Stage ----------
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

WORKDIR /src

COPY . .

RUN dotnet publish "./src/CabaVS.ExpenseTracker.API/CabaVS.ExpenseTracker.API.csproj" -c Release -o /publish

# ---------- Runtime Stage ----------
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime

WORKDIR /app

COPY --from=build /publish .

EXPOSE 443

ENTRYPOINT ["dotnet", "CabaVS.ExpenseTracker.API.dll"]
