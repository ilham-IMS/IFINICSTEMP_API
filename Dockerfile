FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8008

FROM base AS final
WORKDIR /app
COPY ./publish .
COPY ./ReportTemplate ./ReportTemplate
ENV ASPNETCORE_URLS=http://*:8008
USER root

ENTRYPOINT ["dotnet", "API.dll"]