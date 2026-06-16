using UnityEngine;
using UnityEngine.UI; // Required for RawImage
using TMPro;
using System; // Required for Convert.FromBase64String

public class TicketInfoUpdater : MonoBehaviour
{
    [Header("TextMeshPro UI References")]
    public TextMeshProUGUI ticketNumText;
    public TextMeshProUGUI referenceCodeText;
    public TextMeshProUGUI emergencyTypeText;
    public TextMeshProUGUI severityText;
    public TextMeshProUGUI peopleInvolvedText;
    public TextMeshProUGUI latText; 
    public TextMeshProUGUI longText;
    public TextMeshProUGUI addressText;
    public TextMeshProUGUI notesText;

    [Header("UI Image Reference")]
    [Tooltip("Use a RawImage component instead of an Image component for Base64 textures.")]
    public RawImage incidentRawImage; 

    /// <summary>
    /// Takes incident data, maps text to components, and decodes the Base64 image string.
    /// </summary>
    public void ProcessAndShowTicket(IncidentData incident)
    {
        if (incident == null)
        {
            Debug.LogError("TicketInfoUpdater: Incident data is null! Cannot update UI.");
            return;
        }

        // 1. Assign values to UI text fields
        if (ticketNumText != null)       ticketNumText.text = $"Ticket #{incident.ticketNum}";
        if (referenceCodeText != null)   referenceCodeText.text = incident.referenceCode;
        if (emergencyTypeText != null)   emergencyTypeText.text = incident.emergencyType;
        if (severityText != null)        severityText.text = incident.severityThreshold;
        if (peopleInvolvedText != null)  peopleInvolvedText.text = incident.peopleInvolved;
        if (addressText != null)         addressText.text = incident.addressText;
        if (notesText != null)           notesText.text = incident.situationalNotes;
        if (latText != null)             latText.text = incident.TextLat;
        if (longText != null)            longText.text = incident.TextLong;

        // 2. Decode and display the Base64 image
        if (incidentRawImage != null && !string.IsNullOrEmpty(incident.incidentImage))
        {
            DisplayBase64Image(incident.incidentImage);
        }
    }

    /// <summary>
    /// Converts a Base64 string into a Texture2D and applies it to the RawImage component.
    /// </summary>
    private void DisplayBase64Image(string base64String)
    {
        try
        {
            // Clean up the string if it contains standard web prefixes like "data:image/png;base64,"
            if (base64String.Contains(","))
            {
                base64String = base64String.Split(',')[1];
            }

            // Convert the Base64 string back into a raw byte array
            byte[] imageBytes = Convert.FromBase64String(base64String);

            // Create a temporary blank texture (size doesn't matter, LoadImage auto-resizes it)
            Texture2D tex = new Texture2D(2, 2);
            
            // Load the raw bytes directly into the texture
            if (tex.LoadImage(imageBytes))
            {
                // Assign the newly generated texture to your UI RawImage component
                incidentRawImage.texture = tex;
            }
            else
            {
                Debug.LogError("TicketInfoUpdater: Failed to load bytes into Texture2D.");
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"TicketInfoUpdater: Error parsing Base64 image string: {ex.Message}");
        }
    }
}