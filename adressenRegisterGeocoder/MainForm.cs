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
using System.Linq;
using System.Text.RegularExpressions;
using BruTile.Web;
using BruTile;
using NetTopologySuite;
using NetTopologySuite.IO;
using vlaanderen.informatie;
using SharpMap.Layers;
using SharpMap.Data.Providers;

namespace adressenRegisterGeocoder
{
   public partial class MainForm : Form
   {
       
      crsFactory crs;
      geoUtils gu;
      GeoAPI.Geometries.IGeometryFactory gf;
      GeometryProvider geoprov;

      int maxRows = Int32.MaxValue;
      int straatCol;
      int huisnrCol;
      int gemeenteCol;
      int pcCol;
 
      VectorLayer vlay;
      DataGridViewRow[] rows2validate;

      bool dataloaded = false;
      bool prikLoc = false;
      bool isBuzy = false;
      bool canceling = false;

      public MainForm()
      {
         InitializeComponent();
         initMap();
         sepCbx.SelectedIndex = 0;
         encodingCbx.SelectedIndex = 0;

         //extra events
         this.closeBtn.Click += (sender, e) => this.Close();
         this.FormClosing += (sender, e) => {
            var window = MessageBox.Show("Ben u zeker dat wilt afsluiten?", "Afsluiten", MessageBoxButtons.YesNo);
            e.Cancel = (window == DialogResult.No);
         };
         this.cancelValidationBtn.Click += (sender, e) => canceling = true;
      }

      #region save
      private void saveBtn_Click(object sender, EventArgs e)
      {
         if (saveShapeDlg.ShowDialog(this) == System.Windows.Forms.DialogResult.Cancel) return;

         bool result = false; 
         var ext = Path.GetExtension(saveShapeDlg.FileName);
         if (ext == ".shp")
         {
            result = gu.saveShpFromDatagrid(saveShapeDlg.FileName, csvDataGrid);
         }
         else if (ext == ".csv")
         {
            result = gu.saveCsvFromDatagrid(saveShapeDlg.FileName, csvDataGrid);
         }
         if (result) 
            MessageBox.Show(Path.GetFileName(saveShapeDlg.FileName) + " is opgeslagen.");
         else
            MessageBox.Show(Path.GetFileName(saveShapeDlg.FileName) + " werd niet opgeslagen, mogelijk zijn er geen records met een geometrie");
         
      }
      #endregion

