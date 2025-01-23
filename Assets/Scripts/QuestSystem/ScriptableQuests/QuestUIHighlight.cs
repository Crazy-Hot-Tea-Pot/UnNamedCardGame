using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "QuestUIHighlight", menuName = "Quest/QuestUIHighlight")]
public class QuestUIHighlight : Quest
{
    public Material mat;
    [Header("This will find and utalize a button")]
    public GameObject UIElement;

    public override void RunQuest()
    {
        //If we have a button to change
        if (UIElement.GetComponent<Button>() == true)
        {
            //Get the image component and apply our material
            UIElement.GetComponent<Image>().material = mat;

            //Add a button component
            UIElement.GetComponent<Button>().onClick.RemoveListener(ButtonCheck);
            UIElement.GetComponent<Button>().onClick.AddListener(ButtonCheck);
        }
    }

    public void ButtonCheck()
    {
        UIElement.GetComponent<Button>().onClick.RemoveListener(ButtonCheck);
        UIElement.GetComponent<Image>().material = null;
        CompleteQuest();
    }
}
