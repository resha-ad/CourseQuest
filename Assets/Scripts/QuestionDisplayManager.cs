// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;
// using UnityEngine.Video;
// using TMPro;
// using System.IO;

// public class QuestionDisplayManager : MonoBehaviour
// {
//     public GameObject videoCanvas; // Should be Q1VideoCanvas
//     public RawImage videoDisplay;
//     public VideoPlayer videoPlayer;
//     public GameObject optionsPanel;
//     public Button[] optionButtons;
//     public GameObject knowledgeIntroCanvas;
//     public GameObject preferenceIntroCanvas;
//     public AudioSource audioSource;
//     public AudioClip correctAnswerClip;
//     public AudioClip wrongAnswerClip;
//     public Color correctColor = Color.green;
//     public Color wrongColor = Color.red;
//     public Color preferenceSelectedColor = new Color(0.6f, 0.8f, 1f);
//     private Color defaultButtonColor;
//     public GameObject correctEffectPrefab;
//     public GameObject wrongEffectPrefab;

//     [System.Serializable]
//     public class Question
//     {
//         public int questionID;
//         public string videoFile;
//         public string questionType;
//         public string questionText;
//         public List<AnswerOption> options;
//         public string feedbackVideo;
//     }

//     [System.Serializable]
//     public class AnswerOption
//     {
//         public string optionID;
//         public string text;
//         public string course;
//         public string explanation;
//         public bool isCorrect;
//     }

//     [System.Serializable]
//     public class QuestionList
//     {
//         public List<Question> questions;
//     }

//     private QuestionList questionList = new QuestionList();
//     private List<Question> knowledgeQuestions = new();
//     private List<Question> preferenceQuestions = new();
//     private int currentQuestionIndex = 0;
//     private bool inKnowledgePhase = true;
//     private bool isOptionSelected = false;

//     void Start()
//     {
//         Debug.Log("QuestionDisplayManager initialized on " + gameObject.name);
//         LoadJSON();
//         SplitQuestions();
//         videoCanvas.SetActive(false);
//         optionsPanel.SetActive(false);
//         if (optionButtons.Length > 0)
//             defaultButtonColor = optionButtons[0].GetComponent<Image>().color;
//     }

//     void LoadJSON()
//     {
//         TextAsset jsonText = Resources.Load<TextAsset>("vr_survey_questions");
//         if (jsonText != null)
//         {
//             questionList = JsonUtility.FromJson<QuestionList>(jsonText.text);
//             Debug.Log("Loaded " + (questionList?.questions?.Count ?? 0) + " questions.");
//         }
//         else
//         {
//             Debug.LogError("JSON file 'vr_survey_questions' not found in Resources folder!");
//         }
//     }

//     void SplitQuestions()
//     {
//         if (questionList != null && questionList.questions != null)
//         {
//             knowledgeQuestions.Clear();
//             preferenceQuestions.Clear();
//             foreach (var q in questionList.questions)
//             {
//                 if (q != null && q.questionType == "knowledge") knowledgeQuestions.Add(q);
//                 else if (q != null && q.questionType == "preference") preferenceQuestions.Add(q);
//             }
//             Debug.Log($"Split {knowledgeQuestions.Count} knowledge and {preferenceQuestions.Count} preference questions.");
//         }
//         else
//         {
//             Debug.LogError("QuestionList or questions list is null or invalid!");
//         }
//     }

//     public void StartKnowledgeQuizButton()
//     {
//         if (knowledgeIntroCanvas != null) knowledgeIntroCanvas.SetActive(false);
//         if (knowledgeQuestions.Count > 0)
//         {
//             inKnowledgePhase = true;
//             currentQuestionIndex = 0;
//             videoCanvas.SetActive(true);
//             ShowQuestion();
//             Debug.Log("Knowledge quiz started.");
//         }
//         else
//         {
//             Debug.LogError("No knowledge questions available to start quiz!");
//         }
//     }

//     public void StartPreferenceQuizButton()
//     {
//         if (preferenceIntroCanvas != null) preferenceIntroCanvas.SetActive(false);
//         if (preferenceQuestions.Count > 0)
//         {
//             inKnowledgePhase = false;
//             currentQuestionIndex = 0;
//             videoCanvas.SetActive(true);
//             ShowQuestion();
//             Debug.Log("Preference quiz started.");
//         }
//         else
//         {
//             Debug.LogError("No preference questions available to start quiz!");
//         }
//     }

//     void ShowQuestion()
//     {
//         isOptionSelected = false;
//         Debug.Log($"Displaying question. Current list size: {(inKnowledgePhase ? knowledgeQuestions.Count : preferenceQuestions.Count)}, currentIndex: {currentQuestionIndex}");

//         List<Question> currentList = inKnowledgePhase ? knowledgeQuestions : preferenceQuestions;
//         if (currentQuestionIndex >= currentList.Count)
//         {
//             videoCanvas.SetActive(false);
//             if (inKnowledgePhase && preferenceIntroCanvas != null)
//             {
//                 preferenceIntroCanvas.SetActive(true); // Open PreferenceIntroCanvas after knowledge questions
//                 Debug.Log("Knowledge questions complete, opening PreferenceIntroCanvas.");
//             }
//             else if (!inKnowledgePhase && FindObjectOfType<SurveyManager>() != null)
//             {
//                 FindObjectOfType<SurveyManager>().OnQuizComplete();
//                 Debug.Log("Preference questions complete, notifying SurveyManager.");
//             }
//             return;
//         }