      #region map interaction;
      private void initMap()
      {
         crs = new crsFactory();
         //create geometry collection
         GeoAPI.GeometryServiceProvider.Instance = new NtsGeometryServices();
         gf = GeoAPI.GeometryServiceProvider.Instance.CreateGeometryFactory();
         gu = new geoUtils(gf);

         SharpMap.Data.FeatureDataTable feats = new SharpMap.Data.FeatureDataTable();
         geoprov = new GeometryProvider(feats);
         geoprov.SRID = 3857;

         //create layers
         vlay = new SharpMap.Layers.VectorLayer("Points");
         vlay.DataSource = geoprov;

         var grb = new SharpMap.Layers.WmsLayer("GRB", "https://geoservices.informatievlaanderen.be/raadpleegdiensten/GRB-basiskaart/wms");
         grb.SetImageFormat("image/png");
         grb.AddLayer("GRB_BSK");
         grb.SRID = 3857; 
         grb.Enabled = true;

         var lufo = new SharpMap.Layers.WmsLayer("GRB", "https://inspire.informatievlaanderen.be/raadpleegdiensten/oi/wms");
         lufo.SetImageFormat("image/jpeg");
         lufo.AddLayer("OI.OrthoimageCoverage.OMW");
         lufo.SRID = 3857;
         lufo.Enabled = true;

          //var grb = new TileAsyncLayer(new TmsTileSource("http://tile.informatievlaanderen.be/ws/raadpleegdiensten/tms/1.0.0/grb_bsk@GoogleMapsVL",
            //                   new BruTile.PreDefined.SphericalMercatorWorldSchema()), "GRB") { Enabled = true };
        // var lufo = new TileAsyncLayer(new TmsTileSource("http://tile.informatievlaanderen.be/ws/raadpleegdiensten/tms/1.0.0/omwrgbmrvl@GoogleMapsVL",
          //                                 new BruTile.PreDefined.SphericalMercatorWorldSchema()), "Luchtfoto") { Enabled = false,  };

         var antTile = new BruTile.PreDefined.SphericalMercatorInvertedWorldSchema();
         antTile.Resolutions.Add(new Resolution() { Id = "19", UnitsPerPixel = 0.298582141647617 });
         var ant = new TileAsyncLayer(new ArcGisTileSource("https://tiles.arcgis.com/tiles/1KSVSmnHT2Lw9ea6/arcgis/rest/services/basemap_stadsplan_v5/MapServer",
                                        antTile ), "Antwerpen") { Enabled = false };
         var osm = new TileAsyncLayer(new BruTile.Web.OsmTileSource(), "OSM") { Enabled = false };
         //Add layers to map
         mapBox.Map.Layers.Add(vlay);
         mapBox.Map.BackgroundLayer.Add(grb);
         mapBox.Map.BackgroundLayer.Add(lufo);
         mapBox.Map.BackgroundLayer.Add(ant);
         mapBox.Map.BackgroundLayer.Add(osm);
         basemapCbx.SelectedIndex = 0;

         mapBox.Map.SRID = 3857;
         mapBox.ActiveTool = SharpMap.Forms.MapBox.Tools.Pan;
         mapBox.Map.ZoomToBox(new GeoAPI.Geometries.Envelope(458235.9, 515348.5, 6689330.3, 6646648.0));
         mapBox.Refresh();
      }

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
      private void basemapCbx_SelectedIndexChanged(object sender, EventArgs e)
      {
         switch (basemapCbx.SelectedIndex)
         {
            case 0:
               mapBox.Map.BackgroundLayer[0].Enabled = true;
               mapBox.Map.BackgroundLayer[1].Enabled = false;
               mapBox.Map.BackgroundLayer[2].Enabled = false;
               mapBox.Map.BackgroundLayer[3].Enabled = false;
               break;
            case 1:
               mapBox.Map.BackgroundLayer[0].Enabled = false;
               mapBox.Map.BackgroundLayer[1].Enabled = true;
               mapBox.Map.BackgroundLayer[2].Enabled = false;
               mapBox.Map.BackgroundLayer[3].Enabled = false;
               break;
            case 2:
               mapBox.Map.BackgroundLayer[0].Enabled = false;
               mapBox.Map.BackgroundLayer[1].Enabled = false;
               mapBox.Map.BackgroundLayer[2].Enabled = true;
               mapBox.Map.BackgroundLayer[3].Enabled = false;
               break;
            case 3:
               mapBox.Map.BackgroundLayer[0].Enabled = false;
               mapBox.Map.BackgroundLayer[1].Enabled = false;
               mapBox.Map.BackgroundLayer[2].Enabled = false;
               mapBox.Map.BackgroundLayer[3].Enabled = true;
               break;
            default:
               break;
         }
         mapBox.Refresh();
      }

      #endregion

