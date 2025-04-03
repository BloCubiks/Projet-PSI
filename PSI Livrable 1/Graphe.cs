using System;
using System.Collections.Generic;

namespace PSI_Livrable_1
{
    public class Graphe<T>
    {
        private List<Noeud<T>> noeuds;
        private List<Lien<T>> liens;
        private Dictionary<Noeud<T>, List<Noeud<T>>> liste_Adjacence;

        public Graphe()
        {
            noeuds = new List<Noeud<T>>();
            liens = new List<Lien<T>>();
            liste_Adjacence = new Dictionary<Noeud<T>, List<Noeud<T>>>();
        }

        public bool Noeud_Existe(Noeud<T> noeud)
        {
            return noeuds.Contains(noeud);
        }

        public void AjouterNoeud(Noeud<T> noeud)
        {
            if (noeud != null && !noeuds.Contains(noeud))
            {
                noeuds.Add(noeud);
                liste_Adjacence[noeud] = new List<Noeud<T>>();
            }
        }

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
            }
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
    }
}
