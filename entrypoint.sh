#!/bin/bash
cd /out
sed -i 's/YOUR_SENDER_EMAIL_ADDRESS_HERE/'"$From"'/g' appsettings.Production.json
sed -i 's/YOUR_USERNAME_HERE/'"$UserName"'/g' appsettings.Production.json
sed -i 's/YOUR_PASSWORD_HERE/'"$Password"'/g' appsettings.Production.json
sed -i 's/YOUR_HOST_URL_HERE/'"$Host"'/g' appsettings.Production.json
sed -i 's/YOUR_PORT_NUMBER_HERE/'"$Port"'/g' appsettings.Production.json
sed -i 's/SET_SSL_TLS_Unsecured/'"$SetSmtpProtocol"'/g' appsettings.Production.json

sed -i 's/YOUR_SENDGRID_API/'"$SendGridApiKey"'/g' appsettings.Production.json
sed -i 's/YOUR_EXCEPTIONLESS_API_KEY/'"$ExceptionLessApiKey"'/g' appsettings.Production.json

sed -i 's/PROMACT_OAUTH_URL/'"$PromactOAuthUrl"'/g' appsettings.Production.json
sed -i 's/NUMBER_OF_SICK_LEAVE/'"$SickLeave"'/g' appsettings.Production.json
sed -i 's/NUMBER_OF_CASUAL_LEAVE/'"$CasualLeave"'/g' appsettings.Production.json
sed -i 's/PROMACT_ERP_URL/'"$PromactErpUrl"'/g' appsettings.Production.json

jq '.ConnectionStrings.DefaultConnection |= "'"$ConnectionString"'" ' appsettings.json

env

cat appsettings.json
cat appsettings.Production.json


/usr/bin/dotnet /out/app.dll
