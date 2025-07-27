using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class BuildingUIManager : MonoBehaviour
{
    public GameObject buttonPrefab;
    public Transform gridParent;
    public BuildingData[] buildings;

    private Dictionary<BuildingData, bool> unlocked = new Dictionary<BuildingData, bool>();

    void Start()
    {
        foreach (BuildingData data in buildings)
        {
            GameObject btnObj = Instantiate(buttonPrefab, gridParent);

            btnObj.transform.Find("Icon").GetComponent<Image>().sprite = data.icon;
            btnObj.transform.Find("Label").GetComponent<Text>().text = data.buildingName;

            btnObj.GetComponent<Button>().onClick.AddListener(() =>
            {
                FindFirstObjectByType<BuildingPlacer>().StartPlacement(data);
            });

            Button btn = btnObj.GetComponent<Button>();
            bool isFirst = (data == buildings[0]);
            btn.interactable = isFirst;

            // ✅ BuildingButton 붙이기
            btnObj.AddComponent<BuildingButton>().buildingData = data;

            // ✅ 해금 여부 초기화
            unlocked[data] = isFirst;
        }
    }

    public void UnlockBuilding(BuildingData data)
    {
        if (unlocked.ContainsKey(data) && unlocked[data])
            return;

        foreach (Transform child in gridParent)
        {
            BuildingButton btn = child.GetComponent<BuildingButton>();
            if (btn != null && btn.buildingData == data)
            {
                btn.GetComponent<Button>().interactable = true;
                unlocked[data] = true;
                Debug.Log($"🔓 {data.buildingName} 해금 완료!");
            }
        }
    }
}

// ✅ UI 버튼에 붙어서 어떤 건물 버튼인지 알려주는 단순 클래스
public class BuildingButton : MonoBehaviour
{
    public BuildingData buildingData;
}
