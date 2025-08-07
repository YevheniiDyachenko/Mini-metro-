using UnityEngine;

/// <summary>
/// Defines and manages the objectives and state for a single level.
/// It tracks the timer and checks for win/loss conditions.
/// </summary>
public class LevelManager : MonoBehaviour
{
    [Header("Level Objectives")]
    [Tooltip("The time limit for the level in seconds.")]
    public float timeLimit = 180f; // 3 minutes

    [Tooltip("The target data flow rate (e.g., in GB/s) to win the level.")]
    public float targetFlow = 100f;

    [Tooltip("The starting budget for the level.")]
    public float initialBudget = 5000f;

    [Header("Component References")]
    [Tooltip("Reference to the PipelineManager in the scene.")]
    public PipelineManager pipelineManager;

    // Internal state
    private float timeRemaining;
    private float currentBudget;
    private bool isLevelActive = false;

    void Start()
    {
        if (pipelineManager == null)
        {
            Debug.LogError("LevelManager requires a reference to the PipelineManager!");
            this.enabled = false; // Disable this component if dependencies are missing
            return;
        }

        InitializeLevel();
    }

    void InitializeLevel()
    {
        timeRemaining = timeLimit;
        currentBudget = initialBudget;
        isLevelActive = true;

        // Set initial HUD values via the DashboardManager Singleton
        if (DashboardManager.Instance != null)
        {
            DashboardManager.Instance.SetTime(timeRemaining);
            DashboardManager.Instance.SetBudget(currentBudget);
            DashboardManager.Instance.SetFlow(0);
        }

        Time.timeScale = 1f; // Ensure the game is running
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
        if (pipelineManager.CalculateFlow() >= targetFlow)
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
