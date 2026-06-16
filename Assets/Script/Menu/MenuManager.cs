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
    [SerializeField] private GameObject buttonOptionsFirst; // Ej: Botón Continuar/Volver
    [SerializeField] private GameObject sliderAudioFirst;    // Ej: Slider Master Volume
    [SerializeField] private GameObject sliderVideoFirst;    // Ej: Slider Brillo
    [SerializeField] private GameObject buttonControlFirst;  // Ej: Botón Volver de Controles
    [SerializeField] private GameObject buttonAreUSureFirst;
    [SerializeField] private GameObject buttonCreditsFirst;

    [Header("Control Global del Menú")]
    [SerializeField] private GameObject menuRootObject;

    public bool isMenuOpen=false;




    // --- FUNCIÓN CRÍTICA: ABRE O CIERRA TODO ---
    // --- FUNCIÓN CRÍTICA: ABRE O CIERRA TODO ---
    public void ToggleMenu()
    {
        isMenuOpen = !isMenuOpen;

        if (isMenuOpen)
        {
            // 1. Activamos el objeto raíz del menú (Canvas_EntreScenes_def)
            if (menuRootObject != null)
                menuRootObject.SetActive(true);

            // 2. SOLUCIÓN COMPLETA: Cada vez que se abra el menú (isMenuOpen es true),
            // llamamos directamente a la función. Esto apagará cualquier subpanel residual (Audio, Video...)
            // y reactivará el panel de Opciones con su botón del mando enfocado.
            OpenOptionsPanel();
        }
        else
        {
            // Al cerrar el menú con ESC, simplemente desactivamos la raíz por completo
            if (menuRootObject != null)
                menuRootObject.SetActive(false);

            // Limpiamos la selección del EventSystem para evitar errores de navegación con mando/teclado
            if (UnityEngine.EventSystems.EventSystem.current != null)
            {
                UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
            }
        }
    }

    // --- PANEL OPCIONES PRINCIPAL ---
    public void OpenOptionsPanel()
    {
        SetAllPanelsActive(false);
        panelOptions.SetActive(true);
        SelectElement(buttonOptionsFirst);
    }

    // --- PANEL AUDIO ---
    public void OpenAudioPanel()
    {
        SetAllPanelsActive(false);
        panelAudio.SetActive(true);
        SelectElement(sliderAudioFirst);
    }

    // --- PANEL VIDEO / BRILLO ---
    public void OpenVideoPanel()
    {
        SetAllPanelsActive(false);
        panelVideo.SetActive(true);
        SelectElement(sliderVideoFirst); // Selecciona el Slider de Brillo automáticamente
    }

    // --- PANEL CONTROLES ---
    public void OpenControlPanel()
    {
        SetAllPanelsActive(false);
        panelControl.SetActive(true);
        SelectElement(buttonControlFirst); // Selecciona el botón 'Volver' para poder salir con mando
    }

    public void OpenAreUSurePanel()
    {
        SetAllPanelsActive(false);
        panelAreUSure.SetActive(true);
        SelectElement(buttonAreUSureFirst); // Selecciona el botón de confirmación por defecto
    }

    public void OpenCreditsPanel()
    {
        SetAllPanelsActive(false);
        panelCredits.SetActive(true);
        SelectElement(buttonCreditsFirst);
    }

    // Módulos de ayuda para simplificar el código
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