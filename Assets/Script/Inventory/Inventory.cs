using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Inventory : MonoBehaviour
{
    [Header("OTHER SCRIPTS REFERENCES")]
    [SerializeField]
    private Equipment equipment;

    [SerializeField]
    private ItemActionsSystem itemActionsSystem;

    [Header("INVENTORY SYSTEM VARIABLES")]
    [SerializeField]
    private List<ItemInInventory> content = new List<ItemInInventory>();

    [SerializeField]
    private GameObject inventoryPanel;

    [SerializeField]
    private Transform inventorySlotsParent;

    [SerializeField]
    private GameObject optionsPanel; // Référence au panel des options

    public Sprite emptySlotVisual;
    public MoveBehaviour playerMovement;



    public static Inventory instance;

    const int InventorySize = 24;
    private bool isOpen = false;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
      
        CloseInventory();
        RefreshContent();
    }

    private void Update()
    {
        // Vérifie si le panel d'options est actif
        if (!optionsPanel.activeSelf) 
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                if (isOpen)
                {
                    CloseInventory();
                }
                else
                {
                    OpenInventory();
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape) && isOpen)
        {
            CloseInventory();
        }
    }

    public void AddItem(ItemData item)
    {
        ItemInInventory[] itemInInventory = content.Where(elem => elem.itemData == item).ToArray();
        bool itemAdded = false;

        if (itemInInventory.Length > 0 && item.stackable)
        {
            for (int i = 0; i < itemInInventory.Length; i++)
            {
                if (itemInInventory[i].count < item.maxStack)
                {
                    itemAdded = true;
                    itemInInventory[i].count++;
                    break;
                }
            }

            if (!itemAdded)
            {
                content.Add(new ItemInInventory
                {
                    itemData = item,
                    count = 1
                });
            }
        }
        else
        {
            content.Add(new ItemInInventory
            {
                itemData = item,
                count = 1
            });
        }

        RefreshContent();
    }

    public void RemoveItem(ItemData item)
    {
        ItemInInventory itemInInventory = content.Where(elem => elem.itemData == item).FirstOrDefault();

        if (itemInInventory != null && itemInInventory.count > 1)
        {
            itemInInventory.count--;
        }
        else
        {
            content.Remove(itemInInventory);
        }

        RefreshContent();
    }

    public List<ItemInInventory> GetContent()
    {
        return content;
    }

    private void OpenInventory()
    {
        inventoryPanel.SetActive(true);
        playerMovement.StopMovement();

        isOpen = true;
    }

    public void CloseInventory()
    {
        inventoryPanel.SetActive(false);
        itemActionsSystem.actionPanel.SetActive(false);
        TooltipSystem.instance.Hide();
        playerMovement.ResumeMovement();
        isOpen = false;
    }

    public void RefreshContent()
    {
        for (int i = 0; i < inventorySlotsParent.childCount; i++)
        {
            Slot currentSlot = inventorySlotsParent.GetChild(i).GetComponent<Slot>();

            currentSlot.item = null;
            currentSlot.itemVisual.sprite = emptySlotVisual;
            currentSlot.countText.enabled = false;
        }

        for (int i = 0; i < content.Count; i++)
        {
            Slot currentSlot = inventorySlotsParent.GetChild(i).GetComponent<Slot>();

            currentSlot.item = content[i].itemData;
            currentSlot.itemVisual.sprite = content[i].itemData.visual;

            if (currentSlot.item.stackable)
            {
                currentSlot.countText.enabled = true;
                currentSlot.countText.text = content[i].count.ToString();
            }
        }

        equipment.UpdateEquipmentsDesequipButtons();
    }

    public bool IsFull()
    {
        return InventorySize == content.Count;
    }

    public void LoadData(List<ItemInInventory> savedData)
    {
        content = savedData;
        RefreshContent();
    }

    public void ClearContent()
    {
        content.Clear();
    }
}

[System.Serializable]
public class ItemInInventory
{
    public ItemData itemData;
    public int count;
}
