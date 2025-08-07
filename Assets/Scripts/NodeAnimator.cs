using UnityEngine;
using System.Collections;

/// <summary>
/// Handles simple animations on a node to provide visual feedback for events
/// like selection, deselection, and data processing.
/// This component should be on the same GameObject as the NodeBase.
/// </summary>
public class NodeAnimator : MonoBehaviour
{
    [Header("Visual Components")]
    [Tooltip("The SpriteRenderer to change the color of.")]
    public SpriteRenderer nodeSprite;

    [Header("Animation Properties")]
    [Tooltip("The color to apply when the node is selected.")]
    public Color selectedColor = Color.yellow;
    [Tooltip("The scale multiplier to apply when the node is selected.")]
    public float selectedScaleMultiplier = 1.15f;
    [Tooltip("The duration of the selection/deselection animation.")]
    public float transitionDuration = 0.1f;

    private Color originalColor;
    private Vector3 originalScale;
    private Coroutine activeAnimation;

    void Start()
    {
        if (nodeSprite == null)
        {
            nodeSprite = GetComponent<SpriteRenderer>();
        }
        originalColor = nodeSprite.color;
        originalScale = transform.localScale;
    }

    /// <summary>
    /// Plays the selection animation.
    /// </summary>
    public void PlaySelectionAnimation()
    {
        if (activeAnimation != null) StopCoroutine(activeAnimation);
        activeAnimation = StartCoroutine(AnimateToState(selectedColor, originalScale * selectedScaleMultiplier));
    }

    /// <summary>
    /// Plays the deselection animation, returning the node to its original state.
    /// </summary>
    public void PlayDeselectionAnimation()
    {
        if (activeAnimation != null) StopCoroutine(activeAnimation);
        activeAnimation = StartCoroutine(AnimateToState(originalColor, originalScale));
    }

    /// <summary>
    /// A generic coroutine to smoothly transition color and scale over time.
    /// </summary>
    private IEnumerator AnimateToState(Color targetColor, Vector3 targetScale)
    {
        float timer = 0f;
        Color startingColor = nodeSprite.color;
        Vector3 startingScale = transform.localScale;

        while (timer < transitionDuration)
        {
            timer += Time.deltaTime;
            float progress = Mathf.Clamp01(timer / transitionDuration);

            nodeSprite.color = Color.Lerp(startingColor, targetColor, progress);
            transform.localScale = Vector3.Lerp(startingScale, targetScale, progress);

            yield return null;
        }

        // Ensure final state is set
        nodeSprite.color = targetColor;
        transform.localScale = targetScale;
    }

    /// <summary>
    /// Plays a quick "pulse" animation to indicate processing.
    /// </summary>
    public void PlayProcessingPulse()
    {
        if (activeAnimation != null) StopCoroutine(activeAnimation);
        activeAnimation = StartCoroutine(PulseCoroutine());
    }

    private IEnumerator PulseCoroutine()
    {
        // For a pulse, we can just quickly scale up and back down.
        Vector3 pulseScale = originalScale * selectedScaleMultiplier;
        yield return AnimateToState(selectedColor, pulseScale);
        yield return AnimateToState(originalColor, originalScale);
    }
}
