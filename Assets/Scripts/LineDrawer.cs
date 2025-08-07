using UnityEngine;

/// <summary>
/// Visually renders a line using a LineRenderer component. This is used to give
/// the player feedback when they are dragging to create a connection between nodes.
/// This script should be attached to a dedicated GameObject in the scene.
/// </summary>
[RequireComponent(typeof(LineRenderer))]
public class LineDrawer : MonoBehaviour
{
    private LineRenderer lineRenderer;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        SetupLineRenderer();
    }

    /// <summary>
    /// Sets the default properties for the LineRenderer on startup.
    /// </summary>
    private void SetupLineRenderer()
    {
        lineRenderer.positionCount = 2; // A simple line has a start and an end.
        lineRenderer.startWidth = 0.15f;
        lineRenderer.endWidth = 0.15f;

        // A simple unlit material is good for UI lines.
        lineRenderer.material = new Material(Shader.Find("Legacy Shaders/Particles/Alpha Blended Premultiply"));

        // A bright color to make the line visible.
        lineRenderer.startColor = Color.yellow;
        lineRenderer.endColor = Color.yellow;

        // The line should not be visible initially.
        lineRenderer.enabled = false;
    }

    /// <summary>
    /// Activates the line and sets its starting point.
    /// </summary>
    /// <param name="startPosition">The world-space position where the line should begin.</param>
    public void StartDrawing(Vector3 startPosition)
    {
        // Set both start and end points to the same position initially
        lineRenderer.SetPosition(0, startPosition);
        lineRenderer.SetPosition(1, startPosition);
        lineRenderer.enabled = true;
    }

    /// <summary>
    /// Updates the end position of the line, typically to follow the mouse cursor.
    /// </summary>
    /// <param name="endPosition">The new world-space position for the end of the line.</param>
    public void UpdateLinePosition(Vector3 endPosition)
    {
        if (lineRenderer.enabled)
        {
            lineRenderer.SetPosition(1, endPosition);
        }
    }

    /// <summary>
    /// Hides the line from view.
    /// </summary>
    public void StopDrawing()
    {
        lineRenderer.enabled = false;
    }
}
