using System;

namespace PSI_Livrable_1_ClovisNOE_JaimeSOUSA_ThomasMAYE
{
    public class Noeud<T>
    {
        private int id;
        private T type;

        public int Id
        {
            get { return id; }
        }
        public T Type
        {
            get { return type; }
        }
        public Noeud(int Id, T Type)
        {
            this.id = Id;
            this.type = Type;
        }
    }
}
