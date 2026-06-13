using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class DatabaseFetcher : MonoBehaviour
{
    private string baseUrl = "https://dev.vantiq.com/api/v1/resources/custom/a.a.Database";
    private string token = "IJkTMtZwFfRT7VtqIXrxeNEmCYmFa-jUZDdv9WF6s74=";

    public CesiumCoordinateSampler cesiumCoordinateSampler;

    void Start()
    {
        // Example: Automatically fetching ID "26" on start using the public function
        FetchDataById("27");
    }


    public void FetchDataById(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            Debug.LogWarning("FetchDataById called with an empty or null ID.");
            return;
        }

        // Dynamically build the URL with the custom ID
        string fullUrl = $"{baseUrl}?token={token}&where={{\"Id\":{id}}}&props=[\"Data\"]";
        
        // Start the network request coroutine
        StartCoroutine(FetchDatabaseData(fullUrl));
    }

    IEnumerator FetchDatabaseData(string url)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"Error fetching database data: {webRequest.error}");
            }
            else
            {
                string rawJson = webRequest.downloadHandler.text;
                Debug.Log($"Raw JSON Received: {rawJson}");

                ParseDatabaseResponse(rawJson);
            }
        }
    }

    private void ParseDatabaseResponse(string jsonArrayString)
    {
        try
        {
            // Wrap the top-level array so JsonUtility can read it
            string fixedJson = "{\"items\":" + jsonArrayString + "}";
            
            DatabaseWrapper wrapper = JsonUtility.FromJson<DatabaseWrapper>(fixedJson);

            if (wrapper != null && wrapper.items != null && wrapper.items.Length > 0)
            {
                // Grab the Data object from the first item in the array
                IncidentData incident = wrapper.items[0].Data;

                // Log all the retrieved data fields
                Debug.Log($"--- Incident Data Retrieved ---");
                Debug.Log($"Ticket Num: {incident.ticketNum}");
                Debug.Log($"Reference Code: {incident.referenceCode}");
                Debug.Log($"Emergency Type: {incident.emergencyType}");
                Debug.Log($"Severity: {incident.severityThreshold}");
                Debug.Log($"People Involved: {incident.peopleInvolved}");
                Debug.Log($"Location: Lat {incident.TextLat}, Long {incident.TextLong}");
                Debug.Log($"Address: {incident.addressText}");
                Debug.Log($"Notes: {incident.situationalNotes}");
                Debug.Log($"Image Ref: {incident.incidentImage}");

                // --- Convert string coordinates to double safely ---
                double latitude = 0.0;
                double longitude = 0.0;

                bool hasValidLat = double.TryParse(incident.TextLat, out latitude);
                bool hasValidLong = double.TryParse(incident.TextLong, out longitude);

                if (hasValidLat && hasValidLong)
                {
                    // Note: Ensure the parameter names/order match your specific Cesium component setup
                    cesiumCoordinateSampler.RequestHeightFromServer(longitude, latitude);
                }
                else
                {
                    Debug.LogWarning($"Skipped Cesium request. Coordinates could not be parsed. Raw Lat: '{incident.TextLat}', Raw Long: '{incident.TextLong}'");
                }

                
                // You can now pass 'incident' to your UI or game logic systems
            }
            else
            {
                Debug.LogWarning("JSON parsed successfully, but the array was empty.");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to parse Database JSON: {e.Message}");
        }
    }
}

// Data structures mapped to your new JSON format
[Serializable]
public class DatabaseWrapper
{
    public DatabaseItem[] items;
}

[Serializable]
public class DatabaseItem
{
    public string _id;
    public IncidentData Data;
}

[Serializable]
public class IncidentData
{
    public int ticketNum;
    public string referenceCode;
    public string TextLat;
    public string TextLong;
    public string addressText;
    public string emergencyType;
    public string severityThreshold;
    public string peopleInvolved; // Mapped as string since it's "2" in JSON
    public string situationalNotes;
    public string incidentImage;
}