using UnityEngine;
using UnityEngine.UI;

public class ResourceUI : MonoBehaviour
{
    public PlayerInventory inventory;
    public Text[] resourceTexts;

    void Update()
    {
        // ✅ 안전 코드: 더 적은 쪽에 맞춰 반복
        int count = Mathf.Min(inventory.resourceTypes.Count, resourceTexts.Length);

        for (int i = 0; i < count; i++)
        {
            ResourceData data = inventory.resourceTypes[i];
            int amount = inventory.GetResourceAmount(data.resourceName);

            resourceTexts[i].text = $"{data.displayName}: {amount}";
        }
    }
}
