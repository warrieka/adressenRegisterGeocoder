using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.IO;

namespace adressenRegisterGeocoder
{
   class csvReader
   {
      /// <summary>read csv into as List of List of string </summary>
      /// <param name="csvPath">the path to the csv-file</param>
      /// <param name="enc">the ecoding of file</param>
      /// <param name="sep">the character separating the values</param>
      /// <returns>A 2-d list containg the values</returns>
      public static List<List<string>> read(string csvPath, Encoding enc = null, string sep = ";")
      {
         string[] csvRows;
         if (enc != null) { csvRows = System.IO.File.ReadAllLines(csvPath, enc); }
         else { csvRows = System.IO.File.ReadAllLines(csvPath); }
         
         string field = "";
         bool quoteStarted = false;
         var fieldsList = new List<List<string>>();

         foreach (string csvRow in csvRows)
         {
            var fields = new List<string>();

            for (int i = 0; i < csvRow.Length; i++)
            {
               var character = csvRow[i].ToString();
               
               if ( String.Compare(character, "\"") == 0)
               {
                  quoteStarted = !quoteStarted;
               }

               if (String.Compare(character, sep) == 0 && !quoteStarted)
               {
                  fields.Add(field);
                  field = "";
               }
               else if (String.Compare(character, "\"") != 0)
               {
                  field += character;
               }
            }
            if (!string.IsNullOrEmpty(field))
            {
               fields.Add(field);
               field = "";
            }

            fieldsList.Add(fields);
         }
         return fieldsList;
      }

      /// <summary>read csv into as List of List of string </summary>
      /// <param name="csvPath">the path to the csv-file</param>
      /// <param name="sep">the character separating the values</param>
      /// <returns>A 2-d list containg the values</returns>
      public static List<List<string>> read(string csvPath, string sep = ";" )
      {
         return read(csvPath, null , sep);
      }

      /// <summary>Read a csv file into a datatable </summary>
      /// <param name="csvPath">the path to the csv-file</param>
      /// <param name="separator">the character separating the values, can be "COMMA", "PUNTCOMMA", "SPATIE" or "TAB", 
      /// for any sepator string the the input is used</param>
      /// <returns>a datable containing the values form the file</returns>
      public static DataTable loadCSV2datatable(string csvPath, string separator, int maxRows, System.Text.Encoding codex)
      {
         FileInfo csv = new FileInfo(csvPath);
         string sep;
         DataTable tbl = new DataTable();

         System.Text.Encoding textEncoding = System.Text.Encoding.Default;
         if (codex != null) textEncoding = codex;

         if (!csv.Exists)
            throw new Exception("Deze csv-file bestaat niet: " + csv.Name);
         if (separator == "" || separator == null)
            throw new Exception("Deze separator is niet toegelaten");

         switch (separator)
         {
            case "Comma":
               sep = ",";
               break;
            case "Puntcomma":
               sep = ";";
               break;
            case "Spatie":
               sep = " ";
               break;
            case "Tab":
               sep = "/t";
               break;
            default:
               sep = separator;
               break;
         }

         using (Microsoft.VisualBasic.FileIO.TextFieldParser csvParser =
                         new Microsoft.VisualBasic.FileIO.TextFieldParser(csv.FullName, textEncoding, true))
         {
            csvParser.Delimiters = new string[] { sep };
            csvParser.HasFieldsEnclosedInQuotes = true;
            csvParser.TextFieldType = Microsoft.VisualBasic.FileIO.FieldType.Delimited;
            csvParser.TrimWhiteSpace = !(separator == "TAB" || separator == "SPATIE");

            string[] colNames = csvParser.ReadFields();
            string[] row;
            int counter = 0;

            foreach (string colName in colNames)
            {
               tbl.Columns.Add(colName);
            }
            while (!csvParser.EndOfData)
            {
               try
               {
                  if (counter >= maxRows)
                  {
                     return tbl;
                  }
                  counter++;

                  row = csvParser.ReadFields();

                  if (tbl.Columns.Count != row.Count())
                  {
                     throw new Exception("Niet alle rijen hebben hetzelfde aantal kolommen, op eerste lijn: " +
                      tbl.Rows.Count.ToString() + " gevonden: " + row.Count() + " op lijn: " + string.Join(sep, row));
                  }
                  tbl.Rows.Add(row);
               }
               catch (Microsoft.VisualBasic.FileIO.MalformedLineException ex)
               {
                  throw new Exception("CSV is kan niet worden gelezen, het heeft niet de correcte vorm: " + csvParser.ErrorLine, ex);
               }
            }
         }
         return tbl;
      }
      /// <summary>Read a csv file into a datatable </summary>
      /// <param name="csvPath">the path to the csv-file</param>
      /// <param name="separator">the character separating the values, can be "COMMA", "PUNTCOMMA", "SPATIE" or "TAB", 
      /// for any sepator string the the input is used</param>
      /// <returns>a datable containing the values form the file</returns>
      public static DataTable loadCSV2datatable(string csvPath, string separator, int maxRows)
      {
         return loadCSV2datatable(csvPath, separator, maxRows, null);
      }
      /// <summary>Read a csv file into a datatable </summary>
      /// <param name="csvPath">the path to the csv-file</param>
      /// <param name="separator">the character separating the values, can be "COMMA", "PUNTCOMMA", "SPATIE" or "TAB", 
      /// for any sepator string the the input is used</param>
      /// <returns>a datable containing the values form the file</returns>
      public static DataTable loadCSV2datatable(string csvPath, string separator)
      {
         return loadCSV2datatable(csvPath, separator, 500, null);
      }


   }
}
