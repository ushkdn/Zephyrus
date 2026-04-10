FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["Zephyrus.Logger/Zephyrus.Logger.csproj", "Zephyrus.Logger/"]
COPY ["Zephyrus.SharedKernel/Zephyrus.SharedKernel.csproj", "Zephyrus.SharedKernel/"]
COPY ["Zephyrus.Supplier/Zephyrus.Supplier.Domain/Zephyrus.Supplier.Domain.csproj", "Zephyrus.Supplier/Zephyrus.Supplier.Domain/"]
COPY ["Zephyrus.Supplier/Zephyrus.Supplier.Application/Zephyrus.Supplier.Application.csproj", "Zephyrus.Supplier/Zephyrus.Supplier.Application/"]
COPY ["Zephyrus.Supplier/Zephyrus.Supplier.Infrastructure/Zephyrus.Supplier.Infrastructure.csproj", "Zephyrus.Supplier/Zephyrus.Supplier.Infrastructure/"]
COPY ["Zephyrus.Supplier/Zephyrus.Supplier.Presentation/Zephyrus.Supplier.Presentation.csproj", "Zephyrus.Supplier/Zephyrus.Supplier.Presentation/"]
COPY ["Zephyrus.Supplier/Zephyrus.Supplier.WebApi/Zephyrus.Supplier.WebApi.csproj", "Zephyrus.Supplier/Zephyrus.Supplier.WebApi/"]

RUN dotnet restore "Zephyrus.Supplier/Zephyrus.Supplier.WebApi/Zephyrus.Supplier.WebApi.csproj"

COPY . .

WORKDIR "/src/Zephyrus.Supplier/Zephyrus.Supplier.WebApi"
RUN dotnet build "Zephyrus.Supplier.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Zephyrus.Supplier.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Zephyrus.Supplier.WebApi.dll"]
