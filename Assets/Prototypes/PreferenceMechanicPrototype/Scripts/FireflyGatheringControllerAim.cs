using UnityEngine;

/// <summary>
/// FIREFLY GATHERING variant B (seated): aim your controller toward whichever pole appeals
/// to you. Tracks whichever hand's forward direction more clearly favors one pole each
/// frame, and averages that lean over a fixed observation window. Press the secondary
/// button (B/Y) to restart the window at any time.
/// </summary>
public class FireflyGatheringControllerAim : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private Transform lowPole;
    [SerializeField] private Transform highPole;
    [SerializeField] private PreferenceStationReadout readout;
    [SerializeField] private OVRCameraRig cameraRig;

    [Header("Tuning")]
    [SerializeField] private float observationWindow = 10f;

    private float dwellTimeAccumulated;
    private float leanWeightedSum;
    private bool finalized;

    private void Start()
    {
        readout?.SetValue(1);
        readout?.SetStatus("Aim toward a pole");
    }

    private void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.RTouch) ||
            OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.LTouch))
        {
            ResetWindow();
        }

        if (finalized || cameraRig == null || lowPole == null || highPole == null) return;

        Vector3 axis = (highPole.position - lowPole.position).normalized;

        float leanRight = AimLean(cameraRig.rightHandAnchor, axis);
        float leanLeft = AimLean(cameraRig.leftHandAnchor, axis);
        // Use whichever hand gives the clearer (more confident/extreme) signal.
        float lean = Mathf.Abs(leanRight - 0.5f) >= Mathf.Abs(leanLeft - 0.5f) ? leanRight : leanLeft;

        dwellTimeAccumulated += Time.deltaTime;
        leanWeightedSum += lean * Time.deltaTime;

        float remaining = Mathf.Max(0f, observationWindow - dwellTimeAccumulated);
        readout?.SetStatus($"Observing... {remaining:F1}s left (lean {lean:F2})");

        if (dwellTimeAccumulated >= observationWindow)
        {
            FinalizeResult();
        }
    }

    private float AimLean(Transform hand, Vector3 axis)
    {
        if (hand == null) return 0.5f;
        float dot = Vector3.Dot(hand.forward, axis); // -1..1
        return Mathf.Clamp01((dot + 1f) * 0.5f); // remap to 0..1
    }

    private void FinalizeResult()
    {
        finalized = true;
        float avgLean = dwellTimeAccumulated > 0f ? leanWeightedSum / dwellTimeAccumulated : 0.5f;
        int value = Mathf.Clamp(Mathf.FloorToInt(avgLean * 5f) + 1, 1, 5);
        if (avgLean >= 1f) value = 5;
        readout?.SetValue(value);
        readout?.SetStatus($"Final (avg lean {avgLean:F2}) - press B/Y to retry");
    }

    private void ResetWindow()
    {
        finalized = false;
        dwellTimeAccumulated = 0f;
        leanWeightedSum = 0f;
        readout?.SetValue(1);
        readout?.SetStatus("Aim toward a pole");
    }
}
