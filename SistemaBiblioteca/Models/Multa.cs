using System;
using System.Collections.Generic;
using System.Linq;
using SistemaBiblioteca.Utils;

namespace SistemaBiblioteca.Models
{
    /// <summary>
    /// Modelo de Multa
    /// </summary>
    public class Multa
    {
        public int Id { get; set; }
        public int IdUsuario { get; set; }
        public decimal Monto { get; set; }
        public string Concepto { get; set; }
        public string FechaGeneracion { get; set; }
        public string FechaPago { get; set; }
        public string Estado { get; set; } // pendiente, pagada
    }

    /// <summary>
    /// Módulo de gestión de Multas
    /// Maneja las transacciones de multas por retrasos
    /// </summary>
    public static class MultaService
    {
        public const string ARCHIVO_MULTAS = "multas";
        private const string CONTADOR_MULTAS = "multas";

        public static Multa CrearMultaAutomatica(int idUsuario, decimal monto, string concepto)
        {
            var multa = new Multa
            {
                Id = ManejoArchivos.ObtenerSiguienteId(CONTADOR_MULTAS),
                IdUsuario = idUsuario,
                Monto = Math.Round(monto, 2),
                Concepto = concepto,
                FechaGeneracion = DateTime.Now.ToString("dd/MM/yyyy"),
                FechaPago = null,
                Estado = "pendiente"
            };

            // Guardar multa
            var multas = ManejoArchivos.CargarDatos<Multa>(ARCHIVO_MULTAS);
            multas.Add(multa);
            ManejoArchivos.GuardarDatos(ARCHIVO_MULTAS, multas);

            // Actualizar contador de multas del usuario
            var usuarios = ManejoArchivos.CargarDatos<Usuario>(UsuarioService.ARCHIVO_USUARIOS);
            var usuario = ManejoArchivos.BuscarPorId(usuarios, idUsuario, u => u.Id);
            if (usuario != null)
            {
                usuario.MultasPendientes = usuario.MultasPendientes + 1;
                ManejoArchivos.GuardarDatos(UsuarioService.ARCHIVO_USUARIOS, usuarios);
            }

            return multa;
        }

        public static Multa CrearMultaManual()
        {
            Console.WriteLine("\n--- REGISTRAR NUEVA MULTA ---\n");

            int idUsuario = Validaciones.ValidarNumeroEntero("ID del usuario: ", 1);

            // Verificar que el usuario existe
            var usuarios = ManejoArchivos.CargarDatos<Usuario>(UsuarioService.ARCHIVO_USUARIOS);
            var usuario = ManejoArchivos.BuscarPorId(usuarios, idUsuario, u => u.Id);

            if (usuario == null)
            {
                Console.WriteLine("\nERROR: Usuario no encontrado.");
                return null;
            }

            decimal monto = Validaciones.ValidarNumeroDecimal("Monto de la multa: $", 0.01m, 10000.00m);
            Console.Write("Concepto de la multa: ");
            string concepto = Console.ReadLine()?.Trim();

            return CrearMultaAutomatica(idUsuario, monto, concepto);
        }

        public static void PagarMulta()
        {
            Console.WriteLine("\n--- PAGAR MULTA ---\n");

            int idMulta = Validaciones.ValidarNumeroEntero("ID de la multa: ", 1);
            var multas = ManejoArchivos.CargarDatos<Multa>(ARCHIVO_MULTAS);
            var multa = ManejoArchivos.BuscarPorId(multas, idMulta, m => m.Id);

            if (multa == null)
            {
                Console.WriteLine($"\nERROR: No se encontró una multa con ID {idMulta}");
                return;
            }

            if (multa.Estado == "pagada")
            {
                Console.WriteLine("\nERROR: Esta multa ya fue pagada.");
                return;
            }

            // Registrar pago
            multa.FechaPago = DateTime.Now.ToString("dd/MM/yyyy");
            multa.Estado = "pagada";
            ManejoArchivos.GuardarDatos(ARCHIVO_MULTAS, multas);

            // Actualizar contador de multas del usuario
            var usuarios = ManejoArchivos.CargarDatos<Usuario>(UsuarioService.ARCHIVO_USUARIOS);
            var usuario = ManejoArchivos.BuscarPorId(usuarios, multa.IdUsuario, u => u.Id);
            if (usuario != null)
            {
                usuario.MultasPendientes = Math.Max(0, usuario.MultasPendientes - 1);
                ManejoArchivos.GuardarDatos(UsuarioService.ARCHIVO_USUARIOS, usuarios);
            }

            Console.WriteLine($"\nMulta pagada exitosamente. Monto: ${multa.Monto:F2}");
        }

