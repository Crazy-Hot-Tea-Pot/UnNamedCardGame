using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeController : MonoBehaviour
{
    public GameObject ChipPrefab;
    public UpgradeTerminalUIController UIController;
    public GameObject IntroText;
    public GameObject IdleText;

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
            IdleText.SetActive(false);
            IntroText.SetActive(true);
            UIController.DisplayIntro();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            IdleText.SetActive(true);
            IntroText.SetActive(false);
}
    }
}
