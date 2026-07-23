using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestionManager : MonoBehaviour
{
    public NutritionQuestions nutritionQuestions; // Reference to the NutritionQuestions script
    public TMP_Text questionTitleText; // Reference to the question title TMP
    public List<Button> optionButtons; // List of the four option buttons
    public TMP_Text[] optionTexts; // Array of TMP texts for the options (child of the buttons)
    public Button nextButton; // Reference to the next button
    public GameObject feedbackCanvas; // Reference to the Feedback Canvas

    public GameObject mainCanvas; // Reference to the Canvas

    private List<NutritionQuestions.Question> questions; // List to hold all questions
    private int currentQuestionIndex = 0; // Track the current question index
    public CanvasTextUpdater canvasTextUpdater;
    public FeedbackManager feedbackManager; // Reference to FeedbackManager

    // Category-wise points storage
    private Dictionary<string, int> categoryPoints = new Dictionary<string, int>()
    {
        { "Category 1", 0 },
        { "Category 2", 0 },
        { "Category 4", 0 }
    };

    // Array to store points for each option (A = 10, B = 7, C = 4, D = 1)
    private int[] points = { 10, 7, 4, 1 };

    // Store the selected option index
    private int selectedOptionIndex = -1;

    void Start()
    {
        // Fetch questions from NutritionQuestions
        questions = nutritionQuestions.GetQuestions();

        // Initialize first question
        SetQuestion(currentQuestionIndex);

        // Add listeners to option buttons
        for (int i = 0; i < optionButtons.Count; i++)
        {
            int optionIndex = i; // Capture index for the listener
            optionButtons[i].onClick.AddListener(() => OnOptionSelected(optionIndex));
        }

        // Add listener to the next button
        nextButton.onClick.AddListener(NextQuestion);
    }

    // Method to set the question and options in the UI
    void SetQuestion(int index)
    {
        if (index < questions.Count)
        {
            NutritionQuestions.Question question = questions[index];
            questionTitleText.text = question.questionText;

            // Reset the formatting and set the options' text
            for (int i = 0; i < optionButtons.Count; i++)
            {
                if (i < question.options.Count)
                {
                    optionTexts[i].text = question.options[i];
                    ResetOptionFormatting(i); // Reset formatting for each option
                }
            }

            // Reset the selected option index
            selectedOptionIndex = -1;
        }
        else
        {
            Debug.Log("No more questions available.");
        }
    }

    // Method to handle option selection
void OnOptionSelected(int optionIndex)
{
    // Ensure the selected option is valid
    if (optionIndex >= 0 && optionIndex < points.Length)
    {
        // Store the selected option index
        selectedOptionIndex = optionIndex;

        // Reset the background color for all buttons
        for (int i = 0; i < optionButtons.Count; i++)
        {
            ResetOptionFormatting(i);
        }

        // Change the background color of the selected option's button to yellow
        optionButtons[optionIndex].GetComponent<Image>().color = Color.yellow;
    }
}


    // Method to go to the next question and add points based on the selected option
    void NextQuestion()
    {
        // Check if an option has been selected
        if (selectedOptionIndex != -1)
        {
            // Determine the current category based on question index
            string currentCategory = GetCurrentCategory(currentQuestionIndex);

            // Add points to the current category based on the selected option
            categoryPoints[currentCategory] += points[selectedOptionIndex];
            Debug.Log($"Selected Option {selectedOptionIndex + 1}: {points[selectedOptionIndex]} points added to {currentCategory}");

            // Move to the next question or show feedback canvas if at the last question
            currentQuestionIndex++;

            if (currentQuestionIndex < questions.Count)
            {
                SetQuestion(currentQuestionIndex);
            }
            else
            {
                // If the current question is the last one, show the feedback canvas
                ShowFeedbackCanvas();
            }
        }
        else
        {
            Debug.LogWarning("No option selected. Please choose an option before proceeding.");
        }
    }

    // Method to display the feedback canvas
    void ShowFeedbackCanvas()
    {
        feedbackCanvas.SetActive(true); // Activate the feedback canvas
        mainCanvas.SetActive(false); // Activate the feedback canvas
        DisplayCategoryPoints(); // Optionally display points (for testing/logging)
    }

    // Method to display final category points (for testing/logging purposes)
    void DisplayCategoryPoints()
    {
        // Find and deactivate the "NextButton" in the feedbackCanvas
        nextButton = feedbackCanvas.transform.Find("NextButton")?.GetComponent<Button>();
        if (nextButton != null)
        {
            nextButton.gameObject.SetActive(false);
        }

        // Get the Canvas component of the feedbackCanvas
        Canvas canvas = feedbackCanvas.GetComponent<Canvas>();

        
        string accumulatedFeedback ="";

        // Iterate through each category in categoryPoints dictionary
        foreach (var category in categoryPoints)
        {
            string categoryName = category.Key;
            int score = category.Value;

            // Assuming categoryIndex is derived from the category name (convert category name to index if needed)
            int categoryIndex = GetCategoryIndex(categoryName);

            // Fetch the feedback for the category based on the score
            string feedback = feedbackManager.GetFeedback(categoryIndex, score);

            // Accumulate the feedback text
            accumulatedFeedback += feedback;
        }
        
        // Use the canvasTextUpdater to display the accumulated feedback on the feedbackCanvas
        canvasTextUpdater.UpdateText(canvas, accumulatedFeedback);
    }
    // Method to convert categoryPoints dictionary to a formatted string
  

    // Method to convert category name to index (assuming you use this elsewhere)
    int GetCategoryIndex(string categoryName)
    {
        switch (categoryName)
        {
            case "Category 1": return 0;
            case "Category 2": return 1;
            case "Category 4": return 2;
            default: return -1; // Default case if category is not found
        }
    }


    // Method to reset option formatting
        // Method to reset option formatting (background color) for each option
    void ResetOptionFormatting(int optionIndex)
    {
        // Convert RGBA values to [0, 1] range
        Color defaultColor = new Color(0f / 255f, 220f / 255f, 0f / 255f, 225f / 255f); // RGBA: 0, 120, 0, 225
        optionButtons[optionIndex].GetComponent<Image>().color = defaultColor; // Reset to the specified background color
    }


    // Method to determine the current category based on the question index
    string GetCurrentCategory(int questionIndex)
    {
        if (questionIndex < 4)
            return "Category 1";
        else if (questionIndex < 7)
            return "Category 2";
        else
            return "Category 4";
    }

}
