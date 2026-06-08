FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["UserService/UserService.API/UserService.API.csproj", "UserService/UserService.API/"]
COPY ["UserService/UserService.Application/UserService.Application.csproj", "UserService/UserService.Application/"]
COPY ["UserService/UserService.Domain/UserService.Domain.csproj", "UserService/UserService.Domain/"]
COPY ["UserService/UserService.Infrastructure/UserService.Infrastructure.csproj", "UserService/UserService.Infrastructure/"]
COPY ["Shared/Shared.csproj", "Shared/"]

RUN ["dotnet", "restore", "UserService/UserService.API/UserService.API.csproj"]

COPY ["UserService/", "UserService/"]
COPY ["Shared/", "Shared/"]

WORKDIR /src/UserService/UserService.API

RUN ["dotnet", "publish", "UserService.API.csproj", "-c", "Release", "--no-restore", "-o", "/app/publish"]

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

COPY --from=build /app/publish .
EXPOSE 8080

ENTRYPOINT ["dotnet", "UserService.API.dll"]