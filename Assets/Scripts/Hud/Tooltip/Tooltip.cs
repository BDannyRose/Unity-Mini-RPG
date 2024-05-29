using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour, IService
{
    [SerializeField]
    private RectTransform canvasRectTransform;

    private RectTransform tooltipRectTransform;
    private TextMeshProUGUI tooltipText;
    private RectTransform backgroundRectTransform;
    private System.Func<string> getTooltipStringFunc;

    private void Awake()
    {
        tooltipText = transform.Find("Text").GetComponent<TextMeshProUGUI>();
        backgroundRectTransform = transform.Find("Background").GetComponent<RectTransform>();
        tooltipRectTransform = GetComponent<RectTransform>();

        HideTooltip();
    }

    private void Update()
    {
        SetText(getTooltipStringFunc());

        // To make the tooltip work with scalable interfaces
        tooltipRectTransform.anchoredPosition = Input.mousePosition / canvasRectTransform.localScale.x;
        // Position of the tooltip relative to its anchor (should be anchored to the lower left corner of the canvas)
        Vector2 anchoredPosition = tooltipRectTransform.anchoredPosition;
        // Since the background is anchored to the lower left corner of the Tooltip object,
        // anchoredPosition.x + backgroundWidth is the rightmost point of the tooltip background
        // so if it's bigger than the width of the canvas, then the tooltip is outside the screen
        if (anchoredPosition.x + backgroundRectTransform.rect.width > canvasRectTransform.rect.width)
        {
            // thus deducting background width from canvas width gives us the righmost point for tooltip
            anchoredPosition.x = canvasRectTransform.rect.width - backgroundRectTransform.rect.width;
        }
        if (anchoredPosition.y + backgroundRectTransform.rect.height > canvasRectTransform.rect.height)
        {
            anchoredPosition.y = canvasRectTransform.rect.height - backgroundRectTransform.rect.height;
        }
        tooltipRectTransform.anchoredPosition = anchoredPosition;
    }

    public void ShowTooltip(System.Func<string> getTooltipStringFunc)
    {
        gameObject.SetActive(true);
        // To make the tooltip always be on top of other UI elements
        transform.SetAsLastSibling();
        this.getTooltipStringFunc = getTooltipStringFunc;
        SetText(getTooltipStringFunc()); 
    }

    public void HideTooltip()
    {
        gameObject.SetActive(false);
    }

    private void SetText(string tooltipString)
    {
        tooltipText.text = tooltipString;
        tooltipText.ForceMeshUpdate();

        Vector2 textSize = tooltipText.GetRenderedValues(false);
        Vector2 paddingSize = new Vector2(8f, 8f);

        backgroundRectTransform.sizeDelta = textSize + paddingSize;
    }
}
