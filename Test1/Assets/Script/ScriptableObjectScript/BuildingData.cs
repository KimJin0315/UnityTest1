using UnityEngine;
using System.Collections.Generic;

public enum UnlockType
{
    None,           // �⺻�� (�ر� ����)
    Fishing,        // ���� ������ �ر�
    Farming,        // ��� ������ �ر�
    Mining,         // ä�� ������ �ر�
    CraftingTable,  // ���۴�  �ر�
    Furnace,        // ȭ��  �ر�
    Boat            // ��  �ر�
}


[System.Serializable]
public class ResourceCost
{
    public string resourceName;
    public int amount;
}

[CreateAssetMenu(menuName = "Building/BuildingData")]
public class BuildingData : ScriptableObject
{
    public string buildingName;                // �ǹ� �̸�
    public Sprite icon;                        // UI���� ����� ������
    public GameObject prefab;                  // ���� ��ġ�� ������
    public GameObject previewPrefab;           // �̸����� ������
    public int width;                          // ��ġ �� ���� Ÿ�� ��
    public int height;                         // ��ġ �� ���� Ÿ�� ��
    public ResourceCost[] resourceCosts;       // �ʿ��� �ڿ�

    public UnlockType unlockType;              // ������ �رݿ� (ex: ����, ���)
    public BuildingData[] nextUnlocks;         // ��ġ �� �رݵ� �ǹ���

    public bool isDefaultUnlocked = false; // ���� ���� �� �ڵ� �ر� ����

}

