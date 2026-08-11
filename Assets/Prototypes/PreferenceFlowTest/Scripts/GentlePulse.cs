using UnityEngine;

/// <summary>Small continuous scale pulse so orbs/crystals don't read as static/flat.</summary>
public class GentlePulse : MonoBehaviour
{
    [SerializeField] private float amount = 0.06f;
    [SerializeField] private float speed = 1.6f;
    private Vector3 baseScale;
    private float phase;

    private void Start()
    {
        baseScale = transform.localScale;
        phase = Random.Range(0f, 10f);
    }

    private void Update()
    {
        float s = 1f + Mathf.Sin((Time.time + phase) * speed) * amount;
        transform.localScale = baseScale * s;
    }
}
