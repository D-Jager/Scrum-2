using UnityEngine;
using TMPro;

public class LevelScoreData : MonoBehaviour
{
    public LeaderboardDataManager leaderboard;
    
    [Header("UI & Score")]
    public TMP_InputField playerNameInputField;
    public int behaaldeScore;
    public TextMeshProUGUI scoreTextField;

    public void VerstuurBehaaldeScore()
    {
        string playerName = playerNameInputField.text; 
        // string playerName = "Speler";
        
        if (LevelOverdracht.GeselecteerdLevel == null)
        {
            Debug.LogError("Fout: LevelOverdracht.GeselecteerdLevel is leeg!");
            return;
        }

        string automatischeLevelNaam = LevelOverdracht.GeselecteerdLevel.levelNaam; 
        
        Debug.Log("Behaalde score: " + behaaldeScore);
        Debug.Log("Speler naam: " + playerName);
        Debug.Log("Level naam: " + automatischeLevelNaam);

        leaderboard.SaveScore(playerName, automatischeLevelNaam, behaaldeScore);
    }
}