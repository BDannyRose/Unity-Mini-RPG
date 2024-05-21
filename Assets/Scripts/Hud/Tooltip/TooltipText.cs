using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipText : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string text;

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        Tooltip.ShowTooltip_Static(() => text + "\n<color=#ff0000>Testing color!</color>");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Tooltip.HideTooltip_Static();
    }
}
