# ABAManager
Aplicación Desktop que gestiona todos los procesos de billing de las compañías.


## Visual Studio 2019 Professional Edition

- [Download visual studio]("https://visualstudio.microsoft.com/es/downloads/")

## Pasos para configurar el proyecto y que corra localmente
- Cargar el proyecto en Visual Studio y ejecutar.
- Configurar en el `App.config` con la ruta a los endpoints de las compañías y sus respectivas base de datos.

## Dependencias del proyecto Base de datos 

- La base de datos se encuentra alojada en `AWS` en un `RDS de SQL Server Express` la versión gratuita por un año. 

- Los backups son gestionados por AWS. La frecuencia es 1 por semana.

- Dentro de la propia aplicación de backend tiene un archivo que se encarga de inizializar la base dedatos con los párametros que son comunes para todas las compañías.