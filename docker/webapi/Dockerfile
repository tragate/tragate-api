FROM microsoft/aspnetcore:latest
ARG source
WORKDIR /app
COPY app/publish /app
COPY setup.sh /app
RUN chmod +x /app/setup.sh
EXPOSE 5002/tcp
ENV ASPNETCORE_URLS http://*:5002
ENV ASPNETCORE_ENVIRONMENT docker
ENTRYPOINT ["/app/setup.sh"]