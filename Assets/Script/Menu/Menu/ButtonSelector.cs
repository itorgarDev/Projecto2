using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonSelector : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject iconoSeleccion;

    public void OnSelect(BaseEventData eventData)
    {
        if (iconoSeleccion != null)
            iconoSeleccion.SetActive(true);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        if (iconoSeleccion != null)
            iconoSeleccion.SetActive(false);
    }

    // 🔹 Para que funcione con el ratón
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (iconoSeleccion != null)
            iconoSeleccion.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (iconoSeleccion != null)
            iconoSeleccion.SetActive(false);
    }
}
