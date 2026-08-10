using UnityEngine;

/// <summary>
/// FIREFLY GATHERING prototype station: a zone between two poles ("low interest" /
/// "high interest"). While the player's headset stays within the zone, a fixed-length
/// dwell window accumulates a time-weighted lean toward whichever pole they're closer
/// to, then quantizes the result into 1-5. Leaving the zone pauses (not resets) the window.
/// </summary>
public class FireflyGatheringZone : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private Transform lowPole;
    [SerializeField] private Transform highPole;
    [SerializeField] private PreferenceStationReadout readout;

    [Header("Tuning")]
    [SerializeField] private Vector3 zoneSize = new Vector3(4f, 3f, 3f);
    [SerializeField] private float observationWindow = 10f;

    private float dwellTimeAccumulated;
    private float leanWeightedSum;
    private bool finalized;

    private void Start()
    {
        readout?.SetValue(1);
        readout?.SetStatus("Walk into the zone");
    }

    private void Update()
    {
        if (finalized) return;
        if (Camera.main == null) return;

        Vector3 pos = Camera.main.transform.position;
        if (!IsInsideZone(pos))
        {
            readout?.SetStatus(dwellTimeAccumulated > 0f ? "Paused (left zone)" : "Walk into the zone");
            return;
        }

        float lean = ComputeLean(pos); // 0 = low pole, 1 = high pole
        dwellTimeAccumulated += Time.deltaTime;
        leanWeightedSum += lean * Time.deltaTime;

        float remaining = Mathf.Max(0f, observationWindow - dwellTimeAccumulated);
        readout?.SetStatus($"Observing... {remaining:F1}s left (lean {lean:F2})");

        if (dwellTimeAccumulated >= observationWindow)
        {
            FinalizeResult();
        }
    }

    private bool IsInsideZone(Vector3 worldPos)
    {
        Vector3 local = transform.InverseTransformPoint(worldPos);
        Vector3 half = zoneSize * 0.5f;
        return Mathf.Abs(local.x) <= half.x && Mathf.Abs(local.y) <= half.y && Mathf.Abs(local.z) <= half.z;
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
        readout?.SetStatus($"Final (avg lean {avgLean:F2})");
    }
}
