using Goodson.Database;

namespace Goodson.CSVData
{
    public class Importer
    {
        public Importer(string csvPath)
        {        
        }

        public bool Import(SQLiteDatabaseHandler database)
        {
            // Als beispiel starten wir mit einem hard codierten test import
            // zuerst erstellen wir unseren datenbank helper fuer die article Tabelle
            var repo = database.CreateArticleRepository();

            // dann fuegen wir ein beispiel datensaetz hinzu
            repo.InsertArticle(articleNumber: "1000", "Hundefutter", "Testbeschreibung", deliveryStateId: 1);

            // Hier bitte die über den Konstruktor übergebene csv datei importieren ...
            // Die Testdaten Articles.csv sind Komma "," separiert und enthalten nur einzeilge Daten.

            // TODO: Code Position: #2

            // irgendwas fehlt wohl noch das die Daten auch gespeichert werden ... lese mal den code von Goodson.Database.ArticleDBRepository durch.
            repo.Commit();
            return true;
        }
    }
}
