using UnityEngine;
using UnityEngine.Networking;              
using System.Collections;
using System;

public class LevelDatabaseManager : MonoBehaviour
{
    // public string serverUrl = "http://localhost:3000/levels";
    public string serverUrl = "https://vastness-siding-winking.ngrok-free.dev/levels";

    public void HaalAlleLevelsOp(Action<LevelData[]> callback)
    {
        StartCoroutine(DownloadAlleRoutine(callback));
    }

    private IEnumerator DownloadAlleRoutine(Action<LevelData[]> callback)
    {
        UnityWebRequest request = UnityWebRequest.Get(serverUrl);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string gedownloadeJson = request.downloadHandler.text;
            LevelData[] alleLevels = JsonHelper.FromJson<LevelData>(gedownloadeJson);
            callback?.Invoke(alleLevels);
        }
        else
        {
            Debug.LogError("Fout bij ophalen levels: " + request.error);
        }
    }


    public void SlaLevelOp(LevelData nieuwLevel)
    {
        StartCoroutine(UploadRoutine(nieuwLevel));
    }

    private IEnumerator UploadRoutine(LevelData level)
    {
        string json = JsonUtility.ToJson(level);
        UnityWebRequest request = new UnityWebRequest(serverUrl, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Level succesvol opgeslagen in de database!");
        }
        else
        {
            Debug.LogError("Fout bij opslaan level: " + request.error);
        }
    }
}