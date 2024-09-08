using UnityEngine;
using System.Linq; // Pour utiliser LINQ

public class BowAttackBehaviour : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private Animator animator;

    [SerializeField]
    private Equipment equipmentSystem;

    [SerializeField]
    private UIManager uiManager;

    [SerializeField]
    private InteractBehaviour interactBehaviour;

    [Header("Configuration")]
    private bool isCharging;

    [SerializeField]
    private LayerMask layerMask;

    [SerializeField]
    private Vector3 attackOffset;

    [SerializeField]
    private GameObject arrowPrefab;

    [SerializeField]
    private Transform arrowSpawnPoint;

    [SerializeField]
    private float arrowSpeed = 30f; // La vitesse de la flèche

    // Reference to the Inventory
    private Inventory inventory;

    void Start()
    {
        // Find the Inventory component in the scene
        inventory = Inventory.instance; // Assuming the inventory is a singleton
    }

    void Update()
    {
        if (Input.GetMouseButton(0) && CanAttack()) // Utiliser le clic gauche pour charger l'arc
        {
            if (!isCharging)
            {
                isCharging = true;
                animator.SetBool("IsCharging", true); // Activer l'animation de bandage de l'arc
            }
        }
        else if (Input.GetMouseButtonUp(0) && isCharging) // Utiliser le clic gauche pour tirer l'arc
        {
            ShootArrow();
            animator.SetBool("IsCharging", false); // Arrêter l'animation de bandage de l'arc
            isCharging = false;
        }
    }

    void ShootArrow()
    {
        if (inventory == null)
        {
            Debug.LogError("Inventory n'est pas initialisé !");
            return;
        }

        // Check if the player has arrows in the inventory
        ItemInInventory arrowItem = inventory.GetContent()
            .FirstOrDefault(item => item.itemData.name == "Arrow");

        if (arrowItem != null && arrowItem.count > 0)
        {
            GameObject arrowInstance = Instantiate(arrowPrefab, arrowSpawnPoint.position, arrowSpawnPoint.rotation);
            Rigidbody rb = arrowInstance.GetComponent<Rigidbody>();

            if (rb != null)
            {
                Vector3 shootDirection = arrowSpawnPoint.forward;
                rb.velocity = shootDirection * arrowSpeed;
                rb.freezeRotation = true;

                // Remove one arrow from inventory after shooting
                inventory.RemoveItem(arrowItem.itemData);
            }
            else
            {
                Debug.LogError("Aucun Rigidbody trouvé sur la flèche.");
            }
        }
        else
        {
            Debug.Log("Pas de flèches dans l'inventaire.");
        }
    }

    bool CanAttack()
    {
        return equipmentSystem.equipedWeaponItem != null &&
               equipmentSystem.equipedWeaponItem.itemType == ItemType.Equipment &&
               equipmentSystem.equipedWeaponItem.equipmentType == EquipmentType.Bow && // Vérifier si l'équipement est un arc
               !uiManager.atLeastOnePanelOpened &&
               !interactBehaviour.isBusy;
    }
}
