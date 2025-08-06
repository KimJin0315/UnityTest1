using UnityEngine;

public class BuildingUIController : MonoBehaviour
{
    public GameObject uiRootPanel;   // 상단 전체 UI
    public GameObject iconPanel;     // 오른쪽 하단 Icon
    public PanelSwitcher panelSwitcher;

    private bool isVisible = false;

    void Start()
    {
        // 시작 시 UI 비활성화
        uiRootPanel.SetActive(false);
        iconPanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            isVisible = !isVisible;

            uiRootPanel.SetActive(isVisible);
            iconPanel.SetActive(isVisible);

            if (isVisible)
            {
                panelSwitcher.ShowBuildingPanel();
            }
        }
    }
}
