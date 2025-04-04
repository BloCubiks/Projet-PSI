using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace PSI_Livrable_1_ClovisNOE_JaimeSOUSA_ThomasMAYE
{
    public class Graphe<T>
    {
        private List<Noeud<T>> noeuds;
        private List<Lien<T>> liens;
        private Dictionary<Noeud<T>, List<Noeud<T>>> liste_Adjacence;
        private int[,] matrice_Adjacence;

        public Graphe()
        {
            noeuds = new List<Noeud<T>>();
            liens = new List<Lien<T>>();
            liste_Adjacence = new Dictionary<Noeud<T>, List<Noeud<T>>>();
        }

        /// <summary>
        /// Fonction de calcul de distance de Haversine entre 2 points
        /// </summary>
        /// <param name="lat1">latitude du premier point</param>
        /// <param name="lon1">longitude du premier point</param>
        /// <param name="lat2">latitude du deuxieme point</param>
        /// <param name="lon2">longitude du deuxieme point</param>
        /// <returns> la distance entre les 2 points</returns>
        public static double HaversineDistance(double lat1, double lon1, double lat2, double lon2)
        {
            //formule de calcul de distance de harversine
            double R = 6371; // Rayon de la Terre en kilomètres
            double dLat = (lat2 - lat1)*Math.PI/180;
            double dLon = (lon2 - lon1) * Math.PI / 180;
            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                       Math.Cos((lat1) * Math.PI / 180) * Math.Cos((lat2) * Math.PI / 180) *
                       Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            double c = 2 * Math.Asin(Math.Sqrt(a));
            return R * c; // Distance en kilomètres
        }

        /// <summary>
        /// on verifie si un noeud existe dans le graphe
        /// </summary>
        /// <param name="noeud"></param>
        /// <returns></returns>
        public bool Noeud_Existe(Noeud<T> noeud)
        {
            return noeuds.Contains(noeud);
        }

        /// <summary>
        /// Ajoute un noeud au graphe seulement si il n'existe pas
        /// </summary>
        /// <param name="noeud">noeud a ajouter dans le graphe</param>
        public void AjouterNoeud(Noeud<T> noeud)
        {
            if (noeud != null && !noeuds.Contains(noeud))
            {
                noeuds.Add(noeud);
                liste_Adjacence[noeud] = new List<Noeud<T>>();
            }
        }

        /// <summary>
        /// Ajoute un lien au graphe seulement si les noeuds du lien existent et sont dans le grapge
        /// </summary>
        /// <param name="lien">lien a ajouter au graphe</param>
        public void AjouterLien(Lien<T> lien)
        {
            if (lien.NoeudDepart != null && lien.NoeudArrive != null &&
                noeuds.Contains(lien.NoeudDepart) && noeuds.Contains(lien.NoeudArrive))
            {
                liens.Add(lien);
                if (!liste_Adjacence[lien.NoeudDepart].Contains(lien.NoeudArrive))
                {
                    liste_Adjacence[lien.NoeudDepart].Add(lien.NoeudArrive);
                }
                if (!liste_Adjacence[lien.NoeudArrive].Contains(lien.NoeudDepart))
                {
                    liste_Adjacence[lien.NoeudArrive].Add(lien.NoeudDepart);
                }
            }
        }
        /// <summary>
        /// Verifie si un lien existe dans le graphe
        /// </summary>
        /// <param name="Noeud_Depart">noeud de depart</param>
        /// <param name="Noeud_arrive">noeud d'arrive</param>
        /// <returns></returns>
        public Lien<T> Rechercher_Lien(Noeud<T> Noeud_Depart, Noeud<T> Noeud_Arrive)
        {
            foreach (Lien<T> lien in liens)
            {
                if (lien.NoeudDepart == Noeud_Depart && lien.NoeudArrive == Noeud_Arrive)
                {
                    return lien;
                }
            }
            return null;
        }
        /// <summary>
        /// Fonction qui genere la matrice d'ajacence a partir des liens du graphe et qui est appelée a la fin de la creation du graphe
        /// </summary>
        public void Generer_Matrice()
        {
            long n = Noeuds.Count;
            matrice_Adjacence = new int[n, n];
            for (int i = 0; i < n; i++)
            {
                for (int j =0;j<n; j++)
                {
                    matrice_Adjacence[i, j] = int.MaxValue; // Initialisation à l'infini
                }
            }
            for (int i = 0; i < liens.Count; i++)
            {
                matrice_Adjacence[liens[i].NoeudDepart.Id-1, liens[i].NoeudArrive.Id-1] = liens[i].Poids;//on met-1 car notre graphe commence a 1
                
                matrice_Adjacence[liens[i].NoeudArrive.Id-1, liens[i].NoeudDepart.Id-1] = liens[i].Poids;
            }
        }
        public void AfficherListeAdjacence()
        {
            string ligne = "";
            for (int i = 0; i < noeuds.Count; i++)
            {
                ligne += noeuds[i].Id + " : ";
                for (int j = 0; j < liste_Adjacence[noeuds[i]].Count; j++)
                {
                    ligne += liste_Adjacence[noeuds[i]][j].Id + " ";
                }
                Console.WriteLine(ligne);
                ligne = "";
            }
        }
        /// <summary>
        /// Parcours en largeur du graphe a partir d'un noeud de depart
        /// </summary>
        /// <param name="Noeud_Depart">noeud de depart</param>
        /// <returns>list de noeuds qui est un parcours en largeur du graphe</returns>
        public List<Noeud<T>> Parcours_Largeur(Noeud<T> Noeud_Depart)
        {
            List<Noeud<T>> Parcours = new List<Noeud<T>>();
            Queue<Noeud<T>> Noeuds_a_tester = new Queue<Noeud<T>>(); //on utilise une file pour le parcours en largeur car on peut parcourir etage par etage
            Noeuds_a_tester.Enqueue(Noeuds[Noeud_Depart.Id]);
            while (Noeuds_a_tester.Count != 0)
            {
                Noeud<T> n = Noeuds_a_tester.Dequeue();
                if (!Parcours.Contains(n))
                {
                    Parcours.Add(n);
                    foreach (Noeud<T> voisin in liste_Adjacence[n])
                    {
                        if (!Parcours.Contains(voisin))
                        {
                            Noeuds_a_tester.Enqueue(voisin);
                        }
                    }
                }
            }
            return Parcours;
        }
        /// <summary>
        /// Parcours en profondeur du graphe a partir d'un noeud de depart
        /// </summary>
        /// <param name="Noeud_Depart">noeud de depart</param>
        /// <returns>list de noeuds qui est un parcours en profondeur du graphe</returns>
        public List<Noeud<T>> Parcours_Profondeur(Noeud<T> Noeud_Depart)
        {
            List<Noeud<T>> Parcours = new List<Noeud<T>>();
            Stack<Noeud<T>> Noeuds_a_tester = new Stack<Noeud<T>>(); //on utilise une pile car on peut aller en profondeur puis revenir
            Noeuds_a_tester.Push(Noeuds[Noeud_Depart.Id]);
            while (Noeuds_a_tester.Count != 0)
            {
                Noeud<T> n = Noeuds_a_tester.Pop();
                if (!Parcours.Contains(n))
                {
                    Parcours.Add(n);
                    foreach (Noeud<T> voisin in liste_Adjacence[n])
                    {
                        if (!Parcours.Contains(voisin))
                        {
                            Noeuds_a_tester.Push(voisin);
                        }
                    }
                }
            }
            return Parcours;
        }
        /// <summary>
        /// On verifie si le graphe est connexe lorsqu'un parcours est de la meme taille que le nombre de noeuds
        /// </summary>
        /// <returns>vrai il est connexe ou faux il ne l'est pas</returns>
        public bool Est_Connexe()
        {
            List<Noeud<T>> Parcours = Parcours_Profondeur(noeuds[0]);
            return Parcours.Count == Noeuds.Count;
        }
        /// <summary>
        /// Programme qui permet de detecter un cycle dans un graphe
        /// </summary>
        /// <returns>retourne un cycle</returns>
        public List<Noeud<T>> Cycle()
        {
            List<Noeud<T>> cycle = null;
            List<Noeud<T>> visites = new List<Noeud<T>>(); // Liste pour suivre les nœuds visités
            Dictionary<Noeud<T>, Noeud<T>> parents = new Dictionary<Noeud<T>, Noeud<T>>(); // Pour retracer le cycle

            for (int i = 0; i < Noeuds.Count; i++)
            {
                if (!visites.Contains(Noeuds[i]))
                {
                    Stack<Noeud<T>> Noeuds_a_tester = new Stack<Noeud<T>>();
                    Noeuds_a_tester.Push(Noeuds[i]);
                    while (Noeuds_a_tester.Count > 0)
                    {
                        Noeud<T> actuel = Noeuds_a_tester.Pop();
                        if (!visites.Contains(actuel))
                        {
                            visites.Add(actuel);

                            foreach (Noeud<T> voisin in liste_Adjacence[actuel])
                            {
                                if (!visites.Contains(voisin))
                                {
                                    Noeuds_a_tester.Push(voisin);
                                    parents[voisin] = actuel; // Enregistrer le parent pour retracer le cycle
                                }
                                else if (parents.ContainsKey(actuel) && voisin != parents[actuel])// Cycle détecté
                                {
                                    cycle = new List<Noeud<T>>();
                                    Noeud<T> current = actuel;
                                    while (current != voisin)
                                    {
                                        cycle.Add(current);
                                        current = parents[current];
                                    }
                                    cycle.Add(voisin);
                                    cycle.Add(actuel); // Ajouter le point de départ pour fermer le cycle
                                    cycle.Reverse(); // Reconstituer l'ordre correct
                                    return cycle;
                                }
                            }
                        }
                    }
                }
            }
            return cycle;
        }
        /// <summary>
        /// Algorithme de Recherche de chemin de Dijkstra qui se base sur un parcours en largeur
        /// </summary>
        /// <param name="Noeud_Depart">Noeud a partir du quel on veut chercher les chemins</param>
        /// <returns>renvoie une matrice qui donne le poids et le predecesseur du noeud en index</returns>
        public int[,] Dijkstra(Noeud<T> Noeud_Depart)
        {
            int[,] Chemin = new int[Noeuds.Count, 2];
            Dictionary<Noeud<T>, int> distances = new Dictionary<Noeud<T>, int>();
            Dictionary<Noeud<T>, Noeud<T>> predecesseur = new Dictionary<Noeud<T>, Noeud<T>>();
            foreach (Noeud<T> n in noeuds)
            {
                distances[n] = int.MaxValue;
            }
            distances[Noeud_Depart] = 0;
            predecesseur[Noeud_Depart] = Noeud_Depart;

            Queue<Noeud<T>> Noeuds_a_tester = new Queue<Noeud<T>>();
            Noeuds_a_tester.Enqueue(Noeud_Depart);
            while (Noeuds_a_tester.Count != 0)
            {
                Noeud<T> n = Noeuds_a_tester.Dequeue();
                foreach (Noeud<T> voisin in liste_Adjacence[n])
                {
                    Lien<T> liaison = Rechercher_Lien(n, voisin);
                    int distance = distances[n] + liaison.Poids;
                    if (distance < distances[voisin])
                    {
                        distances[voisin] = distance;
                        predecesseur[voisin] = n;
                        Noeuds_a_tester.Enqueue(voisin);
                    }
                }
            }
            // on remplit la matrice de chemin
            for (int i = 0; i < noeuds.Count; i++)
            {
                if (distances[noeuds[i]] == int.MaxValue) // Si le noeud n'est pas accessible, on met -1
                {
                    Chemin[i, 0] = -1;
                    Chemin[i, 1] = -1;
                }

                else 
                {
                    Chemin[i, 0] = distances[noeuds[i]];
                    Chemin[i, 1] = predecesseur[noeuds[i]].Id-1;
                }
              
            }
            //for (int i = 0; i < noeuds.Count; i++)
            //{
            //    Console.Write(i + " " + Chemin[i, 0] + " " + Chemin[i, 1] + "\n"); // Affichage de la matrice de chemin
            //}
            return Chemin;
        }
        /// <summary>
        /// Algorithme de Recherche de chemin de Bellman-Ford
        /// </summary>
        /// <param name="Noeud_Depart">Noeud a partir du quel on veut chercher les chemins</param>
        /// <returns>renvoie une matrice qui donne le poids et le predecesseur du noeud en index</returns>
        public int[,] Bellman_Ford(Noeud<T> Noeud_Depart)
        {
            int[,] Chemin = new int[Noeuds.Count, 2];
            Dictionary<Noeud<T>, int> distances = new Dictionary<Noeud<T>, int>();
            Dictionary<Noeud<T>, Noeud<T>> predecesseur = new Dictionary<Noeud<T>, Noeud<T>>();
            foreach (Noeud<T> n in noeuds)
            {
                distances[n] = int.MaxValue;
            }
            distances[Noeud_Depart] = 0;

            for (int i = 0; i < noeuds.Count - 1; i++)
            {
                foreach (Lien<T> liaison in liens)
                {
                    if (distances[noeuds[liaison.NoeudDepart.Id - 1]] != int.MaxValue && distances[noeuds[liaison.NoeudDepart.Id-1]] + liaison.Poids < distances[noeuds[liaison.NoeudArrive.Id - 1]])
                    {
                        distances[noeuds[liaison.NoeudArrive.Id - 1]] = distances[noeuds[liaison.NoeudDepart.Id - 1]] + liaison.Poids;
                        predecesseur[noeuds[liaison.NoeudArrive.Id - 1]] = noeuds[liaison.NoeudDepart.Id - 1];
                    }
                }
            }
            // on remplit la matrice de chemin
            for (int i = 0; i < noeuds.Count; i++)
            {
                if (distances[noeuds[i]] == int.MaxValue) // Si le noeud n'est pas accessible, on met -1
                {
                    Chemin[i, 0] = -1;
                    Chemin[i, 1] = -1;
                }

                else
                {
                    Chemin[i, 0] = distances[noeuds[i]];
                    Chemin[i, 1] = predecesseur.ContainsKey(noeuds[i]) ? predecesseur[noeuds[i]].Id - 1 : -1;
                }
            }
            return Chemin;
        }
        /// <summary>
        /// Algorithme de Recherche de chemin de Floyd-Warshall
        /// </summary>
        /// <returns>matrice des chemins les plus courts</returns>
        public int[,] Floyd_Warshall()
        {
            Generer_Matrice();
            int[,] matriceFloyd = new int[noeuds.Count, noeuds.Count];
            for (int p = 0; p < noeuds.Count; p++)
            {
                for (int f = 0; f < noeuds.Count; f++)
                {
                    matriceFloyd[p, f] = matrice_Adjacence[p, f];
                }
            }
            for (int k = 0; k < noeuds.Count; k++)
            {
                for (int i = 0; i < noeuds.Count; i++)
                {
                    for (int j = 0; j < noeuds.Count; j++)
                    {
                        if (matriceFloyd[i, k] != int.MaxValue && matriceFloyd[k, j] != int.MaxValue && matriceFloyd[i, k] + matriceFloyd[k, j] < matriceFloyd[i, j])
                        {
                            matriceFloyd[i, j] = matriceFloyd[i, k] + matriceFloyd[k, j];
                        }
                        
                    }
                }
            }
            return matriceFloyd;
        }
        public List<Noeud<T>> Noeuds
        {
            get { return noeuds; }
        }

        public List<Lien<T>> Liens
        {
            get { return liens; }
        }

        public Dictionary<Noeud<T>, List<Noeud<T>>> Liste_Adjacence
        {
            get { return liste_Adjacence; }
        }
        public int[,] Matrice_Adjacence
        {
            get { return matrice_Adjacence; }
        }
    }
}