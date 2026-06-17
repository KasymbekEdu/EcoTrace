# 1. С# кодын жинақтауға (Build) арналған SDK бейнесі (.NET 10)
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build-env
WORKDIR /app

# Жоба файлдарын көшіру және тәуелділіктерді (Nuget) қалпына келтіру
COPY *.csproj ./
RUN dotnet restore

# Барлық кодты көшіріп, релиздік нұсқаны жинақтау
COPY . ./
RUN dotnet publish -c Release -o out

# 2. Бағдарламаны іске қосатын жеңіл Runtime бейнесі
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build-env /app/out .

# Render беретін портты автоматты түрде оқу
ENV ASPNETCORE_URLS=http://+:10000
EXPOSE 10000

ENTRYPOINT ["dotnet", "EcoSystem.dll"]
