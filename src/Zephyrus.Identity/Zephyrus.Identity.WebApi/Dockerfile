FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["Zephyrus.Logger/Zephyrus.Logger.csproj", "Zephyrus.Logger/"]
COPY ["Zephyrus.SharedKernel/Zephyrus.SharedKernel.csproj", "Zephyrus.SharedKernel/"]
COPY ["Zephyrus.Identity/Zephyrus.Identity.Domain/Zephyrus.Identity.Domain.csproj", "Zephyrus.Identity/Zephyrus.Identity.Domain/"]
COPY ["Zephyrus.Identity/Zephyrus.Identity.Application/Zephyrus.Identity.Application.csproj", "Zephyrus.Identity/Zephyrus.Identity.Application/"]
COPY ["Zephyrus.Identity/Zephyrus.Identity.Infrastructure/Zephyrus.Identity.Infrastructure.csproj", "Zephyrus.Identity/Zephyrus.Identity.Infrastructure/"]
COPY ["Zephyrus.Identity/Zephyrus.Identity.Presentation/Zephyrus.Identity.Presentation.csproj", "Zephyrus.Identity/Zephyrus.Identity.Presentation/"]
COPY ["Zephyrus.Identity/Zephyrus.Identity.WebApi/Zephyrus.Identity.WebApi.csproj", "Zephyrus.Identity/Zephyrus.Identity.WebApi/"]

RUN dotnet restore "Zephyrus.Identity/Zephyrus.Identity.WebApi/Zephyrus.Identity.WebApi.csproj"

COPY . .

WORKDIR "/src/Zephyrus.Identity/Zephyrus.Identity.WebApi"

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Zephyrus.Identity.WebApi.csproj" -c $BUILD_CONFIGURATION --no-restore -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Zephyrus.Identity.WebApi.dll"]
