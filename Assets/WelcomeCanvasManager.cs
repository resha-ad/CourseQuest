using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class WelcomeCanvasManager : MonoBehaviour
{
    public GameObject welcomeVideoCanvas;
    public GameObject knowledgeIntroCanvas;

    // This function will be called by the NextButton
    public void GoToQuestionCanvas()
    {
        welcomeVideoCanvas.SetActive(false);
        knowledgeIntroCanvas.SetActive(true);
    }
}