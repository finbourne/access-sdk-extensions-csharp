FROM mcr.microsoft.com/dotnet/core/sdk:3.1

RUN mkdir -p /usr/src
WORKDIR /usr/src

COPY Finbourne.Access.Sdk.Extensions.sln /usr/src/
COPY Finbourne.Access.Sdk.Extensions/Finbourne.Access.Sdk.Extensions.csproj /usr/src/Finbourne.Access.Sdk.Extensions/
COPY Finbourne.Access.Sdk.Extensions.Tests/Finbourne.Access.Sdk.Extensions.Tests.csproj /usr/src/Finbourne.Access.Sdk.Extensions.Tests/
COPY Finbourne.Access.Sdk.Extensions.IntegrationTests/Finbourne.Access.Sdk.Extensions.IntegrationTests.csproj /usr/src/Finbourne.Access.Sdk.Extensions.IntegrationTests/
RUN dotnet restore Finbourne.Access.Sdk.Extensions

ENTRYPOINT dotnet test --configuration Release --verbosity normal