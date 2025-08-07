using UnityEngine;
using System.Collections.Generic;

public enum UnlockType
{
    None,           // 기본값 (해금 없음)
    Fishing,        // 낚시 아이콘 해금
    Farming,        // 농사 아이콘 해금
    Mining,         // 채광 아이콘 해금
    CraftingTable,  // 제작대  해금
    Furnace,        // 화로  해금
    Boat            // 배  해금
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
    public string buildingName;                // 건물 이름
    public Sprite icon;                        // UI에서 사용할 아이콘
    public GameObject prefab;                  // 실제 설치될 프리팹
    public GameObject previewPrefab;           // 미리보기 프리팹
    public int width;                          // 설치 시 가로 타일 수
    public int height;                         // 설치 시 세로 타일 수
    public ResourceCost[] resourceCosts;       // 필요한 자원

    public UnlockType unlockType;              // 콘텐츠 해금용 (ex: 낚시, 농사)
    public BuildingData[] nextUnlocks;         // 설치 시 해금될 건물들

    public bool isDefaultUnlocked = false; // 게임 시작 시 자동 해금 여부

}

