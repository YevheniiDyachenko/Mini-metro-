using UnityEngine;

/// <summary>
/// A concrete implementation of a transformation node that filters data.
/// It reduces the data flow that passes through it based on its throughput property.
/// </summary>
public class FilterNode : NodeBase
{
    [Header("Filter Settings")]
    [Tooltip("The percentage of data that passes through this filter. 1.0 means 100% passes through, 0.5 means 50% is filtered out.")]
    [Range(0f, 1f)]
    public float throughput = 0.8f; // Default: 80% of data passes through

    void Awake()
    {
        // Set the node type for identification by other systems.
        if (nodeData != null)
        {
            nodeData.nodeType = "Filter";
        }
    }

    /// <summary>
    /// The specific processing logic for a Filter Node.
    /// In a real pipeline, this would involve receiving data from inputs and
    /// passing the reduced amount to outputs.
    /// </summary>
    public override void ProcessDataFlow()
    {
        // The core calculation logic will be handled by the PipelineManager's traversal.
        // This method could be used for node-specific animations or status updates.
        Debug.Log($"Filter Node {nodeData.id} is active with a throughput of {throughput}.");
    }
}
