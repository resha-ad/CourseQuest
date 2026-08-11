using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// "Reach for your Truth" -- 5 orbs of increasing size/brightness laid out in an arc.
/// Point (laser pointer) and click the one that matches how the statement feels --
/// fires OnAnswered so a flow controller can advance to the next question. The chosen
/// orb glows, the others fade -- no number is ever shown.
/// </summary>
public class OrbSelectionMechanic : MonoBehaviour
{
    [SerializeField] private Button[] orbButtons; // exactly 5, left(1) to right(5)
    [SerializeField] private Image[] orbImages;
    [SerializeField] private GameObject confirmEffectPrefab;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip selectClip;
    [SerializeField] private Color dimColor = new Color(1f, 1f, 1f, 0.35f);
    [SerializeField] private Color[] baseColors; // set by builder to restore on reset

    public event System.Action<int> OnAnswered;
    public int LastValue { get; private set; } = -1;
    private bool answered;

    private void Start()
    {
        for (int i = 0; i < orbButtons.Length; i++)
        {
            int value = i + 1; // 1..5
            orbButtons[i].onClick.AddListener(() => Choose(value));
        }
        ResetMechanic();
    }

    /// <summary>Called by the flow controller before showing this mechanic for a new question.</summary>
    public void ResetMechanic()
    {
        answered = false;
        LastValue = -1;
        if (baseColors != null)
        {
            for (int i = 0; i < orbImages.Length && i < baseColors.Length; i++)
                orbImages[i].color = baseColors[i];
        }
    }

    private void Choose(int value)
    {
        if (answered) return;
        answered = true;
        LastValue = value;

        for (int i = 0; i < orbImages.Length; i++)
            orbImages[i].color = (i + 1) == value ? Color.white : dimColor;

        var chosenTransform = orbButtons[value - 1].transform;
        if (confirmEffectPrefab != null)
        {
            var fx = Instantiate(confirmEffectPrefab, chosenTransform.position, Quaternion.identity);
            Destroy(fx, 3f);
        }
        if (audioSource != null && selectClip != null)
            audioSource.PlayOneShot(selectClip);

        Debug.Log($"[OrbSelectionMechanic] answered = {value}/5"); // verification only
        OnAnswered?.Invoke(value);
    }
}
