using UnityEngine;

/// <summary>
/// Marker component identifying a GameObject as a valid seated-teleport destination.
/// The collider on this object defines the raycast hit target; landingPoint (if set) is
/// where the player's tracking space is moved to, otherwise this object's own position is used.
/// </summary>
public class TeleportTarget : MonoBehaviour
{
    [SerializeField] private Transform landingPoint;
    [SerializeField] private Renderer highlightRenderer;
    [SerializeField] private Color normalColor = new Color(0.3f, 0.6f, 0.9f);
    [SerializeField] private Color highlightColor = new Color(0.4f, 1f, 0.5f);

    private MaterialPropertyBlock mpb;

    public Vector3 LandingPosition => landingPoint != null ? landingPoint.position : transform.position;

    private void Awake()
    {
        mpb = new MaterialPropertyBlock();
    }

    public void SetHighlighted(bool highlighted)
    {
        if (highlightRenderer == null) return;
        highlightRenderer.GetPropertyBlock(mpb);
        mpb.SetColor("_Color", highlighted ? highlightColor : normalColor);
        highlightRenderer.SetPropertyBlock(mpb);
    }
}
