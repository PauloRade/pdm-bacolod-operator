using UnityEngine;
using System.Collections.Generic; // Required for using Lists

public class TicketSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [Tooltip("The ticket summary prefab you want to spawn.")]
    public GameObject ticketSummary;

    [Tooltip("The parent GameObject (Content UI panel) that will hold the spawned ticket.")]
    public Transform contentTicketSummary;

    [Header("Collected Tickets")]
    [Tooltip("List tracking all currently spawned ticket managers.")]
    [SerializeField] private List<TicketManager> spawnedTickets = new List<TicketManager>();

  
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
        spawnedTickets.Add(ticketManager);
        
        Debug.Log($"Successfully spawned {spawnedTicket.name} inside contentTicketSummary.", spawnedTicket);
    }

    public void LoopThroughTickets(int targetTicket)
    {
        // Safety check to make sure the list isn't empty
        if (spawnedTickets.Count == 0)
        {
            Debug.Log("No tickets have been spawned yet.");
            return;
        }

        foreach (TicketManager ticket in spawnedTickets)
        {
            // Null check just in case a ticket GameObject was destroyed externally
            if (ticket != null)
            {
                ticket.borderColorController.SetBorderInactive();
                Debug.Log("NOT mATCH");
                if(targetTicket == ticket.ticketNum)
                {
                    Debug.Log("mATCH");
                    ticket.borderColorController.SetBorderActive();
                }

               
            }
        }
    }

   

}