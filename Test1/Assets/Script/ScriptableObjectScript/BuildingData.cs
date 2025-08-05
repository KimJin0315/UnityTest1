using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ResourceCost
{
    public string resourceName;
    public int amount;
}

public enum UnlockType
{
    None,
    CraftingTable,
    Furnace,
    Ship,
    Fishing,
    Farming,
    Mining
}


[CreateAssetMenu(fileName = "NewBuildingData", menuName = "Building/BuildingData")]
public class BuildingData : ScriptableObject
{
    public string buildingName;
    public int width, height;

    public List<ResourceCost> resourceCosts = new List<ResourceCost>();

    public GameObject prefab;
    public GameObject previewPrefab;
    public BuildingData nextUnlock;
    public Sprite icon;

    public UnlockType unlockType;
}
