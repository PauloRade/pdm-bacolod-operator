using UnityEngine;
using TMPro; // Required to use TextMeshPro types

public class TicketManager : MonoBehaviour
{
    [Header("UI Text References")]
    public TextMeshProUGUI ticketCodeText;
    public TextMeshProUGUI severityText;
    public TextMeshProUGUI typeText;
    public TextMeshProUGUI noteText;
    public TextMeshProUGUI locationText;


    public DatabaseFetcher databaseFetcher;

    public int ticketNum;

    /// <summary>
    /// Sets the values of the UI text elements for this ticket.
    /// </summary>
    public void SetTicketDetails(int ticketNumber,string code, string severity, string type, string note, string location)
    {
        // 1. Safety check to make sure you didn't forget to link the fields in the inspector
        ticketNum = ticketNumber;
      

        // 2. Assign the string values to the .text property of the TMP components
        ticketCodeText.text = code;
        severityText.text = "SEVERITY: "+ severity;
        typeText.text = type;
        noteText.text = note;
        locationText.text = location;
    }

    public void FocusTicket()
    {
        databaseFetcher.ShowTicket(ticketNum);
    }
}