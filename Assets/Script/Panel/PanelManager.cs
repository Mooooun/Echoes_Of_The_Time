using UnityEngine;

public class PanelManager : MonoBehaviour
{
    public GameObject mapPanel;
    public GameObject questPanel;
    public GameObject inventoryPanel;
    public GameObject optionPanel;

    void Start()
    {
        // Assurez-vous que tous les panels sont désactivés au début
        CloseAllPanels();
    }

    // Fonction pour fermer tous les panels
    private void CloseAllPanels()
    {
        mapPanel.SetActive(false);
        questPanel.SetActive(false);
        inventoryPanel.SetActive(false);
        optionPanel.SetActive(false);
    }

    // Appelée lorsque le bouton "Map" est cliqué
    public void OpenMapPanel()
    {
        CloseAllPanels();
        mapPanel.SetActive(true);
    }

    // Appelée lorsque le bouton "Quest" est cliqué
    public void OpenQuestPanel()
    {
        CloseAllPanels();
        questPanel.SetActive(true);
    }

    // Appelée lorsque le bouton "Inventory" est cliqué
    public void OpenInventoryPanel()
    {
        CloseAllPanels();
        inventoryPanel.SetActive(true);
    }

    // Appelée lorsque le bouton "Option" est cliqué
    public void OpenOptionPanel()
    {
        CloseAllPanels();
        optionPanel.SetActive(true);
    }
}
