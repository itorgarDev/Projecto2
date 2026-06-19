using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuManager : MonoBehaviour
{
    [Header("Paneles del Menú")]
    [SerializeField] private GameObject panelOptions;
    [SerializeField] private GameObject panelAudio;
    [SerializeField] private GameObject panelVideo;
    [SerializeField] private GameObject panelControl;
    [SerializeField] private GameObject panelAreUSure;
    [SerializeField] private GameObject panelCredits;

    [Header("Primeros Elementos Seleccionados (Mando)")]
    [SerializeField] private GameObject buttonOptionsFirst; 
    [SerializeField] private GameObject sliderAudioFirst;    
    [SerializeField] private GameObject sliderVideoFirst;    
    [SerializeField] private GameObject buttonControlFirst;  
    [SerializeField] private GameObject buttonAreUSureFirst;
    [SerializeField] private GameObject buttonCreditsFirst;

    [Header("Control Global del Menú")]
    [SerializeField] private GameObject menuRootObject;

    public bool isMenuOpen=false;





    public void ToggleMenu()
    {
        isMenuOpen = !isMenuOpen;

        if (isMenuOpen)
        {
            
            if (menuRootObject != null)
                menuRootObject.SetActive(true);
            OpenOptionsPanel();
        }
        else
        {
            
            if (menuRootObject != null)
                menuRootObject.SetActive(false);

            
            if (UnityEngine.EventSystems.EventSystem.current != null)
            {
                UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
            }
        }
    }

    //al abrir el panel de opciones se detecta el primer boton del Canvas como el boton donde poder comenzar a desplazarte
    public void OpenOptionsPanel()
    {
        SetAllPanelsActive(false);
        panelOptions.SetActive(true);
        SelectElement(buttonOptionsFirst);
    }

    //al abrir el panel de audio se detecta el primer slider del Canvas como el slider donde poder comenzar a desplazarte
    public void OpenAudioPanel()
    {
        SetAllPanelsActive(false);
        panelAudio.SetActive(true);
        SelectElement(sliderAudioFirst);
    }

    //al abrir el panel de audio se detecta el primer slider del Canvas como el slider donde poder comenzar a desplazarte
    public void OpenVideoPanel()
    {
        SetAllPanelsActive(false);
        panelVideo.SetActive(true);
        SelectElement(sliderVideoFirst); 
    }

  
    public void OpenControlPanel()
    {
        SetAllPanelsActive(false);
        panelControl.SetActive(true);
        SelectElement(buttonControlFirst); 
    }

    public void OpenAreUSurePanel()
    {
        SetAllPanelsActive(false);
        panelAreUSure.SetActive(true);
        SelectElement(buttonAreUSureFirst);
    }

    public void OpenCreditsPanel()
    {
        SetAllPanelsActive(false);
        panelCredits.SetActive(true);
        SelectElement(buttonCreditsFirst);
    }

    private void SetAllPanelsActive(bool state)
    {
        if (panelOptions != null) panelOptions.SetActive(state);
        if (panelAudio != null) panelAudio.SetActive(state);
        if (panelVideo != null) panelVideo.SetActive(state);
        if (panelControl != null) panelControl.SetActive(state);
    }

    private void SelectElement(GameObject element)
    {
        if (element != null && EventSystem.current != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(element);
        }
    }
}