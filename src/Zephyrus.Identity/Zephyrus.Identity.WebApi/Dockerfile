FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Zephyrus.Identity.WebApi/Zephyrus.Identity.WebApi.csproj", "Zephyrus.Identity.WebApi/"]
RUN dotnet restore "Zephyrus.Identity.WebApi/Zephyrus.Identity.WebApi.csproj"
COPY . .
WORKDIR "/src/Zephyrus.Identity.WebApi"
RUN dotnet build "./Zephyrus.Identity.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Zephyrus.Identity.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Zephyrus.Identity.WebApi.dll"]
