using UnityEngine;

[CreateAssetMenu(fileName = "New Resource", menuName = "Resource/ResourceData")]
public class ResourceData : ScriptableObject
{
    public string resourceName;   // 내부 로직용 (ex: "Wood")
    public string displayName;    // HUD용 (ex: "나무")
    public Sprite icon;           // 나중에 HUD 아이콘으로 쓸 수 있음
}
