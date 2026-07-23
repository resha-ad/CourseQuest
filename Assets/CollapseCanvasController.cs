using UnityEngine;

public class CollapseCanvasController : MonoBehaviour
{
    // Reference to the Canvas that needs to be hidden
    public GameObject canvasToCollapse;

    // Method to collapse the Canvas
    public void CollapseCanvas()
    {
        if (canvasToCollapse != null)
        {
            canvasToCollapse.SetActive(false); // Hide the Canvas
        }
    }
}
