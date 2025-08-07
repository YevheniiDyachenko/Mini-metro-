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
    // `connections` stores outgoing connections: fromId -> [toId_1, toId_2, ...]
    private Dictionary<int, List<int>> connections = new Dictionary<int, List<int>>();
    // `inputConnections` stores incoming connections: toId -> [fromId_1, fromId_2, ...]
    private Dictionary<int, List<int>> inputConnections = new Dictionary<int, List<int>>();

    private int nextNodeId = 0;

    void Start()
    {
        Debug.Log("PipelineManager Initialized. Ready to build data flows.");
    }

    /// <summary>
    /// Registers a new node with the pipeline, assigning it a unique ID.
    /// </summary>
    /// <param name="node">The node to register.</param>
    public void AddNode(NodeBase node)
    {
        int newId = nextNodeId++;
        node.nodeData.id = newId;

        if (!nodes.ContainsKey(newId))
        {
            nodes.Add(newId, node);
            Debug.Log($"Node registered with new ID: {newId}, Type: {node.nodeData.nodeType}");
        }
        else
        {
            // This case should ideally never happen with dynamic IDs
            Debug.LogError($"CRITICAL: Duplicate Node ID generated: {newId}.");
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
            // Add to outgoing connections map
            if (!connections.ContainsKey(fromId))
            {
                connections[fromId] = new List<int>();
            }
            connections[fromId].Add(toId);

            // Add to incoming connections map
            if (!inputConnections.ContainsKey(toId))
            {
                inputConnections[toId] = new List<int>();
            }
            inputConnections[toId].Add(fromId);

            // Fire the event to notify listeners that a connection was made
            OnConnectionMade?.Invoke(nodes[fromId], nodes[toId]);
        }
        else
        {
            Debug.LogError($"Cannot connect nodes. One or both IDs not found: fromId={fromId}, toId={toId}");
        }
    }

    /// <summary>
    /// Calculates the total flow delivered to all data sinks in the pipeline.
    /// It initiates a recursive graph traversal for each sink.
    /// </summary>
    /// <returns>The total final flow rate.</returns>
    public float CalculateFlow()
    {
        float totalFlow = 0f;
        var memo = new Dictionary<int, float>(); // Memoization cache for this calculation run
        var path = new HashSet<int>(); // Path tracker for cycle detection

        foreach (var node in nodes.Values)
        {
            if (node is DataSinkNode)
            {
                totalFlow += CalculateNodeOutput(node.nodeData.id, memo, path);
            }
        }
        return totalFlow;
    }

    /// <summary>
    /// Recursively calculates the output flow of a single node using depth-first search.
    /// Uses memoization to avoid redundant calculations and a path tracker to prevent cycles.
    /// </summary>
    private float CalculateNodeOutput(int nodeId, Dictionary<int, float> memo, HashSet<int> path)
    {
        // 1. Memoization Check: If we've already calculated this node, return the stored value.
        if (memo.ContainsKey(nodeId))
        {
            return memo[nodeId];
        }

        // 2. Cycle Detection: If the node is already in our current calculation path, we have a cycle.
        if (path.Contains(nodeId))
        {
            Debug.LogWarning($"Cycle detected in pipeline involving node {nodeId}. Breaking loop.");
            return 0f; // Return 0 flow to break the infinite recursion.
        }

        // Add node to the current path
        path.Add(nodeId);

        NodeBase currentNode = nodes[nodeId];
        float outputFlow = 0f;

        // 2. Base Case: If the node is a data source, its output is its generation rate.
        if (currentNode is DataSourceNode dataSource)
        {
            outputFlow = dataSource.generationRate;
        }
        // 3. Recursive Step: For other nodes, calculate the sum of their inputs first.
        else
        {
            float totalInputFlow = 0f;
            if (inputConnections.ContainsKey(nodeId))
            {
                foreach (int inputNodeId in inputConnections[nodeId])
                {
                    totalInputFlow += CalculateNodeOutput(inputNodeId, memo, path);
                }
            }

            // Apply the current node's logic to its total input flow.
            if (currentNode is FilterNode filter)
            {
                outputFlow = totalInputFlow * filter.throughput;
            }
            else if (currentNode is AggregateNode) // or other node types
            {
                outputFlow = totalInputFlow; // An aggregator just sums its inputs.
            }
            else // Includes DataSinkNode
            {
                outputFlow = totalInputFlow;
            }
        }

        // 4. Cache Result: Store the result in the memo before returning.
        path.Remove(nodeId); // Remove node from path before returning
        memo[nodeId] = outputFlow;
        return outputFlow;
    }

    // Helper method to get a node by its ID.
    public NodeBase GetNodeById(int id)
    {
        nodes.TryGetValue(id, out NodeBase node);
        return node;
    }
}
