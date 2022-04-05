FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["FlowerShop.Cli/FlowerShop.Cli.csproj", "FlowerShop.Cli/"]
COPY ["FlowerShop.Application/FlowerShop.Application.csproj", "FlowerShop.Application/"]
COPY ["FlowerShop.Domain/FlowerShop.Domain.csproj", "FlowerShop.Domain/"]   
COPY ["FlowerShop.Infrastructure/FlowerShop.Infrastructure.csproj", "FlowerShop.Infrastructure/"]
RUN dotnet restore "FlowerShop.Cli/FlowerShop.Cli.csproj"
COPY . .
WORKDIR "/src/FlowerShop.Cli"
RUN dotnet build "FlowerShop.Cli.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FlowerShop.Cli.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FlowerShop.Cli.dll"]
