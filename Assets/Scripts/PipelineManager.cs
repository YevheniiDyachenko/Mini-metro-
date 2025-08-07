using UnityEngine;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Manages the logical state of the data pipeline, including all nodes and connections.
/// </summary>
public class PipelineManager : MonoBehaviour
{
    // Event to notify other systems (like the visualizer) when a connection is made.
    public static event System.Action<NodeBase, NodeBase> OnConnectionMade;

    private Dictionary<int, NodeBase> nodes = new Dictionary<int, NodeBase>();
    // The connections are stored as a dictionary where the key is the source node ID
    // and the value is a list of target node IDs.
    private Dictionary<int, List<int>> connections = new Dictionary<int, List<int>>();

    void Start()
    {
        Debug.Log("PipelineManager Initialized. Ready to build data flows.");
    }

    /// <summary>
    /// Adds a node to the pipeline. This should be called when a node is instantiated.
    /// </summary>
    /// <param name="node">The node to add.</param>
    public void AddNode(NodeBase node)
    {
        if (!nodes.ContainsKey(node.nodeData.id))
        {
            nodes.Add(node.nodeData.id, node);
            Debug.Log($"Node added: ID={node.nodeData.id}, Type={node.nodeData.nodeType}");
        }
        else
        {
            Debug.LogWarning($"Node with ID {node.nodeData.id} already exists.");
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

            // Fire the event to notify listeners that a connection was made
            OnConnectionMade?.Invoke(nodes[fromId], nodes[toId]);
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
            if (node.nodeData.nodeType == "DataSource")
            {
                totalFlow += node.nodeData.capacity;
            }
        }
        // Disabling this log as it's called every frame by the LevelManager
        // Debug.Log($"Total calculated flow is: {totalFlow}");
        return totalFlow;
    }

    // Helper method to get a node by its ID.
    public NodeBase GetNodeById(int id)
    {
        nodes.TryGetValue(id, out NodeBase node);
        return node;
    }
}
