using UnityEngine;
using System.Collections;

/// <summary>
/// COMPASS ROOT prototype station: a totem dial the player grabs (grip button, in-range)
/// and twists like a valve. Tracks the controller's angular position around the dial's
/// vertical axis while held, quantizes rotation into 5 discrete detents (72 degrees apart),
/// and pulses on each detent change. Uses OVRInput/OVRCameraRig directly rather than XR
/// Interaction Toolkit's Interactor/Interactable events -- see prototype summary for why.
/// </summary>
public class CompassRootInteractable : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private Transform dial;
    [SerializeField] private Transform pulseRing;
    [SerializeField] private PreferenceStationReadout readout;
    [SerializeField] private OVRCameraRig cameraRig;

    [Header("Tuning")]
    [SerializeField] private float grabRadius = 0.35f;
    [SerializeField] private float pulseScale = 1.3f;
    [SerializeField] private float pulseDuration = 0.18f;

    private const int DetentCount = 5;
    private const float DetentStep = 360f / DetentCount;

    private bool isHeld;
    private bool heldByLeft;
    private float grabStartControllerAngle;
    private float grabStartDialYaw;
    private int lastDetentIndex = -1;
    private Coroutine pulseRoutine;

    private void Reset()
    {
        cameraRig = FindObjectOfType<OVRCameraRig>();
    }

    private void Start()
    {
        readout?.SetValue(NearestDetentIndex(dial != null ? dial.localEulerAngles.y : 0f) + 1);
        readout?.SetStatus("Grab and twist");
    }

    private void Update()
    {
        if (cameraRig == null || dial == null) return;

        if (!isHeld)
        {
            TryStartGrab(OVRInput.Controller.RTouch, cameraRig.rightHandAnchor, false);
            if (!isHeld)
                TryStartGrab(OVRInput.Controller.LTouch, cameraRig.leftHandAnchor, true);
        }
        else
        {
            Transform hand = heldByLeft ? cameraRig.leftHandAnchor : cameraRig.rightHandAnchor;
            OVRInput.Controller controller = heldByLeft ? OVRInput.Controller.LTouch : OVRInput.Controller.RTouch;

            if (hand == null || !OVRInput.Get(OVRInput.Button.PrimaryHandTrigger, controller))
            {
                EndGrab();
                return;
            }

            float currentAngle = AngleAroundDial(hand.position);
            float delta = Mathf.DeltaAngle(grabStartControllerAngle, currentAngle);
            float newYaw = grabStartDialYaw + delta;
            dial.localRotation = Quaternion.Euler(0f, newYaw, 0f);

            CheckDetent(newYaw);
        }
    }

    private void TryStartGrab(OVRInput.Controller controller, Transform hand, bool isLeft)
    {
        if (hand == null) return;
        if (Vector3.Distance(hand.position, dial.position) > grabRadius) return;
        if (!OVRInput.Get(OVRInput.Button.PrimaryHandTrigger, controller)) return;

        isHeld = true;
        heldByLeft = isLeft;
        grabStartControllerAngle = AngleAroundDial(hand.position);
        grabStartDialYaw = dial.localEulerAngles.y;
    }

    private void EndGrab()
    {
        isHeld = false;
        // Snap cleanly to the nearest detent's exact angle on release.
        int nearest = NearestDetentIndex(dial.localEulerAngles.y);
        dial.localRotation = Quaternion.Euler(0f, nearest * DetentStep, 0f);
        readout?.SetStatus("Released");
    }

    private float AngleAroundDial(Vector3 worldPos)
    {
        Vector3 local = worldPos - dial.position;
        local.y = 0f;
        return Mathf.Atan2(local.x, local.z) * Mathf.Rad2Deg;
    }

    private int NearestDetentIndex(float yaw)
    {
        float normalized = ((yaw % 360f) + 360f) % 360f;
        int index = Mathf.RoundToInt(normalized / DetentStep) % DetentCount;
        return index;
    }

    private void CheckDetent(float yaw)
    {
        int index = NearestDetentIndex(yaw);
        if (index != lastDetentIndex)
        {
            lastDetentIndex = index;
            Pulse();
            int value = index + 1; // 1-5
            readout?.SetValue(value);
            readout?.SetStatus("Turning...");
        }
    }

    private void Pulse()
    {
        if (pulseRing == null) return;
        if (pulseRoutine != null) StopCoroutine(pulseRoutine);
        pulseRoutine = StartCoroutine(PulseCoroutine());
    }

    private IEnumerator PulseCoroutine()
    {
        Vector3 baseScale = Vector3.one;
        float t = 0f;
        while (t < pulseDuration)
        {
            t += Time.deltaTime;
            float p = t / pulseDuration;
            float s = Mathf.Lerp(pulseScale, 1f, p);
            pulseRing.localScale = baseScale * s;
            yield return null;
        }
        pulseRing.localScale = baseScale;
    }
}
