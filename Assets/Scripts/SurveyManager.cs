using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;
using System.Linq;
using System.IO;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;
using Unity.Services.CloudSave.Models;
using System;

[System.Serializable]
public class PlayerResponseList
{
    public List<string> responses;

    public PlayerResponseList(List<string> responses)
    {
        this.responses = responses;
    }
}

[System.Serializable]
public class Question
{
    public int questionID;
    public string videoFile;
    public string questionType; // "preference" or "knowledge"
    public string questionText;
    public List<AnswerOption> options;
    public string feedbackVideo;
}

[System.Serializable]
public class AnswerOption
{
    public string optionID;
    public string text;
    public string course;
    public string explanation;
    public bool isCorrect;
}

[System.Serializable]
public class QuestionList
{
    public List<Question> questions;
}

[System.Serializable]
public class AnswerRecord
{
    public int index;
    public int questionID;
    public string questionType;
    public string selectedOptionID;
    public string optionText;
    public string course;
    public int pointsEarned;
}

public class SurveyManager : MonoBehaviour
{
    public GameObject feedbackCanvas;
    public TMP_Text feedbackText;
    public Button continueButton;
    public RawImage videoDisplay;
    public GameObject videoCanvas;

    public VideoPlayer videoPlayer;
    public GameObject optionsPanel;
    public Button[] optionButtons;
    public GameObject knowledgeIntroCanvas;
    public GameObject preferenceIntroCanvas;
    public AudioSource audioSource;
    public AudioClip correctAnswerClip;
    public AudioClip wrongAnswerClip;
    public Color correctColor = Color.green;
    public Color wrongColor = Color.red;
    public Color preferenceSelectedColor = new Color(0.6f, 0.8f, 1f);
    private Color defaultButtonColor;

    public GameObject correctEffectPrefab; // Assign CorrectEffect.prefab in Inspector
    public GameObject wrongEffectPrefab;   // Assign WrongEffect.prefab in Inspector
    public GameObject preferenceEffectPrefab; // Assign PreferenceEffect.prefab in Inspector
    public GameObject confettiEffectPrefab; // Assign ConfettiEffect.prefab in Inspector

    public GameObject quizEndCanvas;
    public Button viewResultButton;
    public Button homeButton; // Reference to HomeButton on QuizEndCanvas
    public Button checkButton; // Reference to CheckButton on QuizEndCanvas
    public GameObject recommendationCanvas;
    public GameObject MainVideoCanvas;
    public VideoPlayer recommendationVideoPlayer;

    public GameObject collectiveFeedbackCanvas; // Assign CollectiveFeedbackCanvas in Inspector
    public VideoPlayer collectiveFeedbackVideoPlayer; // Assign VideoPlayer on CollectiveFeedbackCanvas
    public Button endQuizButton; // Assign EndQuizButton on CollectiveFeedbackCanvas

    public QuestionList questionList;
    private int currentQuestionIndex = 0;
    private List<Question> knowledgeQuestions = new();
    private List<Question> preferenceQuestions = new();
    private bool inKnowledgePhase = true;
    private bool isOptionSelected = false;

    public GameObject answersCanvasPart1;
    public TMP_Text answersTextPart1;
    public Button nextButtonPart1;
    public GameObject answersCanvasPart2;
    public TMP_Text answersTextPart2;
    public Button nextButtonPart2;
    public GameObject pointsCanvas;
    public TMP_Text pointsText;

    private List<AnswerRecord> answerRecords = new();
    private List<string> cloudSaveAnswers = new();
    private Dictionary<string, int> coursePoints = new()
    {
        { "AI", 0 },
        { "Computing", 0 },
        { "Cybersecurity", 0 }
    };
    private int answerIndex = 0;
    private bool isCloudSaveInitialized = false;
    private bool isSaving = false;

    public SceneManagerController sceneManager;

