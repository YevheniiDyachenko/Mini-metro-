using UnityEngine;

/// <summary>
/// Handles all player input for interacting with the data pipeline.
/// It detects clicks on nodes and manages the process of creating connections.
/// This script assumes a 2D environment with 2D colliders on the node objects.
/// </summary>
public class PlayerInputController : MonoBehaviour
{
    [Header("Dependencies")]
    [Tooltip("Reference to the PipelineManager for creating logical connections.")]
    public PipelineManager pipelineManager;

    [Tooltip("Reference to the LineDrawer for showing the connection being made.")]
    public LineDrawer lineDrawer;

    private Camera mainCamera;
    private NodeBase startNode; // The node where the drag started
    private bool isDrawingConnection = false;

    void Start()
    {
        mainCamera = Camera.main;
        if (pipelineManager == null || lineDrawer == null)
        {
            Debug.LogError("PlayerInputController is missing critical dependencies (PipelineManager or LineDrawer)!");
            this.enabled = false;
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandleMouseDown();
        }

        if (isDrawingConnection)
        {
            UpdateLineToMouse();
        }

        if (Input.GetMouseButtonUp(0))
        {
            HandleMouseUp();
        }
    }

    private void HandleMouseDown()
    {
        NodeBase targetNode = GetNodeUnderMouse();
        if (targetNode != null)
        {
            isDrawingConnection = true;
            startNode = targetNode;
            lineDrawer.StartDrawing(startNode.transform.position);
        }
    }

    private void UpdateLineToMouse()
    {
        Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0; // Ensure it's in the 2D plane
        lineDrawer.UpdateLinePosition(mousePosition);
    }

    private void HandleMouseUp()
    {
        if (!isDrawingConnection) return;

        NodeBase endNode = GetNodeUnderMouse();

        // Check if the mouse was released over a valid, different node
        if (endNode != null && endNode != startNode)
        {
            // Create the logical connection in the backend
            pipelineManager.ConnectNodes(startNode.nodeData.id, endNode.nodeData.id);

            // Here you would typically instantiate a permanent line visual
            // For now, the logical connection is made, but not visualized permanently.
            Debug.Log($"Connection created between node {startNode.nodeData.id} and {endNode.nodeData.id}");
        }

        // Stop drawing the temporary line regardless of success
        lineDrawer.StopDrawing();

        // Reset state
        isDrawingConnection = false;
        startNode = null;
    }

    /// <summary>
    /// Uses a 2D raycast to find and return a NodeBase component under the mouse cursor.
    /// </summary>
    /// <returns>The NodeBase if found, otherwise null.</returns>
    private NodeBase GetNodeUnderMouse()
    {
        Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

        if (hit.collider != null)
        {
            // Check if the hit object has a NodeBase component
            return hit.collider.GetComponent<NodeBase>();
        }

        return null;
    }
}
