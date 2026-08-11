using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// "Light the Crystals" -- 5 empty crystal slots. Clicking a slot lights it and every
/// slot before it (so the lit count is always an unambiguous 1-5), and can be freely
/// re-clicked to change the count before it settles -- the player can always see exactly
/// how many are lit. No number is ever displayed, just the crystals themselves.
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

    public int CurrentCount { get; private set; } = 0;

    private void Start()
    {
        for (int i = 0; i < crystalButtons.Length; i++)
        {
            int slot = i + 1; // 1..5
            crystalButtons[i].onClick.AddListener(() => SetCount(slot));
        }
        SetCount(0, playFx: false);
    }

    private void SetCount(int count, bool playFx = true)
    {
        CurrentCount = count;
        for (int i = 0; i < crystalImages.Length; i++)
        {
            bool lit = (i + 1) <= count;
            crystalImages[i].color = lit ? litColor : unlitColor;
        }

        if (playFx && count > 0)
        {
            var slotTransform = crystalButtons[count - 1].transform;
            if (confirmEffectPrefab != null)
            {
                var fx = Instantiate(confirmEffectPrefab, slotTransform.position, Quaternion.identity);
                Destroy(fx, 2f);
            }
            if (audioSource != null && litClip != null)
                audioSource.PlayOneShot(litClip);
        }

        Debug.Log($"[CrystalLightingMechanic] current count = {count}/5"); // verification only
    }
}