    async void Start()
    {
        try
        {
            await UnityServices.InitializeAsync();
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            isCloudSaveInitialized = true;
            Debug.Log("Unity Services and Authentication initialized successfully. Player ID: " + AuthenticationService.Instance.PlayerId);
        }
        catch (ServicesInitializationException e)
        {
            Debug.LogError("Failed to initialize Unity Services: " + e.Message);
        }
        catch (AuthenticationException e)
        {
            Debug.LogError("Authentication failed: " + e.Message);
        }

        LoadJSON();
        SplitQuestions();
        preferenceIntroCanvas.SetActive(false);
        videoCanvas.SetActive(false);
        optionsPanel.SetActive(false);
        answersCanvasPart1.SetActive(false);
        answersCanvasPart2.SetActive(false);
        pointsCanvas.SetActive(false);
        feedbackCanvas.SetActive(false);
        quizEndCanvas.SetActive(false);
        recommendationCanvas.SetActive(false);
        collectiveFeedbackCanvas.SetActive(false);
        MainVideoCanvas.SetActive(false);
        knowledgeIntroCanvas.SetActive(true);
        Debug.Log("SurveyManager: Initial canvas states set for ClassRoom scene with KnowledgeIntroCanvas active.");

        if (sceneManager == null)
        {
            sceneManager = FindObjectOfType<SceneManagerController>();
            if (sceneManager == null)
                Debug.LogError("SceneManagerController not found in scene!");
        }

        if (optionButtons.Length > 0)
            defaultButtonColor = optionButtons[0].GetComponent<Image>().color;

        ResetQuizData();

        if (checkButton != null)
        {
            checkButton.onClick.RemoveAllListeners();
            checkButton.onClick.AddListener(ShowPointsCanvas);
        }
        else
        {
            Debug.LogWarning("CheckButton not assigned in Inspector!");
        }
    }

    void LoadJSON()
    {
        TextAsset jsonText = Resources.Load<TextAsset>("vr_survey_questions");
        if (jsonText != null)
        {
            questionList = JsonUtility.FromJson<QuestionList>(jsonText.text);
            Debug.Log($"Loaded {questionList?.questions?.Count ?? 0} questions from vr_survey_questions.json.");
            if (questionList == null || questionList.questions == null || questionList.questions.Count == 0)
            {
                Debug.LogError("Failed to parse JSON or no questions found!");
            }
            foreach (var q in questionList?.questions ?? new List<Question>())
            {
                if (q.options != null)
                {
                    foreach (var opt in q.options)
                    {
                        if (string.IsNullOrEmpty(opt.optionID))
                        {
                            Debug.LogWarning($"Missing optionID for Q{q.questionID}, Option: {opt.text}");
                        }
                    }
                }
            }
        }
        else
        {
            Debug.LogError("JSON file 'vr_survey_questions' not found in Resources!");
        }
    }

    void SplitQuestions()
    {
        knowledgeQuestions.Clear();
        preferenceQuestions.Clear();
        if (questionList?.questions == null)
        {
            Debug.LogError("No questions to split!");
            return;
        }

        foreach (var q in questionList.questions)
        {
            if (q == null || string.IsNullOrEmpty(q.questionType))
            {
                Debug.LogWarning($"Invalid question detected: ID={q?.questionID}, Type={q?.questionType}");
                continue;
            }
            bool allOptionsValid = true;
            if (q.options == null || q.options.Count == 0)
            {
                Debug.LogWarning($"Q{q.questionID} has no options!");
                allOptionsValid = false;
            }
            else
            {
                foreach (var opt in q.options)
                {
                    if (string.IsNullOrEmpty(opt.optionID) || !new[] { "A", "B", "C" }.Contains(opt.optionID))
                    {
                        Debug.LogWarning($"Invalid or missing optionID for Q{q.questionID}, Option: {opt.text}, optionID: {opt.optionID}");
                        allOptionsValid = false;
                    }
                }
            }

            if (allOptionsValid)
            {
                if (q.questionType == "knowledge")
                    knowledgeQuestions.Add(q);
                else if (q.questionType == "preference")
                    preferenceQuestions.Add(q);
                else
                    Debug.LogWarning($"Unknown question type for Q{q.questionID}: {q.questionType}");
            }
            else
            {
                Debug.LogWarning($"Skipping Q{q.questionID} due to invalid options.");
            }
        }
        Debug.Log($"Split questions: {knowledgeQuestions.Count} knowledge, {preferenceQuestions.Count} preference, Total: {knowledgeQuestions.Count + preferenceQuestions.Count}");
        if (knowledgeQuestions.Count + preferenceQuestions.Count != 30)
        {
            Debug.LogWarning($"Expected 30 questions, but got {knowledgeQuestions.Count + preferenceQuestions.Count}. Check JSON for errors.");
        }
    }

    public void StartKnowledgeQuestions()
    {
        sceneManager.LoadClassRoomScene(() =>
        {
            knowledgeIntroCanvas.SetActive(false);
            videoCanvas.SetActive(true);
            currentQuestionIndex = 0;
            inKnowledgePhase = true;
            Debug.Log($"Starting knowledge questions: {knowledgeQuestions.Count} questions available.");
            ShowQuestion();
        });
    }

