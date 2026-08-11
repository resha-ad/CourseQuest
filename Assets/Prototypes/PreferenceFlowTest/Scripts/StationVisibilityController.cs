using UnityEngine;

/// <summary>
/// Keeps only the station closest to the player's current position active, so question
/// text/avatars from other stations never show simultaneously. Checked against the
/// headset position (Camera.main), same pattern used elsewhere in this prototype work.
/// </summary>
public class StationVisibilityController : MonoBehaviour
{
    [System.Serializable]
    public class Station
    {
        public GameObject root;
        public Vector3 triggerPosition;
    }

    [SerializeField] private Station[] stations;
    [SerializeField] private float activationRadius = 3.5f;

    private int activeIndex = -1;

    private void Update()
    {
        if (Camera.main == null || stations == null || stations.Length == 0) return;

        Vector3 pos = Camera.main.transform.position;
        int closest = -1;
        float closestDist = float.MaxValue;
        for (int i = 0; i < stations.Length; i++)
        {
            float d = Vector3.Distance(pos, stations[i].triggerPosition);
            if (d < activationRadius && d < closestDist)
            {
                closestDist = d;
                closest = i;
            }
        }

        if (closest != activeIndex)
        {
            if (activeIndex >= 0 && stations[activeIndex].root != null)
                stations[activeIndex].root.SetActive(false);
            if (closest >= 0 && stations[closest].root != null)
                stations[closest].root.SetActive(true);
            activeIndex = closest;
        }
    }
}
