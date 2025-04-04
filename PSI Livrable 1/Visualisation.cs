using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace PSI_Livrable_1_ClovisNOE_JaimeSOUSA_ThomasMAYE
{
    public class Visualisation<T> : Form
    {
        private Graphe<T> graph;
        private Dictionary<Noeud<T>, Station> nodeToStation;
        private Dictionary<Noeud<T>, (double, double)> nodePositions;

        // Variables pour la mise à l'échelle
        private double minLongitude, maxLongitude, minLatitude, maxLatitude;
        private double scaleX, scaleY;
        private List<string> dejadessine;

        // Dictionnaire des couleurs pour chaque ligne
        private Dictionary<string, Color> lineColors;

        public Visualisation(Graphe<T> graph, Dictionary<Noeud<T>, Station> nodeToStation, Dictionary<Noeud<T>, (double, double)> nodePositions)
        {
            this.graph = graph;
            this.nodeToStation = nodeToStation;
            this.nodePositions = nodePositions;

            this.Text = "Visualisation du Graphe";
            this.Size = new Size(800, 600);
            this.MinimumSize = new Size(500, 400);  // Optionnel: définir une taille minimale pour la fenêtre
            this.Resize += OnResize;  // S'abonner à l'événement de redimensionnement
            InitializeScaling();  // Initialisation des échelles

            // Initialiser le dictionnaire des couleurs des lignes
            InitializeLineColors();
            dejadessine = new List<string>();
        }

        private void InitializeScaling()
        {
            // Trouver les coordonnées minimales et maximales
            minLongitude = double.MaxValue;
            maxLongitude = double.MinValue;
            minLatitude = double.MaxValue;
            maxLatitude = double.MinValue;

            foreach (var position in nodePositions.Values)
            {
                minLongitude = Math.Min(minLongitude, position.Item1);
                maxLongitude = Math.Max(maxLongitude, position.Item1);
                minLatitude = Math.Min(minLatitude, position.Item2);
                maxLatitude = Math.Max(maxLatitude, position.Item2);
            }

            // Calculer l'échelle en fonction des coordonnées
            scaleX = (this.ClientSize.Width - 50) / (maxLongitude - minLongitude);  // 50 pour une marge
            scaleY = (this.ClientSize.Height - 50) / (maxLatitude - minLatitude);    // 50 pour une marge
        }

        private void InitializeLineColors()
        {
            // Associer chaque ligne de métro à sa couleur conventionnelle
            lineColors = new Dictionary<string, Color>
        {
        { "1", ColorTranslator.FromHtml("#FFCE00") },    // Ligne 1 - Jaune
        { "2", ColorTranslator.FromHtml("#0064B0") },    // Ligne 2 - Bleu
        { "3", ColorTranslator.FromHtml("#9F9825") },    // Ligne 3 - Vert
        { "3bis", ColorTranslator.FromHtml("#98D4E2") }, // Ligne 3 bis - Bleu clair
        { "4", ColorTranslator.FromHtml("#C04191") },    // Ligne 4 - Violet
        { "5", ColorTranslator.FromHtml("#F28E42") },    // Ligne 5 - Orange
        { "6", ColorTranslator.FromHtml("#83C491") },    // Ligne 6 - Vert pâle
        { "7", ColorTranslator.FromHtml("#F3A4BA") },    // Ligne 7 - Rose
        { "7bis", ColorTranslator.FromHtml("#83C491") }, // Ligne 7 bis - Vert pâle
        { "8", ColorTranslator.FromHtml("#CEADD2") },    // Ligne 8 - Lavande
        { "9", ColorTranslator.FromHtml("#D5C900") },    // Ligne 9 - Jaune vif
        { "10", ColorTranslator.FromHtml("#E3B32A") },   // Ligne 10 - Or
        { "11", ColorTranslator.FromHtml("#8D5E2A") },   // Ligne 11 - Brun
        { "12", ColorTranslator.FromHtml("#00814F") },   // Ligne 12 - Vert foncé
        { "13", ColorTranslator.FromHtml("#98D4E2") },   // Ligne 13 - Bleu clair
        { "14", ColorTranslator.FromHtml("#662483") },   // Ligne 14 - Violet foncé
        { "15", ColorTranslator.FromHtml("#B90845") },   // Ligne 15 - Rouge
        { "16", ColorTranslator.FromHtml("#F3A4BA") },   // Ligne 16 - Rose
        { "17", ColorTranslator.FromHtml("#D5C900") },   // Ligne 17 - Jaune vif
        { "18", ColorTranslator.FromHtml("#00A88F") },   // Ligne 18 - Turquoise
            };
        }


        private void OnResize(object sender, EventArgs e)
        {
            // Recalculez les échelles chaque fois que la fenêtre est redimensionnée
            InitializeScaling();
            this.Invalidate();  // Redessiner le contenu de la fenêtre
            dejadessine = new List<string>();
        }
        private float GetDistanceFromPointToLine(PointF lineStart, PointF lineEnd, PointF point)
        {
            // Calculer la longueur du segment de ligne
            float lineLength = (float)Math.Sqrt(Math.Pow(lineEnd.X - lineStart.X, 2) + Math.Pow(lineEnd.Y - lineStart.Y, 2));

            // Cas où la ligne est un point (les deux extrémités sont les mêmes)
            if (lineLength == 0)
            {
                return (float)Math.Sqrt(Math.Pow(point.X - lineStart.X, 2) + Math.Pow(point.Y - lineStart.Y, 2));
            }

            // Calculer le produit scalaire entre les vecteurs (point - start) et (lineEnd - lineStart)
            float dotProduct = ((point.X - lineStart.X) * (lineEnd.X - lineStart.X)) + ((point.Y - lineStart.Y) * (lineEnd.Y - lineStart.Y));

            // Calculer la projection du point sur la ligne
            float projection = dotProduct / lineLength;

            // Si la projection est en dehors du segment de ligne, prendre la distance avec les extrémités
            if (projection < 0)
            {
                return (float)Math.Sqrt(Math.Pow(point.X - lineStart.X, 2) + Math.Pow(point.Y - lineStart.Y, 2));
            }
            else if (projection > lineLength)
            {
                return (float)Math.Sqrt(Math.Pow(point.X - lineEnd.X, 2) + Math.Pow(point.Y - lineEnd.Y, 2));
            }

            // Calculer la distance perpendiculaire entre le point et la ligne
            float height = (float)(Math.Abs((lineEnd.Y - lineStart.Y) * point.X - (lineEnd.X - lineStart.X) * point.Y + lineEnd.X * lineStart.Y - lineEnd.Y * lineStart.X) / lineLength);
            return height;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;

            // 1. Placer les stations (nœuds)
            Dictionary<Noeud<T>, PointF> scaledPositions = new Dictionary<Noeud<T>, PointF>();

            foreach (var node in graph.Noeuds)
            {

                var position = nodePositions[node];

                // Normalisation des coordonnées en fonction de la mise à l'échelle
                float x = (float)((position.Item1 - minLongitude) * scaleX);
                // Inverser l'axe Y en soustrayant la position Y de la hauteur totale de la fenêtre
                float y = (float)((maxLatitude - position.Item2) * scaleY);  // Inverser la position Y ici

                scaledPositions[node] = new PointF(x, y);
            }

            // 2. Dessiner les liens (lignes) entre les stations
            List<(PointF start, PointF end)> lineSegments = new List<(PointF start, PointF end)>();

            foreach (var lien in graph.Liens)
            {
                var start = scaledPositions[lien.NoeudDepart];
                var end = scaledPositions[lien.NoeudArrive];

                // Ajouter les segments de ligne à la liste
                lineSegments.Add((start, end));

                // Obtenir la couleur de la ligne
                string lineName = lien.Line; // La ligne est associée au lien
                if (lineName == null)
                {
                    lineName = "Transfert"; // Ligne de transfert
                }
                Color lineColor = lineColors.ContainsKey(lineName) ? lineColors[lineName] : Color.Gray; // Couleur par défaut (gris) si ligne inconnue

                // Dessiner une ligne entre les stations avec la couleur correspondante
                using (Pen pen = new Pen(lineColor, 2))
                {
                    g.DrawLine(pen, start, end);
                }
            }

            // 3. Dessiner les stations sous forme de cercles (ou autres formes)
            HashSet<RectangleF> occupiedAreas = new HashSet<RectangleF>();  // Pour suivre les zones occupées par les textes

            foreach (var node in graph.Noeuds)
            {
                var position = scaledPositions[node];

                // Dessiner un cercle pour chaque station (blanc)
                g.FillEllipse(Brushes.White, position.X - 5, position.Y - 5, 10, 10);

                // Dessiner un contour noir autour du cercle
                using (Pen blackPen = new Pen(Color.Black, 1))  // 1 pixel de largeur
                {
                    g.DrawEllipse(blackPen, position.X - 5, position.Y - 5, 10, 10);
                }

                // Initialisation de la position du texte
                float textX = position.X + 12;
                float textY = position.Y;

                // Vérifier si l'espace est déjà occupé par un texte
                SizeF textSize = g.MeasureString(nodeToStation[node].LibelleStation, new Font("Arial", 6));
                RectangleF textArea = new RectangleF(textX, textY, textSize.Width, textSize.Height);

                // Si la zone est déjà occupée, essayer de décaler verticalement
                int offsetY = 0;
                while (occupiedAreas.Contains(textArea))
                {
                    offsetY += (int)textSize.Height + 2;  // Décaler de la hauteur du texte + un peu d'espace
                    textArea = new RectangleF(textX, textY + offsetY, textSize.Width, textSize.Height);
                }

                // Vérifier la distance avec les lignes (segments)
                bool overlapWithLine = false;
                foreach (var segment in lineSegments)
                {
                    float distance = GetDistanceFromPointToLine(segment.start, segment.end, new PointF(textX, textY));
                    if (distance < 5) // Si l'étiquette est trop proche de la ligne
                    {
                        overlapWithLine = true;
                        break;
                    }
                }

                // Si l'étiquette se superpose à une ligne, la déplacer
                if (overlapWithLine)
                {
                    textY += (int)(textSize.Height + 5);  // Déplacer l'étiquette plus loin
                }

                // Dessiner le texte de la station
                if (!dejadessine.Contains(nodeToStation[node].LibelleStation))
                {
                    g.DrawString(nodeToStation[node].LibelleStation, new Font("Arial", 6), Brushes.Black, textX, textY + offsetY);
                    dejadessine.Add(nodeToStation[node].LibelleStation);
                }

                // Marquer cette zone comme occupée
                occupiedAreas.Add(textArea);
            }
        }
    }
}