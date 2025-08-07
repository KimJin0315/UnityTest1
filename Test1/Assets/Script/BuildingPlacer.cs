using UnityEngine;

public class BuildingPlacer : MonoBehaviour
{
    public Camera mainCamera;
    public Material previewMaterial;
    public float gridSize = 1f;
    public LayerMask buildingLayer;

    private GameObject previewObject;
    private BuildingData currentBuilding;
    private bool isPlacing = false;
    private bool isFlipped = false;

    void Update()
    {
        if (!isPlacing || previewObject == null) return;

        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10;
        Vector3 worldPos = mainCamera.ScreenToWorldPoint(mousePos);
        worldPos.z = 0;

        // 격자 스냅
        worldPos.x = Mathf.Round(worldPos.x / gridSize) * gridSize;
        worldPos.y = Mathf.Round(worldPos.y / gridSize) * gridSize;
        previewObject.transform.position = worldPos;

        // 좌우 반전
        if (Input.GetKeyDown(KeyCode.R))
        {
            isFlipped = !isFlipped;
            Vector3 scale = previewObject.transform.localScale;
            scale.x *= -1;
            previewObject.transform.localScale = scale;
        }

        // 충돌 체크
        Vector2 checkSize = Vector2.one;
        if (previewObject.TryGetComponent<BoxCollider2D>(out BoxCollider2D box))
        {
            checkSize = Vector2.Scale(box.size, previewObject.transform.localScale);
        }

        Collider2D hit = Physics2D.OverlapBox(worldPos, checkSize, 0f, buildingLayer.value);
        bool canPlace = (hit == null);

        // 프리뷰 색상
        Color previewColor = canPlace ? new Color(0, 1, 0, 0.5f) : new Color(1, 0, 0, 0.5f);
        SetPreviewColor(previewObject, previewColor);

        // 마우스 클릭 처리
        if (Input.GetMouseButtonDown(0) && canPlace)
        {
            PlaceBuilding(worldPos);
        }
        else if (Input.GetMouseButtonDown(0) && !canPlace)
        {
            Debug.LogWarning("🚫 겹쳐서 건물 설치 불가!");
        }

        if (Input.GetMouseButtonDown(1))
        {
            CancelPlacement();
        }
    }

    public void StartPlacement(BuildingData building)
    {
        if (previewObject != null) Destroy(previewObject);

        currentBuilding = building;
        isPlacing = true;

        previewObject = Instantiate(building.previewPrefab);
        previewObject.layer = LayerMask.NameToLayer("Ignore Raycast");

        foreach (Collider2D col in previewObject.GetComponentsInChildren<Collider2D>())
        {
            col.isTrigger = true;
        }

        AdjustSize(previewObject, building.width, building.height);
        SetPreviewMode(previewObject);
    }

    void PlaceBuilding(Vector3 position)
    {
        PlayerInventory inventory = FindFirstObjectByType<PlayerInventory>();

        // 설치 전 재료 체크
        foreach (ResourceCost cost in currentBuilding.resourceCosts)
        {
            if (!inventory.HasResource(cost.resourceName, cost.amount))
            {
                Debug.LogWarning($"🚫 {cost.resourceName} 부족! 건물을 설치할 수 없습니다.");
                return;
            }
        }

        // 재료 소모
        foreach (ResourceCost cost in currentBuilding.resourceCosts)
        {
            inventory.UseResource(cost.resourceName, cost.amount);
            Debug.Log($"{cost.resourceName} -{cost.amount} (남은 수량: {inventory.GetResourceAmount(cost.resourceName)})");
        }

        // 건물 설치
        GameObject newBuilding = Instantiate(currentBuilding.prefab, position, Quaternion.identity);
        newBuilding.transform.localScale = previewObject.transform.localScale;

        foreach (Collider2D col in newBuilding.GetComponentsInChildren<Collider2D>())
        {
            col.isTrigger = false;
        }

        newBuilding.layer = LayerMask.NameToLayer("Building");

        // 건물 해금 처리
        var unlockManager = FindFirstObjectByType<ContentUnlockManager>();
        if (unlockManager != null)
        {
            unlockManager.OnBuildingPlaced(currentBuilding);
        }
        else
        {
            Debug.LogWarning("❗ ContentUnlockManager를 찾을 수 없습니다.");
        }

        CancelPlacement();
    }


    void CancelPlacement()
    {
        if (previewObject != null) Destroy(previewObject);
        previewObject = null;
        isPlacing = false;
        currentBuilding = null;
        isFlipped = false;
    }

    void SetPreviewMode(GameObject obj)
    {
        foreach (Renderer rend in obj.GetComponentsInChildren<Renderer>())
        {
            rend.material = previewMaterial;
        }
    }

    void SetPreviewColor(GameObject obj, Color color)
    {
        foreach (SpriteRenderer sr in obj.GetComponentsInChildren<SpriteRenderer>())
        {
            sr.color = color;
        }
    }

    void AdjustSize(GameObject obj, int width, int height)
    {
        SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            Vector2 spriteSize = sr.sprite.bounds.size;
            float scaleX = width / spriteSize.x;
            float scaleY = height / spriteSize.y;

            obj.transform.localScale = new Vector3(scaleX, scaleY, 1);

            BoxCollider2D box = obj.GetComponent<BoxCollider2D>();
            if (box != null)
            {
                box.size = spriteSize;
            }
        }
    }
}
