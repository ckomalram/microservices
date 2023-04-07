# Comandos utiles

dotnet tool list --global

dotnet tool install --global dotnet-ef --version 7.0.4

# Comandos EF

## para crear una migracion

dotnet ef migrations add NOMBRE_MIGRAICON --project NOMBRE_PROYECTO

## para ejecutar la migracion

dotnet ef database update --project NOMBRE_PROYECTO
dotnet ef database update

# Versiones

## 1.0.5 API Gateway y Ocelot
