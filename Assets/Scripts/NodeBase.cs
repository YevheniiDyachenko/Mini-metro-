using UnityEngine;

/// <summary>
/// The abstract base class for all functional node types in the data pipeline.
/// It defines the common structure and properties that all nodes must have.
/// This component should be on the main GameObject for any node.
/// </summary>
[RequireComponent(typeof(Collider2D))] // Ensures all nodes are clickable
public abstract class NodeBase : MonoBehaviour
{
    [Header("Node Configuration")]
    [Tooltip("The data module containing the core properties of this node.")]
    public NodeModule nodeData;

    /// <summary>
    /// Abstract method for processing data. Each concrete node type must implement
    /// its own logic for what happens when data flows through it.
    /// </summary>
    public abstract void ProcessDataFlow();

    /// <summary>
    /// A virtual method that can be called when the node is selected by the player.
    /// Subclasses can override this to provide specific visual feedback.
    /// </summary>
    public virtual void OnNodeSelected()
    {
        // Example: a simple log or could be a visual effect like an outline.
        Debug.Log($"Node {nodeData.id} ({nodeData.nodeType}) was selected.");
    }

    /// <summary>
    /// A virtual method that can be called when the node is deselected.
    /// </summary>
    public virtual void OnNodeDeselected()
    {
        // Example: could be used to remove a highlight effect.
        Debug.Log($"Node {nodeData.id} ({nodeData.nodeType}) was deselected.");
    }

    // You could also add common methods here like:
    // public virtual void UpgradeNode() { ... }
    // public virtual float GetCost() { return cost; }
}