    public void StartPreferenceQuestions()
    {
        preferenceIntroCanvas.SetActive(false);
        videoCanvas.SetActive(true);
        currentQuestionIndex = 0;
        inKnowledgePhase = false;
        Debug.Log($"Starting preference questions: {preferenceQuestions.Count} questions available.");
        ShowQuestion();
    }

    void ShowQuestion()
    {
        isOptionSelected = false;

        List<Question> currentList = inKnowledgePhase ? knowledgeQuestions : preferenceQuestions;

        if (currentQuestionIndex >= currentList.Count)
        {
            if (inKnowledgePhase)
            {
                Debug.Log($"Knowledge section complete. Recorded {answerRecords.Count} answers: [{string.Join(", ", answerRecords.Select(r => $"{r.questionID}:{r.selectedOptionID}"))}]");
                Debug.Log($"Cloud save answers: [{string.Join(", ", cloudSaveAnswers)}]");
                videoDisplay.transform.parent.gameObject.SetActive(false);
                optionsPanel.SetActive(false);
                preferenceIntroCanvas.SetActive(true);
                videoCanvas.SetActive(false);
            }
            else
            {
                Debug.Log($"Preference section complete. Recorded {answerRecords.Count} answers: [{string.Join(", ", answerRecords.Select(r => $"{r.questionID}:{r.selectedOptionID}"))}]");
                Debug.Log($"Cloud save answers: [{string.Join(", ", cloudSaveAnswers)}]");
                videoCanvas.SetActive(false);
                ShowCollectiveFeedback();
            }
            return;
        }

        Question q = currentList[currentQuestionIndex];
        if (q == null)
        {
            currentQuestionIndex++;
            ShowQuestion();
            return;
        }

        string videoPath = Path.Combine(Application.streamingAssetsPath, q.videoFile);
        videoPlayer.url = videoPath;
        videoPlayer.Prepare();
        videoPlayer.prepareCompleted += (source) => videoPlayer.Play();
        Debug.Log($"Loading question {currentQuestionIndex + 1}/{currentList.Count} (Q{q.questionID}, {q.questionType}): {q.questionText}");

        for (int i = 0; i < optionButtons.Length; i++)
        {
            if (i >= q.options.Count)
            {
                optionButtons[i].gameObject.SetActive(false);
                continue;
            }

            optionButtons[i].gameObject.SetActive(true);
            optionButtons[i].interactable = true;
            TextMeshProUGUI btnText = optionButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            btnText.text = q.options[i].text;
            optionButtons[i].GetComponent<Image>().color = defaultButtonColor;

            int index = i;
            optionButtons[i].onClick.RemoveAllListeners();
            optionButtons[i].onClick.AddListener(() =>
            {
                if (!isOptionSelected)
                {
                    isOptionSelected = true;
                    Debug.Log($"Button {index} clicked for Q{q.questionID} ({q.questionType})");
                    OnOptionSelected(q, q.options[index], index);
                }
            });
        }

        videoDisplay.transform.parent.gameObject.SetActive(true);
        optionsPanel.SetActive(true);
    }

