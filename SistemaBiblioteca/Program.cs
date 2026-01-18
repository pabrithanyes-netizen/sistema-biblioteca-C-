using System;
using SistemaBiblioteca.Models;
using SistemaBiblioteca.Utils;

namespace SistemaBiblioteca
{
    /// <summary>
    /// Sistema de Gestión de Biblioteca
    /// Archivo principal del programa
    /// Fecha: 2026-01-17
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            MenuPrincipal();
        }

        /// <summary>
        /// Función principal que muestra el menú del sistema
        /// Permite navegar entre los diferentes módulos
        /// </summary>
        static void MenuPrincipal()
        {
            while (true)
            {
                Validaciones.LimpiarPantalla();
                Console.WriteLine(new string('=', 50));
                Console.WriteLine("  SISTEMA DE GESTIÓN DE BIBLIOTECA".PadLeft(42));
                Console.WriteLine(new string('=', 50));
                Console.WriteLine("  [Almacenamiento: JSON Local]".PadLeft(40));
                Console.WriteLine(new string('=', 50));

                Console.WriteLine("\n1. Gestión de Libros");
                Console.WriteLine("2. Gestión de Usuarios");
                Console.WriteLine("3. Gestión de Préstamos");
                Console.WriteLine("4. Gestión de Multas");
                Console.WriteLine("5. Gestión de Categorías");
                Console.WriteLine("6. Gestión de Autores");
                Console.WriteLine("0. Salir del Sistema");
                Console.WriteLine(new string('=', 50));

                Console.Write("\nSeleccione una opción: ");
                string opcion = Console.ReadLine()?.Trim();

                switch (opcion)
                {
                    case "1":
                        LibroService.MenuLibros();
                        break;
                    case "2":
                        UsuarioService.MenuUsuarios();
                        break;
                    case "3":
                        PrestamoService.MenuPrestamos();
                        break;
                    case "4":
                        MultaService.MenuMultas();
                        break;
                    case "5":
                        CategoriaService.MenuCategorias();
                        break;
                    case "6":
                        AutorService.MenuAutores();
                        break;
                    case "0":
                        Console.WriteLine("\n¡Gracias por usar el sistema! Hasta pronto.");
                        return;
                    default:
                        Console.WriteLine("\nERROR: Opción inválida. Intente nuevamente.");
                        Validaciones.Pausar();
                        break;
                }
            }
        }
    }
}
