# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build

LABEL description="Skonteneryzowana \
odsłona najlepszej aplikacji ksiegarnianej pod sloncem."

WORKDIR /source

# copy csproj and restore as distinct layers
COPY BookStore/*.csproj BookStore/
COPY BookStore.BusinessLayer/*.csproj BookStore.BusinessLayer/
COPY BookStore.DataLayer/*.csproj BookStore.DataLayer/
COPY BookStore.Tests/*.csproj BookStore.Tests/
RUN dotnet restore BookStore/BookStore.csproj

# copy and build app and libraries
COPY BookStore/ BookStore/
COPY BookStore.BusinessLayer/ BookStore.BusinessLayer/
COPY BookStore.DataLayer/ BookStore.DataLayer/ 
COPY BookStore.Tests/ BookStore.Tests/
WORKDIR /source/BookStore
RUN dotnet build -c release --no-restore

# test stage -- exposes optional entrypoint
# target entrypoint with: docker build --target test
FROM build AS test 
WORKDIR /source/BookStore.Tests
COPY BookStore.Tests/ .
ENTRYPOINT ["dotnet", "test", "--logger:trx"]

# kolejne FROM - "multi-staged build"; kazdy zaczyna od alpine:latest + artefaktow z argumentu build
FROM build AS publish
RUN dotnet publish -c release --no-build -o /app

# final stage/image
FROM mcr.microsoft.com/dotnet/runtime:3.1
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "BookStore.dll"]
