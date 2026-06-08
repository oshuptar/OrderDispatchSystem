FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["OrderService/OrderService.API/OrderService.API.csproj", "OrderService/OrderService.API/"]
COPY ["OrderService/OrderService.Application/OrderService.Application.csproj", "OrderService/OrderService.Application/"]
COPY ["OrderService/OrderService.Domain/OrderService.Domain.csproj", "OrderService/OrderService.Domain/"]
COPY ["OrderService/OrderService.Infrastructure/OrderService.Infrastructure.csproj", "OrderService/OrderService.Infrastructure/"]
COPY ["Shared/Shared.csproj", "Shared/"]

RUN ["dotnet", "restore", "OrderService/OrderService.API/OrderService.API.csproj"]

COPY ["OrderService/", "OrderService/"]
COPY ["Shared/", "Shared/"]

WORKDIR /src/OrderService/OrderService.API

RUN ["dotnet", "publish", "OrderService.API.csproj" ,"-c", "Release", "--no-restore", "-o", "/app/publish"]

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

COPY --from=build /app/publish ./
EXPOSE 8080

ENTRYPOINT ["dotnet", "OrderService.API.dll"]