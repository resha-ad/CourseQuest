using UnityEngine;

/// <summary>
/// BLOOMING VESSEL prototype station: hold the trigger while a controller is near the
/// vessel to charge it. Hold duration (0-3s) is bucketed into 5 growth stages; releasing
/// (or moving out of range) locks in the value. Growth is shown continuously via a
/// scale + color lerp while charging, snapping to the final discrete stage on release.
/// </summary>
public class BloomingVesselInteractable : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private Transform vessel;
    [SerializeField] private Renderer vesselRenderer;
    [SerializeField] private PreferenceStationReadout readout;
    [SerializeField] private OVRCameraRig cameraRig;

    [Header("Tuning")]
    [SerializeField] private float interactRadius = 0.5f;
    [SerializeField] private float maxHoldSeconds = 3f;
    [SerializeField] private Vector3 baseScale = new Vector3(0.5f, 0.5f, 0.5f);
    [SerializeField] private Vector3 maxScale = new Vector3(1.4f, 1.4f, 1.4f);
    [SerializeField] private Color baseColor = new Color(0.4f, 0.6f, 0.4f);
    [SerializeField] private Color maxColor = new Color(1f, 0.5f, 0.8f);

    private float heldTime;
    private bool isCharging;
    private MaterialPropertyBlock mpb;

    private void Reset()
    {
        cameraRig = FindObjectOfType<OVRCameraRig>();
    }

    private void Start()
    {
        mpb = new MaterialPropertyBlock();
        ApplyGrowth(0f);
        readout?.SetValue(1);
        readout?.SetStatus("Idle");
    }

    private void Update()
    {
        if (cameraRig == null || vessel == null) return;

        bool inRange =
            (cameraRig.rightHandAnchor != null && Vector3.Distance(cameraRig.rightHandAnchor.position, vessel.position) <= interactRadius) ||
            (cameraRig.leftHandAnchor != null && Vector3.Distance(cameraRig.leftHandAnchor.position, vessel.position) <= interactRadius);

        bool triggerHeld = OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch)
                         || OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.LTouch);

        if (inRange && triggerHeld)
        {
            isCharging = true;
            heldTime = Mathf.Min(heldTime + Time.deltaTime, maxHoldSeconds);
            ApplyGrowth(heldTime / maxHoldSeconds);
            readout?.SetStatus($"Charging... {heldTime:F1}s");
        }
        else if (isCharging)
        {
            // Released (or moved out of range) -- lock in the value.
            isCharging = false;
            int band = BucketToBand(heldTime);
            ApplyGrowth((band - 1) / 4f); // snap visual to the discrete stage
            readout?.SetValue(band);
            readout?.SetStatus($"Locked: {heldTime:F1}s held");
            heldTime = 0f;
        }
    }

    private int BucketToBand(float seconds)
    {
        float clamped = Mathf.Clamp(seconds, 0f, maxHoldSeconds);
        float t = clamped / maxHoldSeconds; // 0..1
        int band = Mathf.Clamp(Mathf.FloorToInt(t * 5f) + 1, 1, 5);
        if (clamped >= maxHoldSeconds) band = 5; // guard against float imprecision at the max
        return band;
    }

    private void ApplyGrowth(float t)
    {
        t = Mathf.Clamp01(t);
        vessel.localScale = Vector3.Lerp(baseScale, maxScale, t);
        if (vesselRenderer != null)
        {
            vesselRenderer.GetPropertyBlock(mpb);
            mpb.SetColor("_Color", Color.Lerp(baseColor, maxColor, t));
            vesselRenderer.SetPropertyBlock(mpb);
        }
    }
}
