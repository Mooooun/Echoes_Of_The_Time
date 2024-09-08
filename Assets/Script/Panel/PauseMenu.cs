using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject optionsMenu;

    [SerializeField]
    private GameObject inventoryMenu;

    [SerializeField]
    private GameObject mapMenu;

    [SerializeField]
    private GameObject questMenu;

    [SerializeField]
    private ThirdPersonOrbitCamBasic cameraScript;

    [SerializeField]
    private MoveBehaviour playerMovementScript; // Mise à jour pour utiliser MoveBehaviour

    [SerializeField]
    private Button closeOptionsButton;

    [SerializeField]
    private Button closeInventoryButton;

    [SerializeField]
    private Button closeMapButton;

    [SerializeField]
    private Button closeQuestButton;

    void Start()
    {
        // Assurez-vous que les boutons de fermeture sont liés aux méthodes
        if (closeOptionsButton != null)
        {
            closeOptionsButton.onClick.AddListener(() => CloseMenu(optionsMenu));
        }
        if (closeInventoryButton != null)
        {
            closeInventoryButton.onClick.AddListener(() => CloseMenu(inventoryMenu));
        }
        if (closeMapButton != null)
        {
            closeMapButton.onClick.AddListener(() => CloseMenu(mapMenu));
        }
        if (closeQuestButton != null)
        {
            closeQuestButton.onClick.AddListener(() => CloseMenu(questMenu));
        }
    }

    void Update()
    {
        // Gestion des entrées pour ouvrir ou fermer les menus
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (optionsMenu.activeSelf || inventoryMenu.activeSelf || mapMenu.activeSelf || questMenu.activeSelf)
            {
                // Fermer tous les menus ouverts si Échap est pressé
                CloseAllMenus();
            }
            else
            {
                // Ouvrir le menu de pause si aucun autre menu n'est ouvert
                OpenOptionsMenu();
            }
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (mapMenu.activeSelf || questMenu.activeSelf)
            {
                // Fermer tous les menus ouverts si Tab est pressé
                CloseAllMenus();
            }
            else
            {
                // Cette partie est à compléter selon les besoins spécifiques pour Tab
            }
        }

        // Désactiver la caméra si un menu est ouvert
        CheckMenuStatus();
    }

    private void OpenOptionsMenu()
    {
        optionsMenu.SetActive(true);
        Time.timeScale = 0;
        if (playerMovementScript != null)
        {
            playerMovementScript.enabled = false; // Désactiver le mouvement du joueur
        }
    }

    private void CloseAllMenus()
    {
        // Fermer tous les menus
        optionsMenu.SetActive(false);
        inventoryMenu.SetActive(false);
        mapMenu.SetActive(false);
        questMenu.SetActive(false);

        // Réactiver le jeu
        Time.timeScale = 1;
        if (playerMovementScript != null)
        {
            playerMovementScript.enabled = true; // Réactiver le mouvement du joueur
        }
    }

    private void CloseMenu(GameObject menu)
    {
        if (menu != null)
        {
            menu.SetActive(false);
        }

        // Réactiver le jeu et le mouvement du joueur si tous les menus sont fermés
        if (!optionsMenu.activeSelf && !inventoryMenu.activeSelf && !mapMenu.activeSelf && !questMenu.activeSelf)
        {
            Time.timeScale = 1;
            if (playerMovementScript != null)
            {
                playerMovementScript.enabled = true; // Réactiver le mouvement du joueur
            }
        }
    }

    private void CheckMenuStatus()
    {
        // Activer/désactiver la caméra en fonction de l'état des menus
        if (cameraScript != null)
        {
            cameraScript.enabled = !(optionsMenu.activeSelf || inventoryMenu.activeSelf || mapMenu.activeSelf || questMenu.activeSelf);
        }
    }
}
