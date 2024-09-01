using UnityEngine;
using UnityEngine.UI;

public class EchoPower : MonoBehaviour
{
    public GameObject spherePrefab; // Référence au prefab de la sphère
    public float sphereLifetime = 10f; // Durée de vie de la sphère
    public float cooldownTime = 5f; // Temps de cooldown entre les créations de sphères

    private GameObject activeSphere; // Référence à la sphère active
    private bool sphereExists = false; // Vérifie si la sphère existe
    private float timeSinceSphereCreated; // Temps écoulé depuis la création de la sphère
    private float timeSinceLastUse; // Temps écoulé depuis la dernière utilisation du pouvoir
    private bool isOnCooldown = false; // Indique si le pouvoir est en cooldown

    // Références pour le visuel UI du cooldown
    public Image cooldownImage; // Référence à l'image UI qui représente le cooldown

    void Start()
    {
        if (cooldownImage != null)
        {
            cooldownImage.fillAmount = 0f; // Assurez-vous que l'image est vide au départ
            cooldownImage.gameObject.SetActive(false); // Masquer l'image au départ
        }
    }

    void Update()
    {
        HandleCooldown();
        
        // Si on appuie sur "R" et que le cooldown est écoulé ou s'il s'agit de téléportation
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (!sphereExists)
            {
                if (!isOnCooldown)
                {
                    CreateSphere();
                }
            }
            else
            {
                TeleportToSphere();
                StartCooldown(); // Commencer le cooldown après la téléportation
            }
        }

        // Si la sphère existe, on met à jour le temps écoulé
        if (sphereExists)
        {
            timeSinceSphereCreated += Time.deltaTime;

            // Si le temps écoulé dépasse la durée de vie de la sphère, on téléporte le joueur
            if (timeSinceSphereCreated >= sphereLifetime)
            {
                TeleportToSphere();
                StartCooldown(); // Commencer le cooldown après la téléportation
            }
        }
    }

    void CreateSphere()
    {
        // Crée une sphère à la position actuelle du joueur
        activeSphere = Instantiate(spherePrefab, transform.position, Quaternion.identity);
        sphereExists = true;
        timeSinceSphereCreated = 0f;
    }

    void TeleportToSphere()
    {
        // Téléporte le joueur à la position de la sphère
        transform.position = activeSphere.transform.position;

        // Désactive la sphère
        Destroy(activeSphere);
        sphereExists = false;
    }

    void StartCooldown()
    {
        isOnCooldown = true;
        timeSinceLastUse = 0f;
    }

    private void HandleCooldown()
    {
        if (isOnCooldown)
        {
            timeSinceLastUse += Time.deltaTime;

            if (cooldownImage != null)
            {
                // Mettre à jour la barre de cooldown
                cooldownImage.fillAmount = 1f - (timeSinceLastUse / cooldownTime);
                if (!cooldownImage.gameObject.activeSelf) // Si l'image n'est pas déjà visible
                {
                    cooldownImage.gameObject.SetActive(true); // Afficher l'image UI
                }
            }

            if (timeSinceLastUse >= cooldownTime)
            {
                isOnCooldown = false;
                if (cooldownImage != null && cooldownImage.gameObject.activeSelf) // Si l'image est visible
                {
                    cooldownImage.gameObject.SetActive(false); // Cacher l'image UI
                }
            }
        }
    }
}
