using UnityEngine;
using CesiumForUnity; // Don't forget this namespace!
using Unity.Mathematics; // Needed for the double3 data type

public class MarkerSpawner : MonoBehaviour
{
    [Header("Marker Settings")]
    [Tooltip("The prefab of the marker pin/flag you want to spawn.")]
    public GameObject markerPrefab;

    [Tooltip("Height in meters above the WGS84 ellipsoid. 0 is rough sea-level. Adjust if marker spawns inside terrain.")]
    public double defaultSpawnHeight = 10.0; 

    /// <summary>
    /// Public function to spawn a marker using just Latitude and Longitude.
    /// </summary>
    /// <param name="latitude">The GPS Latitude value</param>
    /// <param name="longitude">The GPS Longitude value</param>
    public void SpawnMarkerAtCoordinates(double latitude, double longitude)
    {
        if (markerPrefab == null)
        {
            Debug.LogError("Marker Spawner: Please assign a Marker Prefab in the inspector!");
            return;
        }

        // 1. Instantiate your marker prefab at standard default zeroed vectors
        GameObject newMarker = Instantiate(markerPrefab, Vector3.zero, Quaternion.identity);

        // 2. Add the CesiumGlobeAnchor component dynamically
        CesiumGlobeAnchor globeAnchor = newMarker.AddComponent<CesiumGlobeAnchor>();

        // 3. Cesium requires data in a double3 vector format organized as: (Longitude, Latitude, Height)
        // Note: Make sure to pass Longitude FIRST, then Latitude.
        double3 globalCoordinates = new double3(longitude, latitude, defaultSpawnHeight);

        // 4. Assign the coordinates to the globe anchor. Cesium snaps the object to its globe position automatically.
        globeAnchor.longitudeLatitudeHeight = globalCoordinates;

        Debug.Log($"Marker successfully spawned at Lat: {latitude}, Long: {longitude}");
    }
}