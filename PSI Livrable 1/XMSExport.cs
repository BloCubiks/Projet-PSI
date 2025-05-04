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
    class ExportXML
    {
        static string connectionString = "Server=localhost;Database=psi;Trusted_Connection=True;";

        public static void XMSExport()
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

        static void ExportTableToXml(string tableName)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = $"SELECT * FROM {tableName}";

                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
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
