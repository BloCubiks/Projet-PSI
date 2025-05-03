using K4os.Compression.LZ4.Streams.Abstractions;
using Microsoft.VisualBasic;
using MySql.Data.MySqlClient;
using Mysqlx.Crud;
using Org.BouncyCastle.Cms;
using Org.BouncyCastle.Tls.Crypto;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Pipelines;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using System;
using Google.Protobuf.WellKnownTypes;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TreeView;
using System.Diagnostics.Eventing.Reader;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Threading;

namespace PSI_ClovisNOE_JaimeSOUSA_ThomasMAYE
{
    public class BDD
    {
        public static Dictionary<int, string> Cuisiniers()
        {
            string connectionString = "SERVER=localhost;PORT=3306;DATABASE=psi;UID=root;PASSWORD=root;";
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT NumeroCuisinier,NomC FROM Cuisinier;";
            MySqlDataReader reader = command.ExecuteReader();

            Dictionary<int, string> cuisiniers = new Dictionary<int, string>();

            while (reader.Read())
            {
                int numeroCuisinier = reader.GetInt32(reader.GetOrdinal("NumeroCuisinier"));
                string nomC = reader.GetString(reader.GetOrdinal("NomC"));
                cuisiniers[numeroCuisinier] = nomC;
            }
            return cuisiniers;
        }
        public static Dictionary<int, string> Clients()
        {
            string connectionString = "SERVER=localhost;PORT=3306;DATABASE=psi;UID=root;PASSWORD=root;";
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT NumeroParticulier,NomP FROM Particulier;";
            MySqlDataReader reader = command.ExecuteReader();

            Dictionary<int, string> clients = new Dictionary<int, string>();

            while (reader.Read())
            {
                int numeroParticulier = reader.GetInt32(reader.GetOrdinal("NumeroParticulier"));
                string nomP = reader.GetString(reader.GetOrdinal("NomP"));
                clients[numeroParticulier] = nomP;
            }
            command = connection.CreateCommand();
            command.CommandText = "SELECT NumeroEntreprise,NomE FROM Entreprise;";
            reader.Close();
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                int numeroEntreprise = reader.GetInt32(reader.GetOrdinal("NumeroEntreprise")) + 2000;
                string nomE = reader.GetString(reader.GetOrdinal("NomE"));
                clients[numeroEntreprise] = nomE;
            }

            return clients;
        }
        public static Dictionary<int, int> Commandes()
        {
            string connectionString = "SERVER=localhost;PORT=3306;DATABASE=psi;UID=root;PASSWORD=root;";
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT NumeroCuisinier,NumeroParticulier,NumeroEntreprise FROM Commande;";
            MySqlDataReader reader = command.ExecuteReader();
            Dictionary<int, int> commandes = new Dictionary<int, int>();
            while (reader.Read())
            {
                int numeroCuisinier = reader.GetInt32(reader.GetOrdinal("NumeroCuisinier")); ;
                int numeroParticulier = reader.IsDBNull(reader.GetOrdinal("numeroParticulier")) ? -1 : reader.GetInt32(reader.GetOrdinal("numeroParticulier"));
                int numeroEntreprise = reader.IsDBNull(reader.GetOrdinal("NumeroEntreprise")) ? -1 : reader.GetInt32(reader.GetOrdinal("NumeroEntreprise")) + 2000;
                if (numeroParticulier == -1)
                {
                    commandes[numeroCuisinier] = numeroEntreprise;
                }
                else commandes[numeroCuisinier] = numeroParticulier;
            }
            return commandes;
        }
        public static void Appelle_BDD(Graphe<Station> graph)
        {
            string choix = "";
            string connectionString = "SERVER=localhost;PORT=3306;DATABASE=psi;UID=root;PASSWORD=root;";
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();
            MySqlCommand command = connection.CreateCommand();
            MySqlDataReader reader = null;
            while (choix != "0")
            {
                Console.Clear();
                Console.WriteLine("Menu Principal");
                Console.WriteLine("1. Gestion des clients");
                Console.WriteLine("2. Gestion des commandes");
                Console.WriteLine("3. Gestion des cuisiniers");
                Console.WriteLine("4. Gestion des plats");
                Console.WriteLine("5. Module statistiques");
                Console.WriteLine("6. Démo");
                Console.WriteLine("0. Quitter");

                Console.Write("Choisissez une option : ");
                choix = Console.ReadLine();

                switch (choix)
                {
                    case "1":
                        GestionClients(command, reader);
                        break;
                    case "2":
                        GestionCommandes(command, reader);
                        break;
                    case "3":
                        GestionCuisinier(command, reader);
                        break;
                    case "4":
                        GestionPlat(command, reader);
                        break;
                    case "5":
                        ModuleStatistique(command, reader);
                        break;
                    case "6":
                        Demo(command, reader, graph);
                        break;
                    case "0":
                        Console.WriteLine("Au revoir !");
                        return;
                    default:
                        Console.Clear();
                        Console.WriteLine("Option invalide, veuillez réessayer.");
                        System.Threading.Thread.Sleep(1000);
                        break;
                }
            }
            connection.Close();
        }

