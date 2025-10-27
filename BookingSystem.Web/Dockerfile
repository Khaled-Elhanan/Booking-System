# ===== BUILD STAGE =====
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy everything
COPY . .

# Restore dependencies
RUN dotnet restore "BookingSystem.csproj" --ignore-failed-sources

# Publish the app
RUN dotnet publish "BookingSystem.csproj" -c Release -o /app

# ===== RUNTIME STAGE =====
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copy published files from build stage
COPY --from=build /app .

# Expose port 8080 for Render
EXPOSE 8080

# Start the app
ENTRYPOINT ["dotnet", "BookingSystem.dll"]
