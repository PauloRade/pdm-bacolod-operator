using UnityEngine;

public class MarkerSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private GameObject prefabToSpawn; // Drag your marker prefab here
    [SerializeField] private Transform parentObject;    // Drag the parent GameObject here

    // Call this function to spawn the marker
    public void SpawnMarker(double longitude, double latitude, double height, int ticketNum)
    {
      

        // Instantiates the prefab and sets its parent automatically
        GameObject spawnedMarker = Instantiate(prefabToSpawn, parentObject);

        // Resets the position so it snaps directly to the parent's position
        spawnedMarker.transform.localPosition = Vector3.zero;
        spawnedMarker.transform.localRotation = Quaternion.identity;

        MarkerManager manager = spawnedMarker.GetComponent<MarkerManager>();

        manager.moveMarkerTo(longitude, latitude,height,ticketNum);
    }

 
}