using UnityEngine;
using UnityEngine.UI;

public class CanvasScalerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CanvasScaler canvasScaler;

    [Header("Settings")]
    [SerializeField] private Vector2 referenceResolution = new Vector2(1920,1080);
    
    private int lastWidth;
    private int lastHeight;

    void Awake()
    {
        if (canvasScaler == null)
        {
            Debug.LogError("CanvasScaler is missing from CanvasScalerController!", this);
            return;
        }

        canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasScaler.referenceResolution = referenceResolution;
        
        AdjustScale();
    }

    void Update()
    {
        // Only executes math if the screen size actually changes
        if (Screen.width != lastWidth || Screen.height != lastHeight)
        {
            AdjustScale();
            Debug.Log("Adjusted Canvas");
        }
    }

    void AdjustScale()
    {
        if (canvasScaler == null) return;

        lastWidth = Screen.width;
        lastHeight = Screen.height;

        float targetAspect = referenceResolution.x / referenceResolution.y;
        float currentAspect = (float)lastWidth / lastHeight;

        if (currentAspect >= targetAspect)
        {
            canvasScaler.matchWidthOrHeight = 1f; // Lock to Height
        }
        else
        {
            canvasScaler.matchWidthOrHeight = 0f; // Lock to Width
        }
    }
}