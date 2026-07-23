using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneManagerController : MonoBehaviour
{
    public static SceneManagerController Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("SceneManagerController initialized and set to persist across scenes.");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadTechLabScene(System.Action onSceneLoaded = null)
    {
        if (SceneManager.GetActiveScene().name != "TechLab")
        {
            SceneManager.LoadScene("TechLab");
            if (onSceneLoaded != null)
            {
                StartCoroutine(WaitForSceneLoad("TechLab", onSceneLoaded));
            }
            Debug.Log("Loading TechLab scene.");
        }
        else
        {
            onSceneLoaded?.Invoke();
            Debug.Log("Already in TechLab scene.");
        }
    }

    public void LoadClassRoomScene(System.Action onSceneLoaded = null)
    {
        if (SceneManager.GetActiveScene().name != "ClassRoom")
        {
            SceneManager.LoadScene("ClassRoom");
            if (onSceneLoaded != null)
            {
                StartCoroutine(WaitForSceneLoad("ClassRoom", onSceneLoaded));
            }
            Debug.Log("Loading ClassRoom scene.");
        }
        else
        {
            onSceneLoaded?.Invoke();
            Debug.Log("Already in ClassRoom scene.");
        }
    }

    private IEnumerator WaitForSceneLoad(string sceneName, System.Action onSceneLoaded)
    {
        while (SceneManager.GetActiveScene().name != sceneName)
        {
            yield return null;
        }
        onSceneLoaded?.Invoke();
        Debug.Log($"Scene {sceneName} loaded, callback executed.");
    }
}