#!/bin/bash
cd /out
sed -i 's/YOUR_SENDER_EMAIL_ADDRESS_HERE/'"$From"'/g' appsettings.production.json
sed -i 's/YOUR_USERNAME_HERE/'"$UserName"'/g' appsettings.production.json
sed -i 's/YOUR_PASSWORD_HERE/'"$Password"'/g' appsettings.production.json
sed -i 's/YOUR_HOST_URL_HERE/'"$Host"'/g' appsettings.production.json
sed -i 's/YOUR_PORT_NUMBER_HERE/'"$Port"'/g' appsettings.production.json
sed -i 's/SET_SSL_TLS_Unsecured/'"$SetSmtpProtocol"'/g' appsettings.production.json

sed -i 's/YOUR_SENDGRID_API/'"$SendGridApiKey"'/g' appsettings.production.json
sed -i 's/YOUR_EXCEPTIONLESS_API_KEY/'"$ExceptionLessApiKey"'/g' appsettings.production.json

sed -i 's/PROMACT_OAUTH_URL/'"$PromactOAuthUrl"'/g' appsettings.production.json
sed -i 's/NUMBER_OF_SICK_LEAVE/'"$SickLeave"'/g' appsettings.production.json
sed -i 's/NUMBER_OF_CASUAL_LEAVE/'"$CasualLeave"'/g' appsettings.production.json
sed -i 's/PROMACT_ERP_URL/'"$PromactErpUrl"'/g' appsettings.production.json

jq '.ConnectionStrings.DefaultConnection |= "'"$ConnectionString"'" ' appsettings.json 
dotnet /out/Promact.Oauth.Server.dll