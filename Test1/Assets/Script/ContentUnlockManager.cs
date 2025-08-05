using UnityEngine;

public class ContentUnlockManager : MonoBehaviour
{
    public GameObject fishingIcon;
    public GameObject farmingIcon;
    public GameObject miningIcon;

    public GameObject craftingPanel; // 제작대/화로/배 있는 탭

    public void EnableCraftingTab(string tabName)
    {
        // 탭 이름 기반으로 활성화
        Transform tab = craftingPanel.transform.Find(tabName);
        if (tab != null)
        {
            tab.gameObject.SetActive(true);
            Debug.Log($"🔓 제작 탭 해금: {tabName}");
        }
    }

    public void ShowFishingIcon()
    {
        if (fishingIcon != null) fishingIcon.SetActive(true);
    }

    public void ShowFarmingIcon()
    {
        if (farmingIcon != null) farmingIcon.SetActive(true);
    }

    public void ShowMiningIcon()
    {
        if (miningIcon != null) miningIcon.SetActive(true);
    }
}
