FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["Bootstrapper/Api/Api.csproj", "Bootstrapper/Api/"]
COPY ["Modules/Menu/Menu/Menu.csproj", "Modules/Menu/Menu/"]
COPY ["Modules/Menu/Menu.Contracts/Menu.Contracts.csproj", "Modules/Menu/Menu.Contracts/"]
COPY ["Modules/Order/Order/Order.csproj", "Modules/Order/Order/"]
COPY ["Modules/Order/Order.Contracts/Order.Contracts.csproj", "Modules/Order/Order.Contracts/"]
COPY ["Modules/Tenant/Tenant/Tenant.csproj", "Modules/Tenant/Tenant/"]
COPY ["Modules/Tenant/Tenant.Contracts/Tenant.Contracts.csproj", "Modules/Tenant/Tenant.Contracts/"]
COPY ["Modules/Identity/Identity/Identity.csproj", "Modules/Identity/Identity/"]
COPY ["Modules/Identity/Identity.Contracts/Identity.Contracts.csproj", "Modules/Identity/Identity.Contracts/"]
COPY ["Shared/Shared/Shared.csproj", "Shared/Shared/"]
COPY ["Shared/Shared.Contracts/Shared.Contracts.csproj", "Shared/Shared.Contracts/"]
COPY ["Shared/Shared.Messaging/Shared.Messaging.csproj", "Shared/Shared.Messaging/"]

RUN dotnet restore "Bootstrapper/Api/Api.csproj"

COPY . .
WORKDIR "/src/Bootstrapper/Api"
RUN dotnet build "Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Api.dll"]
