using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestCollision : MonoBehaviour
{
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
        //If the collision is a player
        if(other.transform.tag == "Player")
        {
            //For each quest in active quest
            foreach(Quest quest in QuestManager.Instance.activeQuest)
            {
                //Use touch pass a method that will only do things on it's corresponding quest
                quest.TouchPassThrough();
            }
            Destroy(this.gameObject);
        }
    }
}
