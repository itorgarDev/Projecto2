using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; // ¡Importante!

// Este script va EN CADA BOTÓN o elemento interactuable
public class UIFeedback : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerEnterHandler, IPointerExitHandler
{
    // Esta será la imagen del selector (ej: una flechita al lado del botón)
    [SerializeField] private GameObject selectionVisual;

    private void Start()
    {
        // Por seguridad, apagamos el selector al inicio
        Deselect();
    }

    // --- Métodos obligatorios de las Interfaces ---

    // Se llama cuando el mando SELECCIONA este objeto
    public void OnSelect(BaseEventData eventData)
    {
        Select();
    }

    // Se llama cuando el mando PASA a otro objeto
    public void OnDeselect(BaseEventData eventData)
    {
        Deselect();
    }

    // Lo mismo pero con el ratón por encima/fuera
    public void OnPointerEnter(PointerEventData eventData)
    {
        Select();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Opcional: Podrías apagarlo o dejarlo
        // Deselect(); 
    }

    // Módulos de ayuda
    private void Select()
    {
        if (selectionVisual != null) selectionVisual.SetActive(true);
        // Opcional: Reproducir un sonido de 'tick' de menú aquí
    }

    private void Deselect()
    {
        if (selectionVisual != null) selectionVisual.SetActive(false);
    }
}