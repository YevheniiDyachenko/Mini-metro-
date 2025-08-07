using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages the main game HUD, displaying global information like timer, budget, and flow.
/// Implements the Singleton pattern to provide easy, global access for other scripts.
/// </summary>
public class DashboardManager : MonoBehaviour
{
    // Singleton instance
    public static DashboardManager Instance { get; private set; }

    [Header("HUD Text Elements")]
    [Tooltip("Text element for the level timer.")]
    public Text timerText;

    [Tooltip("Text element for the player's remaining budget.")]
    public Text budgetText;

    [Tooltip("Text element for the total data flow.")]
    public Text flowText;

    void Awake()
    {
        // Standard Singleton implementation
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    /// <summary>
    /// Updates the timer text on the HUD.
    /// </summary>
    /// <param name="remainingSeconds">The time left in seconds.</param>
    public void SetTime(float remainingSeconds)
    {
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(remainingSeconds / 60);
            int seconds = Mathf.FloorToInt(remainingSeconds % 60);
            timerText.text = $"Time: {minutes:00}:{seconds:00}";
        }
    }

    /// <summary>
    /// Updates the budget text on the HUD.
    /// </summary>
    /// <param name="currentBudget">The current budget value.</param>
    public void SetBudget(float currentBudget)
    {
        if (budgetText != null)
        {
            budgetText.text = $"Budget: ${currentBudget:N0}";
        }
    }

    /// <summary>
    /// Updates the data flow text on the HUD.
    /// </summary>
    /// <param name="currentFlow">The current data flow rate.</param>
    public void SetFlow(float currentFlow)
    {
        if (flowText != null)
        {
            flowText.text = $"Flow: {currentFlow:F1} GB/s";
        }
    }
}
