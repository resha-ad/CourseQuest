using UnityEngine;

/// <summary>
/// FIREFLY GATHERING variant A (seated): reach your hand toward whichever pole appeals to
/// you and hold it there. Tracks whichever hand is closer to the zone each frame, projects
/// its position along the low-to-high pole axis, and averages that lean over a fixed
/// observation window. Press the secondary button (B/Y) to restart the window at any time.
/// </summary>
public class FireflyGatheringHandReach : MonoBehaviour
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
        readout?.SetStatus("Reach toward a pole");
    }

    private void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.RTouch) ||
            OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.LTouch))
        {
            ResetWindow();
        }

        if (finalized || cameraRig == null) return;

        Transform hand = ClosestHand();
        if (hand == null) return;

        float lean = ComputeLean(hand.position);
        dwellTimeAccumulated += Time.deltaTime;
        leanWeightedSum += lean * Time.deltaTime;

        float remaining = Mathf.Max(0f, observationWindow - dwellTimeAccumulated);
        readout?.SetStatus($"Observing... {remaining:F1}s left (lean {lean:F2})");

        if (dwellTimeAccumulated >= observationWindow)
        {
            FinalizeResult();
        }
    }

    private Transform ClosestHand()
    {
        Vector3 center = ZoneCenter();
        Transform left = cameraRig.leftHandAnchor;
        Transform right = cameraRig.rightHandAnchor;
        if (left == null) return right;
        if (right == null) return left;
        float dL = Vector3.Distance(left.position, center);
        float dR = Vector3.Distance(right.position, center);
        return dL <= dR ? left : right;
    }

    private Vector3 ZoneCenter()
    {
        if (lowPole == null || highPole == null) return transform.position;
        return (lowPole.position + highPole.position) * 0.5f;
    }

    private float ComputeLean(Vector3 worldPos)
    {
        if (lowPole == null || highPole == null) return 0.5f;
        Vector3 axis = highPole.position - lowPole.position;
        float axisLenSq = axis.sqrMagnitude;
        if (axisLenSq < 0.0001f) return 0.5f;
        float t = Vector3.Dot(worldPos - lowPole.position, axis) / axisLenSq;
        return Mathf.Clamp01(t);
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
        readout?.SetStatus("Reach toward a pole");
    }
}
