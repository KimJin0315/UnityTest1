using UnityEngine;

public class ContentUnlockManager : MonoBehaviour
{
    public GameObject fishingIcon;
    public GameObject farmingIcon;
    public GameObject miningIcon;

    public BuildingUIManager buildingUIManager;
    public CraftingUIManager craftingUIManager;

    public void OnBuildingPlaced(BuildingData building)
    {
        Debug.Log($"설치된 건물: {building.buildingName}");

        // 다음 해금 대상이 있다면 처리
        if (building.nextUnlocks != null)
        {
            foreach (BuildingData next in building.nextUnlocks)
            {
                if (next == null) continue;

                // 건물 해금
                buildingUIManager.UnlockBuilding(next);

                // 제작 건물 해금
                if (next.unlockType != UnlockType.None)
                {
                    craftingUIManager.UnlockCraftingBuilding(next);
                }
            }
        }

        // 아이콘 해금
        switch (building.buildingName)
        {
            case "Wooden House":
                if (fishingIcon != null)
                {
                    fishingIcon.SetActive(true);
                    Debug.Log("🎣 낚시 아이콘 해금됨");
                }
                break;

            case "Bamboo House":
                if (farmingIcon != null)
                {
                    farmingIcon.SetActive(true);
                    Debug.Log("🌾 농사 아이콘 해금됨");
                }
                break;

            case "Enbony House":
                if (miningIcon != null)
                {
                    miningIcon.SetActive(true);
                    Debug.Log("⛏️ 채광 아이콘 해금됨");
                }
                break;
        }
    }
}
