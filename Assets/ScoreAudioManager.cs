using UnityEngine;
using UnityEngine.UI; // Include this for UI elements like Image
using System.Collections; // Include this for IEnumerator

public class ScoreAudioManager : MonoBehaviour
{
    public AudioSource audio1;   // AudioSource for score 30-40
    public AudioSource audio2;   // AudioSource for score 20-30
    public AudioSource audio3;   // AudioSource for score 10-20

    public GameObject image1;         // Image to show for score 30-40
    public GameObject image2;         // Image to show for score 20-30
    public GameObject image3;         // Image to show for score 10-20

    // This method can be called from another script, passing the score as an argument
    public void PlayAudioAndShowImageBasedOnScore(int score)
    {
        
        if (score >= 75 && score <= 100)
        {
            ShowImage(image1);
            PlayAudio(audio1);
            
        }
        else if (score >= 40 && score < 80)
        {
            ShowImage(image2);
            PlayAudio(audio2);
        }
        else
        {
            ShowImage(image3);
            PlayAudio(audio3);
        }
        StartCoroutine(HideImageAfterDelay(9f));
    }

    // Helper method to play the audio clip if it's not already playing
    private void PlayAudio(AudioSource audio)
    {
        if (audio != null && !audio.isPlaying)
        {
            audio.Play();
        }
    }

    // Helper method to show a specific image
    private void ShowImage(GameObject image)
    {
        if (image != null)
        {
            image.gameObject.SetActive(true);
        }
    }

    // Helper method to hide all images
    private void HideAllImages()
    {
        if (image1 != null) image1.gameObject.SetActive(false);
        if (image2 != null) image2.gameObject.SetActive(false);
        if (image3 != null) image3.gameObject.SetActive(false);
    }
    // Coroutine to hide the image after a delay
    private IEnumerator HideImageAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        HideAllImages(); // Hide all images after the delay
    }
}
