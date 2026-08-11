using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// "The Lever" -- a handle that visibly rests at one of 5 marked notches. Clicking a
/// notch moves the handle to rest there and commits that as the answer -- fires
/// OnAnswered so a flow controller can advance to the next question.
/// </summary>
public class LeverMechanic : MonoBehaviour
{
    [SerializeField] private Button[] notchButtons; // exactly 5, in order
    [SerializeField] private RectTransform handle;
    [SerializeField] private RectTransform[] notchPositions; // matching handle-rest positions per notch
    [SerializeField] private GameObject confirmEffectPrefab;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip clunkClip;
    [SerializeField] private float moveDuration = 0.18f;
    [SerializeField] private Vector2 restPosition = new Vector2(0f, 25f); // where the handle sits before answering

    public event System.Action<int> OnAnswered;
    public int CurrentPosition { get; private set; } = -1;
    private bool answered;
    private Coroutine moveRoutine;

    private void Start()
    {
        for (int i = 0; i < notchButtons.Length; i++)
        {
            int position = i + 1; // 1..5
            notchButtons[i].onClick.AddListener(() => Choose(position));
        }
        ResetMechanic();
    }

    /// <summary>Called by the flow controller before showing this mechanic for a new question.</summary>
    public void ResetMechanic()
    {
        answered = false;
        CurrentPosition = -1;
        if (moveRoutine != null) StopCoroutine(moveRoutine);
        if (handle != null) handle.anchoredPosition = restPosition;
    }

    private void Choose(int position)
    {
        if (answered) return;
        answered = true;
        CurrentPosition = position;

        if (moveRoutine != null) StopCoroutine(moveRoutine);
        moveRoutine = StartCoroutine(MoveHandle(notchPositions[position - 1].anchoredPosition));

        if (confirmEffectPrefab != null)
        {
            var fx = Instantiate(confirmEffectPrefab, notchButtons[position - 1].transform.position, Quaternion.identity);
            Destroy(fx, 2f);
        }
        if (audioSource != null && clunkClip != null)
            audioSource.PlayOneShot(clunkClip);

        Debug.Log($"[LeverMechanic] answered = {position}/5"); // verification only
        OnAnswered?.Invoke(position);
    }

    private System.Collections.IEnumerator MoveHandle(Vector2 target)
    {
        Vector2 start = handle.anchoredPosition;
        float t = 0f;
        while (t < moveDuration)
        {
            t += Time.deltaTime;
            handle.anchoredPosition = Vector2.Lerp(start, target, t / moveDuration);
            yield return null;
        }
        handle.anchoredPosition = target;
    }
}
