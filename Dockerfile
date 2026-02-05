FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["ReolinkAPI/ReolinkAPI.csproj", "ReolinkAPI/"]
COPY ["SecurePanelAPI/SecurePanelAPI.csproj", "SecurePanelAPI/"]
COPY ["SecurePanelDb/SecurePanelDb.csproj", "SecurePanelDb/"]
COPY ["SecurePanelModels/SecurePanelModels.csproj", "SecurePanelModels/"]

RUN dotnet restore "SecurePanelAPI/SecurePanelAPI.csproj"

COPY . .
WORKDIR "/src/SecurePanelAPI"
RUN dotnet build "SecurePanelAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SecurePanelAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "SecurePanelAPI.dll"]