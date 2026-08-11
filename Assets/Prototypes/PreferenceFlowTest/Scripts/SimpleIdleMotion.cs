using UnityEngine;

/// <summary>
/// Stand-in idle motion for the guide avatar -- this model has zero baked animation
/// clips, so without this it reads as a frozen statue. Gentle bob + sway gives a "the
/// character is alive" cue until real animation (idle/talk/gesture clips) is authored.
/// </summary>
public class SimpleIdleMotion : MonoBehaviour
{
    [SerializeField] private float bobHeight = 0.03f;
    [SerializeField] private float bobSpeed = 1.4f;
    [SerializeField] private float swayDegrees = 4f;
    [SerializeField] private float swaySpeed = 0.9f;

    private Vector3 basePosition;
    private Quaternion baseRotation;
    private float phaseOffset;

    private void Start()
    {
        basePosition = transform.localPosition;
        baseRotation = transform.localRotation;
        phaseOffset = Random.Range(0f, 10f); // so multiple avatars don't bob in lockstep
    }

    private void Update()
    {
        float t = Time.time + phaseOffset;
        float bob = Mathf.Sin(t * bobSpeed) * bobHeight;
        float sway = Mathf.Sin(t * swaySpeed) * swayDegrees;
        transform.localPosition = basePosition + new Vector3(0f, bob, 0f);
        transform.localRotation = baseRotation * Quaternion.Euler(0f, sway, 0f);
    }
}
