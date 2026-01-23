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

                // Initial connections - Create bidirectional k-nearest neighbor graph
                // First, find each node's k nearest neighbors
                foreach (var marker in markers)
                {
                    marker.NearestNeighbors = markers
                        .Where(m => m != marker)
                        .OrderBy(m => GetDistance(marker.Position, m.Position))
                        .Take(3)
                        .ToList();
                }

                // Then, make all connections bidirectional
                foreach (var marker in markers)
                {
                    foreach (var neighbor in marker.NearestNeighbors.ToList()) // ToList() to avoid modification during iteration
                    {
                        // If neighbor doesn't have marker in its list, add it to ensure bidirectional connection
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

                // Debug: Print graph statistics
                Console.WriteLine("\n=== GRAPH CONSTRUCTION COMPLETE ===");
                Console.WriteLine($"Total nodes: {markers.Count}");
                Console.WriteLine($"Nodes with connections:");
                foreach (var marker in markers)
                {
                    Console.WriteLine($"  Node {marker.Label}: {marker.NearestNeighbors.Count} neighbors -> {string.Join(", ", marker.NearestNeighbors.Select(n => n.Label))}");
                }
                
                // Check for isolated nodes
                var isolatedNodes = markers.Where(m => m.NearestNeighbors.Count == 0).ToList();
                if (isolatedNodes.Any())
                {
                    Console.WriteLine($"\nWARNING: {isolatedNodes.Count} isolated nodes found: {string.Join(", ", isolatedNodes.Select(n => n.Label))}");
                }
                Console.WriteLine("====================================\n");

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

                // Track drawn edges to avoid duplicates (since graph is bidirectional)
                HashSet<(CustomMarker, CustomMarker)> drawnEdges = new HashSet<(CustomMarker, CustomMarker)>();

                foreach (var marker in markers)
                {
                    foreach (var neighbor in marker.NearestNeighbors)
                    {
                        // Only draw each edge once (check both directions)
                        if (!drawnEdges.Contains((marker, neighbor)) && !drawnEdges.Contains((neighbor, marker)))
                        {
                            e.Graphics.DrawLine(pen, marker.Position, neighbor.Position);
                            drawnEdges.Add((marker, neighbor));

                            if (!depthfirstSearchToolStripMenuItem.Checked)
                            {
                                double distance = GetDistance(marker.Position, neighbor.Position);
                                Point midPoint = new Point((marker.Position.X + neighbor.Position.X) / 2, (marker.Position.Y + neighbor.Position.Y) / 2);
                                e.Graphics.DrawString($"{distance:F2}", font, brush, midPoint);
                            }
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
            if (!depthfirstSearchToolStripMenuItem.Checked &&
    !breadthToolStripMenuItem.Checked &&
    !bestFirstSearchToolStripMenuItem.Checked &&
    !aSearchToolStripMenuItem.Checked &&
    !hillClimbingSearchToolStripMenuItem.Checked &&
    !greedySearchToolStripMenuItem.Checked)
            {
                MessageBox.Show("Please select a search method (Depth First Search, Breadth First Search, Best First Search, A* Search, Hill Climbing Search, or Greedy Search) before computing the distance.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

            // Debug: Log connectivity information
            Console.WriteLine($"\n=== PATHFINDING DEBUG ===");
            Console.WriteLine($"Start: {startMarker.Label} (Neighbors: {string.Join(", ", startMarker.NearestNeighbors.Select(n => n.Label))})");
            Console.WriteLine($"End: {endMarker.Label} (Neighbors: {string.Join(", ", endMarker.NearestNeighbors.Select(n => n.Label))})");
            Console.WriteLine($"Total nodes: {markers.Count}");
            Console.WriteLine($"Total connections: {markers.Sum(m => m.NearestNeighbors.Count) / 2}");
            Console.WriteLine("=========================\n");

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
                
                if (visited.Contains(current))
                    continue;
                    
                visited.Add(current);
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

                var sortedNeighbors = current.NearestNeighbors.OrderBy(n => n.Position.X).ToList();

                foreach (var neighbor in sortedNeighbors)
                {
                    if (!visited.Contains(neighbor) && !cameFrom.ContainsKey(neighbor))
                    {
                        Console.WriteLine($"Adding Neighbor: {neighbor.Label}");
                        stack.Push(neighbor);
                        cameFrom[neighbor] = current;
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
            
            Console.WriteLine($"BFS: Starting from {startMarker.Label}, searching for {endMarker.Label}");

            while (queue.Count > 0)
            {
                CustomMarker current = queue.Dequeue();
                Console.WriteLine($"BFS: Visiting {current.Label}, Neighbors: {string.Join(", ", current.NearestNeighbors.Select(n => n.Label))}");

                if (current == endMarker)
                {
                    Console.WriteLine($"BFS: Found path to {endMarker.Label}!");
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
                        Console.WriteLine($"BFS: Enqueueing {neighbor.Label} from {current.Label}");
                        queue.Enqueue(neighbor);
                        visited.Add(neighbor);
                        cameFrom[neighbor] = current;
                    }
                }
            }
            Console.WriteLine($"BFS: No path found. Visited {visited.Count} nodes.");
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

                // Skip if already processed (can be in queue multiple times)
                if (closedSet.Contains(current))
                    continue;

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
                    // Check if neighbor is not in closed set and connection is valid
                    if (!closedSet.Contains(neighbor) && !cameFrom.ContainsKey(neighbor))
                    {
                        double priority = GetDistance(neighbor.Position, endMarker.Position);
                        Enqueue(neighbor, priority);
                        cameFrom[neighbor] = current;
                    }
                }
            }

            return false;
        }

        private bool AStarSearch(CustomMarker startMarker, CustomMarker endMarker, List<CustomMarker> path)
        {
            var openSet = new SortedDictionary<double, Queue<CustomMarker>>();
            var gScore = new Dictionary<CustomMarker, double>();
            var fScore = new Dictionary<CustomMarker, double>();
            var cameFrom = new Dictionary<CustomMarker, CustomMarker>();
            var closedSet = new HashSet<CustomMarker>();

            void Enqueue(CustomMarker node, double priority)
            {
                if (!openSet.ContainsKey(priority))
                    openSet[priority] = new Queue<CustomMarker>();
                openSet[priority].Enqueue(node);
            }

            CustomMarker Dequeue()
            {
                var firstKey = openSet.Keys.First();
                var queue = openSet[firstKey];
                var node = queue.Dequeue();
                if (queue.Count == 0)
                    openSet.Remove(firstKey);
                return node;
            }

            foreach (var marker in markers)
            {
                gScore[marker] = double.PositiveInfinity;
                fScore[marker] = double.PositiveInfinity;
            }
            gScore[startMarker] = 0;
            fScore[startMarker] = GetDistance(startMarker.Position, endMarker.Position);

            Enqueue(startMarker, fScore[startMarker]);
            cameFrom[startMarker] = null;

            while (openSet.Count > 0)
            {
                CustomMarker current = Dequeue();

                if (current == endMarker)
                {
                    while (current != null)
                    {
                        path.Insert(0, current);
                        current = cameFrom[current];
                    }
                    return true;
                }

                closedSet.Add(current);

                foreach (var neighbor in current.NearestNeighbors)
                {
                    if (closedSet.Contains(neighbor))
                        continue;

                    double tentativeGScore = gScore[current] + GetDistance(current.Position, neighbor.Position);

                    if (tentativeGScore < gScore[neighbor])
                    {
                        cameFrom[neighbor] = current;
                        gScore[neighbor] = tentativeGScore;
                        fScore[neighbor] = tentativeGScore + GetDistance(neighbor.Position, endMarker.Position);

                        // Only enqueue if not already in openSet
                        bool inOpenSet = openSet.Values.Any(q => q.Contains(neighbor));
                        if (!inOpenSet)
                            Enqueue(neighbor, fScore[neighbor]);
                    }
                }
            }

            return false;
        }

        private bool HillClimbingSearch(CustomMarker startMarker, CustomMarker endMarker, List<CustomMarker> path)
        {
            // Hill Climbing with Random Restart - tries multiple times if stuck at local optimum
            int maxRestarts = 5;
            Random random = new Random();
            
            for (int restart = 0; restart < maxRestarts; restart++)
            {
                path.Clear();
                var current = startMarker;
                var visited = new HashSet<CustomMarker>();
                path.Add(current);
                visited.Add(current);
                
                while (current != endMarker)
                {
                    // Select the neighbor closest to the goal (steepest ascent/descent)
                    var neighbors = current.NearestNeighbors
                        .Where(n => !visited.Contains(n))
                        .OrderBy(n => GetDistance(n.Position, endMarker.Position))
                        .ToList();
                    
                    if (neighbors.Count == 0)
                    {
                        // Dead end - no unvisited neighbors
                        break;
                    }
                    
                    var best = neighbors.First();
                    double currentDist = GetDistance(current.Position, endMarker.Position);
                    double bestDist = GetDistance(best.Position, endMarker.Position);
                    
                    if (bestDist >= currentDist)
                    {
                        // Stuck at local optimum - try sideways move with probability
                        if (restart < maxRestarts - 1 && neighbors.Count > 1)
                        {
                            // Allow sideways/plateau moves on non-final attempts
                            var sidewaysNeighbor = neighbors.FirstOrDefault(n => 
                                Math.Abs(GetDistance(n.Position, endMarker.Position) - currentDist) < 50);
                            if (sidewaysNeighbor != null)
                            {
                                current = sidewaysNeighbor;
                                path.Add(current);
                                visited.Add(current);
                                continue;
                            }
                        }
                        break; // No improvement possible
                    }
                    
                    current = best;
                    path.Add(current);
                    visited.Add(current);
                }
                
                if (current == endMarker)
                {
                    return true; // Found path!
                }
            }
            
            return false; // Failed after all restarts
        }

        private bool GreedySearch(CustomMarker startMarker, CustomMarker endMarker, List<CustomMarker> path)
        {
            // Greedy Best-First Search using priority queue (h(n) only, no g(n))
            // Unlike A*, it only considers heuristic distance to goal
            // Unlike simple Hill Climbing, it maintains a frontier and can backtrack
            
            var priorityQueue = new SortedDictionary<double, Queue<CustomMarker>>();
            var cameFrom = new Dictionary<CustomMarker, CustomMarker>();
            var visited = new HashSet<CustomMarker>();
            
            void Enqueue(CustomMarker node, double priority)
            {
                if (!priorityQueue.ContainsKey(priority))
                    priorityQueue[priority] = new Queue<CustomMarker>();
                priorityQueue[priority].Enqueue(node);
            }
            
            CustomMarker Dequeue()
            {
                var firstKey = priorityQueue.Keys.First();
                var queue = priorityQueue[firstKey];
                var node = queue.Dequeue();
                if (queue.Count == 0)
                    priorityQueue.Remove(firstKey);
                return node;
            }
            
            // h(n) = straight-line distance to goal
            double Heuristic(CustomMarker node) => GetDistance(node.Position, endMarker.Position);
            
            Enqueue(startMarker, Heuristic(startMarker));
            cameFrom[startMarker] = null;
            
            while (priorityQueue.Count > 0)
            {
                var current = Dequeue();
                
                if (visited.Contains(current))
                    continue;
                    
                visited.Add(current);
                
                if (current == endMarker)
                {
                    // Reconstruct path
                    while (current != null)
                    {
                        path.Insert(0, current);
                        current = cameFrom[current];
                    }
                    return true;
                }
                
                foreach (var neighbor in current.NearestNeighbors)
                {
                    if (!visited.Contains(neighbor) && !cameFrom.ContainsKey(neighbor))
                    {
                        cameFrom[neighbor] = current;
                        Enqueue(neighbor, Heuristic(neighbor));
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
            else if (aSearchToolStripMenuItem.Checked)
            {
                pathFound = AStarSearch(startMarker, endMarker, path);
            }
            else if (hillClimbingSearchToolStripMenuItem.Checked)
            {
                pathFound = HillClimbingSearch(startMarker, endMarker, path);
            }
            else if (greedySearchToolStripMenuItem.Checked)
            {
                pathFound = GreedySearch(startMarker, endMarker, path);
            }

            if (!pathFound)
            {
                string algorithm = depthfirstSearchToolStripMenuItem.Checked ? "DFS" :
                                 breadthToolStripMenuItem.Checked ? "BFS" :
                                 bestFirstSearchToolStripMenuItem.Checked ? "Best First" :
                                 aSearchToolStripMenuItem.Checked ? "A*" :
                                 hillClimbingSearchToolStripMenuItem.Checked ? "Hill Climbing" : "Greedy";
                MessageBox.Show($"No path found using {algorithm}.\n\nDebug Info:\nStart: {startMarker.Label} (Neighbors: {startMarker.NearestNeighbors.Count})\nEnd: {endMarker.Label} (Neighbors: {endMarker.NearestNeighbors.Count})\n\nCheck the console output for detailed search trace.", "Path Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

                    // Always show distance on the path for informed search algorithms
                    if (bestFirstSearchToolStripMenuItem.Checked || aSearchToolStripMenuItem.Checked || hillClimbingSearchToolStripMenuItem.Checked || greedySearchToolStripMenuItem.Checked)
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
            aSearchToolStripMenuItem.Checked = false;
            hillClimbingSearchToolStripMenuItem.Checked = false;
            greedySearchToolStripMenuItem.Checked = false; 


            pictureBox1.Invalidate();
        }

        private void breadthToolStripMenuItem_Click(object sender, EventArgs e)
        {
            depthfirstSearchToolStripMenuItem.Checked = false;
            breadthToolStripMenuItem.Checked = true;
            bestFirstSearchToolStripMenuItem.Checked = false;
            aSearchToolStripMenuItem.Checked = false;
            hillClimbingSearchToolStripMenuItem.Checked = false;
            greedySearchToolStripMenuItem.Checked = false;

            pictureBox1.Invalidate();
        }

        private void bestFirstSearchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bestFirstSearchToolStripMenuItem.Checked = true;
            depthfirstSearchToolStripMenuItem.Checked = false;
            breadthToolStripMenuItem.Checked = false;
            aSearchToolStripMenuItem.Checked = false;
            hillClimbingSearchToolStripMenuItem.Checked = false;
            greedySearchToolStripMenuItem.Checked = false;

            pictureBox1.Invalidate();
        }

        private void aSearchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            depthfirstSearchToolStripMenuItem.Checked = false;
            breadthToolStripMenuItem.Checked = false;
            bestFirstSearchToolStripMenuItem.Checked = false;
            aSearchToolStripMenuItem.Checked = true;
            hillClimbingSearchToolStripMenuItem.Checked = false;
            greedySearchToolStripMenuItem.Checked = false;

            pictureBox1.Invalidate();
        }

        private void hillClimbingSearchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            depthfirstSearchToolStripMenuItem.Checked = false;
            breadthToolStripMenuItem.Checked = false;
            bestFirstSearchToolStripMenuItem.Checked = false;
            aSearchToolStripMenuItem.Checked = false;
            hillClimbingSearchToolStripMenuItem.Checked = true;
            greedySearchToolStripMenuItem.Checked = false;

            pictureBox1.Invalidate();
        }

        private void greedySearchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            depthfirstSearchToolStripMenuItem.Checked = false;
            breadthToolStripMenuItem.Checked = false;
            bestFirstSearchToolStripMenuItem.Checked = false;
            aSearchToolStripMenuItem.Checked = false;
            hillClimbingSearchToolStripMenuItem.Checked = false;
            greedySearchToolStripMenuItem.Checked = true;

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