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

    private Dictionary<BuildingData, GameObject> buttonMap = new Dictionary<BuildingData, GameObject>();

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

            btn.interactable = data.isDefaultUnlocked; // 기본 해금 여부 설정

            CraftingButton cb = btnObj.AddComponent<CraftingButton>();
            cb.buildingData = data;

            buttonMap[data] = btnObj;
        }
    }

    public void UnlockCraftingButton(string buildingName)
    {
        foreach (var pair in buttonMap)
        {
            if (pair.Key.buildingName == buildingName)
            {
                pair.Value.GetComponent<Button>().interactable = true;
                Debug.Log($"🔓 {buildingName} 버튼 해금 완료!");
                break;
            }
        }
    }

    public void UnlockBuilding(BuildingData data)
    {
        if (!buttonMap.ContainsKey(data))
        {
            Debug.LogWarning($"⚠️ 알 수 없는 건물 데이터: {data.buildingName}");
            return;
        }

        Button btn = buttonMap[data].GetComponent<Button>();
        if (!btn.interactable)
        {
            btn.interactable = true;
            Debug.Log($"🔓 {data.buildingName} 해금 완료!");
        }
        else
        {
            Debug.Log($"ℹ️ 이미 해금됨: {data.buildingName}");
        }
    }

    public void UnlockCraftingBuilding(BuildingData building)
    {
        foreach (Transform child in craftingGridParent)
        {
            CraftingButton cb = child.GetComponent<CraftingButton>();
            if (cb != null && cb.buildingData == building)
            {
                Button btn = cb.GetComponent<Button>();
                if (btn != null)
                {
                    btn.interactable = true;
                    Debug.Log($"🔨 제작 건물 해금: {building.buildingName}");
                }
                break;
            }
        }
    }

}
