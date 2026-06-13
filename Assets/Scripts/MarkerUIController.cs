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

    
  
}