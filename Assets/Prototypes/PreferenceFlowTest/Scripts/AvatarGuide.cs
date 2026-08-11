using UnityEngine;
using TMPro;
using System.Collections;

/// <summary>
/// Drives the guide avatar's floating caption text (stand-in for voice narration --
/// real audio narration/VO can replace the "speak" call later without touching callers).
/// A simple idle "talk" bob plays on the caption while a line is showing, so there's a
/// visible cue that the avatar is "saying" something even without audio yet.
/// </summary>
public class AvatarGuide : MonoBehaviour
{
    [SerializeField] private TMP_Text captionText;
    [SerializeField] private Transform captionAnchor;
    [SerializeField] private float bobAmount = 0.02f;
    [SerializeField] private float bobSpeed = 6f;
    [SerializeField] private float holdSecondsPerWord = 0.35f; // caption stays up roughly this long per word, then clears
    [SerializeField] private float minHoldSeconds = 1.6f;

    private Coroutine talkRoutine;

    public void Speak(string line)
    {
        if (captionText == null) return;
        if (talkRoutine != null) StopCoroutine(talkRoutine);
        int wordCount = string.IsNullOrEmpty(line) ? 0 : line.Split(' ').Length;
        float duration = Mathf.Max(minHoldSeconds, wordCount * holdSecondsPerWord);
        talkRoutine = StartCoroutine(TalkBobThenClear(line, duration));
    }

    public void Clear()
    {
        if (captionText != null) captionText.text = "";
        if (talkRoutine != null) { StopCoroutine(talkRoutine); talkRoutine = null; }
    }

    private IEnumerator TalkBobThenClear(string line, float duration)
    {
        captionText.text = line;
        if (captionAnchor != null)
        {
            Vector3 basePos = captionAnchor.localPosition;
            float t = 0f;
            while (t < duration)
            {
                t += Time.deltaTime;
                float y = Mathf.Sin(t * bobSpeed) * bobAmount;
                captionAnchor.localPosition = basePos + new Vector3(0f, y, 0f);
                yield return null;
            }
            captionAnchor.localPosition = basePos;
        }
        else
        {
            yield return new WaitForSeconds(duration);
        }
        // Caption is transient (stand-in for spoken narration) -- clear it so it never
        // permanently overlaps the persistent question text once the "line" has finished.
        captionText.text = "";
    }
}
