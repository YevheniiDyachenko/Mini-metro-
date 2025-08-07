using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// A ScriptableObject that holds all the configuration data for a single level.
/// To create a new level, create a new asset of this type in the Unity Project window.
/// </summary>
[CreateAssetMenu(fileName = "Level_01", menuName = "Big Data Flow/Level Data", order = 1)]
public class LevelData : ScriptableObject
{
    [Header("Level Objectives")]
    [Tooltip("The time limit for the level in seconds.")]
    public float timeLimit = 180f;

    [Tooltip("The target data flow rate (e.g., in GB/s) required to win.")]
    public float targetFlow = 100f;

    [Tooltip("The starting budget for constructing the pipeline.")]
    public float initialBudget = 5000f;

    [Header("Initial Scene Setup")]
    [Tooltip("A list of all nodes to be spawned at the beginning of the level.")]
    public List<NodePlacementInfo> initialNodePlacements;

    /// <summary>
    /// A serializable class to hold the information needed to spawn a single node.
    /// </summary>
    [System.Serializable]
    public class NodePlacementInfo
    {
        [Tooltip("The prefab for the node (must have a NodeBase-derived component).")]
        public GameObject nodePrefab;

        [Tooltip("The world position where this node will be placed.")]
        public Vector2 position;

        [Tooltip("The unique ID to assign to this node's NodeModule.")]
        public int nodeId;

        // Future extension: could add initial capacity, cost override, etc.
        // public float startingCapacity;
    }
}
