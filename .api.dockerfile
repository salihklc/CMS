FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env
ENV ASPNETCORE_VERSION 5.0
WORKDIR /src

COPY ./ ./cms
RUN dotnet restore ./cms/CMS.Api/CMS.Api.csproj 
RUN dotnet publish ./cms/CMS.Api/CMS.Api.csproj -o /publish -c release

#Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim

# RUN sed -i 's/DEFAULT@SECLEVEL=2/DEFAULT@SECLEVEL=1/g' /etc/ssl/openssl.cnf
# RUN sed -i 's/MinProtocol = TLSv1.2/MinProtocol = TLSv1/g' /etc/ssl/openssl.cnf
# RUN sed -i 's/DEFAULT@SECLEVEL=2/DEFAULT@SECLEVEL=1/g' /usr/lib/ssl/openssl.cnf
# RUN sed -i 's/MinProtocol = TLSv1.2/MinProtocol = TLSv1/g' /usr/lib/ssl/openssl.cnf
# ENV TZ=Europe/Istanbul
# RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone
  
COPY --from=build-env /publish /publish

WORKDIR /publish
ENTRYPOINT [ "dotnet", "CMS.Api.dll" ]