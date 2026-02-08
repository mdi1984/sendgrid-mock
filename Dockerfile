# Use the SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:10.0-alpine3.22-aot AS publish
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

RUN apk add clang binutils musl-dev build-base zlib-static

COPY ["SendGridMock/SendGridMock.csproj", "SendGridMock/"]
RUN dotnet restore --runtime linux-musl-x64 "./SendGridMock/SendGridMock.csproj"
COPY . .
WORKDIR "/src/SendGridMock"
RUN dotnet publish -r linux-musl-x64 -c $BUILD_CONFIGURATION -o /app/publish "./SendGridMock.csproj"

FROM scratch AS final
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:8080
ENV DOTNET_SYSTEM_NET_SECURITY_OPENSSL_VERSION=""
COPY --from=publish /app/publish .
ENTRYPOINT ["/app/SendGridMock"]
