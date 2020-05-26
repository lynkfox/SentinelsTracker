FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["website/website.csproj", "website/"]
RUN dotnet restore "website/website.csproj"
COPY . .
WORKDIR "/src/website"
RUN dotnet build "website.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "website.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "website.dll"]