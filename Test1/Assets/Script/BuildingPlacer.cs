using UnityEngine;

public class BuildingPlacer : MonoBehaviour
{
    public Camera mainCamera;
    public Material previewMaterial;
    public float gridSize = 1f;
    public LayerMask buildingLayer;   // ✅ 실제 건물 전용 레이어 (겹침 체크)

    private GameObject previewObject;
    private BuildingData currentBuilding;
    private bool isPlacing = false;
    private bool isFlipped = false;

    void Update()
    {
        if (isPlacing && previewObject != null)
        {
            // ✅ 마우스 → 월드 좌표 변환
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 10;
            Vector3 worldPos = mainCamera.ScreenToWorldPoint(mousePos);
            worldPos.z = 0;

            // ✅ 격자 스냅
            worldPos.x = Mathf.Round(worldPos.x / gridSize) * gridSize;
            worldPos.y = Mathf.Round(worldPos.y / gridSize) * gridSize;

            previewObject.transform.position = worldPos;

            // ✅ Collider 크기 체크 (SpriteRenderer + Scale 반영)
            Vector2 checkSize = Vector2.one;
            if (previewObject.TryGetComponent<BoxCollider2D>(out BoxCollider2D box))
            {
                checkSize = Vector2.Scale(box.size, previewObject.transform.localScale);
            }

            // ✅ OverlapBox로 충돌 체크
            Collider2D hit = Physics2D.OverlapBox(
                worldPos,
                checkSize,
                0f,
                buildingLayer.value
            );

            bool canPlace = (hit == null);

            // ✅ 프리뷰 색상 (초록=설치 가능, 빨강=설치 불가)
            Color previewColor = canPlace ? new Color(0, 1, 0, 0.5f) : new Color(1, 0, 0, 0.5f);
            SetPreviewColor(previewObject, previewColor);

            // ✅ 좌클릭 → 설치
            if (Input.GetMouseButtonDown(0) && canPlace)
            {
                PlaceBuilding(worldPos);
            }
            else if (Input.GetMouseButtonDown(0) && !canPlace)
            {
                Debug.LogWarning("🚫 겹쳐서 건물 설치 불가!");
            }

            // ✅ 우클릭 → 취소
            if (Input.GetMouseButtonDown(1))
            {
                CancelPlacement();
            }
        }
    }

    // ✅ 건설 모드 시작
    public void StartPlacement(BuildingData building)
    {
        // ✅ 기존 프리뷰가 있다면 먼저 삭제
        if (previewObject != null)
        {
            Destroy(previewObject);
        }

        currentBuilding = building;
        isPlacing = true;

        previewObject = Instantiate(building.prefab);
        previewObject.layer = LayerMask.NameToLayer("Ignore Raycast"); // ✅ 프리뷰는 충돌 무시

        // ✅ 프리뷰는 Collider 비활성화 (플레이어 안 밀리게)
        foreach (Collider2D col in previewObject.GetComponentsInChildren<Collider2D>())
        {
            col.isTrigger = true;
        }

        AdjustSize(previewObject, building.width, building.height);
        SetPreviewMode(previewObject);
    }


    // ✅ 실제 건물 설치
    void PlaceBuilding(Vector3 position)
    {
        PlayerInventory inventory = FindFirstObjectByType<PlayerInventory>();

        // ✅ 1) 설치 전 재료 체크
        foreach (ResourceCost cost in currentBuilding.resourceCosts)
        {
            if (!inventory.HasResource(cost.resourceName, cost.amount))
            {
                Debug.LogWarning($"🚫 {cost.resourceName} 부족! 건물을 설치할 수 없습니다.");
                return;  // ❌ 재료 부족 → 설치 중단
            }
        }

        // ✅ 2) 재료 소모
        foreach (ResourceCost cost in currentBuilding.resourceCosts)
        {
            inventory.UseResource(cost.resourceName, cost.amount);
            Debug.Log($"{cost.resourceName} -{cost.amount} (남은 수량: {inventory.GetResourceAmount(cost.resourceName)})");
        }

        // ✅ 3) 건물 설치
        GameObject newBuilding = Instantiate(currentBuilding.prefab, position, Quaternion.identity);
        newBuilding.transform.localScale = previewObject.transform.localScale;

        foreach (Collider2D col in newBuilding.GetComponentsInChildren<Collider2D>())
        {
            col.isTrigger = false;
        }

        foreach (SpriteRenderer sr in newBuilding.GetComponentsInChildren<SpriteRenderer>())
        {
            sr.flipX = isFlipped;
        }

        // ✅ 4) Building 레이어 적용
        newBuilding.layer = LayerMask.NameToLayer("Building");

        // ✅ 5) 다음 건물 해금
        if (currentBuilding.nextUnlock != null)
        {
            FindFirstObjectByType<BuildingUIManager>().UnlockBuilding(currentBuilding.nextUnlock);
        }

        CancelPlacement();
    }


    // ✅ 프리뷰 모드 설정 (투명 머티리얼)
    void SetPreviewMode(GameObject obj)
    {
        foreach (Renderer rend in obj.GetComponentsInChildren<Renderer>())
        {
            rend.material = previewMaterial;
        }
    }

    // ✅ 프리뷰 색상
    void SetPreviewColor(GameObject obj, Color color)
    {
        foreach (SpriteRenderer sr in obj.GetComponentsInChildren<SpriteRenderer>())
        {
            sr.color = color;
        }
    }

    // ✅ 설치 취소 (프리뷰 삭제)
    void CancelPlacement()
    {
        Destroy(previewObject);
        previewObject = null;
        isPlacing = false;
        currentBuilding = null;
        isFlipped = false;
    }

    // ✅ 건물 크기 조정 (width/height 기반)
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
                box.size = spriteSize; // scale과 합쳐져서 최종 크기 결정됨
            }
        }
    }
}
