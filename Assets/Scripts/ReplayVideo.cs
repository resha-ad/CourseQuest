using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video; // for video

public class ReplayVideo : MonoBehaviour
{
    public VideoPlayer videoPlayer;

    public void Replay()
    {
        if (videoPlayer != null)
        {
            videoPlayer.Stop();
            videoPlayer.Play();
        }
    }
}
