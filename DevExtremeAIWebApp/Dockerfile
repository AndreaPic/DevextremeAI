#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["DevExtremeAIWebApp/DevExtremeAIWebApp.csproj", "DevExtremeAIWebApp/"]
RUN dotnet restore "DevExtremeAIWebApp/DevExtremeAIWebApp.csproj"
COPY . .
WORKDIR "/src/DevExtremeAIWebApp"
RUN dotnet build "DevExtremeAIWebApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DevExtremeAIWebApp.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DevExtremeAIWebApp.dll"]