FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["Zephyrus.Logger/Zephyrus.Logger.csproj", "Zephyrus.Logger/"]
COPY ["Zephyrus.SharedKernel/Zephyrus.SharedKernel.csproj", "Zephyrus.SharedKernel/"]
COPY ["Zephyrus.Catalog/Zephyrus.Catalog.Domain/Zephyrus.Catalog.Domain.csproj", "Zephyrus.Catalog/Zephyrus.Catalog.Domain/"]
COPY ["Zephyrus.Catalog/Zephyrus.Catalog.Application/Zephyrus.Catalog.Application.csproj", "Zephyrus.Catalog/Zephyrus.Catalog.Application/"]
COPY ["Zephyrus.Catalog/Zephyrus.Catalog.Infrastructure/Zephyrus.Catalog.Infrastructure.csproj", "Zephyrus.Catalog/Zephyrus.Catalog.Infrastructure/"]
COPY ["Zephyrus.Catalog/Zephyrus.Catalog.Presentation/Zephyrus.Catalog.Presentation.csproj", "Zephyrus.Catalog/Zephyrus.Catalog.Presentation/"]
COPY ["Zephyrus.Catalog/Zephyrus.Catalog.WebApi/Zephyrus.Catalog.WebApi.csproj", "Zephyrus.Catalog/Zephyrus.Catalog.WebApi/"]

RUN dotnet restore "Zephyrus.Catalog/Zephyrus.Catalog.WebApi/Zephyrus.Catalog.WebApi.csproj"

COPY . .

WORKDIR "/src/Zephyrus.Catalog/Zephyrus.Catalog.WebApi"

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Zephyrus.Catalog.WebApi.csproj" -c $BUILD_CONFIGURATION --no-restore -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Zephyrus.Catalog.WebApi.dll"]
