using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using SimpleJSON;
using Newtonsoft.Json.Linq;

/// <summary>
/// Handles API calls for retrieving and posting data in the game.
/// </summary>
public class ApiCaller : MonoBehaviour
{
    #region Fields

    private string apiUrl;  // URL for GET API request
    private string postApiUrl;  // URL for POST API request
   
    public static string[][] apiData;  // 2D array for storing API data
   
  //  public static string rValue;  // URL parameter value for 'r'
   //public static string rValue = "EXhg%2f82oUP1rISTtKeUWRSThzsbzIWS4yUH3pOK%40%40%40%2f2YeZbKxfmb7FAbud2Cz3sfNoxGlGmAxBNO49Ouha6JD6Q%3d%3d";  // URL parameter value for 'r'
  public static string rValue = "hYxn%2fzZs0e3%2fw7hyhnD90g%3d%3d";  // URL parameter value for 'r'
  // public static string rValue = "We2BvG2a6hDlrlHuxuOdHcE6JM8Fgy3e";  // URL parameter value for 'r'
  
    public float rating;  // Player rating retrieved from API
  //  [SerializeField] Text ratingText;
    #endregion

    #region Unity Methods

    /// <summary>
    /// Initializes the component. Called on object creation.
    /// </summary>
    void Awake()
    {
        ParseUrlParameters();  // Parse URL parameters
        // Initialize API URLs with r value
        apiUrl = $"{ApiConfig.BaseUrl}{ApiConfig.GetApiEndpoint}?r={rValue}";
        postApiUrl = $"{ApiConfig.BaseUrl}{ApiConfig.PostApiEndpoint}";
      
    }

    /// <summary>
    /// Called before the first frame update.
    /// </summary>
    async void Start()
    {
        await GetDataFromApi();  // Fetch data from API on start
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Parses URL parameters to extract the 'r' value.
    /// </summary>
    private void ParseUrlParameters()
    {
        string url = Application.absoluteURL;
        if (!string.IsNullOrEmpty(url))
        {
            string[] urlParts = url.Split('?');
            if (urlParts.Length > 1)
            {
                string[] parameters = urlParts[1].Split('&');
                foreach (string param in parameters)
                {
                    string[] keyValue = param.Split('=');
                    if (keyValue.Length == 2 && keyValue[0] == "r")
                    {
                        rValue = keyValue[1];
                    }
                }
            }
        }
    }

    /// <summary>
    /// Fetches data from the API asynchronously.
    /// </summary>
    private async Task GetDataFromApi()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(apiUrl))
        {
            // Set the required header
            webRequest.SetRequestHeader("security-key", ApiConfig.SecurityKey);

            // Request and wait for the desired page
            var asyncOperation = webRequest.SendWebRequest();
            while (!asyncOperation.isDone)
            {
                await Task.Yield();
            }

            if (webRequest.result == UnityWebRequest.Result.ConnectionError ||
                webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + webRequest.error);
            }
            else
            {
                // Print the JSON data
                Debug.Log("Received: " + webRequest.downloadHandler.text);

                string json = webRequest.downloadHandler.text;
                JSONNode parsedData = JSON.Parse(json);

                if (parsedData["isError"].AsBool)
                {
                    Debug.LogError("Error in API response: " + parsedData["message"]);
                }
                else
                {
                    // Access the result field
                    string resultString = parsedData["result"];
                    JSONArray jsonArray = JSON.Parse(resultString).AsArray;

                    // Initialize the apiData array with the correct size
                    apiData = new string[jsonArray.Count][];
                    for (int i = 0; i < jsonArray.Count; i++)
                    {
                        JSONArray innerArray = jsonArray[i].AsArray;
                        apiData[i] = new string[innerArray.Count];
                        for (int j = 0; j < innerArray.Count; j++)
                        {
                            apiData[i][j] = innerArray[j].ToString();
                        }
                    }
                   
               

                }
            }
        }
    }

    /// <summary>
    /// Posts data to the API.
    /// </summary>
    public void SaveDataToApi()
    {
       StartCoroutine(PostDataToApi(DataManager.jsonData));
    }

    /// <summary>
    /// Coroutine to post data to the API.
    /// </summary>
    private IEnumerator PostDataToApi(string jsonData)
    {
        return PostDataToApiAsync(jsonData).AsCoroutine();
    }

    /// <summary>
    /// Asynchronously posts data to the API.
    /// </summary>
    private async Task PostDataToApiAsync(string jsonData)
    {
        using (UnityWebRequest webRequest = new UnityWebRequest(postApiUrl, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
            webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/json");
            webRequest.SetRequestHeader("security-key", ApiConfig.SecurityKey);

            var asyncOperation = webRequest.SendWebRequest();
            while (!asyncOperation.isDone)
            {
                await Task.Yield();
            }

            if (webRequest.result == UnityWebRequest.Result.ConnectionError ||
                webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + webRequest.error);
            }
            else
            {
               // Debug.Log("Data successfully posted: " + webRequest.downloadHandler.text);

                string response = webRequest.downloadHandler.text;
                JObject jsonResponse = JObject.Parse(response);
                JObject result = JObject.Parse(jsonResponse["result"].ToString());
                string studentName = result["Table"][0]["student_name"].ToString();
                rating = (float)result["Table"][0]["rating"];
             //   ratingText.text = "Rating: " + (int)rating;
             //   ratingText.gameObject.SetActive(true);
             //   Debug.Log("Student Name: " + studentName);
             //   Debug.Log("Rating: " + rating);
              
            }
        }
    }

    #endregion
}

/// <summary>
/// Extension methods for converting tasks to coroutines.
/// </summary>
public static class TaskExtensions
{
    /// <summary>
    /// Converts a Task to a Coroutine.
    /// </summary>
    public static IEnumerator AsCoroutine(this Task task)
    {
        while (!task.IsCompleted)
        {
            yield return null;
        }
        if (task.IsFaulted)
        {
            throw task.Exception;
        }
    }
}
