using UnityEngine;

/// <summary>
/// Seated-play locomotion: point the right controller at a TeleportTarget, it highlights,
/// press the primary button (A/X) to instantly move the OVRCameraRig's tracking space there.
/// No physical walking assumed -- this is the only way to move between prototype stations.
/// </summary>
public class SeatedTeleportLocomotion : MonoBehaviour
{
    [SerializeField] private OVRCameraRig cameraRig;
    [SerializeField] private LayerMask teleportLayer = ~0;
    [SerializeField] private float maxRayDistance = 30f;

    private TeleportTarget currentTarget;

    private void Reset()
    {
        cameraRig = FindObjectOfType<OVRCameraRig>();
    }

    private void Update()
    {
        if (cameraRig == null || cameraRig.rightHandAnchor == null) return;

        Transform hand = cameraRig.rightHandAnchor;
        TeleportTarget hitTarget = null;
        if (Physics.Raycast(hand.position, hand.forward, out RaycastHit hit, maxRayDistance, teleportLayer))
        {
            hitTarget = hit.collider.GetComponentInParent<TeleportTarget>();
        }

        if (hitTarget != currentTarget)
        {
            if (currentTarget != null) currentTarget.SetHighlighted(false);
            currentTarget = hitTarget;
            if (currentTarget != null) currentTarget.SetHighlighted(true);
        }

        if (currentTarget != null && OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.RTouch))
        {
            TeleportTo(currentTarget.LandingPosition);
        }
    }

    private void TeleportTo(Vector3 landingPosition)
    {
        // Move the whole rig's XZ to the target, preserving current facing and rig-root Y
        // (real seated/standing height comes from live headset tracking above that).
        cameraRig.transform.position = new Vector3(landingPosition.x, cameraRig.transform.position.y, landingPosition.z);
    }
}
