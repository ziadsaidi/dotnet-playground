# Use the official ASP.NET Core runtime as the base image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 5001

# Set environment variable to listen on port 5001
ENV ASPNETCORE_URLS=http://+:5001

# Use the official .NET SDK image as the build image
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG configuration=Release
WORKDIR /src

# Copy the csproj and restore the dependencies
COPY ["Sales.Api/Sales.Api.csproj", "Sales.Api/"]
RUN dotnet restore "Sales.Api/Sales.Api.csproj"

# Copy the entire source code
COPY . ./
WORKDIR "/src/Sales.Api"

# Build the application
RUN dotnet build "Sales.Api.csproj" -c $configuration -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "Sales.Api.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

# Use the base image for the final stage
FROM base AS final
WORKDIR /app

# Copy the published files from the publish stage
COPY --from=publish /app/publish .

# Set the entry point to run the application
ENTRYPOINT ["dotnet", "Sales.Api.dll"]
