using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHandContainer : MonoBehaviour
{
    public GameObject PlayerHand;
    public Button SliderButton;
    private Animator animator;
    private bool isVisible = false;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        SliderButton.onClick.AddListener(TogglePanel);
    }

    private void TogglePanel()
    {
        isVisible = !isVisible;

        if(isVisible)
            FillPlayerHand();

        animator.SetTrigger("Slide");


    }
    public void FillPlayerHand()
    {
        // first delete all children
        foreach (Transform child in PlayerHand.transform)
        {
            // Destroy each child
            Destroy(child.gameObject);
        }

        ChipManager.Instance.RefreshPlayerHand();

        foreach (var chip in ChipManager.Instance.PlayerHand)
        {
            GameObject tempNewChip = Instantiate(ChipManager.Instance.chipPrefab, PlayerHand.transform);

            tempNewChip.GetComponent<Chip>().Mode = Chip.ChipMode.Combat;

            tempNewChip.GetComponent<Chip>().NewChip = chip;
        }
    }
}
