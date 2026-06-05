using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;

public class ShrinkCesiumCredits : MonoBehaviour
{
    void Start()
    {
        // Give Cesium a split second to generate the UI elements at start
        StartCoroutine(FindAndShrinkCredits());
    }

    private IEnumerator FindAndShrinkCredits()
    {
        yield return new WaitForSeconds(0.2f);

        UIDocument uiDoc = FindAnyObjectByType<UIDocument>();
        if (uiDoc != null && uiDoc.rootVisualElement != null)
        {
            // Cesium internally wraps its text elements inside a container. 
            // We search the UI tree for any element matching their credits layout.
            VisualElement creditsElement = uiDoc.rootVisualElement.Q(className: "cesium-credits") 
                                           ?? uiDoc.rootVisualElement.Q("cesium-credits");

            // If a specific class isn't found, target the text/image container directly
            if (creditsElement == null)
            {
                creditsElement = uiDoc.rootVisualElement.ElementAt(0);
            }

            if (creditsElement != null)
            {
                // Force font size reduction across all child elements inside the credits
                creditsElement.style.fontSize = 9;
                
                // Shrink the boundaries and the image elements to 40% size
                creditsElement.style.scale = new Scale(new Vector2(0.4f, 0.4f));
                
                // Align it tightly to the bottom right of your screen/render window
                creditsElement.style.transformOrigin = new TransformOrigin(Length.Percent(100), Length.Percent(100));
                
                Debug.Log("Cesium internal credits localized and shrunk!");
            }
        }
    }
}