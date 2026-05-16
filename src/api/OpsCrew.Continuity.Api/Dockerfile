FROM mcr.microsoft.com/dotnet/sdk:10.0-alpine AS build
WORKDIR /repo

COPY src/api/OpsCrew.Continuity.Api/OpsCrew.Continuity.Api.csproj src/api/OpsCrew.Continuity.Api/
COPY src/api/OpsCrew.Continuity.Core/OpsCrew.Continuity.Core.csproj src/api/OpsCrew.Continuity.Core/
COPY src/api/OpsCrew.Continuity.Infrastructure/OpsCrew.Continuity.Infrastructure.csproj src/api/OpsCrew.Continuity.Infrastructure/
COPY src/api/OpsCrew.Continuity.Contracts/OpsCrew.Continuity.Contracts.csproj src/api/OpsCrew.Continuity.Contracts/
RUN dotnet restore src/api/OpsCrew.Continuity.Api/OpsCrew.Continuity.Api.csproj

COPY src/api src/api
RUN dotnet publish src/api/OpsCrew.Continuity.Api/OpsCrew.Continuity.Api.csproj \
    --configuration Release \
    --output /app/publish \
    --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0-alpine AS runtime
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "OpsCrew.Continuity.Api.dll"]
