using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;
using System.Text;

public class MyQuestionManager : MonoBehaviour
{
    public MyQuestions myQuestions; // Reference to the MyQuestions script
    public TMP_Text questionTitleText; // Reference to the question title TMP
    public List<Button> optionButtons; // List of the four option buttons
    public TMP_Text[] optionTexts; // Array of TMP texts for the options (child of the buttons)
    public Button nextButton; // Reference to the next button
    public GameObject scoreCanvas; // Reference to the score Canvas
    public GameObject mainCanvas; // Reference to the main Canvas
    public TMP_Text scoreText; // Reference to the score text on the Canvas
    public GameObject passCanvas;
    public GameObject failCanvas;
    public GameObject failMusic;

    public GameObject feedbackCanvas; // Reference to the feedback Canvas
    public TMP_Text feedbackText; // Reference to the feedback TMP text
    public Button feedbackNextButton; // Reference to the feedback Next button

    private int[] answerCorrectness = new int[15]; // Stores 1 for correct and 0 for incorrect

    public List<GameObject> questionVideos; // List of video objects for each question

    public List<GameObject> feedbackImagesQ1;
    public List<GameObject> feedbackImagesQ2;
    public List<GameObject> feedbackImagesQ3;
    public List<GameObject> feedbackImagesQ4;
    public List<GameObject> feedbackImagesQ5;
    public List<GameObject> feedbackImagesQ6;
    public List<GameObject> feedbackImagesQ7;
    public List<GameObject> feedbackImagesQ8;
    public List<GameObject> feedbackImagesQ9;
    public List<GameObject> feedbackImagesQ10;
    public List<GameObject> feedbackImagesQ11;
    public List<GameObject> feedbackImagesQ12;
    public List<GameObject> feedbackImagesQ13;
    public List<GameObject> feedbackImagesQ14;
    public List<GameObject> feedbackImagesQ15;

    public AudioSource rightAnswerAudio; // AudioSource for correct answer sound
    public AudioSource wrongAnswerAudio; // AudioSource for incorrect answer sound
    public List<AudioClip> rightAnswerClips; // List of AudioClips for correct answers
    public List<AudioClip> wrongAnswerClips; // List of AudioClips for incorrect answers

    private List<MyQuestions.Question> questions; // List to hold all questions
    private int currentQuestionIndex = 0; // Track the current question index
    public int score; // Track the player's score

    private float vibrationDuration = 1.0f; // Increased duration for controller vibration
    private float vibrationFrequency = 1.0f; // Frequency range from 0 to 1 (normal maximum vibration speed)
    private float vibrationAmplitude = 1.0f; // Amplitude range from 0 to 1 (normal maximum vibration strength)

    [System.Obsolete]
    void Start()
    {
        mainCanvas.SetActive(true);
        SetupAndSignIn();
        questions = myQuestions.GetQuestions();
        SetQuestion(currentQuestionIndex);

        for (int i = 0; i < optionButtons.Count; i++)
        {
            int optionIndex = i;
            optionButtons[i].onClick.AddListener(() => OnOptionSelected(optionIndex));
        }

        feedbackNextButton.onClick.AddListener(NextQuestionAfterFeedback);
        scoreCanvas.SetActive(false);
        feedbackCanvas.SetActive(false);

        // Set all video canvases to inactive initially
        foreach (var video in questionVideos)
        {
            video.SetActive(false);
        }
        ShowVideoForCurrentQuestion();
    }

    void SetQuestion(int index)
    {
        if (index < questions.Count)
        {
            MyQuestions.Question question = questions[index];
            questionTitleText.text = question.questionText;

            for (int i = 0; i < optionButtons.Count; i++)
            {
                if (i < question.options.Count)
                {
                    optionTexts[i].text = question.options[i];
                    ResetOptionFormatting(i);
                }
            }
        }
        else
        {
            Debug.Log("No more questions available.");
        }
    }

    void ShowVideoForCurrentQuestion()
    {
        for (int i = 0; i < questionVideos.Count; i++)
        {
            questionVideos[i].SetActive(i == currentQuestionIndex);
        }
    }

