using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetTopologySuite;
using NetTopologySuite.IO;
using GeoAPI.Geometries;
using NetTopologySuite.LinearReferencing;

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

   }
}
