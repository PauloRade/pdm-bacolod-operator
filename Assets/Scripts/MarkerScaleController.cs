using UnityEngine;
using UnityEngine.UI;

public class MarkerScaleController : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Slider scaleSlider;

    [Header("Scale Settings")]
    [Tooltip("The Transform you want to scale. Defaults to this GameObject if left empty.")]
    [SerializeField] private Transform targetTransform;
    
    // Minimum and maximum values for mapping
    private const float MinSliderValue = 0f;
    private const float MaxSliderValue = 100f;
    private const float MinScaleValue = 100f;
    private const float MaxScaleValue = 1000f;

    public float newScaleValue ;

    void Start()
    {
        

        
        // Set slider limits just in case they aren't set in the Inspector
        scaleSlider.minValue = MinSliderValue;
        scaleSlider.maxValue = MaxSliderValue;

        // Listen for slider value changes
        scaleSlider.onValueChanged.AddListener(OnSliderValueChanged);

        // Set initial scale based on current slider value
        UpdateScale(scaleSlider.value);
      
    }

    void OnDestroy()
    {
        // Clean up listener when the object is destroyed to prevent memory leaks
      
        scaleSlider.onValueChanged.RemoveListener(OnSliderValueChanged);
        
    }

    private void OnSliderValueChanged(float value)
    {
        UpdateScale(value);
    }

    private void UpdateScale(float sliderValue)
    {

        // InverseLerp outputs a 0 to 1 percentage based on where sliderValue is between 0 and 100
        float percentage = Mathf.InverseLerp(MinSliderValue, MaxSliderValue, sliderValue);

        float invertedPercentage = 1f - percentage;

        // Lerp takes that 0 to 1 percentage and maps it to the 100 to 1000 scale range
        newScaleValue = Mathf.Lerp(MinScaleValue, MaxScaleValue, invertedPercentage);

        // Apply the new scale to X and Y, keeping Z at its original value
        targetTransform.localScale = new Vector3(newScaleValue, targetTransform.localScale.y, newScaleValue);
    }
}