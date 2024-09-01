using UnityEngine;
using UnityEngine.UI;

public class GrapplinPower : MonoBehaviour
{
    public Camera cam;
    public RaycastHit hit;
    public LayerMask surfaces;
    public float maxDistance;
    public bool isMoving;
    public Vector3 location;
    public float speed = 10;
    public Transform hook;
    public MoveBehaviour FPC; // Assurez-vous que MoveBehaviour est le bon type
    public LineRenderer LR;

    private bool grapplingStarted = false;
    public float cooldownTime = 2.0f; // Temps de cooldown en secondes
    private float lastGrappleTime;

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
        
        // Envoi du grappin lors du relâchement du clic gauche de la souris, avec gestion du cooldown
        if (Input.GetMouseButtonUp(0) && !grapplingStarted && Time.time >= lastGrappleTime + cooldownTime)
        {
            Grapple();
            grapplingStarted = true;
        }

        // Si le personnage se déplace, on l'envoie vers le point d'arrivée 
        if (isMoving)
        {
            MoveToSpot();
        }

        // Annulation / décrochage du grappin
        if (Input.GetKey(KeyCode.Space) && isMoving)
        {
            StopGrapple();
        }
    }

    private void HandleCooldown()
    {
        if (Time.time < lastGrappleTime + cooldownTime)
        {
            if (cooldownImage != null)
            {
                // Mettre à jour la barre de cooldown
                cooldownImage.fillAmount = 1f - (Time.time - lastGrappleTime) / cooldownTime;
                if (!cooldownImage.gameObject.activeSelf) // Si l'image n'est pas déjà visible
                {
                    cooldownImage.gameObject.SetActive(true); // Afficher l'image UI
                }
            }
        }
        else
        {
            if (cooldownImage != null && cooldownImage.gameObject.activeSelf) // Si l'image est visible
            {
                cooldownImage.gameObject.SetActive(false); // Cacher l'image UI
            }
        }
    }

    // Lors de l'envoi du grappin
    public void Grapple()
    {
        // Création d'un raycast de "maxDistance" unités depuis la caméra vers l'avant.
        // Si ce raycast touche quelque chose, le grappin est utilisable
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, maxDistance, surfaces))
        {
            isMoving = true;
            location = hit.point;
            FPC.canMove = false;
            GetComponent<Rigidbody>().useGravity = false;
            LR.enabled = true;
            LR.SetPosition(1, location);
        }
    }

    // Arrêter le grappin (appelé lorsqu'on décroche le grappin ou qu'on atteint la cible)
    public void StopGrapple()
    {
        isMoving = false;
        FPC.canMove = true;
        LR.enabled = false;
        GetComponent<Rigidbody>().useGravity = true;
        grapplingStarted = false; // Réinitialiser la variable pour permettre un nouveau grappin
        lastGrappleTime = Time.time; // Enregistrer l'heure actuelle pour le cooldown
    }

    // Déplacement du joueur vers le point touché par le grappin
    public void MoveToSpot()
    {
        transform.position = Vector3.Lerp(transform.position, location, speed * Time.deltaTime / Vector3.Distance(transform.position, location));
        LR.SetPosition(0, hook.position);

        // Si on est à moins de 1 unité(s) de la cible finale, on décroche le grappin automatiquement
        if (Vector3.Distance(transform.position, location) < 1f)
        {
            StopGrapple();
        }
    }
}
