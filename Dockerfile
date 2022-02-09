FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
LABEL MAINTAINER="DevOps"
LABEL APPLICATION="Halobiz-Backend"
WORKDIR /app
EXPOSE 80
EXPOSE 443

ENV ASPNETCORE_ENVIRONMENT="Development"

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["HaloBiz.csproj", "./"]
COPY . .
RUN dotnet restore "HaloBiz.csproj"
WORKDIR /src/.
RUN dotnet build "HaloBiz.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HaloBiz.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HaloBiz.dll"]
