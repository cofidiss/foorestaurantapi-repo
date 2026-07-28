FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src

COPY . .

RUN dotnet restore FooRestaurantApi.sln
RUN dotnet publish FooRestaurantApi/APILayer/FooRestaurantApi.APILayer.csproj \
    -c Release \
    -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 3000

ENTRYPOINT ["dotnet", "FooRestaurantApi.APILayer.dll"]