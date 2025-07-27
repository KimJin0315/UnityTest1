using UnityEngine;
using System.Collections.Generic;

public class PlayerInventory : MonoBehaviour
{
    // ✅ ResourceData 리스트 (Inspector에 넣어서 순서 관리)
    public List<ResourceData> resourceTypes;

    // ✅ 실제 자원 개수 저장
    private Dictionary<string, int> resources = new Dictionary<string, int>();

    void Start()
    {
        // ✅ 시작 시 Dictionary 초기화 (0으로)
        foreach (ResourceData data in resourceTypes)
        {
            resources[data.resourceName] = 0;
        }
    }

    public void AddResource(string resourceName, int amount)
    {
        if (!resources.ContainsKey(resourceName))
            resources[resourceName] = 0;

        resources[resourceName] += amount;
        Debug.Log($"{resourceName} +{amount} (총 {resources[resourceName]})");
    }

    public bool HasResource(string resourceName, int amount)
    {
        return resources.ContainsKey(resourceName) && resources[resourceName] >= amount;
    }

    public void UseResource(string resourceName, int amount)
    {
        if (HasResource(resourceName, amount))
        {
            resources[resourceName] -= amount;
        }
    }

    public int GetResourceAmount(string resourceName)
    {
        return resources.ContainsKey(resourceName) ? resources[resourceName] : 0;
    }
}
