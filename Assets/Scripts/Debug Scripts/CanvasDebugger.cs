using UnityEngine;
using UnityEngine.UI;

public class CanvasDebugger : MonoBehaviour
{
    // Assign the broken Canvas in the Inspector
    public Canvas canvas; 

    void Start()
    {
        DebugCanvasObjects();
    }

    void DebugCanvasObjects()
    {
        foreach (RectTransform rect in canvas.GetComponentsInChildren<RectTransform>())
        {
            Debug.Log($"Object: {rect.name}, Position: {rect.position}, SizeDelta: {rect.sizeDelta}, Scale: {rect.localScale}");

            // Check for invalid or extreme values
            if (float.IsNaN(rect.position.x) || float.IsNaN(rect.position.y) || float.IsNaN(rect.localScale.x) || float.IsNaN(rect.localScale.y))
            {
                Debug.LogError($"Invalid RectTransform values detected on {rect.name}");
            }

            if (rect.sizeDelta.x > 10000 || rect.sizeDelta.y > 10000)
            {
                Debug.LogError($"Extreme SizeDelta on {rect.name}: {rect.sizeDelta}");
            }

            if (rect.position.magnitude > 100000)
            {
                Debug.LogError($"Extreme Position on {rect.name}: {rect.position}");
            }
        }
    }
}
