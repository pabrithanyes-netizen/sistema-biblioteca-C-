using System;
using System.Collections.Generic;
using SistemaBiblioteca.Utils;

namespace SistemaBiblioteca.Models
{
    /// <summary>
    /// Modelo de Usuario
    /// </summary>
    public class Usuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public string Direccion { get; set; }
        public bool Activo { get; set; }
        public int MultasPendientes { get; set; }
    }

    /// <summary>
    /// Módulo de gestión de Usuarios
    /// Maneja el CRUD de usuarios de la biblioteca
    /// </summary>
    public static class UsuarioService
    {
        public const string ARCHIVO_USUARIOS = "usuarios";
        private const string CONTADOR_USUARIOS = "usuarios";

        public static Usuario CrearUsuario()
        {
            Console.WriteLine("\n--- REGISTRAR NUEVO USUARIO ---\n");

            string nombre = Validaciones.ValidarTexto("Nombre: ", 2, 50);
            string apellido = Validaciones.ValidarTexto("Apellido: ", 2, 50);
            string email = Validaciones.ValidarEmail("Correo electrónico: ");
            string telefono = Validaciones.ValidarTelefono("Teléfono: ");
            string direccion = Validaciones.ValidarTexto("Dirección: ", 5, 100);

            var usuario = new Usuario
            {
                Id = ManejoArchivos.ObtenerSiguienteId(CONTADOR_USUARIOS),
                Nombre = nombre,
                Apellido = apellido,
                Email = email,
                Telefono = telefono,
                Direccion = direccion,
                Activo = true,
                MultasPendientes = 0
            };

            var usuarios = ManejoArchivos.CargarDatos<Usuario>(ARCHIVO_USUARIOS);
            usuarios.Add(usuario);
            ManejoArchivos.GuardarDatos(ARCHIVO_USUARIOS, usuarios);

            Console.WriteLine($"\nUsuario registrado exitosamente con ID: {usuario.Id}");
            return usuario;
        }

        public static void ListarUsuarios()
        {
            var usuarios = ManejoArchivos.CargarDatos<Usuario>(ARCHIVO_USUARIOS);

            Console.WriteLine("\n--- LISTA DE USUARIOS ---\n");

            if (usuarios.Count == 0)
            {
                Console.WriteLine("No hay usuarios registrados.");
            }
            else
            {
                Console.WriteLine($"{"ID",-5} {"Nombre",-25} {"Email",-30} {"Teléfono",-15}");
                Console.WriteLine(new string('-', 80));
                foreach (var usuario in usuarios)
                {
                    string nombreCompleto = $"{usuario.Nombre} {usuario.Apellido}";
                    string nombre = nombreCompleto.Length > 25
                        ? nombreCompleto.Substring(0, 22) + "..."
                        : nombreCompleto;
                    Console.WriteLine($"{usuario.Id,-5} {nombre,-25} {usuario.Email,-30} {usuario.Telefono,-15}");
                }
            }

            Console.WriteLine($"\nTotal de usuarios: {usuarios.Count}");
        }

        public static void BuscarUsuario()
        {
            Console.WriteLine("\n--- BUSCAR USUARIO ---\n");

            int idUsuario = Validaciones.ValidarNumeroEntero("Ingrese el ID del usuario: ", 1);
            var usuarios = ManejoArchivos.CargarDatos<Usuario>(ARCHIVO_USUARIOS);
            var usuario = ManejoArchivos.BuscarPorId(usuarios, idUsuario, u => u.Id);

            if (usuario != null)
            {
                Console.WriteLine("\nUsuario encontrado:");
                Console.WriteLine($"ID: {usuario.Id}");
                Console.WriteLine($"Nombre: {usuario.Nombre} {usuario.Apellido}");
                Console.WriteLine($"Email: {usuario.Email}");
                Console.WriteLine($"Teléfono: {usuario.Telefono}");
                Console.WriteLine($"Dirección: {usuario.Direccion}");
                Console.WriteLine($"Estado: {(usuario.Activo ? "Activo" : "Inactivo")}");
                Console.WriteLine($"Multas pendientes: {usuario.MultasPendientes}");
            }
            else
            {
                Console.WriteLine($"\nERROR: No se encontró un usuario con ID {idUsuario}");
            }
        }

        public static void ActualizarUsuario()
        {
            Console.WriteLine("\n--- ACTUALIZAR USUARIO ---\n");

            int idUsuario = Validaciones.ValidarNumeroEntero("Ingrese el ID del usuario a actualizar: ", 1);
            var usuarios = ManejoArchivos.CargarDatos<Usuario>(ARCHIVO_USUARIOS);
            var usuario = ManejoArchivos.BuscarPorId(usuarios, idUsuario, u => u.Id);

            if (usuario != null)
            {
                Console.WriteLine($"\nUsuario actual: {usuario.Nombre} {usuario.Apellido}");
                Console.WriteLine("\nIngrese los nuevos datos (Enter para mantener el actual):");

                Console.Write($"Nombre [{usuario.Nombre}]: ");
                string nuevoNombre = Console.ReadLine()?.Trim();
                if (!string.IsNullOrEmpty(nuevoNombre))
                {
                    if (nuevoNombre.Length >= 2 && nuevoNombre.Length <= 50)
                    {
                        usuario.Nombre = nuevoNombre;
                    }
                    else
                    {
                        Console.WriteLine("ERROR: El nombre debe tener entre 2 y 50 caracteres.");
                    }
                }

                Console.Write($"Apellido [{usuario.Apellido}]: ");
                string nuevoApellido = Console.ReadLine()?.Trim();
                if (!string.IsNullOrEmpty(nuevoApellido))
                {
                    if (nuevoApellido.Length >= 2 && nuevoApellido.Length <= 50)
                    {
                        usuario.Apellido = nuevoApellido;
                    }
                    else
                    {
                        Console.WriteLine("ERROR: El apellido debe tener entre 2 y 50 caracteres.");
                    }
                }

                Console.Write($"Email [{usuario.Email}]: ");
                string nuevoEmail = Console.ReadLine()?.Trim();
                if (!string.IsNullOrEmpty(nuevoEmail))
                {
                    usuario.Email = nuevoEmail;
                }

                Console.Write($"Teléfono [{usuario.Telefono}]: ");
                string nuevoTelefono = Console.ReadLine()?.Trim();
                if (!string.IsNullOrEmpty(nuevoTelefono))
                {
                    usuario.Telefono = nuevoTelefono;
                }

                Console.Write($"Dirección [{usuario.Direccion}]: ");
                string nuevaDireccion = Console.ReadLine()?.Trim();
                if (!string.IsNullOrEmpty(nuevaDireccion))
                {
                    if (nuevaDireccion.Length >= 5 && nuevaDireccion.Length <= 100)
                    {
                        usuario.Direccion = nuevaDireccion;
                    }
                    else
                    {
                        Console.WriteLine("ERROR: La dirección debe tener entre 5 y 100 caracteres.");
                    }
                }

                ManejoArchivos.GuardarDatos(ARCHIVO_USUARIOS, usuarios);
                Console.WriteLine("\nUsuario actualizado exitosamente.");
            }
            else
            {
                Console.WriteLine($"\nERROR: No se encontró un usuario con ID {idUsuario}");
            }
        }

        public static void EliminarUsuario()
        {
            Console.WriteLine("\n--- ELIMINAR USUARIO ---\n");

            int idUsuario = Validaciones.ValidarNumeroEntero("Ingrese el ID del usuario a eliminar: ", 1);
            var usuarios = ManejoArchivos.CargarDatos<Usuario>(ARCHIVO_USUARIOS);
            var usuario = ManejoArchivos.BuscarPorId(usuarios, idUsuario, u => u.Id);

            if (usuario != null)
            {
                usuario.Activo = false;
                ManejoArchivos.GuardarDatos(ARCHIVO_USUARIOS, usuarios);
                Console.WriteLine($"\nUsuario con ID {idUsuario} desactivado exitosamente.");
            }
            else
            {
                Console.WriteLine($"\nERROR: No se encontró un usuario con ID {idUsuario}");
            }
        }

        public static void MenuUsuarios()
        {
            while (true)
            {
                Validaciones.LimpiarPantalla();
                Console.WriteLine(new string('=', 50));
                Console.WriteLine("  GESTIÓN DE USUARIOS".PadLeft(36));
                Console.WriteLine(new string('=', 50));
                Console.WriteLine("\n1. Registrar nuevo usuario");
                Console.WriteLine("2. Listar todos los usuarios");
                Console.WriteLine("3. Buscar usuario");
                Console.WriteLine("4. Actualizar usuario");
                Console.WriteLine("5. Eliminar usuario");
                Console.WriteLine("0. Volver al menú principal");
                Console.WriteLine(new string('=', 50));

                Console.Write("\nSeleccione una opción: ");
                string opcion = Console.ReadLine()?.Trim();

                switch (opcion)
                {
                    case "1":
                        CrearUsuario();
                        Validaciones.Pausar();
                        break;
                    case "2":
                        ListarUsuarios();
                        Validaciones.Pausar();
                        break;
                    case "3":
                        BuscarUsuario();
                        Validaciones.Pausar();
                        break;
                    case "4":
                        ActualizarUsuario();
                        Validaciones.Pausar();
                        break;
                    case "5":
                        EliminarUsuario();
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
