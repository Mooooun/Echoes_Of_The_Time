using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Equipment : MonoBehaviour
{
    [Header("OTHER SCRIPTS REFERENCES")]
    [SerializeField]
    private ItemActionsSystem itemActionsSystem;

    [SerializeField]
    private PlayerStats playerStats;

    [Header("EQUIPMENT SYSTEM VARIABLES")]
    [SerializeField]
    private EquipmentLibrary equipmentLibrary;

    [SerializeField]
    private Image headSlotImage;

    [SerializeField]
    private Image chestSlotImage;

    [SerializeField]
    private Image handsSlotImage;

    [SerializeField]
    private Image legsSlotImage;

    [SerializeField]
    private Image weaponSlotImage;

    [SerializeField]
    private Image bowSlotImage; // Nouveau slot pour l'arc

    // Garde une trace des équipements actuels
    [HideInInspector]
    public ItemData equipedHeadItem;
    [HideInInspector]
    public ItemData equipedChestItem;
    [HideInInspector]
    public ItemData equipedHandsItem;
    [HideInInspector]
    public ItemData equipedLegsItem;
    [HideInInspector]
    public ItemData equipedFeetItem;
    [HideInInspector]
    public ItemData equipedWeaponItem;
    [HideInInspector]
    public ItemData equipedBowItem; // Nouveau pour l'arc

    [SerializeField]
    private Button headSlotDesequipButton;

    [SerializeField]
    private Button chestSlotDesequipButton;

    [SerializeField]
    private Button handsSlotDesequipButton;

    [SerializeField]
    private Button legsSlotDesequipButton;

    [SerializeField]
    private Button weaponSlotDesequipButton;

    [SerializeField]
    private Button bowSlotDesequipButton; // Nouveau bouton pour l'arc

    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private AudioClip equipSound;

    private void DisablePreviousEquipedEquipment(ItemData itemToDisable)
    {
        if (itemToDisable == null)
        {
            return;
        }

        EquipmentLibraryItem equipmentLibraryItem = equipmentLibrary.content
            .Where(elem => elem.itemData == itemToDisable)
            .FirstOrDefault();

        if (equipmentLibraryItem != null)
        {
            for (int i = 0; i < equipmentLibraryItem.elementsToDisable.Length; i++)
            {
                equipmentLibraryItem.elementsToDisable[i].SetActive(true);
            }

            equipmentLibraryItem.itemPrefab.SetActive(false);
        }

        playerStats.currentArmorPoints -= itemToDisable.armorPoints;
        Inventory.instance.AddItem(itemToDisable);
    }

    public void DesequipEquipment(EquipmentType equipmentType)
    {
        if (Inventory.instance.IsFull())
        {
            Debug.Log("L'inventaire est plein, impossible de se déséquiper de cet élément");
            return;
        }

        ItemData currentItem = null;

        switch (equipmentType)
        {
            case EquipmentType.Head:
                currentItem = equipedHeadItem;
                equipedHeadItem = null;
                headSlotImage.sprite = Inventory.instance.emptySlotVisual;
                break;

            case EquipmentType.Chest:
                currentItem = equipedChestItem;
                equipedChestItem = null;
                chestSlotImage.sprite = Inventory.instance.emptySlotVisual;
                break;

            case EquipmentType.Hands:
                currentItem = equipedHandsItem;
                equipedHandsItem = null;
                handsSlotImage.sprite = Inventory.instance.emptySlotVisual;
                break;

            case EquipmentType.Legs:
                currentItem = equipedLegsItem;
                equipedLegsItem = null;
                legsSlotImage.sprite = Inventory.instance.emptySlotVisual;
                break;

            case EquipmentType.Weapon:
                currentItem = equipedWeaponItem;
                equipedWeaponItem = null;
                weaponSlotImage.sprite = Inventory.instance.emptySlotVisual;
                break;

            case EquipmentType.Bow: // Nouveau cas pour l'arc
                currentItem = equipedBowItem;
                equipedBowItem = null;
                bowSlotImage.sprite = Inventory.instance.emptySlotVisual;
                break;
        }

        DisablePreviousEquipedEquipment(currentItem);

        Inventory.instance.RefreshContent();
    }

    public void UpdateEquipmentsDesequipButtons()
    {
        headSlotDesequipButton.onClick.RemoveAllListeners();
        headSlotDesequipButton.onClick.AddListener(() => DesequipEquipment(EquipmentType.Head));
        headSlotDesequipButton.gameObject.SetActive(equipedHeadItem != null);

        chestSlotDesequipButton.onClick.RemoveAllListeners();
        chestSlotDesequipButton.onClick.AddListener(() => DesequipEquipment(EquipmentType.Chest));
        chestSlotDesequipButton.gameObject.SetActive(equipedChestItem != null);

        handsSlotDesequipButton.onClick.RemoveAllListeners();
        handsSlotDesequipButton.onClick.AddListener(() => DesequipEquipment(EquipmentType.Hands));
        handsSlotDesequipButton.gameObject.SetActive(equipedHandsItem != null);

        legsSlotDesequipButton.onClick.RemoveAllListeners();
        legsSlotDesequipButton.onClick.AddListener(() => DesequipEquipment(EquipmentType.Legs));
        legsSlotDesequipButton.gameObject.SetActive(equipedLegsItem != null);

        weaponSlotDesequipButton.onClick.RemoveAllListeners();
        weaponSlotDesequipButton.onClick.AddListener(() => DesequipEquipment(EquipmentType.Weapon));
        weaponSlotDesequipButton.gameObject.SetActive(equipedWeaponItem != null);

        // Ajouter la configuration pour le bouton de déséquipement du slot d'arc
        bowSlotDesequipButton.onClick.RemoveAllListeners();
        bowSlotDesequipButton.onClick.AddListener(() => DesequipEquipment(EquipmentType.Bow));
        bowSlotDesequipButton.gameObject.SetActive(equipedBowItem != null);
    }

    public void EquipAction(ItemData equipment = null)
    {
        ItemData itemToEquip = equipment ? equipment : itemActionsSystem.itemCurrentlySelected;

        // Recherche de l'item dans la bibliothèque d'équipement
        EquipmentLibraryItem equipmentLibraryItem = equipmentLibrary.content
            .Where(elem => elem.itemData == itemToEquip)
            .FirstOrDefault();

        if (equipmentLibraryItem != null)
        {
            // Déséquipe l'équipement précédent
            switch (itemToEquip.equipmentType)
            {
                case EquipmentType.Head:
                    DisablePreviousEquipedEquipment(equipedHeadItem);
                    headSlotImage.sprite = itemToEquip.visual;
                    equipedHeadItem = itemToEquip;
                    break;

                case EquipmentType.Chest:
                    DisablePreviousEquipedEquipment(equipedChestItem);
                    chestSlotImage.sprite = itemToEquip.visual;
                    equipedChestItem = itemToEquip;
                    break;

                case EquipmentType.Hands:
                    DisablePreviousEquipedEquipment(equipedHandsItem);
                    handsSlotImage.sprite = itemToEquip.visual;
                    equipedHandsItem = itemToEquip;
                    break;

                case EquipmentType.Legs:
                    DisablePreviousEquipedEquipment(equipedLegsItem);
                    legsSlotImage.sprite = itemToEquip.visual;
                    equipedLegsItem = itemToEquip;
                    break;

                case EquipmentType.Weapon:
                    DisablePreviousEquipedEquipment(equipedWeaponItem);
                    weaponSlotImage.sprite = itemToEquip.visual;  // Mise à jour du visuel du slot d'arme
                    equipedWeaponItem = itemToEquip;
                    break;

                case EquipmentType.Bow: // Nouveau cas pour l'arc
                    DisablePreviousEquipedEquipment(equipedBowItem);
                    bowSlotImage.sprite = itemToEquip.visual; // Mise à jour du visuel du slot d'arc
                    equipedBowItem = itemToEquip;
                    break;
            }

            // Désactive les éléments correspondants dans la bibliothèque et active l'objet équipé
            for (int i = 0; i < equipmentLibraryItem.elementsToDisable.Length; i++)
            {
                equipmentLibraryItem.elementsToDisable[i].SetActive(false);
            }

            equipmentLibraryItem.itemPrefab.SetActive(true);

            playerStats.currentArmorPoints += itemToEquip.armorPoints;

            Inventory.instance.RemoveItem(itemToEquip);

            audioSource.PlayOneShot(equipSound);
        }
        else
        {
            Debug.LogError("Equipment : " + itemToEquip.name + " non existant dans la librairie des équipements");
        }

        itemActionsSystem.CloseActionPanel();
    }

    public void LoadEquipments(ItemData[] savedEquipments)
    {
        // 1. On efface le contenu de l'inventaire pour éviter que DesequipEquipment ne soit bloqué par un inventaire plein
        Inventory.instance.ClearContent();

        // 2. On déséquipe tout ce qu'il y a actuellement sur le joueur
        foreach (EquipmentType type in System.Enum.GetValues(typeof(EquipmentType)))
        {
            DesequipEquipment(type);
        }

        // 3. Chargement des équipements
        foreach (ItemData item in savedEquipments)
        {
            if (item)
            {
                EquipAction(item);
            }
        }
    }
}
