using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace PSI_ClovisNOE_JaimeSOUSA_ThomasMAYE
{
    static class Program
    {
        
        static void Main()
        {
            XMSExport.XMLExport();
            XML.Import();
            ExportJson.JsonExport();
            JSON.Import();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            // Initialisation de la liste des stations et du graphe
            string filePath = "MetroParis.csv";
            List<Station> stations = LireCSV(filePath);
            Dictionary<string, List<Station>> Lignes = new Dictionary<string, List<Station>>(); 
            Dictionary<string, int> doublons = new Dictionary<string, int>();
            Dictionary<Noeud<Station>, Station> nodeToStation = new Dictionary<Noeud<Station>, Station>();
            Dictionary<Noeud<Station>, (double, double)> nodePositions = new Dictionary<Noeud<Station>, (double, double)>();

            Graphe<Station> graph = new Graphe<Station>();

            foreach (var station in stations)
            {
                Noeud<Station> noeud = new Noeud<Station>(station.IdStation, station);
                graph.AjouterNoeud(noeud);
                
                //gestion d'une station presente dans 2 lignes
                if (doublons.ContainsKey(noeud.Type.LibelleStation) && doublons[noeud.Type.LibelleStation] != noeud.Id)
                {
                    graph.AjouterLien(new Lien<Station>(noeud, graph.Noeuds[doublons[noeud.Type.LibelleStation]-1], 2));
                    graph.AjouterLien(new Lien<Station>(graph.Noeuds[doublons[noeud.Type.LibelleStation]-1], noeud, 2));
                }
                doublons[station.LibelleStation] = noeud.Id;

                nodeToStation[noeud] = station;
                nodePositions[noeud] = (station.Longitude, station.Latitude);
                if (!Lignes.ContainsKey(station.LibelleLine))
                {
                    Lignes[station.LibelleLine] = new List<Station>();
                }
                if (!Lignes[station.LibelleLine].Contains(station))
                {
                    Lignes[station.LibelleLine].Add(station);
                }
            }
            var groupedByLine = graph.Noeuds.GroupBy(r => r.Type.LibelleLine);
            foreach (var group in groupedByLine)
            {
                var sorted = group.OrderBy(r => r.Id).ToList();
                for (int i = 0; i < sorted.Count - 1; i++)
                {
                    Noeud<Station> nodeStart = sorted[i];
                    Noeud<Station> nodeEnd = sorted[i + 1];

                    double distance = Graphe<Station>.HaversineDistance(sorted[i].Type.Latitude, sorted[i].Type.Longitude, sorted[i + 1].Type.Latitude, sorted[i + 1].Type.Longitude);
                    int travelTime = (int)Math.Round(distance*2);
                    graph.AjouterLien(new Lien<Station>(nodeStart, nodeEnd, travelTime, group.Key));
                    graph.AjouterLien(new Lien<Station>(nodeEnd, nodeStart, travelTime, group.Key));
                }
            }
            // fin de l'initialisation du graphe

            //menu
            int choix = 0;
            bool fin = false;
            while (!fin)
            {
                graph.Generer_Matrice();
                Console.WriteLine("                       __ \r\n                      (_ )\r\n██╗     ██╗██╗   ██╗   |/   ██╗███╗   ██╗        ██████╗  █████╗ ██████╗ ██╗███████╗\r\n██║     ██║██║   ██║        ██║████╗  ██║        ██╔══██╗██╔══██╗██╔══██╗██║██╔════╝\r\n██║     ██║██║   ██║        ██║██╔██╗ ██║        ██████╔╝███████║██████╔╝██║███████╗\r\n██║     ██║╚██╗ ██╔╝        ██║██║╚██╗██║        ██╔═══╝ ██╔══██║██╔══██╗██║╚════██║\r\n███████╗██║ ╚████╔╝         ██║██║ ╚████║        ██║     ██║  ██║██║  ██║██║███████║\r\n╚══════╝╚═╝  ╚═══╝          ╚═╝╚═╝  ╚═══╝        ╚═╝     ╚═╝  ╚═╝╚═╝  ╚═╝╚═╝╚══════╝\r\n                                                                                    \r\n");
                Console.WriteLine("Bienvenue dans la démonstration de l'application Liv'In Paris !\r\n");
                Console.WriteLine("Veuillez choisir une option :");
                Console.WriteLine("1. Visualiser le plan metro");
                Console.WriteLine("2. Afficher la liste d'adjacence d'un sommet");
                Console.WriteLine("3. Afficher la matrice d'adjacence");
                Console.WriteLine("4. Utiliser l'algorithme de Dijkstra");
                Console.WriteLine("5. Utiliser l'algorithme de Bellman-Ford");
                Console.WriteLine("6. Utiliser l'algorithme de Floyd-Warshall");
                Console.WriteLine("7. Se connecter à la base de donnée // rendu 3");
                Console.WriteLine("8. Afficher le graphe cuisinier client // rendu 3");
                Console.WriteLine("9. Quitter l'application");
                Console.Write("Entrez votre choix : ");
                string input = Console.ReadLine();
                if (!int.TryParse(input, out choix) || choix < 1 || choix > 9)
                {
                    Console.WriteLine("Choix invalide. Veuillez réessayer.");
                }
                else
                {
                    switch (choix)
                    {
                        case 1:
                            // Visualiser le plan metro
                            Application.Run(new Visualisation<Station>(graph, nodeToStation, nodePositions));
                            Console.WriteLine("Fermez le plan pour continuer.\r\n");
                            break;
                        case 2:
                            // Afficher la liste d'adjacence d'un sommet
                            bool trouve = false;
                            Console.Write("Entrez le nom de la station (sans faute) : ");
                            string nomStation = Console.ReadLine();
                            foreach (var station in graph.Noeuds)
                            {
                                if (station.Type.LibelleStation.Equals(nomStation))
                                {
                                    trouve = true;
                                    Console.WriteLine($"\nListe d'adjacence pour la station {nomStation} sur la ligne {station.Type.LibelleLine}:");
                                    foreach (var adjacent in graph.Liste_Adjacence[station])
                                    {
                                        Lien<Station> lienStation = graph.Rechercher_Lien(station, adjacent);
                                        Console.WriteLine($"- {lienStation.NoeudArrive.Type.LibelleStation} (Ligne: {lienStation.Line}, Temps: {lienStation.Poids} minutes)");
                                    }
                                }
                            }
                            if (!trouve)
                            {
                                Console.WriteLine("Station non trouvée dans le graphe.");
                            }
                            Console.WriteLine("Appuyez sur une touche pour continuer...");
                            Console.ReadKey();
                            break;
                        case 3:
                            // Afficher la matrice d'adjacence
                            Console.WriteLine("Matrice d'adjacence :");
                            for (int i = 0; i < graph.Matrice_Adjacence.GetLength(0); i++)
                            {
                                for (int j = 0; j < graph.Matrice_Adjacence.GetLength(1); j++)
                                {
                                    if (graph.Matrice_Adjacence[i, j] != int.MaxValue)
                                    {
                                        Console.Write(graph.Matrice_Adjacence[i, j] + " ");
                                    }
                                    else
                                    {
                                        Console.Write("INF ");
                                    }
                                }
                                Console.WriteLine();
                            }
                            Console.WriteLine("Appuyez sur une touche pour continuer...");
                            Console.ReadKey();
                            break;
                        case 4:
                            // Utiliser l'algorithme de Dijkstra
                            bool trouveS = false;
                            Console.Write("Entrez le nom de la station de départ (sans faute) : ");
                            string nomStationD = Console.ReadLine();
                            foreach (var station in graph.Noeuds)
                            {
                                if (!trouveS && station.Type.LibelleStation.Equals(nomStationD))
                                {
                                    trouveS = true;
                                    int[,] distances = graph.Dijkstra(station);
                                    Console.WriteLine($"\nDistances depuis la station {nomStationD} :");
                                    for (int i = 0; i < distances.GetLength(0); i++)
                                    {
                                        if (distances[graph.Noeuds[i].Id - 1, 0] != -1)
                                        {
                                            Console.WriteLine($"- Station {graph.Noeuds[i].Type.LibelleStation} : {distances[graph.Noeuds[i].Id - 1, 0]} minutes");
                                        }
                                        else
                                        {
                                            Console.WriteLine($"- Station {graph.Noeuds[i].Type.LibelleStation} : Inaccessible");
                                        }
                                    }

                                    bool trouveZ = false;
                                    Console.Write("Entrez le nom de la station d'arrivée (sans faute) : ");
                                    string nomStationA = Console.ReadLine();
                                    Graphe<Station> graphD = new Graphe<Station>();
                                    List<Station> Parcours = new List<Station>();

                                    foreach (var noeud in graph.Noeuds)
                                    {
                                        if (!trouveZ && noeud.Type.LibelleStation.Equals(nomStationA))
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

                                    Application.Run(new Visualisation<Station>(graphD, nodeToStationD, nodePositionsD,true));
                                    Console.WriteLine("Fermez le plan pour continuer.\r\n");
                                    if (!trouveZ)
                                    {
                                        Console.WriteLine("Station d'arrivée non trouvée dans le graphe.");
                                    }
                                }
                            }
                            if (!trouveS)
                            {
                                Console.WriteLine("Station de départ non trouvée dans le graphe.");
                            }
                            
                            Console.WriteLine("Appuyez sur une touche pour continuer...");
                            Console.ReadKey();
                            break;
                        case 5:
                            // Utiliser l'algorithme de Bellman-Ford
                            bool trouveB = false;
                            Console.Write("Entrez le nom de la station de départ (sans faute) : ");
                            string nomStationF = Console.ReadLine();
                            foreach (var station in graph.Noeuds)
                            {
                                if (!trouveB && station.Type.LibelleStation.Equals(nomStationF))
                                {
                                    trouveS = true;
                                    int[,] distances = graph.Bellman_Ford(station);
                                    Console.WriteLine($"\nDistances depuis la station {nomStationF} :");
                                    for (int i = 0; i < distances.GetLength(0); i++)
                                    {
                                        if (distances[graph.Noeuds[i].Id - 1, 0] != -1)
                                        {
                                            Console.WriteLine($"- Station {graph.Noeuds[i].Type.LibelleStation} : {distances[graph.Noeuds[i].Id - 1, 0]} minutes");
                                        }
                                        else
                                        {
                                            Console.WriteLine($"- Station {graph.Noeuds[i].Type.LibelleStation} : Inaccessible");
                                        }
                                    }

                                    bool trouveZ = false;
                                    Console.Write("Entrez le nom de la station d'arrivée (sans faute) : ");
                                    string nomStationA = Console.ReadLine();
                                    Graphe<Station> graphD = new Graphe<Station>();
                                    List<Station> Parcours = new List<Station>();

                                    foreach (var noeud in graph.Noeuds)
                                    {
                                        if (!trouveZ && noeud.Type.LibelleStation.Equals(nomStationA))
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

                                    Application.Run(new Visualisation<Station>(graphD, nodeToStationD, nodePositionsD, true));
                                    Console.WriteLine("Fermez le plan pour continuer.\r\n");
                                    if (!trouveZ)
                                    {
                                        Console.WriteLine("Station d'arrivée non trouvée dans le graphe.");
                                    }
                                }
                            }
                            if (!trouveB)
                            {
                                Console.WriteLine("Station de départ non trouvée dans le graphe.");
                            }

                            Console.WriteLine("Appuyez sur une touche pour continuer...");
                            Console.ReadKey();
                            break;
                        case 6:
                            // Utiliser l'algorithme de Floyd-Warshall
                            Console.WriteLine("Matrice de distances (Floyd-Warshall) :");
                            int[,] distancesFW = graph.Floyd_Warshall();
                            for (int i = 0; i < graph.Noeuds.Count; i++)
                            {
                                for (int j = 0; j < graph.Noeuds.Count; j++)
                                {
                                    if (distancesFW[i, j] != int.MaxValue)
                                    {
                                        Console.Write(distancesFW[i, j] + " ");
                                    }
                                    else
                                    {
                                        Console.Write("INF ");
                                    }
                                }
                                Console.WriteLine();
                            }
                            Console.WriteLine("Appuyez sur une touche pour continuer...");
                            Console.ReadKey();
                            break;
                        case 7:
                            // Se connecter à la base de donnée
                            Console.WriteLine("Connexion à la base de données...");
                            BDD.Appelle_BDD(graph);
                            break;
                        case 8:
                            Graphe<string> grapheCommande = new Graphe<string>();
                            Dictionary<int,string> cuisiniers = BDD.Cuisiniers();
                            Dictionary<int, string> clients = BDD.Clients();
                            List<(int, int)> commandes = BDD.Commandes();
                            
                            foreach (int id in cuisiniers.Keys)
                            {
                                Noeud<string> noeud = new Noeud<string>(id, cuisiniers[id]);
                                grapheCommande.AjouterNoeud(noeud);
                            }
                            foreach (int id in clients.Keys)
                            {
                                Noeud<string> noeud = new Noeud<string>(id + 4000, clients[id]); //on additionne 4000 pour ne pas avoir de doublon avec les cuisiniers
                                grapheCommande.AjouterNoeud(noeud);
                            }
                            foreach (var commande in commandes)
                            {
                                int idCuisinier = commande.Item1;
                                int idClient = commande.Item2;
                                string nomCuisinier = cuisiniers[idCuisinier];
                                string nomClient = clients[idClient];
                                Noeud<string> noeudCuisinier = grapheCommande.Noeuds.FirstOrDefault(n => n.Id == idCuisinier);
                                Noeud<string> noeudClient = grapheCommande.Noeuds.FirstOrDefault(n => n.Id == idClient + 4000);
                                if (noeudCuisinier != null && noeudClient != null)
                                {
                                    grapheCommande.AjouterLien(new Lien<string>(noeudCuisinier, noeudClient, 1));
                                }
                            }
                            Console.WriteLine(grapheCommande.Liens.Count + " liens ajoutés au graphe cuisinier-client.\r\n");
                            Dictionary<Noeud<string>,int> couleurs = grapheCommande.Welsh_Powel();
                            CuisinierClient.VisualiserGraphe(grapheCommande, cuisiniers, clients, couleurs);

                            break;
                        case 9:
                            fin = true;
                            break;
                    }
                }
                Console.Clear();
            }
            while (choix < 1 || choix > 9)
            {
                Console.Write("Entrez votre choix : ");
                string input = Console.ReadLine();
                if (!int.TryParse(input, out choix) || choix < 1 || choix > 9)
                {
                    Console.WriteLine("Choix invalide. Veuillez réessayer.");
                }
            }
        }

        static List<Station> LireCSV(string filePath)
        {
            List<Station> stations = new List<Station>();
            using (var reader = new StreamReader(filePath))
            {
                string header = reader.ReadLine(); 
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    if (string.IsNullOrWhiteSpace(line))
                        continue;
                    string[] parts = line.Split(',');

                    int id = int.Parse(parts[0]);
                    string libelleLine = parts[1];
                    string libelleStation = parts[2];
                    double longitude = double.Parse(parts[3], CultureInfo.InvariantCulture);
                    double latitude = double.Parse(parts[4], CultureInfo.InvariantCulture);

                    stations.Add(new Station(id, libelleLine, libelleStation, longitude, latitude));
                }
            }
            return stations;
        }
    }
}