      #region load CSV
      private void openBtn_Click(object sender, EventArgs e)
      {
         if (openTableDlg.ShowDialog(this) == System.Windows.Forms.DialogResult.Cancel) return;
         var csv = openTableDlg.FileName;
         var sep = sepCbx.Text == "Ander:" ? otherSepBox.Text : sepCbx.Text;
         csvPathTxt.Text = csv;
         DataTable csvTable;

         csvDataGrid.Columns.Clear();
         dataloaded = false;

         if (encodingCbx.Text == "UTF-8") {
            csvTable = csvReader.loadCSV2datatable(csv, sep, maxRows, Encoding.UTF8);
         }
         else if (encodingCbx.Text == "ANSI") {
            csvTable = csvReader.loadCSV2datatable(csv, sep, maxRows, Encoding.ASCII);
         }
         else {
            csvTable = csvReader.loadCSV2datatable(csv, sep, maxRows);
         }

         straatColCbx.Items.Clear();
         straatColCbx.Items.Add("");
         HuisNrCbx.Items.Clear();
         HuisNrCbx.Items.Add("");
         postcodeColCbx.Items.Clear();
         postcodeColCbx.Items.Add("");
         gemeenteColCbx.Items.Clear();
         gemeenteColCbx.Items.Add("");

         //set extra columns
         var validatedRow = new DataGridViewTextBoxColumn() { HeaderText = "validAdres", Name = "validAdres", Width = 120, ReadOnly = true}; //DataGridViewComboBoxColumn
         var infoRow = new DataGridViewTextBoxColumn() { HeaderText = "info", Name = "info", Width = 120, ReadOnly = true};
         var idRow = new DataGridViewTextBoxColumn() { HeaderText = "adresID", Name = "adresID", Width = 60, ReadOnly = true };
         var xRow = new DataGridViewTextBoxColumn() { HeaderText = "X", Name = "X", Width = 60, ReadOnly = true};
         xRow.DefaultCellStyle.Format = "F0";
         var yRow = new DataGridViewTextBoxColumn() { HeaderText = "Y", Name = "Y", Width = 60 , ReadOnly = true};
         yRow.DefaultCellStyle.Format = "F0";

         foreach (DataColumn column in csvTable.Columns)
         {
            csvDataGrid.Columns.Add(column.ColumnName, column.ColumnName);
            csvDataGrid.Columns[column.ColumnName].SortMode = DataGridViewColumnSortMode.Automatic;
            straatColCbx.Items.Add(column.ColumnName);
            HuisNrCbx.Items.Add(column.ColumnName);
            postcodeColCbx.Items.Add(column.ColumnName);
            gemeenteColCbx.Items.Add(column.ColumnName);
         }
         foreach (DataRow row in csvTable.Rows)
         {
            csvDataGrid.Rows.Add(row.ItemArray);
         }
         csvDataGrid.Columns.Add(validatedRow);
         csvDataGrid.Columns.Add(infoRow);
         csvDataGrid.Columns.Add(idRow);
         csvDataGrid.Columns.Add(xRow);
         csvDataGrid.Columns.Add(yRow);
         dataloaded = true;
      }   
      #endregion 

      #region validation 

      private void validateAllBtn_Click(object sender, EventArgs e)
      {
         if (csvDataGrid.RowCount == 0) return;

         if (straatColCbx.Text == "")
         {
            MessageBox.Show(this, "Stel eerst de adres kolommen in.", "Waarschuwing");
            return;
         }
         if (isBuzy == false)
         {
            setCols();
            rows2validate = new DataGridViewRow[csvDataGrid.Rows.Count];
            csvDataGrid.Rows.CopyTo(rows2validate, 0);
            doGeocode(rows2validate);
         }
      }

      private void validateSelBtn_Click(object sender, EventArgs e)
      {
         if (csvDataGrid.SelectedRows.Count == 0) return;

         if (straatColCbx.Text == "")
         {
            MessageBox.Show(this, "Stel eerst de adres kolommen in.", "Waarschuwing");
            return;
         }
         
         if (isBuzy == false)
         {
            setCols();
            rows2validate = new DataGridViewRow[csvDataGrid.SelectedRows.Count];
            csvDataGrid.SelectedRows.CopyTo(rows2validate, 0);
            csvDataGrid.ClearSelection();
            doGeocode(rows2validate);
         }
      }

      async void doGeocode(DataGridViewRow[] rows)
      {
         toggleInteraction(false);

         progressBar.Maximum = rows.Count();
         progressBar.Value = 0;

         foreach (DataGridViewRow row in rows)
         {
            if (canceling)
            {
               canceling = false;
               break;
            }

            var inAdres = new adres();
            inAdres.street = straatCol >= 0 ? (string)row.Cells[straatCol].Value : "";
            inAdres.housnr = huisnrCol >= 0 ? (string)row.Cells[huisnrCol].Value : "";
            inAdres.municapalname = gemeenteCol >= 0 ? (string)row.Cells[gemeenteCol].Value : "";
            inAdres.pc = pcCol >= 0 ? (string)row.Cells[pcCol].Value : "";

            if (inAdres.municapalname.Trim() == "" && inAdres.pc.Trim() == "") inAdres.municapalname = "Antwerpen";

            var dVal = new dataValidator();
            var adresses = await dVal.findAdres(inAdres , nearNrChk.Checked);

            var adr = dVal.adresValidation(adresses, randomRadio.Checked, centerRadio.Checked);
            row.Cells["validAdres"].Value = adr.validadres;
            row.Cells["info"].Value = adr.info;
            row.Cells["adresID"].Value = adr.adresID;
            row.Cells["X"].Value = adr.x;
            row.Cells["Y"].Value = adr.y;

            foreach (DataGridViewCell cel in row.Cells) cel.Style.BackColor = adr.colorCode;

            progressBar.Value++;
         }
         progressBar.Value = 0;
         toggleInteraction(true);
      }
      #endregion

