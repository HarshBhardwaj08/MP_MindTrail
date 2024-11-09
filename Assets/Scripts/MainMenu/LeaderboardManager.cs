using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;
using Unity.VisualScripting;
public class LeaderboardManager : MonoBehaviour
{
    private string apiUrl;
    private string apiKey;

    public Transform leaderboardContainer;
    public GameObject leaderboardEntryPrefab;
    public float updateInterval = 5f; // Time interval for updating the leaderboard
    public float transitionDuration = 1f; // Duration of the smooth transition
    public float gapValue = 75; // Gap between leaderboard entries
    public float startDelay = 1f; // Delay before fetching leaderboard data
    public GameObject leaderboardPanel;
    [SerializeField] AudioSource transitionSfx;

    public AudioClip moveUpSfx; // Sound effect for moving up
    public AudioClip moveDownSfx; // Sound effect for moving down

    public static string myName; // Player's name to check for highlighting
    public Color highlightColor = Color.yellow; // Color to highlight the player's entry
    private bool isLeaderboardUpdated = false; // Flag to check if leaderboard data is updated

    public Sprite medal1Image; // Medal image for 1st place
    public Sprite medal2Image; // Medal image for 2nd place
    public Sprite medal3Image; // Medal image for 3rd place

    private Dictionary<string, int> previousPositions = new Dictionary<string, int>(); // Store previous positions

    void Start()
    {
        // Initialize API URL and security key
        apiKey = ApiConfig.SecurityKey;

        // Start the periodic leaderboard update
        StartCoroutine(UpdateLeaderboardPeriodically());
    }

    IEnumerator UpdateLeaderboardPeriodically()
    {
        while (true)
        {
            // Wait before fetching data
            yield return new WaitForSeconds(startDelay);

            if (leaderboardPanel.activeSelf) // Check if leaderboard panel is active
            {
                // Construct API URL with r parameter
                apiUrl = $"{ApiConfig.BaseUrl}{ApiConfig.LeaderboardApiEndpoint}?r={ApiCaller.rValue}";
            
                yield return StartCoroutine(GetLeaderboardData());

                // Play transition sound effect if the leaderboard was updated
                if (isLeaderboardUpdated)
                {
                    transitionSfx.Play();
                    isLeaderboardUpdated = false; // Reset the flag
                }
            }

            yield return new WaitForSeconds(updateInterval);
        }
    }

