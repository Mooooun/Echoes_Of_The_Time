using UnityEngine;

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

    void Update()
    {
        // Gestion des entrées pour ouvrir ou fermer les menus
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (optionsMenu.activeSelf || inventoryMenu.activeSelf || mapMenu.activeSelf || questMenu.activeSelf)
            {
                // Fermer tous les menus ouverts si Échap ou Tab est pressé
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
            if (mapMenu.activeSelf)
            {
                // Fermer tous les menus ouverts si Échap ou Tab est pressé
                CloseAllMenus();
            }
            else
            {

            }
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (questMenu.activeSelf)
            {
                // Fermer tous les menus ouverts si Échap ou Tab est pressé
                CloseAllMenus();
            }
            else
            {

            }
        }

        // Désactiver la caméra si un menu est ouvert
        CheckMenuStatus();
    }

    private void OpenOptionsMenu()
    {
        optionsMenu.SetActive(true);
        Time.timeScale = 0;
        // Note: La variable isMenuOpened a été supprimée, aucune action n'est nécessaire ici.
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
        // Note: La variable isMenuOpened a été supprimée, aucune action n'est nécessaire ici.
    }

    private void CheckMenuStatus()
    {
        // Activer/désactiver la caméra en fonction de l'état des menus
        cameraScript.enabled = !(optionsMenu.activeSelf || inventoryMenu.activeSelf || mapMenu.activeSelf || questMenu.activeSelf);
    }
}
