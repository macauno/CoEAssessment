# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["TA-API.csproj", "./"]
RUN dotnet restore "TA-API.csproj"

COPY . .
RUN dotnet build "TA-API.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "TA-API.csproj" -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
EXPOSE 5148
ENV ASPNETCORE_URLS=http://+:5148
ENV ASPNETCORE_ENVIRONMENT=Production

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TA-API.dll"]
