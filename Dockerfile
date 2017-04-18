FROM promact/docker-dotnet-nodejs:latest
MAINTAINER Promact Infotech<info@promactinfo.com>

WORKDIR /app

COPY ./Promact.Oauth.Server/src/Promact.Oauth.Server/package.json .
COPY ./Promact.Oauth.Server/src/Promact.Oauth.Server/typings.json .
RUN npm install

COPY ./Promact.Oauth.Server/src/Promact.Oauth.Server/bower.json .
COPY ./Promact.Oauth.Server/src/Promact.Oauth.Server/.bowerrc .
RUN bower install --allow-root

# copy project.json and restore as distinct layers
COPY ./Promact.Oauth.Server/src/Promact.Oauth.Server ./

# copy and build everything else
RUN gulp copytowwwroot && npm run aot && npm run rollup && npm run build && mkdir /out
RUN cd.. && cd.. && dotnet restore
RUN dotnet publish -c Release -o /out && cp appsettings.development.example.json /out/appsettings.production.json && rm -rf /app 
ENV ASPNETCORE_ENVIRONMENT Production
COPY entrypoint.sh /usr/local/bin/
RUN chmod +x /usr/local/bin/entrypoint.sh
EXPOSE 5000
ENTRYPOINT ["entrypoint.sh"]
