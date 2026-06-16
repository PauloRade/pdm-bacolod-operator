using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class TicketFetcher : MonoBehaviour
{
    private string apiUrl = "https://dev.vantiq.com/api/v1/resources/custom/a.a.StateDatabase?token=IJkTMtZwFfRT7VtqIXrxeNEmCYmFa-jUZDdv9WF6s74=&where={\"Id\":1}&props=[\"Data\"]";
    public DatabaseFetcher databaseFetcher;
    void Start()
    {
        // Start the fetch request
        StartCoroutine(FetchTicketCountLoop());
    }
    private IEnumerator FetchTicketCountLoop()
    {
        while (true)
        {
            // Call your actual ticket fetching logic
            yield return StartCoroutine(FetchTicketCount());

            // Wait for 5 seconds before running the loop again
            yield return new WaitForSeconds(5f);
        }
    }

    IEnumerator FetchTicketCount()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(apiUrl))
        {
            // Send the request and wait for a response
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"Error fetching data: {webRequest.error}");
            }
            else
            {
                string rawJson = webRequest.downloadHandler.text;
                Debug.Log($"Raw JSON Received: {rawJson}");

                // Parse the JSON array
                ParseTickets(rawJson);
            }
        }
    }

    private void ParseTickets(string jsonArrayString)
    {
        try
        {
            // Fix JSON array for JsonUtility by wrapping it in an object
            string fixedJson = "{\"items\":" + jsonArrayString + "}";
            
            StateDatabaseWrapper wrapper = JsonUtility.FromJson<StateDatabaseWrapper>(fixedJson);

            if (wrapper != null && wrapper.items != null && wrapper.items.Length > 0)
            {
                int ticketCount = wrapper.items[0].Data.NumberOfTickets;
                databaseFetcher.FetchAllDataUpTo(ticketCount);
                Debug.Log($"Success! Number of Tickets: {ticketCount}");
                
                // Do something with ticketCount here (e.g., update UI)
            }
            else
            {
                Debug.LogWarning("JSON parsed, but no items were found.");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to parse JSON: {e.Message}");
        }
    }
}

// Data structures mapped to your JSON format
[Serializable]
public class StateDatabaseWrapper
{
    public TicketDataContainer[] items;
}

[Serializable]
public class TicketDataContainer
{
    public string _id;
    public TicketData Data;
}

[Serializable]
public class TicketData
{
    public int NumberOfTickets;
}