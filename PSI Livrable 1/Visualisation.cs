using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace PSI_ClovisNOE_JaimeSOUSA_ThomasMAYE
{
    public class Visualisation<T> : Form
    {
        private Graphe<T> graph;
        private Dictionary<Noeud<T>, Station> nodeToStation;
        private Dictionary<Noeud<T>, (double, double)> nodePositions;
        private bool ponderation;

        private double minLongitude, maxLongitude, minLatitude, maxLatitude;
        private double scaleX, scaleY;
        private List<string> dejadessine;

        private Dictionary<string, Color> lineColors;

        private HashSet<string> drawnLines;
        
        public static void DessinerGrapheAvecRoles(Graphics g, Graphe<string> graphe, Dictionary<int, string> cuisiniers, Dictionary<int, string> particuliers)
        {
            var nodePositions = new Dictionary<Noeud<string>, PointF>();
            int nodeIndex = 0;
            int radius = 200; 
            PointF center = new PointF(300, 300); 

            foreach (var noeud in graphe.Noeuds)
            {
                double angle = 2 * Math.PI * nodeIndex / graphe.Noeuds.Count;
                float x = center.X + (float)(radius * Math.Cos(angle));
                float y = center.Y + (float)(radius * Math.Sin(angle));
                nodePositions[noeud] = new PointF(x, y);
                nodeIndex++;
            }

            foreach (var lien in graphe.Liens)
            {
                var start = nodePositions[lien.NoeudDepart];
                var end = nodePositions[lien.NoeudArrive];
                g.DrawLine(Pens.Black, start, end);
            }

            foreach (var noeud in graphe.Noeuds)
            {
                var position = nodePositions[noeud];
                bool isCuisinier = cuisiniers.ContainsKey(noeud.Id);
                bool isParticulier = particuliers.ContainsKey(noeud.Id - 4000);

                if (isCuisinier && isParticulier)
                {
                    using (Brush greenBrush = new SolidBrush(Color.Green))
                    using (Brush orangeBrush = new SolidBrush(Color.Orange))
                    {
                        g.FillPie(greenBrush, position.X - 10, position.Y - 10, 20, 20, 0, 180);
                        g.FillPie(orangeBrush, position.X - 10, position.Y - 10, 20, 20, 180, 180);
                    }
                }
                else if (isCuisinier)
                {
                    g.FillEllipse(Brushes.Green, position.X - 10, position.Y - 10, 20, 20);
                }
                else if (isParticulier)
                {
                    g.FillEllipse(Brushes.Orange, position.X - 10, position.Y - 10, 20, 20);
                }

                g.DrawEllipse(Pens.Black, position.X - 10, position.Y - 10, 20, 20);

                string label = isCuisinier ? cuisiniers[noeud.Id] : particuliers[noeud.Id - 4000];
                g.DrawString(label, new Font("Arial", 10), Brushes.Black, position.X + 12, position.Y);
            }
        }
        public Visualisation(Graphe<T> graph, Dictionary<Noeud<T>, Station> nodeToStation, Dictionary<Noeud<T>, (double, double)> nodePositions, bool ponderation=false)
        {
            this.graph = graph;
            this.nodeToStation = nodeToStation;
            this.nodePositions = nodePositions;
            this.ponderation = ponderation;
            this.Text = "Visualisation du Graphe";
            this.Size = new Size(800, 600);
            this.MinimumSize = new Size(500, 400);  // Optionnel: définir une taille minimale pour la fenêtre
            this.Resize += OnResize;  // S'abonner à l'événement de redimensionnement
            InitializeScaling();  // Initialisation des échelles

            InitializeLineColors();
            dejadessine = new List<string>();
            drawnLines = new HashSet<string>();

        }

        private void InitializeScaling()
        {
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

            scaleX = (this.ClientSize.Width - 100) / (maxLongitude - minLongitude); 
            scaleY = (this.ClientSize.Height - 100) / (maxLatitude - minLatitude);  
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
            dejadessine.Clear();
            drawnLines.Clear();
        }
        private float GetDistanceFromPointToLine(PointF lineStart, PointF lineEnd, PointF point)
        {
            float lineLength = (float)Math.Sqrt(Math.Pow(lineEnd.X - lineStart.X, 2) + Math.Pow(lineEnd.Y - lineStart.Y, 2));

            if (lineLength == 0)
            {
                return (float)Math.Sqrt(Math.Pow(point.X - lineStart.X, 2) + Math.Pow(point.Y - lineStart.Y, 2));
            }
            float dotProduct = ((point.X - lineStart.X) * (lineEnd.X - lineStart.X)) + ((point.Y - lineStart.Y) * (lineEnd.Y - lineStart.Y));
            float projection = dotProduct / lineLength;
            if (projection < 0)
            {
                return (float)Math.Sqrt(Math.Pow(point.X - lineStart.X, 2) + Math.Pow(point.Y - lineStart.Y, 2));
            }
            else if (projection > lineLength)
            {
                return (float)Math.Sqrt(Math.Pow(point.X - lineEnd.X, 2) + Math.Pow(point.Y - lineEnd.Y, 2));
            }

            float height = (float)(Math.Abs((lineEnd.Y - lineStart.Y) * point.X - (lineEnd.X - lineStart.X) * point.Y + lineEnd.X * lineStart.Y - lineEnd.Y * lineStart.X) / lineLength);
            return height;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;

            Dictionary<Noeud<T>, PointF> scaledPositions = new Dictionary<Noeud<T>, PointF>();

            foreach (var node in graph.Noeuds)
            {

                var position = nodePositions[node];

                float x = (float)((position.Item1 - minLongitude) * scaleX)+50;
                float y = (float)((maxLatitude - position.Item2) * scaleY)+50; 

                scaledPositions[node] = new PointF(x, y);
            }

            List<(PointF start, PointF end)> lineSegments = new List<(PointF start, PointF end)>();

            foreach (var lien in graph.Liens)
            {
                var start = scaledPositions[lien.NoeudDepart];
                var end = scaledPositions[lien.NoeudArrive];

                lineSegments.Add((start, end));

                string lineName = lien.Line;
                if (lineName == null)
                {
                    lineName = "Transfert"; 
                }
                Color lineColor = lineColors.ContainsKey(lineName) ? lineColors[lineName] : Color.Gray; 

                using (Pen pen = new Pen(lineColor, 4))
                {
                    g.DrawLine(pen, start, end);
                }
                if (ponderation)
                {
                    PointF midPoint = new PointF((start.X + end.X) / 2, (start.Y + end.Y) / 2);
                    if (lien.Poids > 0)
                    {
                        g.DrawString(lien.Poids.ToString(), new Font("Arial", 12), Brushes.Black, midPoint);
                        if (!drawnLines.Contains(lineName))
                        {
                            PointF labelPosition = new PointF((start.X + end.X) / 2, (start.Y + end.Y) / 2);
                            labelPosition.Y -= 15;
                            Color textColor = lineColors.ContainsKey(lineName) ? lineColors[lineName] : Color.Gray;
                            g.DrawString("Ligne : " + lineName, new Font("Arial", 10, FontStyle.Bold), new SolidBrush(textColor), labelPosition);
                            drawnLines.Add(lineName);
                        }
                    }
                }
            }

            HashSet<RectangleF> occupiedAreas = new HashSet<RectangleF>(); 

            foreach (var node in graph.Noeuds)
            {
                var position = scaledPositions[node];

                g.FillEllipse(Brushes.White, position.X - 5, position.Y - 5, 10, 10);

                using (Pen blackPen = new Pen(Color.Black, 1)) 
                {
                    g.DrawEllipse(blackPen, position.X - 5, position.Y - 5, 10, 10);
                }

                float textX = position.X + 12;
                float textY = position.Y;

                SizeF textSize = g.MeasureString(nodeToStation[node].LibelleStation, new Font("Arial", 6));
                RectangleF textArea = new RectangleF(textX, textY, textSize.Width, textSize.Height);

                int offsetY = 0;
                while (occupiedAreas.Contains(textArea))
                {
                    offsetY += (int)textSize.Height + 2; 
                    textArea = new RectangleF(textX, textY + offsetY, textSize.Width, textSize.Height);
                }

                bool overlapWithLine = false;
                foreach (var segment in lineSegments)
                {
                    float distance = GetDistanceFromPointToLine(segment.start, segment.end, new PointF(textX, textY));
                    if (distance < 5) 
                    {
                        overlapWithLine = true;
                        break;
                    }
                }

                if (overlapWithLine)
                {
                    textY += (int)(textSize.Height + 5); 
                }

                if (!dejadessine.Contains(nodeToStation[node].LibelleStation))
                {
                    g.DrawString(nodeToStation[node].LibelleStation, new Font("Arial", 6), Brushes.Black, textX, textY + offsetY);
                    dejadessine.Add(nodeToStation[node].LibelleStation);
                }

                occupiedAreas.Add(textArea);
            }
        }
    }
}