    void OnOptionSelected(Question question, AnswerOption selectedOption, int selectedIndex)
    {
        if (question == null || selectedOption == null)
        {
            Debug.LogError($"OnOptionSelected: Question or selectedOption is null! QID={question?.questionID}, OptionID={selectedOption?.optionID}");
            isOptionSelected = false;
            return;
        }

        if (string.IsNullOrEmpty(selectedOption.optionID) || !new[] { "A", "B", "C" }.Contains(selectedOption.optionID))
        {
            Debug.LogError($"Invalid optionID for Q{question.questionID} ({question.questionType}): {selectedOption.optionID}. Expected 'A', 'B', or 'C'.");
            isOptionSelected = false;
            return;
        }

        Debug.Log($"Option selected for Q{question.questionID} ({question.questionType}): OptionID={selectedOption.optionID}, Text={selectedOption.text}, Course={selectedOption.course}, IsCorrect={selectedOption.isCorrect}");

        cloudSaveAnswers.Add(selectedOption.optionID);
        Debug.Log($"Added to cloudSaveAnswers for Q{question.questionID} ({question.questionType}): {selectedOption.optionID}, Total: {cloudSaveAnswers.Count} [{string.Join(", ", cloudSaveAnswers)}]");

        foreach (var button in optionButtons)
            button.interactable = false;

        if (question.questionType == "knowledge")
        {
            if (selectedOption.isCorrect)
            {
                PlaySound(correctAnswerClip);
                optionButtons[selectedIndex].GetComponent<Image>().color = correctColor;
                AddPointsToCourse(selectedOption.course, 2);
                RecordAnswer(question.questionID, question.questionType, selectedOption.optionID, selectedOption.text, selectedOption.course, 2);
                if (correctEffectPrefab != null)
                {
                    GameObject effect = Instantiate(correctEffectPrefab, optionButtons[selectedIndex].transform.position, Quaternion.identity);
                    Destroy(effect, 3f);
                }
                StartCoroutine(NextQuestionDelay(3f));
            }
            else
            {
                PlaySound(wrongAnswerClip);
                OVRInput.SetControllerVibration(1, 1, OVRInput.Controller.RTouch);
                HighlightCorrectAndWrongOptions(question, selectedIndex);
                if (wrongEffectPrefab != null)
                {
                    GameObject effect = Instantiate(wrongEffectPrefab, optionButtons[selectedIndex].transform.position, Quaternion.identity);
                    Destroy(effect, 3f);
                }
                StartCoroutine(StopVibration(2f));
                StartCoroutine(NextQuestionDelay(3f));
                RecordAnswer(question.questionID, question.questionType, selectedOption.optionID, selectedOption.text, selectedOption.course, 0);
            }
        }
        else
        {
            PlaySound(correctAnswerClip);
            optionButtons[selectedIndex].GetComponent<Image>().color = preferenceSelectedColor;
            AddPointsToCourse(selectedOption.course, 1);
            RecordAnswer(question.questionID, question.questionType, selectedOption.optionID, selectedOption.text, selectedOption.course, 1);
            if (preferenceEffectPrefab != null)
            {
                GameObject effect = Instantiate(preferenceEffectPrefab, optionButtons[selectedIndex].transform.position, Quaternion.identity);
                Destroy(effect, 3f);
                Debug.Log($"Preference effect instantiated for Q{question.questionID}, Option: {selectedOption.text}");
            }
            StartCoroutine(NextQuestionDelay(1.5f));
        }
    }

    void ShowCollectiveFeedback()
    {
        collectiveFeedbackCanvas.SetActive(true);
        videoCanvas.SetActive(false);

        var topCourses = coursePoints.OrderByDescending(x => x.Value).Take(2).Select(x => x.Key).ToList();
        string course1 = topCourses.Count > 0 ? topCourses[0] : "None";
        string course2 = topCourses.Count > 1 ? topCourses[1] : "None";

        string videoName;
        if (course1 == "AI" && course2 == "Computing")
            videoName = "Preference_Feedback_AI_Computing.mp4";
        else if (course1 == "Computing" && course2 == "AI")
            videoName = "Preference_Feedback_Computing_AI.mp4";
        else if (course1 == "AI" && course2 == "Cybersecurity")
            videoName = "Preference_Feedback_AI_Cybersecurity.mp4";
        else if (course1 == "Cybersecurity" && course2 == "AI")
            videoName = "Preference_Feedback_Cybersecurity_AI.mp4";
        else if (course1 == "Computing" && course2 == "Cybersecurity")
            videoName = "Preference_Feedback_Computing_Cybersecurity.mp4";
        else if (course1 == "Cybersecurity" && course2 == "Computing")
            videoName = "Preference_Feedback_Cybersecurity_Computing.mp4";
        else
            videoName = "Preference_Feedback_AI_Computing.mp4";

        string videoPath = Path.Combine(Application.streamingAssetsPath, videoName);
        collectiveFeedbackVideoPlayer.url = videoPath;
        collectiveFeedbackVideoPlayer.Prepare();
        collectiveFeedbackVideoPlayer.prepareCompleted += (source) => collectiveFeedbackVideoPlayer.Play();
        Debug.Log($"Playing collective feedback video: {videoName}");

        endQuizButton.onClick.RemoveAllListeners();
        endQuizButton.onClick.AddListener(() =>
        {
            collectiveFeedbackCanvas.SetActive(false);
            ShowQuizEndCanvas();
        });
    }

