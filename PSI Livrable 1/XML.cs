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

namespace PSI_Livrable_1
{
    class XML
    {
        static string connectionString = "server=localhost;user=root;password=root;database=psi;";

        static void Main(string[] args)
        {
            InsertCuisiniers("Cuisiniers.xml");
            InsertParticuliers("Particuliers.xml");
            InsertEntreprises("Entreprises.xml");
            InsertCommandes("Commandes.xml");
            InsertPlats("Plats.xml");
            InsertIngredients("Ingredients.xml");
            InsertEstCompose("EstCompose.xml");
            InsertContient("Contient.xml");
        }

        static void InsertCuisiniers(string filePath)
        {
            XDocument doc = XDocument.Load(filePath);
            foreach (XElement element in doc.Root.Elements("Cuisinier"))
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "INSERT INTO Cuisinier VALUES (@id, @nom, @prenom, @adresse, @cp, @ville, @tel, @email, @metro)";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", (int)element.Element("NumeroCuisinier"));
                    cmd.Parameters.AddWithValue("@nom", (string)element.Element("NomC"));
                    cmd.Parameters.AddWithValue("@prenom", (string)element.Element("PrenomC"));
                    cmd.Parameters.AddWithValue("@adresse", (string)element.Element("AdresseC"));
                    cmd.Parameters.AddWithValue("@cp", (string)element.Element("CodePostalC"));
                    cmd.Parameters.AddWithValue("@ville", (string)element.Element("VilleC"));
                    cmd.Parameters.AddWithValue("@tel", (string)element.Element("TelC"));
                    cmd.Parameters.AddWithValue("@email", (string)element.Element("EmailC"));
                    cmd.Parameters.AddWithValue("@metro", (string)element.Element("MetroC"));
                    cmd.ExecuteNonQuery();
                }
            }
        }

        static void InsertParticuliers(string filePath)
        {
            XDocument doc = XDocument.Load(filePath);
            foreach (XElement element in doc.Root.Elements("Particulier"))
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "INSERT INTO Particulier VALUES (@id, @nom, @prenom, @adresse, @cp, @tel, @email, @metro)";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", (int)element.Element("NumeroParticulier"));
                    cmd.Parameters.AddWithValue("@nom", (string)element.Element("NomP"));
                    cmd.Parameters.AddWithValue("@prenom", (string)element.Element("PrenomP"));
                    cmd.Parameters.AddWithValue("@adresse", (string)element.Element("AdresseP"));
                    cmd.Parameters.AddWithValue("@cp", (string)element.Element("CodePostalP"));
                    cmd.Parameters.AddWithValue("@tel", (string)element.Element("TelP"));
                    cmd.Parameters.AddWithValue("@email", (string)element.Element("EmailP"));
                    cmd.Parameters.AddWithValue("@metro", (string)element.Element("MetroP"));
                    cmd.ExecuteNonQuery();
                }
            }
        }

        static void InsertEntreprises(string filePath)
        {
            XDocument doc = XDocument.Load(filePath);
            foreach (XElement element in doc.Root.Elements("Entreprise"))
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "INSERT INTO Entreprise VALUES (@id, @nom, @contact, @adresse, @cp, @tel, @email, @metro)";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", (int)element.Element("NumeroEntreprise"));
                    cmd.Parameters.AddWithValue("@nom", (string)element.Element("NomE"));
                    cmd.Parameters.AddWithValue("@contact", (string)element.Element("ContactE"));
                    cmd.Parameters.AddWithValue("@adresse", (string)element.Element("AdresseE"));
                    cmd.Parameters.AddWithValue("@cp", (string)element.Element("CodePostalE"));
                    cmd.Parameters.AddWithValue("@tel", (string)element.Element("TelE"));
                    cmd.Parameters.AddWithValue("@email", (string)element.Element("EmailE"));
                    cmd.Parameters.AddWithValue("@metro", (string)element.Element("MetroE"));
                    cmd.ExecuteNonQuery();
                }
            }
        }

        static void InsertCommandes(string filePath)
        {
            XDocument doc = XDocument.Load(filePath);
            foreach (XElement element in doc.Root.Elements("Commande"))
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "INSERT INTO Commande (IDCommande, DateCommande, AdresseLivraison, Satisfaction, NumeroCuisinier, NumeroParticulier, NumeroEntreprise) VALUES (@id, @date, @adresse, @satisfaction, @cuisinier, @particulier, @entreprise)";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", (int)element.Element("IDCommande"));
                    cmd.Parameters.AddWithValue("@date", DateTime.Parse((string)element.Element("DateCommande")));
                    cmd.Parameters.AddWithValue("@adresse", (string)element.Element("AdresseLivraison"));
                    cmd.Parameters.AddWithValue("@satisfaction", (int)element.Element("Satisfaction"));
                    cmd.Parameters.AddWithValue("@cuisinier", (int?)element.Element("NumeroCuisinier") ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@particulier", (int?)element.Element("NumeroParticulier") ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@entreprise", (int?)element.Element("NumeroEntreprise") ?? (object)DBNull.Value);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        static void InsertPlats(string filePath)
        {
            XDocument doc = XDocument.Load(filePath);
            foreach (XElement element in doc.Root.Elements("Plat"))
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "INSERT INTO Plat VALUES (@id, @nom, @prix, @quantite, @type, @dateFab, @datePer, @regime, @nat, @cuisinier)";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", (int)element.Element("IdPlat"));
                    cmd.Parameters.AddWithValue("@nom", (string)element.Element("NomPlat"));
                    cmd.Parameters.AddWithValue("@prix", decimal.Parse((string)element.Element("Prix")));
                    cmd.Parameters.AddWithValue("@quantite", int.Parse((string)element.Element("Quantite")));
                    cmd.Parameters.AddWithValue("@type", (string)element.Element("TypePlat"));
                    cmd.Parameters.AddWithValue("@dateFab", DateTime.Parse((string)element.Element("DateFabrication")));
                    cmd.Parameters.AddWithValue("@datePer", DateTime.Parse((string)element.Element("DatePeremption")));
                    cmd.Parameters.AddWithValue("@regime", (string)element.Element("RegimeAlim"));
                    cmd.Parameters.AddWithValue("@nat", (string)element.Element("Nationalite"));
                    cmd.Parameters.AddWithValue("@cuisinier", (int)element.Element("NumeroCuisinier"));
                    cmd.ExecuteNonQuery();
                }
            }
        }

        static void InsertIngredients(string filePath)
        {
            XDocument doc = XDocument.Load(filePath);
            foreach (XElement element in doc.Root.Elements("Ingredient"))
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "INSERT INTO Ingredient (IdIngredient, Nom, Quantite) VALUES (@id, @nom, @quantite)";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", (int)element.Element("IdIngredient"));
                    cmd.Parameters.AddWithValue("@nom", (string)element.Element("Nom"));
                    cmd.Parameters.AddWithValue("@quantite", (int)element.Element("Quantite"));
                    cmd.ExecuteNonQuery();
                }
            }
        }

        static void InsertEstCompose(string filePath)
        {
            XDocument doc = XDocument.Load(filePath);
            foreach (XElement element in doc.Root.Elements("EstCompose"))
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "INSERT INTO EstCompose VALUES (@plat, @ingredient)";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@plat", (int)element.Element("IdPlat"));
                    cmd.Parameters.AddWithValue("@ingredient", (int)element.Element("IdIngredient"));
                    cmd.ExecuteNonQuery();
                }
            }
        }

        static void InsertContient(string filePath)
        {
            XDocument doc = XDocument.Load(filePath);
            foreach (XElement element in doc.Root.Elements("Contient"))
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "INSERT INTO Contient VALUES (@commande, @plat)";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@commande", (int)element.Element("IdCommande"));
                    cmd.Parameters.AddWithValue("@plat", (int)element.Element("IdPlat"));
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
    class ExportXML
    {
        static string connectionString = "Server=localhost;Database=psi;Trusted_Connection=True;";

        static void Main(string[] args)
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
