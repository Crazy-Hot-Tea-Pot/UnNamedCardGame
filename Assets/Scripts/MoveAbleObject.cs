using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAbleObject : MonoBehaviour
{
    private bool selected;
    private Renderer objectRenderer;
    public Material material;
    public GameObject Selected
    {
        get
        {
            if (selected)                
                return this.gameObject;
            else               
                return null;
        }
    }
    void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        material.SetColor("_OutlineColor", Color.clear);
    }
    private void OnMouseDown()
    {
        MoveAbleObject[] allObjects = FindObjectsOfType<MoveAbleObject>();
        foreach (var obj in allObjects)
        {
            obj.Deselect();  // Make sure only one object is selected at a time
        }

        // Set this object as selected
        selected = true;
        material.SetColor("_OutlineColor", Color.green);
        //objectRenderer.material = outlineMaterial;
    }
    /// <summary>
    /// Deselect object 
    /// here for future if we have animation or some icon to show the player we have the object selected.
    /// </summary>
    public void Deselect()
    {
        material.SetColor("_OutlineColor", Color.clear);
        selected = false;
    }
}
