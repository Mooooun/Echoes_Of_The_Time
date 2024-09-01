using UnityEngine;

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
        if (arrowPrefab != null && arrowSpawnPoint != null)
        {
            GameObject arrowInstance = Instantiate(arrowPrefab, arrowSpawnPoint.position, arrowSpawnPoint.rotation);
            Rigidbody rb = arrowInstance.GetComponent<Rigidbody>();

            if (rb != null)
            {
                Vector3 shootDirection = arrowSpawnPoint.forward;
                rb.velocity = shootDirection * arrowSpeed;
                rb.freezeRotation = true;
                Debug.Log("Flèche tirée avec une vitesse de : " + rb.velocity); // Message de débogage
            }
            else
            {
                Debug.LogError("Aucun Rigidbody trouvé sur la flèche.");
            }
        }
        else
        {
            Debug.LogError("Prefab de la flèche ou point de spawn non configuré.");
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
