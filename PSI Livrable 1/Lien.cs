using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSI_Livrable_1
{
    public class Lien
    {
        private Noeud noeudDepart;
        private Noeud noeudArrive;
        private int poids;

        public Noeud NoeudDepart
        {
            get { return noeudDepart; }
        }
        public Noeud NoeudArrive
        {
            get { return noeudArrive; }
        }
        public int Poids
        {
            get { return poids; }
        }
        public Lien(Noeud Noeud1, Noeud Noeud2, int poids)
        {
            noeudDepart = Noeud1;
            noeudArrive = Noeud2;
            this.poids = poids;
        }
    }
}
