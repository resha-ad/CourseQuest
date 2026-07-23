using UnityEngine;

public class AndroidTextToSpeech : MonoBehaviour
{
    AndroidJavaObject ttsObject;

    void Start()
    {
        // Initialize Android TTS
        InitializeTTS();
        
        // Play a test message
        Speak("Hello! This is an example of Android text to speech in Unity.");
    }

    // Method to initialize Android TTS
    void InitializeTTS()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
                ttsObject = new AndroidJavaObject("android.speech.tts.TextToSpeech", activity, new TTSListener());
                ttsObject.Call<int>("setLanguage", new AndroidJavaObject("java.util.Locale", "US"));
            }
        }
    }

    // Method to play the text as speech
    public void Speak(string text)
    {
        if (ttsObject != null)
        {
            ttsObject.Call<int>("speak", text, 0, null, null);
        }
    }

    // Cleanup when quitting the app
    void OnDestroy()
    {
        if (ttsObject != null)
        {
            ttsObject.Call("shutdown");
        }
    }

    // Listener for TextToSpeech status (required for TTS initialization in Android)
    private class TTSListener : AndroidJavaProxy
    {
        public TTSListener() : base("android.speech.tts.TextToSpeech$OnInitListener") { }

        void onInit(int status)
        {
            if (status == 0) // Success
            {
                Debug.Log("Text-to-Speech initialized successfully");
            }
            else
            {
                Debug.LogError("Text-to-Speech initialization failed");
            }
        }
    }
}
