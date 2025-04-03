using System;

namespace PSI_Livrable_1
{
    public class Noeud<T>
    {
        public T Id { get; set; } 
        public string Type { get; set; } 

        public Noeud(T id, string type)
        {
            this.Id = id;
            this.Type = type;
        }
    }
}
