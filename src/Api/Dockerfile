#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["src/Api/PublicApi.csproj", "src/Api/"]
COPY ["src/Api/PublicApi.xml", "src/Api/"]
COPY ["src/ApplicationCore/Application.csproj", "src/ApplicationCore/"]
COPY ["src/Infraestructure/Infraestructure.csproj", "src/Infraestructure/"]
COPY ["src/Domain/Domain.csproj", "src/Domain/"]
RUN dotnet restore "src/Api/PublicApi.csproj"
COPY . .
WORKDIR "/src/src/Api"
RUN dotnet build "PublicApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PublicApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PublicApi.dll"]