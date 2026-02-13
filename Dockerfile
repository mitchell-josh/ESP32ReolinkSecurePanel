FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ReolinkAPI/ ReolinkAPI/
COPY SecurePanelAPI/ SecurePanelAPI/
COPY SecurePanelDb/ SecurePanelDb/
COPY SecurePanelModels/ SecurePanelModels/

RUN rm -rf "SecurePanelDb/bin\Debug"
RUN rm -rf "SecurePanelDb/obj\Debug"
RUN rm -rf "SecurePanelAPI/bin\Debug"
RUN rm -rf "SecurePanelAPI/obj\Debug"

RUN find . -type d \( -name "obj" -o -name "bin" \) -exec rm -rf {} +

RUN dotnet restore "SecurePanelAPI/SecurePanelAPI.csproj"

RUN dotnet build "SecurePanelAPI/SecurePanelAPI.csproj" -c Release

RUN dotnet publish "SecurePanelAPI/SecurePanelAPI.csproj" -c Release -o /app/publish --no-build

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "SecurePanelAPI.dll"]