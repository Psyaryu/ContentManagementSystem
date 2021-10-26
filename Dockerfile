FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
RUN apt-get update -y && apt-get install python3-pip python3.7 -y
RUN python3 -m pip install pymysql cryptography

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["ContentManagementSystem/ContentManagementSystem.csproj", "ContentManagementSystem/"]
RUN dotnet restore "ContentManagementSystem/ContentManagementSystem.csproj"
COPY . .
WORKDIR "/src/ContentManagementSystem"
RUN dotnet build "ContentManagementSystem.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ContentManagementSystem.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY rds.py .
COPY --from=publish /app/publish .
COPY commands.sh /app/commands.sh
RUN ["chmod", "+x", "/app/commands.sh"]
ENTRYPOINT ["/app/commands.sh"]