        static void Demo(MySqlCommand command, MySqlDataReader reader, Graphe<Station> graph)
        {
            Console.Clear();

            // Création d'un particulier
            Console.WriteLine("Création d'un particulier...");
            System.Threading.Thread.Sleep(500);
            Console.Clear();
            Console.WriteLine("Création d'un particulier en cours...");
            System.Threading.Thread.Sleep(500);
            command.CommandText =
                "INSERT INTO Particulier (NumeroParticulier, NomP, PrenomP, AdresseP, CodePostalP, TelP, EmailP, MetroP) " +
                "VALUES (617, 'Dupuis', 'Marie', '10 Rue de la République', '75011', '0687654321', 'marie.dupuis@mail.com', 'République');";
            command.ExecuteNonQuery();
            Console.WriteLine("Particulier créé !");
            Read(command, reader, "SELECT * FROM Particulier;");

            // Création d'un cuisinier
            Console.WriteLine("Création d'un cuisinier...");
            System.Threading.Thread.Sleep(500);
            Console.Clear();
            Console.WriteLine("Création d'un cuisinier en cours...");
            System.Threading.Thread.Sleep(500);
            command.CommandText =
                "INSERT INTO Cuisinier (NumeroCuisinier, NomC, PrenomC, AdresseC, CodePostalC, VilleC, TelC, EmailC, MetroC) " +
                "VALUES (23, 'Durand', 'Lucie', '15 Avenue de la Liberté', '75012', 'Paris', '0698765432', 'lucie.durand@mail.com', 'Gare de Lyon');";
            command.ExecuteNonQuery();
            Console.WriteLine("Cuisinier créé !");
            Read(command, reader, "SELECT * FROM Cuisinier;");

            // Création d'un plat
            Console.WriteLine("Création d'un plat...");
            System.Threading.Thread.Sleep(500);
            Console.Clear();
            Console.WriteLine("Création d'un plat en cours...");
            System.Threading.Thread.Sleep(500);
            command.CommandText =
                "INSERT INTO Plat (IdPlat, NomPlat, Prix, Quantite, TypePlat, DateFabrication, DatePeremption, RegimeAlim, Nationalite, NumeroCuisinier) " +
                "VALUES (56, 'Tacos Mexicain', 12.00, 8, 'Mexicain', '2024-03-30', '2024-04-05', 'Omnivore', 'Mexicain', 4);";
            command.ExecuteNonQuery();
            Console.WriteLine("Plat créé !");
            Read(command, reader, "SELECT * FROM Plat;");

            // Création d'une commande avec jointure pour afficher les lignes de métro
            Console.WriteLine("Création d'une commande...");
            System.Threading.Thread.Sleep(500);
            Console.Clear();
            Console.WriteLine("Création de la commande en cours...");
            System.Threading.Thread.Sleep(500);
            command.CommandText =
                "INSERT INTO Commande (DateCommande, AdresseLivraison, Satisfaction, NumeroCuisinier, NumeroParticulier, NumeroEntreprise) " +
                "VALUES ('2024-04-01', 'Rue de la République, 12', 9, 4, 3, NULL);";
            command.ExecuteNonQuery();
            Console.WriteLine("Commande créée !");

            // Affichage des lignes de métro du cuisinier et du particulier liés à la commande
            command.CommandText =
                "SELECT c.NomC, c.PrenomC, c.MetroC, p.NomP, p.PrenomP, p.MetroP " +
                "FROM Commande cmd " +
                "JOIN Cuisinier c ON cmd.NumeroCuisinier = c.NumeroCuisinier " +
                "JOIN Particulier p ON cmd.NumeroParticulier = p.NumeroParticulier " +
                "WHERE cmd.IDCommande = LAST_INSERT_ID();";

            reader = command.ExecuteReader();
            Console.WriteLine(" Informations des lignes de métro associées à la commande :");
            while (reader.Read())
            {
                Console.WriteLine($" Cuisinier : {reader["NomC"]} {reader["PrenomC"]} - Métro : {reader["MetroC"]}");
                Console.WriteLine($" Particulier : {reader["NomP"]} {reader["PrenomP"]} - Métro : {reader["MetroP"]}");
            }
            bool trouveS = false;
            foreach (var station in graph.Noeuds)
            {
                if (!trouveS && station.Type.LibelleStation.Equals(reader["MetroC"]))
                {
                    trouveS = true;
                    int[,] distances = graph.Dijkstra(station);
                    bool trouveZ = false;
                    Graphe<Station> graphD = new Graphe<Station>();
                    List<Station> Parcours = new List<Station>();

                    foreach (var noeud in graph.Noeuds)
                    {
                        if (!trouveZ && noeud.Type.LibelleStation.Equals(reader["MetroP"]))
                        {
                            Noeud<Station> precedent = noeud;
                            trouveZ = true;
                            Parcours.Add(precedent.Type);
                            while (precedent.Id != station.Id)
                            {
                                precedent = graph.Noeuds[distances[precedent.Id - 1, 1]];
                                Parcours.Add(precedent.Type);
                            }
                        }
                    }

                    Dictionary<Noeud<Station>, Station> nodeToStationD = new Dictionary<Noeud<Station>, Station>();
                    Dictionary<Noeud<Station>, (double, double)> nodePositionsD = new Dictionary<Noeud<Station>, (double, double)>();

                    // Ajouter les nœuds du parcours au nouveau graphe
                    foreach (var etape in Parcours)
                    {
                        Noeud<Station> noeud = new Noeud<Station>(etape.IdStation, etape);
                        graphD.AjouterNoeud(noeud);
                        nodeToStationD[noeud] = etape;
                        nodePositionsD[noeud] = (etape.Longitude, etape.Latitude);
                    }

                    // Ajouter les liens entre les nœuds du parcours
                    for (int i = 0; i < Parcours.Count - 1; i++)
                    {
                        Noeud<Station> nodeStart = graphD.Noeuds[i];
                        Noeud<Station> nodeEnd = graphD.Noeuds[i + 1];

                        double distance = Graphe<Station>.HaversineDistance(Parcours[i].Latitude, Parcours[i].Longitude, Parcours[i + 1].Latitude, Parcours[i + 1].Longitude);
                        int travelTime = (int)Math.Round(distance * 2);
                        graphD.AjouterLien(new Lien<Station>(nodeStart, nodeEnd, travelTime, Parcours[i].LibelleLine));
                        graphD.AjouterLien(new Lien<Station>(nodeEnd, nodeStart, travelTime, Parcours[i].LibelleLine));
                    }
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new Visualisation<Station>(graphD, nodeToStationD, nodePositionsD, true));
                }
            }
            reader.Close();
            Console.WriteLine("\nAppuyez sur une touche pour continuer...");
            Console.ReadKey();

            Read(command, reader, "SELECT * FROM Commande;");

            // Ajout de plats à la commande
            Console.WriteLine("Ajout de plats à la commande...");
            System.Threading.Thread.Sleep(500);
            command.CommandText =
                "INSERT INTO Contient (IdCommande, IdPlat) " +
                "VALUES (LAST_INSERT_ID(), 4);";
            command.ExecuteNonQuery();
            Console.WriteLine("Plat ajouté à la commande !");
            Read(command, reader, "SELECT * FROM Contient WHERE IdCommande = LAST_INSERT_ID();");

            // Suppression de la commande
            Console.WriteLine("Suppression de la commande...");
            command.CommandText = "DELETE FROM Commande WHERE IDCommande = LAST_INSERT_ID();";
            command.ExecuteNonQuery();
            Console.WriteLine("Commande supprimée !");
            Read(command, reader, "SELECT * FROM Commande WHERE IDCommande = LAST_INSERT_ID();");

            // Suppression du plat
            Console.WriteLine("Suppression du plat 'Tacos Mexicain'...");
            command.CommandText = "DELETE FROM Plat WHERE IdPlat = 4;";
            command.ExecuteNonQuery();
            Console.WriteLine("Plat supprimé !");
            Read(command, reader, "SELECT * FROM Plat WHERE IdPlat = 4;");

            // Suppression du cuisinier
            Console.WriteLine("Suppression du cuisinier Lucie Durand...");
            command.CommandText = "DELETE FROM Cuisinier WHERE NumeroCuisinier = 4;";
            command.ExecuteNonQuery();
            Console.WriteLine("Cuisinier supprimé !");
            Read(command, reader, "SELECT * FROM Cuisinier WHERE NumeroCuisinier = 4;");

