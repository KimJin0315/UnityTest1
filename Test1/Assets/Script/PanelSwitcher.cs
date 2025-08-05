using UnityEngine;

public class PanelSwitcher : MonoBehaviour
{
    public GameObject buildingPanel;
    public GameObject craftingPanel;

    public void ShowBuildingPanel()
    {
        buildingPanel.SetActive(true);
        craftingPanel.SetActive(false);
    }

    public void ShowCraftingPanel()
    {
        buildingPanel.SetActive(false);
        craftingPanel.SetActive(true);
    }
}
