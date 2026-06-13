using UnityEngine;
using CesiumForUnity; // Don't forget this namespace!
using Unity.Mathematics;



public class MarkerManager : MonoBehaviour
{
    public CesiumGlobeAnchor cubeMarker;

    public MarkerScaleController markerScaleController;

    private float lastScaleValue;


    void Start()
    {
        lastScaleValue = markerScaleController.newScaleValue;
    }

    private void Update()
    {
       
        float currentScale = markerScaleController.newScaleValue;

        // Check if the scale value on the controller has changed
        if (currentScale != lastScaleValue)
        {
            
            // Apply the new scale to X and Z, keeping Y the same
            transform.localScale = new Vector3(currentScale, transform.localScale.y, currentScale);
            
            // Save the new value so we don't run this code again until it changes next time
            lastScaleValue = currentScale;
        }
       
    }



    public void moveMarkerTo(double longitude, double latitude, double height)
    {
        double3 globalCoordinates = new double3(longitude, latitude,height+20);

        // 4. Assign the coordinates to the globe anchor. Cesium snaps the object to its globe position automatically.
        cubeMarker.longitudeLatitudeHeight = globalCoordinates;
    }
}
