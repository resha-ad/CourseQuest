using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenuCanvas; // Reference to the Main Menu Canvas
    public GameObject otherCanvas;   // Reference to the Other Canvas
    public GameObject leaderBoard;   // Reference to the Other Canvas
    public GameObject vulBoard;   // Reference to the Other Canvas

    // Method to be called when the Start button is clicked
    public void OnStartButtonClicked()
    {
        // Hide the Main Menu Canvas
        mainMenuCanvas.SetActive(false);

        // Show the Other Canvas
        otherCanvas.SetActive(true);
    }
    public void OnLeaderBoardButtonClicked()
    {
        // Hide the Main Menu Canvas
        mainMenuCanvas.SetActive(false);

        // Show the Other Canvas
        leaderBoard.SetActive(true);
    }


    public void OnVulBoardButtonClicked()
    {
        // Hide the Main Menu Canvas
        mainMenuCanvas.SetActive(false);

        // Show the Other Canvas
        vulBoard.SetActive(true);
    }
}
