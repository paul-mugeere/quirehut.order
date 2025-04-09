FROM mcr.microsoft.com/dotnet/sdk:9.0 AS publish
ARG BUILD_CONFIGURATION=Release
WORKDIR /app

COPY ./quirehut.order.api.fsproj ./src/
RUN dotnet restore "./src/quirehut.order.api.fsproj"

COPY . ./src
RUN dotnet publish "src/quirehut.order.api.fsproj" -c $BUILD_CONFIGURATION -o ./publish 

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "quirehut.order.api.dll"]
