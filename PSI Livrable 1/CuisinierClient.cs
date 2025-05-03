using PSI_ClovisNOE_JaimeSOUSA_ThomasMAYE;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

public class CuisinierClient : Form
{
    private Graphe<string> grapheCommande;
    private Dictionary<int, string> cuisiniers;
    private Dictionary<int, string> particuliers;
    private Dictionary<Noeud<string>, int> couleurs;
    private List<Color> colors;

    public CuisinierClient(Graphe<string> grapheCommande, Dictionary<int, string> cuisiniers, Dictionary<int, string> particuliers, Dictionary<Noeud<string>,int> couleurs)
    {
        this.grapheCommande = grapheCommande;
        this.cuisiniers = cuisiniers;
        this.particuliers = particuliers;
        this.couleurs = couleurs;
        this.Text = "Visualisation du Graphe";
        this.Size = new Size(800, 600);
        this.DoubleBuffered = true; 
        this.colors = new List<Color>
        {
            Color.Red,
            Color.Blue,
            Color.Green,
            Color.Yellow,
            Color.Purple,
            Color.Orange,
            Color.Cyan,
            Color.Magenta
        };

        this.Resize += (sender, args) => this.Invalidate();
    }
    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);

        Graphics g = e.Graphics;

        int width = this.ClientSize.Width;
        int height = this.ClientSize.Height;
        int nodeRadius = Math.Min(width, height) / 35;
        int circleRadius = Math.Min(width, height) / 3;
        Point center = new Point(width / 2, height / 2);

        Dictionary<int, Point> positions = new Dictionary<int, Point>();

        int totalNodes = grapheCommande.Noeuds.Count;
        double angleStep = 2 * Math.PI / totalNodes;

        // Dessiner les noeuds
        int index = 0;
        foreach (var noeud in grapheCommande.Noeuds)
        {
            double angle = index * angleStep;
            int x = center.X + (int)(circleRadius * Math.Cos(angle));
            int y = center.Y + (int)(circleRadius * Math.Sin(angle));
            positions[noeud.Id] = new Point(x, y);

            string nom = "";
            bool isCuisinier = false;
            bool isParticulier = false;

            if (cuisiniers.ContainsKey(noeud.Id))
            {
                nom = cuisiniers[noeud.Id];
                isCuisinier = true;
            }
            else if (particuliers.ContainsKey(noeud.Id - 4000))
            {
                nom = particuliers[noeud.Id - 4000];
                isParticulier = true;
            }

            Pen borderPen = new Pen(Color.Black, 2);

            if (isCuisinier && isParticulier)
            {
                using (Pen orangePen = new Pen(Color.FromArgb(194, 233, 148), 5))//vert
                using (Pen greenPen = new Pen(Color.FromArgb(233, 192, 44), 5))//beige
                {
                    g.DrawArc(orangePen, x - nodeRadius, y - nodeRadius, 2 * nodeRadius, 2 * nodeRadius, 0, 180);
                    g.DrawArc(greenPen, x - nodeRadius, y - nodeRadius, 2 * nodeRadius, 2 * nodeRadius, 180, 180);
                }
            }
            else if (isCuisinier)
            {
                borderPen = new Pen(Color.FromArgb(194, 233, 148), 5); //vert
            }
            else if (isParticulier)
            {
                borderPen = new Pen(Color.FromArgb(233, 192, 44), 5); //beige
            }
            Color fillColor = Color.White;
            if (couleurs.TryGetValue(noeud, out int colorIndex) && colorIndex < colors.Count)
            {
                fillColor = colors[colorIndex];
            }
            // Dessiner le cercle rempli
            using (Brush brush = new SolidBrush(fillColor))
            {
                g.FillEllipse(brush, x - nodeRadius, y - nodeRadius, 2 * nodeRadius, 2 * nodeRadius);
            }
            g.DrawEllipse(borderPen, x - nodeRadius, y - nodeRadius, 2 * nodeRadius, 2 * nodeRadius);

            Font font = new Font("Arial", 10);
            g.DrawString(nom, font, Brushes.Black, new PointF(x + nodeRadius + 5, y - nodeRadius));

            index++;
        }

        // Dessiner les liens
        foreach (var lien in grapheCommande.Liens)
        {
            if (positions.TryGetValue(lien.NoeudDepart.Id, out var start) &&
                positions.TryGetValue(lien.NoeudArrive.Id, out var end))
            {
                Pen pen = new Pen(Color.Black, 2);
                g.DrawLine(pen, start, end);
            }
        }
    }


    public static void VisualiserGraphe(Graphe<string> graphe, Dictionary<int, string> cuisiniers, Dictionary<int, string> particuliers, Dictionary<Noeud<string>, int> couleurs)
    {
        Application.Run(new CuisinierClient(graphe,cuisiniers,particuliers,couleurs));
    }
}
