using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem; // Required for the New Input System package

public class MapController : MonoBehaviour
{
    [Header("UI Sliders (0 - 100)")]
    public Slider slider1; // Scroll Wheel (Up / Down)
    public Slider slider2; // Horizontal (Mouse X Drag)
    public Slider slider3; // Vertical (Mouse Y Drag)

    [Header("Sensitivity Settings")]
    [Tooltip("How fast Slider 1 moves when scrolling.")]
    public float scrollSensitivity = 1f;
    [Tooltip("How fast Sliders 2 & 3 move when dragging the mouse.")]
    public float dragSensitivity = 15f;

    void Start()
    {
        // Force all sliders to use a strict 0 to 100 range
        ConfigureSlider(slider1);
        ConfigureSlider(slider2);
        ConfigureSlider(slider3);
    }

    void Update()
    {
        if (Mouse.current == null) return;

        // --- HANDLE SLIDER 1: MOUSE SCROLL WHEEL ---
        // Read the vertical scroll delta value
        float scrollValue = Mouse.current.scroll.ReadValue().y;

        if (scrollValue != 0 && slider1 != null)
        {
            // Normalize the scroll value (scrollValue is typically large, like 120 or -120)
            float scrollDirection = Mathf.Sign(scrollValue);
            
            slider1.value = Mathf.Clamp(slider1.value + (scrollDirection * scrollSensitivity), 0f, 100f);
        }

        // --- HANDLE SLIDERS 2 & 3: LEFT MOUSE DRAG ---
        if (Mouse.current.leftButton.isPressed)
        {
            Vector2 mouseDelta = Mouse.current.delta.ReadValue();

            if (mouseDelta.x != 0 || mouseDelta.y != 0)
            {
                // Adjust Slider 2 based on horizontal mouse drag (Delta X)
                if (slider2 != null)
                {
                    slider2.value = Mathf.Clamp(slider2.value + (mouseDelta.x * dragSensitivity * Time.deltaTime), 0f, 100f);
                }

                // Adjust Slider 3 based on vertical mouse drag (Delta Y)
                if (slider3 != null)
                {
                    slider3.value = Mathf.Clamp(slider3.value + (mouseDelta.y * dragSensitivity * Time.deltaTime), 0f, 100f);
                }
            }
        }
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