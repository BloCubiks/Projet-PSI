using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace PSI_Livrable_1
{
    [TestClass]
    public class UnitTestGraphe
    {
        [TestMethod]
        public void TestNoeudExiste()
        {
            Graphe graphe = new Graphe();
            Noeud noeud = new Noeud(1);
            graphe.AjouterNoeud(noeud);
            Assert.IsTrue(graphe.Noeud_Existe(noeud));
        }

        [TestMethod]
        public void TestAjouterLien()
        {
            Graphe graphe = new Graphe();
            Noeud n1 = new Noeud(1);
            Noeud n2 = new Noeud(2);
            graphe.AjouterNoeud(n1);
            graphe.AjouterNoeud(n2);
            Lien lien = new Lien(n1, n2);
            graphe.AjouterLien(lien);
            Assert.IsTrue(graphe.Liens.Contains(lien));
        }
        [TestMethod]
        public void TestGenererMatrice()
        {
            Graphe graphe = new Graphe();
            Noeud n1 = new Noeud(1);
            Noeud n2 = new Noeud(2);
            graphe.AjouterNoeud(n1);
            graphe.AjouterNoeud(n2);
            graphe.AjouterLien(new Lien(n1, n2));
            graphe.Generer_Matrice();
            Assert.AreEqual(1, graphe.Matrice_Adjacence[0, 1]);
            Assert.AreEqual(1, graphe.Matrice_Adjacence[1, 0]);
        }

        [TestMethod]
        public void TestParcoursLargeur()
        {
            Graphe graphe = new Graphe();
            Noeud n1 = new Noeud(1);
            Noeud n2 = new Noeud(2);
            Noeud n3 = new Noeud(3);
            graphe.AjouterNoeud(n1);
            graphe.AjouterNoeud(n2);
            graphe.AjouterNoeud(n3);
            graphe.AjouterLien(new Lien(n1, n2));
            graphe.AjouterLien(new Lien(n2, n3));
            List<Noeud> parcours = graphe.Parcours_Largeur(n1);
            Assert.AreEqual(3, parcours.Count);
            Assert.IsTrue(parcours.Contains(n1) && parcours.Contains(n2) && parcours.Contains(n3));
        }

        [TestMethod]
        public void TestEstConnexe()
        {
            Graphe graphe = new Graphe();
            Noeud n1 = new Noeud(1);
            Noeud n2 = new Noeud(2);
            Noeud n3 = new Noeud(3);
            graphe.AjouterNoeud(n1);
            graphe.AjouterNoeud(n2);
            graphe.AjouterNoeud(n3);
            graphe.AjouterLien(new Lien(n1, n2));
            graphe.AjouterLien(new Lien(n2, n3));
            Assert.IsTrue(graphe.Est_Connexe());
        }
    }
}
    
