using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System.Data.SqlClient;
using System.Data;

namespace PSI_Livrable_1
{
    class ExportJson
    {
        static string connectionString = "Server=localhost;Database=psi;Trusted_Connection=True;";

        public static void Export(string[] args)
        {
            ExportTableToJson("Cuisinier");
            ExportTableToJson("Particulier");
            ExportTableToJson("Entreprise");
            ExportTableToJson("Commande");
            ExportTableToJson("Plat");
            ExportTableToJson("Ingredient");
            ExportTableToJson("EstCompose");
            ExportTableToJson("Contient");

            Console.WriteLine("Export JSON terminé.");
        }

        static void ExportTableToJson(string tableName)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = $"SELECT * FROM {tableName}";

                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                string json = JsonConvert.SerializeObject(dt, Formatting.Indented);
                File.WriteAllText($"{tableName}.json", json);

                Console.WriteLine($"{tableName}.json exporté.");
            }
        }
    }
}
