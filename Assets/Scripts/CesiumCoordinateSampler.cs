using UnityEngine;
using CesiumForUnity;
using Unity.Mathematics;
using System.Threading.Tasks;
using TMPro; // Required for TMP_InputField

public class CesiumCoordinateSampler : MonoBehaviour
{
    public Cesium3DTileset cesiumTileset;

    // Any script can read this variable at any time to get the last fetched height
    [HideInInspector]
    public double latestHeight; 

    [Header("UI Input References")]
    [Tooltip("Drag your Longitude Input Field here.")]
    public TMP_InputField longitudeInputField;
    
    [Tooltip("Drag your Latitude Input Field here.")]
    public TMP_InputField latitudeInputField;

    public MarkerSpawner markerSpawner;

    /// <summary>
    /// A normal public function you can call. It updates the public 'latestHeight' variable.
    /// </summary>
    public void RequestHeight()
    {

        double longitude = 0;
        double latitude = 0;

        bool isLongitudeValid = double.TryParse(longitudeInputField.text, out longitude);
        bool isLatitudeValid = double.TryParse(latitudeInputField.text, out latitude);

        if (!isLongitudeValid || !isLatitudeValid)
        {
            Debug.LogWarning("[CesiumSampler Warning] Invalid input text! Please make sure you only type numbers into the fields.");
            return;
        }

        // 4. Fire off the async task with our parsed numbers
       
        _ = FetchAndAssignHeight(longitude, latitude,0);
    }

    public void RequestHeightFromServer(double longitude, double latitude, int ticketNum)
    {

   
        // 4. Fire off the async task with our parsed numbers
       
        _ = FetchAndAssignHeight(longitude, latitude, ticketNum);
    }

    private async Task FetchAndAssignHeight(double longitude, double latitude,int ticketNum)
    {
        if (cesiumTileset == null) return;

        // The Z value (0.0) is ignored during input sampling
        double3 targetLocation = new double3(longitude, latitude, 0.0);
        
        // Request height mapping from Cesium
        CesiumSampleHeightResult result = await cesiumTileset.SampleHeightMostDetailed(targetLocation);

        // FIX: Verify Cesium successfully sampled the height array at our first index
        if (result.sampleSuccess != null && result.sampleSuccess.Length > 0 && result.sampleSuccess[0]) 
        {
            // The Z value of the output position contains our processed height!
            latestHeight = result.longitudeLatitudeHeightPositions[0].z; 

            markerSpawner.SpawnMarker(longitude, latitude,latestHeight ,ticketNum);
            
            Debug.Log($"Height updated to: {latestHeight} meters");
        }
        else
        {
            Debug.LogWarning($"Failed to sample height at Lon: {longitude}, Lat: {latitude}");
        }
    }
}