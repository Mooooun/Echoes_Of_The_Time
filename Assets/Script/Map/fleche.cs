using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fleche : MonoBehaviour
{
    [SerializeField] private Transform player;   // Référence au joueur
    [SerializeField] private Transform arrow;    // Référence à la flèche sur la minimap

    void Update()
    {
        // Mise à jour de la position de la minimap pour qu'elle suive le joueur
        Vector3 newPosition = player.position;
        newPosition.y = transform.position.y;
        transform.position = newPosition;

        // Mise à jour de l'orientation de la flèche pour qu'elle corresponde à celle du joueur
        Vector3 newRotation = new Vector3(0, 0, -player.eulerAngles.y);
        arrow.localEulerAngles = newRotation;
    }
}
