using System;
using System.Collections.Generic;
using SistemaBiblioteca.Utils;

namespace SistemaBiblioteca.Models
{
    /// <summary>
    /// Modelo de Libro
    /// </summary>
    public class Libro
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Isbn { get; set; }
        public int IdAutor { get; set; }
        public int IdCategoria { get; set; }
        public int AñoPublicacion { get; set; }
        public int CantidadCopias { get; set; }
        public int CopiasDisponibles { get; set; }
        public bool Activo { get; set; }
    }

    /// <summary>
    /// Módulo de gestión de Libros
    /// Maneja el CRUD de libros en la biblioteca
    /// </summary>
    public static class LibroService
    {
        public const string ARCHIVO_LIBROS = "libros";
        private const string CONTADOR_LIBROS = "libros";

        public static Libro CrearLibro()
        {
            Console.WriteLine("\n--- REGISTRAR NUEVO LIBRO ---\n");

            string titulo = Validaciones.ValidarTexto("Título del libro: ", 2, 100);
            string isbn = Validaciones.ValidarIsbn("ISBN: ");
            int idAutor = Validaciones.ValidarNumeroEntero("ID del autor: ", 1);
            int idCategoria = Validaciones.ValidarNumeroEntero("ID de la categoría: ", 1);
            int añoPublicacion = Validaciones.ValidarNumeroEntero("Año de publicación: ", 1500, 2026);
            int cantidadCopias = Validaciones.ValidarNumeroEntero("Cantidad de copias: ", 1, 1000);

            var libro = new Libro
            {
                Id = ManejoArchivos.ObtenerSiguienteId(CONTADOR_LIBROS),
                Titulo = titulo,
                Isbn = isbn,
                IdAutor = idAutor,
                IdCategoria = idCategoria,
                AñoPublicacion = añoPublicacion,
                CantidadCopias = cantidadCopias,
                CopiasDisponibles = cantidadCopias,
                Activo = true
            };

            var libros = ManejoArchivos.CargarDatos<Libro>(ARCHIVO_LIBROS);
            libros.Add(libro);
            ManejoArchivos.GuardarDatos(ARCHIVO_LIBROS, libros);

            Console.WriteLine($"\nLibro registrado exitosamente con ID: {libro.Id}");
            return libro;
        }

        public static void ListarLibros()
        {
            var libros = ManejoArchivos.CargarDatos<Libro>(ARCHIVO_LIBROS);

            Console.WriteLine("\n--- LISTA DE LIBROS ---\n");

            if (libros.Count == 0)
            {
                Console.WriteLine("No hay libros registrados.");
            }
            else
            {
                Console.WriteLine($"{"ID",-5} {"Título",-30} {"ISBN",-15} {"Año",-6} {"Disponibles",-12}");
                Console.WriteLine(new string('-', 73));
                foreach (var libro in libros)
                {
                    string titulo = libro.Titulo.Length > 30
                        ? libro.Titulo.Substring(0, 27) + "..."
                        : libro.Titulo;
                    Console.WriteLine($"{libro.Id,-5} {titulo,-30} {libro.Isbn,-15} {libro.AñoPublicacion,-6} {libro.CopiasDisponibles}/{libro.CantidadCopias}");
                }
            }

            Console.WriteLine($"\nTotal de libros: {libros.Count}");
        }

        public static void BuscarLibro()
        {
            Console.WriteLine("\n--- BUSCAR LIBRO ---\n");

            int idLibro = Validaciones.ValidarNumeroEntero("Ingrese el ID del libro: ", 1);
            var libros = ManejoArchivos.CargarDatos<Libro>(ARCHIVO_LIBROS);
            var libro = ManejoArchivos.BuscarPorId(libros, idLibro, l => l.Id);

            if (libro != null)
            {
                Console.WriteLine("\nLibro encontrado:");
                Console.WriteLine($"ID: {libro.Id}");
                Console.WriteLine($"Título: {libro.Titulo}");
                Console.WriteLine($"ISBN: {libro.Isbn}");
                Console.WriteLine($"ID Autor: {libro.IdAutor}");
                Console.WriteLine($"ID Categoría: {libro.IdCategoria}");
                Console.WriteLine($"Año de publicación: {libro.AñoPublicacion}");
                Console.WriteLine($"Copias totales: {libro.CantidadCopias}");
                Console.WriteLine($"Copias disponibles: {libro.CopiasDisponibles}");
                Console.WriteLine($"Estado: {(libro.Activo ? "Activo" : "Inactivo")}");
            }
            else
            {
                Console.WriteLine($"\nERROR: No se encontró un libro con ID {idLibro}");
            }
        }

        public static void ActualizarLibro()
        {
            Console.WriteLine("\n--- ACTUALIZAR LIBRO ---\n");

            int idLibro = Validaciones.ValidarNumeroEntero("Ingrese el ID del libro a actualizar: ", 1);
            var libros = ManejoArchivos.CargarDatos<Libro>(ARCHIVO_LIBROS);
            var libro = ManejoArchivos.BuscarPorId(libros, idLibro, l => l.Id);

            if (libro != null)
            {
                Console.WriteLine($"\nLibro actual: {libro.Titulo}");
                Console.WriteLine("\nIngrese los nuevos datos (Enter para mantener el actual):");

                Console.Write($"Título [{libro.Titulo}]: ");
                string nuevoTitulo = Console.ReadLine()?.Trim();
                if (!string.IsNullOrEmpty(nuevoTitulo))
                {
                    libro.Titulo = nuevoTitulo;
                }

                Console.Write($"ISBN [{libro.Isbn}]: ");
                string nuevoIsbn = Console.ReadLine()?.Trim();
                if (!string.IsNullOrEmpty(nuevoIsbn))
                {
                    libro.Isbn = nuevoIsbn;
                }

                Console.Write($"Año [{libro.AñoPublicacion}]: ");
                string nuevoAñoStr = Console.ReadLine()?.Trim();
                if (!string.IsNullOrEmpty(nuevoAñoStr))
                {
                    if (int.TryParse(nuevoAñoStr, out int añoValidado))
                    {
                        if (añoValidado >= 1500 && añoValidado <= 2026)
                        {
                            libro.AñoPublicacion = añoValidado;
                        }
                        else
                        {
                            Console.WriteLine("ERROR: El año debe estar entre 1500 y 2026.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("ERROR: Debe ingresar un año válido (solo números).");
                    }
                }

                Console.Write($"Cantidad de copias [{libro.CantidadCopias}]: ");
                string nuevaCantidadStr = Console.ReadLine()?.Trim();
                if (!string.IsNullOrEmpty(nuevaCantidadStr))
                {
                    if (int.TryParse(nuevaCantidadStr, out int cantidadValidada))
                    {
                        if (cantidadValidada >= 1 && cantidadValidada <= 1000)
                        {
                            int diferencia = cantidadValidada - libro.CantidadCopias;
                            libro.CantidadCopias = cantidadValidada;
                            libro.CopiasDisponibles += diferencia;
                        }
                        else
                        {
                            Console.WriteLine("ERROR: La cantidad debe estar entre 1 y 1000.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("ERROR: Debe ingresar un número válido (solo números).");
                    }
                }

                ManejoArchivos.GuardarDatos(ARCHIVO_LIBROS, libros);
                Console.WriteLine("\nLibro actualizado exitosamente.");
            }
            else
            {
                Console.WriteLine($"\nERROR: No se encontró un libro con ID {idLibro}");
            }
        }

        public static void EliminarLibro()
        {
            Console.WriteLine("\n--- ELIMINAR LIBRO ---\n");

            int idLibro = Validaciones.ValidarNumeroEntero("Ingrese el ID del libro a eliminar: ", 1);
            var libros = ManejoArchivos.CargarDatos<Libro>(ARCHIVO_LIBROS);
            var libro = ManejoArchivos.BuscarPorId(libros, idLibro, l => l.Id);

            if (libro != null)
            {
                libro.Activo = false;
                ManejoArchivos.GuardarDatos(ARCHIVO_LIBROS, libros);
                Console.WriteLine($"\nLibro con ID {idLibro} desactivado exitosamente.");
            }
            else
            {
                Console.WriteLine($"\nERROR: No se encontró un libro con ID {idLibro}");
            }
        }

        public static void MenuLibros()
        {
            while (true)
            {
                Validaciones.LimpiarPantalla();
                Console.WriteLine(new string('=', 50));
                Console.WriteLine("  GESTIÓN DE LIBROS".PadLeft(34));
                Console.WriteLine(new string('=', 50));
                Console.WriteLine("\n1. Registrar nuevo libro");
                Console.WriteLine("2. Listar todos los libros");
                Console.WriteLine("3. Buscar libro");
                Console.WriteLine("4. Actualizar libro");
                Console.WriteLine("5. Eliminar libro");
                Console.WriteLine("0. Volver al menú principal");
                Console.WriteLine(new string('=', 50));

                Console.Write("\nSeleccione una opción: ");
                string opcion = Console.ReadLine()?.Trim();

                switch (opcion)
                {
                    case "1":
                        CrearLibro();
                        Validaciones.Pausar();
                        break;
                    case "2":
                        ListarLibros();
                        Validaciones.Pausar();
                        break;
                    case "3":
                        BuscarLibro();
                        Validaciones.Pausar();
                        break;
                    case "4":
                        ActualizarLibro();
                        Validaciones.Pausar();
                        break;
                    case "5":
                        EliminarLibro();
                        Validaciones.Pausar();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("\nERROR: Opción inválida.");
                        Validaciones.Pausar();
                        break;
                }
            }
        }
    }
}
