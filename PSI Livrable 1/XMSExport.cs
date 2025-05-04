using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Xml.Linq;
using System.Data.SqlClient;
using System.Data;
using System.IO;

namespace PSI_ClovisNOE_JaimeSOUSA_ThomasMAYE
{
    public class XMSExport
    {
        static string connectionString = "SERVER=localhost;PORT=3306;DATABASE=psi;UID=root;PASSWORD=root;";

        public static void XMLExport()
        {
            ExportTableToXml("Cuisinier");
            ExportTableToXml("Particulier");
            ExportTableToXml("Entreprise");
            ExportTableToXml("Commande");
            ExportTableToXml("Plat");
            ExportTableToXml("Ingredient");
            ExportTableToXml("EstCompose");
            ExportTableToXml("Contient");

            Console.WriteLine("Export XML terminé.");
        }

        public static void ExportTableToXml(string tableName)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string query = $"SELECT * FROM {tableName}";

                MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                DataTable dt = new DataTable(tableName);
                adapter.Fill(dt);

                using (FileStream fs = new FileStream($"{tableName}.xml", FileMode.Create))
                {
                    dt.WriteXml(fs, XmlWriteMode.WriteSchema);
                }

                Console.WriteLine($"{tableName}.xml exporté.");
            }
        }
    }
}
