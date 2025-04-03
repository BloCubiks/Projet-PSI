using System;

namespace PSI_Livrable_1
{
    public class Lien<T>
    {
        public Noeud<T> NoeudDepart { get; }
        public Noeud<T> NoeudArrive { get; }
        public int Poids { get; }
        /// <summary>
        /// Ligne associée à cette liaison (null pour un transfert entre lignes)
        /// </summary>
        public string Line { get; }

        public Lien(Noeud<T> depart, Noeud<T> arrive, int poids, string line = null)
        {
            NoeudDepart = depart;
            NoeudArrive = arrive;
            Poids = poids;
            Line = line;
        }
    }
}
