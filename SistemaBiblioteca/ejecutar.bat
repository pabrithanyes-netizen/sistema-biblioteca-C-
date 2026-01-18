@echo off
echo ========================================
echo   SISTEMA DE GESTION DE BIBLIOTECA
echo ========================================
echo.
echo Compilando y ejecutando el proyecto...
echo.

dotnet run

if errorlevel 1 (
    echo.
    echo ERROR: No se pudo ejecutar el proyecto.
    echo.
    echo Verifica que tengas .NET 6.0 SDK instalado:
    echo https://dotnet.microsoft.com/download
    echo.
    pause
) else (
    echo.
    pause
)
