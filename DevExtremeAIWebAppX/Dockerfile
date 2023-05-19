#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["DevextremeAIWebApp/DevextremeAIWebApp.csproj", "DevextremeAIWebApp/"]
RUN dotnet restore "DevextremeAIWebApp/DevextremeAIWebApp.csproj"
COPY . .
WORKDIR "/src/DevextremeAIWebApp"
RUN dotnet build "DevextremeAIWebApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DevextremeAIWebApp.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DevextremeAIWebApp.dll"]