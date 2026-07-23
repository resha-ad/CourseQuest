using UnityEngine;

public class AnotherScript : MonoBehaviour
{
    // Reference to the CanvasTextUpdater component
    public CanvasTextUpdater canvasTextUpdater;

    // Reference to the Canvas
    public Canvas someCanvas;

    private void Start()
    {
        // Ensure that canvasTextUpdater and someCanvas are assigned
        if (canvasTextUpdater != null && someCanvas != null)
        {
            // Call the UpdateText method of CanvasTextUpdater
            canvasTextUpdater.UpdateText(someCanvas, "Welcome to Unity VR!");
        }
        else
        {
            Debug.LogError("CanvasTextUpdater or Canvas is not assigned.");
        }
    }
}
