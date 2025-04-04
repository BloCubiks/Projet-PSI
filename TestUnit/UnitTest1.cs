using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace PSI_ClovisNOE_JaimeSOUSA_ThomasMAYE
{
    [TestClass]
    public class UnitTestGraphe
    {
        [TestMethod]
        public void TestNoeudExiste()
        {
            Graphe<int> graphe = new Graphe<int>();
            Noeud<int> noeud = new Noeud<int>(1,1);
            graphe.AjouterNoeud(noeud);
            Assert.IsTrue(graphe.Noeud_Existe(noeud));
        }

        [TestMethod]
        public void TestAjouterLien()
        {
            Graphe<int> graphe = new Graphe<int>();
            Noeud<int> n1 = new Noeud<int>(1,1);
            Noeud<int> n2 = new Noeud<int>(2,2);
            graphe.AjouterNoeud(n1);
            graphe.AjouterNoeud(n2);
            Lien<int> lien = new Lien<int>(n1, n2,0);
            graphe.AjouterLien(lien);
            Assert.IsTrue(graphe.Liens.Contains(lien));
        }
        [TestMethod]
        public void TestGenererMatrice()
        {
            Graphe<int> graphe = new Graphe<int>();
            Noeud<int> n1 = new Noeud<int>(1, 1);
            Noeud<int> n2 = new Noeud<int>(2, 2);
            graphe.AjouterNoeud(n1);
            graphe.AjouterNoeud(n2);
            graphe.AjouterLien(new Lien<int>(n1, n2, 1));
            graphe.Generer_Matrice();
            Assert.AreEqual(1, graphe.Matrice_Adjacence[0, 1]);
            Assert.AreEqual(1, graphe.Matrice_Adjacence[1, 0]);
        }

        [TestMethod]
        public void TestParcoursLargeur()
        {
            Graphe<int> graphe = new Graphe<int>();
            Noeud<int> n1 = new Noeud<int>(1, 1);
            Noeud<int> n2 = new Noeud<int>(2, 2);
            Noeud<int> n3 = new Noeud<int>(3, 3);
            graphe.AjouterNoeud(n1);
            graphe.AjouterNoeud(n2);
            graphe.AjouterNoeud(n3);
            graphe.AjouterLien(new Lien<int>(n1, n2,1));
            graphe.AjouterLien(new Lien<int>(n2, n3,1));
            List<Noeud<int>> parcours = graphe.Parcours_Largeur(n1);
            Assert.AreEqual(3, parcours.Count);
            Assert.IsTrue(parcours.Contains(n1) && parcours.Contains(n2) && parcours.Contains(n3));
        }

        [TestMethod]
        public void TestEstConnexe()
        {
            Graphe<int> graphe = new Graphe<int>();
            Noeud<int> n1 = new Noeud<int>(1,1);
            Noeud<int> n2 = new Noeud<int>(2, 2);
            Noeud<int> n3 = new Noeud<int>(3, 3);
            graphe.AjouterNoeud(n1);
            graphe.AjouterNoeud(n2);
            graphe.AjouterNoeud(n3);
            graphe.AjouterLien(new Lien<int>(n1, n2, 1));
            graphe.AjouterLien(new Lien<int>(n2, n3, 1));
            Assert.IsTrue(graphe.Est_Connexe());
        }
    }
}
    
