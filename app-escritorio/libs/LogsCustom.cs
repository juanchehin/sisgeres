using System;
using System.IO;


namespace sisgeres.libs
{
    public class LogsCustom
    {
        public void alta_log(string mensaje)
        {
            try
            {

                string appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                string date = DateTime.Now.ToString("yyyy-MM-dd");
                string fileName = $"log_{date}.txt";
                string filePath = Path.Combine(appDataFolder, "sisgeres", fileName);

                // Verifica si el directorio del archivo existe, si no, lo crea
                string directoryPath = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                // Verifica si el archivo existe, si no, lo crea y escribe contenido inicial
                if (!File.Exists(filePath))
                {
                    using (StreamWriter writer = File.CreateText(filePath))
                    {
                        string mensaje_creacion = "Este es un nuevo archivo creado.";
                        string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); // Obtiene la fecha y hora actual
                        writer.WriteLine($"[{timestamp}] - {mensaje_creacion}");
                    }
                }

                // Escribir el mensaje en el archivo
                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); // Obtiene la fecha y hora actual
                    writer.WriteLine($"[{timestamp}] - {mensaje}");
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message.ToString(), "Error al crear el archivo logs");
            }

        }

    }
}
