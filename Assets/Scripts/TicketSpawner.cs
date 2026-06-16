using UnityEngine;

public class TicketSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [Tooltip("The ticket summary prefab you want to spawn.")]
    public GameObject ticketSummary;

    [Tooltip("The parent GameObject (Content UI panel) that will hold the spawned ticket.")]
    public Transform contentTicketSummary;

  
    public void SpawnAsChild(int ticketNumber,string code, string severity, string type, string note, string location)
    {
       
        // 2. Spawn the object
        GameObject spawnedTicket = Instantiate(ticketSummary, contentTicketSummary.position, contentTicketSummary.rotation);

        // 3. Set the parent
        // 'false' ensures UI elements scale and position correctly relative to the parent panel
        spawnedTicket.transform.SetParent(contentTicketSummary, false);

        // 4. Get the TicketManager component from the spawned prefab
        TicketManager ticketManager = spawnedTicket.GetComponent<TicketManager>();
        ticketManager.SetTicketDetails(ticketNumber,code, severity, type, note, location);
        
        Debug.Log($"Successfully spawned {spawnedTicket.name} inside contentTicketSummary.", spawnedTicket);
    }

   

}