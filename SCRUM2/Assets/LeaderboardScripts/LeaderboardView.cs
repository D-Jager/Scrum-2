using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using TMPro;

public class LeaderboardView : MonoBehaviour
{
    public string leaderboardUrl = "https://vastness-siding-winking.ngrok-free.dev/leaderboard";

    [Header("UI Elements")]
    public Transform contentParent;
    public GameObject playerscorePrefab;

    void Start() => StartCoroutine(GetScores());
    
    IEnumerator GetScores()
    {
        UnityWebRequest request = UnityWebRequest.Get(leaderboardUrl);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string jsonList = "{ \"items\": " + request.downloadHandler.text + " }";
            LeaderboardWrapper datalist = JsonUtility.FromJson<LeaderboardWrapper>(jsonList);

            foreach (LeaderboardData data in datalist.items)
            {
                GameObject newItem = Instantiate(playerscorePrefab, contentParent);

                TextMeshProUGUI tekstveld = newItem.GetComponentInChildren<TextMeshProUGUI>();
                if (tekstveld != null)
                {
                    tekstveld.text = data.playerName + " - " + data.score + " punten (" + data.levelName + ")";
                }
            }
        }
        else
        {
            Debug.LogError("Error with getting leaderboard: " + request.error);
        }
    }
}

[System.Serializable]
public class LeaderboardWrapper
{
    public LeaderboardData[] items; // Gecorrigeerd van LeaderboardDat naar LeaderboardData
}