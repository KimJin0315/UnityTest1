using UnityEngine;

public class ContentUnlockManager : MonoBehaviour
{
    public GameObject fishingIcon;
    public GameObject farmIcon;
    public GameObject miningIcon;

    public GameObject craftingPanel;
    public CraftingUIManager craftingUIManager;

    public void OnBuildingPlaced(BuildingData data)
    {
        Debug.Log($"[해금 시스템] {data.buildingName} 설치됨 → 해금 처리 시작");

        // 다음 해금 대상이 있을 경우
        if (data.nextUnlocks != null && data.nextUnlocks.Length > 0)
        {
            foreach (BuildingData unlock in data.nextUnlocks)
            {
                craftingUIManager.UnlockBuilding(unlock);
            }
        }

        // 아이콘 해금 처리
        switch (data.buildingName)
        {
            case "Wooden House":
                if (fishingIcon != null)
                    fishingIcon.SetActive(true);
                break;
            case "Bamboo House":
                if (farmIcon != null)
                    farmIcon.SetActive(true);
                break;
            case "Enbony House":
                if (miningIcon != null)
                    miningIcon.SetActive(true);
                break;
        }
    }

    public void UnlockContent(UnlockType type, string buildingName)
    {
        switch (type)
        {
            case UnlockType.Fishing:
                if (fishingIcon != null) fishingIcon.SetActive(true);
                Debug.Log("🎣 낚시 아이콘 해금됨");
                break;

            case UnlockType.Farming:
                if (farmIcon != null) farmIcon.SetActive(true);
                Debug.Log("🌾 농사 아이콘 해금됨");
                break;

            case UnlockType.Mining:
                if (miningIcon != null) miningIcon.SetActive(true);
                Debug.Log("⛏️ 채광 아이콘 해금됨");
                break;

            case UnlockType.CraftingTable:
            case UnlockType.Furnace:
            case UnlockType.Boat:
                EnableCraftingTab(type.ToString(), buildingName);
                break;
        }
    }

    private void EnableCraftingTab(string tabName, string buildingName)
    {
        if (craftingPanel == null)
        {
            Debug.LogWarning("❗ craftingPanel이 연결되지 않았습니다.");
            return;
        }

        Transform tabButtons = craftingPanel.transform.Find("TabButtons");
        if (tabButtons == null)
        {
            Debug.LogWarning("❗ TabButtons를 찾을 수 없습니다.");
            return;
        }

        foreach (Transform tab in tabButtons)
        {
            if (tab.name.Contains(tabName))
            {
                tab.gameObject.SetActive(true);
                Debug.Log($"✅ {tabName} 탭 해금됨!");
                break;
            }
        }

        if (craftingUIManager != null)
        {
            craftingUIManager.UnlockCraftingButton(buildingName);
        }
    }
}
