using UnityEngine;
using UnityEngine.UI;
using TMPro; // Required for TextMeshPro text elements
using CesiumForUnity; // Don't forget this namespace!
using Unity.Mathematics;


public class AdjustMap : MonoBehaviour
{
    [Header("UI References")]
    [Tooltip("Drag your 0-100 Slider here.")]
    public Slider uiSlider;

    public Slider uiSliderLat;

    public Slider uiSliderLong;

    [Tooltip("Drag your standard 0-100 Text element here.")]
    public TextMeshProUGUI valueText;

    [Tooltip("Drag your new scaled text element (500 - 50,000) here.")]
    public TextMeshProUGUI scaledValueText;

    [Header("Text Settings")]
    [Tooltip("Prefix for the standard 0-100 number.")]
    public string textPrefix = "Value: ";
    
    [Tooltip("Prefix for the scaled 500-50,000 number.")]
    public string scaledTextPrefix = "Range: ";

    [Header("Scaling Boundaries")]
    public float minScaleValue = 500f;
    public float maxScaleValue = 25000f;

    [Header("Scaling Boundaries Lat")]
    public float minScaleValueLat = 10.6f;
    public float maxScaleValueLat = 10.8f;


    [Header("Scaling Boundaries Long")]
    public float minScaleValueLong = 122.8f;
    public float maxScaleValueLong = 123f;


    public CesiumGlobeAnchor cubeMarker;

    void Start()
    {
     
    }


    public void UpdateText()
    {
        // 1. Update the original standard text (0 - 100)

        float value1 = uiSlider.value;
        float value2 = uiSliderLat.value;
        float value3 = uiSliderLong.value;
        
        valueText.text = textPrefix + value1.ToString("0");
    
        // Normalize the 0-100 value down to a 0-1 float percentage (e.g., 50 becomes 0.5)
        float percentage = value1 / 100f;
        float invertedPercentage = 1f - percentage;

        float percentageLat = value2 / 100f;
        float invertedPercentageLat = 1f - percentageLat;

        float percentageLong = value3 / 100f;
        float invertedPercentageLong = 1f - percentageLong;


        // Interpolate smoothly between 500 and 50,000 based on that percentage
        double scaledValue = math.lerp(minScaleValue, maxScaleValue, invertedPercentage);

        double scaledValueLat = math.lerp(minScaleValueLat, maxScaleValueLat, invertedPercentageLat);

        double scaledValueLong = math.lerp(minScaleValueLong, maxScaleValueLong, invertedPercentageLong);

        // "N0" formats the number with clean thousands-separators (e.g., 50,000 instead of 50000)
        scaledValueText.text = scaledTextPrefix + scaledValue.ToString("N0");

        double3 globalCoordinates = new double3(scaledValueLong, scaledValueLat, scaledValue);

        // 4. Assign the coordinates to the globe anchor. Cesium snaps the object to its globe position automatically.
        cubeMarker.longitudeLatitudeHeight = globalCoordinates;
        
    }

    // Clean up the listener when this object is destroyed
    void OnDestroy()
    {
      
    }
}