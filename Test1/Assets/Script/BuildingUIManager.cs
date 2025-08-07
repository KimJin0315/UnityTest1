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

            btnObj.AddComponent<BuildingButton>().buildingData = data;

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

public class BuildingButton : MonoBehaviour
{
    public BuildingData buildingData;
}
