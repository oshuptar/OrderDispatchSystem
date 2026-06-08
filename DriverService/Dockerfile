FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["DriverService/DriverService.API/DriverService.API.csproj", "DriverService/DriverService.API/"]
COPY ["DriverService/DriverService.Application/DriverService.Application.csproj", "DriverService/DriverService.Application/"]
COPY ["DriverService/DriverService.Domain/DriverService.Domain.csproj", "DriverService/DriverService.Domain/"]
COPY ["DriverService/DriverService.Infrastructure/DriverService.Infrastructure.csproj", "DriverService/DriverService.Infrastructure/"]
COPY ["Shared/Shared.csproj", "Shared/"]

RUN ["dotnet", "restore", "DriverService/DriverService.API/DriverService.API.csproj"]

COPY ./DriverService ./DriverService
COPY ./Shared ./Shared

WORKDIR /src/DriverService/DriverService.API
RUN ["dotnet", "publish", "DriverService.API.csproj", "-c", "Release", "-o", "/app/publish", "--no-restore"]

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "DriverService.API.dll"]
