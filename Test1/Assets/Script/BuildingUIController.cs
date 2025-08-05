using UnityEngine;

public class BuildingUIController : MonoBehaviour
{
    public GameObject uiRootPanel;          // ��ü UI ���� �г� (BuildingPanel + CraftingPanel ����)
    public PanelSwitcher panelSwitcher;     // �� ��ȯ ��ũ��Ʈ

    private bool isVisible = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            isVisible = !isVisible;
            uiRootPanel.SetActive(isVisible);

            if (isVisible)
            {
                panelSwitcher.ShowBuildingPanel(); // �⺻ ���� �ǹ� �г�
            }
        }
    }
}
