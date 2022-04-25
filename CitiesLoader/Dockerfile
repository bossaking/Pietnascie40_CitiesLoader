FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["CitiesLoader/CitiesLoader.csproj", "CitiesLoader/"]
RUN dotnet restore "CitiesLoader/CitiesLoader.csproj"
COPY . .
WORKDIR "/src/CitiesLoader"
RUN dotnet build "CitiesLoader.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CitiesLoader.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CitiesLoader.dll"]
