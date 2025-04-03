using System;
using System.Collections.Generic;

namespace PSI_Livrable_1
{
    public class Station
    {
        public int ID { get; set; }
        public string LibelleLine { get; set; }
        public string LibelleStation { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public List<string> Lignes { get; set; } 

        public Station(int id, string libelleLine, string libelleStation, double longitude, double latitude)
        {
            ID = id;
            LibelleLine = libelleLine;
            LibelleStation = libelleStation;
            Longitude = longitude;
            Latitude = latitude;
            Lignes = new List<string> { libelleLine };
        }

        public void AjouterLigne(string ligne)
        {
            if (!Lignes.Contains(ligne))
                Lignes.Add(ligne);
        }
    }
}
