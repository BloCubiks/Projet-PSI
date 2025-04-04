using System;
using System.Collections.Generic;

namespace PSI_Livrable_1_ClovisNOE_JaimeSOUSA_ThomasMAYE
{
    public class Station
    {
        private int idStation;
        private string libelleStation;
        private string libelleLine;
        private double longitude;
        private double latitude;

        public int IdStation
        {
            get { return idStation; }
        }
        public string LibelleStation
        {
            get { return libelleStation; }
        }
        public string LibelleLine
        {
            get { return libelleLine; }
        }
        public double Longitude
        {
            get { return longitude; }
        }
        public double Latitude
        {
            get { return latitude; }
        }
        public Station(int Id, string LibelleLine, string LibelleStation, double Longitude, double Latitude)
        {
            idStation = Id;
            libelleStation = LibelleStation;
            libelleLine = LibelleLine;
            longitude = Longitude;
            latitude = Latitude;
        }
    }
}
