using UnityEngine;

public class BuildingUIController : MonoBehaviour
{
    public GameObject uiRootPanel;   // ��� ��ü UI
    public GameObject iconPanel;     // ������ �ϴ� Icon
    public PanelSwitcher panelSwitcher;

    private bool isVisible = false;

    void Start()
    {
        // ���� �� UI ��Ȱ��ȭ
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
