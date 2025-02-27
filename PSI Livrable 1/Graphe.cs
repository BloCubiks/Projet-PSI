using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSI_Livrable_1
{
    internal class Graphe
    {
        private List<Noeud> noeuds;
        private List<Lien> liens;
        private Dictionary<Noeud, List<Noeud>> liste_Adjacence;
        private int[,] matrice_Adjacence; 

        public Graphe()
        {
            noeuds = new List<Noeud>();
            liens = new List<Lien>();
            liste_Adjacence = new Dictionary<Noeud, List<Noeud>>();
        }
        /// <summary>
        /// on verifie si un noeud existe dans le graphe
        /// </summary>
        /// <param name="noeud"></param>
        /// <returns></returns>
        public bool Noeud_Existe(Noeud noeud)
        {
            return noeuds.Contains(noeud);
        }
        /// <summary>
        /// Ajoute un noeud au graphe seulement si il n'existe pas
        /// </summary>
        /// <param name="noeud">noeud a ajouter dans le graphe</param>
        public void AjouterNoeud(Noeud noeud)
        {
            if (noeud != null && !noeuds.Contains(noeud))
            {
                noeuds.Add(noeud);
                liste_Adjacence.Add(noeud, new List<Noeud>());
            }
        }
        /// <summary>
        /// Ajoute un lien au graphe seulement si les noeuds du lien existent et sont dans le grapge
        /// </summary>
        /// <param name="lien">lien a ajouter au graphe</param>
        public void AjouterLien(Lien lien)
        {
            if (lien.Noeud1 != null && lien.Noeud2 != null && noeuds.Contains(lien.Noeud1) && noeuds.Contains(lien.Noeud2))
            {
                liens.Add(lien);
                if (!liste_Adjacence[lien.Noeud1].Contains(lien.Noeud2))
                {
                    liste_Adjacence[lien.Noeud1].Add(lien.Noeud2);
                }
                if (!liste_Adjacence[lien.Noeud2].Contains(lien.Noeud1))
                {
                    liste_Adjacence[lien.Noeud2].Add(lien.Noeud1);
                }
            }
        }
        /// <summary>
        /// Fonction qui genere la matrice d'ajacence a partir des liens du graphe et qui est appelée a la fin de la creation du graphe
        /// </summary>
        public void Generer_Matrice()
        {
            int n = Noeuds.Count;
            matrice_Adjacence = new int[n, n];
            for (int i =0; i < liens.Count; i++)
            {
                matrice_Adjacence[liens[i].Noeud1.Id-1, liens[i].Noeud2.Id-1] = 1;//on met-1 car notre graphe commence a 1
                matrice_Adjacence[liens[i].Noeud2.Id-1, liens[i].Noeud1.Id-1] = 1;
            }
        }
        public void AfficherListeAdjacence()
        {
            string ligne = "";
            for (int i = 0; i < noeuds.Count;i++)
            {
                ligne += noeuds[i].Id + " : ";
                for (int j = 0;j < liste_Adjacence[noeuds[i]].Count; j++)
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
        public List<Noeud> Parcours_Largeur(Noeud Noeud_Depart)
        {
            List<Noeud> Parcours = new List<Noeud>();
            Queue<Noeud> Noeuds_a_tester = new Queue<Noeud>(); //on utilise une file pour le parcours en largeur car on peut parcourir etage par etage
            Noeuds_a_tester.Enqueue(Noeuds[Noeud_Depart.Id]);
            while (Noeuds_a_tester.Count != 0)
            {
                Noeud n = Noeuds_a_tester.Dequeue();
                if (!Parcours.Contains(n))
                {
                    Parcours.Add(n);
                    foreach (Noeud voisin in liste_Adjacence[n])
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
        public List<Noeud> Parcours_Profondeur(Noeud Noeud_Depart)
        {
            List<Noeud> Parcours = new List<Noeud>();
            Stack<Noeud> Noeuds_a_tester = new Stack<Noeud>(); //on utilise une pile car on peut aller en profondeur puis revenir
            Noeuds_a_tester.Push(Noeuds[Noeud_Depart.Id]);
            while (Noeuds_a_tester.Count != 0)
            {
                Noeud n = Noeuds_a_tester.Pop();
                if (!Parcours.Contains(n))
                {
                    Parcours.Add(n);
                    foreach (Noeud voisin in liste_Adjacence[n])
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
        public List<Noeud> Noeuds
        {
            get { return noeuds; }
        }

        public List<Lien> Liens
        {
            get { return liens; }
        }
        public Dictionary<Noeud, List<Noeud>> Liste_Adjacence
        {
            get { return liste_Adjacence; }
        }
        public int[,] Matrice_Adjacence
        {
            get { return matrice_Adjacence; }
        }
    }
}
