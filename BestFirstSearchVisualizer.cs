using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Map
{
    public partial class BestFirstSearchVisualizer : Form
    {
        public BestFirstSearchVisualizer()
        {
            InitializeComponent();
        }

        public void UpdateOpenSet(List<KeyValuePair<string, double>> openSet)
        {
            openListView.Items.Clear();
            foreach (var item in openSet)
            {
                var listViewItem = new ListViewItem(item.Key); // Node
                listViewItem.SubItems.Add(item.Value.ToString("F2")); // Heuristic (formatted to 2 decimal places)
                openListView.Items.Add(listViewItem);
            }
        }

        public void UpdateClosedSet(List<KeyValuePair<string, string>> closedSet)
        {
            closedListView.Items.Clear();
            foreach (var item in closedSet)
            {
                var listViewItem = new ListViewItem(item.Key); // Node
                listViewItem.SubItems.Add(item.Value ?? "None"); // Parent (or "None" if null)
                closedListView.Items.Add(listViewItem);
            }
        }

    }
}