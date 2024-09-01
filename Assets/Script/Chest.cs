using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public ItemData[] items;  // Les objets que ce coffre va contenir.
    public float dropForce = 2f;  // Force à laquelle les objets seront éjectés du coffre.
    public float dropOffset = 1f;  // Distance à laquelle les objets seront placés devant le coffre.

    private bool isOpen = false;

    public void OpenChest()
    {
        if (isOpen) return;

        isOpen = true;

        foreach (ItemData item in items)
        {
            // Instancie l'objet devant le coffre et lui applique une force pour le faire tomber au sol.
            Vector3 dropPosition = transform.position + transform.forward * dropOffset + Vector3.up;
            GameObject itemInstance = Instantiate(item.prefab, dropPosition, Quaternion.identity);
            Rigidbody rb = itemInstance.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce((transform.forward + Vector3.up) * dropForce, ForceMode.Impulse);
            }
        }

        // Le coffre reste interactif, mais ne peut plus être ouvert.
        // Vous pouvez changer l'apparence du coffre ici si nécessaire.
    }

    public bool IsOpen()
    {
        return isOpen;
    }
}
