using UnityEngine;

/// <summary>
/// Manages the state of a single level, driven by a LevelData ScriptableObject.
/// It initializes the level, tracks objectives, and checks for win/loss conditions.
/// </summary>
public class LevelManager : MonoBehaviour
{
    [Header("Level Configuration")]
    [Tooltip("The ScriptableObject that defines the current level's properties.")]
    public LevelData currentLevelData;

    [Header("Component References")]
    [Tooltip("Reference to the PipelineManager in the scene.")]
    public PipelineManager pipelineManager;

    // Internal state
    private float timeRemaining;
    private float currentBudget;
    private bool isLevelActive = false;

    void Start()
    {
        if (currentLevelData == null || pipelineManager == null)
        {
            Debug.LogError("LevelManager is missing critical references (LevelData or PipelineManager)!");
            this.enabled = false;
            return;
        }

        InitializeLevel();
    }

    void InitializeLevel()
    {
        // Load objectives from the LevelData ScriptableObject
        timeRemaining = currentLevelData.timeLimit;
        currentBudget = currentLevelData.initialBudget;
        isLevelActive = true;

        // Set initial HUD values
        if (DashboardManager.Instance != null)
        {
            DashboardManager.Instance.SetTime(timeRemaining);
            DashboardManager.Instance.SetBudget(currentBudget);
            DashboardManager.Instance.SetFlow(0);
        }

        // Spawn the initial nodes defined in LevelData
        SpawnInitialNodes();

        Time.timeScale = 1f; // Ensure the game is running
    }

    void SpawnInitialNodes()
    {
        foreach (var nodeInfo in currentLevelData.initialNodePlacements)
        {
            if (nodeInfo.nodePrefab == null) continue;

            // Instantiate the node prefab at the specified position
            GameObject nodeObject = Instantiate(nodeInfo.nodePrefab, nodeInfo.position, Quaternion.identity);
            nodeObject.name = $"{nodeInfo.nodePrefab.name}_{nodeInfo.nodeId}";

            // Get the NodeBase component and initialize its data
            NodeBase nodeBase = nodeObject.GetComponent<NodeBase>();
            if (nodeBase != null)
            {
                // This assumes the NodeModule is already created or will be created.
                // A better approach might be to have NodeModule as a struct.
                // For now, let's create a new one if it's null.
                if(nodeBase.nodeData == null) nodeBase.nodeData = new NodeModule(nodeInfo.nodeId, "", 0, nodeObject);
                nodeBase.nodeData.id = nodeInfo.nodeId;

                // Register the new node with the pipeline manager
                pipelineManager.AddNode(nodeBase);
            }
        }
    }

    void Update()
    {
        if (!isLevelActive) return;

        // Update timer
        timeRemaining -= Time.deltaTime;
        if (DashboardManager.Instance != null)
        {
            DashboardManager.Instance.SetTime(timeRemaining);
        }

        // Check for win/loss conditions
        CheckLevelStatus();
    }

    void CheckLevelStatus()
    {
        // Loss condition: Time runs out
        if (timeRemaining <= 0)
        {
            EndLevel(false); // Pass 'false' for loss
            return;
        }

        // Win condition: Target flow is met or exceeded
        if (pipelineManager.CalculateFlow() >= currentLevelData.targetFlow)
        {
            EndLevel(true); // Pass 'true' for win
        }
    }

    /// <summary>
    /// Ends the level and triggers the win or loss state.
    /// </summary>
    /// <param name="isWin">True if the level was won, false otherwise.</param>
    void EndLevel(bool isWin)
    {
        isLevelActive = false;
        Time.timeScale = 0f; // Pause the game

        if (isWin)
        {
            Debug.Log("LEVEL COMPLETE! You have successfully met the objective.");
            // Here you would trigger a 'Level Won' UI screen.
        }
        else
        {
            Debug.Log("LEVEL FAILED! You ran out of time.");
            // Here you would trigger a 'Level Lost' UI screen.
        }
    }

    /// <summary>
    /// Call this method when a player action costs money.
    /// </summary>
    /// <param name="amount">The amount to deduct from the budget.</param>
    public bool SpendBudget(float amount)
    {
        if (currentBudget >= amount)
        {
            currentBudget -= amount;
            if (DashboardManager.Instance != null)
            {
                DashboardManager.Instance.SetBudget(currentBudget);
            }
            return true;
        }
        return false; // Not enough budget
    }
}
