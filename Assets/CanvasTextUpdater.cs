using UnityEngine;
using TMPro;

public class CanvasTextUpdater : MonoBehaviour
{
    // Function to update the TMP text by accepting the canvas and text externally
    public void UpdateText(Canvas canvas, string newText)
    {
        if (canvas != null)
        {
            // Get the TMP_Text component from the child of the canvas
            TMP_Text tmpText = canvas.GetComponentInChildren<TMP_Text>();

            if (tmpText != null)
            {
                // Set the new text
                tmpText.text = newText;
            }
            else
            {
                Debug.LogError("TMP_Text component not found in the child objects of the Canvas.");
            }
        }
        else
        {
            Debug.LogError("Canvas is null.");
        }
    }
}
