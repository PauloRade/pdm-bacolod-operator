using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using CesiumForUnity;

public class CesiumMapController : MonoBehaviour
{
    [Header("UI Window Setup")]
    public RectTransform mapRawImageRect; // Assign your UI Raw Image here
    public Camera mapCamera;               // Assign your Cesium Camera here

    [Header("Spawning Setup")]
    public CesiumGeoreference georeference;
    public GameObject prefabToSpawn;

    [Header("Movement Settings")]
    public float panSpeed = 0.5f;
    public float zoomSpeed = 50f;
    public float minHeight = 20f;
    public float maxHeight = 3000f;

    private bool isDragging = false;

    void Update()
    {
        // --- 1. DRAG TO PAN ---
        if (Mouse.current.leftButton.wasPressedThisFrame && IsMouseInsideUIWindow())
        {
            isDragging = true;
        }

        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            isDragging = false;
        }

        if (isDragging)
        {
            Vector2 mouseDelta = Mouse.current.delta.ReadValue();
            if (mouseDelta.sqrMagnitude > 0.01f)
            {
                Vector3 forward = transform.forward;
                Vector3 right = transform.right;
                forward.y = 0f; right.y = 0f;
                forward.Normalize(); right.Normalize();

                float currentHeight = transform.position.y;
                float speedMultiplier = panSpeed * (Mathf.Max(currentHeight, 10f) / 200f);
                Vector3 moveDirection = (right * -mouseDelta.x) + (forward * -mouseDelta.y);

                transform.Translate(moveDirection * speedMultiplier, Space.World);
            }
        }

        // --- 2. SCROLL TO ZOOM ---
        if (IsMouseInsideUIWindow())
        {
            Vector2 scrollVector = Mouse.current.scroll.ReadValue();
            if (scrollVector.y != 0)
            {
                float scrollDirection = Mathf.Sign(scrollVector.y);
                Vector3 zoomMove = transform.forward * scrollDirection * zoomSpeed;
                Vector3 targetPosition = transform.position + zoomMove;

                if (targetPosition.y >= minHeight && targetPosition.y <= maxHeight)
                {
                    transform.position = targetPosition;
                }
            }
        }

        // --- 3. CLICK TO SPAWN (Right-Click to test) ---
        if (Mouse.current.rightButton.wasPressedThisFrame && IsMouseInsideUIWindow())
        {
            HandleUiClickToSpawn();
        }
    }

    private bool IsMouseInsideUIWindow()
    {
        if (mapRawImageRect == null) return false;
        return RectTransformUtility.RectangleContainsScreenPoint(mapRawImageRect, Mouse.current.position.ReadValue(), null);
    }

    private void HandleUiClickToSpawn()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();

        // Convert global screen mouse coordinates to local 2D pixel coordinates inside the UI Window
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(mapRawImageRect, mousePos, null, out Vector2 localPoint))
        {
            // Normalize coordinates from (-width/2, width/2) to (0, 1) viewport space
            float normalizedX = (localPoint.x / mapRawImageRect.rect.width) + 0.5f;
            float normalizedY = (localPoint.y / mapRawImageRect.rect.height) + 0.5f;

            // Shoot a ray out of the hidden map camera using the normalized window position
            Ray ray = mapCamera.ViewportPointToRay(new Vector3(normalizedX, normalizedY, 0));

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                // Spawn object at the exact 3D location hit on the Cesium globe
                GameObject spawnedObj = Instantiate(prefabToSpawn, hit.point, Quaternion.identity);
                spawnedObj.transform.SetParent(georeference.transform, true);
                
                Debug.Log("Spawned object on map inside UI window!");
            }
        }
    }
}