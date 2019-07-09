using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using vlaanderen.informatie;

namespace adressenRegisterGeocoder
{
   class dataValidator
   {
      private AdresMatchClient adresMatch;

      public dataValidator()
      {
         this.adresMatch = new AdresMatchClient();
      }

      public async Task<AdresMatchItem[]> findAdres(adres inputAdres, bool nearNr = false)
      {
         List<AdresMatchItem> adresmatches;
         try
         {
            var adresses = await adresMatch.GetAsync("1", inputAdres.municapalname, adresMatchRequest_straatnaam: inputAdres.street,
                                 adresMatchRequest_postcode: inputAdres.pc, adresMatchRequest_huisnummer: inputAdres.housnr);
            adresmatches = adresses.AdresMatches.ToList();
            int numsFound = (from n in adresmatches where n.VolledigAdres != null select n).Count();
            int streetsFound = (from n in adresmatches where n.VolledigAdres == null && n.Straatnaam != null select n).Count();
            
            if (nearNr && numsFound == 0 && streetsFound >= 1)
            {
               int[] testRange = {-2, 2, -4, 4 };
               int numPart;
               if (int.TryParse(Regex.Match(inputAdres.housnr, @"^\d+").Value, out numPart))
               {
                  foreach (int i in testRange)
                  {
                     inputAdres.housnr = Convert.ToString(numPart + i);

                     adresses = await adresMatch.GetAsync("1", inputAdres.municapalname, adresMatchRequest_straatnaam: inputAdres.street,
                                                adresMatchRequest_postcode: inputAdres.pc, adresMatchRequest_huisnummer: inputAdres.housnr);
                     numsFound = (from n in adresses.AdresMatches where n.VolledigAdres != null select n).Count();
                     if (numsFound >= 1)
                     {
                        adresmatches = adresses.AdresMatches.ToList();
                        break;
                     }
                  }
               }
            }
         }
         catch (SwaggerException)
         {
            return null;
         }
         return adresmatches.ToArray();
      }

      public adres adresValidation(AdresMatchItem[] adresses, bool randomXY = false, bool centerXY = false )
      {
         var adr = new adres() {info="0 | Geen overeenkomstige adressen gevonden."};
         var gu = new geoUtils(GeoAPI.GeometryServiceProvider.Instance.CreateGeometryFactory());
         
         // adres found
        if ((from n in adresses where n.VolledigAdres != null select n).Count() >= 1)
         {
            var adresMatch = adresses.Where(n => n.VolledigAdres != null).First(); //TODO multiple results
            adr.x = adresMatch.AdresPositie.Point1.Coordinates[0];
            adr.y = adresMatch.AdresPositie.Point1.Coordinates[1];
            adr.adresID = adresMatch.Identificator.Id;
            adr.validadres = adresMatch.VolledigAdres.GeografischeNaam.Spelling;
            adr.info = (adresMatch.Score != null ? ((double)adresMatch.Score).ToString("000.0") : "") +
               " | " + adresMatch.PositieGeometrieMethode + " | " + adresMatch.PositieSpecificatie;

            adr.colorCode = ColorTranslator.FromHtml("#D0F5A9");
         }
         //only street found
         else if ((from n in adresses where n.VolledigAdres == null && n.Straatnaam != null select n).Count() >= 1)
         {
            var straat = adresses.Where(n => n.Straatnaam != null).First();
            string straatNaam = straat.Straatnaam.Straatnaam.GeografischeNaam.Spelling;
            string gemeente = straat.Gemeente.Gemeentenaam.GeografischeNaam.Spelling;
            adr.adresID = "https://data.vlaanderen.be/id/straatnaam/" + straat.Straatnaam.ObjectId; 

            adr.validadres = straatNaam + ", " + gemeente;
            adr.info = (straat.Score != null ? ((double)straat.Score).ToString("000.0") : "0") + " | Huisnummer niet gevonden | Straatnaam";
            if (randomXY)
            {
               int roadId = Convert.ToInt32(straat.Straatnaam.ObjectId);
               var geom = gu.getRoadByID(roadId);
               if (geom != null)
               {
                  var xy = gu.randomPointOnLine(geom);
                  adr.x = Math.Round(xy.X, 2);
                  adr.y = Math.Round(xy.Y, 2);
               }
            }
            else if (centerXY)
            {
               int roadId = Convert.ToInt32(straat.Straatnaam.ObjectId);
               var geom = gu.getRoadByID(roadId);
               adr.x = Math.Round(geom.Centroid.X, 2);
               adr.y = Math.Round(geom.Centroid.Y, 2);
            }
            adr.colorCode = ColorTranslator.FromHtml("#ffcc99");

         }
         return adr;
      }
   }
}
