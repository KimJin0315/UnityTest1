using UnityEngine;

[CreateAssetMenu(fileName = "New Resource", menuName = "Resource/ResourceData")]
public class ResourceData : ScriptableObject
{
    public string resourceName;   // ���� ������ (ex: "Wood")
    public string displayName;    // HUD�� (ex: "����")
    public Sprite icon;           // ���߿� HUD ���������� �� �� ����
}
