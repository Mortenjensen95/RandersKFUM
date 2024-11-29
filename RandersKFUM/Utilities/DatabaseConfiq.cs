using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandersKFUM.Utilities
{
    public class DatabaseConfig
    {
        // Metoden GetConnectionString returnerer connection string fra appsettings.json
        public static string GetConnectionString()
        {
            // Bygger konfigurationen og læser fra appsettings.json
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory()) // Finder den aktuelle mappe
                .AddJsonFile("appsettings.json") // Tilføjer appsettings.json til konfigurationen
                .Build();

            // Henter connection string fra "ConnectionStrings" sektionen i appsettings.json
            return configuration.GetConnectionString("DefaultConnection");
        }
    }
}
