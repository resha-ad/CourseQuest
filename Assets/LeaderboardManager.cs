using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;
using TMPro;
using System.Threading.Tasks;


public class LeaderboardManager : MonoBehaviour
{
    public GameObject leaderboard; // Reference to the main Canvas
    public GameObject mainMenu; // Reference to the main Canvas
    public TMP_Text scoreText; // Reference to the score text on the Canvas


    // Start is called before the first frame update
    // Start is called before the first frame update
    [System.Obsolete]
    async void Start()
    {
        await SetupAndSignIn(); // Ensure initialization and sign-in is complete
        LoadData();  // Load data after successful sign-in and initialization
    }

    public void ShowMenu()
    {
        leaderboard.SetActive(false);
        mainMenu.SetActive(true);
    }

    async Task SetupAndSignIn()
    {
        await UnityServices.InitializeAsync();
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }


    //cloud
    [System.Obsolete]
    async void LoadData()
    {
        try
        {
            // Fetch all data from Cloud Save
            Dictionary<string, string> savedData = await CloudSaveService.Instance.Data.LoadAllAsync();

            if (savedData.Count > 0)
            {
                // Convert the dictionary to a list of key-value pairs
                List<KeyValuePair<string, string>> sortedData = new List<KeyValuePair<string, string>>(savedData);

                // Sort the list by the value (score) in descending order
                sortedData.Sort((x, y) => int.Parse(y.Value).CompareTo(int.Parse(x.Value)));

                // Prepare the data to display
                string loadedData = "Rank\t\tID\t\tScore\n";
                int rank = 1;

                // Iterate through the sorted data and display only top 7
                foreach (var entry in sortedData)
                {
                    if (rank > 7) break;  // Stop after displaying top 7 entries

                    loadedData += $"{rank}\t\t{entry.Key}\t\t{entry.Value}\n";
                    rank++;
                }

                scoreText.text = loadedData; // Display the sorted data in the UI
            }
            else
            {
                scoreText.text = "No saved data found.";
            }
        }
        catch (System.Exception ex)
        {
            scoreText.text = "Error loading data: " + ex.Message;
            Debug.LogError("Cloud load failed: " + ex.Message);
        }
    }




    // Update is called once per frame
    void Update()
    {

    }
}
