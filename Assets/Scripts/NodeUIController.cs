using UnityEngine;
using UnityEngine.UI; // Include the standard UI namespace

/// <summary>
/// Manages a UI panel that displays information about a selected data node.
/// This should be attached to a UI Panel GameObject. The public Text fields
/// should be linked to Text components within that panel in the Unity Editor.
/// </summary>
public class NodeUIController : MonoBehaviour
{
    [Header("UI Text Fields")]
    [Tooltip("Text element to display the node's ID.")]
    public Text nodeIdText;

    [Tooltip("Text element to display the node's type.")]
    public Text nodeTypeText;

    [Tooltip("Text element to display the node's capacity.")]
    public Text nodeCapacityText;

    // The node currently being displayed by this UI.
    private NodeModule currentTargetNode;

    void Awake()
    {
        // It's good practice to start with the panel hidden.
        // A manager class would call Show() when a node is selected.
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Displays the UI panel and populates it with information from a specific node.
    /// </summary>
    /// <param name="nodeToShow">The NodeModule to display.</param>
    public void ShowNodeInfo(NodeModule nodeToShow)
    {
        if (nodeToShow == null)
        {
            Debug.LogError("ShowNodeInfo called with a null node.");
            return;
        }

        currentTargetNode = nodeToShow;
        UpdatePanel();
        gameObject.SetActive(true);
    }

    /// <summary>
    /// Hides the UI panel.
    /// </summary>
    public void HidePanel()
    {
        gameObject.SetActive(false);
        currentTargetNode = null;
    }

    /// <summary>
    /// Updates the UI text elements with the data from the currentTargetNode.
    /// </summary>
    private void UpdatePanel()
    {
        if (currentTargetNode == null) return;

        if (nodeIdText != null)
            nodeIdText.text = $"Node ID: {currentTargetNode.id}";

        if (nodeTypeText != null)
            nodeTypeText.text = $"Type: {currentTargetNode.nodeType}";

        if (nodeCapacityText != null)
            nodeCapacityText.text = $"Capacity: {currentTargetNode.capacity.ToString("N1")}";
    }
}