            // Suppression du particulier
            Console.WriteLine("Suppression du particulier Marie Dupuis...");
            command.CommandText = "DELETE FROM Particulier WHERE NumeroParticulier = 3;";
            command.ExecuteNonQuery();
            Console.WriteLine("Particulier supprimé !");
            Read(command, reader, "SELECT * FROM Particulier WHERE NumeroParticulier = 3;");
        }

        static void ModuleStatistique(MySqlCommand command, MySqlDataReader reader)
        {
            Console.Clear();
            Console.WriteLine("Voici le nombre de plats créés par chaque cuisinier :");
            // Nombre de plats créés par chaque cuisinier
            Read(command, reader,
                "SELECT Cuisinier.NomC, Cuisinier.PrenomC, COUNT(Plat.IdPlat) AS NombreDePlats " +
                "FROM Cuisinier " +
                "LEFT JOIN Plat ON Plat.NumeroCuisinier = Cuisinier.NumeroCuisinier " +
                "GROUP BY Cuisinier.NumeroCuisinier " +
                "ORDER BY NombreDePlats DESC;");

            Console.WriteLine("\nVoici les commandes prises en charge par chaque cuisinier :");
            // Nombre de commandes prises en charge par chaque cuisinier
            Read(command, reader,
                "SELECT Cuisinier.NomC, Cuisinier.PrenomC, COUNT(DISTINCT Commande.IDCommande) AS NombreDeCommandes " +
                "FROM Cuisinier " +
                "LEFT JOIN Commande ON Commande.NumeroCuisinier = Cuisinier.NumeroCuisinier " +
                "GROUP BY Cuisinier.NumeroCuisinier " +
                "ORDER BY NombreDeCommandes DESC;");

            Console.WriteLine("\nVoici les plats dans chaque commande :");
            // Plats dans chaque commande
            Read(command, reader,
                "SELECT Commande.IDCommande, GROUP_CONCAT(Plat.NomPlat) AS PlatsDansCommande " +
                "FROM Commande " +
                "JOIN Contient ON Contient.IdCommande = Commande.IDCommande " +
                "JOIN Plat ON Plat.IdPlat = Contient.IdPlat " +
                "GROUP BY Commande.IDCommande " +
                "ORDER BY Commande.IDCommande;");

            Console.WriteLine("\nVoici les commandes avec le nombre total de plats :");
            // Nombre total de plats dans chaque commande
            Read(command, reader,
                "SELECT Commande.IDCommande, COUNT(Contient.IdPlat) AS NombreDePlats " +
                "FROM Commande " +
                "JOIN Contient ON Contient.IdCommande = Commande.IDCommande " +
                "GROUP BY Commande.IDCommande " +
                "ORDER BY NombreDePlats DESC;");

            Console.WriteLine("\nVoici la satisfaction moyenne des commandes :");
            // Satisfaction moyenne des commandes
            Read(command, reader,
                "SELECT AVG(Satisfaction) AS SatisfactionMoyenne " +
                "FROM Commande;");

            Console.WriteLine("\nVoici les informations détaillées sur les plats :");
            // Détails sur les plats (nom, prix, quantités)
            Read(command, reader,
                "SELECT Plat.NomPlat, Plat.Prix, Plat.Quantite, COUNT(Contient.IdPlat) AS QuantiteVendue " +
                "FROM Plat " +
                "JOIN Contient ON Plat.IdPlat = Contient.IdPlat " +
                "GROUP BY Plat.IdPlat " +
                "ORDER BY QuantiteVendue DESC;");
            Console.WriteLine("Appuyez sur une touche pour revenir au menu.");
            Console.ReadKey();
            Console.Clear();
        }

        static void GestionClients(MySqlCommand command, MySqlDataReader reader)
        {
            string choix = "";
            while (choix != "0")
            {
                Console.Clear();
                Console.WriteLine("Gestion des clients...");
                try
                {
                    Console.WriteLine("1. Créer un particulier");
                    Console.WriteLine("2. Mettre à jour un particulier");
                    Console.WriteLine("3. Supprimer un particulier");
                    Console.WriteLine("4. Créer une entreprise client");
                    Console.WriteLine("5. Mettre à jour une entreprise client");
                    Console.WriteLine("6. Supprimer une entreprise client");
                    Console.WriteLine("7. Afficher les particuliers");
                    Console.WriteLine("8. Afficher les entreprises clients");
                    Console.WriteLine("0. Retourner au menu principal");
                    Console.Write("Entrez le numéro de l'action souhaitée : ");
                    choix = Console.ReadLine();
                    switch (choix)
                    {
                        case "1":
                            CreerParticulier(command);
                            break;
                        case "2":
                            MajParticulier(command);
                            break;
                        case "3":
                            SupprimerParticulier(command);
                            break;
                        case "4":
                            CreerEntreprise(command);
                            break;
                        case "5":
                            MajEntreprise(command);
                            break;
                        case "6":
                            SupprimerEntreprise(command);
                            break;
                        case "7":
                            AfficherParticulier(command, reader);
                            break;
                        case "8":
                            AfficherEntreprise(command, reader);
                            break;
                        case "9":
                            Console.WriteLine("Retour au menu principal...");
                            break;
                        default:
                            Console.WriteLine("Option invalide");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erreur lors de la gestion des clients : " + ex.Message);
                    System.Threading.Thread.Sleep(1000);
                }
            }
        }

        static void CreerParticulier(MySqlCommand command)
        {
            Console.Clear();
            Console.WriteLine("Création d'un particulier...");

            try
            {
                Console.Write("Entrez le numéro du particulier : ");
                if (!int.TryParse(Console.ReadLine(), out int NumeroParticulier))
                {
                    Console.WriteLine("Le numéro du particulier doit être un nombre.");
                    System.Threading.Thread.Sleep(1000);
                    return;
                }

                if (ParticulierExiste(NumeroParticulier, command))
                {
                    Console.WriteLine("Un particulier avec ce numéro existe déjà.");
                    System.Threading.Thread.Sleep(1000);
                    return;
                }

                Console.Write("Entrez le nom du particulier : ");
                string NomP = Console.ReadLine();

                Console.Write("Entrez le prénom du particulier : ");
                string PrenomP = Console.ReadLine();

                Console.Write("Entrez l'adresse du particulier : ");
                string AdresseP = Console.ReadLine();

                Console.Write("Entrez le code postal : ");
                string CodePostalP = Console.ReadLine();

                Console.Write("Entrez le numéro de téléphone : ");
                string TelP = Console.ReadLine();

                Console.Write("Entrez l'email : ");
                string EmailP = Console.ReadLine();

                Console.Write("Entrez la station de métro la plus proche : ");
                string MetroP = Console.ReadLine();

                command.CommandText = "INSERT INTO Particulier (NumeroParticulier, NomP, PrenomP, AdresseP, CodePostalP, TelP, EmailP, MetroP) " +
                                      "VALUES (@NumeroParticulier, @NomP, @PrenomP, @AdresseP, @CodePostalP, @TelP, @EmailP, @MetroP)";

                command.Parameters.Clear();
                command.Parameters.AddWithValue("@NumeroParticulier", NumeroParticulier);
                command.Parameters.AddWithValue("@NomP", NomP);
                command.Parameters.AddWithValue("@PrenomP", PrenomP);
                command.Parameters.AddWithValue("@AdresseP", AdresseP);
                command.Parameters.AddWithValue("@CodePostalP", CodePostalP);
                command.Parameters.AddWithValue("@TelP", TelP);
                command.Parameters.AddWithValue("@EmailP", EmailP);
                command.Parameters.AddWithValue("@MetroP", MetroP);

                command.ExecuteNonQuery();
                Console.WriteLine("Particulier créé avec succès !");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur lors de la création du particulier : " + ex.Message);
            }

            System.Threading.Thread.Sleep(1000);
        }
        static void SupprimerParticulier(MySqlCommand command)
        {
            Console.Clear();
            Console.WriteLine("Suppression d'un particulier : ");
            try
            {
                command.CommandText = "DELETE FROM Particulier WHERE NumeroParticulier = @NumeroParticulier";
                Console.Write("Entrez le numéro du particulier à supprimer : ");
                if (!int.TryParse(Console.ReadLine(), out int NumeroParticulier))
                {
                    Console.WriteLine("Le numéro du particulier doit être un nombre.");
                    System.Threading.Thread.Sleep(1000);
                    return;
                }
                //int NumeroParticulier = Console.ReadLine();
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@NumeroParticulier", NumeroParticulier);
                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected > 0)
                    Console.WriteLine("Particulier supprimé avec succès !");
                else
                    Console.WriteLine("Aucun particulier trouvé avec ce numéro.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur lors de la suppression du particulier : " + ex.Message);
            }

            System.Threading.Thread.Sleep(1000);
        }
        static void MajParticulier(MySqlCommand command)
        {
            Console.Clear();
            Console.WriteLine("Mise à jour d'un particulier");
            Console.Write("Entrez le numéro du particulier à mettre à jour : ");
            if (!int.TryParse(Console.ReadLine(), out int NumeroParticulier))
            {
                Console.WriteLine("Le numéro du particulier doit être un nombre.");
                System.Threading.Thread.Sleep(1000);
                return;
            }

            Console.Write("Entrez la nouvelle adresse du particulier : ");
            string AdresseP = Console.ReadLine();

            Console.Write("Entrez le nouveau code postal : ");
            string CodePostalP = Console.ReadLine();

            Console.Write("Entrez le nouveau numéro de téléphone : ");
            string TelP = Console.ReadLine();

            Console.Write("Entrez le nouvel email : ");
            string EmailP = Console.ReadLine();

            Console.Write("Entrez le nouveau métro : ");
            string MetroP = Console.ReadLine();

            command.Parameters.Clear();
            command.CommandText = "UPDATE Particulier SET AdresseP = @AdresseP, CodePostalP = @CodePostalP, TelP = @TelP, EmailP = @EmailP, MetroP = @MetroP WHERE NumeroParticulier = @NumeroParticulier";
            command.Parameters.AddWithValue("@NumeroParticulier", NumeroParticulier);
            command.Parameters.AddWithValue("@AdresseP", AdresseP);
            command.Parameters.AddWithValue("@CodePostalP", CodePostalP);
            command.Parameters.AddWithValue("@TelP", TelP);
            command.Parameters.AddWithValue("@EmailP", EmailP);
            command.Parameters.AddWithValue("@MetroP", MetroP);

            int rowsAffected = command.ExecuteNonQuery();
            if (rowsAffected > 0)
            {
                Console.WriteLine("Particulier mis à jour avec succès !");
            }
            else
            {
                Console.WriteLine("Aucun particulier mis à jour. Vérifiez que le numéro est correct.");
            }

            System.Threading.Thread.Sleep(1000);
        }
        static void AfficherParticulier(MySqlCommand command, MySqlDataReader reader)
        {
            Console.Clear();
            command.CommandText = "SELECT * FROM Particulier;";
            reader = command.ExecuteReader();
            Console.WriteLine("Particuliers :");
            while (reader.Read())
            {
                string currentRowAsString = "";
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    string valueAsString = reader.GetValue(i).ToString();
                    currentRowAsString += i == reader.FieldCount - 1 ? valueAsString : valueAsString + ", ";
                }
                Console.WriteLine(currentRowAsString);
            }
            reader.Close();
            Console.WriteLine("Appuyez sur une touche pour continuer");
            Console.ReadKey();
            Console.Clear();
        }

        static void CreerEntreprise(MySqlCommand command)
        {
            Console.Clear();
            Console.WriteLine("Création d'une entreprise cliente...");

            try
            {
                Console.Write("Entrez le numéro de l'entreprise : ");
                if (!int.TryParse(Console.ReadLine(), out int NumeroEntreprise))
                {
                    Console.WriteLine("Le numéro d'entreprise doit être un nombre.");
                    System.Threading.Thread.Sleep(1000);
                    return;
                }
                if (EntrepriseExiste(NumeroEntreprise, command))
                {
                    Console.WriteLine("Une entreprise avec ce numéro existe déjà.");
                    System.Threading.Thread.Sleep(1000);
                    return;
                }

                Console.Write("Entrez le nom de l'entreprise : ");
                string NomE = Console.ReadLine();

                Console.Write("Entrez le contact de l'entreprise : ");
                string ContactE = Console.ReadLine();

                Console.Write("Entrez l'adresse de l'entreprise : ");
                string AdresseE = Console.ReadLine();

                Console.Write("Entrez le code postal de l'entreprise : ");
                string CodePostalE = Console.ReadLine();

                Console.Write("Entrez le numéro de téléphone de l'entreprise : ");
                string TelE = Console.ReadLine();

                Console.Write("Entrez l'email de l'entreprise : ");
                string EmailE = Console.ReadLine();

                Console.Write("Entrez la station de métro la plus proche : ");
                string MetroE = Console.ReadLine();

                command.CommandText = "INSERT INTO Entreprise (NumeroEntreprise, NomE, ContactE, AdresseE, CodePostalE, TelE, EmailE, MetroE) " +
                                      "VALUES (@NumeroEntreprise, @NomE, @ContactE, @AdresseE, @CodePostalE, @TelE, @EmailE, @MetroE)";

                command.Parameters.Clear();
                command.Parameters.AddWithValue("@NumeroEntreprise", NumeroEntreprise);
                command.Parameters.AddWithValue("@NomE", NomE);
                command.Parameters.AddWithValue("@ContactE", ContactE);
                command.Parameters.AddWithValue("@AdresseE", AdresseE);
                command.Parameters.AddWithValue("@CodePostalE", CodePostalE);
                command.Parameters.AddWithValue("@TelE", TelE);
                command.Parameters.AddWithValue("@EmailE", EmailE);
                command.Parameters.AddWithValue("@MetroE", MetroE);

                command.ExecuteNonQuery();
                Console.WriteLine("Entreprise créée avec succès !");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur lors de la création de l'entreprise : " + ex.Message);
            }

            System.Threading.Thread.Sleep(1000);
        }
        static void MajEntreprise(MySqlCommand command)
        {
            Console.Clear();
            Console.WriteLine("Mise à jour d'une entreprise client");
            Console.Write("Entrez le numéro de l'entreprise à mettre à jour : ");

            if (!int.TryParse(Console.ReadLine(), out int NumeroEntreprise))
            {
                Console.WriteLine("L'entrée n'est pas un nombre valide.");
                System.Threading.Thread.Sleep(1000);
                return;
            }

            Console.Write("Entrez le nouveau contact de l'entreprise : ");
            string ContactE = Console.ReadLine();

            Console.Write("Entrez la nouvelle adresse de l'entreprise : ");
            string AdresseE = Console.ReadLine();

            Console.Write("Entrez le nouveau code postal : ");
            string CodePostalE = Console.ReadLine();

            Console.Write("Entrez le nouveau numéro de téléphone : ");
            string TelE = Console.ReadLine();

            Console.Write("Entrez le nouvel email : ");
            string EmailE = Console.ReadLine();

            Console.Write("Entrez le nouveau métro : ");
            string MetroE = Console.ReadLine();

            command.Parameters.Clear();
            command.CommandText = "UPDATE Entreprise SET ContactE = @ContactE, AdresseE = @AdresseE, CodePostalE = @CodePostalE, TelE = @TelE, EmailE = @EmailE, MetroE = @MetroE WHERE NumeroEntreprise = @NumeroEntreprise";
            command.Parameters.AddWithValue("@NumeroEntreprise", NumeroEntreprise);
            command.Parameters.AddWithValue("@ContactE", ContactE);
            command.Parameters.AddWithValue("@AdresseE", AdresseE);
            command.Parameters.AddWithValue("@CodePostalE", CodePostalE);
            command.Parameters.AddWithValue("@TelE", TelE);
            command.Parameters.AddWithValue("@EmailE", EmailE);
            command.Parameters.AddWithValue("@MetroE", MetroE);

            int rowsAffected = command.ExecuteNonQuery();

            if (rowsAffected > 0)
            {
                Console.WriteLine("Entreprise mise à jour avec succès !");
            }
            else
            {
                Console.WriteLine("Aucune entreprise mise à jour. Vérifiez que le numéro est correct.");
            }

            System.Threading.Thread.Sleep(1000);
        }
        static void SupprimerEntreprise(MySqlCommand command)
        {
            Console.Clear();
            Console.WriteLine("Suppression d'une entreprise cliente : ");
            try
            {
                command.CommandText = "DELETE FROM Entreprise WHERE NumeroEntreprise = @NumeroEntreprise";
                Console.Write("Entrez le numéro de l'entreprise à supprimer : ");
                if (!int.TryParse(Console.ReadLine(), out int NumeroEntreprise))
                {
                    Console.WriteLine("Le numéro d'entreprise doit être un nombre.");
                    System.Threading.Thread.Sleep(1000);
                    return;
                }

                command.Parameters.Clear();
                command.Parameters.AddWithValue("@NumeroEntreprise", NumeroEntreprise);

                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected > 0)
                    Console.WriteLine("Entreprise supprimée avec succès !");
                else
                    Console.WriteLine("Aucune entreprise trouvée avec cet ID.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur lors de la suppression de l'entreprise : " + ex.Message);
            }
            System.Threading.Thread.Sleep(1000);
        }
        static void AfficherEntreprise(MySqlCommand command, MySqlDataReader reader)
        {
            Console.Clear();
            command.CommandText = "SELECT * FROM Entreprise;";
            reader = command.ExecuteReader();
            Console.WriteLine("Entreprises clientes :");
            while (reader.Read())
            {
                string currentRowAsString = "";
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    string valueAsString = reader.GetValue(i).ToString();
                    currentRowAsString += i == reader.FieldCount - 1 ? valueAsString : valueAsString + ", ";
                }
                Console.WriteLine(currentRowAsString);
            }
            reader.Close();
            Console.WriteLine("Appuyez sur une touche pour continuer");
            Console.ReadKey();
            Console.Clear();
        }

        static void GestionCommandes(MySqlCommand command, MySqlDataReader reader)
        {
            string choix = "";
            while (choix != "0")
            {
                Console.Clear();
                Console.WriteLine("Gestion des commandes...");
                try
                {
                    Console.WriteLine("1. Créer une commande");
                    Console.WriteLine("2. Mettre à jour une commande");
                    Console.WriteLine("3. Supprimer une commande");
                    Console.WriteLine("4. Afficher les commandes existantes");
                    Console.WriteLine("5. Afficher le contenu des commandes");
                    Console.WriteLine("0. Retourner au menu principal");
                    Console.Write("Entrez le numéro de l'action souhaitée : ");
                    choix = Console.ReadLine();

                    switch (choix)
                    {
                        case "1":
                            CreerCommande(command);
                            break;
                        case "2":
                            MajCommande(command);
                            break;
                        case "3":
                            SupprimerCommande(command);
                            break;
                        case "4":
                            AfficherCommande(command, reader);
                            break;
                        case "5":
                            AfficherContenu(command, reader);
                            break;
                        case "0":
                            Console.WriteLine("Retour au menu principal...");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erreur lors de la gestion des commandes : " + ex.Message);
                    System.Threading.Thread.Sleep(1000);
                }
            }
        }
        static void CreerCommande(MySqlCommand command)
        {
            Console.Clear();
            Console.WriteLine("Création d'une nouvelle commande...");

            try
            {
                Console.Write("Entrez l'ID de la commande : ");
                if (!int.TryParse(Console.ReadLine(), out int idCommande))
                {
                    Console.WriteLine("ID invalide. Veuillez entrer un nombre entier.");
                    Thread.Sleep(1000);
                    return;
                }

                if (CommandeExiste(idCommande, command))
                {
                    Console.WriteLine("Une commande avec cet ID existe déjà.");
                    Thread.Sleep(1000);
                    return;
                }

                Console.Write("Entrez la date de la commande (format YYYY-MM-DD) : ");
                if (!DateTime.TryParse(Console.ReadLine(), out DateTime dateCommande))
                {
                    Console.WriteLine("La date de commande n'est pas valide.");
                    Thread.Sleep(1000);
                    return;
                }

                Console.Write("Entrez l'adresse de livraison : ");
                string adresseLivraison = Console.ReadLine();

                Console.Write("Entrez le niveau de satisfaction (entre 0 et 10) : ");
                if (!int.TryParse(Console.ReadLine(), out int satisfaction) || satisfaction < 0 || satisfaction > 10)
                {
                    Console.WriteLine("Satisfaction invalide.");
                    Thread.Sleep(1000);
                    return;
                }

                Console.Write("Entrez le numéro du cuisinier associé à la commande : ");
                if (!int.TryParse(Console.ReadLine(), out int numeroCuisinier))
                {
                    Console.WriteLine("Numéro invalide.");
                    Thread.Sleep(1000);
                    return;
                }

                if (!CuisinierExiste(numeroCuisinier, command))
                {
                    Console.WriteLine("Le cuisinier spécifié n'existe pas.");
                    Thread.Sleep(1000);
                    return;
                }

                Console.Write("Commande pour un particulier ou une entreprise ? (p/e) : ");
                string choix = Console.ReadLine().ToLower();
                int numeroParticulier = 0, numeroEntreprise = 0;

                if (choix == "e")
                {
                    Console.Write("Numéro de l'entreprise : ");
                    if (!int.TryParse(Console.ReadLine(), out numeroEntreprise) || !EntrepriseExiste(numeroEntreprise, command))
                    {
                        Console.WriteLine("Numéro d'entreprise invalide ou introuvable.");
                        Thread.Sleep(1000);
                        return;
                    }
                }
                else if (choix == "p")
                {
                    Console.Write("Numéro du particulier : ");
                    if (!int.TryParse(Console.ReadLine(), out numeroParticulier) || !ParticulierExiste(numeroParticulier, command))
                    {
                        Console.WriteLine("Numéro de particulier invalide ou introuvable.");
                        Thread.Sleep(1000);
                        return;
                    }
                }
                else
                {
                    Console.WriteLine("Choix invalide.");
                    Thread.Sleep(1000);
                    return;
                }

                // Insertion dans Commande
                command.Parameters.Clear();
                command.CommandText = @"
            INSERT INTO Commande (IDCommande, DateCommande, AdresseLivraison, Satisfaction, NumeroCuisinier, 
                                  NumeroEntreprise, NumeroParticulier)
            VALUES (@IDCommande, @DateCommande, @AdresseLivraison, @Satisfaction, @NumeroCuisinier, 
                    @NumeroEntreprise, @NumeroParticulier)";
                command.Parameters.AddWithValue("@IDCommande", idCommande);
                command.Parameters.AddWithValue("@DateCommande", dateCommande);
                command.Parameters.AddWithValue("@AdresseLivraison", adresseLivraison);
                command.Parameters.AddWithValue("@Satisfaction", satisfaction);
                command.Parameters.AddWithValue("@NumeroCuisinier", numeroCuisinier);
                command.Parameters.AddWithValue("@NumeroEntreprise", (choix == "e") ? numeroEntreprise : (object)DBNull.Value);
                command.Parameters.AddWithValue("@NumeroParticulier", (choix == "p") ? numeroParticulier : (object)DBNull.Value);
                command.ExecuteNonQuery();

                // Ajout des plats à la commande
                while (true)
                {
                    Console.Write("Entrez l'ID d'un plat à ajouter à la commande (ou laissez vide pour terminer) : ");
                    string input = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(input)) break;

                    if (!int.TryParse(input, out int idPlat))
                    {
                        Console.WriteLine("ID de plat invalide.");
                        continue;
                    }

                    if (!PlatExiste(idPlat, command))
                    {
                        Console.WriteLine("Aucun plat avec cet ID.");
                        continue;
                    }

                    // Insertion dans Contient sans quantité
                    command.Parameters.Clear();
                    command.CommandText = "INSERT INTO Contient (IdCommande, IdPlat) VALUES (@IdCommande, @IdPlat)";
                    command.Parameters.AddWithValue("@IdCommande", idCommande);
                    command.Parameters.AddWithValue("@IdPlat", idPlat);
                    command.ExecuteNonQuery();

                    Console.WriteLine("Plat ajouté !");
                }

                Console.WriteLine("Commande créée avec ses plats !");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur lors de la création de la commande : " + ex.Message);
            }

            Thread.Sleep(1000);
        }
        static void MajCommande(MySqlCommand command)
        {
            Console.Clear();
            Console.WriteLine("Mise à jour d'une commande...");

            Console.Write("Entrez l'ID de la commande à mettre à jour : ");
            if (!int.TryParse(Console.ReadLine(), out int idCommande))
            {
                Console.WriteLine("ID invalide. Veuillez entrer un nombre entier.");
                System.Threading.Thread.Sleep(1000);
                return;
            }

            if (!CommandeExiste(idCommande, command))
            {
                Console.WriteLine("La commande spécifiée n'existe pas.");
                System.Threading.Thread.Sleep(1000);
                return;
            }

            Console.Write("Entrez la nouvelle adresse de livraison : ");
            string adresseLivraison = Console.ReadLine();

            Console.Write("Entrez le nouveau niveau de satisfaction (entre 0 et 10) : ");
            if (!int.TryParse(Console.ReadLine(), out int satisfaction) || satisfaction < 0 || satisfaction > 10)
            {
                Console.WriteLine("Satisfaction invalide. Veuillez entrer un nombre entier entre 0 et 10.");
                System.Threading.Thread.Sleep(1000);
                return;
            }

            Console.Write("Entrez le numéro du cuisinier associé à la commande : ");
            if (!int.TryParse(Console.ReadLine(), out int numeroCuisinier))
            {
                Console.WriteLine("Numéro invalide. Veuillez entrer un nombre entier.");
                System.Threading.Thread.Sleep(1000);
                return;
            }

            Console.Write("La commande est-elle réalisée par un particulier ou une entreprise ? (p/e) : ");
            string choix = Console.ReadLine().ToLower();
            int numeroParticulier = 0, numeroEntreprise = 0;

            if (choix == "e")
            {
                Console.Write("Entrez le numéro de l'entreprise associée à la commande : ");
                if (!int.TryParse(Console.ReadLine(), out numeroEntreprise))
                {
                    Console.WriteLine("Numéro invalide. Veuillez entrer un nombre entier.");
                    System.Threading.Thread.Sleep(1000);
                    return;
                }
            }
            else if (choix == "p")
            {
                Console.Write("Entrez le numéro du particulier associé à la commande : ");
                if (!int.TryParse(Console.ReadLine(), out numeroParticulier))
                {
                    Console.WriteLine("Numéro invalide. Veuillez entrer un nombre entier.");
                    System.Threading.Thread.Sleep(1000);
                    return;
                }
            }
            else
            {
                Console.WriteLine("Choix invalide. Tapez 'p' pour particulier ou 'e' pour entreprise.");
                System.Threading.Thread.Sleep(1000);
                return;
            }

            command.Parameters.Clear();
            command.CommandText = "UPDATE Commande SET " +
                                  "AdresseLivraison = @AdresseLivraison, " +
                                  "Satisfaction = @Satisfaction, " +
                                  "NumeroCuisinier = @NumeroCuisinier, " +
                                  "NumeroParticulier = @NumeroParticulier, " +
                                  "NumeroEntreprise = @NumeroEntreprise " +
                                  "WHERE IDCommande = @IDCommande";

            command.Parameters.AddWithValue("@IDCommande", idCommande);
            command.Parameters.AddWithValue("@AdresseLivraison", adresseLivraison);
            command.Parameters.AddWithValue("@Satisfaction", satisfaction);
            command.Parameters.AddWithValue("@NumeroCuisinier", numeroCuisinier);
            command.Parameters.AddWithValue("@NumeroParticulier", numeroParticulier == 0 ? DBNull.Value : (object)numeroParticulier);
            command.Parameters.AddWithValue("@NumeroEntreprise", numeroEntreprise == 0 ? DBNull.Value : (object)numeroEntreprise);

            int rowsAffected = command.ExecuteNonQuery();
            if (rowsAffected > 0)
            {
                Console.WriteLine("Commande mise à jour avec succès !");
            }
            else
            {
                Console.WriteLine("Aucune commande mise à jour. Vérifiez que l'ID de la commande est correct.");
            }
            System.Threading.Thread.Sleep(1000);
        }
        static void SupprimerCommande(MySqlCommand command)
        {
            Console.Clear();
            Console.WriteLine("Suppression d'une commande...");
            try
            {
                Console.Write("Entrez l'ID de la commande à supprimer : ");
                if (!int.TryParse(Console.ReadLine(), out int idCommande))
                {
                    Console.WriteLine("ID invalide. Veuillez entrer un nombre entier.");
                    System.Threading.Thread.Sleep(1000);
                    return;
                }

                if (!CommandeExiste(idCommande, command))
                {
                    Console.WriteLine("La commande spécifiée n'existe pas.");
                    System.Threading.Thread.Sleep(1000);
                    return;
                }

                command.Parameters.Clear();
                command.CommandText = "DELETE FROM Commande WHERE IDCommande = @IDCommande";
                command.Parameters.AddWithValue("@IDCommande", idCommande);

                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    Console.WriteLine("Commande supprimée avec succès !");
                }
                else
                {
                    Console.WriteLine("Aucune commande supprimée. Vérifiez que l'ID de la commande est correct.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur lors de la suppression de la commande : " + ex.Message);
            }
            System.Threading.Thread.Sleep(1000);
        }
        static void AfficherCommande(MySqlCommand command, MySqlDataReader reader)
        {
            Console.Clear();
            command.CommandText = "SELECT * FROM Commande;";
            reader = command.ExecuteReader();
            Console.WriteLine("Commandes :");
            while (reader.Read())
            {
                string currentRowAsString = "";
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    string valueAsString = reader.GetValue(i).ToString();
                    currentRowAsString += i == reader.FieldCount - 1 ? valueAsString : valueAsString + ", ";
                }
                Console.WriteLine(currentRowAsString);
            }
            reader.Close();
            Console.WriteLine("Appuyez sur une touche pour continuer");
            Console.ReadKey();
            Console.Clear();
        }
        static void AfficherContenu(MySqlCommand command, MySqlDataReader reader)
        {
            Console.Clear();

            command.CommandText = @"
        SELECT 
            cmd.IDCommande,
            cmd.DateCommande,
            cmd.AdresseLivraison,
            cmd.Satisfaction,

            c.NomC,
            c.PrenomC,
            c.MetroC,

            p.NomP,
            p.PrenomP,
            p.MetroP,

            e.NomE,
            e.MetroE,

            plat.NomPlat,
            plat.Prix

        FROM Commande cmd
        LEFT JOIN Cuisinier c ON cmd.NumeroCuisinier = c.NumeroCuisinier
        LEFT JOIN Particulier p ON cmd.NumeroParticulier = p.NumeroParticulier
        LEFT JOIN Entreprise e ON cmd.NumeroEntreprise = e.NumeroEntreprise
        LEFT JOIN Contient co ON cmd.IDCommande = co.IdCommande
        LEFT JOIN Plat plat ON co.IdPlat = plat.IdPlat
        ORDER BY cmd.IDCommande;
    ";

            reader = command.ExecuteReader();
            Console.WriteLine("=== Contenu des Commandes ===\n");

            int lastCommandeId = -1;

            while (reader.Read())
            {
                int currentCommandeId = Convert.ToInt32(reader["IDCommande"]);
                if (currentCommandeId != lastCommandeId)
                {
                    Console.WriteLine($"\n Commande #{reader["IDCommande"]} - Date : {reader["DateCommande"]} - Satisfaction : {reader["Satisfaction"]}/10");
                    Console.WriteLine($"   Adresse de livraison : {reader["AdresseLivraison"]}");

                    // Cuisinier
                    Console.WriteLine($"    Cuisinier : {reader["NomC"]} {reader["PrenomC"]} (Métro : {reader["MetroC"]})");

                    // Client : soit Particulier soit Entreprise
                    if (!reader.IsDBNull(reader.GetOrdinal("NomP")))
                    {
                        Console.WriteLine($"    Client Particulier : {reader["NomP"]} {reader["PrenomP"]} (Métro : {reader["MetroP"]})");
                    }
                    else if (!reader.IsDBNull(reader.GetOrdinal("NomE")))
                    {
                        Console.WriteLine($"    Client Entreprise : {reader["NomE"]} (Métro : {reader["MetroE"]})");
                    }

                    Console.WriteLine("    Plats commandés :");
                    lastCommandeId = currentCommandeId;
                }

                // Affichage des plats si présents
                if (!reader.IsDBNull(reader.GetOrdinal("NomPlat")))
                {
                    Console.WriteLine($"    {reader["NomPlat"]} ({reader["Prix"]} euros)");
                }
            }

            reader.Close();
            Console.WriteLine("\nAppuyez sur une touche pour continuer...");
            Console.ReadKey();
            Console.Clear();
        }



        static void GestionCuisinier(MySqlCommand command, MySqlDataReader reader)
        {
            string choix = "";
            while (choix != "0")
            {
                Console.Clear();
                Console.WriteLine("Gestion des Cuisiniers...");
                try
                {
                    Console.WriteLine("1. Créer un cuisinier");
                    Console.WriteLine("2. Mettre à jour un cuisinier");
                    Console.WriteLine("3. Supprimer un cuisinier");
                    Console.WriteLine("4. Afficher les cuisiniers existants");
                    Console.WriteLine("0. Retourner au menu principal");
                    Console.Write("Entrez le numéro de l'action souhaitée : ");
                    choix = Console.ReadLine();

                    switch (choix)
                    {
                        case "1":
                            CreerCuisinier(command);
                            break;
                        case "2":
                            MajCuisinier(command);
                            break;
                        case "3":
                            SupprimerCuisinier(command);
                            break;
                        case "4":
                            AfficherCuisiniers(command, reader);
                            break;
                        case "5":
                            Console.WriteLine("Retour au menu principal...");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erreur lors de la gestion des commandes : " + ex.Message);
                    System.Threading.Thread.Sleep(1000);
                }
            }
        }
        static void CreerCuisinier(MySqlCommand command)
        {
            Console.Clear();
            Console.WriteLine("Création d'un cuisinier...");

            try
            {
                Console.Write("Entrez le numéro du cuisinier : ");
                if (!int.TryParse(Console.ReadLine(), out int NumeroCuisinier))
                {
                    Console.WriteLine("Le numéro du cuisinier doit être un nombre.");
                    System.Threading.Thread.Sleep(1000);
                    return;
                }

                if (CuisinierExiste(NumeroCuisinier, command))
                {
                    Console.WriteLine("Un cuisinier avec ce numéro existe déjà.");
                    System.Threading.Thread.Sleep(1000);
                    return;
                }

                Console.Write("Entrez le nom du cuisinier : ");
                string NomC = Console.ReadLine();

                Console.Write("Entrez le prénom du cuisinier : ");
                string PrenomC = Console.ReadLine();

                Console.Write("Entrez l'adresse du cuisinier : ");
                string AdresseC = Console.ReadLine();

                Console.Write("Entrez le code postal du cuisinier : ");
                string CodePostalC = Console.ReadLine();

                Console.Write("Entrez la ville du cuisinier : ");
                string VilleC = Console.ReadLine();

                Console.Write("Entrez le numéro de téléphone du cuisinier : ");
                string TelC = Console.ReadLine();

                Console.Write("Entrez l'email du cuisinier : ");
                string EmailC = Console.ReadLine();

                Console.Write("Entrez la station de métro la plus proche : ");
                string MetroC = Console.ReadLine();

                command.CommandText = "INSERT INTO Cuisinier (NumeroCuisinier, NomC, PrenomC, AdresseC, CodePostalC, VilleC, TelC, EmailC, MetroC) " +
                                      "VALUES (@NumeroCuisinier, @NomC, @PrenomC, @AdresseC, @CodePostalC, @VilleC, @TelC, @EmailC, @MetroC)";

                command.Parameters.Clear();
                command.Parameters.AddWithValue("@NumeroCuisinier", NumeroCuisinier);
                command.Parameters.AddWithValue("@NomC", NomC);
                command.Parameters.AddWithValue("@PrenomC", PrenomC);
                command.Parameters.AddWithValue("@AdresseC", AdresseC);
                command.Parameters.AddWithValue("@CodePostalC", CodePostalC);
                command.Parameters.AddWithValue("@VilleC", VilleC);
                command.Parameters.AddWithValue("@TelC", TelC);
                command.Parameters.AddWithValue("@EmailC", EmailC);
                command.Parameters.AddWithValue("@MetroC", MetroC);

                command.ExecuteNonQuery();
                Console.WriteLine("Cuisinier créé avec succès !");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur lors de la création du cuisinier : " + ex.Message);
            }

            System.Threading.Thread.Sleep(1000);
        }
        static void MajCuisinier(MySqlCommand command)
        {
            Console.Clear();
            Console.WriteLine("Mise à jour d'un cuisinier");
            Console.Write("Entrez le numéro du cuisinier à mettre à jour : ");

            if (!int.TryParse(Console.ReadLine(), out int NumeroCuisinier))
            {
                Console.WriteLine("L'entrée n'est pas un nombre valide.");
                System.Threading.Thread.Sleep(1000);
                return;
            }

            Console.Write("Entrez la nouvelle adresse du cuisinier : ");
            string AdresseC = Console.ReadLine();

            Console.Write("Entrez le nouveau code postal : ");
            string CodePostalC = Console.ReadLine();

            Console.Write("Entrez la nouvelle ville : ");
            string VilleC = Console.ReadLine();

            Console.Write("Entrez le nouveau numéro de téléphone : ");
            string TelC = Console.ReadLine();

            Console.Write("Entrez le nouvel email : ");
            string EmailC = Console.ReadLine();

            Console.Write("Entrez le nouveau métro : ");
            string MetroC = Console.ReadLine();

            command.Parameters.Clear();
            command.CommandText = "UPDATE Cuisinier SET AdresseC = @AdresseC, CodePostalC = @CodePostalC, VilleC = @VilleC, TelC = @TelC, EmailC = @EmailC, MetroC = @MetroC WHERE NumeroCuisinier = @NumeroCuisinier";
            command.Parameters.AddWithValue("@NumeroCuisinier", NumeroCuisinier);
            command.Parameters.AddWithValue("@AdresseC", AdresseC);
            command.Parameters.AddWithValue("@CodePostalC", CodePostalC);
            command.Parameters.AddWithValue("@VilleC", VilleC);
            command.Parameters.AddWithValue("@TelC", TelC);
            command.Parameters.AddWithValue("@EmailC", EmailC);
            command.Parameters.AddWithValue("@MetroC", MetroC);

            int rowsAffected = command.ExecuteNonQuery();

            if (rowsAffected > 0)
            {
                Console.WriteLine("Cuisinier mis à jour avec succès !");
            }
            else
            {
                Console.WriteLine("Aucun cuisinier mis à jour. Vérifiez que le numéro est correct.");
            }

            System.Threading.Thread.Sleep(1000);
        }
        static void SupprimerCuisinier(MySqlCommand command)
        {
            Console.Clear();
            Console.WriteLine("Suppression d'un cuisinier : ");
            try
            {
                command.CommandText = "DELETE FROM Cuisinier WHERE NumeroCuisinier = @NumeroCuisinier";
                Console.Write("Entrez le numéro du cuisinier à supprimer : ");

                if (!int.TryParse(Console.ReadLine(), out int NumeroCuisinier))
                {
                    Console.WriteLine("Le numéro du cuisinier doit être un nombre.");
                    System.Threading.Thread.Sleep(1000);
                    return;
                }
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@NumeroCuisinier", NumeroCuisinier);

                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected > 0)
                    Console.WriteLine("Cuisinier supprimé avec succès !");
                else
                    Console.WriteLine("Aucun cuisinier trouvé avec cet ID.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur lors de la suppression du cuisinier : " + ex.Message);
            }
            System.Threading.Thread.Sleep(1000);
        }
        static void AfficherCuisiniers(MySqlCommand command, MySqlDataReader reader)
        {
            Console.Clear();
            command.CommandText = "SELECT * FROM Cuisinier;";

            try
            {
                reader = command.ExecuteReader();
                Console.WriteLine("Liste des cuisiniers :");

                while (reader.Read())
                {
                    string currentRowAsString = "";
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        string valueAsString = reader.GetValue(i).ToString();
                        currentRowAsString += i == reader.FieldCount - 1 ? valueAsString : valueAsString + ", ";
                    }
                    Console.WriteLine(currentRowAsString);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur lors de l'affichage des cuisiniers : " + ex.Message);
                if (reader != null && !reader.IsClosed) reader.Close();
            }

            Console.WriteLine("Appuyez sur une touche pour continuer");
            Console.ReadKey();
            Console.Clear();
        }

        static void GestionPlat(MySqlCommand command, MySqlDataReader reader)
        {
            string choix = "";
            while (choix != "0")
            {
                Console.Clear();
                Console.WriteLine("Gestion des Plats...");
                try
                {
                    Console.WriteLine("1. Créer un plat");
                    Console.WriteLine("2. Mettre à jour un plat");
                    Console.WriteLine("3. Supprimer un plat");
                    Console.WriteLine("4. Afficher les plats existants");
                    Console.WriteLine("0. Retourner au menu principal");
                    Console.Write("Entrez le numéro de l'action souhaitée : ");
                    choix = Console.ReadLine();

                    switch (choix)
                    {
                        case "1":
                            CreerPlat(command);
                            break;
                        case "2":
                            MajPlat(command);
                            break;
                        case "3":
                            SupprimerPlat(command);
                            break;
                        case "4":
                            AfficherPlat(command, reader);
                            break;
                        case "5":
                            Console.WriteLine("Retour au menu principal...");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erreur lors de la gestion des commandes : " + ex.Message);
                    System.Threading.Thread.Sleep(1000);
                }
            }
        }
        static void CreerPlat(MySqlCommand command)
        {
            Console.Clear();
            Console.WriteLine("Création d'un plat");

            try
            {
                Console.Write("Entrez le numéro du cuisinier responsable : ");
                if (!int.TryParse(Console.ReadLine(), out int NumeroCuisinier))
                {
                    Console.WriteLine("Le numéro du cuisinier doit être un nombre.");
                    System.Threading.Thread.Sleep(1000);
                    return;
                }
                if (!CuisinierExiste(NumeroCuisinier, command))
                {
                    Console.WriteLine("Le cuisinier spécifié n'existe pas.");
                    System.Threading.Thread.Sleep(1000);
                    return;
                }

                Console.Write("Entrez l'ID du plat : ");
                if (!int.TryParse(Console.ReadLine(), out int IdPlat))
                {
                    Console.WriteLine("ID invalide. Veuillez entrer un nombre entier.");
                    System.Threading.Thread.Sleep(1000);
                    return;
                }

                if (PlatExiste(IdPlat, command))
                {
                    Console.WriteLine("Un plat avec cet ID existe déjà.");
                    System.Threading.Thread.Sleep(1000);
                    return;
                }

                Console.Write("Entrez le nom du plat : ");
                string NomPlat = Console.ReadLine();

                Console.Write("Entrez le prix du plat : ");
                if (!decimal.TryParse(Console.ReadLine(), out decimal Prix))
                {
                    Console.WriteLine("Le prix doit être un nombre valide.");
                    System.Threading.Thread.Sleep(1000);
                    return;
                }

                Console.Write("Entrez la quantité disponible : ");
                if (!int.TryParse(Console.ReadLine(), out int Quantite))
                {
                    Console.WriteLine("La quantité doit être un nombre entier.");
                    System.Threading.Thread.Sleep(1000);
                    return;
                }

                Console.Write("Entrez le type du plat : ");
                string TypePlat = Console.ReadLine();

                Console.Write("Entrez la date de fabrication (YYYY-MM-DD) : ");
                if (!DateTime.TryParse(Console.ReadLine(), out DateTime DateFabrication))
                {
                    Console.WriteLine("Format de date invalide.");
                    System.Threading.Thread.Sleep(1000);
                    return;
                }

                Console.Write("Entrez la date de péremption (YYYY-MM-DD) : ");
                if (!DateTime.TryParse(Console.ReadLine(), out DateTime DatePeremption))
                {
                    Console.WriteLine("Format de date invalide.");
                    System.Threading.Thread.Sleep(1000);
                    return;
                }

                Console.Write("Entrez le régime alimentaire associé : ");
                string RegimeAlim = Console.ReadLine();

                Console.Write("Entrez la nationalité du plat : ");
                string Nationalite = Console.ReadLine();

                command.CommandText = "INSERT INTO Plat (IdPlat, NomPlat, Prix, Quantite, TypePlat, DateFabrication, DatePeremption, RegimeAlim, Nationalite, NumeroCuisinier) " +
                                      "VALUES (@IdPlat, @NomPlat, @Prix, @Quantite, @TypePlat, @DateFabrication, @DatePeremption, @RegimeAlim, @Nationalite, @NumeroCuisinier)";


                command.Parameters.Clear();
                command.Parameters.AddWithValue("@IdPlat", IdPlat);
                command.Parameters.AddWithValue("@NomPlat", NomPlat);
                command.Parameters.AddWithValue("@Prix", Prix);
                command.Parameters.AddWithValue("@Quantite", Quantite);
                command.Parameters.AddWithValue("@TypePlat", TypePlat);
                command.Parameters.AddWithValue("@DateFabrication", DateFabrication);
                command.Parameters.AddWithValue("@DatePeremption", DatePeremption);
                command.Parameters.AddWithValue("@RegimeAlim", RegimeAlim);
                command.Parameters.AddWithValue("@Nationalite", Nationalite);
                command.Parameters.AddWithValue("@NumeroCuisinier", NumeroCuisinier);

                command.ExecuteNonQuery();
                Console.WriteLine("Plat créé avec succès !");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur lors de la création du plat : " + ex.Message);
            }
            System.Threading.Thread.Sleep(1000);
        }
        static void MajPlat(MySqlCommand command)
        {
            Console.Clear();
            Console.WriteLine("Mise à jour d'un plat");

            Console.Write("Entrez le numéro du plat à mettre à jour : ");
            if (!int.TryParse(Console.ReadLine(), out int IdPlat))
            {
                Console.WriteLine("Le numéro du plat doit être un nombre.");
                System.Threading.Thread.Sleep(1000);
                return;
            }

            Console.Write("Entrez le nouveau prix du plat : ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal Prix))
            {
                Console.WriteLine("Le prix doit être un nombre valide.");
                System.Threading.Thread.Sleep(1000);
                return;
            }

            Console.Write("Entrez la nouvelle quantité disponible : ");
            if (!int.TryParse(Console.ReadLine(), out int Quantite))
            {
                Console.WriteLine("La quantité doit être un nombre entier.");
                System.Threading.Thread.Sleep(1000);
                return;
            }

            command.CommandText = "UPDATE Plat SET Prix = @Prix, Quantite = @Quantite WHERE IdPlat = @NumeroPlat";
            command.Parameters.Clear();
            command.Parameters.AddWithValue("@Prix", Prix);
            command.Parameters.AddWithValue("@Quantite", Quantite);
            command.Parameters.AddWithValue("@NumeroPlat", IdPlat);

            int rowsAffected = command.ExecuteNonQuery();
            if (rowsAffected > 0)
            {
                Console.WriteLine("Plat mis à jour avec succès !");
            }
            else
            {
                Console.WriteLine("Aucun plat mis à jour.");
            }

            System.Threading.Thread.Sleep(1000);
        }
        static void SupprimerPlat(MySqlCommand command)
        {
            Console.Clear();
            Console.WriteLine("Suppression d'un plat");

            Console.Write("Entrez le numéro du plat à supprimer : ");
            if (!int.TryParse(Console.ReadLine(), out int IdPlat))
            {
                Console.WriteLine("Le numéro du plat doit être un nombre.");
                System.Threading.Thread.Sleep(1000);
                return;
            }

            command.CommandText = "DELETE FROM Plat WHERE IdPlat = @NumeroPlat";
            command.Parameters.Clear();
            command.Parameters.AddWithValue("@NumeroPlat", IdPlat);

            int rowsAffected = command.ExecuteNonQuery();
            if (rowsAffected > 0)
            {
                Console.WriteLine("Plat supprimé avec succès !");
            }
            else
            {
                Console.WriteLine("Aucun plat trouvé avec cet ID.");
            }

            System.Threading.Thread.Sleep(1000);
        }
        static void AfficherPlat(MySqlCommand command, MySqlDataReader reader)
        {
            Console.Clear();
            command.CommandText = "SELECT * FROM Plat;";
            reader = command.ExecuteReader();
            Console.WriteLine("Liste des plats disponibles :");
            while (reader.Read())
            {
                string currentRowAsString = "";
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    string valueAsString = reader.GetValue(i).ToString();
                    currentRowAsString += i == reader.FieldCount - 1 ? valueAsString : valueAsString + ", ";
                }
                Console.WriteLine(currentRowAsString);
            }

            reader.Close();
            Console.WriteLine("Appuyez sur une touche pour continuer");
            Console.ReadKey();
            Console.Clear();
        }




        static void Read(MySqlCommand command, MySqlDataReader reader, string commandText)
        {
            command.CommandText = commandText;
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                string currentRowAsString = "";
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    string valueAsString = reader.GetValue(i).ToString();
                    currentRowAsString += i == reader.FieldCount - 1 ? valueAsString : valueAsString + ", ";
                }
                Console.WriteLine(currentRowAsString);
            }
            reader.Close();
            Console.WriteLine("Appuyez sur une touche du clavier");
            Console.ReadKey();
            Console.Clear();
        }
        static bool ParticulierExiste(int NumeroParticulier, MySqlCommand command)
        {
            command.CommandText = "SELECT COUNT(*) FROM Particulier WHERE NumeroParticulier = @NumeroParticulier";
            command.Parameters.Clear();
            command.Parameters.AddWithValue("@NumeroParticulier", NumeroParticulier);
            int count = Convert.ToInt32(command.ExecuteScalar());
            return count > 0;
        }
        static bool EntrepriseExiste(int NumeroEntreprise, MySqlCommand command)
        {
            command.CommandText = "SELECT COUNT(*) FROM Entreprise WHERE NumeroEntreprise = @NumeroEntreprise";
            command.Parameters.Clear();
            command.Parameters.AddWithValue("@NumeroEntreprise", NumeroEntreprise);
            int count = Convert.ToInt32(command.ExecuteScalar());
            return count > 0;
        }
        static bool CuisinierExiste(int NumeroCuisinier, MySqlCommand command)
        {
            command.CommandText = "SELECT COUNT(*) FROM Cuisinier WHERE NumeroCuisinier = @NumeroCuisinier";
            command.Parameters.Clear();
            command.Parameters.AddWithValue("@NumeroCuisinier", NumeroCuisinier);
            int count = Convert.ToInt32(command.ExecuteScalar());
            return count > 0;
        }
        static bool CommandeExiste(int idCommande, MySqlCommand command)
        {
            command.CommandText = "SELECT COUNT(*) FROM Commande WHERE IDCommande = @IDCommande";
            command.Parameters.Clear();
            command.Parameters.AddWithValue("@IDCommande", idCommande);
            int count = Convert.ToInt32(command.ExecuteScalar());
            return count > 0;
        }
        static bool PlatExiste(int IdPlat, MySqlCommand command)
        {
            command.CommandText = "SELECT COUNT(*) FROM Plat WHERE IdPlat = @IdPlat";
            command.Parameters.Clear();
            command.Parameters.AddWithValue("@IdPlat", IdPlat);
            int count = Convert.ToInt32(command.ExecuteScalar());
            return count > 0;
        }
    }
}
