using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// "The Lever" -- a handle that visibly rests at one of 5 marked notches. Point and click
/// a notch (same laser-pointer pattern as the rest of the mechanics) and the handle moves
/// to rest there, so the player can always see exactly which position it's holding.
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

    public int CurrentPosition { get; private set; } = -1;
    private Coroutine moveRoutine;

    private void Start()
    {
        for (int i = 0; i < notchButtons.Length; i++)
        {
            int position = i + 1; // 1..5
            notchButtons[i].onClick.AddListener(() => SetPosition(position));
        }
    }

    private void SetPosition(int position)
    {
        CurrentPosition = position;
        if (moveRoutine != null) StopCoroutine(moveRoutine);
        moveRoutine = StartCoroutine(MoveHandle(notchPositions[position - 1].localPosition));

        if (confirmEffectPrefab != null)
        {
            var fx = Instantiate(confirmEffectPrefab, notchButtons[position - 1].transform.position, Quaternion.identity);
            Destroy(fx, 2f);
        }
        if (audioSource != null && clunkClip != null)
            audioSource.PlayOneShot(clunkClip);

        Debug.Log($"[LeverMechanic] position = {position}/5"); // verification only
    }

    private System.Collections.IEnumerator MoveHandle(Vector3 target)
    {
        Vector3 start = handle.localPosition;
        float t = 0f;
        while (t < moveDuration)
        {
            t += Time.deltaTime;
            handle.localPosition = Vector3.Lerp(start, target, t / moveDuration);
            yield return null;
        }
        handle.localPosition = target;
    }
}
