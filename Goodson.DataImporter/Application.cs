using Goodson.Database;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Goodson.DataImporter
{
    class Application
    {
        static int Main(string[] args)
        {
            Console.WriteLine($"Goodson Dataimporter Version: {Assembly.GetEntryAssembly().GetName().Version}");

            // TODO: Code Position: #1
            var importDataPath = args[0];
            if (!File.Exists(importDataPath))
            {
                Console.WriteLine("CSV-Datei ist nicht vorhanden");
                return 1;
            }

            

            // Instantiate a new Database Handler
            var dataBase = new SQLiteDatabaseHandler();
            bool ok = dataBase.InitDatabase(); // Will create a fresh database, when ok all things are set to start import

            if (ok)
            {
                // Import CSV Data
                var csvImporter = new CSVData.Importer(importDataPath);
                ok = csvImporter.Import(dataBase);
                if (!ok)
                {
                    return 1; 
                }

                // TODO: Code Position: #3
                // Folgende Felder der Articles Tabelle: ArticleNumberId, ArticleNumber, Name, Description, DeliveryStateId
                // Folgende Felder der DeliveryStates Tabelle: DeliveryStatesId, Name
                var rows = dataBase.QueryData("SELECT * FROM litlepim.db.Articles");
                PrintToConsole(rows);
            } else {
                return 1; 
            }

            return 0; // this application uses CLI return codes
        }

        #region Pretty Print Helper
        private static void PrintToConsole(List<List<Tuple<string, string>>> rows)
        {
            // TODO: Code Position: #4
            int columnWidth = 20;

            Console.Write(Environment.NewLine);
            // Print header
            if (rows.Any())
            {
                foreach (var col in rows[0]) { Console.Write('+' + Repeat(columnWidth + 1, "-")); }
                Console.Write("+"); Console.Write(Environment.NewLine);
                foreach (var col in rows[0]) { Console.Write("| " + ZeroPad(col.Item1, columnWidth)); }
                Console.Write("|"); Console.Write(Environment.NewLine);
                foreach (var col in rows[0]) { Console.Write('+' + Repeat(columnWidth + 1, "-")); }
                Console.Write("+"); Console.Write(Environment.NewLine);
            }

            // Print rows
            foreach (var row in rows)
            {
                foreach (var col in row) { Console.Write("| " + ZeroPad(col.Item2, columnWidth)); }
                Console.Write("|"); Console.Write(Environment.NewLine);
            }

            // EOL
            if (rows.Any())
            {
                foreach (var col in rows[0]) { Console.Write('+' + Repeat(columnWidth + 1, "-")); }
                Console.Write("|"); Console.Write(Environment.NewLine);
            }
        }

        private static string Repeat(int count, string str)
        {
            return string.Concat(Enumerable.Repeat(str, count));
        }

        private static string ZeroPad(string text, int count)
        {
            int paddAmount = count > text.Length ? count - text.Length : -1;
            if (paddAmount > 0)
            {
                return text + string.Concat(Enumerable.Repeat(" ", paddAmount));
            }
            return text.Substring(0, count);
        }
        #endregion
    }
}
