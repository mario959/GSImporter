## Goodson Dataimporter  

## Beschreibung  
Dieses Kommandozeilen Programm soll Daten von einer CSV Datei  in eine SQLite Datenbank importieren.  
Das Program kann wie folgt aufgerufen werden:  

```Goodson.Importer <pfad der importdatei>```   

Es gibt also einen Übergabe Parameter und das ist der Pfad zu der CSV Datei.  
Es liegt eine Beispiel CSV Datei dem Projekt bei.  

## Deine Programmieraufgaben  
Über die Codeposition findest du die einzelnen Stellen im Code die erweitert werden sollen.  
Also einfach nach "Code Position: #1" in der Solution suchen, dann findest du die Stelle.  

## Aufgaben  

## Aufgabe 1: Prüfen des Eingabeparameters 
**Code Position: #1**  
Prüfe den Eingabeparameter des Benutzers. Überlege was der Benutzer falsch machen könnte und Benden das Programm bei Falscheingabe mit einer klaren Fehlermeldung.  

## Aufgabe 2: Lesen und Importieren einer CSV Datei  
**Code Position: #2**  
Vervollständige die Klassenbibliothek um deine Implementierung, sodass die beigelegte Beispiel CSV Datei (Articles.csv) über die Goodson.Database.TestDatabase Klasse importiert wird.  

Das Grundgerüst dieser Klassenbibliothek ist schon im vorliegenden Projekt vorhanden. Deine Aufgabe ist es, diese fertig zu implementieren und die im Beispiel befindliche CSV Datei mit Hilfe der Goodson.Database. TestDatabase Klasse in eine SQLite Datenbank zu importieren.  
Die Goodson.Database.TestDatabase Klasse ist ebenfalls schon implementiert und kann verwendet werden. Die Anwendung der Goodson.Database.TestDatabase Klasse entnimmst du dem Code.  

## Aufgabe 3: Ausgabe  
**Code Position: #3**  
Gib alle importierten Daten, absteigend sortiert nach dem Feld Artikelnummer, über einen SQL-Befehl aus.  
Optional kannst du über den SQL Befehl die Fremdtabelle Place joinen und den Lieferstatus Namen ausgeben. Wenn kein Lieferstatus im Datensatz vorhanden ist, gib einfach nichts aus.  

## Aufgabe 4: Ausgabe verbessern
**Code Position: #4**  
Es scheinen bei der Ausgabe manchen Felder abgeschnitten zu sein. 
Schön wäre wenn sich die Spalten an die längste Text in einer Zeile anpassen würde.

## Sonstiges  
Wenn du Fragen hast, kannst du mich jederzeit fragen.

**Viel Erfolg.**
