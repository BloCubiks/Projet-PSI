using System;

namespace PSI_ClovisNOE_JaimeSOUSA_ThomasMAYE
{
    public class Lien<T>
    {
        private Noeud<T> noeudDepart;
        private Noeud<T> noeudArrive;
        private int poids;
        private string line;// Ligne associée à cette liaison (null pour un transfert entre lignes)

        public Noeud<T> NoeudDepart
        {
            get { return noeudDepart; }
        }
        public Noeud<T> NoeudArrive
        {
            get { return noeudArrive; }
        }
        public int Poids
        {
            get { return poids; }
        }
        public string Line
        {
            get { return line; }
        }
        public Lien(Noeud<T> Depart, Noeud<T> Arrive, int Poids, string Line = null)
        {
            noeudDepart = Depart;
            noeudArrive = Arrive;
            poids = Poids;
            line = Line;
        }
    }
}
