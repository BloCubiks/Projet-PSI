using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSI_Livrable_1
{
    public class Noeud
    {
        private int id;

        public int Id
        {
            get { return id; }
        }
        public Noeud(int Id)
        {
            id = Id;
        }
    }
}
