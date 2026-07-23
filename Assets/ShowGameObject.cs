using UnityEngine;

public class ShowGameObject : MonoBehaviour
{
    public GameObject targetObject; // Assign the GameObject to be shown in the Inspector.
    public GameObject video; // Assign the GameObject to be shown in the Inspector.

    public void Show()
    {
        if (targetObject != null)
        {
            targetObject.SetActive(true); // Set the GameObject active.
            video.SetActive(false); // Set the GameObject active.
        }
    }
}
