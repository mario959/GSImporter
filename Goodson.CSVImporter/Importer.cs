using Goodson.Database;
using System;
using System.Collections.Generic;
using System.IO;

namespace Goodson.CSVData
{
    public class Importer
    {
        private string path;
        public string Artikelnummer { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Delivery { get; set; }

        public Importer(string csvPath)
        {
            path = csvPath;
        }

        public bool Import(SQLiteDatabaseHandler database)
        {
            // Als beispiel starten wir mit einem hard codierten test import
            // zuerst erstellen wir unseren datenbank helper fuer die article Tabelle
            var repo = database.CreateArticleRepository();

            // dann fuegen wir ein beispiel datensaetz hinzu
            repo.InsertArticle(articleNumber: "1000", "Hundefutter", "Testbeschreibung", deliveryStateId: 1);
             
            string[] data = File.ReadAllLines(path);
            for (int i = 1; i < data.Length; i++)
            {
                string[] text = data[i].Split(",");
                Artikelnummer = text[0];
                Name = text[1];
                Description = text[2];
                try
                {
                    Delivery = Convert.ToInt32(text[3]);
                }
                catch (FormatException)
                {

                    Console.WriteLine("kein gültiges Format");

                }
                Console.ReadKey();
                

                repo.InsertArticle(Artikelnummer, Name, Description, Delivery);
            }

         

            // Hier bitte die über den Konstruktor übergebene csv datei importieren ...

            // Die Testdaten Articles.csv sind Komma "," separiert und enthalten nur einzeilge Daten.

            // TODO: Code Position: #2

            // irgendwas fehlt wohl noch das die Daten auch gespeichert werden ... lese mal den code von Goodson.Database.ArticleDBRepository durch.
            repo.Commit();
            return true;
        }
    }
}
