FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Tiny.Worker.DomainGenerator/Tiny.Worker.DomainGenerator.csproj", "Tiny.Worker.DomainGenerator/"]
RUN dotnet restore "Tiny.Worker.DomainGenerator/Tiny.Worker.DomainGenerator.csproj"
COPY . .
WORKDIR "/src/Tiny.Worker.DomainGenerator"
RUN dotnet build "Tiny.Worker.DomainGenerator.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Tiny.Worker.DomainGenerator.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Tiny.Worker.DomainGenerator.dll"]
