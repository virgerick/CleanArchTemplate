#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Server/CleanArchTemplate.Server.csproj", "src/Server/"]
COPY ["src/Client/CleanArchTemplate.Client.csproj", "src/Client/"]
COPY ["src/Shared/CleanArchTemplate.Shared.csproj", "src/Shared/"]
COPY ["src/Core/CleanArchTemplate.Application/CleanArchTemplate.Application.csproj", "src/Core/CleanArchTemplate.Application/"]
COPY ["src/Core/CleanArchTemplate.Domain/CleanArchTemplate.Domain.csproj", "src/Core/CleanArchTemplate.Domain/"]
COPY ["src/CleanArchTemplate.Abstraction/CleanArchTemplate.Abstraction.csproj", "src/CleanArchTemplate.Abstraction/"]
COPY ["src/CleanArchTemplate.Infrastructure/CleanArchTemplate.Infrastructure.csproj", "src/CleanArchTemplate.Infrastructure/"]
RUN dotnet restore "src/Server/CleanArchTemplate.Server.csproj"
COPY . .
WORKDIR "/src/src/Server"
RUN dotnet build "CleanArchTemplate.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CleanArchTemplate.Server.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CleanArchTemplate.Server.dll"]