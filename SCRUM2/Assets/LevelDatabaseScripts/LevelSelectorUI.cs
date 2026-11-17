using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelectorUI : MonoBehaviour
{
    [Header("Sleep de UI elementen hierin:")]
    public LevelDatabaseManager databaseManager;
    public LevelSelectorUI levelSelectorUI;
    public GameObject levelKnopPrefab;
    public Transform contentPanel;

    [Header("Instellingen")]
    public string speelSceneNaam = "GameScene";

    void Start() => databaseManager?.HaalAlleLevelsOp(BouwLevelLijst);

    public void BouwLevelLijst(LevelData[] levels)
    {
        foreach (Transform child in contentPanel) Destroy(child.gameObject);

        if (levels == null || levels.Length == 0) return;

        foreach (LevelData level in levels)
        {
            GameObject nieuweKnop = Instantiate(levelKnopPrefab, contentPanel);

            TextMeshProUGUI knopTekst = nieuweKnop.GetComponentInChildren<TextMeshProUGUI>();
            if (knopTekst != null)
            {
                knopTekst.text = $"{level.levelNaam}\n<size=50%>Door: {level.makerNaam}</size>";
            }

            Button buttonComponent = nieuweKnop.GetComponent<Button>();
            if (buttonComponent != null)
            {
                buttonComponent.onClick.AddListener(() => SpeelLevel(level));
            }
        }
    }

    private void SpeelLevel(LevelData geselecteerdLevel)
    {
        LevelOverdracht.GeselecteerdLevel = geselecteerdLevel;
        SceneManager.LoadScene(speelSceneNaam);
    }

    public void klikOpVersersen()
    {
        databaseManager?.HaalAlleLevelsOp(BouwLevelLijst);

        Debug.Log("Lijst met levels wordt ververst via de server...");
        Debug.Log(databaseManager.name);
    }
}