    void HideAllVideos()
    {
        foreach (var video in questionVideos)
        {
            video.SetActive(false);
        }
    }

    void OnOptionSelected(int optionIndex)
    {
        if (optionIndex >= 0 && optionIndex < optionButtons.Count)
        {
            MyQuestions.Question currentQuestion = questions[currentQuestionIndex];

            if (optionIndex == currentQuestion.correctAnswerIndex)
            {
                optionButtons[optionIndex].GetComponent<Image>().color = Color.green;
                // Play random correct answer sound
                AudioClip randomRightClip = rightAnswerClips[Random.Range(0, rightAnswerClips.Count)];
                rightAnswerAudio.clip = randomRightClip;
                rightAnswerAudio.Play();
                score++;
                answerCorrectness[currentQuestionIndex] = 1; // Mark as correct
            }
            else
            {
                optionButtons[optionIndex].GetComponent<Image>().color = Color.red;
                optionButtons[currentQuestion.correctAnswerIndex].GetComponent<Image>().color = Color.green;
                // Play random wrong answer sound
                AudioClip randomWrongClip = wrongAnswerClips[Random.Range(0, wrongAnswerClips.Count)];
                wrongAnswerAudio.clip = randomWrongClip;
                wrongAnswerAudio.Play();
                StartCoroutine(VibrateController(OVRInput.Controller.All)); // Vibrate controller for wrong answer
                answerCorrectness[currentQuestionIndex] = 0; // Mark as incorrect
            }

            HideAllVideos(); // Hide videos during feedback
            StartCoroutine(HandleFeedbackWithDelay(currentQuestion.feedback[optionIndex]));
        }
    }

    IEnumerator VibrateController(OVRInput.Controller controller)
    {
        OVRInput.SetControllerVibration(vibrationFrequency, vibrationAmplitude, controller);
        yield return new WaitForSeconds(vibrationDuration);
        OVRInput.SetControllerVibration(0, 0, controller); // Stop vibration
    }

    IEnumerator HandleFeedbackWithDelay(string feedback)
    {
        foreach (Button button in optionButtons)
        {
            button.interactable = false;
        }

        yield return new WaitForSeconds(2);

        ShowFeedback(feedback);
    }

    void ShowFeedback(string feedback)
    {
        feedbackCanvas.SetActive(true);
        mainCanvas.SetActive(false);
        feedbackText.text = feedback;

        List<GameObject> currentFeedbackImages = GetFeedbackImagesForCurrentQuestion();
        if (currentFeedbackImages != null && currentFeedbackImages.Count > 0)
        {
            int randomIndex = Random.Range(0, currentFeedbackImages.Count);
            currentFeedbackImages[randomIndex].SetActive(true);
        }
    }

    [System.Obsolete]
    void NextQuestionAfterFeedback()
    {
        feedbackCanvas.SetActive(false);
        mainCanvas.SetActive(true);

        List<GameObject> currentFeedbackImages = GetFeedbackImagesForCurrentQuestion();
        if (currentFeedbackImages != null)
        {
            foreach (var image in currentFeedbackImages)
            {
                image.SetActive(false);
            }
        }

        currentQuestionIndex++;

        if (currentQuestionIndex < questions.Count)
        {
            SetQuestion(currentQuestionIndex);
            ShowVideoForCurrentQuestion(); // Show video for the next question
            foreach (Button button in optionButtons)
            {
                button.interactable = true;
            }
        }
        else
        {
            DisplayScore();
        }
    }

    void ResetOptionFormatting(int optionIndex)
    {
        optionButtons[optionIndex].GetComponent<Image>().color = Color.yellow;
    }

    IEnumerator TypewriterEffect(TMP_Text targetText, string fullText, float delay = 0.05f)
    {
        targetText.text = "";
        StringBuilder sb = new StringBuilder(); // Use StringBuilder for efficient string concatenation

        foreach (char c in fullText)
        {
            sb.Append(c);
            targetText.text = sb.ToString(); // Update the text
            yield return new WaitForSeconds(delay);
        }
    }

