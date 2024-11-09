using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using System.Collections;
using System.IO;
using System.Threading.Tasks;

[System.Serializable]
public class AnswerData
{
    public int question_id;
    public string answer;
    public bool is_correct;
    public string time_taken;
    public string submission_date_time;
}

[System.Serializable]
public class GameData
{
   
    public string r;
    public string token;
    public List<AnswerData> question_data = new List<AnswerData>();
}

public class DataManager : MonoBehaviour
{
    private static GameData currentGameData;
    public static string jsonData;

    private void Start()
    {
        StartNewGame();
    }

    // Load answer data from a JSON file
    public static GameData LoadAnswerData()
    {
        string filePath = Path.Combine(Application.persistentDataPath, "answerData.json");
        if (File.Exists(filePath))
        {
            jsonData = File.ReadAllText(filePath);
            return JsonUtility.FromJson<GameData>(jsonData);
        }
        else
        {
            Debug.LogError("No answer data file found at: " + filePath);
            return null;
        }
    }

    // Save answer data to a JSON file
    public static async Task SaveAnswerDataAsync(AnswerData answerData)
    {
        if (currentGameData == null)
        {
            currentGameData = new GameData
            {
               r = ApiCaller.rValue,
                token = "sahks-sadasd-asdas-asdas"
            };
        }

        currentGameData.question_data.Add(answerData);
        jsonData = JsonUtility.ToJson(currentGameData, true);
        string filePath = Path.Combine(Application.persistentDataPath, "answerData.json");

        await Task.Run(() => File.WriteAllText(filePath, jsonData));
    }

    //public IEnumerator SaveGameDataToAPI(GameData gameData)
    //{
    //    string json = JsonUtility.ToJson(gameData);
    //    byte[] jsonToSend = new UTF8Encoding().GetBytes(json);

    //    using (UnityWebRequest request = new UnityWebRequest("https://test.moonr.com/LMSService/api/Game/SaveMathMantraGameData", "POST"))
    //    {
    //        request.uploadHandler = new UploadHandlerRaw(jsonToSend);
    //        request.downloadHandler = new DownloadHandlerBuffer();
    //        request.SetRequestHeader("Content-Type", "application/json");
    //        request.SetRequestHeader("security-key", "F3404E43-E4C9-4554-8AB5-93E3B8448674");

    //        yield return request.SendWebRequest();

    //        if (request.result == UnityWebRequest.Result.Success)
    //        {
    //          //  Debug.Log("Form upload complete! Response: " + request.downloadHandler.text);
    //        }
    //        else
    //        {
    //            Debug.LogError("Error: " + request.error);
    //        }
    //    }
    //}

    //public async void SaveGameData(AnswerData answerData)
    //{
    //    await SaveAnswerDataAsync(answerData);

    //    if (currentGameData != null)
    //    {
    //        StartCoroutine(SaveGameDataToAPI(currentGameData));
    //    }
    //}

    // Call this method to start a new game session
    public void StartNewGame()
    {
        currentGameData = new GameData
        {
            r = ApiCaller.rValue,
            token = "sahks-sadasd-asdas-asdas"
        };

        jsonData = JsonUtility.ToJson(currentGameData, true);
        string filePath = Path.Combine(Application.persistentDataPath, "answerData.json");
        File.WriteAllText(filePath, jsonData);
    }
}
