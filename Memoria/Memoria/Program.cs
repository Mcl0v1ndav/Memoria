using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

class MemoryMonitor
{
    static void Main()
    {
        // Configurar los parámetros del monitoreo
        int interval = 5; // Intervalo de muestreo en segundos
        int duration = 60; // Duración total del monitoreo en segundos

        // Ruta del archivo CSV de salida
        string outputFilePath = @"C:\Users\User\Desktop\hoja.csv";

        // Obtener la ruta completa del archivo CSV
        string outputFullPath = Path.GetFullPath(outputFilePath);

        // Crear el archivo CSV y escribir el encabezado si no existe
        if (!File.Exists(outputFullPath))
        {
            using (var writer = new StreamWriter(outputFullPath))
            {
                writer.WriteLine("Timestamp,RAM Usage (%),Swap Usage (%)");
            }
        }

        // Obtener el tiempo actual
        DateTime startTime = DateTime.Now;

        // Realizar el monitoreo durante la duración especificada
        while ((DateTime.Now - startTime).TotalSeconds < duration)
        {
            // Obtener el uso de memoria del sistema
            PerformanceCounter ramCounter = new PerformanceCounter("Memory", "Available MBytes");
            PerformanceCounter swapCounter = new PerformanceCounter("Paging File", "% Usage", "_Total");

            // Calcular el uso de memoria en porcentaje
            float ramUsagePercent = 100 - (ramCounter.NextValue() / (float)ramCounter.NextValue()) * 100;
            float swapUsagePercent = swapCounter.NextValue();

            // Obtener el tiempo actual en formato legible
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            // Escribir los datos en el archivo CSV
            using (var writer = new StreamWriter(outputFullPath, true))
            {
                writer.WriteLine($"{timestamp},{ramUsagePercent},{swapUsagePercent}");
            }

            // Esperar el intervalo especificado antes de la siguiente lectura
            Thread.Sleep(interval * 1000);
        }

        Console.WriteLine("El monitoreo ha finalizado. Los datos se han guardado en: " + outputFullPath);
        Console.ReadLine();
    }
}
