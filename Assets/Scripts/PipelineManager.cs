using UnityEngine;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Auto-generated Service for managing data pipelines.
/// This class handles the creation, connection, and calculation of data flows.
/// Corresponds to: jg generate service PipelineManager --methods "AddNode(NodeModule), ConnectNodes(int fromId,int toId), CalculateFlow():float"
/// </summary>
public class PipelineManager : MonoBehaviour
{
    private Dictionary<int, NodeModule> nodes = new Dictionary<int, NodeModule>();
    // The connections are stored as a dictionary where the key is the source node ID
    // and the value is a list of target node IDs.
    private Dictionary<int, List<int>> connections = new Dictionary<int, List<int>>();

    void Start()
    {
        Debug.Log("PipelineManager Initialized. Ready to build data flows.");
    }

    /// <summary>
    /// Adds a node to the pipeline.
    /// </summary>
    /// <param name="node">The node to add.</param>
    public void AddNode(NodeModule node)
    {
        if (!nodes.ContainsKey(node.id))
        {
            nodes.Add(node.id, node);
            Debug.Log($"Node added: ID={node.id}, Type={node.nodeType}");
        }
        else
        {
            Debug.LogWarning($"Node with ID {node.id} already exists.");
        }
    }

    /// <summary>
    /// Connects two nodes in the pipeline, creating a directed edge.
    /// </summary>
    /// <param name="fromId">The ID of the source node.</param>
    /// <param name="toId">The ID of the target node.</param>
    public void ConnectNodes(int fromId, int toId)
    {
        if (nodes.ContainsKey(fromId) && nodes.ContainsKey(toId))
        {
            if (!connections.ContainsKey(fromId))
            {
                connections[fromId] = new List<int>();
            }
            connections[fromId].Add(toId);
            Debug.Log($"Connection created: {fromId} -> {toId}");
        }
        else
        {
            Debug.LogError($"Cannot connect nodes. One or both IDs not found: fromId={fromId}, toId={toId}");
        }
    }

    /// <summary>
    /// Calculates the total flow of the pipeline based on some logic.
    /// This is a placeholder for the actual game logic.
    /// </summary>
    /// <returns>A float representing the total calculated flow.</returns>
    public float CalculateFlow()
    {
        // Example logic: Sum of capacities of all "Source" nodes.
        float totalFlow = 0f;
        foreach (var node in nodes.Values)
        {
            if (node.nodeType == "DataSource")
            {
                totalFlow += node.capacity;
            }
        }
        Debug.Log($"Total calculated flow is: {totalFlow}");
        return totalFlow;
    }

    // Helper method to get a node by its ID, could be useful.
    public NodeModule GetNodeById(int id)
    {
        nodes.TryGetValue(id, out NodeModule node);
        return node;
    }
}
