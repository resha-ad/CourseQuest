using UnityEngine;
using TMPro;

/// <summary>
/// Shared world-space debug readout used by all preference-mechanic prototype stations.
/// Displays the station's current/final quantized 1-5 value plus an optional status line.
/// </summary>
public class PreferenceStationReadout : MonoBehaviour
{
    [SerializeField] private TMP_Text valueText;
    [SerializeField] private TMP_Text statusText;
    [SerializeField] private string stationLabel = "Station";

    public void SetValue(int value)
    {
        if (valueText != null)
            valueText.text = $"{stationLabel}\nValue: {value}/5";
    }

    public void SetStatus(string status)
    {
        if (statusText != null)
            statusText.text = status;
    }

    public void SetLabel(string label)
    {
        stationLabel = label;
    }
}
