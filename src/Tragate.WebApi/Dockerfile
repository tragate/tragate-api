FROM microsoft/aspnetcore:latest
ARG source
WORKDIR /app

EXPOSE 5002/tcp
ENV ASPNETCORE_URLS http://*:5002
ENV ASPNETCORE_ENVIRONMENT docker

COPY ${source:-obj/Docker/publish} .
ENTRYPOINT ["dotnet", "Tragate.WebApi.dll"]