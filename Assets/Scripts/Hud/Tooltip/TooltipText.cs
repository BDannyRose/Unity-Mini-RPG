using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipText : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string text;

    private Tooltip tooltip;

    private void Start()
    {
        tooltip = ServiceLocator.Current.Get<Tooltip>();
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        tooltip.ShowTooltip(() => text + "\n<color=#ff0000>Testing color!</color>");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltip.HideTooltip();
    }
}
