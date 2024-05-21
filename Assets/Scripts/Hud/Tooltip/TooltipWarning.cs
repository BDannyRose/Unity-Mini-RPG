using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class TooltipWarning : MonoBehaviour
{
    public static TooltipWarning Instance;

    [SerializeField]
    private RectTransform canvasRectTransform;
    private float showTimer;
    private float flashTimer;
    private int flashState;

    private RectTransform tooltipRectTransform;
    private TextMeshProUGUI tooltipText;
    private RectTransform backgroundRectTransform;
    private Image backgroundImage;
    private System.Func<string> getTooltipStringFunc;

    private void Awake()
    {
        Instance = this;

        tooltipText = transform.Find("Text").GetComponent<TextMeshProUGUI>();
        backgroundRectTransform = transform.Find("Background").GetComponent<RectTransform>();
        backgroundImage = transform.Find("Background").GetComponent<Image>();
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

        flashTimer += Time.deltaTime;
        float flashTimerMax = 0.02f;
        if (flashTimer > flashTimerMax)
        {
            flashState++;
            switch(flashState)
            {
                case 1:
                case 3:
                case 5:
                    tooltipText.color = Color.black;
                    backgroundImage.color = Color.red;
                    flashTimer = 0;
                    break;
                case 2:
                case 4:
                case 6:
                    tooltipText.color = Color.red;
                    backgroundImage.color = Color.black;
                    flashTimer = 0;
                    break;
            }
        }

        showTimer -= Time.deltaTime;
        if (showTimer <= 0f)
        {
            HideTooltip();
        }
    }

    private void ShowTooltip(string tooltipString, float showTimerMax = 2f)
    {
        ShowTooltip(() => tooltipString, showTimerMax);
    }

    private void ShowTooltip(System.Func<string> getTooltipStringFunc, float showTimerMax = 2f)
    {
        gameObject.SetActive(true);
        // To make the tooltip always be on top of other UI elements
        transform.SetAsLastSibling();
        this.getTooltipStringFunc = getTooltipStringFunc;
        showTimer = showTimerMax;
        flashTimer = 0;
        flashState = 0;
        SetText(getTooltipStringFunc()); 
    }

    private void SetText(string tooltipString)
    {
        tooltipText.text = tooltipString;
        tooltipText.ForceMeshUpdate();

        Vector2 textSize = tooltipText.GetRenderedValues(false);
        Vector2 paddingSize = new Vector2(8f, 8f);
        
        backgroundRectTransform.sizeDelta = textSize + paddingSize;
    }

    private void HideTooltip()
    {
        gameObject.SetActive(false);
    }

    public static void ShowTooltip_Static(System.Func<string> getTooltipStringFunc, float showTimerMax = 2f)
    {
        Instance.ShowTooltip(getTooltipStringFunc);
    }

    public static void HideTooltip_Static()
    {
        Instance.HideTooltip();
    }

}
