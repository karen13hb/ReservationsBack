## Sitema de Gestion de reservas

## Descripción

Este proyecto es un sistema backend para gestionar reservas de espacios compartidos utilizando una arquitectura hexagonal. 
El sistema asegura que no existan conflictos de horarios al crear reservas y permite gestionar (crear, cancelar y obtener) dichas reservas. 
Además, implementa eventos de dominio para garantizar una lógica de negocio desacoplada y extensible.

## Requisitos

1. .NET 8 o superior: Framework para construir el backend.
2. EF Core: ORM para gestionar la persistencia de datos.
3. Swagger: Documentación interactiva de la API.
4. SQL Server: Base de datos de ejemplo para desarrollo local.
5. FluentAssertion: Test.

## Instalación

1. Clonar repositorio:
   git clone https://github.com/tuusuario/reservationsBack.git
    cd reservationsBack

2. Configura la base de datos en el archivo appsettings.json:
    "ConnectionStrings": {
   "DefaultConnection": "Server=Localhost;Database=reserveCompanyDB;Trusted_Connection=True;Encrypt=False;
   }
4. Ejecutar script base de datos en SQL Server 
    Ubicaión del archivo en la capa de persistencia
3. Inicia el servidor:
    dotnet run
    
## Endpoints de la API

  ### Reservas

1. GET	/api/reservations	Lista todas las reservas con filtros.
2. POST	/api/reservations	Crea una nueva reserva.
3. DELETE	/api/reservations/{id}	Cancela una reserva por su ID.

## Pruebas

Ejecutar pruebas:
  dotnet test

   
