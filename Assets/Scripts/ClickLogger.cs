using UnityEngine;
using UnityEngine.InputSystem; // REQUIRED for Unity 6 New Input System

public class ClickLogger : MonoBehaviour
{
    void Update()
    {
        // 1. Check if the left mouse button was clicked this frame
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            // 2. Create a ray from the camera through the mouse pointer position
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit hit;

            // 3. Perform a physics raycast to see if it hits anything
            if (Physics.Raycast(ray, out hit))
            {
                // 4. Check if the object that got hit is THIS cube
                if (hit.transform == this.transform)
                {
                    Debug.Log("hi");
                    
                    // You can call your API fetch functions here!
                    // FetchDataById("27");
                }
            }
        }
    }
}