      #region set values
      private void toggleInteraction(bool active)
      {
         isBuzy = !active; 
         csvBox.Enabled = active;
         adresSettingsBox.Enabled = active;
         manualLocBtn.Enabled = active;
         validateSelBtn.Enabled = active;
         validateAllBtn.Enabled = active;
      }

      private void setCols()
      {
         straatCol = straatColCbx.SelectedIndex - 1;
         huisnrCol = HuisNrCbx.SelectedIndex - 1;
         gemeenteCol = gemeenteColCbx.SelectedIndex - 1;
         pcCol = postcodeColCbx.SelectedIndex - 1;
      }
      #endregion

      #region gridinteraction
      private void zoom2selBtn_Click(object sender, EventArgs e)
      {
         var minx = double.MaxValue;
         var maxx = 0.0;
         var miny = double.MaxValue;
         var maxy = 0.0;

         foreach (DataGridViewRow row in csvDataGrid.SelectedRows)
         {
            var hasx = row.Cells["X"].Value is double; 
            var hasy = row.Cells["Y"].Value is double;
            if(hasx && hasy){
               var X = (double)row.Cells["X"].Value;
               var Y = (double)row.Cells["Y"].Value;
               if (X < minx) minx = X;
               if (X > maxx) maxx = X;
               if (Y < miny) miny = Y;
               if (Y > maxy) maxy = Y;
            }
         }
         var top = crs.transformlam72Merc(maxx + 100, maxy + 100);
         var bottom = crs.transformlam72Merc(minx - 100, miny - 100);
         var bbox = new GeoAPI.Geometries.Envelope(top, bottom);
         mapBox.Map.ZoomToBox(bbox);
         mapBox.Refresh();
      }

      private void csvDataGrid_SelectionChanged(object sender, EventArgs e)
      {
         if ( !dataloaded || csvDataGrid.SelectedRows.Count == 0) return;
         geoprov.Geometries.Clear();

         foreach (DataGridViewRow row in csvDataGrid.SelectedRows)
         {
            var hasx = row.Cells["X"].Value is double;
            var hasy = row.Cells["Y"].Value is double;
            if (hasx && hasy)
            {
               var X = (double)row.Cells["X"].Value;
               var Y = (double)row.Cells["Y"].Value;
               var pt = crs.transformlam72Merc(X, Y);
               geoprov.Geometries.Add(gf.CreatePoint(pt));
            }
         }
         mapBox.Refresh();

      }

      private void manualLocBtn_Click(object sender, EventArgs e)
      {
         if (csvDataGrid.SelectedRows.Count == 1) prikLoc = true;
         else if (csvDataGrid.SelectedRows.Count > 1) MessageBox.Show("Meer dan 1 rij geselecteerd: Selecteer 1 rij in de tabel.", "Meer dan 1 rij geselecteerd");
         else MessageBox.Show("Geen rij geselecteerd: Selecteer een rij in de tabel.", "Geen rij geselecteerd");
      }

      private void mapBox_MouseClick(object sender, MouseEventArgs e)
      {
         if (!prikLoc) return;

         var pt = mapBox.Map.ImageToWorld(e.Location);
         geoprov.Geometries.Add(gf.CreatePoint(pt));
         var ptlam = crs.transformMerc2lam72(pt);

         var row = csvDataGrid.SelectedRows[0];
         row.Cells["info"].Value = "0 | Manueel geplaatst";
         row.Cells["X"].Value = ptlam.X;
         row.Cells["Y"].Value = ptlam.Y;

         Color clr = ColorTranslator.FromHtml("#D0F5A9");
         foreach (DataGridViewCell cel in row.Cells)
         {
            cel.Style.BackColor = clr;
         }
         prikLoc = false;
      }
      #endregion

   }
}
