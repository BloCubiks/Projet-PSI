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

namespace PSI_ClovisNOE_JaimeSOUSA_ThomasMAYE
{
    public class ExportJson
    {
        static string connectionString = "SERVER=localhost;PORT=3306;DATABASE=psi;UID=root;PASSWORD=root;";

        public static void JsonExport()
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

        public static void ExportTableToJson(string tableName)
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
