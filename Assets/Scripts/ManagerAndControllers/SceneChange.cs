using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    /// <summary>
    /// Stores the level name as a string
    /// </summary>
    public string nextLevel;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        //If player collids with object
        if (other.transform.tag == "Player")
        {
                //load next scene
                SceneManager.LoadScene(nextLevel);
        }
    }
}
