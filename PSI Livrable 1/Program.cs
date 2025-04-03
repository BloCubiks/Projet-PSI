using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace PSI_Livrable_1
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            string filePath = "MetroParis.csv";
            List<Station> records = LoadCSV(filePath);

            Dictionary<string, Noeud<int>> stationNodes = new Dictionary<string, Noeud<int>>();
            Dictionary<Noeud<int>, Station> nodeToStation = new Dictionary<Noeud<int>, Station>();
            Dictionary<Noeud<int>, List<string>> nodeLines = new Dictionary<Noeud<int>, List<string>>(); 
            Dictionary<Noeud<int>, (double, double)> nodePositions = new Dictionary<Noeud<int>, (double, double)>(); 

            Graphe<int> graph = new Graphe<int>();

            foreach (var record in records)
            {
                if (stationNodes.ContainsKey(record.LibelleStation))
                {
                    nodeLines[stationNodes[record.LibelleStation]].Add(record.LibelleLine);
                }
                else
                {
                    var node = new Noeud<int>(record.ID, "Station");
                    stationNodes[record.LibelleStation] = node;
                    nodeToStation[node] = record;
                    nodeLines[node] = new List<string> { record.LibelleLine }; 
                    nodePositions[node] = (record.Longitude, record.Latitude); 
                    graph.AjouterNoeud(node);
                }
            }

            var groupedByLine = records.GroupBy(r => r.LibelleLine);
            foreach (var group in groupedByLine)
            {
                var sorted = group.OrderBy(r => r.ID).ToList();
                for (int i = 0; i < sorted.Count - 1; i++)
                {
                    var nodeStart = stationNodes[sorted[i].LibelleStation];
                    var nodeEnd = stationNodes[sorted[i + 1].LibelleStation];

                    double distance = HaversineDistance(sorted[i].Latitude, sorted[i].Longitude, sorted[i + 1].Latitude, sorted[i + 1].Longitude);
                    int travelTime = (int)Math.Round(distance * 2 * 60); 

                    graph.AjouterLien(new Lien<int>(nodeStart, nodeEnd, travelTime, group.Key));
                    graph.AjouterLien(new Lien<int>(nodeEnd, nodeStart, travelTime, group.Key));
                }
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Visualisation<int>(graph, nodeToStation, nodePositions));
        }

        static List<Station> LoadCSV(string filePath)
        {
            List<Station> stations = new List<Station>();
            using (var reader = new StreamReader(filePath))
            {
                string header = reader.ReadLine(); 
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    if (string.IsNullOrWhiteSpace(line))
                        continue;
                    string[] parts = line.Split(',');

                    int id = int.Parse(parts[0]);
                    string libelleLine = parts[1];
                    string libelleStation = parts[2];
                    double longitude = double.Parse(parts[3], CultureInfo.InvariantCulture);
                    double latitude = double.Parse(parts[4], CultureInfo.InvariantCulture);

                    stations.Add(new Station(id, libelleLine, libelleStation, longitude, latitude));
                }
            }
            return stations;
        }

        static double HaversineDistance(double lat1, double lon1, double lat2, double lon2)
        {
            double R = 6371; 
            double dLat = (lat2 - lat1) * Math.PI / 180;
            double dLon = (lon2 - lon1) * Math.PI / 180;
            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                       Math.Cos(lat1 * Math.PI / 180) * Math.Cos(lat2 * Math.PI / 180) *
                       Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return R * c; 
        }
    }
}
