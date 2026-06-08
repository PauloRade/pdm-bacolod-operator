using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem; 
using UnityEngine.EventSystems;
using TMPro; // Standard practice in modern Unity for UI Text

public class MapController : MonoBehaviour
{
    [Header("UI Sliders (0 - 100)")]
    public Slider slider1; // Scroll Wheel (Up / Down)
    public Slider slider2; // Horizontal (Mouse X Drag)
    public Slider slider3; // Vertical (Mouse Y Drag)

    [Header("Sensitivity Settings")]
    [Tooltip("How fast Slider 1 moves when scrolling.")]
    public float scrollSensitivity = 1f;
    [Header("Dynamic Drag Sensitivity")]
    [Tooltip("Drag sensitivity when Slider 1 is at 0.")]
    public float minDragSensitivity = 0.01f;
    [Tooltip("Drag sensitivity when Slider 1 is at 100.")]
    public float maxDragSensitivity = 0.05f;

    [Header("UI Text Display")]
    [Tooltip("Drop your TextMeshPro component here to show current sensitivity.")]
    public TextMeshProUGUI sensitivityText;

    private bool isHovering = false; // Tracks if the mouse is over the specific area

    void Start()
    {
        ConfigureSlider(slider1);
        ConfigureSlider(slider2);
        ConfigureSlider(slider3);
        UpdateSensitivityText();
    }

    void Update()
    {
        if (Mouse.current == null) return;

        // --- HANDLE SLIDER 1: MOUSE SCROLL WHEEL ---
        // Only run this logic if the mouse is actively hovering over the image
        if (isHovering)
        {
            float scrollValue = Mouse.current.scroll.ReadValue().y;

            if (scrollValue != 0 && slider1 != null)
            {
                float scrollDirection = Mathf.Sign(scrollValue);
                slider1.value = Mathf.Clamp(slider1.value + (scrollDirection * scrollSensitivity), 0f, 100f);
                UpdateSensitivityText();
            }
        }
    }

    // --- PUBLIC DRAG FUNCTION ---
    public void OnAreaDragged(BaseEventData data)
    {
        PointerEventData eventData = data as PointerEventData;
        if (eventData == null) return;

        Vector2 mouseDelta = eventData.delta;

        // 1. Calculate the current drag sensitivity based on Slider 1's position (0 to 100)
        // We divide slider1.value by 100f to turn it into a 0.0 to 1.0 percentage for Lerp
        float dynamicDragSensitivity = GetCurrentSensitivity();

        if (slider2 != null && mouseDelta.x != 0)
        {
            slider2.value = Mathf.Clamp(slider2.value + (mouseDelta.x * dynamicDragSensitivity), 0f, 100f);
        }

        if (slider3 != null && mouseDelta.y != 0)
        {
            slider3.value = Mathf.Clamp(slider3.value + (mouseDelta.y * dynamicDragSensitivity), 0f, 100f);
        }
        UpdateSensitivityText();
    }

    // Central place to calculate the current sensitivity so we don't repeat math formulas
    private float GetCurrentSensitivity()
    {
        float currentSliderPercentage = slider1.value / 100f;
        return Mathf.Lerp(maxDragSensitivity, minDragSensitivity, currentSliderPercentage);
    }

    // Updates the UI text element format smoothly
    private void UpdateSensitivityText()
    {
        if (sensitivityText != null)
        {
            float currentSens = GetCurrentSensitivity();
            
            // "F3" formats it to 3 decimal places (e.g., 0.025). 
            // You can change decimalPlaces in the Inspector.
            string formatSpecifier = "F" + 10; 
            sensitivityText.text = $"Drag Sens: {currentSens.ToString(formatSpecifier)}";
        }
    }

    // --- HOVER FUNCTIONS ---
    // Called via Event Trigger when mouse enters the image boundaries
    public void OnPointerEnterArea()
    {
        isHovering = true;
    }

    // Called via Event Trigger when mouse leaves the image boundaries
    public void OnPointerExitArea()
    {
        isHovering = false;
    }

    private void ConfigureSlider(Slider slider)
    {
        if (slider != null)
        {
            slider.minValue = 0f;
            slider.maxValue = 100f;
        }
        else
        {
            Debug.LogWarning("MapController: A slider reference is missing in the Inspector!", this);
        }
    }
}