        public static void ListarMultas()
        {
            var multas = ManejoArchivos.CargarDatos<Multa>(ARCHIVO_MULTAS);

            Console.WriteLine("\n--- LISTA DE MULTAS ---\n");

            if (multas.Count == 0)
            {
                Console.WriteLine("No hay multas registradas.");
            }
            else
            {
                Console.WriteLine($"{"ID",-5} {"Usuario",-10} {"Monto",-10} {"Fecha Gen.",-15} {"Estado",-10}");
                Console.WriteLine(new string('-', 55));
                foreach (var multa in multas)
                {
                    string monto = $"${multa.Monto:F2}";
                    Console.WriteLine($"{multa.Id,-5} {multa.IdUsuario,-10} {monto,-10} {multa.FechaGeneracion,-15} {multa.Estado,-10}");
                }
            }

            Console.WriteLine($"\nTotal de multas: {multas.Count}");
        }

        public static void ListarMultasPendientes()
        {
            var multas = ManejoArchivos.CargarDatos<Multa>(ARCHIVO_MULTAS);
            var pendientes = multas.Where(m => m.Estado == "pendiente").ToList();

            Console.WriteLine("\n--- MULTAS PENDIENTES ---\n");

            if (pendientes.Count == 0)
            {
                Console.WriteLine("No hay multas pendientes.");
            }
            else
            {
                Console.WriteLine($"{"ID",-5} {"Usuario",-10} {"Monto",-10} {"Concepto",-30}");
                Console.WriteLine(new string('-', 60));
                foreach (var multa in pendientes)
                {
                    string monto = $"${multa.Monto:F2}";
                    string concepto = multa.Concepto.Length > 30
                        ? multa.Concepto.Substring(0, 27) + "..."
                        : multa.Concepto;
                    Console.WriteLine($"{multa.Id,-5} {multa.IdUsuario,-10} {monto,-10} {concepto,-30}");
                }

                decimal total = pendientes.Sum(m => m.Monto);
                Console.WriteLine($"\nTotal de multas pendientes: {pendientes.Count} - Monto total: ${total:F2}");
            }
        }

        public static void BuscarMulta()
        {
            Console.WriteLine("\n--- BUSCAR MULTA ---\n");

            int idMulta = Validaciones.ValidarNumeroEntero("Ingrese el ID de la multa: ", 1);
            var multas = ManejoArchivos.CargarDatos<Multa>(ARCHIVO_MULTAS);
            var multa = ManejoArchivos.BuscarPorId(multas, idMulta, m => m.Id);

            if (multa != null)
            {
                Console.WriteLine("\nMulta encontrada:");
                Console.WriteLine($"ID: {multa.Id}");
                Console.WriteLine($"ID Usuario: {multa.IdUsuario}");
                Console.WriteLine($"Monto: ${multa.Monto:F2}");
                Console.WriteLine($"Concepto: {multa.Concepto}");
                Console.WriteLine($"Fecha de generación: {multa.FechaGeneracion}");
                Console.WriteLine($"Fecha de pago: {multa.FechaPago ?? "No pagada"}");
                Console.WriteLine($"Estado: {multa.Estado}");
            }
            else
            {
                Console.WriteLine($"\nERROR: No se encontró una multa con ID {idMulta}");
            }
        }

        public static void MenuMultas()
        {
            while (true)
            {
                Validaciones.LimpiarPantalla();
                Console.WriteLine(new string('=', 50));
                Console.WriteLine("  GESTIÓN DE MULTAS".PadLeft(34));
                Console.WriteLine(new string('=', 50));
                Console.WriteLine("\n1. Registrar nueva multa");
                Console.WriteLine("2. Pagar multa");
                Console.WriteLine("3. Listar todas las multas");
                Console.WriteLine("4. Listar multas pendientes");
                Console.WriteLine("5. Buscar multa");
                Console.WriteLine("0. Volver al menú principal");
                Console.WriteLine(new string('=', 50));

                Console.Write("\nSeleccione una opción: ");
                string opcion = Console.ReadLine()?.Trim();

                switch (opcion)
                {
                    case "1":
                        CrearMultaManual();
                        Validaciones.Pausar();
                        break;
                    case "2":
                        PagarMulta();
                        Validaciones.Pausar();
                        break;
                    case "3":
                        ListarMultas();
                        Validaciones.Pausar();
                        break;
                    case "4":
                        ListarMultasPendientes();
                        Validaciones.Pausar();
                        break;
                    case "5":
                        BuscarMulta();
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
