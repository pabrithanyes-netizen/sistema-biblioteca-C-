using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Linq;

namespace SistemaBiblioteca.Utils
{
    /// <summary>
    /// Módulo de manejo de archivos
    /// Funciones para guardar y cargar datos en archivos JSON locales
    /// </summary>
    public static class ManejoArchivos
    {
        // Ruta de la carpeta de datos (en la raíz del proyecto)
        private static readonly string RutaDatos = ObtenerRutaData();

        /// <summary>
        /// Obtiene la ruta de la carpeta Data en la raíz del proyecto
        /// </summary>
        private static string ObtenerRutaData()
        {
            // Obtener el directorio actual del ejecutable
            string directorioBase = AppDomain.CurrentDomain.BaseDirectory;

            // Navegar hacia arriba hasta encontrar la carpeta del proyecto
            // (sube desde bin/Debug/net10.0/ hasta la raíz del proyecto)
            DirectoryInfo dirInfo = new DirectoryInfo(directorioBase);

            // Buscar el archivo .csproj subiendo niveles
            while (dirInfo != null && dirInfo.GetFiles("*.csproj").Length == 0)
            {
                dirInfo = dirInfo.Parent;
            }

            // Si encontramos la carpeta del proyecto, usar Data/ ahí
            if (dirInfo != null)
            {
                return Path.Combine(dirInfo.FullName, "Data");
            }

            // Si no encontramos el .csproj, usar la carpeta actual
            return Path.Combine(directorioBase, "Data");
        }

        static ManejoArchivos()
        {
            // Crear carpeta de datos si no existe
            if (!Directory.Exists(RutaDatos))
            {
                Directory.CreateDirectory(RutaDatos);
            }
        }

        /// <summary>
        /// Guarda una lista de objetos en archivo JSON local
        /// </summary>
        public static void GuardarDatos<T>(string nombreArchivo, List<T> listaDatos)
        {
            string rutaArchivo = Path.Combine(RutaDatos, $"{nombreArchivo}.json");
            try
            {
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                };
                string json = JsonSerializer.Serialize(listaDatos, options);
                File.WriteAllText(rutaArchivo, json);
            }
            catch (Exception e)
            {
                Console.WriteLine($"ERROR: Error al guardar datos en {nombreArchivo}: {e.Message}");
            }
        }

        /// <summary>
        /// Carga datos desde archivo JSON local
        /// </summary>
        public static List<T> CargarDatos<T>(string nombreArchivo)
        {
            string rutaArchivo = Path.Combine(RutaDatos, $"{nombreArchivo}.json");
            try
            {
                if (File.Exists(rutaArchivo))
                {
                    string json = File.ReadAllText(rutaArchivo);
                    return JsonSerializer.Deserialize<List<T>>(json) ?? new List<T>();
                }
                else
                {
                    return new List<T>();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"ERROR: Error al cargar datos desde {nombreArchivo}: {e.Message}");
                return new List<T>();
            }
        }

        /// <summary>
        /// Guarda el valor de un contador en archivo JSON local
        /// </summary>
        public static void GuardarContador(string nombreContador, int valor)
        {
            string rutaArchivo = Path.Combine(RutaDatos, $"contador_{nombreContador}.json");
            try
            {
                var datos = new { contador = valor };
                var options = new JsonSerializerOptions { WriteIndented = true };
                string json = JsonSerializer.Serialize(datos, options);
                File.WriteAllText(rutaArchivo, json);
            }
            catch (Exception e)
            {
                Console.WriteLine($"ERROR: Error al guardar contador {nombreContador}: {e.Message}");
            }
        }

        /// <summary>
        /// Carga el valor de un contador desde archivo JSON local
        /// </summary>
        public static int CargarContador(string nombreContador)
        {
            string rutaArchivo = Path.Combine(RutaDatos, $"contador_{nombreContador}.json");
            try
            {
                if (File.Exists(rutaArchivo))
                {
                    string json = File.ReadAllText(rutaArchivo);
                    using (JsonDocument doc = JsonDocument.Parse(json))
                    {
                        if (doc.RootElement.TryGetProperty("contador", out JsonElement contador))
                        {
                            return contador.GetInt32();
                        }
                    }
                }
                return 1;
            }
            catch (Exception e)
            {
                Console.WriteLine($"ERROR: Error al cargar contador {nombreContador}: {e.Message}");
                return 1;
            }
        }

        /// <summary>
        /// Obtiene el siguiente ID disponible e incrementa el contador
        /// </summary>
        public static int ObtenerSiguienteId(string nombreContador)
        {
            int contadorActual = CargarContador(nombreContador);
            GuardarContador(nombreContador, contadorActual + 1);
            return contadorActual;
        }

        /// <summary>
        /// Busca un elemento por su ID en una lista de diccionarios
        /// </summary>
        public static T BuscarPorId<T>(List<T> listaDatos, int idBuscar, Func<T, int> obtenerIdFunc)
        {
            return listaDatos.FirstOrDefault(item => obtenerIdFunc(item) == idBuscar);
        }

        /// <summary>
        /// Elimina un elemento por su ID de una lista
        /// </summary>
        public static bool EliminarPorId<T>(List<T> listaDatos, int idEliminar, Func<T, int> obtenerIdFunc)
        {
            var item = listaDatos.FirstOrDefault(i => obtenerIdFunc(i) == idEliminar);
            if (item != null)
            {
                listaDatos.Remove(item);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Retorna el modo de almacenamiento actual
        /// </summary>
        public static string ObtenerModoAlmacenamiento()
        {
            return "JSON Local";
        }
    }
}
