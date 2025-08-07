using UnityEngine;

/// <summary>
/// Auto-generated Module for a data processing node.
/// This class represents a single node in the data pipeline.
/// Corresponds to: jg generate module NodeModule --properties "id:int, nodeType:string, capacity:float"
/// </summary>
[System.Serializable]
public class NodeModule
{
    public int id;
    public string nodeType; // Could be "DataSource", "Transformation", "DataSink"
    public float capacity; // Represents processing power, throughput, etc.

    // A reference to the actual GameObject in the scene
    public GameObject nodeObject;

    public NodeModule(int id, string nodeType, float capacity, GameObject nodeObject)
    {
        this.id = id;
        this.nodeType = nodeType;
        this.capacity = capacity;
        this.nodeObject = nodeObject;
    }
}
