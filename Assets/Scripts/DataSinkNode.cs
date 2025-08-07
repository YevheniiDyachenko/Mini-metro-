using UnityEngine;

/// <summary>
/// Represents a node that is a destination or "sink" for data in the pipeline.
/// It inherits from NodeBase and provides a specific implementation for a data sink.
/// </summary>
public class DataSinkNode : NodeBase
{
    [Header("Data Sink Status")]
    [Tooltip("The total amount of data this sink has successfully received.")]
    public float totalDataReceived = 0f;

    void Awake()
    {
        // Ensure the underlying data module has the correct type.
        if (nodeData != null)
        {
            nodeData.nodeType = "DataSink";
        }
    }

    /// <summary>
    /// The specific logic for a data sink node. In a real simulation, this node
    /// would be passive, and data would be "pushed" to it from connected nodes.
    /// </summary>
    public override void ProcessDataFlow()
    {
        // A sink node doesn't actively process data in the same way a transform node does.
        // Its main job is to be a valid endpoint. The PipelineManager would handle
        // the logic of data reaching a sink.
        Debug.Log($"Data Sink Node {nodeData.id}: Awaiting data delivery.");
    }

    /// <summary>
    /// A method that could be called by the pipeline simulation to deliver data.
    /// </summary>
    /// <param name="amount">The amount of data being delivered.</param>
    public void DeliverData(float amount)
    {
        totalDataReceived += amount;
        Debug.Log($"Data Sink {nodeData.id} has now received a total of {totalDataReceived} data.");
    }
}
