using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CombatZone : MonoBehaviour
{
    
    public List<GameObject> tempList;
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
        if (other.tag == "Enemy")
        {
            tempList.Add(other.gameObject);
            Debug.Log("I am enemy: " + other.name);
        }

        if (other.tag == "Player")
        {
            this.GetComponent<BoxCollider>().enabled = false;                
            
            
            for(int i = 0; i < tempList.Count; i++)
            {
                //GameManager.Instance.RememberEnemy(tempList[i]);
                GameObject.Find("GameManager").GetComponent<GameManager>().RememberEnemy(tempList[i]);
            }
            for (int i = 0; i < tempList.Count; i++)
            {
                tempList.RemoveAt(0);
            }

            GameObject.FindGameObjectWithTag("CombatController").GetComponent<CombatController>().StartCombat();
            Destroy(this.gameObject);
        }
        //if (other.tag == "Enemy")
        //{
        //    //A for each loop to check if the element exists in the list
        //    bool tempCleared = true;
        //    foreach (GameObject tempCheck in tempList)
        //    {
        //        if(other.name == tempCheck.name)
        //        {
        //            tempCleared = false;
        //        }
        //    }
        //    //Adds it to the list if cleared
        //    if(tempCleared == true)
        //    {
        //        tempList.Add(GameObject.Find(other.name));
        //    }
        //}
    }


}