    IEnumerator GetLeaderboardData()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(apiUrl))
        {
            // Set the security key header
            request.SetRequestHeader("security-key", apiKey);

            // Send the request and wait for the response
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(request.error);
            }
            else
            {
                string json = request.downloadHandler.text;
                
                // Deserialize and process the JSON response
                LeaderboardResponse response = JsonConvert.DeserializeObject<LeaderboardResponse>(json);

                if (response != null && response.statusCode == 200)
                {
                    List<PlayerDataApi> players = ParseLeaderboardJson(response.result);
                    DisplayLeaderboard(players);
                }
                else
                {
                    Debug.LogError("Failed to fetch leaderboard data. StatusCode: " + response.statusCode);
                }
            }
        }
    }

        List<PlayerDataApi> ParseLeaderboardJson(string json)
    {
        List<PlayerDataApi> players = new List<PlayerDataApi>();

        try
        {
            JObject jsonObject = JObject.Parse(json);
            JArray table = (JArray)jsonObject["Table"];

            foreach (var entry in table)
            {
                PlayerDataApi player = new PlayerDataApi();
                player.name = entry["student_name"].ToString();
                player.score = (int)entry["score"];
                players.Add(player);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Error parsing leaderboard JSON: " + ex.Message);
        }

        return players;
    }

    void DisplayLeaderboard(List<PlayerDataApi> players)
    {
        // Sort players by score in descending order
        players.Sort((p1, p2) => p2.score.CompareTo(p1.score));

        // Create a dictionary to store the new positions
        Dictionary<string, int> newPositions = new Dictionary<string, int>();
        for (int i = 0; i < players.Count; i++)
        {
            players[i].rank = i + 1; // Assign rank based on sorted position
            newPositions[players[i].name] = i;
        }

        bool dataChanged = false;

        // Animate existing entries to their new positions
        foreach (Transform child in leaderboardContainer)
        {
            string name = child.Find("NameText").GetComponent<Text>().text;
            if (newPositions.ContainsKey(name))
            {
                int newPosition = newPositions[name];
                // Update the score and rank text before animating the position
                int currentScore = int.Parse(child.Find("ScoreText").GetComponent<Text>().text);
                if (currentScore != players[newPosition].score)
                {
                    dataChanged = true;
                }
                child.Find("ScoreText").GetComponent<Text>().text = players[newPosition].score.ToString();
                child.Find("RankText").GetComponent<Text>().text = players[newPosition].rank.ToString(); // Assuming you have a Text component for rank
                StartCoroutine(AnimateEntry(child, newPosition));

                // Highlight player's entry if it matches
                if (name == myName)
                {
                    HighlightEntry(child);
                    // Check for position change and play the appropriate sound
                    if (previousPositions.ContainsKey(name))
                    {
                        int previousPosition = previousPositions[name];
                        if (newPosition < previousPosition)
                        {
                            // Moved up
                            transitionSfx.PlayOneShot(moveUpSfx);
                        }
                        else if (newPosition > previousPosition)
                        {
                            // Moved down
                            transitionSfx.PlayOneShot(moveDownSfx);
                        }
                    }
                    previousPositions[name] = newPosition;
                }


                newPositions.Remove(name);
            }
            else
            {
                Destroy(child.gameObject);
                dataChanged = true;
            }
        }

        // Create new entries for new players
        foreach (KeyValuePair<string, int> entry in newPositions)
        {
            GameObject newEntry = Instantiate(leaderboardEntryPrefab, leaderboardContainer);

            newEntry.transform.Find("NameText").GetComponent<Text>().text = entry.Key;
            newEntry.transform.Find("ScoreText").GetComponent<Text>().text = players[entry.Value].score.ToString();
            newEntry.transform.Find("RankText").GetComponent<Text>().text = players[entry.Value].rank.ToString(); // Assuming you have a Text component for rank

            // Add medal images to top 3 players
            if (players[entry.Value].rank == 1)
            {
               newEntry.transform.Find("MedalImage").AddComponent<Image>().sprite = medal1Image;
                newEntry.transform.Find("MedalImage").GetComponent<Image>().SetNativeSize();

            }
            else if (players[entry.Value].rank == 2)
            {
                newEntry.transform.Find("MedalImage").AddComponent<Image>().sprite = medal2Image;
                newEntry.transform.Find("MedalImage").GetComponent<Image>().SetNativeSize();

            }
            else if (players[entry.Value].rank == 3)
            {
                newEntry.transform.Find("MedalImage").AddComponent<Image>().sprite = medal3Image;
                newEntry.transform.Find("MedalImage").GetComponent<Image>().SetNativeSize();

            }


            StartCoroutine(AnimateEntry(newEntry.transform, entry.Value));

            // Highlight player's entry if it matches
            if (entry.Key == myName)
            {
                HighlightEntry(newEntry.transform);
            }

            dataChanged = true;
        }

        if (dataChanged)
        {
            isLeaderboardUpdated = true; // Set the flag if data has changed
        }
    }

    void HighlightEntry(Transform entry)
    {
        // Example highlight effect: changing the background color of the entry
        Image backgroundImage = entry.GetComponent<Image>();
        if (backgroundImage != null)
        {
            backgroundImage.color = highlightColor;
        }
    }

    IEnumerator AnimateEntry(Transform entry, int newPosition)
    {
        Vector3 startPosition = entry.localPosition;
        Vector3 endPosition = new Vector3(startPosition.x, -newPosition * gapValue, startPosition.z); // Assuming each entry has a height of 75 units

        float elapsedTime = 0f;
        while (elapsedTime < transitionDuration)
        {
            entry.localPosition = Vector3.Lerp(startPosition, endPosition, elapsedTime / transitionDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        entry.localPosition = endPosition;
    }
}

// Define a class to hold player data
public class PlayerDataApi
{
    public string name;
    public int score;
    public int rank; // New rank property
}

// Define a class to deserialize the API response
[System.Serializable]
public class LeaderboardResponse
{
    public string version;
    public int statusCode;
    public string message;
    public bool isError;
    public string result;
}

//using UnityEngine;
//using UnityEngine.Networking;
//using UnityEngine.UI;
//using System.Collections;
//using System.Collections.Generic;
//using Newtonsoft.Json;
//using Newtonsoft.Json.Linq;
//using System;

//public class LeaderboardManager : MonoBehaviour
//{
//    private string apiUrl = "https://test.moonr.com/LMSService/api/Game/GetLeaderboardData?r=Jl4vEoyVLLohAn4JRoLbgA%3d%3d";
//    private string apiKey = "F3404E43-E4C9-4554-8AB5-93E3B8448674"; // Security key header

//    public Transform leaderboardContainer;
//    public GameObject leaderboardEntryPrefab;
//    public float updateInterval = 5f; // Time interval for updating the leaderboard
//    public float transitionDuration = 1f; // Duration of the smooth transition
//    public float gapValue = 75;

//    void Start()
//    {
//        StartCoroutine(UpdateLeaderboardPeriodically());
//    }

//    IEnumerator UpdateLeaderboardPeriodically()
//    {
//        while (true)
//        {
//            yield return StartCoroutine(GetLeaderboardData());
//            yield return new WaitForSeconds(updateInterval);
//        }
//    }

//    IEnumerator GetLeaderboardData()
//    {
//        using (UnityWebRequest request = UnityWebRequest.Get(apiUrl))
//        {
//            request.SetRequestHeader("security-key", apiKey);

//            yield return request.SendWebRequest();

//            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
//            {
//                Debug.LogError(request.error);
//            }
//            else
//            {
//                string json = request.downloadHandler.text;

//                // Parse the JSON response directly to get student names and scores
//                LeaderboardResponse response = JsonConvert.DeserializeObject<LeaderboardResponse>(json);

//                if (response != null && response.statusCode == 200)
//                {
//                    List<PlayerDataApi> players = ParseLeaderboardJson(response.result);
//                    DisplayLeaderboard(players);
//                }
//                else
//                {
//                    Debug.LogError("Failed to fetch leaderboard data. StatusCode: " + response.statusCode);
//                }
//            }
//        }
//    }

//    List<PlayerDataApi> ParseLeaderboardJson(string json)
//    {
//        List<PlayerDataApi> players = new List<PlayerDataApi>();

//        // Deserialize the JSON and extract student names and scores
//        try
//        {
//            JObject jsonObject = JObject.Parse(json);
//            JArray table = (JArray)jsonObject["Table"];

//            foreach (var entry in table)
//            {
//                PlayerDataApi player = new PlayerDataApi();
//                player.name = entry["student_name"].ToString();
//                player.score = (int)entry["score"];
//                players.Add(player);
//            }
//        }
//        catch (Exception ex)
//        {
//            Debug.LogError("Error parsing leaderboard JSON: " + ex.Message);
//        }

//        return players;
//    }

//    void DisplayLeaderboard(List<PlayerDataApi> players)
//    {
//        // Sort players by score in descending order
//        players.Sort((p1, p2) => p2.score.CompareTo(p1.score));

//        // Create a dictionary to store the new positions
//        Dictionary<string, int> newPositions = new Dictionary<string, int>();
//        for (int i = 0; i < players.Count; i++)
//        {
//            players[i].rank = i + 1; // Assign rank based on sorted position
//            newPositions[players[i].name] = i;
//        }

//        // Animate existing entries to their new positions
//        foreach (Transform child in leaderboardContainer)
//        {
//            string name = child.Find("NameText").GetComponent<Text>().text;
//            if (newPositions.ContainsKey(name))
//            {
//                int newPosition = newPositions[name];
//                // Update the score and rank text before animating the position
//                child.Find("ScoreText").GetComponent<Text>().text = players[newPosition].score.ToString();
//                child.Find("RankText").GetComponent<Text>().text = players[newPosition].rank.ToString(); // Assuming you have a Text component for rank
//                StartCoroutine(AnimateEntry(child, newPosition));
//                newPositions.Remove(name);
//            }
//            else
//            {
//                Destroy(child.gameObject);
//            }
//        }

//        // Create new entries for new players
//        foreach (KeyValuePair<string, int> entry in newPositions)
//        {
//            GameObject newEntry = Instantiate(leaderboardEntryPrefab, leaderboardContainer);

//            newEntry.transform.Find("NameText").GetComponent<Text>().text = entry.Key;
//            newEntry.transform.Find("ScoreText").GetComponent<Text>().text = players[entry.Value].score.ToString();
//            newEntry.transform.Find("RankText").GetComponent<Text>().text = players[entry.Value].rank.ToString(); // Assuming you have a Text component for rank
//            StartCoroutine(AnimateEntry(newEntry.transform, entry.Value));
//        }
//    }

//    IEnumerator AnimateEntry(Transform entry, int newPosition)
//    {
//        Vector3 startPosition = entry.localPosition;
//        Vector3 endPosition = new Vector3(startPosition.x, -newPosition * gapValue, startPosition.z); // Assuming each entry has a height of 75 units

//        float elapsedTime = 0f;
//        while (elapsedTime < transitionDuration)
//        {
//            entry.localPosition = Vector3.Lerp(startPosition, endPosition, elapsedTime / transitionDuration);
//            elapsedTime += Time.deltaTime;
//            yield return null;
//        }
//        entry.localPosition = endPosition;
//    }
//}

//// Define a class to hold player data
//public class PlayerDataApi
//{
//    public string name;
//    public int score;
//    public int rank; // New rank property
//}

//// Define a class to deserialize the API response
//[System.Serializable]
//public class LeaderboardResponse
//{
//    public string version;
//    public int statusCode;
//    public string message;
//    public bool isError;
//    public string result;
//}


//using UnityEngine;
//using UnityEngine.Networking;
//using UnityEngine.UI;
//using System.Collections;
//using System.Collections.Generic;
//using Newtonsoft.Json;

//public class LeaderboardManager : MonoBehaviour
//{
//    private string apiUrl = "https://6682d57a4102471fa4c8673c.mockapi.io/api/v1/MPCheck";
//    public Transform leaderboardContainer;
//    public GameObject leaderboardEntryPrefab;
//    public float updateInterval = 5f; // Time interval for updating the leaderboard
//    public float transitionDuration = 1f; // Duration of the smooth transition
//    public AudioSource sfxSource;
//    public AudioClip moveUpSfx;
//    public AudioClip moveDownSfx;

//    public string myName; // Player's name to check for highlighting
//    public Color highlightColor = Color.yellow; // Color to highlight the player's entry
//    private Dictionary<string, int> previousPositions = new Dictionary<string, int>(); // Store previous positions

//    void Start()
//    {
//        StartCoroutine(UpdateLeaderboardPeriodically());
//    }

//    IEnumerator UpdateLeaderboardPeriodically()
//    {
//        while (true)
//        {
//            yield return StartCoroutine(GetLeaderboardData());
//            yield return new WaitForSeconds(updateInterval);
//        }
//    }

//    IEnumerator GetLeaderboardData()
//    {
//        UnityWebRequest request = UnityWebRequest.Get(apiUrl);
//        yield return request.SendWebRequest();

//        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
//        {
//            Debug.LogError(request.error);
//        }
//        else
//        {
//            string json = request.downloadHandler.text;
//            List<PlayerDataApi> players = JsonConvert.DeserializeObject<List<PlayerDataApi>>(json);
//            DisplayLeaderboard(players);
//        }
//    }

//    void DisplayLeaderboard(List<PlayerDataApi> players)
//    {
//        // Sort players by score in descending order
//        players.Sort((p1, p2) => p2.score.CompareTo(p1.score));

//        // Create a dictionary to store the new positions
//        Dictionary<string, int> newPositions = new Dictionary<string, int>();
//        for (int i = 0; i < players.Count; i++)
//        {
//            newPositions[players[i].name] = i;
//        }

//        // Check for player position changes and highlight
//        bool dataChanged = false;

//        // Animate existing entries to their new positions
//        foreach (Transform child in leaderboardContainer)
//        {
//            string name = child.Find("NameText").GetComponent<Text>().text;
//            if (newPositions.ContainsKey(name))
//            {
//                int newPosition = newPositions[name];
//                // Update the score text before animating the position
//                int currentScore = int.Parse(child.Find("ScoreText").GetComponent<Text>().text);
//                if (currentScore != players[newPosition].score)
//                {
//                    dataChanged = true;
//                }
//                child.Find("ScoreText").GetComponent<Text>().text = players[newPosition].score.ToString();
//                StartCoroutine(AnimateEntry(child, newPosition));

//                // Highlight player's entry if it matches
//                if (name == myName)
//                {
//                    HighlightEntry(child);
//                    // Check if player's position has changed
//                    if (previousPositions.ContainsKey(name))
//                    {
//                        int previousPosition = previousPositions[name];
//                        if (newPosition < previousPosition)
//                        {
//                            // Moved up
//                            sfxSource.PlayOneShot(moveUpSfx);
//                        }
//                        else if (newPosition > previousPosition)
//                        {
//                            // Moved down
//                            sfxSource.PlayOneShot(moveDownSfx);
//                        }
//                    }
//                    previousPositions[name] = newPosition;
//                }
//                newPositions.Remove(name);
//            }
//            else
//            {
//                Destroy(child.gameObject);
//                dataChanged = true;
//            }
//        }

//        // Create new entries for new players
//        foreach (KeyValuePair<string, int> entry in newPositions)
//        {
//            GameObject newEntry = Instantiate(leaderboardEntryPrefab, leaderboardContainer);
//            newEntry.transform.Find("NameText").GetComponent<Text>().text = entry.Key;
//            newEntry.transform.Find("ScoreText").GetComponent<Text>().text = players[entry.Value].score.ToString();
//            StartCoroutine(AnimateEntry(newEntry.transform, entry.Value));

//            // Highlight player's entry if it matches
//            if (entry.Key == myName)
//            {
//                HighlightEntry(newEntry.transform);
//                previousPositions[entry.Key] = entry.Value;
//            }

//            dataChanged = true;
//        }
//    }

//    void HighlightEntry(Transform entry)
//    {
//        // Example highlight effect: changing the background color of the entry
//        Image backgroundImage = entry.GetComponent<Image>();
//        if (backgroundImage != null)
//        {
//            backgroundImage.color = highlightColor;
//        }
//    }

//    IEnumerator AnimateEntry(Transform entry, int newPosition)
//    {
//        Vector3 startPosition = entry.localPosition;
//        Vector3 endPosition = new Vector3(startPosition.x, -newPosition * 50, startPosition.z); // Assuming each entry has a height of 50 units

//        float elapsedTime = 0f;
//        while (elapsedTime < transitionDuration)
//        {
//            entry.localPosition = Vector3.Lerp(startPosition, endPosition, elapsedTime / transitionDuration);
//            elapsedTime += Time.deltaTime;
//            yield return null;
//        }
//        entry.localPosition = endPosition;
//    }
//}

//public class PlayerDataApi
//{
//    public string name;
//    public int score;
//}
