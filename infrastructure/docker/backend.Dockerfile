FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["Backend/Backend.csproj", "./"]
RUN dotnet restore

COPY ["Backend/", "./"]
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

COPY ./Backend/Keys /app/Keys

RUN apt-get update \
 && apt-get install -y --no-install-recommends postgresql-client \
 && rm -rf /var/lib/apt/lists/*

ENV ASPNETCORE_URLS=http://+:5000

ENTRYPOINT ["sh", "-c", "\
  echo \"Waiting for Postgres at postgres:5432...\" && \
  until pg_isready -h postgres -p 5432 -U postgres; do sleep 1; done && \
  echo \"Postgres is up — starting Backend.dll\" && \
  exec dotnet Backend.dll"]
