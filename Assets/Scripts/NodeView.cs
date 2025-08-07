using UnityEngine;

/// <summary>
/// A component to be placed on any GameObject that represents a node in the scene.
/// It acts as a bridge between the visual object in the game world and its
/// underlying data model (NodeModule).
/// </summary>
public class NodeView : MonoBehaviour
{
    // This reference would be assigned when the node is instantiated,
    // linking the visual GameObject to its logical data.
    public NodeModule nodeData;
}
