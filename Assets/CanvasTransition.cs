using UnityEngine;

public class CanvasTransition : MonoBehaviour
{
    public GameObject mainVideoCanvas;
    public GameObject welcomeVideoCanvas;
    public SceneManagerController sceneManager;

    public void TransitionToWelcomeVideo()
    {
        if (mainVideoCanvas != null && welcomeVideoCanvas != null)
        {
            mainVideoCanvas.SetActive(false);
            welcomeVideoCanvas.SetActive(true);
            Debug.Log("Transitioned from MainVideoCanvas to WelcomeVideoCanvas.");
        }
    }

    public void TransitionToClassRoom()
    {
        sceneManager.LoadClassRoomScene(() =>
        {
            SurveyManager surveyManager = FindObjectOfType<SurveyManager>();
            if (surveyManager != null)
            {
                surveyManager.StartKnowledgeQuestions();
                Debug.Log("Called StartKnowledgeQuestions in ClassRoom scene.");
            }
            else
            {
                Debug.LogError("SurveyManager not found in ClassRoom scene!");
            }
        });
    }
}