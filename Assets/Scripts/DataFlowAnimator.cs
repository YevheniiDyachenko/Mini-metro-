using UnityEngine;

/// <summary>
/// Animates a particle effect or sprite along the path of a LineRenderer
/// to give the visual impression of data flowing through a connection.
/// This component should be on the same GameObject as the permanent line's LineRenderer.
/// </summary>
[RequireComponent(typeof(LineRenderer))]
public class DataFlowAnimator : MonoBehaviour
{
    [Header("Animation Settings")]
    [Tooltip("The speed at which the particle travels along the line.")]
    public float speed = 3.0f;

    private LineRenderer lineRenderer;
    private float journeyLength;
    private float journeyStartTime;

    private Transform particleTransform;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();

        // Auto-detect the particle transform if it's a child.
        if (transform.childCount > 0)
        {
            particleTransform = transform.GetChild(0);
        }
        else
        {
            Debug.LogError("DataFlowAnimator requires a child object to act as the particle!", this);
            enabled = false;
            return;
        }

        // Initialize the journey
        journeyLength = Vector3.Distance(lineRenderer.GetPosition(0), lineRenderer.GetPosition(1));
        RestartJourney();
    }

    void Update()
    {
        if (journeyLength <= 0) return;

        // Calculate the distance covered based on time and speed.
        float distCovered = (Time.time - journeyStartTime) * speed;

        // Calculate the fraction of the journey completed.
        float fractionOfJourney = distCovered / journeyLength;

        // Use Lerp to find the current position along the line.
        particleTransform.position = Vector3.Lerp(lineRenderer.GetPosition(0), lineRenderer.GetPosition(1), fractionOfJourney);

        // When the particle reaches the end, restart the journey from the beginning.
        if (fractionOfJourney >= 1f)
        {
            RestartJourney();
        }
    }

    /// <summary>
    /// Resets the animation to the starting point.
    /// </summary>
    private void RestartJourney()
    {
        journeyStartTime = Time.time;
        particleTransform.position = lineRenderer.GetPosition(0);
    }
}
