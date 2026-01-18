using System;
using System.Collections.Generic;
using SistemaBiblioteca.Utils;

namespace SistemaBiblioteca.Models
{
    /// <summary>
    /// Modelo de Autor
    /// </summary>
    public class Autor
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Nacionalidad { get; set; }
    }

    /// <summary>
    /// Módulo de gestión de Autores
    /// Maneja el CRUD de autores de libros
    /// </summary>
    public static class AutorService
    {
        private const string ARCHIVO_AUTORES = "autores";
        private const string CONTADOR_AUTORES = "autores";

        public static Autor CrearAutor()
        {
            Console.WriteLine("\n--- REGISTRAR NUEVO AUTOR ---\n");

            string nombre = Validaciones.ValidarTexto("Nombre del autor: ", 2, 50);
            string apellido = Validaciones.ValidarTexto("Apellido del autor: ", 2, 50);
            string nacionalidad = Validaciones.ValidarTexto("Nacionalidad: ", 2, 30);

            var autor = new Autor
            {
                Id = ManejoArchivos.ObtenerSiguienteId(CONTADOR_AUTORES),
                Nombre = nombre,
                Apellido = apellido,
                Nacionalidad = nacionalidad
            };

            var autores = ManejoArchivos.CargarDatos<Autor>(ARCHIVO_AUTORES);
            autores.Add(autor);
            ManejoArchivos.GuardarDatos(ARCHIVO_AUTORES, autores);

            Console.WriteLine($"\nAutor registrado exitosamente con ID: {autor.Id}");
            return autor;
        }

        public static void ListarAutores()
        {
            var autores = ManejoArchivos.CargarDatos<Autor>(ARCHIVO_AUTORES);

            Console.WriteLine("\n--- LISTA DE AUTORES ---\n");

            if (autores.Count == 0)
            {
                Console.WriteLine("No hay autores registrados.");
            }
            else
            {
                Console.WriteLine($"{"ID",-5} {"Nombre",-20} {"Apellido",-20} {"Nacionalidad",-15}");
                Console.WriteLine(new string('-', 65));
                foreach (var autor in autores)
                {
                    Console.WriteLine($"{autor.Id,-5} {autor.Nombre,-20} {autor.Apellido,-20} {autor.Nacionalidad,-15}");
                }
            }

            Console.WriteLine($"\nTotal de autores: {autores.Count}");
        }

        public static void BuscarAutor()
        {
            Console.WriteLine("\n--- BUSCAR AUTOR ---\n");

            int idAutor = Validaciones.ValidarNumeroEntero("Ingrese el ID del autor: ", 1);
            var autores = ManejoArchivos.CargarDatos<Autor>(ARCHIVO_AUTORES);
            var autor = ManejoArchivos.BuscarPorId(autores, idAutor, a => a.Id);

            if (autor != null)
            {
                Console.WriteLine("\nAutor encontrado:");
                Console.WriteLine($"ID: {autor.Id}");
                Console.WriteLine($"Nombre: {autor.Nombre} {autor.Apellido}");
                Console.WriteLine($"Nacionalidad: {autor.Nacionalidad}");
            }
            else
            {
                Console.WriteLine($"\nERROR: No se encontró un autor con ID {idAutor}");
            }
        }

        public static void ActualizarAutor()
        {
            Console.WriteLine("\n--- ACTUALIZAR AUTOR ---\n");

            int idAutor = Validaciones.ValidarNumeroEntero("Ingrese el ID del autor a actualizar: ", 1);
            var autores = ManejoArchivos.CargarDatos<Autor>(ARCHIVO_AUTORES);
            var autor = ManejoArchivos.BuscarPorId(autores, idAutor, a => a.Id);

            if (autor != null)
            {
                Console.WriteLine($"\nAutor actual: {autor.Nombre} {autor.Apellido}");
                Console.WriteLine("\nIngrese los nuevos datos (Enter para mantener el actual):");

                Console.Write($"Nombre [{autor.Nombre}]: ");
                string nuevoNombre = Console.ReadLine()?.Trim();
                if (!string.IsNullOrEmpty(nuevoNombre))
                {
                    if (nuevoNombre.Length >= 2 && nuevoNombre.Length <= 50)
                    {
                        autor.Nombre = nuevoNombre;
                    }
                    else
                    {
                        Console.WriteLine("ERROR: El nombre debe tener entre 2 y 50 caracteres.");
                    }
                }

                Console.Write($"Apellido [{autor.Apellido}]: ");
                string nuevoApellido = Console.ReadLine()?.Trim();
                if (!string.IsNullOrEmpty(nuevoApellido))
                {
                    if (nuevoApellido.Length >= 2 && nuevoApellido.Length <= 50)
                    {
                        autor.Apellido = nuevoApellido;
                    }
                    else
                    {
                        Console.WriteLine("ERROR: El apellido debe tener entre 2 y 50 caracteres.");
                    }
                }

                Console.Write($"Nacionalidad [{autor.Nacionalidad}]: ");
                string nuevaNacionalidad = Console.ReadLine()?.Trim();
                if (!string.IsNullOrEmpty(nuevaNacionalidad))
                {
                    if (nuevaNacionalidad.Length >= 2 && nuevaNacionalidad.Length <= 30)
                    {
                        autor.Nacionalidad = nuevaNacionalidad;
                    }
                    else
                    {
                        Console.WriteLine("ERROR: La nacionalidad debe tener entre 2 y 30 caracteres.");
                    }
                }

                ManejoArchivos.GuardarDatos(ARCHIVO_AUTORES, autores);
                Console.WriteLine("\nAutor actualizado exitosamente.");
            }
            else
            {
                Console.WriteLine($"\nERROR: No se encontró un autor con ID {idAutor}");
            }
        }

        public static void EliminarAutor()
        {
            Console.WriteLine("\n--- ELIMINAR AUTOR ---\n");

            int idAutor = Validaciones.ValidarNumeroEntero("Ingrese el ID del autor a eliminar: ", 1);
            var autores = ManejoArchivos.CargarDatos<Autor>(ARCHIVO_AUTORES);

            if (ManejoArchivos.EliminarPorId(autores, idAutor, a => a.Id))
            {
                ManejoArchivos.GuardarDatos(ARCHIVO_AUTORES, autores);
                Console.WriteLine($"\nAutor con ID {idAutor} eliminado exitosamente.");
            }
            else
            {
                Console.WriteLine($"\nERROR: No se encontró un autor con ID {idAutor}");
            }
        }

        public static void MenuAutores()
        {
            while (true)
            {
                Validaciones.LimpiarPantalla();
                Console.WriteLine(new string('=', 50));
                Console.WriteLine("  GESTIÓN DE AUTORES".PadLeft(35));
                Console.WriteLine(new string('=', 50));
                Console.WriteLine("\n1. Registrar nuevo autor");
                Console.WriteLine("2. Listar todos los autores");
                Console.WriteLine("3. Buscar autor");
                Console.WriteLine("4. Actualizar autor");
                Console.WriteLine("5. Eliminar autor");
                Console.WriteLine("0. Volver al menú principal");
                Console.WriteLine(new string('=', 50));

                Console.Write("\nSeleccione una opción: ");
                string opcion = Console.ReadLine()?.Trim();

                switch (opcion)
                {
                    case "1":
                        CrearAutor();
                        Validaciones.Pausar();
                        break;
                    case "2":
                        ListarAutores();
                        Validaciones.Pausar();
                        break;
                    case "3":
                        BuscarAutor();
                        Validaciones.Pausar();
                        break;
                    case "4":
                        ActualizarAutor();
                        Validaciones.Pausar();
                        break;
                    case "5":
                        EliminarAutor();
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
