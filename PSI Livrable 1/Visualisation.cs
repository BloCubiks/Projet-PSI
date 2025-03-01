using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace PSI_Livrable_1
{
    public class Visualisation : Form
    {
        private Graphe graphe;
        private Dictionary<Noeud, Point> positionsNoeuds;
        private const int NODE_RADIUS = 20;
        private const int MARGIN = 50;
        public Visualisation(Graphe g)
        {
            graphe = g;
            positionsNoeuds = new Dictionary<Noeud, Point>();
            this.Text = "Visualisation du Graphe";
            this.Size = new Size(800, 600);
            this.DoubleBuffered = true;
            CalculerPositions();
        }

        private void CalculerPositions()
        {
            int centerX = this.ClientSize.Width / 2;
            int centerY = this.ClientSize.Height / 2;
            int radius = Math.Min(centerX, centerY) - MARGIN;

            double angleStep = 2 * Math.PI / graphe.Noeuds.Count;
            int i = 0;

            foreach (Noeud n in graphe.Noeuds)
            {
                double angle = angleStep * i;
                int x = centerX + (int)(radius * Math.Cos(angle));
                int y = centerY + (int)(radius * Math.Sin(angle));
                positionsNoeuds[n] = new Point(x, y);
                i++;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // Dessiner les liens
            foreach (Lien lien in graphe.Liens)
            {
                Point p1 = positionsNoeuds[lien.Noeud1];
                Point p2 = positionsNoeuds[lien.Noeud2];
                g.DrawLine(Pens.Black, p1, p2);
            }

            // Dessiner les noeuds
            foreach (var kvp in positionsNoeuds)
            {
                Rectangle rect = new Rectangle(
                    kvp.Value.X - NODE_RADIUS,
                    kvp.Value.Y - NODE_RADIUS,
                    NODE_RADIUS * 2,
                    NODE_RADIUS * 2
                );

                g.FillEllipse(Brushes.LightBlue, rect);
                g.DrawEllipse(Pens.Black, rect);

                StringFormat sf = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                };

                g.DrawString(
                    kvp.Key.Id.ToString(),
                    this.Font,
                    Brushes.Black,
                    rect,
                    sf
                );
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            CalculerPositions();
            this.Invalidate();
        }
    }
}