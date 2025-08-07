using UnityEngine;

/// <summary>
/// A concrete implementation of a node that aggregates (sums) data flows
/// from multiple input connections into a single output stream.
/// </summary>
public class AggregateNode : NodeBase
{
    void Awake()
    {
        // Set the node type for identification by other systems.
        if (nodeData != null)
        {
            nodeData.nodeType = "Aggregate";
        }
    }

    /// <summary>
    /// The specific processing logic for an Aggregate Node.
    /// The actual summing of inputs will be handled by the PipelineManager's
    /// new graph traversal logic. This method is a placeholder for any unique
    /// visual or state-based behavior for this node type.
    /// </summary>
    public override void ProcessDataFlow()
    {
        // This method could, for example, trigger a specific animation on the
        // NodeAnimator to show that it's actively combining data.
        Debug.Log($"Aggregate Node {nodeData.id} is actively combining its input streams.");
    }
}
