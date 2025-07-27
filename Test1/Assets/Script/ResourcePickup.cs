using UnityEngine;

public class ResourcePickup : MonoBehaviour
{
    public string resourceName;
    public int amount;

    public float pickupRange = 1.5f;

    void OnMouseDown()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.transform.position);

        if (distance <= pickupRange)  // ✅ 플레이어가 일정 거리 안에 있을 때만
        {
            PlayerInventory inventory = FindFirstObjectByType<PlayerInventory>();

            if (inventory != null)
            {
                inventory.AddResource(resourceName, amount);
                Debug.Log($"{resourceName} {amount}개 획득!");
                // Destroy(gameObject);   ✅ 테스트니까 주석 처리 (삭제 안 함)
            }
        }
        else
        {
            Debug.Log($"🚫 {resourceName} 너무 멀어서 획득 불가");
        }
    }
}