    [System.Obsolete]
    void DisplayScore()
    {
        scoreCanvas.SetActive(true);
        mainCanvas.SetActive(false);

        string feedbackMessage = "";

        if (score == 15)
        {
            feedbackMessage =
                "This is an outstanding achievement! Your attention to detail and understanding of security protocols are remarkable. " +
                "You're setting a great example for others. Keep up the excellent work!";

            passCanvas.SetActive(true);
            failCanvas.SetActive(false);
        }
        else if (score >= 13 && score < 15)
        {
            feedbackMessage =
                "This is a good attempt, but unfortunately, in a security environment, even minor mistakes can have significant consequences. " +
                "Please review the key areas where errors were made and strive to improve. Security is a shared responsibility, and precision matters!";

            passCanvas.SetActive(false);
            failCanvas.SetActive(true);
            failMusic.SetActive(true);
        }
        else if (score >= 10 && score <= 12)
        {
            feedbackMessage =
                "Your performance is average, but there’s significant room for improvement. Mid-level scores like this indicate some understanding " +
                "of security protocols, but gaps remain. Take time to review the feedback and identify the critical areas where you need to focus. " +
                "Remember, security vulnerabilities can have major consequences.";

            passCanvas.SetActive(false);
            failCanvas.SetActive(true);
            failMusic.SetActive(true);
        }
        else
        {
            feedbackMessage =
                "This performance is unacceptable. Security breaches can be catastrophic, and understanding proper protocols is essential. " +
                "Please take this opportunity to carefully review the material, as it's crucial to be prepared to handle potential threats. " +
                "Your role in maintaining security standards is critical, and improvement is necessary to prevent risks.";

            passCanvas.SetActive(false);
            failCanvas.SetActive(true);
            failMusic.SetActive(true);
        }

        // Use the typewriter effect to display the feedback message
        StartCoroutine(ShowScoreWithTypewriterEffect(feedbackMessage));
    }

    [System.Obsolete]
    IEnumerator ShowScoreWithTypewriterEffect(string feedbackMessage)
    {
        // Show feedback message first
        yield return StartCoroutine(TypewriterEffect(scoreText, feedbackMessage));

        // After typewriter effect finishes, show the score and save data
        SaveData();
    }

    List<GameObject> GetFeedbackImagesForCurrentQuestion()
    {
        switch (currentQuestionIndex)
        {
            case 0: return feedbackImagesQ1;
            case 1: return feedbackImagesQ2;
            case 2: return feedbackImagesQ3;
            case 3: return feedbackImagesQ4;
            case 4: return feedbackImagesQ5;
            case 5: return feedbackImagesQ6;
            case 6: return feedbackImagesQ7;
            case 7: return feedbackImagesQ8;
            case 8: return feedbackImagesQ9;
            case 9: return feedbackImagesQ10;
            case 10: return feedbackImagesQ11;
            case 11: return feedbackImagesQ12;
            case 12: return feedbackImagesQ13;
            case 13: return feedbackImagesQ14;
            case 14: return feedbackImagesQ15;
            default: return null;
        }
    }

    async void SetupAndSignIn()
    {
        await UnityServices.InitializeAsync();
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    [System.Obsolete]
    async void SaveData()
    {
        try
        {
            Dictionary<string, string> savedData = await CloudSaveService.Instance.Data.LoadAllAsync();
            int currentLevel = 420129;

            foreach (var entry in savedData)
            {
                if (int.TryParse(entry.Key, out int level))
                {
                    currentLevel = Mathf.Max(currentLevel, level);
                }
            }

            int newLevel = currentLevel + 1;

            // Create a dictionary containing both score and correctness array
            var data = new Dictionary<string, object>
            {
                { newLevel.ToString(), new Dictionary<string, object>
                    {
                        { "score", score },
                        { "answers", answerCorrectness } // Store correctness array in the same row
                    }
                }
            };

            await CloudSaveService.Instance.Data.ForceSaveAsync(data);
            scoreText.text = $"{scoreText.text}\nYour Score: {score} / {questions.Count}. \nYour score has been saved with \nUser ID: {newLevel}\n";
        }
        catch (System.Exception ex)
        {
            scoreText.text = "Error saving data. Please try again.";
            Debug.LogError("Error saving data: " + ex.Message);
        }
    }
}
