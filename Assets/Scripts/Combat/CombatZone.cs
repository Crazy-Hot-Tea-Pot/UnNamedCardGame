using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatZone : MonoBehaviour
{
    
    public GameManager managerClass;
    public List<GameObject> tempList;
    // Start is called before the first frame update
    void Start()
    {
        managerClass=GameManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            managerClass.StartCombat();
            for(int i = 0; i < tempList.Count; i++)
            {
                managerClass.RememberEnemy(tempList[i]);
            }
            for (int i = 0; i < tempList.Count; i++)
            {
                tempList.RemoveAt(0);
            }
        }
        if (other.tag == "Enemy")
        {
            //A for each loop to chceck if the element exists in the list
            bool tempCleared = true;
            foreach (GameObject tempCheck in tempList)
            {
                if(other.name == tempCheck.name)
                {
                    tempCleared = false;
                }
            }
            //Adds it to the list if cleared
            if(tempCleared == true)
            {
                tempList.Add(GameObject.Find(other.name));
            }
        }
    }

   
}
