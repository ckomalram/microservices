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

## 1.0.2

    Se creo nuevo proyecto de seguridad
    Se creo entidad de user
    Se creo contexto de seguridad
    Se configuro conecction string para sql server
    Se inyecto con EF el uso de sql server en program cs
    Se configuro identity core en el program cs
    Se creo contexto estatico de seguridad data
    Se inyecto en program cs seguridadData con "using" logica para alcance de servicio.

## 1.0.3 Implementando patterns CQRS

    Se crea clase register
    Se crea clase dto
    Se instaa MediaTR
    Se crea clase mapping profile
    Se inyecta automapper en program.cs
    Se inyecta mediatr en program.cs
    Se crea controlador
    Se instala fluentvalidation aspnetcore
    Se inyecta FV en program.cs
    Se agrega FV a register class.

## 1.0.4 JWT en nuestros Microservices ( https://randomkeygen.com/)

    Crear clase de JwtGenerator
    Se Inyecta con addscope clase de token
    Se agrega campo al userdto de token
    Se agrega logica a register.class
    Se crea login.cs
    Se crea httpost en controlador user para login.
    Se crea clase UserSesion
    Se inyecta UserSession como scoped.
    Se crea clase cqrs userCurrent para obtener ese usuario sesion.
