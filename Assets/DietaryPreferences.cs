using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class DietaryPreferences : MonoBehaviour
{
    public GameObject[] categoryOneCanvases;
    public GameObject[] categoryTwoCanvases;
    public GameObject[] categoryFourCanvases;
    public GameObject feedbackCanvas; // Single feedback canvas
    private TMP_Text tmpText;
    public CanvasTextUpdater canvasTextUpdater;
    public FeedbackManager feedbackManager; // Reference to FeedbackManager

    private GameObject[][] allCategories; // Array of all categories
    private int currentCategoryIndex = 0; // Index to track current category
    private int currentCanvasIndex = 0; // To track current canvas in random selection
    private Button option1Button;
    private Button option2Button;
    private Button option3Button;
    private Button option4Button;
    private Button nextButton; // For navigation to the next canvas
    private Button selectedButton;
    private List<int> randomCanvasIndices;
    private int[] categoryPoints = new int[3]; // 3 categories

    void Start()
    {
        // Initialize all categories into a 2D array, feedbackCanvas is added as the last category
        allCategories = new GameObject[][] {
            categoryOneCanvases,
            categoryTwoCanvases,
            categoryFourCanvases,
            new GameObject[] { feedbackCanvas } // Single feedback canvas
        };

        // Start with the first category
        GenerateRandomCanvasIndices();
        UpdateCanvasVisibility();
        AssignButtonsForCurrentCanvas();
        AssignNextButton();
    }
    void Update()
    {
        // Check if the 'B' button is pressed
        if (OVRInput.GetDown(OVRInput.Button.Two))
        {
            OnNextButtonClicked();
        }
    }
    void GenerateRandomCanvasIndices()
    {
        var currentCategoryCanvases = allCategories[currentCategoryIndex];
        List<int> allIndices = new List<int>();

        for (int i = 0; i < currentCategoryCanvases.Length; i++)
        {
            allIndices.Add(i);
        }

        Shuffle(allIndices);
        randomCanvasIndices = allIndices.GetRange(0, Mathf.Min(5, currentCategoryCanvases.Length));
    }

    void Shuffle<T>(List<T> list)
    {
        System.Random rng = new System.Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    void AssignButtonsForCurrentCanvas()
    {
        var currentCategoryCanvases = allCategories[currentCategoryIndex];
        var currentCanvas = currentCategoryCanvases[randomCanvasIndices[currentCanvasIndex]];

        option1Button = currentCanvas.transform.Find("Option1Button")?.GetComponent<Button>();
        option2Button = currentCanvas.transform.Find("Option2Button")?.GetComponent<Button>();
        option3Button = currentCanvas.transform.Find("Option3Button")?.GetComponent<Button>();
        option4Button = currentCanvas.transform.Find("Option4Button")?.GetComponent<Button>();

        if (option1Button == null || option2Button == null || option3Button == null || option4Button == null)
        {
            Debug.LogError("One or more option buttons not found on canvas: " + currentCanvas.name);
            return;
        }

        option1Button.onClick.AddListener(() => OnOptionSelected(option1Button));
        option2Button.onClick.AddListener(() => OnOptionSelected(option2Button));
        option3Button.onClick.AddListener(() => OnOptionSelected(option3Button));
        option4Button.onClick.AddListener(() => OnOptionSelected(option4Button));
    }

    void OnOptionSelected(Button selected)
    {
        if (selectedButton != null)
        {
            DeselectButton(selectedButton);
        }

        selectedButton = selected;
        FormatButtonText(selectedButton, true);
    }

    void FormatButtonText(Button button, bool applyStyle)
    {
        var tmpText = button.GetComponentInChildren<TextMeshProUGUI>();

        if (tmpText != null)
        {
            if (applyStyle)
            {
                tmpText.fontStyle |= FontStyles.Bold | FontStyles.Underline;
            }
            else
            {
                tmpText.fontStyle &= ~(FontStyles.Bold | FontStyles.Underline);
            }
        }
    }

    void DeselectButton(Button button)
    {
        FormatButtonText(button, false);
    }

    void AssignNextButton()
    {
        var currentCategoryCanvases = allCategories[currentCategoryIndex];
        var currentCanvas = currentCategoryCanvases[randomCanvasIndices[currentCanvasIndex]];
        nextButton = currentCanvas.transform.Find("NextButton")?.GetComponent<Button>();

        if (nextButton == null)
        {
            Debug.LogError("NextButton not found on canvas: " + currentCanvas.name);
            return;
        }

        nextButton.onClick.RemoveAllListeners();
        nextButton.onClick.AddListener(OnNextButtonClicked);
    }

    void OnNextButtonClicked()
    {
        if (selectedButton != null)
        {
            // Add points to the current category based on selection
            if (selectedButton == option1Button) categoryPoints[currentCategoryIndex] += 10;
            else if (selectedButton == option2Button) categoryPoints[currentCategoryIndex] += 7;
            else if (selectedButton == option3Button) categoryPoints[currentCategoryIndex] += 4;
            else if (selectedButton == option4Button) categoryPoints[currentCategoryIndex] += 1;

            currentCanvasIndex++;

            if (currentCanvasIndex < randomCanvasIndices.Count)
            {
                UpdateCanvasVisibility();
                AssignButtonsForCurrentCanvas();
                AssignNextButton();
            }
            else
            {
                // Move to the next category
                currentCanvasIndex = 0;
                currentCategoryIndex++;

                if (currentCategoryIndex < allCategories.Length - 1)
                {
                    GenerateRandomCanvasIndices();
                    UpdateCanvasVisibility();
                    AssignButtonsForCurrentCanvas();
                    AssignNextButton();
                }
                else if (currentCategoryIndex == allCategories.Length - 1) // Feedback canvas
                {
                    UpdateCanvasVisibility(); // Show feedback canvas
                    DisplayFeedback();
                }

                selectedButton = null;
            }
        }
        else
        {
            Debug.LogError("No option selected!");
        }
    }

    void UpdateCanvasVisibility()
    {
        var currentCategoryCanvases = allCategories[currentCategoryIndex];
        for (int i = 0; i < currentCategoryCanvases.Length; i++)
        {
            currentCategoryCanvases[i].SetActive(i == randomCanvasIndices[currentCanvasIndex]);
        }

        // Handle feedback canvas separately
        if (currentCategoryIndex == allCategories.Length - 1)
        {
            feedbackCanvas.SetActive(true); // Show feedback canvas
        }
    }

    void DisplayFeedback()
    {
        nextButton = feedbackCanvas.transform.Find("NextButton")?.GetComponent<Button>();
        if (nextButton != null)
        {
            nextButton.gameObject.SetActive(false);
        }
        
        Canvas canvas = feedbackCanvas.GetComponent<Canvas>();
        string accumulatedFeedback = "";
        
        for (int categoryIndex = 0; categoryIndex < categoryPoints.Length; categoryIndex++)
        {
            int score = categoryPoints[categoryIndex];
            string feedback = feedbackManager.GetFeedback(categoryIndex, score);
            accumulatedFeedback += feedback + "\n"; // Append feedback for each category
        }
        canvasTextUpdater.UpdateText(canvas, accumulatedFeedback);
    }
}
