FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
# copy all the layers' csproj files into respective folders
COPY ["./Backend/Backend.Api/Backend.Api.csproj", "src/Backend/Backend.Api/"]
RUN ls -l
COPY ["./Backend/Backend.Applications/Backend.Applications.csproj", "src/Backend/Backend.Applications/"]
RUN ls -l
COPY ["./Backend/Backend.Infrastructure/Backend.Infrastructure.csproj", "src/Backend/Backend.Infrastructure/"]
RUN ls -l
COPY ["./Backend/Backend.Domain/Backend.Domain.csproj", "src/Backend/Backend.Domain/"]
RUN ls -l
# run restore over Api project - this pulls restore over the dependent projects as well
RUN dotnet restore "src/Backend/Backend.Api/Backend.Api.csproj"

COPY . .

# run build over the Api project
WORKDIR "/src/Backend/Backend.Api/"
RUN dotnet build -c Release -o /app/build 

# run publish over the Api project
FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

FROM base AS runtime
WORKDIR /app

COPY --from=publish /app/publish .
RUN ls -l
ENTRYPOINT [ "dotnet", "Backend.Api.dll" ]

