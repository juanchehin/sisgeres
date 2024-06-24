using System;
using System.IO;
using System.Runtime.CompilerServices;


namespace sisgeres.libs
{
    public class LogsCustom
    {
        public void alta_log(string mensaje,
                         [CallerFilePath] string filePath = "",
                         [CallerMemberName] string memberName = "",
                         [CallerLineNumber] int lineNumber = 0)
        {
            try
            {
                string appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                string date = DateTime.Now.ToString("yyyy-MM-dd");
                string logFileName = $"log_{date}.txt";
                string logFilePath = Path.Combine(appDataFolder, "sisgeres", logFileName);

                // Verifica si el directorio del archivo existe, si no, lo crea
                string directoryPath = Path.GetDirectoryName(logFilePath);
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                // Verifica si el archivo existe, si no, lo crea y escribe contenido inicial
                if (!File.Exists(logFilePath))
                {
                    using (StreamWriter writer = File.CreateText(logFilePath))
                    {
                        string mensaje_creacion = "Este es un nuevo archivo creado.";
                        string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        writer.WriteLine($"[{timestamp}] - {mensaje_creacion}");
                    }
                }

                // Escribir el mensaje en el archivo
                using (StreamWriter writer = new StreamWriter(logFilePath, true))
                {
                    string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    writer.WriteLine($"[{timestamp}] - {mensaje} (File: {Path.GetFileName(filePath)}, Member: {memberName}, Line: {lineNumber})");
                }
            }
            catch (Exception ex)
            {
                // Manejar excepciones si es necesario
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

    }
}
