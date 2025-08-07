using UnityEngine;

/// <summary>
/// Auto-generated Service for handling random game events and power-ups.
/// This class can trigger failures, apply boosts, and manage other dynamic occurrences.
/// Corresponds to: jg generate service EventController --methods "TriggerFailure(NodeModule), ApplyPowerUp(string type, NodeModule target)"
/// </summary>
public class EventController : MonoBehaviour
{
    void Start()
    {
        Debug.Log("EventController Initialized. Ready to handle random events.");
    }

    /// <summary>
    /// Triggers a failure event on a specific node in the pipeline.
    /// </summary>
    /// <param name="node">The node that will be affected by the failure.</param>
    public void TriggerFailure(NodeModule node)
    {
        if (node != null)
        {
            Debug.LogWarning($"Event: A failure has been triggered on node ID: {node.id} ({node.nodeType})!");
            // In a full game, this would trigger gameplay effects, such as:
            // - Halving the node's capacity temporarily.
            // - Disabling the node for a few seconds.
            // - Triggering a visual alert on the UI.
            // Example: if(node.nodeObject != null) node.nodeObject.GetComponent<Animator>().SetTrigger("FailureState");
        }
        else
        {
            Debug.LogError("TriggerFailure was called with a null node.");
        }
    }

    /// <summary>
    /// Applies a specified power-up to a target node.
    /// </summary>
    /// <param name="type">A string identifying the power-up, e.g., "Turbo-Transform".</param>
    /// <param name="target">The node to apply the power-up to.</param>
    public void ApplyPowerUp(string type, NodeModule target)
    {
        if (target != null)
        {
            Debug.Log($"Event: Applying power-up '{type}' to node ID: {target.id}!");
            // In a full game, a switch statement or factory would handle different power-up types.
            // switch(type)
            // {
            //    case "Turbo-Transform":
            //        // Logic to double the node's capacity for 10 seconds.
            //        break;
            //    case "Swap-Stream":
            //        // Logic to handle stream swapping.
            //        break;
            // }
        }
        else
        {
            Debug.LogError("ApplyPowerUp was called with a null target node.");
        }
    }
}
