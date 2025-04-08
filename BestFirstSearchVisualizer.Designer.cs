namespace Map
{
    partial class BestFirstSearchVisualizer
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.openListView = new System.Windows.Forms.ListView();
            this.closedListView = new System.Windows.Forms.ListView();
            this.openLabel = new System.Windows.Forms.Label();
            this.closedLabel = new System.Windows.Forms.Label();
            this.columnHeaderNode = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderHeuristic = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderClosedNode = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderParent = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // openListView
            // 
            this.openListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderNode,
            this.columnHeaderHeuristic});
            this.openListView.FullRowSelect = true;
            this.openListView.GridLines = true;
            this.openListView.Location = new System.Drawing.Point(12, 29);
            this.openListView.Name = "openListView";
            this.openListView.Size = new System.Drawing.Size(300, 290);
            this.openListView.TabIndex = 0;
            this.openListView.UseCompatibleStateImageBehavior = false;
            this.openListView.View = System.Windows.Forms.View.Details;
            // 
            // closedListView
            // 
            this.closedListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderClosedNode,
            this.columnHeaderParent});
            this.closedListView.FullRowSelect = true;
            this.closedListView.GridLines = true;
            this.closedListView.Location = new System.Drawing.Point(330, 29);
            this.closedListView.Name = "closedListView";
            this.closedListView.Size = new System.Drawing.Size(300, 290);
            this.closedListView.TabIndex = 1;
            this.closedListView.UseCompatibleStateImageBehavior = false;
            this.closedListView.View = System.Windows.Forms.View.Details;
            // 
            // openLabel
            // 
            this.openLabel.AutoSize = true;
            this.openLabel.Location = new System.Drawing.Point(12, 9);
            this.openLabel.Name = "openLabel";
            this.openLabel.Size = new System.Drawing.Size(33, 13);
            this.openLabel.TabIndex = 2;
            this.openLabel.Text = "OPEN";
            // 
            // closedLabel
            // 
            this.closedLabel.AutoSize = true;
            this.closedLabel.Location = new System.Drawing.Point(330, 9);
            this.closedLabel.Name = "closedLabel";
            this.closedLabel.Size = new System.Drawing.Size(47, 13);
            this.closedLabel.TabIndex = 3;
            this.closedLabel.Text = "CLOSED";
            // 
            // columnHeaderNode
            // 
            this.columnHeaderNode.Text = "Node";
            this.columnHeaderNode.Width = 150;
            // 
            // columnHeaderHeuristic
            // 
            this.columnHeaderHeuristic.Text = "H(n)";
            this.columnHeaderHeuristic.Width = 150;
            // 
            // columnHeaderClosedNode
            // 
            this.columnHeaderClosedNode.Text = "Node";
            this.columnHeaderClosedNode.Width = 150;
            // 
            // columnHeaderParent
            // 
            this.columnHeaderParent.Text = "Parent";
            this.columnHeaderParent.Width = 150;
            // 
            // BestFirstSearchVisualizer
            // 
            this.ClientSize = new System.Drawing.Size(650, 340);
            this.Controls.Add(this.closedLabel);
            this.Controls.Add(this.openLabel);
            this.Controls.Add(this.closedListView);
            this.Controls.Add(this.openListView);
            this.Name = "BestFirstSearchVisualizer";
            this.Text = "Best First Search Visualizer";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.ListView openListView;
        private System.Windows.Forms.ListView closedListView;
        private System.Windows.Forms.Label openLabel;
        private System.Windows.Forms.Label closedLabel;
        private System.Windows.Forms.ColumnHeader columnHeaderNode;
        private System.Windows.Forms.ColumnHeader columnHeaderHeuristic;
        private System.Windows.Forms.ColumnHeader columnHeaderClosedNode;
        private System.Windows.Forms.ColumnHeader columnHeaderParent;
    }
}