using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// "Reach for your Truth" -- 5 orbs of increasing size/brightness laid out in an arc.
/// Point (laser pointer, same OVRRaycaster pattern as the rest of the app) and click the
/// one that matches how the statement feels. Single action, single clear choice, the
/// choice stays visibly confirmed (chosen orb glows, others fade) -- no number is ever shown.
/// </summary>
public class OrbSelectionMechanic : MonoBehaviour
{
    [SerializeField] private Button[] orbButtons; // exactly 5, left(1) to right(5)
    [SerializeField] private Image[] orbImages;
    [SerializeField] private GameObject confirmEffectPrefab;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip selectClip;
    [SerializeField] private Color dimColor = new Color(1f, 1f, 1f, 0.35f);

    private bool answered;
    public int LastValue { get; private set; } = -1;

    private void Start()
    {
        for (int i = 0; i < orbButtons.Length; i++)
        {
            int value = i + 1; // 1..5
            orbButtons[i].onClick.AddListener(() => OnOrbChosen(value, orbButtons[value - 1].transform));
        }
    }

    private void OnOrbChosen(int value, Transform chosenTransform)
    {
        if (answered) return;
        answered = true;
        LastValue = value;

        for (int i = 0; i < orbImages.Length; i++)
        {
            bool isChosen = (i + 1) == value;
            orbImages[i].color = isChosen ? Color.white : dimColor;
        }

        if (confirmEffectPrefab != null)
        {
            var fx = Instantiate(confirmEffectPrefab, chosenTransform.position, Quaternion.identity);
            Destroy(fx, 3f);
        }
        if (audioSource != null && selectClip != null)
            audioSource.PlayOneShot(selectClip);

        Debug.Log($"[OrbSelectionMechanic] answered = {value}/5"); // verification only, not shown in-world
    }
}
