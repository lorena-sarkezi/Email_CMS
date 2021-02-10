FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim
WORKDIR /src
EXPOSE 80
EXPOSE 443

RUN apt-get update
RUN apt-get install -y curl
RUN curl -sL https://deb.nodesource.com/setup_10.x | bash -
RUN apt-get install -y nodejs
#RUN apt-get install -f -y npm

#RUN npm cache clean --force

COPY ["CMS.Core/CMS.Core.csproj","CMS.Core/"]
COPY ["CMS.Models/CMS.Data.csproj", "CMS.Models/"]
RUN dotnet restore "CMS.Core/CMS.Core.csproj"
COPY . .
#RUN dotnet build "CMS.Core/CMS.Core.csproj" -c Release -o /app/build
RUN dotnet publish "CMS.Core/CMS.Core.csproj" -c Release -o /app/publish
WORKDIR /app/publish
ENTRYPOINT ["dotnet","CMS.Core.dll"]