using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnswerCanvas2Manager : MonoBehaviour
{
    public Button continueButton2; // Reference to the continue button (nextButtonPart2)
    public GameObject answersCanvasPart2; // Reference to the answersCanvasPart2
    public GameObject pointsCanvas; // Reference to the pointsCanvas
    public SurveyManager surveyManager; // Reference to SurveyManager for Cloud Save

    void Start()
    {
        // Ensure the button has a listener for the click event
        if (continueButton2 != null)
        {
            continueButton2.onClick.RemoveAllListeners();
            continueButton2.onClick.AddListener(OnContinueButtonClicked);
            Debug.Log("continueButton2 listener set up. Button interactable: " + continueButton2.interactable);
        }
        else
        {
            Debug.LogError("ContinueButton2 is not assigned in the Inspector!");
        }

        // Verify canvas references
        if (answersCanvasPart2 == null)
        {
            Debug.LogError("AnswersCanvasPart2 is not assigned in the Inspector!");
        }
        if (pointsCanvas == null)
        {
            Debug.LogError("PointsCanvas is not assigned in the Inspector!");
        }
        if (surveyManager == null)
        {
            Debug.LogError("SurveyManager is not assigned in the Inspector!");
        }
    }

    void OnContinueButtonClicked()
    {
        Debug.Log("ContinueButton2 clicked! answersCanvasPart2: " + (answersCanvasPart2 != null) + ", pointsCanvas: " + (pointsCanvas != null));
        if (answersCanvasPart2 != null)
        {
            answersCanvasPart2.SetActive(false);
            Debug.Log("answersCanvasPart2 deactivated: " + !answersCanvasPart2.activeSelf);
        }
        if (pointsCanvas != null)
        {
            pointsCanvas.SetActive(true);
            Debug.Log("pointsCanvas activated: " + pointsCanvas.activeSelf);
        }
        if (surveyManager != null)
        {
            surveyManager.OnPointsCanvasShown();
            Debug.Log("Called SurveyManager.OnPointsCanvasShown for Cloud Save.");
        }
        else
        {
            Debug.LogError("SurveyManager is not assigned; cannot trigger Cloud Save!");
        }
    }
}
