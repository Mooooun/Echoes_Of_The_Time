using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChestInteraction : MonoBehaviour
{
    public float detectionRadius = 3f;  // Rayon de détection autour du joueur
    public LayerMask chestLayer;  // Le Layer assigné aux coffres
    public Text interactionText;  // Texte UI pour afficher "Appuyez sur E pour ouvrir"
    
    private Chest currentChest;

    void Update()
    {
        // Utiliser Physics.OverlapSphere pour détecter les objets dans le rayon de détection
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius, chestLayer);

        // Variable pour suivre si un coffre est détecté
        bool chestDetected = false;

        foreach (Collider hitCollider in hitColliders)
        {
            Chest chest = hitCollider.GetComponent<Chest>();

            if (chest != null)
            {
                currentChest = chest;
                chestDetected = true;

                if (currentChest.IsOpen())
                {
                    // Cacher le texte UI si le coffre est déjà ouvert
                    interactionText.gameObject.SetActive(false);
                }
                else
                {
                    interactionText.gameObject.SetActive(true);  // Afficher le texte UI

                    // Si la touche "E" est pressée, ouvrir le coffre
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        chest.OpenChest();
                        interactionText.gameObject.SetActive(false);  // Cacher le texte UI après l'ouverture
                    }
                }
                break;  // Arrêter de vérifier les autres colliders une fois qu'un coffre est trouvé
            }
        }

        if (!chestDetected)
        {
            // Cacher le texte UI si aucun coffre n'est détecté
            interactionText.gameObject.SetActive(false);
            currentChest = null;
        }
    }

    // Pour visualiser la sphère de détection dans l'éditeur Unity (optionnel)
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
