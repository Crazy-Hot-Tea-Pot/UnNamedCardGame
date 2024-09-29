using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPickups : MonoBehaviour
{
    public GameObject cardObject;
    public GameManager gameManger;
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
        if(other.tag == "Player")
        {
            gameManger.PickUpCard(cardObject, this.gameObject);
        }
    }
}
