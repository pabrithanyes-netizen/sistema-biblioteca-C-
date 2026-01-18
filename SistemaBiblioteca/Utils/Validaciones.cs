using System;
using System.Text.RegularExpressions;

namespace SistemaBiblioteca.Utils
{
    /// <summary>
    /// Módulo de validaciones
    /// Contiene funciones para validar entrada de datos del usuario
    /// </summary>
    public static class Validaciones
    {
        /// <summary>
        /// Limpia la pantalla de la consola
        /// </summary>
        public static void LimpiarPantalla()
        {
            Console.Clear();
        }

        /// <summary>
        /// Valida que la entrada sea texto válido
        /// </summary>
        public static string ValidarTexto(string mensaje, int minLongitud = 1, int maxLongitud = 100)
        {
            while (true)
            {
                Console.Write(mensaje);
                string texto = Console.ReadLine()?.Trim() ?? "";

                if (texto.Length < minLongitud)
                {
                    Console.WriteLine($"ERROR: El texto debe tener al menos {minLongitud} caracteres.");
                }
                else if (texto.Length > maxLongitud)
                {
                    Console.WriteLine($"ERROR: El texto no puede exceder {maxLongitud} caracteres.");
                }
                else if (!EsSoloLetrasYEspacios(texto))
                {
                    Console.WriteLine("ERROR: El texto solo debe contener letras y espacios.");
                }
                else
                {
                    return texto;
                }
            }
        }

        /// <summary>
        /// Valida que la entrada sea un número entero
        /// </summary>
        public static int ValidarNumeroEntero(string mensaje, int minimo = 0, int maximo = 999999)
        {
            while (true)
            {
                Console.Write(mensaje);
                string entrada = Console.ReadLine()?.Trim() ?? "";

                if (int.TryParse(entrada, out int numero))
                {
                    if (numero < minimo)
                    {
                        Console.WriteLine($"ERROR: El número debe ser mayor o igual a {minimo}.");
                    }
                    else if (numero > maximo)
                    {
                        Console.WriteLine($"ERROR: El número no puede ser mayor a {maximo}.");
                    }
                    else
                    {
                        return numero;
                    }
                }
                else
                {
                    Console.WriteLine("ERROR: Debe ingresar un número entero válido.");
                }
            }
        }

        /// <summary>
        /// Valida que la entrada sea un número decimal
        /// </summary>
        public static decimal ValidarNumeroDecimal(string mensaje, decimal minimo = 0.0m, decimal maximo = 999999.99m)
        {
            while (true)
            {
                Console.Write(mensaje);
                string entrada = Console.ReadLine()?.Trim() ?? "";

                if (decimal.TryParse(entrada, out decimal numero))
                {
                    if (numero < minimo)
                    {
                        Console.WriteLine($"ERROR: El número debe ser mayor o igual a {minimo}.");
                    }
                    else if (numero > maximo)
                    {
                        Console.WriteLine($"ERROR: El número no puede ser mayor a {maximo}.");
                    }
                    else
                    {
                        return Math.Round(numero, 2);
                    }
                }
                else
                {
                    Console.WriteLine("ERROR: Debe ingresar un número decimal válido.");
                }
            }
        }

        /// <summary>
        /// Valida que la entrada sea una fecha válida en formato DD/MM/AAAA
        /// </summary>
        public static string ValidarFecha(string mensaje)
        {
            while (true)
            {
                Console.Write(mensaje);
                string fechaStr = Console.ReadLine()?.Trim() ?? "";

                if (DateTime.TryParseExact(fechaStr, "dd/MM/yyyy", null,
                    System.Globalization.DateTimeStyles.None, out _))
                {
                    return fechaStr;
                }
                else
                {
                    Console.WriteLine("ERROR: Formato de fecha inválido. Use DD/MM/AAAA (ejemplo: 15/03/2024)");
                }
            }
        }

        /// <summary>
        /// Valida que la entrada sea un correo electrónico válido
        /// </summary>
        public static string ValidarEmail(string mensaje)
        {
            string patron = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            while (true)
            {
                Console.Write(mensaje);
                string email = Console.ReadLine()?.Trim() ?? "";

                if (Regex.IsMatch(email, patron))
                {
                    return email;
                }
                else
                {
                    Console.WriteLine("ERROR: Correo electrónico inválido. Ejemplo: usuario@dominio.com");
                }
            }
        }

        /// <summary>
        /// Valida que la entrada sea un número de teléfono válido
        /// </summary>
        public static string ValidarTelefono(string mensaje)
        {
            while (true)
            {
                Console.Write(mensaje);
                string telefono = Console.ReadLine()?.Trim() ?? "";

                if (telefono.Length >= 8 && telefono.Length <= 15 && EsSoloDigitos(telefono))
                {
                    return telefono;
                }
                else
                {
                    Console.WriteLine("ERROR: Teléfono inválido. Debe contener entre 8 y 15 dígitos.");
                }
            }
        }

        /// <summary>
        /// Valida que la entrada sea S/N (Sí/No)
        /// </summary>
        public static bool ValidarBooleano(string mensaje)
        {
            while (true)
            {
                Console.Write($"{mensaje} (S/N): ");
                string respuesta = Console.ReadLine()?.Trim().ToUpper() ?? "";

                if (respuesta == "S" || respuesta == "SI" || respuesta == "SÍ")
                {
                    return true;
                }
                else if (respuesta == "N" || respuesta == "NO")
                {
                    return false;
                }
                else
                {
                    Console.WriteLine("ERROR: Respuesta inválida. Ingrese S para Sí o N para No.");
                }
            }
        }

        /// <summary>
        /// Valida que la entrada sea un ISBN válido (10 o 13 dígitos)
        /// </summary>
        public static string ValidarIsbn(string mensaje)
        {
            while (true)
            {
                Console.Write(mensaje);
                string isbn = Console.ReadLine()?.Trim().Replace("-", "").Replace(" ", "") ?? "";

                if (EsSoloDigitos(isbn) && (isbn.Length == 10 || isbn.Length == 13))
                {
                    return isbn;
                }
                else
                {
                    Console.WriteLine("ERROR: ISBN inválido. Debe contener 10 o 13 dígitos.");
                }
            }
        }

        /// <summary>
        /// Pausa la ejecución hasta que el usuario presione Enter
        /// </summary>
        public static void Pausar()
        {
            Console.Write("\nPresione Enter para continuar...");
            Console.ReadLine();
        }

        // Métodos auxiliares privados
        private static bool EsSoloLetrasYEspacios(string texto)
        {
            foreach (char c in texto)
            {
                if (!char.IsLetter(c) && c != ' ')
                    return false;
            }
            return true;
        }

        private static bool EsSoloDigitos(string texto)
        {
            foreach (char c in texto)
            {
                if (!char.IsDigit(c))
                    return false;
            }
            return true;
        }
    }
}
