FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["DeviceMonitoring.API/DeviceMonitoring.API.csproj", "DeviceMonitoring.API/"]
COPY ["DeviceMonitoring.Data/DeviceMonitoring.Data.csproj", "DeviceMonitoring.Data/"]
COPY ["DeviceMonitoring.Domain/DeviceMonitoring.Domain.csproj", "DeviceMonitoring.Domain/"]
COPY ["DeviceMonitoring.Services/DeviceMonitoring.Services.csproj", "DeviceMonitoring.Services/"]

RUN dotnet restore "DeviceMonitoring.API/DeviceMonitoring.API.csproj"

COPY . .

WORKDIR "/src/DeviceMonitoring.API"

RUN dotnet publish "DeviceMonitoring.API.csproj" \
    --configuration Release \
    --output /app/publish \
    /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

EXPOSE 8080

ENV ASPNETCORE_URLS=http://+:8080

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "DeviceMonitoring.API.dll"]