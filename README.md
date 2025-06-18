# Backend - Turnero Médico

Este es el backend del proyecto de turnero médico, desarrollado en C# con arquitectura en tres capas y patrón MVC.

## Tecnologías utilizadas

- .NET 8
- C#
- Entity Framework
- SQL Server
- Postman + Newman (para testing)
- GitHub Actions (CI/CD)

## Estructura del proyecto

- **Capa de Presentación**: API RESTful (Controllers)
- **Capa de Negocio**: Lógica de negocio
- **Capa de Datos**: Repositorios y conexión con la base de datos
- **Modelos**: Entidades como Usuario, Profesional, Paciente, Turno, etc.

## Funcionalidades principales

- Registro y login de usuarios (con roles: paciente y profesional)
- Generación de token (JWT)
- Reserva y cancelación de turnos
- Validación de solapamientos de turnos
- Gestión de disponibilidad de profesionales

## Cómo ejecutar el proyecto

1. Abrí la solución en Visual Studio
2. Configurá la cadena de conexión en `appsettings.json`
3. Ejecutá las migraciones si es necesario
4. Iniciá el servidor con IIS Express o Kestrel

## Testing

- Se realizaron pruebas automatizadas de endpoints con Postman
- Se ejecutaron los tests automáticamente con Newman y GitHub Actions

## CI/CD

- Configurado con GitHub Actions para:
  - Ejecutar build y test en cada push a `desarrollo-alejandro` y merge a `main`
  - Validación con linter (cuando aplique)
  - Simulación de despliegue en entorno local
- Archivo de workflow: `.github/workflows/ci-cd.yml`

## Branching Strategy

- `main`: rama estable para producción
- `desarrollo-alejandro`: rama de desarrollo principal


