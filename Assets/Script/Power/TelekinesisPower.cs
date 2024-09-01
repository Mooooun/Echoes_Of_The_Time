using UnityEngine;
using UnityEngine.UI;

public class TelekinesisPower : MonoBehaviour
{
    public Camera playerCam;
    public float moveSpeed = 5f;
    public float distanceChangeSpeed = 5f;
    public float minDistance = 1f;
    public float maxDistance = 10f;
    public LayerMask objectLayer;
    public LineRenderer lineRenderer; // Référence au LineRenderer
    public Transform startPoint; // GameObject de départ du LineRenderer

    public float cooldownDuration = 2f; // Temps de cooldown en secondes
    private float cooldownTimer = 0f;
    public Image cooldownImage; // Référence à l'image UI qui représente le cooldown

    private Transform objectHit;
    private bool isObjectHeld = false;
    private float initialDistance;
    private float currentDistance;

    void Start()
    {
        lineRenderer.enabled = false;
        if (cooldownImage != null)
        {
            cooldownImage.fillAmount = 0f; // Assurez-vous que l'image est vide au départ
            cooldownImage.gameObject.SetActive(false); // Masquer l'image au départ
        }
    }

    void Update()
    {
        HandleCooldown();
        HandleObjectDetection();
        HandleObjectMovement();
        HandleDistanceChange();
        UpdateLineRenderer();
        UpdateCooldownUI(); // Mise à jour du visuel UI du cooldown
    }

    private void HandleCooldown()
    {
        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
            cooldownTimer = Mathf.Max(cooldownTimer, 0f);
            if (!cooldownImage.gameObject.activeSelf) // Si l'image n'est pas déjà visible
            {
                cooldownImage.gameObject.SetActive(true); // Afficher l'image UI
            }
        }
        else
        {
            if (cooldownImage.gameObject.activeSelf) // Si l'image est visible
            {
                cooldownImage.gameObject.SetActive(false); // Cacher l'image UI
            }
        }
    }

    private void HandleObjectDetection()
    {
        if (!isObjectHeld)
        {
            RaycastHit hit;
            if (Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out hit, Mathf.Infinity, objectLayer))
            {
                objectHit = hit.transform;
            }
            else
            {
                objectHit = null;
            }
        }
    }

    private void HandleObjectMovement()
    {
        if (objectHit != null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                StartHoldingObject();
            }

            if (isObjectHeld)
            {
                Vector3 targetPosition = playerCam.transform.position + playerCam.transform.forward * currentDistance;
                objectHit.position = Vector3.Lerp(objectHit.position, targetPosition, moveSpeed * Time.deltaTime);
            }

            if (Input.GetMouseButtonUp(0))
            {
                ReleaseObject();
            }
        }
    }

    private void HandleDistanceChange()
    {
        if (isObjectHeld)
        {
            if (Input.GetKey(KeyCode.O))
            {
                currentDistance += distanceChangeSpeed * Time.deltaTime;
                currentDistance = Mathf.Clamp(currentDistance, minDistance, maxDistance);
            }

            if (Input.GetKey(KeyCode.L))
            {
                currentDistance -= distanceChangeSpeed * Time.deltaTime;
                currentDistance = Mathf.Clamp(currentDistance, minDistance, maxDistance);
            }
        }
    }

    private void StartHoldingObject()
    {
        if (cooldownTimer > 0f) return; // Si le cooldown n'est pas terminé, ne rien faire

        isObjectHeld = true;
        Rigidbody rb = objectHit.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
        }

        initialDistance = Vector3.Distance(playerCam.transform.position, objectHit.position);
        currentDistance = initialDistance;

        lineRenderer.enabled = true;
    }

    private void ReleaseObject()
    {
        if (isObjectHeld)
        {
            Rigidbody rb = objectHit.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
            }
            isObjectHeld = false;
            objectHit = null;

            lineRenderer.enabled = false;

            // Commence le cooldown
            cooldownTimer = cooldownDuration;
        }
    }

    private void UpdateLineRenderer()
    {
        if (objectHit != null && isObjectHeld)
        {
            // Définir la position de départ du LineRenderer (point d'origine)
            lineRenderer.SetPosition(0, startPoint.position);

            // Définir la position d'arrivée du LineRenderer (point d'impact)
            lineRenderer.SetPosition(1, objectHit.position);
        }
        else
        {
            // Assurez-vous que la ligne est désactivée si aucun objet n'est tenu
            lineRenderer.enabled = false;
        }
    }

    private void UpdateCooldownUI()
    {
        if (cooldownImage != null)
        {
            cooldownImage.fillAmount = 1f - (cooldownTimer / cooldownDuration);
        }
    }

    // Appelé lorsque le script est désactivé
    private void OnDisable()
    {
        if (cooldownImage != null)
        {
            cooldownImage.gameObject.SetActive(false); // Cacher l'image UI
        }
        // Le cooldownTimer ne doit pas être réinitialisé ici pour permettre au cooldown de reprendre correctement
    }

    // Appelé lorsque le script est activé
    private void OnEnable()
    {
        if (cooldownImage != null)
        {
            cooldownImage.gameObject.SetActive(true); // Réactiver l'image UI si nécessaire
        }
    }
}
