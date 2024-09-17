using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings
{
    // Speed of camera movement
    public float panSpeed = 20f;

    // How close to the edge before moving
    public float floatpenborderThickness=10f;

    // Limits for camera movement in X and Z directions
    public Vector2 panLimit = new Vector2(50, 50);

}
