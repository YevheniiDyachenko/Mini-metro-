using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Handles the visualization of permanent pipeline connections.
/// It listens for when connections are successfully made and draws a line for each one.
/// </summary>
public class PipelineVisualizer : MonoBehaviour
{
    [Header("Visuals")]
    [Tooltip("The prefab to use for drawing a line. It must have a LineRenderer component.")]
    public GameObject linePrefab;

    // A dictionary to keep track of the lines we've created, in case we need to manage them later.
    private Dictionary<(int, int), LineRenderer> activeLines = new Dictionary<(int, int), LineRenderer>();

    void OnEnable()
    {
        // Subscribe to the static event from PipelineManager
        PipelineManager.OnConnectionMade += HandleConnectionMade;
    }

    void OnDisable()
    {
        // Always unsubscribe from static events to prevent memory leaks
        PipelineManager.OnConnectionMade -= HandleConnectionMade;
    }

    /// <summary>
    /// The event handler that is called when a new connection is created.
    /// </summary>
    /// <param name="fromNode">The node where the connection starts.</param>
    /// <param name="toNode">The node where the connection ends.</param>
    private void HandleConnectionMade(NodeBase fromNode, NodeBase toNode)
    {
        if (linePrefab == null)
        {
            Debug.LogError("PipelineVisualizer is missing the Line Prefab!", this);
            return;
        }

        var connectionKey = (fromNode.nodeData.id, toNode.nodeData.id);
        if (activeLines.ContainsKey(connectionKey))
        {
            // Connection already visualized, no need to draw again.
            return;
        }

        GameObject lineObject = Instantiate(linePrefab, transform);
        lineObject.name = $"Connection_{fromNode.nodeData.id}-{toNode.nodeData.id}";

        LineRenderer lineRenderer = lineObject.GetComponent<LineRenderer>();
        if (lineRenderer != null)
        {
            lineRenderer.SetPosition(0, fromNode.transform.position);
            lineRenderer.SetPosition(1, toNode.transform.position);
            activeLines.Add(connectionKey, lineRenderer);
        }
        else
        {
            Debug.LogError("The assigned Line Prefab is missing a LineRenderer component!", linePrefab);
            Destroy(lineObject); // Clean up the instantiated object if it's invalid.
        }
    }

    // Future improvement: Add a method to handle node deletion/disconnection
    // public void RemoveConnection(NodeBase fromNode, NodeBase toNode) { ... }
}
