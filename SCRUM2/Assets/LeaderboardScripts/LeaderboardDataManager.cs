using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

[System.Serializable]
public class LeaderboardData
{
    public int id;
    public string playerName;
    public string levelName;
    public int score;
}

public class LeaderboardDataManager : MonoBehaviour
{
   public string leaderboardUrl = "https://vastness-siding-winking.ngrok-free.dev/leaderboard";

   public void SaveScore(string playerName, string levelName, int score)
   {
       LeaderboardData leaderboardData = new LeaderboardData();
       leaderboardData.playerName = playerName;
       leaderboardData.levelName = levelName;
       leaderboardData.score = score;

       StartCoroutine(VerstuurScore(leaderboardData));
   }

   private IEnumerator VerstuurScore(LeaderboardData data)
   {
       string jsonData = JsonUtility.ToJson(data);

       UnityWebRequest request = new UnityWebRequest(leaderboardUrl, "POST");
       byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
       
       request.uploadHandler = new UploadHandlerRaw(bodyRaw);
       request.downloadHandler = new DownloadHandlerBuffer();
       request.SetRequestHeader("Content-Type", "application/json");

       yield return request.SendWebRequest();

       if (request.result == UnityWebRequest.Result.Success)
       {
           Debug.Log("Score succesvol opgeslagen op het leaderboard!");
       }
       else
       {
           Debug.LogError("Fout bij het opslaan: " + request.error);
       }
   }
}