FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "LMS-Demo"
RUN dotnet publish "LMS-Demo" -c Release -o /publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 as final
WORKDIR /app
COPY --from=build /publish .
CMD [ "mkdir","LMS-Demo" ]
ENTRYPOINT [ "dotnet", "LMS-Demo.dll" ]