using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    /// <summary>
    /// Stores the level name as a string
    /// </summary>
    public Levels nextLevel;


    private void OnTriggerEnter(Collider other)
    {
        //If Player collids with object
        if (other.transform.tag == "Player")
        {
            //load next scene
            GameManager.Instance.RequestScene(nextLevel);
        }
    }
}
