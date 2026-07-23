using UnityEngine;
using UnityEngine.SceneManagement; // Make sure to include this for scene management

public class RestartGame : MonoBehaviour
{
    public void Restart()
    {
                
        // Reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
