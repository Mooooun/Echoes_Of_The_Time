using UnityEngine;

public class GliderController : MonoBehaviour
{
    [Header("Gliding Settings")]
    public float glideForwardSpeed = 10f;
    public float glideGravityScale = 0.5f;
    public float minGlideHeight = 5f;

    [Header("Glider Model")]
    public GameObject gliderModel; // Le modèle 3D du planeur

    private Rigidbody rb;
    private bool isGliding = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Assurez-vous que le modèle est désactivé au départ
        if (gliderModel != null)
        {
            gliderModel.SetActive(false);
        }
    }

    void Update()
    {
        // Vérifiez la distance au terrain en utilisant un raycast
        if (Input.GetKeyDown(KeyCode.Space) && IsAboveMinHeight() && !isGliding)
        {
            StartGliding();
        }
        else if (Input.GetKeyUp(KeyCode.Space) && isGliding)
        {
            StopGliding();
        }
    }

    bool IsAboveMinHeight()
    {
        // Lancer un raycast vers le bas pour mesurer la distance au terrain
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit))
        {
            return hit.distance > minGlideHeight;
        }
        // Si le raycast ne touche rien (le joueur est au-dessus du vide), considérer qu'il est au-dessus de la hauteur minimale
        return true;
    }

    void StartGliding()
    {
        // Oriente le personnage pour qu'il regarde dans la même direction que la caméra
        transform.forward = Camera.main.transform.forward;

        isGliding = true;
        rb.useGravity = false;

        // Affiche le modèle 3D du planeur
        if (gliderModel != null)
        {
            gliderModel.SetActive(true);
        }

        GlideMovement();
    }

    void StopGliding()
    {
        isGliding = false;
        rb.useGravity = true;

        // Masque le modèle 3D du planeur
        if (gliderModel != null)
        {
            gliderModel.SetActive(false);
        }
    }

    void GlideMovement()
    {
        // Direction de planage basée sur la direction de regard du personnage
        Vector3 forwardMovement = transform.forward * glideForwardSpeed * Time.deltaTime;
        // Mouvement descendant avec une gravité réduite
        Vector3 downwardMovement = Physics.gravity * glideGravityScale * Time.deltaTime;

        // Appliquer le mouvement combiné
        rb.velocity = forwardMovement + downwardMovement;
    }

    void FixedUpdate()
    {
        if (isGliding)
        {
            GlideMovement();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Lorsque le joueur touche le sol, désactive le planeur
        if (collision.gameObject.CompareTag("Ground"))
        {
            StopGliding();
        }
    }
}
