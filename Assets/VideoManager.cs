using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoManager : MonoBehaviour
{
    public GameObject currentCanvas;  // Current canvas with video
    public GameObject nextCanvas;     // Canvas to show when the video is skipped
    public GameObject videoFinishedObject;  // Object to show when the video finishes
    public Button skipButton;        // Button to skip the video
    public VideoPlayer videoPlayer;  // Video player to monitor video finish
    public MyQuestionManager questionManager; // Reference to MyQuestionManager script

    private void Start()
    {
        // Add listeners for button click and video finish event
        skipButton.onClick.AddListener(OnSkipButtonClicked);
        videoPlayer.loopPointReached += OnVideoFinished;

        // Ensure score starts at 0 if the video is not skipped
        questionManager.score = 0; 
    }

    // Called when the skip button is clicked
    private void OnSkipButtonClicked()
    {
        // Deduct one point if skipped
        questionManager.score = -1;

        // Hide the current canvas and show the next canvas
        currentCanvas.SetActive(false);
        nextCanvas.SetActive(true);
    }

    // Called when the video finishes playing
    private void OnVideoFinished(VideoPlayer vp)
    {
        // Ensure score remains 0 since the video was watched
        questionManager.score = 0;

        // Hide the current canvas and show the video finished object
        currentCanvas.SetActive(false);
        videoFinishedObject.SetActive(true);
    }
}
