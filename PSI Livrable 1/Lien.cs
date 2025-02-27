using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSI_Livrable_1
{
    internal class Lien
    {
        private Noeud noeud1;
        private Noeud noeud2;

        public Noeud Noeud1
        {
            get { return noeud1; }
        }

        public Noeud Noeud2
        {
            get { return noeud2; }
        }

        public Lien(Noeud Noeud1, Noeud Noeud2)
        {
            noeud1 = Noeud1;
            noeud2 = Noeud2;
        }
    }
}
