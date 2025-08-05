using UnityEngine;

public class BuildingUIController : MonoBehaviour
{
    public GameObject uiRootPanel;          // 전체 UI 묶음 패널 (BuildingPanel + CraftingPanel 포함)
    public PanelSwitcher panelSwitcher;     // 탭 전환 스크립트

    private bool isVisible = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            isVisible = !isVisible;
            uiRootPanel.SetActive(isVisible);

            if (isVisible)
            {
                panelSwitcher.ShowBuildingPanel(); // 기본 탭은 건물 패널
            }
        }
    }
}
