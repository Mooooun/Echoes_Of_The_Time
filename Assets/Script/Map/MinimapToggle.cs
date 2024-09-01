using UnityEngine;

public class MinimapToggle : MonoBehaviour
{
    public GameObject minimap;           // Référence à la minimap
    public GameObject[] panels;          // Tableau pour référencer tous les panels

    void Start()
    {
        // S'assurer que tous les panels sont fermés au début
        foreach (GameObject panel in panels)
        {
            panel.SetActive(false);
        }
    }

    void Update()
    {
        // Vérifie si un panel est ouvert
        bool anyPanelActive = false;
        foreach (GameObject panel in panels)
        {
            if (panel.activeSelf)
            {
                anyPanelActive = true;
                break;
            }
        }

        // Cache la minimap si un panel est actif, sinon montre la minimap
        minimap.SetActive(!anyPanelActive);
    }

    // Méthode pour ouvrir un panel spécifique (appelée lorsque vous ouvrez un panel)
    public void OpenPanel(int panelIndex)
    {
        // Ferme tous les panels
        foreach (GameObject panel in panels)
        {
            panel.SetActive(false);
        }

        // Ouvre le panel sélectionné et cache la minimap
        if (panelIndex >= 0 && panelIndex < panels.Length)
        {
            panels[panelIndex].SetActive(true);
        }
    }

    // Méthode pour fermer un panel spécifique (appelée lorsque vous fermez un panel)
    public void ClosePanel(int panelIndex)
    {
        if (panelIndex >= 0 && panelIndex < panels.Length)
        {
            panels[panelIndex].SetActive(false);
        }
    }
}
