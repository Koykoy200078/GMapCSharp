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
        private DataTable dataTable;
        private double currentZoom;
        private int currentLabelIndex = 0;

        public Main()
        {
            InitializeComponent();
            this.KeyPreview = true;
            this.KeyDown += new KeyEventHandler(Main_KeyDown);
        }

        private void Main_Load(object sender, EventArgs e)
        {
            GMap.NET.GMaps.Instance.Mode = GMap.NET.AccessMode.ServerAndCache;
            gMapControl1.Dock = DockStyle.Fill;
            gMapControl1.MapProvider = GMap.NET.MapProviders.GoogleMapProvider.Instance;
            double lat, lon;

            // base location
            lat = 9.312421855653751;
            lon = 123.3031670697597;
            currentLocation = new PointLatLng(lat, lon);
            currentZoom = 17;
            gMapControl1.Position = currentLocation;
            gMapControl1.Zoom = currentZoom;

            GMapOverlay o = new GMapOverlay("markers");
            CustomMarker m = new CustomMarker(currentLocation, "ME", true, 0, 0, Color.Blue);

            gMapControl1.Overlays.Add(o);
            o.Markers.Add(m);
            gMapControl1.Invalidate();
            gMapControl1.Update();

            routesOverlay = new GMapOverlay("routes");
            gMapControl1.Overlays.Add(routesOverlay);

            dataTable = createData();
            // Capitol
            dataTable.Rows.Add(1, 9.312890082352915, 123.30204284842539);
            // Chowking atbang norsu
            dataTable.Rows.Add(2, 9.31240470295932, 123.30443396578242);
            // CSIT Department
            dataTable.Rows.Add(3, 9.312421855653751, 123.3031670697597);
            // Perpetual Church
            dataTable.Rows.Add(4, 9.311154310990707, 123.30318733469414);
            // NORSU Amphitheater
            dataTable.Rows.Add(5, 9.31175480896306, 123.30271839711514);
            // Silliman University
            dataTable.Rows.Add(6, 9.310792780359494, 123.30551112932926);
            // Tree Hive Guest House
            dataTable.Rows.Add(7, 9.311965856407708, 123.30448271671895);
            // Lamberto L. Macias Sports and Cultural Centre
            dataTable.Rows.Add(8, 9.311961561006411, 123.30071728724853);
            // Negros Oriental Legislative Building
            dataTable.Rows.Add(9, 9.313148521548118, 123.301766593118);
            // Oval
            dataTable.Rows.Add(10, 9.313551858126656, 123.3002320993571);
            // Chinese Cemetery
            dataTable.Rows.Add(11, 9.314633290396607, 123.3004201386834);
            // NOHS
            dataTable.Rows.Add(12, 9.314404347183334, 123.3021496361891);
            // Cang's
            dataTable.Rows.Add(13, 9.315522430533528, 123.30231446078363);
            // Hashtag
            dataTable.Rows.Add(14, 9.310575415055684, 123.3003008835393);
            // INC
            dataTable.Rows.Add(15, 9.312412318006196, 123.29808886246813);
            // Dgte Memorial Park
            dataTable.Rows.Add(16, 9.311873205642943, 123.29544108432437);
            // Grub Hub Grill
            dataTable.Rows.Add(17, 9.313542469741405, 123.29828089100849);
            // Cathedral
            dataTable.Rows.Add(18, 9.305306991742452, 123.30719544671067);
            // Quezon Park
            dataTable.Rows.Add(19, 9.305591471774553, 123.3082035965478);
            // Boulevard
            dataTable.Rows.Add(20, 9.306889053100818, 123.31031221763563);
            // Lee Plaza
            dataTable.Rows.Add(21, 9.307822661822064, 123.30709573958372);
            // Police Station
            dataTable.Rows.Add(22, 9.307064887868925, 123.30452920428395);
            // Asian College
            dataTable.Rows.Add(23, 9.30697765379174, 123.3019552832711);
            // National Museum
            dataTable.Rows.Add(24, 9.30519219749348, 123.30935207493307);
            // Octagon
            dataTable.Rows.Add(25, 9.30409185625435, 123.30776045376166);
            // Unitop
            dataTable.Rows.Add(26, 9.306672220467592, 123.30750564665992);
            // Port of Dumaguete
            dataTable.Rows.Add(27, 9.312721846023248, 123.31090676753956);
            // Silliman University High School
            dataTable.Rows.Add(28, 9.314267432141783, 123.30790078231084);
            // Silliman University Medical Center
            dataTable.Rows.Add(29, 9.316286750827999, 123.30390511152788);
            // Tourism Office
            dataTable.Rows.Add(30, 9.321192193492783, 123.30193312613278);

            LoadPredefinedData();
            TagMarkers(currentLocation);
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
                    CustomMarker m = new CustomMarker(new PointLatLng(newLat, newLon), GetLabelFromIndex(currentLabelIndex), false, 0, 0, Color.Red);

                    gMapControl1.Overlays.Add(o);
                    o.Markers.Add(m);
                    gMapControl1.Invalidate();
                    gMapControl1.Update();

                    currentLabelIndex++;
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
                CustomMarker m = new CustomMarker(new PointLatLng(lat1, lon1), GetLabelFromIndex(currentLabelIndex), false, 0, 0, Color.Red);
                o.Markers.Add(m);
                gMapControl1.Invalidate();
                gMapControl1.Update();

                currentLabelIndex++;
            }
        }

        private void gMapControl1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                PointLatLng clickedPoint = gMapControl1.FromLocalToLatLng(e.X, e.Y);
                currentLocation = clickedPoint;
                currentZoom = gMapControl1.Zoom;

                foreach (var overlay in gMapControl1.Overlays)
                {
                    var markerToRemove = overlay.Markers.OfType<CustomMarker>().FirstOrDefault(marker => marker.Label == "ME");
                    if (markerToRemove != null)
                    {
                        overlay.Markers.Remove(markerToRemove);
                        break;
                    }
                }

                // ME marker
                GMapOverlay o = new GMapOverlay("markers");
                CustomMarker meMarker = new CustomMarker(currentLocation, "ME", true, 0, 0, Color.Blue);
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

            int labelIndex = 0;
            double previousDistance = -1;
            foreach (var marker in markers)
            {
                if (marker.Label != "ME")
                {
                    double distance = GetDistance(startPoint, marker.Position);
                    if (Math.Abs(distance - previousDistance) > 0.0001)
                    {
                        previousDistance = distance;
                    }
                    marker.Label = GetLabelFromIndex(labelIndex);
                    labelIndex++;
                    marker.IsBold = true;
                    marker.Distance = distance;
                    marker.DistanceMiles = distance * 0.621371;
                }
            }

            gMapControl1.Invalidate();
            gMapControl1.Update();
        }

        private string GetLabelFromIndex(int index)
        {
            string label = string.Empty;
            while (index >= 0)
            {
                label = (char)('A' + index % 26) + label;
                index = index / 26 - 1;
            }
            return label;
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
            double radius = 6371;

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
        public Color MarkerColor { get; set; }

        public CustomMarker(PointLatLng p, string label, bool isBold, double distance, double distanceMiles, Color markerColor) : base(p, GMarkerGoogleType.none)
        {
            this.Label = label;
            this.IsBold = isBold;
            this.Distance = distance;
            this.DistanceMiles = distanceMiles;
            this.MarkerColor = markerColor;
        }

        public override void OnRender(Graphics g)
        {
            // background circle
            int diameter = 30;
            Rectangle rect = new Rectangle(LocalPosition.X - diameter / 2, LocalPosition.Y - diameter / 2, diameter, diameter);
            g.FillEllipse(new SolidBrush(MarkerColor), rect);

            // text label centered
            Font font = new Font("Arial", 12, FontStyle.Bold);
            SizeF textSize = g.MeasureString(Label, font);
            PointF textPosition = new PointF(
                LocalPosition.X - textSize.Width / 2,
                LocalPosition.Y - textSize.Height / 2
            );
            g.DrawString(Label, font, Brushes.White, textPosition);

            // distance information
            if (Label != "ME")
            {
                string distanceText = $"{Distance:F2} km / {DistanceMiles:F2} mi";
                Font distanceFont = new Font("Arial", 10);
                SizeF distanceTextSize = g.MeasureString(distanceText, distanceFont);
                PointF distanceTextPosition = new PointF(
                    LocalPosition.X - distanceTextSize.Width / 2,
                    LocalPosition.Y + diameter / 2
                );
                g.DrawString(distanceText, distanceFont, Brushes.Black, distanceTextPosition);
            }
        }
    }
}
