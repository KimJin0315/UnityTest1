using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingUIManager : MonoBehaviour
{
    [Header("UI Prefabs")]
    public GameObject craftingButtonPrefab;
    public Transform craftingGridParent;

    [Header("Crafting Buildings")]
    public BuildingData[] craftingBuildings;

    private Dictionary<BuildingData, bool> unlocked = new Dictionary<BuildingData, bool>();

    void Start()
    {
        foreach (BuildingData data in craftingBuildings)
        {
            GameObject btnObj = Instantiate(craftingButtonPrefab, craftingGridParent);

            // 아이콘 설정
            Image iconImage = btnObj.transform.Find("Icon").GetComponent<Image>();
            if (iconImage != null) iconImage.sprite = data.icon;

            // 텍스트 라벨 설정
            Text label = btnObj.transform.Find("Label").GetComponent<Text>();
            if (label != null) label.text = data.buildingName;

            // 버튼 클릭 시 건물 배치 시작
            btnObj.GetComponent<Button>().onClick.AddListener(() =>
            {
                FindFirstObjectByType<BuildingPlacer>().StartPlacement(data);
            });

            // 초기에는 버튼 비활성화
            btnObj.GetComponent<Button>().interactable = false;

            // CraftingButton 컴포넌트 추가 및 연결
            CraftingButton craftingBtn = btnObj.AddComponent<CraftingButton>();
            craftingBtn.buildingData = data;

            // 해금 여부 저장
            unlocked[data] = false;
        }
    }

    public void UnlockCraftingTab(string tabName)
    {
        foreach (Transform child in craftingGridParent)
        {
            CraftingButton btn = child.GetComponent<CraftingButton>();
            if (btn != null && btn.buildingData != null && btn.buildingData.buildingName == tabName)
            {
                btn.GetComponent<Button>().interactable = true;
                unlocked[btn.buildingData] = true;
                Debug.Log($"🔓 {tabName} 해금 완료!");
            }
        }
    }
}
