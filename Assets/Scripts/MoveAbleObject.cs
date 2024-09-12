using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script was for testing and can be used for other use in future.
/// </summary>
public class MoveAbleObject : MonoBehaviour
{
    // Material of Object. Mainly for debugging/Testing purposes to see if object is selected or not.
    public Material material;

    void Start()
    {
        // Set object Outline to clear as it doesn't start selected.
        //material.SetColor("_OutlineColor", Color.clear);
    }
    //On mouseDown on the object this method is called
    //private void OnMouseDown()
    //{
    //    //Get all objects that are moveable in scene
    //    MoveAbleObject[] allObjects = FindObjectsOfType<MoveAbleObject>();

    //    //Deselect other objects
    //    foreach (var temp in allObjects)
    //    {
    //        temp.Deselect();  // Make sure only one object is selected at a time
    //    }

    //    //Set Color of outline shader to green
    //    material.SetColor("_OutlineColor", Color.green);
    //}
    /// <summary>
    /// Deselect object 
    /// here for future if we have animation or some icon to show the player we have the object selected.
    /// </summary>
    //public void Deselect()
    //{
    //    //Set color of outline shader to clear
    //    material.SetColor("_OutlineColor", Color.clear);

    //}
}
