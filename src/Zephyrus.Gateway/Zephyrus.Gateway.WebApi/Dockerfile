FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["Zephyrus.Logger/Zephyrus.Logger.csproj", "Zephyrus.Logger/"]
COPY ["Zephyrus.SharedKernel/Zephyrus.SharedKernel.csproj", "Zephyrus.SharedKernel/"]
COPY ["Zephyrus.Gateway/Zephyrus.Gateway.WebApi/Zephyrus.Gateway.WebApi.csproj", "Zephyrus.Gateway/Zephyrus.Gateway.WebApi/"]

RUN dotnet restore "Zephyrus.Gateway/Zephyrus.Gateway.WebApi/Zephyrus.Gateway.WebApi.csproj"

COPY . .

WORKDIR "/src/Zephyrus.Gateway/Zephyrus.Gateway.WebApi"

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Zephyrus.Gateway.WebApi.csproj" -c $BUILD_CONFIGURATION --no-restore -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Zephyrus.Gateway.WebApi.dll"]
