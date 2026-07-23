using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoTransition : MonoBehaviour
{
    public VideoPlayer videoPlayer; // Reference to the VideoPlayer component
    public Canvas currentCanvas;    // Current canvas with the video
    public Canvas nextCanvas;       // The canvas to switch to

    void Start()
    {
        // Subscribe to the loopPointReached event, which is triggered when the video finishes
        videoPlayer.loopPointReached += OnVideoFinished;
    }

    void Update()
    {
        // Optionally, you can check if the video is done manually in Update
        if (!videoPlayer.isPlaying && videoPlayer.frame == (long)videoPlayer.frameCount)
        {
            SwitchCanvas();
        }
    }

    // Called when the video finishes
    void OnVideoFinished(VideoPlayer vp)
    {
        SwitchCanvas();
    }

    // Switch to the next canvas and deactivate the current one
    void SwitchCanvas()
    {
        currentCanvas.gameObject.SetActive(false); // Hide the current canvas
        nextCanvas.gameObject.SetActive(true);     // Show the next canvas
    }
}
