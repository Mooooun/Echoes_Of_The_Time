using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour
{
    // Rayon de la sphère, modifiable via l'inspecteur Unity
    [SerializeField]
    private float interactRange = 2.6f;

    public InteractBehaviour playerInteractBehaviour;

    [SerializeField]
    private GameObject interactText;

    [SerializeField]
    private LayerMask layerMask;

    void Update()
    {
        // Utilisation de Physics.OverlapSphere pour détecter tous les objets dans un rayon autour du joueur
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, interactRange, layerMask);

        // Variable pour déterminer si un objet interactif a été trouvé
        bool isInteractable = false;

        foreach (Collider hitCollider in hitColliders)
        {
            // Calculer la direction de l'objet par rapport au joueur
            Vector3 directionToCollider = (hitCollider.transform.position - transform.position).normalized;

            // Vérifier si l'objet est dans la moitié avant (demi-sphère)
            if (Vector3.Dot(transform.forward, directionToCollider) > 0)
            {
                // Vérifiez si l'objet détecté est interactif (avec les tags "Item" ou "Harvestable")
                if (hitCollider.CompareTag("Item") || hitCollider.CompareTag("Harvestable"))
                {
                    // Affichez le texte d'interaction
                    interactText.SetActive(true);
                    isInteractable = true;

                    // Si le joueur appuie sur la touche d'interaction "E"
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        if (hitCollider.CompareTag("Item"))
                        {
                            playerInteractBehaviour.DoPickup(hitCollider.GetComponent<Item>());
                        }

                        if (hitCollider.CompareTag("Harvestable"))
                        {
                            playerInteractBehaviour.DoHarvest(hitCollider.GetComponent<Harvestable>());
                        }
                    }

                    // On sort de la boucle après avoir trouvé un objet interactif (optionnel)
                    break;
                }
            }
        }

        // Si aucun objet interactif n'a été trouvé, désactiver le texte d'interaction
        if (!isInteractable)
        {
            interactText.SetActive(false);
        }
    }

    // Pour visualiser la sphère dans l'éditeur Unity (optionnel)
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactRange);
    }
}
