using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using BruTile.Web;
using BruTile;
using NetTopologySuite;
using NetTopologySuite.IO;
using vlaanderen.informatie;
using SharpMap.Layers;

namespace adressenRegisterGeocoder
{
   public partial class MainForm : Form
   {
      crsFactory crs;
      GeoAPI.Geometries.IGeometryFactory gf;
      SharpMap.Data.Providers.GeometryProvider geoprov;
      AdresMatchClient adresMatch;

      public MainForm()
      {
         InitializeComponent();

         crs = new crsFactory();

         //create geometry collection
         GeoAPI.GeometryServiceProvider.Instance = new NtsGeometryServices();
         gf = GeoAPI.GeometryServiceProvider.Instance.CreateGeometryFactory();
         adresMatch = new AdresMatchClient();
         
         SharpMap.Data.FeatureDataTable feats = new SharpMap.Data.FeatureDataTable();
         geoprov = new SharpMap.Data.Providers.GeometryProvider(feats);
         geoprov.SRID = 3857;

         //create layers
         SharpMap.Layers.VectorLayer vlay = new SharpMap.Layers.VectorLayer("Points");
         vlay.DataSource = geoprov;
         
         var grb = new TileAsyncLayer(new TmsTileSource("http://tile.informatievlaanderen.be/ws/raadpleegdiensten/tms/1.0.0/grb_bsk@GoogleMapsVL",
                                           new BruTile.PreDefined.SphericalMercatorWorldSchema()), "GRB") { Enabled = true };
         var lufo = new TileAsyncLayer(new TmsTileSource("http://tile.informatievlaanderen.be/ws/raadpleegdiensten/tms/1.0.0/omwrgbmrvl@GoogleMapsVL",
                                           new BruTile.PreDefined.SphericalMercatorWorldSchema()), "Luchtfoto") { Enabled = false };
         var ant = new TileAsyncLayer(new ArcGisTileSource("https://tiles.arcgis.com/tiles/1KSVSmnHT2Lw9ea6/arcgis/rest/services/basemap_stadsplan_v5/MapServer",
                                           new BruTile.PreDefined.SphericalMercatorInvertedWorldSchema()), "Antwerpen") { Enabled = false };
         //Add layers to map
         mapBox.Map.Layers.Add(vlay);
         mapBox.Map.BackgroundLayer.Add(grb);
         mapBox.Map.BackgroundLayer.Add(lufo);
         mapBox.Map.BackgroundLayer.Add(ant);
         basemapCbx.SelectedIndex = 0;

         mapBox.Map.SRID = 3857;
         mapBox.ActiveTool = SharpMap.Forms.MapBox.Tools.Pan;
         mapBox.Map.ZoomToBox( new GeoAPI.Geometries.Envelope(458235.9, 515348.5, 6689330.3, 6646648.0));
         mapBox.Refresh();
      }

      private void saveBtn_Click(object sender, EventArgs e)
      {
         if (saveShapeDlg.ShowDialog(this) == System.Windows.Forms.DialogResult.Cancel) return;
         saveShp(saveShapeDlg.FileName);
      }

      //privates
      private AdresMatchItem searchAdres(string straat, string huisnr, string gemeente, string postcode)
      {
         if (gemeente.Trim() == "" & postcode.Trim() == "") gemeente = "Antwerpen";

         var adresses = adresMatch.Get("1", gemeente, adresMatchRequest_straatnaam: straat,
                                 adresMatchRequest_postcode: postcode, adresMatchRequest_huisnummer: huisnr);

         if (adresses.AdresMatches.Count > 0 & adresses.AdresMatches[0].VolledigAdres != null)
         {
            return adresses.AdresMatches[0];
         }
         return null;
      }

      private void saveShp(string shpFile)
      {
         int count = 0;
         var features = new List<NetTopologySuite.Features.Feature>();

         foreach (var geom in geoprov.Geometries)
         {
            var xy = crs.transformMerc2lam72(geom.Coordinate);

            var attr = new NetTopologySuite.Features.AttributesTable();
            attr.AddAttribute("gisid", count);
            var feat = new NetTopologySuite.Features.Feature(gf.CreatePoint(xy), attr);
            features.Add(feat);

            count++;
         }
         var writer = new ShapefileDataWriter(shpFile) { Header = ShapefileDataWriter.GetHeader(features[0], features.Count) };
         writer.Write(features);

         string prj = Path.GetDirectoryName(shpFile) + @"\" + Path.GetFileNameWithoutExtension(shpFile) + ".prj";
         File.WriteAllText(prj, crs.lam72.WKT);
      }

      private void ZoekBtn_Click(object sender, EventArgs e)
      {
         var huisnr = huisnrTxt.Text;
         var straat = straatTxt.Text;
         var gemeente = gemeenteTxt.Text;
         var postcode = postcodeTxt.Text;

         var adres = searchAdres(straat, huisnr, gemeente, postcode);
         if (adres != null)
         {
            var coord = crs.transformMlam72Merc(
               adres.AdresPositie.Point1.Coordinates[0], adres.AdresPositie.Point1.Coordinates[1]);
            var geom = gf.CreatePoint(coord);

            geoprov.Geometries.Add(geom);
            mapBox.Map.ZoomToBox( new GeoAPI.Geometries.Envelope(coord.X - 120, coord.X + 120, coord.Y - 120, coord.Y + 120));
            mapBox.Refresh();

            msgLbl.Text = String.Format("{0} ({1:0.00})", adres.VolledigAdres.GeografischeNaam.Spelling, adres.Score);

            
         }
      }

      #region map interaction;
      private void zoomINBtn_Click(object sender, EventArgs e)
      {
         mapBox.Map.Zoom = mapBox.Map.Zoom / 2;
         mapBox.Refresh();
      }

      private void zoomOUTBtn_Click(object sender, EventArgs e)
      {
         mapBox.Map.Zoom = mapBox.Map.Zoom * 2;
         mapBox.Refresh();
      }

      private void fullExtendBtn_Click(object sender, EventArgs e)
      {
         mapBox.Map.ZoomToBox(new GeoAPI.Geometries.Envelope(458235.9, 515348.5, 6689330.3, 6646648.0));
         mapBox.Refresh();
      }

      private void mapBox_MouseClick(object sender, MouseEventArgs e)
      {
         //TODO
         //var p = mapBox.Map.ImageToWorld(e.Location);
         //var geom = gf.CreatePoint(p);
         //TODO
      }

      private void basemapCbx_SelectedIndexChanged(object sender, EventArgs e)
      {
         switch (basemapCbx.SelectedIndex)
         {
            case 0:
               mapBox.Map.BackgroundLayer[0].Enabled = true;
               mapBox.Map.BackgroundLayer[1].Enabled = false;
               mapBox.Map.BackgroundLayer[2].Enabled = false;
               break;
            case 1:
               mapBox.Map.BackgroundLayer[0].Enabled = false;
               mapBox.Map.BackgroundLayer[1].Enabled = true;
               mapBox.Map.BackgroundLayer[2].Enabled = false;
               break;
            case 2:
               mapBox.Map.BackgroundLayer[0].Enabled = false;
               mapBox.Map.BackgroundLayer[1].Enabled = false;
               mapBox.Map.BackgroundLayer[2].Enabled = true;
               break;
            default:
               break;
         }
         mapBox.Refresh();
      }

      #endregion
   }
}
