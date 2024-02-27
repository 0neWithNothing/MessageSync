FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Generator.csproj", "Generator/"]
RUN dotnet restore "Generator/Generator.csproj"

COPY . Generator/.
RUN dotnet publish -c Release -o /app Generator/Generator.csproj 

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS publish
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "Generator.dll"]