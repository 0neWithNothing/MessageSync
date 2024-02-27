FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Executor.csproj", "Executor/"]
RUN dotnet restore "Executor/Executor.csproj"

COPY . Executor/.
RUN dotnet publish -c Release -o /app Executor/Executor.csproj 

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS publish
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "Executor.dll"]