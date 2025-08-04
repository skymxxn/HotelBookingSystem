FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/HotelBookingSystem.API/HotelBookingSystem.API.csproj", "src/HotelBookingSystem.API/"]
COPY ["src/HotelBookingSystem.Application/HotelBookingSystem.Application.csproj", "src/HotelBookingSystem.Application/"]
COPY ["src/HotelBookingSystem.Domain/HotelBookingSystem.Domain.csproj", "src/HotelBookingSystem.Domain/"]
COPY ["src/HotelBookingSystem.Infrastructure/HotelBookingSystem.Infrastructure.csproj", "src/HotelBookingSystem.Infrastructure/"]
COPY ["src/HotelBookingSystem.Persistence/HotelBookingSystem.Persistence.csproj", "src/HotelBookingSystem.Persistence/"]
RUN dotnet restore "src/HotelBookingSystem.API/HotelBookingSystem.API.csproj"
COPY . .
WORKDIR "/src/src/HotelBookingSystem.API"
RUN dotnet build "./HotelBookingSystem.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./HotelBookingSystem.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HotelBookingSystem.API.dll"]