//         Question q = currentList[currentQuestionIndex];

//         string videoPath = System.IO.Path.Combine(Application.streamingAssetsPath, q.videoFile);
//         if (File.Exists(videoPath) && videoPlayer != null)
//         {
//             videoPlayer.renderMode = VideoRenderMode.APIOnly;
//             videoPlayer.targetTexture = videoDisplay.texture as RenderTexture ?? new RenderTexture(Screen.width, Screen.height, 24);
//             videoPlayer.url = videoPath;
//             videoPlayer.Prepare();
//             videoPlayer.prepareCompleted += (source) =>
//             {
//                 videoPlayer.Play();
//                 Debug.Log($"Playing video: {videoPath}");
//             };
//         }
//         else
//         {
//             Debug.LogError($"Video file not found: {videoPath} or videoPlayer not assigned");
//         }

//         for (int i = 0; i < optionButtons.Length; i++)
//         {
//             if (i >= q.options.Count)
//             {
//                 if (optionButtons[i] != null) optionButtons[i].gameObject.SetActive(false);
//                 continue;
//             }

//             if (optionButtons[i] != null)
//             {
//                 optionButtons[i].gameObject.SetActive(true);
//                 optionButtons[i].interactable = true;
//                 TextMeshProUGUI btnText = optionButtons[i].GetComponentInChildren<TextMeshProUGUI>();
//                 if (btnText != null) btnText.text = q.options[i].text;
//                 optionButtons[i].GetComponent<Image>().color = defaultButtonColor;
//             }

//             int index = i;
//             optionButtons[i].onClick.RemoveAllListeners();
//             optionButtons[i].onClick.AddListener(() =>
//             {
//                 if (!isOptionSelected)
//                 {
//                     isOptionSelected = true;
//                     OnOptionSelected(q, q.options[index], index);
//                 }
//             });
//         }

//         if (videoCanvas != null && videoDisplay != null && videoDisplay.transform.parent != null)
//             videoCanvas.SetActive(true);
//         if (optionsPanel != null) optionsPanel.SetActive(true);
//     }

//     void OnOptionSelected(Question question, AnswerOption selectedOption, int selectedIndex)
//     {
//         if (optionButtons != null)
//         {
//             foreach (var button in optionButtons)
//                 if (button != null) button.interactable = false;
//         }

//         if (question.questionType == "knowledge")
//         {
//             if (selectedOption.isCorrect)
//             {
//                 PlaySound(correctAnswerClip);
//                 if (optionButtons[selectedIndex] != null) optionButtons[selectedIndex].GetComponent<Image>().color = correctColor;
//                 if (correctEffectPrefab != null)
//                 {
//                     GameObject effect = Instantiate(correctEffectPrefab, optionButtons[selectedIndex].transform.position, Quaternion.identity);
//                     Destroy(effect, 1.5f);
//                 }
//             }
//             else
//             {
//                 PlaySound(wrongAnswerClip);
//                 OVRInput.SetControllerVibration(1, 1, OVRInput.Controller.RTouch);
//                 HighlightCorrectAndWrongOptions(question, selectedIndex);
//                 if (wrongEffectPrefab != null)
//                 {
//                     GameObject effect = Instantiate(wrongEffectPrefab, optionButtons[selectedIndex].transform.position, Quaternion.identity);
//                     Destroy(effect, 3f);
//                 }
//                 StartCoroutine(StopVibration(0.3f));
//             }
//             StartCoroutine(NextQuestionDelay(1.5f));
//         }
//         else
//         {
//             PlaySound(correctAnswerClip);
//             if (optionButtons[selectedIndex] != null) optionButtons[selectedIndex].GetComponent<Image>().color = preferenceSelectedColor;
//             StartCoroutine(NextQuestionDelay(0f));
//         }

//         if (FindObjectOfType<SurveyManager>() != null)
//             FindObjectOfType<SurveyManager>().RecordAnswer(question.questionID, question.questionType, selectedOption.optionID, selectedOption.course, question.questionType == "knowledge" && selectedOption.isCorrect ? 2 : 1);
//     }

//     void HighlightCorrectAndWrongOptions(Question question, int selectedIndex)
//     {
//         if (optionButtons != null)
//         {
//             for (int i = 0; i < question.options.Count && i < optionButtons.Length; i++)
//             {
//                 if (optionButtons[i] != null)
//                 {
//                     var buttonImage = optionButtons[i].GetComponent<Image>();
//                     if (i == selectedIndex && !question.options[i].isCorrect)
//                         buttonImage.color = wrongColor;
//                     else if (question.options[i].isCorrect)
//                         buttonImage.color = correctColor;
//                     else
//                         buttonImage.color = defaultButtonColor;
//                 }
//             }
//         }
//     }

//     IEnumerator NextQuestionDelay(float seconds)
//     {
//         Debug.Log($"Waiting {seconds}s before next question...");
//         yield return new WaitForSeconds(seconds);
//         currentQuestionIndex++;
//         ShowQuestion();
//     }

//     IEnumerator StopVibration(float delay)
//     {
//         yield return new WaitForSeconds(delay);
//         OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.RTouch);
//     }

//     void PlaySound(AudioClip clip)
//     {
//         if (audioSource != null && clip != null)
//             audioSource.PlayOneShot(clip);
//     }
// }