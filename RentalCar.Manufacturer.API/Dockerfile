FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["RentalCar.Manufacturer.Core/RentalCar.Manufacturer.Core.csproj", "RentalCar.Manufacturer.Core/"]
COPY ["RentalCar.Manufacturer.Infrastructure/RentalCar.Manufacturer.Infrastructure.csproj", "RentalCar.Manufacturer.Infrastructure/"]
COPY ["RentalCar.Manufacturer.Application/RentalCar.Manufacturer.Application.csproj", "RentalCar.Manufacturer.Application/"]
COPY ["RentalCar.Manufacturer.API/RentalCar.Manufacturer.API.csproj", "RentalCar.Manufacturer.API/"]

RUN dotnet restore "RentalCar.Manufacturer.API/RentalCar.Manufacturer.API.csproj"
COPY . .
WORKDIR "/src/RentalCar.Manufacturer.API"
RUN dotnet build "RentalCar.Manufacturer.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "RentalCar.Manufacturer.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RentalCar.Manufacturer.API.dll"]
