using GeoAPI.Geometries;
using NetTopologySuite.IO;
using NetTopologySuite.LinearReferencing;
using System;
using System.Text;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace adressenRegisterGeocoder
{
   class geoUtils
   {
      IGeometryFactory gf;
      ShapefileDataReader wr;
      DbaseFileHeader wrheader;

      public geoUtils(IGeometryFactory geomFac = null, string wrPath = "data/wr.shp")
      {
         gf = geomFac != null ? geomFac : GeoAPI.GeometryServiceProvider.Instance.CreateGeometryFactory();
         wr = new ShapefileDataReader(wrPath, gf);
         wrheader = wr.DbaseHeader;   
      }

      public IMultiLineString getRoadByID(int roadid)
      {
         wr.Reset();
         while (wr.Read())
         {
            var id = Convert.ToInt32(wr.GetValue(0));
            if (id == roadid) 
            {
               if (wr.Geometry.OgcGeometryType == OgcGeometryType.LineString)
               {
                  var lns = new List<ILineString>();
                  lns.Add((ILineString)wr.Geometry);
                  return gf.CreateMultiLineString(lns.ToArray());
               }
               else if (wr.Geometry.OgcGeometryType == OgcGeometryType.MultiLineString)
               {
                  return (IMultiLineString)wr.Geometry;
               }
               else return null;
            }
         }
         return null;
      }

      public Coordinate randomPointOnLine(IMultiLineString lines)
      {
         var rnd = new Random();
 
         //pick random part of the road, change depending on length
         double rndPercentage = rnd.NextDouble(); //between 0 - 1
         var line = (ILineString)lines[0];
         double cumulative = 0.0;
         for (int i = 0; i < lines.Count; i++)
         {
            line = (ILineString)lines[i];
            cumulative += line.Length / lines.Length;
            if (rndPercentage < cumulative) {
               break; 
            }         
         }
         //random coordinate on part
         var start = line.GetCoordinateN(0);
         var end = line.GetCoordinateN(line.NumPoints - 1);
         return LinearLocation.PointAlongSegmentByFraction(start, end, rnd.NextDouble() );
      }

      public List<String> postcodesByRoadId(int roadid)
      {
         wr.Reset();
         while (wr.Read())
         {
            var id = Convert.ToInt32(wr.GetValue(0));
            if (id == roadid)
            {
               String pcs = Convert.ToString(wr.GetValue(2));
               return pcs.Split(',').ToList();
            }
         }
         return new List<string>(); //if nothing found return empty list
      }

      public bool saveShpFromDatagrid(string shpFile, System.Windows.Forms.DataGridView dgrid)
      {
         //see: https://dominoc925.blogspot.com/2013/04/using-nettopologysuite-to-read-and.html 
         var crs = new crsFactory();
         var features = new List<NetTopologySuite.Features.Feature>();
         foreach (System.Windows.Forms.DataGridViewRow row in dgrid.Rows)
         {
            double X; double Y;
            if (row.Cells["X"].Value is double && row.Cells["Y"].Value is double)
            {
               X = (double)row.Cells["X"].Value;
               Y = (double)row.Cells["Y"].Value;
            }
            else continue;

            var geom = gf.CreatePoint(new GeoAPI.Geometries.Coordinate(X, Y));
            var attr = new NetTopologySuite.Features.AttributesTable();
            var headerNamesCLean = new List<string>();

            for (int i = 0; i < row.Cells.Count; i++)
            {
               //if (row.Cells[i].Value == null) continue;
               var val = row.Cells[i].Value.ToString();
               var name = launderShpName(dgrid.Columns[i].HeaderText, headerNamesCLean);
               headerNamesCLean.Add(name);
               attr.AddAttribute(name, val);
            }
            var feat = new NetTopologySuite.Features.Feature(geom, attr);
            features.Add(feat);
         }
         if (features.Count == 0) return false;

         var outFileNoExt = Path.GetDirectoryName(shpFile) + @"\" + Path.GetFileNameWithoutExtension(shpFile);

         var writer = new ShapefileDataWriter(outFileNoExt) { Header = ShapefileDataWriter.GetHeader(features[0], features.Count) };
         writer.Write(features);
         File.WriteAllText(outFileNoExt + ".prj", crs.lam72.WKT);
         return true;
      }

      public bool saveCsvFromDatagrid(string flName, System.Windows.Forms.DataGridView dgrid)
      {
         var sb = new StringBuilder();
         var headers = dgrid.Columns.Cast<System.Windows.Forms.DataGridViewColumn>();
         sb.AppendLine(string.Join(";", headers.Select(column => "\"" + column.HeaderText + "\"").ToArray()));

         foreach (System.Windows.Forms.DataGridViewRow row in dgrid.Rows)
         {
            var cells = row.Cells.Cast<System.Windows.Forms.DataGridViewCell>();
            sb.AppendLine(string.Join(";", cells.Select(cell => "\"" + cell.Value + "\"").ToArray()));
         }
         var utf8bytes = Encoding.UTF8.GetBytes(sb.ToString());
         var win1252Bytes = Encoding.Convert(Encoding.UTF8, Encoding.GetEncoding("windows-1252"), utf8bytes);
         File.WriteAllBytes(flName, win1252Bytes);

         return true;
      }

      public string launderShpName(string inCellName, List<string> existingNames)
      {
         var outCellName = Regex.Replace(inCellName, @"[^a-zA-Z0-9 -]", "");
         outCellName = outCellName.Replace(@" ", @"_");
         outCellName = Regex.IsMatch(outCellName, @"^\d") ? "c" + outCellName : outCellName;
         outCellName = outCellName.Length > 10 ? outCellName.Substring(0, 10) : outCellName;
         if (existingNames.Contains(outCellName))
         {
            outCellName = outCellName.Length > 8 ? outCellName.Substring(0, 8) : outCellName;
            outCellName = outCellName.Substring(0, 8) + existingNames.Count.ToString("00"); //let asume no more then 100 fields
         }
         return outCellName;
      }

   }
}
