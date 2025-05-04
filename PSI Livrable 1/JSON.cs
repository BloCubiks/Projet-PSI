using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;

namespace PSI_ClovisNOE_JaimeSOUSA_ThomasMAYE
{
    public class JSON
    {
        static string connectionString = "SERVER=localhost;PORT=3306;DATABASE=psi;UID=root;PASSWORD=root;";

        public static void Import()
        {
            InsertCuisiniers("Cuisinier.json");
            InsertParticuliers("Particulier.json");
            InsertEntreprises("Entreprise.json");
            InsertCommandes("Commande.json");
            InsertPlats("Plat.json");
            InsertIngredients("Ingredient.json");
            InsertEstCompose("EstCompose.json");
            InsertContient("Contient.json");
        }

        static void InsertCuisiniers(string filePath)
        {
            var data = JsonConvert.DeserializeObject<List<dynamic>>(File.ReadAllText(filePath));
            foreach (var c in data)
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "INSERT IGNORE INTO Cuisinier  " +
                                   "VALUES (@id, @nom, @prenom, @adresse, @cp, @ville, @tel, @email, @metro)";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", (int)c.NumeroCuisinier);
                    cmd.Parameters.AddWithValue("@nom", (string)c.NomC);
                    cmd.Parameters.AddWithValue("@prenom", (string)c.PrenomC);
                    cmd.Parameters.AddWithValue("@adresse", (string)c.AdresseC);
                    cmd.Parameters.AddWithValue("@cp", (string)c.CodePostalC);
                    cmd.Parameters.AddWithValue("@ville", (string)c.VilleC);
                    cmd.Parameters.AddWithValue("@tel", (string)c.TelC);
                    cmd.Parameters.AddWithValue("@email", (string)c.EmailC);
                    cmd.Parameters.AddWithValue("@metro", (string)c.MetroC);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        static void InsertParticuliers(string filePath)
        {
            var data = JsonConvert.DeserializeObject<List<dynamic>>(File.ReadAllText(filePath));
            foreach (var p in data)
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "INSERT IGNORE INTO Particulier  " +
                                   "VALUES (@id, @nom, @prenom, @adresse, @cp, @tel, @email, @metro)";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", (int)p.NumeroParticulier);
                    cmd.Parameters.AddWithValue("@nom", (string)p.NomP);
                    cmd.Parameters.AddWithValue("@prenom", (string)p.PrenomP);
                    cmd.Parameters.AddWithValue("@adresse", (string)p.AdresseP);
                    cmd.Parameters.AddWithValue("@cp", (string)p.CodePostalP);
                    cmd.Parameters.AddWithValue("@tel", (string)p.TelP);
                    cmd.Parameters.AddWithValue("@email", (string)p.EmailP);
                    cmd.Parameters.AddWithValue("@metro", (string)p.MetroP);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        static void InsertEntreprises(string filePath)
        {
            var data = JsonConvert.DeserializeObject<List<dynamic>>(File.ReadAllText(filePath));
            foreach (var e in data)
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "INSERT IGNORE INTO Entreprise  " +
                                   "VALUES (@id, @nom, @contact, @adresse, @cp, @tel, @email, @metro)";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", (int)e.NumeroEntreprise);
                    cmd.Parameters.AddWithValue("@nom", (string)e.NomE);
                    cmd.Parameters.AddWithValue("@contact", (string)e.ContactE);
                    cmd.Parameters.AddWithValue("@adresse", (string)e.AdresseE);
                    cmd.Parameters.AddWithValue("@cp", (string)e.CodePostalE);
                    cmd.Parameters.AddWithValue("@tel", (string)e.TelE);
                    cmd.Parameters.AddWithValue("@email", (string)e.EmailE);
                    cmd.Parameters.AddWithValue("@metro", (string)e.MetroE);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        static void InsertCommandes(string filePath)
        {
            var data = JsonConvert.DeserializeObject<List<dynamic>>(File.ReadAllText(filePath));
            foreach (var c in data)
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "INSERT IGNORE INTO Commande  " +
                                   "VALUES (@id, @date, @adresse, @satisfaction, @cuisinier, @particulier, @entreprise)";
                    MySqlCommand cmd = new MySqlCommand(query, conn);

                    cmd.Parameters.AddWithValue("@id", (int)c.IDCommande);

                    string dateStr = (string)c.DateCommande;
                    DateTime parsedDate;
                    try
                    {
                        parsedDate = DateTime.Parse(dateStr, null, System.Globalization.DateTimeStyles.AssumeUniversal | System.Globalization.DateTimeStyles.AdjustToUniversal);
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine($"[ERREUR] Format de date invalide pour ID {c.IDCommande} : '{dateStr}'");
                        continue; 
                    }

                    cmd.Parameters.AddWithValue("@date", parsedDate);
                    cmd.Parameters.AddWithValue("@adresse", (string)c.AdresseLivraison);
                    cmd.Parameters.AddWithValue("@satisfaction", (int)c.Satisfaction);
                    cmd.Parameters.AddWithValue("@cuisinier", c.NumeroCuisinier != null ? (int)c.NumeroCuisinier : (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@particulier", c.NumeroParticulier != null ? (int)c.NumeroParticulier : (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@entreprise", c.NumeroEntreprise != null ? (int)c.NumeroEntreprise : (object)DBNull.Value);
                    cmd.ExecuteNonQuery();
                }
            }
        }


        static void InsertPlats(string filePath)
        {
            var data = JsonConvert.DeserializeObject<List<dynamic>>(File.ReadAllText(filePath));
            foreach (var p in data)
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "INSERT IGNORE INTO Plat  " +
                                   "VALUES (@id, @nom, @prix, @quantite, @type, @dateFab, @datePer, @regime, @nat, @cuisinier)";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", (int)p.IdPlat);
                    cmd.Parameters.AddWithValue("@nom", (string)p.NomPlat);
                    cmd.Parameters.AddWithValue("@prix", (decimal)p.Prix);
                    cmd.Parameters.AddWithValue("@quantite", (int)p.Quantite);
                    cmd.Parameters.AddWithValue("@type", (string)p.TypePlat);
                    cmd.Parameters.AddWithValue("@dateFab", DateTime.Parse((string)p.DateFabrication).ToUniversalTime());
                    cmd.Parameters.AddWithValue("@datePer", DateTime.Parse((string)p.DatePeremption).ToUniversalTime());
                    cmd.Parameters.AddWithValue("@regime", (string)p.RegimeAlim);
                    cmd.Parameters.AddWithValue("@nat", (string)p.Nationalite);
                    cmd.Parameters.AddWithValue("@cuisinier", (int)p.NumeroCuisinier);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        static void InsertIngredients(string filePath)
        {
            var data = JsonConvert.DeserializeObject<List<dynamic>>(File.ReadAllText(filePath));
            foreach (var i in data)
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "INSERT IGNORE INTO Ingredient " +
                                   "VALUES (@id, @nom, @quantite)";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", (int)i.IdIngredient);
                    cmd.Parameters.AddWithValue("@nom", (string)i.Nom);
                    cmd.Parameters.AddWithValue("@quantite", (int)i.Quantite);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        static void InsertEstCompose(string filePath)
        {
            var data = JsonConvert.DeserializeObject<List<dynamic>>(File.ReadAllText(filePath));
            foreach (var ec in data)
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "INSERT IGNORE INTO EstCompose  " +
                                   "VALUES (@plat, @ingredient)";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@plat", (int)ec.IdPlat);
                    cmd.Parameters.AddWithValue("@ingredient", (int)ec.IdIngredient);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        static void InsertContient(string filePath)
        {
            var data = JsonConvert.DeserializeObject<List<dynamic>>(File.ReadAllText(filePath));
            foreach (var c in data)
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "INSERT IGNORE INTO Contient  " +
                                   "VALUES (@commande, @plat)";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@commande", (int)c.IdCommande);
                    cmd.Parameters.AddWithValue("@plat", (int)c.IdPlat);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
