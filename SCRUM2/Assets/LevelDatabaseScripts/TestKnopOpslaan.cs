using UnityEngine;

public class TestKnopOpslaan : MonoBehaviour
{
    // Sleep je bestaande LevelDatabaseManager hier in de Unity Inspector
    public LevelDatabaseManager databaseManager;

    // Deze functie koppel je aan de OnClick() van je UI-knop
    public void MaakEnSlaTestLevelOp()
    {
        // 1. Maak een nieuw, nep level aan
        LevelData testLevel = new LevelData();
        testLevel.levelNaam = "Automatisch Test Level";
        testLevel.makerNaam = "De Test Knop";

        // 2. Voeg een paar willekeurige test-blokjes toe
        testLevel.alleTiles.Add(new TileData(1, 0f, 0f));
        testLevel.alleTiles.Add(new TileData(2, 1f, 1f));

        // 3. Voeg een test-object toe
        testLevel.alleObjecten.Add(new ObjectData(99, 5f, 5f));

        // 4. Stuur het level naar jouw json-server
        if (databaseManager != null)
        {
            databaseManager.SlaLevelOp(testLevel);
            Debug.Log("Test level verzonden naar de server!");
        }
        else
        {
            Debug.LogError("Let op: LevelDatabaseManager is niet gekoppeld in de Inspector!");
        }
    }
}