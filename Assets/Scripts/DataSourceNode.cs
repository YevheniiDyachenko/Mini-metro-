using UnityEngine;

/// <summary>
/// Represents a node that is a source of data in the pipeline.
/// It inherits from NodeBase and provides a specific implementation for a data source.
/// </summary>
public class DataSourceNode : NodeBase
{
    [Header("Data Source Settings")]
    [Tooltip("The rate at which this node generates data (e.g., in GB/s).")]
    public float generationRate = 20f;

    void Awake()
    {
        // Ensure the underlying data module has the correct type and links capacity.
        if (nodeData != null)
        {
            nodeData.nodeType = "DataSource";
            // For a source node, its capacity is its generation rate.
            nodeData.capacity = generationRate;
        }
    }

    /// <summary>
    /// The specific logic for a data source node.
    /// In a real simulation, this might push data to its connected outputs.
    /// For now, its primary role is to contribute its generationRate to the total flow.
    /// </summary>
    public override void ProcessDataFlow()
    {
        // The actual processing might be managed by another class that
        // calls this method. For now, a log is sufficient.
        Debug.Log($"Data Source Node {nodeData.id}: Generating {generationRate} GB/s.");
    }
}
