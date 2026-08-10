using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// CROSSROADS prototype station: 3 rounds of two waypoints each ("mild" / "strong" pick).
/// Walking to (or teleporting to) a waypoint registers that round's pick. After 3 rounds,
/// the strong-pick tally (0-3) maps to a 1-5 value: 0-&gt;1, 1-&gt;2, 2-&gt;4, 3-&gt;5.
/// </summary>
public class CrossroadsInteractable : MonoBehaviour
{
    [System.Serializable]
    public class RoundWaypoints
    {
        public Transform mild;
        public Transform strong;
    }

    [Header("Refs")]
    [SerializeField] private List<RoundWaypoints> rounds = new List<RoundWaypoints>();
    [SerializeField] private PreferenceStationReadout readout;

    [Header("Tuning")]
    [SerializeField] private float arrivalRadius = 0.75f;

    private int currentRound;
    private int strongTally;
    private bool finished;

    private static readonly int[] TallyToValue = { 1, 2, 4, 5 }; // index = strong tally 0-3

    private void Start()
    {
        for (int i = 0; i < rounds.Count; i++)
            SetRoundActive(i, i == 0);
        readout?.SetValue(1);
        UpdateStatus();
    }

    private void Update()
    {
        if (finished || Camera.main == null) return;
        if (currentRound >= rounds.Count) return;

        var round = rounds[currentRound];
        Vector3 pos = Camera.main.transform.position;

        if (round.mild != null && round.mild.gameObject.activeSelf && Vector3.Distance(pos, round.mild.position) <= arrivalRadius)
        {
            RegisterPick(false);
        }
        else if (round.strong != null && round.strong.gameObject.activeSelf && Vector3.Distance(pos, round.strong.position) <= arrivalRadius)
        {
            RegisterPick(true);
        }
    }

    private void RegisterPick(bool wasStrong)
    {
        if (wasStrong) strongTally++;

        SetRoundActive(currentRound, false);
        currentRound++;

        if (currentRound >= rounds.Count)
        {
            FinishSequence();
        }
        else
        {
            SetRoundActive(currentRound, true);
            UpdateStatus();
        }
    }

    private void SetRoundActive(int index, bool active)
    {
        if (index < 0 || index >= rounds.Count) return;
        var round = rounds[index];
        if (round.mild != null) round.mild.gameObject.SetActive(active);
        if (round.strong != null) round.strong.gameObject.SetActive(active);
    }

    private void UpdateStatus()
    {
        readout?.SetStatus($"Round {currentRound + 1}/{rounds.Count} - Strong picks: {strongTally}");
    }

    private void FinishSequence()
    {
        finished = true;
        int tallyIndex = Mathf.Clamp(strongTally, 0, 3);
        int value = TallyToValue[tallyIndex];
        readout?.SetValue(value);
        readout?.SetStatus($"Done - Strong picks: {strongTally}/3 -> Value {value}/5");
    }
}
