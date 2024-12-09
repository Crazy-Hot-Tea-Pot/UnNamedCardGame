using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GearInventory : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image GearImage;
    public TextMeshProUGUI GearName;
    public TextMeshProUGUI GearDescription;
    public Button EquipButton;
    public GameObject GearInfoPrefab;
    public GameObject EffectPrefab;
    private GameObject gearInfoDisplay;

    public Item Item
    {
        get
        {
            return item;
        }
        set
        {
            item = value;
        }
    }
    void Start()
    {
        GearImage.sprite = Item.itemImage;
        GearName.SetText(Item.itemName);
        GearDescription.SetText(Item.itemDescription);
        EquipButton.interactable = !Item.IsEquipped;

        EquipButton.onClick.AddListener(EquipGear);
    }
    private Item item;
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (gearInfoDisplay == null)
        {

            gearInfoDisplay = Instantiate(GearInfoPrefab, UiManager.Instance.transform);

            GearInfoController controller = gearInfoDisplay.GetComponent<GearInfoController>();

            StartCoroutine(DelayForAnimator(controller));
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (gearInfoDisplay != null)
        {
            GearInfoController controller = gearInfoDisplay.GetComponent<GearInfoController>();

            // Animate shrinking
            Vector3 targetPosition = this.transform.position;
            Vector3 startPosition = UiManager.Instance.transform.position;

            controller.Shrink(startPosition, targetPosition);

            // Optional: Delay destruction for animation
            Destroy(gearInfoDisplay, 1f);
        }
    }
    private IEnumerator DelayForAnimator(GearInfoController controller)
    {
        yield return null;

        controller.GearName.SetText(Item.itemName);
        controller.GearDescription.SetText(Item.itemDescription);
        controller.GearImage.sprite = Item.itemImage;
        controller.GearType.SetText(Item.itemType.ToString());
        foreach(var effect in Item.itemEffects)
        {
            GameObject temp = null;
            temp = Instantiate(EffectPrefab, controller.EffectsContainer.transform);
            temp.GetComponent<TextMeshProUGUI>().SetText(effect.ItemEffectDescription);
        }
        

        //Animate

        Vector3 targetPosition = UiManager.Instance.transform.position;
        Vector3 startPosition = this.transform.position;
        controller.Enlarge(startPosition, targetPosition);
    }
    private void EquipGear()
    {
        if (GearManager.Instance.EquipGear(Item))
            UiManager.Instance.RefreshInventory();
        else
            Debug.LogWarning("UnExpected error here.");
    }
    void OnDestroy()
    {
        EquipButton.onClick.RemoveAllListeners();

        if (gearInfoDisplay != null)
            Destroy(gearInfoDisplay);
    }
}
