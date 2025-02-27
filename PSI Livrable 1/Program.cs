using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PSI_Livrable_1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string[] lines = File.ReadAllLines("soc-karate.mtx");//lecture fichier
                Graphe Association = new Graphe();
                int i = 0;
                while (lines[i][0] == '%') //ignorer les commentaires
                {
                    i++;
                }
                int nbNoeuds = int.Parse(lines[i].Split(' ')[0]); //nombre de noeuds
                int nbLiens = int.Parse(lines[i].Split(' ')[2]); //nombre de liens
                for (int k = 1; k <= nbNoeuds; k++)//ajouter les noeuds de 0 a nbNoeuds-1
                {
                    Noeud n = new Noeud(k);
                    Association.AjouterNoeud(n);
                }
                for (int j = i+1; j < nbLiens + i + 1; j++) //creation des liens a partir de la ligne i+1
                {
                    int n1 = int.Parse(lines[j].Split(' ')[0]); //noeud 1 du lien
                    int p = 0; //verification de la position du noeud dans les noeuds existants
                    while (Association.Noeuds[p].Id != n1)
                    {
                        p++;
                    }
                    int n2 = int.Parse(lines[j].Split(' ')[1]); //noeud 2 du lien
                    int q = 0; //verification de la position du noeud dans les noeuds existants
                    while (Association.Noeuds[q].Id != n2)
                    {
                        q++;
                    }
                    Association.AjouterLien(new Lien(Association.Noeuds[p], Association.Noeuds[q])); //creation du lien
                }
                Association.AfficherListeAdjacence(); //affichage de la liste d'adjacence
                Association.Generer_Matrice(); //generation de la matrice d'adjacence
                Console.WriteLine("Matrice d'adjacence : "); // affichage de la matrice d'adjacence
                for (int u = 0; u < Association.Noeuds.Count; u++)
                {
                    for (int v = 0; v < Association.Noeuds.Count; v++)
                    {
                        Console.Write(Association.Matrice_Adjacence[u, v] + " ");
                    }
                    Console.WriteLine();
                }
                List<Noeud> parcours_Large = Association.Parcours_Largeur(Association.Noeuds[0]); //parcours en largeur
                Console.WriteLine("Parcours en largeur : ");
                foreach (Noeud n in parcours_Large)
                {
                    Console.Write(n.Id + " ");
                }
                List<Noeud> parcours_Long = Association.Parcours_Longueur(Association.Noeuds[0]); //parcours en Longueur
                Console.WriteLine("\nParcours en Longueur : ");
                foreach (Noeud n in parcours_Long)
                {
                    Console.Write(n.Id + " ");
                }
            }
            catch (FileNotFoundException f)
            {
                Console.WriteLine("le fichier n'existe pas " + f.Message);
            }
            catch (ArgumentException f)
            {
                Console.WriteLine("Erreur " + f.Message);
            }
            catch (PathTooLongException f)
            {
                Console.WriteLine("Erreur " + f.Message);
            }
            catch (DirectoryNotFoundException f)
            {
                Console.WriteLine("Erreur " + f.Message);
            }
            catch (UnauthorizedAccessException f)
            {
                Console.WriteLine("Erreur " + f.Message);
            }
            catch (NotSupportedException f)
            {
                Console.WriteLine("Erreur " + f.Message);
            }
            catch (IOException f)
            {
                Console.WriteLine("Erreur " + f.Message);
            }
        }
    }
}
