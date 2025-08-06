using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CraftingUIManager : MonoBehaviour
{
    [Header("Crafting Button")]
    public GameObject craftingButtonPrefab;
    public Transform craftingGridParent;

    [Header("Buildings")]
    public BuildingData[] craftingBuildings;

    private Dictionary<BuildingData, bool> unlocked = new Dictionary<BuildingData, bool>();

    void Start()
    {
        foreach (BuildingData data in craftingBuildings)
        {
            GameObject btnObj = Instantiate(craftingButtonPrefab, craftingGridParent);
            btnObj.transform.Find("Icon").GetComponent<Image>().sprite = data.icon;
            btnObj.transform.Find("Label").GetComponent<Text>().text = data.buildingName;

            Button btn = btnObj.GetComponent<Button>();
            btn.onClick.AddListener(() =>
            {
                Debug.Log($"[클릭] {data.buildingName} 버튼 클릭됨");
                FindFirstObjectByType<BuildingPlacer>().StartPlacement(data);
            });

            btn.interactable = false;

            CraftingButton cb = btnObj.AddComponent<CraftingButton>();
            cb.buildingData = data;

            unlocked[data] = false;
        }

        if (craftingBuildings.Length > 0)
        {
            UnlockBuilding(craftingBuildings[0]);
        }
    }

    public void UnlockCraftingButton(string buildingName)
    {
        foreach (Transform child in craftingGridParent)
        {
            CraftingButton cb = child.GetComponent<CraftingButton>();
            if (cb != null && cb.buildingData != null && cb.buildingData.buildingName == buildingName)
            {
                child.GetComponent<Button>().interactable = true;
                Debug.Log($"🔓 {buildingName} 버튼 해금 완료!");
                break;
            }
        }
    }

    public void UnlockBuilding(BuildingData data)
    {
        if (!unlocked.ContainsKey(data))
        {
            Debug.LogWarning($"⚠️ 알 수 없는 건물 데이터: {data.buildingName}");
            return;
        }

        if (unlocked[data])
        {
            Debug.Log($"ℹ️ 이미 해금됨: {data.buildingName}");
            return;
        }

        unlocked[data] = true;

        foreach (Transform child in craftingGridParent)
        {
            CraftingButton cb = child.GetComponent<CraftingButton>();
            if (cb != null && cb.buildingData == data)
            {
                child.GetComponent<Button>().interactable = true;
                Debug.Log($"🔓 {data.buildingName} 해금 완료!");
                break;
            }
        }
    }
}