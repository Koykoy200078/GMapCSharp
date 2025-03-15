using GMap.NET;
using GMap.NET.WindowsForms.Markers;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.ToolTips;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Map
{
    public partial class Main : Form
    {
        private PointLatLng currentLocation;
        private GMapOverlay routesOverlay;
        private char currentLabel = 'A';
        private DataTable dataTable;
        private double currentZoom;

        public Main()
        {
            InitializeComponent();
            this.KeyPreview = true; // Ensure the form receives key events
            this.KeyDown += new KeyEventHandler(Main_KeyDown);
        }

        private void Main_Load(object sender, EventArgs e)
        {
            GMap.NET.GMaps.Instance.Mode = GMap.NET.AccessMode.ServerAndCache;
            gMapControl1.Dock = DockStyle.Fill;
            gMapControl1.MapProvider = GMap.NET.MapProviders.GoogleMapProvider.Instance;
            double lat, lon;

            // My Current Location or base location
            lat = 9.312421855653751;
            lon = 123.3031670697597;
            currentLocation = new PointLatLng(lat, lon);
            currentZoom = 17;
            gMapControl1.Position = currentLocation;
            gMapControl1.Zoom = currentZoom;

            GMapOverlay o = new GMapOverlay("markers");
            CustomMarker m = new CustomMarker(currentLocation, "ME", true, 0, 0);

            gMapControl1.Overlays.Add(o);
            o.Markers.Add(m);
            gMapControl1.Invalidate();
            gMapControl1.Update();

            routesOverlay = new GMapOverlay("routes");
            gMapControl1.Overlays.Add(routesOverlay);

            // Initialize and populate the data table
            dataTable = createData();
            dataTable.Rows.Add("1", 9.311754933437783, 123.30271560759176);
            dataTable.Rows.Add("2", 9.312891563368355, 123.30208602879691);
            dataTable.Rows.Add("3", 9.312118827841717, 123.30674372720384);
            dataTable.Rows.Add("4", 9.310055193507296, 123.30416053693382);
            dataTable.Rows.Add("5", 9.31057337739343, 123.30030607016842);

            // Load predefined data
            LoadPredefinedData();
        }

        private void LoadPredefinedData()
        {
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    string id = row["id"].ToString();
                    string lat = row["lat"].ToString();
                    string lon = row["lon"].ToString();

                    double newLat = Convert.ToDouble(lat);
                    double newLon = Convert.ToDouble(lon);

                    GMapOverlay o = new GMapOverlay("markers");
                    CustomMarker m = new CustomMarker(new PointLatLng(newLat, newLon), currentLabel.ToString(), false, 0, 0);

                    gMapControl1.Overlays.Add(o);
                    o.Markers.Add(m);
                    gMapControl1.Invalidate();
                    gMapControl1.Update();

                    currentLabel++;
                }
            }
        }

        private void Main_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.A)
            {
                int x = gMapControl1.Width / 2;
                int y = gMapControl1.Height / 2;
                double lat1, lon1;

                GMapOverlay o = new GMapOverlay("markers");
                gMapControl1.Overlays.Add(o);

                lat1 = gMapControl1.FromLocalToLatLng(x, y).Lat;
                lon1 = gMapControl1.FromLocalToLatLng(x, y).Lng;
                CustomMarker m = new CustomMarker(new PointLatLng(lat1, lon1), currentLabel.ToString(), false, 0, 0);
                o.Markers.Add(m);
                gMapControl1.Invalidate();
                gMapControl1.Update();

                currentLabel++;
            }
        }

        private void gMapControl1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                PointLatLng clickedPoint = gMapControl1.FromLocalToLatLng(e.X, e.Y);
                currentLocation = clickedPoint;
                currentZoom = gMapControl1.Zoom;

                // Remove existing "ME" marker if any
                foreach (var overlay in gMapControl1.Overlays)
                {
                    var markerToRemove = overlay.Markers.OfType<CustomMarker>().FirstOrDefault(marker => marker.Label == "ME");
                    if (markerToRemove != null)
                    {
                        overlay.Markers.Remove(markerToRemove);
                        break;
                    }
                }

                // Add new "ME" marker
                GMapOverlay o = new GMapOverlay("markers");
                CustomMarker meMarker = new CustomMarker(currentLocation, "ME", true, 0, 0);
                gMapControl1.Overlays.Add(o);
                o.Markers.Add(meMarker);
                gMapControl1.Invalidate();
                gMapControl1.Update();

                CustomMarker nearestMarker = null;
                double minDistance = double.MaxValue;

                foreach (var overlay in gMapControl1.Overlays)
                {
                    foreach (var marker in overlay.Markers.OfType<CustomMarker>())
                    {
                        double distance = GetDistance(currentLocation, marker.Position);
                        if (distance < minDistance)
                        {
                            minDistance = distance;
                            nearestMarker = marker;
                        }
                    }
                }

                if (nearestMarker != null)
                {
                    gMapControl1.Position = nearestMarker.Position;
                    gMapControl1.Zoom = currentZoom;
                    DrawRoute(currentLocation, nearestMarker.Position);
                    TagMarkers(nearestMarker.Position);
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                PointLatLng point = gMapControl1.FromLocalToLatLng(e.X, e.Y);
                foreach (var overlay in gMapControl1.Overlays)
                {
                    var markerToRemove = overlay.Markers.OfType<CustomMarker>().FirstOrDefault(marker => marker.Position == point);
                    if (markerToRemove != null)
                    {
                        overlay.Markers.Remove(markerToRemove);
                        gMapControl1.Invalidate();
                        gMapControl1.Update();
                        break;
                    }
                }
            }
        }

        private void TagMarkers(PointLatLng startPoint)
        {
            List<CustomMarker> markers = new List<CustomMarker>();
            foreach (var overlay in gMapControl1.Overlays)
            {
                markers.AddRange(overlay.Markers.OfType<CustomMarker>());
            }

            markers = markers.OrderBy(m => GetDistance(startPoint, m.Position)).ToList();

            char label = 'A';
            double previousDistance = -1;
            foreach (var marker in markers)
            {
                if (marker.Label != "ME")
                {
                    double distance = GetDistance(startPoint, marker.Position);
                    if (Math.Abs(distance - previousDistance) > 0.0001) // Use a small tolerance to handle floating-point precision issues
                    {
                        label++;
                        previousDistance = distance;
                    }
                    marker.Label = label.ToString();
                    marker.IsBold = true;
                    marker.Distance = distance;
                    marker.DistanceMiles = distance * 0.621371; // Convert kilometers to miles
                }
            }

            gMapControl1.Invalidate();
            gMapControl1.Update();
        }

        private void DrawRoute(PointLatLng start, PointLatLng end)
        {
            routesOverlay.Routes.Clear();
            List<PointLatLng> points = new List<PointLatLng> { start, end };
            GMapRoute route = new GMapRoute(points, "route")
            {
                Stroke = new Pen(Color.Red, 2)
            };
            routesOverlay.Routes.Add(route);
            gMapControl1.Invalidate();
            gMapControl1.Update();
        }

        private double GetDistance(PointLatLng point1, PointLatLng point2)
        {
            double lat1 = point1.Lat;
            double lon1 = point1.Lng;
            double lat2 = point2.Lat;
            double lon2 = point2.Lng;

            double dLat = (lat2 - lat1) * (Math.PI / 180.0);
            double dLon = (lon2 - lon1) * (Math.PI / 180.0);

            lat1 = lat1 * (Math.PI / 180.0);
            lat2 = lat2 * (Math.PI / 180.0);

            double a = Math.Pow(Math.Sin(dLat / 2), 2) + Math.Pow(Math.Sin(dLon / 2), 2) * Math.Cos(lat1) * Math.Cos(lat2);
            double c = 2 * Math.Asin(Math.Sqrt(a));
            double radius = 6371; // Radius of Earth in kilometers

            return radius * c;
        }

        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            gMapControl1.Zoom = vScrollBar1.Value;
            currentZoom = gMapControl1.Zoom;
        }

        private DataTable createData()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("id");
            dt.Columns.Add("lat");
            dt.Columns.Add("lon");
            dt.AcceptChanges();
            return dt;
        }
    }

    public class CustomMarker : GMarkerGoogle
    {
        public string Label { get; set; }
        public bool IsBold { get; set; }
        public double Distance { get; set; }
        public double DistanceMiles { get; set; }

        public CustomMarker(PointLatLng p, string label, bool isBold, double distance, double distanceMiles) : base(p, GMarkerGoogleType.none)
        {
            this.Label = label;
            this.IsBold = isBold;
            this.Distance = distance;
            this.DistanceMiles = distanceMiles;
        }

        public override void OnRender(Graphics g)
        {
            //base.OnRender(g);
            Font font = new Font("Arial", 12, FontStyle.Bold);
            g.DrawString(Label, font, Brushes.Red, LocalPosition.X, LocalPosition.Y);
            g.DrawString($"{Distance:F2} km / {DistanceMiles} mi", new Font("Arial", 10), Brushes.Black, LocalPosition.X, LocalPosition.Y + 15);
        }
    }
}
