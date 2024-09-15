using UnityEngine;

public class InspectionObject : MonoBehaviour
{
    public GameObject inventoryPanel;
    public GameObject objectToInspect;

    public float rotationSpeed = 100f;

    // Ajout des limites pour la rotation en X et Y
    public float minRotationX = -45f;  // Limite minimale de rotation sur l'axe X
    public float maxRotationX = 45f;   // Limite maximale de rotation sur l'axe X


    private Vector3 previousMousePosition;
    private float currentRotationX = 0f;  // Pour stocker la rotation actuelle sur X
    private float currentRotationY = 0f;  // Pour stocker la rotation actuelle sur Y
    private void Start()
    {
        Quaternion rotation = Quaternion.Euler(-0.1f, 71f, 9.4f);
        objectToInspect.transform.rotation = rotation;

    }

    void Update()
    {
        objectToInspect.SetActive(inventoryPanel.activeSelf);

        
            Vector3 deltaMousePosition = Input.mousePosition - previousMousePosition;
            float deltaRotationX = deltaMousePosition.y * rotationSpeed * Time.deltaTime;
            float deltaRotationY = -deltaMousePosition.x * rotationSpeed * Time.deltaTime;

            // Applique le delta à la rotation actuelle
            currentRotationX += deltaRotationX;
            currentRotationY += deltaRotationY;

            // Limite les rotations sur les axes X et Y avec Mathf.Clamp
            currentRotationX = Mathf.Clamp(currentRotationX, minRotationX, maxRotationX);

            // Applique les nouvelles rotations à l'objet à inspecter
            Quaternion rotation = Quaternion.Euler(currentRotationX, currentRotationY, 0);
            objectToInspect.transform.rotation = rotation;
        

        previousMousePosition = Input.mousePosition;
    }
}
