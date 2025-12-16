using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuFocusController : MonoBehaviour, IPointerMoveHandler
{
    private Selectable _lastSelectable;

    private void OnEnable()
    {
        _lastSelectable = null;
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        var hovered = eventData.pointerCurrentRaycast.gameObject;
        if (hovered == null) return;

        var selectable = hovered.GetComponentInParent<Selectable>();
        if (selectable == null) return;

        var currentSelected = EventSystem.current.currentSelectedGameObject;
        if (currentSelected == selectable.gameObject) return;

        _lastSelectable = selectable;
        EventSystem.current.SetSelectedGameObject(selectable.gameObject);

    }
}