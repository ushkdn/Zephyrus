FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["Zephyrus.Logger/Zephyrus.Logger.csproj", "Zephyrus.Logger/"]
COPY ["Zephyrus.SharedKernel/Zephyrus.SharedKernel.csproj", "Zephyrus.SharedKernel/"]
COPY ["Zephyrus.Notification/Zephyrus.Notification.Domain/Zephyrus.Notification.Domain.csproj", "Zephyrus.Notification/Zephyrus.Notification.Domain/"]
COPY ["Zephyrus.Notification/Zephyrus.Notification.Application/Zephyrus.Notification.Application.csproj", "Zephyrus.Notification/Zephyrus.Notification.Application/"]
COPY ["Zephyrus.Notification/Zephyrus.Notification.Infrastructure/Zephyrus.Notification.Infrastructure.csproj", "Zephyrus.Notification/Zephyrus.Notification.Infrastructure/"]
COPY ["Zephyrus.Notification/Zephyrus.Notification.Presentation/Zephyrus.Notification.Presentation.csproj", "Zephyrus.Notification/Zephyrus.Notification.Presentation/"]
COPY ["Zephyrus.Notification/Zephyrus.Notification.WebApi/Zephyrus.Notification.WebApi.csproj", "Zephyrus.Notification/Zephyrus.Notification.WebApi/"]

RUN dotnet restore "Zephyrus.Notification/Zephyrus.Notification.WebApi/Zephyrus.Notification.WebApi.csproj"

COPY . .

WORKDIR "/src/Zephyrus.Notification/Zephyrus.Notification.WebApi"

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Zephyrus.Notification.WebApi.csproj" -c $BUILD_CONFIGURATION --no-restore -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Zephyrus.Notification.WebApi.dll"]
