FROM microsoft/dotnet:2.1-sdk-bionic
WORKDIR /dotnet/LHGames

RUN apt-get update
RUN apt-get install -y libunwind-dev

COPY . .

RUN dotnet restore
RUN dotnet publish -c Release -o out -r linux-x64

ENTRYPOINT ["dotnet", "LHGames/out/LHGames.dll"]
EXPOSE 3000
