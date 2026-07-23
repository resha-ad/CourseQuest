using UnityEngine;
using UnityEngine.Video;

public class VideoFinishManager : MonoBehaviour
{
    public GameObject currentCanvas;        // Current canvas with video
    public GameObject videoFinishedObject;  // Object to show when the video finishes
    public VideoPlayer videoPlayer;        // Video player to monitor video finish

    private void Start()
    {
        // Add listener for the video finish event
        videoPlayer.loopPointReached += OnVideoFinished;
    }

    // Called when the video finishes playing
    private void OnVideoFinished(VideoPlayer vp)
    {
        // Hide the current canvas and show the video finished object
        currentCanvas.SetActive(false);
        videoFinishedObject.SetActive(true);
    }
}
