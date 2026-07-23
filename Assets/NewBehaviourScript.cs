using UnityEngine;
using UnityEngine.UI;

public class NewBehaviourScript : MonoBehaviour
{
   // Reference to the Canvas
    public Canvas canvasToHide;

    // This function is called when the button is clicked
    public void HideCanvas()
    {
        if (canvasToHide != null)
        {
            canvasToHide.gameObject.SetActive(false); // Hide the canvas
        }
    }
}
