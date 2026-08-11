using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// "Light the Crystals" -- 5 empty crystal slots. Clicking a slot lights it and every
/// slot before it (so the lit count is always an unambiguous 1-5) and commits that as
/// the answer -- fires OnAnswered so a flow controller can advance to the next question.
/// No number is ever displayed, just the crystals themselves.
/// </summary>
public class CrystalLightingMechanic : MonoBehaviour
{
    [SerializeField] private Button[] crystalButtons; // exactly 5, in order
    [SerializeField] private Image[] crystalImages;
    [SerializeField] private GameObject confirmEffectPrefab;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip litClip;
    [SerializeField] private Color unlitColor = new Color(0.4f, 0.4f, 0.5f, 0.6f);
    [SerializeField] private Color litColor = Color.white;

    public event System.Action<int> OnAnswered;
    public int CurrentCount { get; private set; } = 0;
    private bool answered;

    private void Start()
    {
        for (int i = 0; i < crystalButtons.Length; i++)
        {
            int slot = i + 1; // 1..5
            crystalButtons[i].onClick.AddListener(() => Choose(slot));
        }
        ResetMechanic();
    }

    /// <summary>Called by the flow controller before showing this mechanic for a new question.</summary>
    public void ResetMechanic()
    {
        answered = false;
        CurrentCount = 0;
        for (int i = 0; i < crystalImages.Length; i++)
            crystalImages[i].color = unlitColor;
    }

    private void Choose(int count)
    {
        if (answered) return;
        answered = true;
        CurrentCount = count;

        for (int i = 0; i < crystalImages.Length; i++)
            crystalImages[i].color = (i + 1) <= count ? litColor : unlitColor;

        var slotTransform = crystalButtons[count - 1].transform;
        if (confirmEffectPrefab != null)
        {
            var fx = Instantiate(confirmEffectPrefab, slotTransform.position, Quaternion.identity);
            Destroy(fx, 2f);
        }
        if (audioSource != null && litClip != null)
            audioSource.PlayOneShot(litClip);

        Debug.Log($"[CrystalLightingMechanic] answered = {count}/5"); // verification only
        OnAnswered?.Invoke(count);
    }
}
