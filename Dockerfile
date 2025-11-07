# Definimos la imagen base
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Creamos la carpeta en la que vamos a trabajar 
WORKDIR /src

# Copiamos los archivos .csproj
COPY ["school.Api/school.Api.csproj", "school.Api/"]
COPY ["school.Application/school.Application.csproj", "school.Application/"]
COPY ["school.Domain/school.Domain.csproj", "school.Domain/"]
COPY ["school.Infrastructure/school.Infrastructure.csproj", "school.Infrastructure/"]

# Restauramos las dependencias 
RUN dotnet restore "school.Api/school.Api.csproj"

# Copiamos tdo el codigo fuente
COPY . .


# Ingresamos a la api
WORKDIR /src/school.Api

#  Compilamos el codigo
RUN dotnet build "school.Api.csproj" -c Release -o /app/build

# Pasamos a la etapa de publicacion
FROM build AS publish

WORKDIR /src/school.Api

RUN dotnet publish "school.Api.csproj" -c Release -o /app/publish

# Creamos una nuevva imagen
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final

WORKDIR /app

COPY --from=publish /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "school.Api.dll"]