using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatZone : MonoBehaviour
{
    
    GameManager managerClass;
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
        if (other.tag == "Player")
        {
            managerClass.StartCombat();
            for(int i = 0; i < tempList.Count - 1; i++)
            {
                managerClass.RememberEnemy(tempList[i]);
            }
            for (int i = 0; i < tempList.Count - 1; i++)
            {
                tempList.RemoveAt(0);
            }
        }
        if (other.tag == "Enemy")
        {
            tempList[tempList.Count] = GameObject.Find(other.name);
        }
    }

   
}
