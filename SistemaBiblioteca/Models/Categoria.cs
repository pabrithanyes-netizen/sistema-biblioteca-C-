using System;
using System.Collections.Generic;
using SistemaBiblioteca.Utils;

namespace SistemaBiblioteca.Models
{
    /// <summary>
    /// Modelo de Categoría
    /// </summary>
    public class Categoria
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
    }

    /// <summary>
    /// Módulo de gestión de Categorías
    /// Maneja el CRUD de categorías de libros
    /// </summary>
    public static class CategoriaService
    {
        private const string ARCHIVO_CATEGORIAS = "categorias";
        private const string CONTADOR_CATEGORIAS = "categorias";

        public static Categoria CrearCategoria()
        {
            Console.WriteLine("\n--- REGISTRAR NUEVA CATEGORÍA ---\n");

            string nombre = Validaciones.ValidarTexto("Nombre de la categoría: ", 3, 50);
            string descripcion = Validaciones.ValidarTexto("Descripción: ", 5, 200);

            var categoria = new Categoria
            {
                Id = ManejoArchivos.ObtenerSiguienteId(CONTADOR_CATEGORIAS),
                Nombre = nombre,
                Descripcion = descripcion
            };

            var categorias = ManejoArchivos.CargarDatos<Categoria>(ARCHIVO_CATEGORIAS);
            categorias.Add(categoria);
            ManejoArchivos.GuardarDatos(ARCHIVO_CATEGORIAS, categorias);

            Console.WriteLine($"\nCategoría registrada exitosamente con ID: {categoria.Id}");
            return categoria;
        }

        public static void ListarCategorias()
        {
            var categorias = ManejoArchivos.CargarDatos<Categoria>(ARCHIVO_CATEGORIAS);

            Console.WriteLine("\n--- LISTA DE CATEGORÍAS ---\n");

            if (categorias.Count == 0)
            {
                Console.WriteLine("No hay categorías registradas.");
            }
            else
            {
                Console.WriteLine($"{"ID",-5} {"Nombre",-25} {"Descripción",-40}");
                Console.WriteLine(new string('-', 75));
                foreach (var cat in categorias)
                {
                    string desc = cat.Descripcion.Length > 40
                        ? cat.Descripcion.Substring(0, 37) + "..."
                        : cat.Descripcion;
                    Console.WriteLine($"{cat.Id,-5} {cat.Nombre,-25} {desc,-40}");
                }
            }

            Console.WriteLine($"\nTotal de categorías: {categorias.Count}");
        }

        public static void BuscarCategoria()
        {
            Console.WriteLine("\n--- BUSCAR CATEGORÍA ---\n");

            int idCategoria = Validaciones.ValidarNumeroEntero("Ingrese el ID de la categoría: ", 1);
            var categorias = ManejoArchivos.CargarDatos<Categoria>(ARCHIVO_CATEGORIAS);
            var categoria = ManejoArchivos.BuscarPorId(categorias, idCategoria, c => c.Id);

            if (categoria != null)
            {
                Console.WriteLine("\nCategoría encontrada:");
                Console.WriteLine($"ID: {categoria.Id}");
                Console.WriteLine($"Nombre: {categoria.Nombre}");
                Console.WriteLine($"Descripción: {categoria.Descripcion}");
            }
            else
            {
                Console.WriteLine($"\nERROR: No se encontró una categoría con ID {idCategoria}");
            }
        }

        public static void ActualizarCategoria()
        {
            Console.WriteLine("\n--- ACTUALIZAR CATEGORÍA ---\n");

            int idCategoria = Validaciones.ValidarNumeroEntero("Ingrese el ID de la categoría a actualizar: ", 1);
            var categorias = ManejoArchivos.CargarDatos<Categoria>(ARCHIVO_CATEGORIAS);
            var categoria = ManejoArchivos.BuscarPorId(categorias, idCategoria, c => c.Id);

            if (categoria != null)
            {
                Console.WriteLine($"\nCategoría actual: {categoria.Nombre}");
                Console.WriteLine("\nIngrese los nuevos datos (Enter para mantener el actual):");

                Console.Write($"Nombre [{categoria.Nombre}]: ");
                string nuevoNombre = Console.ReadLine()?.Trim();
                if (!string.IsNullOrEmpty(nuevoNombre))
                {
                    if (nuevoNombre.Length >= 3 && nuevoNombre.Length <= 50)
                    {
                        categoria.Nombre = nuevoNombre;
                    }
                    else
                    {
                        Console.WriteLine("ERROR: El nombre debe tener entre 3 y 50 caracteres.");
                    }
                }

                Console.Write($"Descripción [{categoria.Descripcion}]: ");
                string nuevaDescripcion = Console.ReadLine()?.Trim();
                if (!string.IsNullOrEmpty(nuevaDescripcion))
                {
                    if (nuevaDescripcion.Length >= 5 && nuevaDescripcion.Length <= 200)
                    {
                        categoria.Descripcion = nuevaDescripcion;
                    }
                    else
                    {
                        Console.WriteLine("ERROR: La descripción debe tener entre 5 y 200 caracteres.");
                    }
                }

                ManejoArchivos.GuardarDatos(ARCHIVO_CATEGORIAS, categorias);
                Console.WriteLine("\nCategoría actualizada exitosamente.");
            }
            else
            {
                Console.WriteLine($"\nERROR: No se encontró una categoría con ID {idCategoria}");
            }
        }

        public static void EliminarCategoria()
        {
            Console.WriteLine("\n--- ELIMINAR CATEGORÍA ---\n");

            int idCategoria = Validaciones.ValidarNumeroEntero("Ingrese el ID de la categoría a eliminar: ", 1);
            var categorias = ManejoArchivos.CargarDatos<Categoria>(ARCHIVO_CATEGORIAS);

            if (ManejoArchivos.EliminarPorId(categorias, idCategoria, c => c.Id))
            {
                ManejoArchivos.GuardarDatos(ARCHIVO_CATEGORIAS, categorias);
                Console.WriteLine($"\nCategoría con ID {idCategoria} eliminada exitosamente.");
            }
            else
            {
                Console.WriteLine($"\nERROR: No se encontró una categoría con ID {idCategoria}");
            }
        }

        public static void MenuCategorias()
        {
            while (true)
            {
                Validaciones.LimpiarPantalla();
                Console.WriteLine(new string('=', 50));
                Console.WriteLine("  GESTIÓN DE CATEGORÍAS".PadLeft(37));
                Console.WriteLine(new string('=', 50));
                Console.WriteLine("\n1. Registrar nueva categoría");
                Console.WriteLine("2. Listar todas las categorías");
                Console.WriteLine("3. Buscar categoría");
                Console.WriteLine("4. Actualizar categoría");
                Console.WriteLine("5. Eliminar categoría");
                Console.WriteLine("0. Volver al menú principal");
                Console.WriteLine(new string('=', 50));

                Console.Write("\nSeleccione una opción: ");
                string opcion = Console.ReadLine()?.Trim();

                switch (opcion)
                {
                    case "1":
                        CrearCategoria();
                        Validaciones.Pausar();
                        break;
                    case "2":
                        ListarCategorias();
                        Validaciones.Pausar();
                        break;
                    case "3":
                        BuscarCategoria();
                        Validaciones.Pausar();
                        break;
                    case "4":
                        ActualizarCategoria();
                        Validaciones.Pausar();
                        break;
                    case "5":
                        EliminarCategoria();
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
