FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["AspNetCore.StartUpTemplate.Webapi/AspNetCore.StartUpTemplate.Webapi.csproj", "AspNetCore.StartUpTemplate.Webapi/"]
RUN dotnet restore "AspNetCore.StartUpTemplate.Webapi/AspNetCore.StartUpTemplate.Webapi.csproj"
COPY . .
WORKDIR "/src/AspNetCore.StartUpTemplate.Webapi"
RUN dotnet build "AspNetCore.StartUpTemplate.Webapi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AspNetCore.StartUpTemplate.Webapi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AspNetCore.StartUpTemplate.Webapi.dll"]
