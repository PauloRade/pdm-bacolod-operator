using UnityEngine;
using TMPro; // Required for TextMeshPro
using CesiumForUnity; // Don't forget this namespace!
using Unity.Mathematics;


public class MarkerUIController : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Attach your MarkerSpawner script here.")]
    public MarkerSpawner markerSpawner;

    public CesiumGlobeAnchor cubeMarker;

    [Header("UI Elements")]
    public TMP_InputField latitudeInput;
    public TMP_InputField longitudeInput;

    public void MoveCube()
    {
          // 2. Extract the text strings from the TMP input fields
        string latText = latitudeInput.text;
        string longText = longitudeInput.text;



        // 3. Convert the strings into double numbers safely using double.TryParse
        bool isLatValid = double.TryParse(latText, out double parsedLatitude);
        bool isLongValid = double.TryParse(longText, out double parsedLongitude);

        if (isLatValid && isLongValid)
        {
        
            double3 globalCoordinates = new double3(parsedLongitude, parsedLatitude,263.8);

        // 4. Assign the coordinates to the globe anchor. Cesium snaps the object to its globe position automatically.
            cubeMarker.longitudeLatitudeHeight = globalCoordinates;
        }
        
    
    }

    /// <summary>
    /// The public function with NO arguments. 
    /// Link this function directly to your Unity UI Button's OnClick() event.
    /// </summary>
    public void OnSpawnButtonClicked()
    {
        // 1. Double check that all references are assigned
        if (markerSpawner == null || latitudeInput == null || longitudeInput == null)
        {
            Debug.LogError("UI Controller: Missing references in the inspector!");
            return;
        }

        // 2. Extract the text strings from the TMP input fields
        string latText = latitudeInput.text;
        string longText = longitudeInput.text;

        // 3. Convert the strings into double numbers safely using double.TryParse
        bool isLatValid = double.TryParse(latText, out double parsedLatitude);
        bool isLongValid = double.TryParse(longText, out double parsedLongitude);

        // 4. If the inputs are valid numbers, pass them to your MarkerSpawner script
        if (isLatValid && isLongValid)
        {
            markerSpawner.SpawnMarkerAtCoordinates(parsedLatitude, parsedLongitude);
        }
        else
        {
            // Triggers if the fields are empty or contain letters instead of numbers
            Debug.LogWarning("UI Controller: Invalid coordinates entered. Please use valid decimal numbers (e.g., 48.8584).");
        }
    }
}