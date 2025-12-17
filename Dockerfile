FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY AuthOtpSample.Api/AuthOtpSample.Api.csproj AuthOtpSample.Api/
COPY AuthOtpSample.Application/AuthOtpSample.Application.csproj AuthOtpSample.Application/
COPY AuthOtpSample.Domain/AuthOtpSample.Domain.csproj AuthOtpSample.Domain/
COPY AuthOtpSample.Infrastructure/AuthOtpSample.Infrastructure.csproj AuthOtpSample.Infrastructure/

RUN dotnet restore AuthOtpSample.Api/AuthOtpSample.Api.csproj

COPY . .

WORKDIR /src/AuthOtpSample.Api
RUN dotnet publish -c Release -o /app/publish --no-restore /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "AuthOtpSample.Api.dll"]