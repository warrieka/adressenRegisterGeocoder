using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using vlaanderen.informatie;

namespace adressenRegisterGeocoder
{
   class dataValidator
   {
      public string inputStreet;
      public string inputNr;
      public string inputPC;
      public string inputMunicipality;

      public IEnumerable<AdresMatchItem> adresmatches;
      public SwaggerException errors;

      public adres outAdres;

      private geoUtils gu;
      private AdresMatchClient adresMatch;

      public dataValidator()
      {
         gu = new geoUtils( GeoAPI.GeometryServiceProvider.Instance.CreateGeometryFactory() );
         adresMatch = new AdresMatchClient();
      }

      public IEnumerable<AdresMatchItem> findAdres(bool nearNr = false)
      {
         try
         {
            var adresses = adresMatch.Get("1", inputMunicipality, adresMatchRequest_straatnaam: inputStreet,
                                 adresMatchRequest_postcode: inputPC, adresMatchRequest_huisnummer: inputNr);
            adresmatches = adresses.AdresMatches;
            int numsFound = (from n in adresmatches where n.VolledigAdres != null select n).Count();
            int streetsFound = (from n in adresmatches where n.VolledigAdres == null && n.Straatnaam != null select n).Count();
            
            if (nearNr && numsFound == 0 && streetsFound >= 1)
            {
               int[] testRange = {-2, 2, -4, 4 };
               int numPart;
               if (int.TryParse(Regex.Match(inputNr, @"^\d+").Value, out numPart))
               {
                  foreach (int i in testRange)
                  {
                     inputNr = Convert.ToString(numPart + i);

                     adresses = adresMatch.Get("1", inputMunicipality, adresMatchRequest_straatnaam: inputStreet,
                                                    adresMatchRequest_postcode: inputPC, adresMatchRequest_huisnummer: inputNr);
                     numsFound = (from n in adresses.AdresMatches where n.VolledigAdres != null select n).Count();
                     if (numsFound >= 1)
                     {
                        adresmatches = adresses.AdresMatches;
                        break;
                     }
                  }
               }
            }

            errors = null;
         }
         catch (SwaggerException erro)
         {
            adresmatches = null;
            errors = erro;
         }
         return adresmatches;
      }

      public adres adreValidation(bool randomXY = false, bool centerXY = false)
      {
         var adr = new adres() {info="0 | Geen overeenkomstige adressen gevonden."};

         // if erro occurs
         if (adresmatches == null && errors != null)
         {
            adr.info = "0 | " + errors.Response;
         }
         
         // adres found
         else if ((from n in adresmatches where n.VolledigAdres != null select n).Count() >= 1)
         {
            var adresMatch = adresmatches.Where(n => n.VolledigAdres != null).First(); //TODO multiple results
            adr.x = adresMatch.AdresPositie.Point1.Coordinates[0];
            adr.y = adresMatch.AdresPositie.Point1.Coordinates[1];
            adr.validadres = adresMatch.VolledigAdres.GeografischeNaam.Spelling;
            adr.info = (adresMatch.Score != null ? ((double)adresMatch.Score).ToString("000.0") : "") +
               " | " + adresMatch.PositieGeometrieMethode + " | " + adresMatch.PositieSpecificatie;

            adr.colorCode = ColorTranslator.FromHtml("#D0F5A9");
         }
         //only street found
         else if ((from n in adresmatches where n.VolledigAdres == null && n.Straatnaam != null select n).Count() >= 1)
         {
            var straat = adresmatches.Where(n => n.Straatnaam != null).First();
            string straatNaam = straat.Straatnaam.Straatnaam.GeografischeNaam.Spelling;
            string gemeente = straat.Gemeente.Gemeentenaam.GeografischeNaam.Spelling;

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
         outAdres = adr;
         return adr;
      }
   }
}
