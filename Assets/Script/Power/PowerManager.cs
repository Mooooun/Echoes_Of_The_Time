using UnityEngine;
using UnityEngine.UI;

public class ManagerPower : MonoBehaviour
{
    public GameObject selectionWheel;

    [Header("Grappin")]
    public Button powerButton1;
    public MonoBehaviour powerScript1;
    public Image powerImage1; // Image associated with Grappin

    [Header("Telekinesis")]
    public Button powerButton2;
    public MonoBehaviour powerScript2;
    public Image powerImage2; // Image associated with Telekinesis

    [Header("Echo")]
    public Button powerButton3;
    public MonoBehaviour powerScript3;
    public Image powerImage3; // Image associated with Echo

    [Header("Rewind")]
    public Button powerButton4;
    public MonoBehaviour powerScript4;
    public Image powerImage4; // Image associated with Rewind

    private MonoBehaviour selectedPower;
    private Image selectedImage;

    void Start()
    {
        selectionWheel.SetActive(false);

        // Initialize powers as disabled
        powerScript1.enabled = false;
        powerScript2.enabled = false;
        powerScript3.enabled = false;
        powerScript4.enabled = false;

        // Initialize images as disabled
        powerImage1.gameObject.SetActive(false);
        powerImage2.gameObject.SetActive(false);
        powerImage3.gameObject.SetActive(false);
        powerImage4.gameObject.SetActive(false);

        powerButton1.onClick.AddListener(() => SelectPower(powerScript1, powerImage1));
        powerButton2.onClick.AddListener(() => SelectPower(powerScript2, powerImage2));
        powerButton3.onClick.AddListener(() => SelectPower(powerScript3, powerImage3));
        powerButton4.onClick.AddListener(() => SelectPower(powerScript4, powerImage4));
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.B))
        {
            selectionWheel.SetActive(true);
        }
        else if (Input.GetKeyUp(KeyCode.B))
        {
            selectionWheel.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            if (selectedPower != null)
            {
                // Toggle the selected power
                selectedPower.enabled = !selectedPower.enabled;
            }
        }
    }

    void SelectPower(MonoBehaviour powerScript, Image powerImage)
    {
        // Disable the currently selected power and its image
        if (selectedPower != null && selectedPower != powerScript)
        {
            selectedPower.enabled = false;
        }

        if (selectedImage != null && selectedImage != powerImage)
        {
            selectedImage.gameObject.SetActive(false);
        }

        // Select the new power and activate its image
        selectedPower = powerScript;
        selectedImage = powerImage;

        if (selectedPower != null)
        {
            selectedPower.enabled = true;
        }

        if (selectedImage != null)
        {
            selectedImage.gameObject.SetActive(true);
        }
    }
}
