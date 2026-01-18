using System;
using System.Collections.Generic;
using System.Linq;
using SistemaBiblioteca.Utils;

namespace SistemaBiblioteca.Models
{
    /// <summary>
    /// Modelo de Préstamo
    /// </summary>
    public class Prestamo
    {
        public int Id { get; set; }
        public int IdUsuario { get; set; }
        public int IdLibro { get; set; }
        public string FechaPrestamo { get; set; }
        public string FechaDevolucionEsperada { get; set; }
        public string FechaDevolucionReal { get; set; }
        public string Estado { get; set; } // activo, devuelto, vencido
        public bool MultaGenerada { get; set; }
    }

    /// <summary>
    /// Módulo de gestión de Préstamos
    /// Maneja las transacciones de préstamos de libros
    /// </summary>
    public static class PrestamoService
    {
        private const string ARCHIVO_PRESTAMOS = "prestamos";
        private const string CONTADOR_PRESTAMOS = "prestamos";

        public static Prestamo CrearPrestamo()
        {
            Console.WriteLine("\n--- REGISTRAR NUEVO PRÉSTAMO ---\n");

            int idUsuario = Validaciones.ValidarNumeroEntero("ID del usuario: ", 1);
            int idLibro = Validaciones.ValidarNumeroEntero("ID del libro: ", 1);

            // Verificar que el usuario existe y está activo
            var usuarios = ManejoArchivos.CargarDatos<Usuario>(UsuarioService.ARCHIVO_USUARIOS);
            var usuario = ManejoArchivos.BuscarPorId(usuarios, idUsuario, u => u.Id);

            if (usuario == null || !usuario.Activo)
            {
                Console.WriteLine("\nERROR: Usuario no encontrado o inactivo.");
                return null;
            }

            // Verificar que el usuario no tenga multas pendientes
            if (usuario.MultasPendientes > 0)
            {
                Console.WriteLine($"\nERROR: El usuario tiene {usuario.MultasPendientes} multas pendientes. Debe pagarlas primero.");
                return null;
            }

            // Verificar que el libro existe y tiene copias disponibles
            var libros = ManejoArchivos.CargarDatos<Libro>(LibroService.ARCHIVO_LIBROS);
            var libro = ManejoArchivos.BuscarPorId(libros, idLibro, l => l.Id);

            if (libro == null || !libro.Activo)
            {
                Console.WriteLine("\nERROR: Libro no encontrado o inactivo.");
                return null;
            }

            if (libro.CopiasDisponibles <= 0)
            {
                Console.WriteLine("\nERROR: No hay copias disponibles de este libro.");
                return null;
            }

            // Calcular fechas
            string fechaPrestamo = DateTime.Now.ToString("dd/MM/yyyy");
            string fechaDevolucion = DateTime.Now.AddDays(14).ToString("dd/MM/yyyy");

            var prestamo = new Prestamo
            {
                Id = ManejoArchivos.ObtenerSiguienteId(CONTADOR_PRESTAMOS),
                IdUsuario = idUsuario,
                IdLibro = idLibro,
                FechaPrestamo = fechaPrestamo,
                FechaDevolucionEsperada = fechaDevolucion,
                FechaDevolucionReal = null,
                Estado = "activo",
                MultaGenerada = false
            };

            // Actualizar libro (reducir copias disponibles)
            libro.CopiasDisponibles -= 1;
            ManejoArchivos.GuardarDatos(LibroService.ARCHIVO_LIBROS, libros);

            // Guardar préstamo
            var prestamos = ManejoArchivos.CargarDatos<Prestamo>(ARCHIVO_PRESTAMOS);
            prestamos.Add(prestamo);
            ManejoArchivos.GuardarDatos(ARCHIVO_PRESTAMOS, prestamos);

            Console.WriteLine($"\nPréstamo registrado exitosamente con ID: {prestamo.Id}");
            Console.WriteLine($"Fecha de devolución esperada: {fechaDevolucion}");
            return prestamo;
        }

        public static void DevolverLibro()
        {
            Console.WriteLine("\n--- DEVOLVER LIBRO ---\n");

            int idPrestamo = Validaciones.ValidarNumeroEntero("ID del préstamo: ", 1);
            var prestamos = ManejoArchivos.CargarDatos<Prestamo>(ARCHIVO_PRESTAMOS);
            var prestamo = ManejoArchivos.BuscarPorId(prestamos, idPrestamo, p => p.Id);

            if (prestamo == null)
            {
                Console.WriteLine($"\nERROR: No se encontró un préstamo con ID {idPrestamo}");
                return;
            }

            if (prestamo.Estado == "devuelto")
            {
                Console.WriteLine("\nERROR: Este préstamo ya fue devuelto.");
                return;
            }

            // Registrar fecha de devolución
            string fechaDevolucion = DateTime.Now.ToString("dd/MM/yyyy");
            prestamo.FechaDevolucionReal = fechaDevolucion;
            prestamo.Estado = "devuelto";

            // Verificar si hay multa por retraso
            DateTime fechaEsperada = DateTime.ParseExact(prestamo.FechaDevolucionEsperada, "dd/MM/yyyy", null);
            DateTime fechaReal = DateTime.Now;
            int diasRetraso = (fechaReal - fechaEsperada).Days;

            if (diasRetraso > 0)
            {
                // Generar multa
                decimal montoMulta = diasRetraso * 1.0m; // $1 por día de retraso
                MultaService.CrearMultaAutomatica(prestamo.IdUsuario, montoMulta,
                    $"Retraso de {diasRetraso} días en préstamo #{idPrestamo}");
                prestamo.MultaGenerada = true;
                Console.WriteLine($"\nADVERTENCIA: Devolución con {diasRetraso} días de retraso.");
                Console.WriteLine($"Se generó una multa de ${montoMulta:F2}");
            }

            // Actualizar libro (aumentar copias disponibles)
            var libros = ManejoArchivos.CargarDatos<Libro>(LibroService.ARCHIVO_LIBROS);
            var libro = ManejoArchivos.BuscarPorId(libros, prestamo.IdLibro, l => l.Id);
            if (libro != null)
            {
                libro.CopiasDisponibles += 1;
                ManejoArchivos.GuardarDatos(LibroService.ARCHIVO_LIBROS, libros);
            }

            // Guardar cambios
            ManejoArchivos.GuardarDatos(ARCHIVO_PRESTAMOS, prestamos);
            Console.WriteLine("\nLibro devuelto exitosamente.");
        }

