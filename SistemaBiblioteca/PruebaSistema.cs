using System;
using System.Collections.Generic;
using SistemaBiblioteca.Models;
using SistemaBiblioteca.Utils;

namespace SistemaBiblioteca
{
    /// <summary>
    /// Script de prueba para verificar que el sistema funciona correctamente
    /// Este script crea datos de prueba en el sistema
    /// </summary>
    public static class PruebaSistema
    {
        public static void CrearDatosPrueba()
        {
            Console.WriteLine(new string('=', 60));
            Console.WriteLine("  CREANDO DATOS DE PRUEBA".PadLeft(42));
            Console.WriteLine(new string('=', 60));

            // Crear autores de prueba
            Console.WriteLine("\n[1/6] Creando autores...");
            var autores = new List<Autor>
            {
                new Autor
                {
                    Id = ManejoArchivos.ObtenerSiguienteId("autores"),
                    Nombre = "Gabriel",
                    Apellido = "García Márquez",
                    Nacionalidad = "Colombiana"
                },
                new Autor
                {
                    Id = ManejoArchivos.ObtenerSiguienteId("autores"),
                    Nombre = "Isabel",
                    Apellido = "Allende",
                    Nacionalidad = "Chilena"
                },
                new Autor
                {
                    Id = ManejoArchivos.ObtenerSiguienteId("autores"),
                    Nombre = "Jorge",
                    Apellido = "Luis Borges",
                    Nacionalidad = "Argentina"
                }
            };
            ManejoArchivos.GuardarDatos("autores", autores);
            Console.WriteLine($"    {autores.Count} autores creados");

            // Crear categorías de prueba
            Console.WriteLine("\n[2/6] Creando categorías...");
            var categorias = new List<Categoria>
            {
                new Categoria
                {
                    Id = ManejoArchivos.ObtenerSiguienteId("categorias"),
                    Nombre = "Ficción",
                    Descripcion = "Novelas y cuentos de ficción"
                },
                new Categoria
                {
                    Id = ManejoArchivos.ObtenerSiguienteId("categorias"),
                    Nombre = "Ciencia",
                    Descripcion = "Libros científicos y técnicos"
                },
                new Categoria
                {
                    Id = ManejoArchivos.ObtenerSiguienteId("categorias"),
                    Nombre = "Historia",
                    Descripcion = "Libros de historia y biografías"
                }
            };
            ManejoArchivos.GuardarDatos("categorias", categorias);
            Console.WriteLine($"    {categorias.Count} categorías creadas");

            // Crear libros de prueba
            Console.WriteLine("\n[3/6] Creando libros...");
            var libros = new List<Libro>
            {
                new Libro
                {
                    Id = ManejoArchivos.ObtenerSiguienteId("libros"),
                    Titulo = "Cien Años de Soledad",
                    Isbn = "9780307474728",
                    IdAutor = 1,
                    IdCategoria = 1,
                    AñoPublicacion = 1967,
                    CantidadCopias = 5,
                    CopiasDisponibles = 5,
                    Activo = true
                },
                new Libro
                {
                    Id = ManejoArchivos.ObtenerSiguienteId("libros"),
                    Titulo = "La Casa de los Espíritus",
                    Isbn = "9788401242267",
                    IdAutor = 2,
                    IdCategoria = 1,
                    AñoPublicacion = 1982,
                    CantidadCopias = 3,
                    CopiasDisponibles = 3,
                    Activo = true
                },
                new Libro
                {
                    Id = ManejoArchivos.ObtenerSiguienteId("libros"),
                    Titulo = "Ficciones",
                    Isbn = "9788432248665",
                    IdAutor = 3,
                    IdCategoria = 1,
                    AñoPublicacion = 1944,
                    CantidadCopias = 4,
                    CopiasDisponibles = 4,
                    Activo = true
                }
            };
            ManejoArchivos.GuardarDatos("libros", libros);
            Console.WriteLine($"    {libros.Count} libros creados");

            // Crear usuarios de prueba
            Console.WriteLine("\n[4/6] Creando usuarios...");
            var usuarios = new List<Usuario>
            {
                new Usuario
                {
                    Id = ManejoArchivos.ObtenerSiguienteId("usuarios"),
                    Nombre = "Juan",
                    Apellido = "Pérez",
                    Email = "juan.perez@email.com",
                    Telefono = "12345678",
                    Direccion = "Calle Principal 123",
                    Activo = true,
                    MultasPendientes = 0
                },
                new Usuario
                {
                    Id = ManejoArchivos.ObtenerSiguienteId("usuarios"),
                    Nombre = "María",
                    Apellido = "González",
                    Email = "maria.gonzalez@email.com",
                    Telefono = "87654321",
                    Direccion = "Avenida Central 456",
                    Activo = true,
                    MultasPendientes = 0
                }
            };
            ManejoArchivos.GuardarDatos("usuarios", usuarios);
            Console.WriteLine($"    {usuarios.Count} usuarios creados");

            // Inicializar archivos vacíos para préstamos y multas
            Console.WriteLine("\n[5/6] Inicializando préstamos...");
            ManejoArchivos.GuardarDatos<Prestamo>("prestamos", new List<Prestamo>());
            Console.WriteLine("    Archivo de préstamos inicializado");

            Console.WriteLine("\n[6/6] Inicializando multas...");
            ManejoArchivos.GuardarDatos<Multa>("multas", new List<Multa>());
            Console.WriteLine("    Archivo de multas inicializado");

            Console.WriteLine("\n" + new string('=', 60));
            Console.WriteLine("   DATOS DE PRUEBA CREADOS EXITOSAMENTE".PadLeft(49));
            Console.WriteLine(new string('=', 60));
            Console.WriteLine("\nAhora puedes ejecutar el sistema principal.");
        }

        // Punto de entrada alternativo para ejecutar solo las pruebas
        // Descomentar el siguiente método Main y comentar el del Program.cs
        // para ejecutar solo la creación de datos de prueba
        /*
        static void Main(string[] args)
        {
            CrearDatosPrueba();
            Console.WriteLine("\nPresione Enter para salir...");
            Console.ReadLine();
        }
        */
    }
}
