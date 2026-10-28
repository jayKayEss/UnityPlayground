using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class Reticule : MonoBehaviour
{
    public float minDisplayScale = 0.25f;
    public float maxDisplayScale = 1.0f;
    private Image sprite;

    private void Awake()
    {
        sprite = GetComponent<Image>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CueStick.OnCueTarget += OnCueTarget;
        CueStick.OnCueTargetLost += OnCueTargetLost;
    }

    void OnDestroy()
    {
        CueStick.OnCueTarget -= OnCueTarget;
        CueStick.OnCueTargetLost -= OnCueTargetLost;
    }

    private void OnCueTarget(Vector3 point, float force)
    {
        float displayScale = Mathf.Lerp(minDisplayScale, maxDisplayScale, force);

        transform.localScale = new(displayScale, displayScale);
        transform.position = Camera.main.WorldToScreenPoint(point);

        sprite.enabled = true;
    }

    private void OnCueTargetLost()
    {
        sprite.enabled = false;
    }
}