        public static void ListarPrestamos()
        {
            var prestamos = ManejoArchivos.CargarDatos<Prestamo>(ARCHIVO_PRESTAMOS);

            Console.WriteLine("\n--- LISTA DE PRÉSTAMOS ---\n");

            if (prestamos.Count == 0)
            {
                Console.WriteLine("No hay préstamos registrados.");
            }
            else
            {
                Console.WriteLine($"{"ID",-5} {"Usuario",-10} {"Libro",-10} {"Fecha Préstamo",-15} {"Estado",-10}");
                Console.WriteLine(new string('-', 55));
                foreach (var prestamo in prestamos)
                {
                    Console.WriteLine($"{prestamo.Id,-5} {prestamo.IdUsuario,-10} {prestamo.IdLibro,-10} {prestamo.FechaPrestamo,-15} {prestamo.Estado,-10}");
                }
            }

            Console.WriteLine($"\nTotal de préstamos: {prestamos.Count}");
        }

        public static void ListarPrestamosActivos()
        {
            var prestamos = ManejoArchivos.CargarDatos<Prestamo>(ARCHIVO_PRESTAMOS);
            var activos = prestamos.Where(p => p.Estado == "activo").ToList();

            Console.WriteLine("\n--- PRÉSTAMOS ACTIVOS ---\n");

            if (activos.Count == 0)
            {
                Console.WriteLine("No hay préstamos activos.");
            }
            else
            {
                Console.WriteLine($"{"ID",-5} {"Usuario",-10} {"Libro",-10} {"Fecha Préstamo",-15} {"Devolución",-15}");
                Console.WriteLine(new string('-', 60));
                foreach (var prestamo in activos)
                {
                    Console.WriteLine($"{prestamo.Id,-5} {prestamo.IdUsuario,-10} {prestamo.IdLibro,-10} {prestamo.FechaPrestamo,-15} {prestamo.FechaDevolucionEsperada,-15}");
                }
            }

            Console.WriteLine($"\nTotal de préstamos activos: {activos.Count}");
        }

        public static void BuscarPrestamo()
        {
            Console.WriteLine("\n--- BUSCAR PRÉSTAMO ---\n");

            int idPrestamo = Validaciones.ValidarNumeroEntero("Ingrese el ID del préstamo: ", 1);
            var prestamos = ManejoArchivos.CargarDatos<Prestamo>(ARCHIVO_PRESTAMOS);
            var prestamo = ManejoArchivos.BuscarPorId(prestamos, idPrestamo, p => p.Id);

            if (prestamo != null)
            {
                Console.WriteLine("\nPréstamo encontrado:");
                Console.WriteLine($"ID: {prestamo.Id}");
                Console.WriteLine($"ID Usuario: {prestamo.IdUsuario}");
                Console.WriteLine($"ID Libro: {prestamo.IdLibro}");
                Console.WriteLine($"Fecha de préstamo: {prestamo.FechaPrestamo}");
                Console.WriteLine($"Fecha de devolución esperada: {prestamo.FechaDevolucionEsperada}");
                Console.WriteLine($"Fecha de devolución real: {prestamo.FechaDevolucionReal ?? "No devuelto"}");
                Console.WriteLine($"Estado: {prestamo.Estado}");
                Console.WriteLine($"Multa generada: {(prestamo.MultaGenerada ? "Sí" : "No")}");
            }
            else
            {
                Console.WriteLine($"\nERROR: No se encontró un préstamo con ID {idPrestamo}");
            }
        }

        public static void MenuPrestamos()
        {
            while (true)
            {
                Validaciones.LimpiarPantalla();
                Console.WriteLine(new string('=', 50));
                Console.WriteLine("  GESTIÓN DE PRÉSTAMOS".PadLeft(36));
                Console.WriteLine(new string('=', 50));
                Console.WriteLine("\n1. Registrar nuevo préstamo");
                Console.WriteLine("2. Devolver libro");
                Console.WriteLine("3. Listar todos los préstamos");
                Console.WriteLine("4. Listar préstamos activos");
                Console.WriteLine("5. Buscar préstamo");
                Console.WriteLine("0. Volver al menú principal");
                Console.WriteLine(new string('=', 50));

                Console.Write("\nSeleccione una opción: ");
                string opcion = Console.ReadLine()?.Trim();

                switch (opcion)
                {
                    case "1":
                        CrearPrestamo();
                        Validaciones.Pausar();
                        break;
                    case "2":
                        DevolverLibro();
                        Validaciones.Pausar();
                        break;
                    case "3":
                        ListarPrestamos();
                        Validaciones.Pausar();
                        break;
                    case "4":
                        ListarPrestamosActivos();
                        Validaciones.Pausar();
                        break;
                    case "5":
                        BuscarPrestamo();
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
