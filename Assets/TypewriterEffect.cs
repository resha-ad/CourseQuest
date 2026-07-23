using System.Collections;
using TMPro;
using UnityEngine;

public class TypewriterEffect : MonoBehaviour
{
    public TextMeshProUGUI textComponent; // Reference to your TMP text in Canvas
    public float typingSpeed = 1.0f; // Delay between each character
    public AudioSource audioSource; // Reference to your AudioSource
    public AudioClip analyzingAudioClip; // Reference to the audio clip
    private string fullText; // The full text you want to display
    private string dots = ". . . . . .";
    public ScoreAudioManager scoreAudioManager;  // Reference to the ScoreAudioManager script
    public QuestionManager questionManager;

    void Start()
    {
        if (textComponent == null)
        {
            textComponent = GetComponent<TextMeshProUGUI>(); // Get the TMP component if not set
        }

        fullText = textComponent.text; // Store the full text
        textComponent.text = ""; // Clear the text to start typewriter effect
        StartCoroutine(ProcessAndTypeText());
    }

    IEnumerator ProcessAndTypeText()
    {
        textComponent.text = "\n\n\n\n\n\t\t\t\tAnalyzing your answer";
        yield return new WaitForSeconds(0.05f); // Small delay before starting dots

        // Step 1: Play "Analyzing your answer" audio and display "Analyzing your preferences"
        if (audioSource != null && analyzingAudioClip != null)
        {
            audioSource.clip = analyzingAudioClip;
            audioSource.Play();
        }

        // Step 2: Show the dots one by one with a typewriter effect
        foreach (char letter in dots.ToCharArray())
        {
            textComponent.text += letter; // Add one character (dot) at a time
            yield return new WaitForSeconds(0.5f); // Wait between each dot
        }

        // Step 3: Wait for a moment to simulate "processing"
        yield return new WaitForSeconds(1.0f);

        // Step 1: Display "Getting Response from the Server"
        textComponent.text = "\n\n\n\n\n\t\t\t\tGetting Response from the Server";
        yield return new WaitForSeconds(0.05f); // Small delay before starting dots
        
        // Step 2: Show the dots one by one with a typewriter effect
        foreach (char letter in dots.ToCharArray())
        {
            textComponent.text += letter; // Add one character (dot) at a time
            yield return new WaitForSeconds(0.5f); // Wait between each dot
        }

        // Step 3: Wait for a moment to simulate "processing"
        yield return new WaitForSeconds(1.0f);

        // Step 1: Display "Aggregating Feedback"
        textComponent.text = "\n\n\n\n\n\t\t\t\tAggregating Feedback";
        yield return new WaitForSeconds(0.05f); // Small delay before starting dots
        
        // Step 2: Show the dots one by one with a typewriter effect
        foreach (char letter in dots.ToCharArray())
        {
            textComponent.text += letter; // Add one character (dot) at a time
            yield return new WaitForSeconds(0.5f); // Wait between each dot
        }

        // Step 3: Wait for a moment to simulate "processing"
        yield return new WaitForSeconds(1.0f);

        // Step 4: Clear the text and display the full message with a typewriter effect
        textComponent.text = ""; // Clear the text
        yield return new WaitForSeconds(0.5f); // Optional pause before displaying full text
        
        // int score=questionManager.CalculateTotalScore();

        if (scoreAudioManager != null)
        {
            // scoreAudioManager.PlayAudioAndShowImageBasedOnScore(score);
        }
        yield return new WaitForSeconds(10.0f);

        foreach (char letter in fullText.ToCharArray())
        {
            textComponent.text += letter; // Add one character at a time
            yield return new WaitForSeconds(0.02f); // Wait between each letter
        }
    }
}
