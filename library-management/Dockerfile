# Dockerfile dentro de src/
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

COPY library-management/LibraryManagement.csproj ./
RUN dotnet restore "LibraryManagement.csproj"

COPY library-management/ ./              
RUN dotnet publish "LibraryManagement.csproj" -c Release -o /app/out

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app

COPY --from=build /app/out .


ENTRYPOINT ["dotnet", "LibraryManagement.dll"]
