using UnityEngine;

public class BorderColorController : MonoBehaviour
{
    [Header("References")]
    // Public reference to your custom border component
    public MeshGradientHollowRoundedBorder customBorder;

    [Header("Color Settings")]
    // Pre-setting the colors to E5E5E5 and EE8614
    public Color colorGray = new Color32(0xE5, 0xE5, 0xE5, 0xFF);
    public Color colorOrange = new Color32(0xEE, 0x86, 0x14, 0xFF);

    private void Start()
    {
        
    }

    public void SetBorderActive()
    {
        if (customBorder == null) return;

        

        customBorder.colorA = colorOrange;
        customBorder.colorB = colorOrange;
        customBorder.colorC = colorOrange;
        customBorder.colorD = colorOrange;
        Debug.Log("CHANGING COLOR");

        customBorder.updateColor();
        
        // Note: If your custom mesh component requires a refresh/redraw method 
        // to update visually in real-time, call it here. (e.g., customBorder.SetVerticesDirty();)
    }

    /// <summary>
    /// Turns the border "Inactive" by changing all corners to Gray (E5E5E5).
    /// </summary>
    public void SetBorderInactive()
    {
        if (customBorder == null) return;

        customBorder.colorA = colorGray;
        customBorder.colorB = colorGray;
        customBorder.colorC = colorGray;
        customBorder.colorD = colorGray;
        Debug.Log("CHANGING COLOR");
        customBorder.updateColor();

        // Note: Call your component's refresh/redraw method here if needed.
    }
}