FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 8003

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["Granite_House/Granite_House.csproj", "Granite_House/"]
RUN dotnet restore "Granite_House/Granite_House.csproj"
COPY . .
WORKDIR "/src/Granite_House/"
RUN dotnet build "Granite_House.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Granite_House.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Granite_House.dll"]