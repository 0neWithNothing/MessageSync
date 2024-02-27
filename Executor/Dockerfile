FROM mcr.microsoft.com/dotnet/sdk:6.0-focal AS build
WORKDIR /src
COPY . .
RUN dotnet restore "Executor.csproj"


RUN dotnet publish -c Release -o /app Executor.csproj 

FROM mcr.microsoft.com/dotnet/aspnet:6.0-focal AS publish
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "Executor.dll"]