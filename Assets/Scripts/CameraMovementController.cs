using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMovementController : MonoBehaviour
{
    //Controls how fast the camera moves.
    public float panSpeed = 20f;

    //Defines how close the mouse should be to the screen edges to trigger the camera movement.
    public float panBorderThickness = 10f;

    //Allows you to define boundaries within which the camera can move.
    public Vector2 panLimit = new Vector2(50,50);

    //Speed for rotating the camera
    public float rotationSpeed = 100f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;

        CornerMovement(pos);       
    }
    /// <summary>
    /// Moves Camera when mouse it as border.
    /// </summary>
    private void CornerMovement(Vector3 pos)
    {        

        if (Input.mousePosition.y >= Screen.height - panBorderThickness)
        {
            pos.z += panSpeed * Time.deltaTime;
        }
        if (Input.mousePosition.y <= panBorderThickness)
        {
            pos.z -= panSpeed * Time.deltaTime;
        }
        if (Input.mousePosition.x >= Screen.width - panBorderThickness)
        {
            pos.x += panSpeed * Time.deltaTime;
        }
        if (Input.mousePosition.x <= panBorderThickness)
        {
            pos.x -= panSpeed * Time.deltaTime;
        }

        pos.x = Mathf.Clamp(pos.x, -panLimit.x, panLimit.x);
        pos.z = Mathf.Clamp(pos.z, -panLimit.y, panLimit.y);

        transform.position = pos;
    }    
}
