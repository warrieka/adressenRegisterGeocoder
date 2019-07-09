using System;
using System.Collections.Generic;
using GeoAPI.Geometries;
using GeoAPI.CoordinateSystems;
using GeoAPI.CoordinateSystems.Transformations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjNet.CoordinateSystems;
using ProjNet.CoordinateSystems.Transformations;
using ProjNet.Converters.WellKnownText;

namespace adressenRegisterGeocoder
{
   class crsFactory
   {
      private const string lam72wkt = @"PROJCS[""Belge 1972 / Belgian Lambert 72"",GEOGCS[""Belge 1972"",DATUM[""Reseau_National_Belge_1972"",SPHEROID[""International 1924"",6378388,297,AUTHORITY[""EPSG"",""7022""]],TOWGS84[-106.869,52.2978,-103.724,0.3366,-0.457,1.8422,-1.2747],AUTHORITY[""EPSG"",""6313""]],PRIMEM[""Greenwich"",0,AUTHORITY[""EPSG"",""8901""]],UNIT[""degree"",0.0174532925199433,AUTHORITY[""EPSG"",""9122""]],AUTHORITY[""EPSG"",""4313""]],PROJECTION[""Lambert_Conformal_Conic_2SP""],PARAMETER[""standard_parallel_1"",51.16666723333333],PARAMETER[""standard_parallel_2"",49.8333339],PARAMETER[""latitude_of_origin"",90],PARAMETER[""central_meridian"",4.367486666666666],PARAMETER[""false_easting"",150000.013],PARAMETER[""false_northing"",5400088.438],UNIT[""metre"",1,AUTHORITY[""EPSG"",""9001""]],AXIS[""X"",EAST],AXIS[""Y"",NORTH],AUTHORITY[""EPSG"",""31370""]]";

      private CoordinateSystemFactory cf = new CoordinateSystemFactory();
      private CoordinateTransformationFactory ct = new CoordinateTransformationFactory();
      public ICoordinateSystem merc = ProjectedCoordinateSystem.WebMercator;
      public ICoordinateSystem wgs = GeographicCoordinateSystem.WGS84;
      public ICoordinateSystem lam72;

      public crsFactory() 
      {
          lam72 = cf.CreateFromWkt(lam72wkt);
      }
      public Coordinate transformMerc2lam72(double x, double y)
      {
         var trans = ct.CreateFromCoordinateSystems(merc, lam72);
         var xy = trans.MathTransform.Transform(new double[] { x, y });
         return new GeoAPI.Geometries.Coordinate(xy[0], xy[1]);
      }
      public Coordinate transformMerc2lam72(Coordinate pt)
      {
         return transformMerc2lam72(pt.X, pt.Y);
      }
      public Coordinate transformlam72Merc(double x, double y)
      {
         var trans = ct.CreateFromCoordinateSystems(lam72, merc);
         var xy = trans.MathTransform.Transform(new double[] { x, y });
         return new GeoAPI.Geometries.Coordinate(xy[0], xy[1]);
      }
      public Coordinate transformMlam72Merc(Coordinate pt)
      {
         return transformMerc2lam72(pt.X, pt.Y);
      }


   }
}
