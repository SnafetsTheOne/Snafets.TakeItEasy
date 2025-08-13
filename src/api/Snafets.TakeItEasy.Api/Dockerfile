# Use the official .NET 9 SDK image for build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy csproj and restore as distinct layers
COPY Snafets.TakeItEasy.Api/Snafets.TakeItEasy.Api.csproj Snafets.TakeItEasy.Api/
COPY Snafets.TakeItEasy.Persistence/Snafets.TakeItEasy.Persistence.csproj Snafets.TakeItEasy.Persistence/
COPY Snafets.TakeItEasy.Application/Snafets.TakeItEasy.Application.csproj Snafets.TakeItEasy.Application/
COPY Snafets.TakeItEasy.Domain/Snafets.TakeItEasy.Domain.csproj Snafets.TakeItEasy.Domain/
RUN dotnet restore Snafets.TakeItEasy.Api/Snafets.TakeItEasy.Api.csproj

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o /app/publish Snafets.TakeItEasy.Api/Snafets.TakeItEasy.Api.csproj

# Use the official ASP.NET Core runtime image for final stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Expose port 80
EXPOSE 8080

# Start the web service
ENTRYPOINT ["dotnet", "Snafets.TakeItEasy.Api.dll"]