using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractBehaviour : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private MoveBehaviour playerMoveBehaviour;

    [SerializeField]
    private Animator playerAnimator;

    [SerializeField]
    private Inventory inventory;

    [SerializeField]
    private Equipment equipmentSystem;

    [SerializeField]
    private EquipmentLibrary equipmentLibrary;

    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private Text pickedUpItemText; // Reference to the UI Text component

    [SerializeField]
    private Image pickedUpItemImage; // Reference to the UI Image component for the item visual

    [HideInInspector]
    public bool isBusy = false;

    [Header("Tools Configuration")]
    [SerializeField]
    private GameObject pickaxeVisual;

    [SerializeField]
    private AudioClip pickaxeSound;

    [SerializeField]
    private GameObject axeVisual;

    [SerializeField]
    private AudioClip axeSound;

    [Header("Other")]
    [SerializeField]
    private AudioClip pickupSound;

    private Item currentItem;
    private Harvestable currentHarvestable;
    private Tool currentTool;

    private Vector3 spawnItemOffset = new Vector3(0, 0.5f, 0);

    public void DoPickup(Item item)
    {
        if (isBusy)
        {
            return;
        }

        isBusy = true;

        if (inventory.IsFull())
        {
            Debug.Log("Inventory full, can't pick up : " + item.name);
            return;
        }

        currentItem = item;

        playerAnimator.SetTrigger("Pickup");
        playerMoveBehaviour.canMove = false;
    }

    public void DoHarvest(Harvestable harvestable)
    {
        if (isBusy)
        {
            return;
        }

        isBusy = true;

        currentTool = harvestable.tool;
        EnableToolGameObjectFromEnum(currentTool);

        currentHarvestable = harvestable;
        playerAnimator.SetTrigger("Harvest");
        playerMoveBehaviour.canMove = false;
    }

    // coroutine
    IEnumerator BreakHarvestable()
    {
        Harvestable currentlyHarvesting = currentHarvestable;

        // Permet de désactiver la possibilité d'intéragir avec ce Harvestable + d'un fois (passage du layer Harvestable à Default)
        currentlyHarvesting.gameObject.layer = LayerMask.NameToLayer("Default");

        if (currentlyHarvesting.disableKinematicOnHarvest)
        {
            Rigidbody rigidbody = currentlyHarvesting.gameObject.GetComponent<Rigidbody>();
            rigidbody.isKinematic = false;
            rigidbody.AddForce(transform.forward * 800, ForceMode.Impulse);
        }

        yield return new WaitForSeconds(currentlyHarvesting.destroyDelay);

        for (int i = 0; i < currentlyHarvesting.harvestableItems.Length; i++)
        {
            Ressource ressource = currentlyHarvesting.harvestableItems[i];

            if (Random.Range(1, 101) <= ressource.dropChance)
            {
                GameObject instantiatedRessource = Instantiate(ressource.itemData.prefab);
                instantiatedRessource.transform.position = currentlyHarvesting.transform.position + spawnItemOffset;
            }
        }

        Destroy(currentlyHarvesting.gameObject);
    }

    public void AddItemToInventory()
    {
        inventory.AddItem(currentItem.itemdata);
        audioSource.PlayOneShot(pickupSound);

        // Update the UI text with the WhatObject value and start the coroutine to hide it
        pickedUpItemText.text = currentItem.itemdata.name;  // Correctly use WhatObject
        pickedUpItemImage.sprite = currentItem.itemdata.visual;  // Assign the item's visual to the UI Image
        pickedUpItemText.gameObject.SetActive(true);  // Show the text
        pickedUpItemImage.gameObject.SetActive(true);  // Show the image
        StartCoroutine(HidePickedUpItemText());

        Destroy(currentItem.gameObject);
    }

    private IEnumerator HidePickedUpItemText()
    {
        yield return new WaitForSeconds(2f);
        pickedUpItemText.gameObject.SetActive(false);
        pickedUpItemImage.gameObject.SetActive(false); // Hide the image after 2 seconds
    }

    public void ReEnablePlayerMovement()
    {
        EnableToolGameObjectFromEnum(currentTool, false);
        playerMoveBehaviour.canMove = true;
        isBusy = false;
    }

    private void EnableToolGameObjectFromEnum(Tool toolType, bool enabled = true)
    {
        switch (toolType)
        {
            case Tool.Pickaxe:
                pickaxeVisual.SetActive(enabled);
                audioSource.clip = pickaxeSound;
                break;
            case Tool.Axe:
                axeVisual.SetActive(enabled);
                audioSource.clip = axeSound;
                break;
        }
    }

    public void PlayHarvestingSoundEffect()
    {
        audioSource.Play();
    }
}
