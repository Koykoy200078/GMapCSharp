using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Map
{
    public partial class Main : Form
    {
        private DataTable dataTable;
        private int currentLabelIndex = 0;
        private List<CustomMarker> markers = new List<CustomMarker>();

        public Main()
        {
            InitializeComponent();
            this.KeyPreview = true;
        }

        private void Main_Load(object sender, EventArgs e)
        {
            //pictureBox1.Image = Image.FromFile("C:\\Users\\Franc\\Desktop\\asasasa.png");
            pictureBox1.Paint += new PaintEventHandler(pictureBox1_Paint);

            dataTable = createData();
            // Location Nodes
            dataTable.Rows.Add(1, 932, 492);
            dataTable.Rows.Add(2, 698, 416);
            dataTable.Rows.Add(3, 371, 301);
            dataTable.Rows.Add(4, 1236, 659);
            dataTable.Rows.Add(5, 932, 743);
            dataTable.Rows.Add(6, 585, 636);
            dataTable.Rows.Add(7, 462, 561);
            dataTable.Rows.Add(8, 494, 688);
            dataTable.Rows.Add(9, 135, 616);
            dataTable.Rows.Add(10, 479, 203);
            dataTable.Rows.Add(11, 705, 204);
            dataTable.Rows.Add(12, 1524, 667);
            dataTable.Rows.Add(13, 1182, 228);
            dataTable.Rows.Add(14, 938, 116);
            dataTable.Rows.Add(15, 1119, 580);
            dataTable.Rows.Add(16, 1079, 705);
            dataTable.Rows.Add(17, 1524, 779);
            dataTable.Rows.Add(18, 1372, 359);
            dataTable.Rows.Add(19, 326, 486);
            dataTable.Rows.Add(20, 1517, 165);
            dataTable.Rows.Add(21, 1097, 94);
            dataTable.Rows.Add(22, 1317, 449);
            dataTable.Rows.Add(23, 1415, 527);
            dataTable.Rows.Add(24, 1370, 661);
            dataTable.Rows.Add(25, 1437, 717);
            dataTable.Rows.Add(26, 1197, 408);
            dataTable.Rows.Add(27, 121, 182);
            dataTable.Rows.Add(28, 110, 364);
            dataTable.Rows.Add(29, 77, 806);
            dataTable.Rows.Add(30, 217, 46);
            // End Location Nodes

            LoadPredefinedData();
        }

        private void LoadPredefinedData()
        {
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    int id = Convert.ToInt32(row["id"]);
                    int x = Convert.ToInt32(row["x"]);
                    int y = Convert.ToInt32(row["y"]);

                    CustomMarker m = new CustomMarker(new Point(x, y), GetLabelFromIndex(currentLabelIndex), false, Color.Red);
                    markers.Add(m);
                    currentLabelIndex++;
                }

                // Initial connections
                foreach (var marker in markers)
                {
                    marker.NearestNeighbors = markers
                        .Where(m => m != marker)
                        .OrderBy(m => GetDistance(marker.Position, m.Position))
                        .Take(3)
                        .ToList();

                    foreach (var neighbor in marker.NearestNeighbors)
                    {
                        if (!neighbor.NearestNeighbors.Contains(marker))
                        {
                            neighbor.NearestNeighbors.Add(marker);
                        }
                    }
                }

                // Randomly select 10 nodes to partially disconnect
                Random random = new Random();
                List<CustomMarker> partiallyDisconnectedMarkers = new List<CustomMarker>();

                while (partiallyDisconnectedMarkers.Count < 10)
                {
                    int randomIndex = random.Next(markers.Count);
                    CustomMarker partiallyDisconnectedMarker = markers[randomIndex];
                    if (!partiallyDisconnectedMarkers.Contains(partiallyDisconnectedMarker))
                    {
                        partiallyDisconnectedMarkers.Add(partiallyDisconnectedMarker);
                    }
                }

                foreach (var marker in partiallyDisconnectedMarkers)
                {
                    int disconnectCount = random.Next(1, 3);
                    for (int i = 0; i < disconnectCount; i++)
                    {
                        if (marker.NearestNeighbors.Count > 1)
                        {
                            var neighborToRemove = marker.NearestNeighbors[random.Next(marker.NearestNeighbors.Count)];
                            marker.NearestNeighbors.Remove(neighborToRemove);
                            neighborToRemove.NearestNeighbors.Remove(marker);
                        }
                    }
                }

                pictureBox1.Invalidate();
            }
        }

        private string GetLabelFromIndex(int index)
        {
            return (index + 1).ToString();
        }

        private DataTable createData()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("id");
            dt.Columns.Add("x");
            dt.Columns.Add("y");
            dt.AcceptChanges();
            return dt;
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            using (Pen pen = new Pen(Color.Black, 1))
            {
                Font font = new Font("Arial", 10);
                Brush brush = Brushes.Black;

                foreach (var marker in markers)
                {
                    foreach (var neighbor in marker.NearestNeighbors)
                    {
                        e.Graphics.DrawLine(pen, marker.Position, neighbor.Position);

                        if (!depthfirstSearchToolStripMenuItem.Checked)
                        {
                            double distance = GetDistance(marker.Position, neighbor.Position);
                            Point midPoint = new Point((marker.Position.X + neighbor.Position.X) / 2, (marker.Position.Y + neighbor.Position.Y) / 2);
                            e.Graphics.DrawString($"{distance:F2}", font, brush, midPoint);
                        }
                    }
                }
            }

            foreach (var marker in markers)
            {
                marker.OnRender(e.Graphics);
            }
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            int imgWidth = pictureBox1.Image.Width;
            int imgHeight = pictureBox1.Image.Height;
            int pbWidth = pictureBox1.ClientSize.Width;
            int pbHeight = pictureBox1.ClientSize.Height;

            float imgAspect = (float)imgWidth / imgHeight;
            float pbAspect = (float)pbWidth / pbHeight;

            int imgX, imgY, imgDisplayWidth, imgDisplayHeight;

            if (imgAspect > pbAspect)
            {
                imgDisplayWidth = pbWidth;
                imgDisplayHeight = (int)(pbWidth / imgAspect);
                imgX = 0;
                imgY = (pbHeight - imgDisplayHeight) / 2;
            }
            else
            {
                imgDisplayWidth = (int)(pbHeight * imgAspect);
                imgDisplayHeight = pbHeight;
                imgX = (pbWidth - imgDisplayWidth) / 2;
                imgY = 0;
            }

            if (e.X >= imgX && e.X <= imgX + imgDisplayWidth && e.Y >= imgY && e.Y <= imgY + imgDisplayHeight)
            {
                int imgCoordX = (int)((e.X - imgX) * ((float)imgWidth / imgDisplayWidth));
                int imgCoordY = (int)((e.Y - imgY) * ((float)imgHeight / imgDisplayHeight));
                coordinatesLabel.Text = $"X: {imgCoordX}, Y: {imgCoordY}";
            }
            else
            {
                coordinatesLabel.Text = string.Empty;
            }
        }

        private void computeDistanceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!depthfirstSearchToolStripMenuItem.Checked && !breadthToolStripMenuItem.Checked && !bestFirstSearchToolStripMenuItem.Checked)
            {
                MessageBox.Show("Please select a search method (Depth First Search, Breadth First Search, or Best First Search) before computing the distance.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string startChar = startCharTextBox.Text;
            string endChar = endCharTextBox.Text;

            if (string.IsNullOrEmpty(startChar) || string.IsNullOrEmpty(endChar))
            {
                MessageBox.Show("Please enter both start and end characters.");
                return;
            }

            CustomMarker startMarker = markers.FirstOrDefault(m => m.Label == startChar);
            CustomMarker endMarker = markers.FirstOrDefault(m => m.Label == endChar);

            if (startMarker == null || endMarker == null)
            {
                MessageBox.Show("Invalid start or end character.");
                return;
            }

            DrawPathWithDistances(startMarker, endMarker);
        }

        private bool DepthFirstSearch(CustomMarker startMarker, CustomMarker endMarker, List<CustomMarker> path)
        {
            Stack<CustomMarker> stack = new Stack<CustomMarker>();
            HashSet<CustomMarker> visited = new HashSet<CustomMarker>();
            Dictionary<CustomMarker, CustomMarker> cameFrom = new Dictionary<CustomMarker, CustomMarker>();

            stack.Push(startMarker);
            cameFrom[startMarker] = null;

            while (stack.Count > 0)
            {
                CustomMarker current = stack.Pop();
                Console.WriteLine($"Visiting Marker: {current.Label}");

                if (current == endMarker)
                {
                    while (current != null)
                    {
                        path.Insert(0, current);
                        current = cameFrom[current];
                    }
                    return true;
                }

                if (!visited.Contains(current))
                {
                    visited.Add(current);

                    var sortedNeighbors = current.NearestNeighbors.OrderBy(n => n.Position.X).ToList();

                    foreach (var neighbor in sortedNeighbors)
                    {
                        if (!visited.Contains(neighbor))
                        {
                            Console.WriteLine($"Adding Neighbor: {neighbor.Label}");
                            stack.Push(neighbor);
                            if (!cameFrom.ContainsKey(neighbor))
                            {
                                cameFrom[neighbor] = current;
                            }
                        }
                    }
                }
            }

            return false;
        }

        private bool BreadthFirstSearch(CustomMarker startMarker, CustomMarker endMarker, List<CustomMarker> path)
        {
            if (startMarker == null || endMarker == null)
            {
                throw new ArgumentNullException("Start or end marker cannot be null.");
            }

            Queue<CustomMarker> queue = new Queue<CustomMarker>();
            Dictionary<CustomMarker, CustomMarker> cameFrom = new Dictionary<CustomMarker, CustomMarker>();
            HashSet<CustomMarker> visited = new HashSet<CustomMarker>();

            queue.Enqueue(startMarker);
            cameFrom[startMarker] = null;
            visited.Add(startMarker);

            while (queue.Count > 0)
            {
                CustomMarker current = queue.Dequeue();

                if (current == endMarker)
                {
                    while (current != null)
                    {
                        path.Insert(0, current);
                        current = cameFrom[current];
                    }
                    return true;
                }

                foreach (var neighbor in current.NearestNeighbors)
                {
                    if (!visited.Contains(neighbor))
                    {
                        queue.Enqueue(neighbor);
                        visited.Add(neighbor);
                        cameFrom[neighbor] = current;
                    }
                }
            }
            return false;
        }

        private bool BestFirstSearch(CustomMarker startMarker, CustomMarker endMarker, List<CustomMarker> path)
        {
            if (startMarker == null || endMarker == null)
            {
                throw new ArgumentNullException("Start or end marker cannot be null.");
            }

            var visualizer = new BestFirstSearchVisualizer();
            visualizer.Show();

            var priorityQueue = new SortedDictionary<double, Queue<CustomMarker>>();
            Dictionary<CustomMarker, CustomMarker> cameFrom = new Dictionary<CustomMarker, CustomMarker>();
            HashSet<CustomMarker> closedSet = new HashSet<CustomMarker>();

            void Enqueue(CustomMarker item, double priority)
            {
                if (!priorityQueue.ContainsKey(priority))
                {
                    priorityQueue[priority] = new Queue<CustomMarker>();
                }
                priorityQueue[priority].Enqueue(item);
            }

            CustomMarker Dequeue()
            {
                if (priorityQueue.Count == 0)
                {
                    throw new InvalidOperationException("The priority queue is empty.");
                }

                var firstKey = priorityQueue.Keys.First();
                var queue = priorityQueue[firstKey];
                var item = queue.Dequeue();

                if (queue.Count == 0)
                {
                    priorityQueue.Remove(firstKey);
                }

                return item;
            }

            Enqueue(startMarker, 0);
            cameFrom[startMarker] = null;

            while (priorityQueue.Count > 0)
            {
                // Update the OPEN set in the visualizer
                var openSet = priorityQueue
                    .SelectMany(kvp => kvp.Value.Select(node => new KeyValuePair<string, double>(node.Label, kvp.Key)))
                    .ToList();
                visualizer.UpdateOpenSet(openSet);

                CustomMarker current = Dequeue();

                // Add the current node to the CLOSED set
                closedSet.Add(current);

                // Update the CLOSED set in the visualizer
                var closedSetData = closedSet
                    .Select(node => new KeyValuePair<string, string>(node.Label, cameFrom[node]?.Label))
                    .ToList();
                visualizer.UpdateClosedSet(closedSetData);

                if (current == endMarker)
                {
                    while (current != null)
                    {
                        path.Insert(0, current);
                        current = cameFrom[current];
                    }
                    return true;
                }

                foreach (var neighbor in current.NearestNeighbors)
                {
                    // Neighbor is connected, not already visited, and not in the CLOSED set
                    if (!closedSet.Contains(neighbor) && current.NearestNeighbors.Contains(neighbor))
                    {
                        double priority = GetDistance(neighbor.Position, endMarker.Position);
                        Enqueue(neighbor, priority);
                        if (!cameFrom.ContainsKey(neighbor))
                        {
                            cameFrom[neighbor] = current;
                        }
                    }
                }
            }

            return false;
        }

        private void DrawPathWithDistances(CustomMarker startMarker, CustomMarker endMarker)
        {
            List<CustomMarker> path = new List<CustomMarker>();
            bool pathFound = false;

            if (depthfirstSearchToolStripMenuItem.Checked)
            {
                pathFound = DepthFirstSearch(startMarker, endMarker, path);
            }
            else if (breadthToolStripMenuItem.Checked)
            {
                pathFound = BreadthFirstSearch(startMarker, endMarker, path);
            }
            else if (bestFirstSearchToolStripMenuItem.Checked)
            {
                pathFound = BestFirstSearch(startMarker, endMarker, path);
            }

            if (!pathFound)
            {
                MessageBox.Show("No path found.");
                return;
            }

            sequenceOutputTextBox.Text = string.Join("-", path.Select(m => m.Label));

            pictureBox1.Invalidate();
            pictureBox1.Update();

            using (Graphics g = pictureBox1.CreateGraphics())
            {
                Pen redPen = new Pen(Color.Red, 2);
                Font font = new Font("Arial", 10);
                Brush redBrush = Brushes.Red;

                for (int i = 0; i < path.Count - 1; i++)
                {
                    CustomMarker marker1 = path[i];
                    CustomMarker marker2 = path[i + 1];
                    g.DrawLine(redPen, marker1.Position, marker2.Position);

                   
                    if (!depthfirstSearchToolStripMenuItem.Checked && !breadthToolStripMenuItem.Checked && !bestFirstSearchToolStripMenuItem.Checked)
                    {
                        double distance = GetDistance(marker1.Position, marker2.Position);
                        Point midPoint = new Point((marker1.Position.X + marker2.Position.X) / 2, (marker1.Position.Y + marker2.Position.Y) / 2);
                        g.DrawString($"{distance:F2}", font, redBrush, midPoint);
                    }
                }
            }
        }

        private void depthfirstSearchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            depthfirstSearchToolStripMenuItem.Checked = true;
            breadthToolStripMenuItem.Checked = false;
            bestFirstSearchToolStripMenuItem.Checked = false;

            pictureBox1.Invalidate();
        }

        private void breadthToolStripMenuItem_Click(object sender, EventArgs e)
        {
            depthfirstSearchToolStripMenuItem.Checked = false;
            breadthToolStripMenuItem.Checked = true;
            bestFirstSearchToolStripMenuItem.Checked = false;

            pictureBox1.Invalidate();
        }

        private void bestFirstSearchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bestFirstSearchToolStripMenuItem.Checked = true;
            depthfirstSearchToolStripMenuItem.Checked = false;
            breadthToolStripMenuItem.Checked = false;

            pictureBox1.Invalidate();
        }

        private double GetDistance(Point point1, Point point2)
        {
            double dx = point2.X - point1.X;
            double dy = point2.Y - point1.Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }

        private void resetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            markers.Clear();
            currentLabelIndex = 0;
            LoadPredefinedData();
            pictureBox1.Invalidate();
        }
    }

    public class CustomMarker
    {
        public Point Position { get; set; }
        public string Label { get; set; }
        public bool IsBold { get; set; }
        public Color MarkerColor { get; set; }
        public List<CustomMarker> NearestNeighbors { get; set; }

        public CustomMarker(Point p, string label, bool isBold, Color markerColor)
        {
            this.Position = p;
            this.Label = label;
            this.IsBold = isBold;
            this.MarkerColor = markerColor;
            this.NearestNeighbors = new List<CustomMarker>();
        }

        public void OnRender(Graphics g)
        {
            // background circle
            int diameter = 30;
            Rectangle rect = new Rectangle(Position.X - diameter / 2, Position.Y - diameter / 2, diameter, diameter);
            g.FillEllipse(Brushes.Blue, rect);

            // text label centered
            Font font = new Font("Arial", 12, FontStyle.Bold);
            SizeF textSize = g.MeasureString(Label, font);
            PointF textPosition = new PointF(
                Position.X - textSize.Width / 2,
                Position.Y - textSize.Height / 2
            );
            g.DrawString(Label, font, Brushes.White, textPosition);
        }
    }
}