    void HighlightCorrectAndWrongOptions(Question question, int selectedIndex)
    {
        for (int i = 0; i < question.options.Count && i < optionButtons.Length; i++)
        {
            var option = question.options[i];
            var buttonImage = optionButtons[i].GetComponent<Image>();
            if (i == selectedIndex && !option.isCorrect)
            {
                buttonImage.color = wrongColor;
            }
            else if (option.isCorrect)
            {
                buttonImage.color = correctColor;
            }
            else
            {
                buttonImage.color = defaultButtonColor;
            }
        }
    }

    IEnumerator NextQuestionDelay(float seconds)
    {
        Debug.Log($"Waiting {seconds}s before next question...");
        yield return new WaitForSeconds(seconds);
        currentQuestionIndex++;
        ShowQuestion();
    }

    IEnumerator StopVibration(float delay)
    {
        yield return new WaitForSeconds(delay);
        OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.RTouch);
    }

    void AddPointsToCourse(string course, int amount)
    {
        if (!string.IsNullOrEmpty(course) && coursePoints.ContainsKey(course))
        {
            coursePoints[course] += amount;
            Debug.Log($"Added {amount} point(s) to {course}. Total now: {coursePoints[course]}");
        }
    }

    void RecordAnswer(int questionID, string questionType, string selectedOptionID, string optionText, string course, int pointsEarned)
    {
        if (string.IsNullOrEmpty(selectedOptionID) || !new[] { "A", "B", "C" }.Contains(selectedOptionID))
        {
            Debug.LogWarning($"Recording answer with invalid selectedOptionID for Q{questionID} ({questionType}): {selectedOptionID}. Expected 'A', 'B', or 'C'.");
            selectedOptionID = "UNKNOWN";
        }
        answerRecords.Add(new AnswerRecord
        {
            index = ++answerIndex,
            questionID = questionID,
            questionType = questionType,
            selectedOptionID = selectedOptionID,
            optionText = optionText,
            course = course,
            pointsEarned = pointsEarned
        });
        Debug.Log($"Recorded answer #{answerIndex} for Q{questionID} ({questionType}): {selectedOptionID} - {optionText}, Course: {course}, Points: {pointsEarned}, Total answers: {answerRecords.Count}");
    }

    void ShowPointsCanvas()
    {
        quizEndCanvas.SetActive(false);
        pointsCanvas.SetActive(true);
        PlayerResponseList wrappedResponse = new PlayerResponseList(cloudSaveAnswers);
        string answersJson = JsonUtility.ToJson(wrappedResponse, true);
        pointsText.text = $"Your Answers:\n{answersJson}\n\n" +
                         $"Points - AI: {coursePoints["AI"]}, Computing: {coursePoints["Computing"]}, Cybersecurity: {coursePoints["Cybersecurity"]}";
        Debug.Log($"PointsCanvas displayed with answers JSON: {answersJson}");
        OnPointsCanvasShown();
    }

    void ShowQuizEndCanvas()
    {
        quizEndCanvas.SetActive(true);
        viewResultButton.onClick.RemoveAllListeners();
        viewResultButton.onClick.AddListener(ShowRecommendation);
        homeButton.onClick.RemoveAllListeners();
        homeButton.onClick.AddListener(ReturnToMainVideo);
        checkButton.onClick.RemoveAllListeners();
        checkButton.onClick.AddListener(ShowPointsCanvas);
        Debug.Log($"Showing QuizEndCanvas, answerRecords count: {answerRecords.Count}, Cloud save answers: [{string.Join(", ", cloudSaveAnswers)}]");
        SaveProgressToCloud();
        if (confettiEffectPrefab != null)
        {
            GameObject confetti = Instantiate(confettiEffectPrefab, quizEndCanvas.transform.position, Quaternion.identity);
            Destroy(confetti, 5f);
            Debug.Log("Confetti effect instantiated on QuizEndCanvas.");
        }
        Debug.Log("QuizEndCanvas activated with ViewResultButton, HomeButton, and CheckButton listeners.");
    }

    void ShowRecommendation()
    {
        quizEndCanvas.SetActive(false);
        recommendationCanvas.SetActive(true);

        int maxPoints = coursePoints.Values.Max();
        var topCourses = coursePoints.Where(x => x.Value == maxPoints).Select(x => x.Key).ToList();

        string recommendedCourse = topCourses.Count > 1
            ? topCourses[UnityEngine.Random.Range(0, topCourses.Count)]
            : topCourses.FirstOrDefault() ?? "Computing";

        Debug.Log($"Recommendation: {topCourses.Count} course(s) tied with {maxPoints} points: [{string.Join(", ", topCourses)}]. Selected: {recommendedCourse}");

        string videoPath = Path.Combine(Application.streamingAssetsPath, $"{recommendedCourse}_Recommendation.mp4");
        recommendationVideoPlayer.url = videoPath;
        recommendationVideoPlayer.Prepare();
        recommendationVideoPlayer.prepareCompleted += (source) => recommendationVideoPlayer.Play();
        Debug.Log($"Playing recommendation video for {recommendedCourse}: {videoPath}");
    }

    void ReturnToMainVideo()
    {
        sceneManager.LoadTechLabScene(() =>
        {
            quizEndCanvas.SetActive(false);
            recommendationCanvas.SetActive(false);
            pointsCanvas.SetActive(false);
            GameObject mainCanvas = GameObject.Find("MainVideoCanvas");
            if (mainCanvas != null)
            {
                mainCanvas.SetActive(true);
                Debug.Log("Returned to MainVideoCanvas in TechLab scene.");
            }
            else
            {
                Debug.LogError("MainVideoCanvas not found in TechLab scene!");
            }
            ResetQuizData();
        });
    }

    void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    void ResetQuizData()
    {
        answerRecords.Clear();
        cloudSaveAnswers.Clear();
        answerIndex = 0;
        coursePoints["AI"] = 0;
        coursePoints["Computing"] = 0;
        coursePoints["Cybersecurity"] = 0;
        currentQuestionIndex = 0;
        inKnowledgePhase = true;
        isOptionSelected = false;
        Debug.Log("Quiz data reset: answerRecords, cloudSaveAnswers, answerIndex, coursePoints, currentQuestionIndex, inKnowledgePhase, isOptionSelected.");
    }

    public void OnPointsCanvasShown()
    {
        Debug.Log($"OnPointsCanvasShown called, answerRecords count: {answerRecords.Count}, Cloud save answers: [{string.Join(", ", cloudSaveAnswers)}]");
        if (!isSaving)
            SaveProgressToCloud();
    }

    public async void SaveProgressToCloud()
    {
        if (!isCloudSaveInitialized)
        {
            Debug.LogError("Cloud Save not initialized.");
            return;
        }

        if (isSaving)
        {
            Debug.LogWarning("SaveProgressToCloud already in progress, skipping.");
            return;
        }

        if (cloudSaveAnswers.Count != 30)
        {
            Debug.LogError($"Expected 30 answers in cloudSaveAnswers, but got {cloudSaveAnswers.Count}. Aborting save. Answers: [{string.Join(", ", cloudSaveAnswers)}]");
            return;
        }

        foreach (var answer in cloudSaveAnswers)
        {
            if (string.IsNullOrEmpty(answer) || !new[] { "A", "B", "C" }.Contains(answer))
            {
                Debug.LogError($"Invalid answer in cloudSaveAnswers: {answer}. Expected 'A', 'B', or 'C'. Aborting save.");
                return;
            }
        }

        isSaving = true;

        try
        {
            string responseKey = Guid.NewGuid().ToString();
            Debug.Log($"Preparing to save {cloudSaveAnswers.Count} answers: [{string.Join(", ", cloudSaveAnswers)}]");

            PlayerResponseList wrappedResponse = new PlayerResponseList(cloudSaveAnswers);
            string newResponseJson = JsonUtility.ToJson(wrappedResponse);
            if (string.IsNullOrEmpty(newResponseJson) || newResponseJson == "{}")
            {
                Debug.LogError($"JSON serialization failed, got: {newResponseJson}");
                isSaving = false;
                return;
            }

            Debug.Log($"Saving response with key {responseKey}: {newResponseJson}");

            await CloudSaveService.Instance.Data.Player.SaveAsync(new Dictionary<string, object>
            {
                { responseKey, newResponseJson }
            });

            Debug.Log($"✅ Saved response as ID {responseKey} with {cloudSaveAnswers.Count} answers: {newResponseJson}");

#if UNITY_EDITOR
            string debugJson = JsonUtility.ToJson(wrappedResponse, true);
            string debugFilePath = Path.Combine(Application.persistentDataPath, $"survey_results_{responseKey}.json");
            File.WriteAllText(debugFilePath, debugJson);
            Debug.Log($"📁 Saved debug JSON locally at: {debugFilePath}");
#endif
        }
        catch (Exception e)
        {
            Debug.LogError($"❌ Failed to save to Cloud Save: {e.Message}\nStack Trace: {e.StackTrace}");
        }
        finally
        {
            isSaving = false;
        }
    }
}