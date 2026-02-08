# Use the ASP.NET Core runtime image
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER app
WORKDIR /app
EXPOSE 8080

# Use the SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["SendGridMock/SendGridMock.csproj", "SendGridMock/"]
RUN dotnet restore "./SendGridMock/SendGridMock.csproj"
COPY . .
WORKDIR "/src/SendGridMock"
RUN dotnet build "./SendGridMock.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./SendGridMock.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SendGridMock.dll"]
