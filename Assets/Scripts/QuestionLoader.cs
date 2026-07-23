using UnityEngine;
using System.Collections.Generic;

using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;


public class QuestionLoader : MonoBehaviour
{
    public SurveyManager surveyManager;
    public GameObject q1VideoCanvas; // Reference to Q1VideoCanvas
    public RawImage videoDisplay;
    public VideoPlayer videoPlayer;

    public Button[] optionButtons; // Drag all 4 here in Inspector
    public TMP_Text[] optionTexts; // Drag Text (TMP) children of buttons
    public GameObject feedbackCanvas;
    public VideoPlayer feedbackVideoPlayer;

    private int currentQuestionIndex = 0;

    private Dictionary<int, string> feedbackVideos = new Dictionary<int, string>()
{
    { 4, "Feedback_Q4.mp4" },
    { 7, "Feedback_Q7.mp4" },
    { 10, "Feedback_Q10.mp4" },
    // Add more as needed
};

    void Start()
    {
        LoadQuestion(currentQuestionIndex);
    }

    void LoadQuestion(int index)
    {
        Question question = surveyManager.questionList.questions[index];

        // Load and play video
        string videoPath = System.IO.Path.Combine(Application.streamingAssetsPath, question.videoFile);
        videoPlayer.url = videoPath;
        videoPlayer.Play();

        // Set button texts
        for (int i = 0; i < optionButtons.Length; i++)
        {
            int capturedIndex = i;
            optionTexts[i].text = question.options[i].text;

            optionButtons[i].onClick.RemoveAllListeners();
            optionButtons[i].onClick.AddListener(() =>
            {
                HandleAnswer(question, capturedIndex);
            });
        }
    }

    void HandleAnswer(Question question, int selectedIndex)
    {
        var selectedOption = question.options[selectedIndex];

        if (question.questionType == "knowledge")
        {
            if (selectedOption.isCorrect)
            {
                GoToNextQuestion();
            }
            else
            {
                ShowFeedback(question.questionID); // <-- Pass questionID
            }
        }
        else
        {
            GoToNextQuestion();
        }
    }

    void ShowFeedback(int questionID)
    {
        if (feedbackVideos.ContainsKey(questionID))
        {
            string path = System.IO.Path.Combine(Application.streamingAssetsPath, feedbackVideos[questionID]);
            feedbackVideoPlayer.url = path;

            feedbackCanvas.SetActive(true);
            feedbackVideoPlayer.Play();

            // Wait until the video ends or set a fixed delay
            Invoke("HideFeedbackAndNext", 5f); // Change time based on video length if needed
        }
        else
        {
            Debug.LogWarning("No feedback video found for this question.");
            GoToNextQuestion(); // fallback
        }
    }


    void HideFeedbackAndNext()
    {
        feedbackCanvas.SetActive(false);
        GoToNextQuestion();
    }

    void GoToNextQuestion()
    {
        currentQuestionIndex++;
        if (currentQuestionIndex < surveyManager.questionList.questions.Count)
        {
            LoadQuestion(currentQuestionIndex);
        }
        else
        {
            Debug.Log("Survey Complete!");
        }
    }
}