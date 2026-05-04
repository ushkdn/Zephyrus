FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["Zephyrus.Logger/Zephyrus.Logger.csproj", "Zephyrus.Logger/"]
COPY ["Zephyrus.SharedKernel/Zephyrus.SharedKernel.csproj", "Zephyrus.SharedKernel/"]
COPY ["Zephyrus.Procurement/Zephyrus.Procurement.Domain/Zephyrus.Procurement.Domain.csproj", "Zephyrus.Procurement/Zephyrus.Procurement.Domain/"]
COPY ["Zephyrus.Procurement/Zephyrus.Procurement.Application/Zephyrus.Procurement.Application.csproj", "Zephyrus.Procurement/Zephyrus.Procurement.Application/"]
COPY ["Zephyrus.Procurement/Zephyrus.Procurement.Infrastructure/Zephyrus.Procurement.Infrastructure.csproj", "Zephyrus.Procurement/Zephyrus.Procurement.Infrastructure/"]
COPY ["Zephyrus.Procurement/Zephyrus.Procurement.Presentation/Zephyrus.Procurement.Presentation.csproj", "Zephyrus.Procurement/Zephyrus.Procurement.Presentation/"]
COPY ["Zephyrus.Procurement/Zephyrus.Procurement.WebApi/Zephyrus.Procurement.WebApi.csproj", "Zephyrus.Procurement/Zephyrus.Procurement.WebApi/"]

RUN dotnet restore "Zephyrus.Procurement/Zephyrus.Procurement.WebApi/Zephyrus.Procurement.WebApi.csproj"

COPY . .

WORKDIR "/src/Zephyrus.Procurement/Zephyrus.Procurement.WebApi"

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Zephyrus.Procurement.WebApi.csproj" -c $BUILD_CONFIGURATION --no-restore -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Zephyrus.Procurement.WebApi.dll"]
