# Estructura básica para tu API de Amazon
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
# OJO: Aquí asumo que tu proyecto se llama AmazonAPI.csproj. 
# Si tiene otro nombre, cámbialo abajo.
COPY ["AmazonAPI.csproj", "."]
RUN dotnet restore "./AmazonAPI.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "AmazonAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AmazonAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AmazonAPI.dll"]
