using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class MainCanvasButton : MonoBehaviour
{
    public GameObject mainVideoCanvas;
    public GameObject welcomeVideoCanvas;
    public GameObject knowledgeIntroCanvas;

    public VideoPlayer mainVideoPlayer;
    public VideoPlayer welcomeVideoPlayer;
    public VideoPlayer knowledgeIntroVideoPlayer;

    public void Start()
    {
        // Start only knowledge intro canvas at launch
        mainVideoCanvas.SetActive(false);
        welcomeVideoCanvas.SetActive(false);
        knowledgeIntroCanvas.SetActive(true);

        if (knowledgeIntroVideoPlayer != null)
            knowledgeIntroVideoPlayer.Play();
    }

    public void StartSurvey()
    {
        // Stop knowledge intro video and proceed to survey
        if (knowledgeIntroVideoPlayer != null) knowledgeIntroVideoPlayer.Stop();
        if (knowledgeIntroCanvas != null) knowledgeIntroCanvas.SetActive(false);

        if (welcomeVideoCanvas != null) welcomeVideoCanvas.SetActive(true);
        if (welcomeVideoPlayer != null) welcomeVideoPlayer.Play();
    }

    public void ContinueToKnowledgeIntro()
    {
        // This function is now redundant since we start with knowledgeIntroCanvas
        // Keeping it for compatibility in case it's called elsewhere
        if (welcomeVideoPlayer != null) welcomeVideoPlayer.Stop();
        if (welcomeVideoCanvas != null) welcomeVideoCanvas.SetActive(false);

        if (knowledgeIntroCanvas != null) knowledgeIntroCanvas.SetActive(true);
        if (knowledgeIntroVideoPlayer != null) knowledgeIntroVideoPlayer.Play();
    }
}