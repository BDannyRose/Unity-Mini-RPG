using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class HudController : MonoBehaviour
{
    [SerializeField] private ProgressBar progressBar;
    [SerializeField] private CustomTrigger _healthBarTrigger;
    [SerializeField] private Image Bar;

    private void OnEnable()
    {
        _healthBarTrigger.EnteredTrigger.AddListener(showHealthBar);
        _healthBarTrigger.ExitedTrigger.AddListener(hideHealthBar);
    }

    private void OnDisable()
    {
        _healthBarTrigger.EnteredTrigger.RemoveListener(showHealthBar);
        _healthBarTrigger.ExitedTrigger.RemoveListener(hideHealthBar);
    }

    public void hideHealthBar()
    {
        Color newColor = Bar.color;
        newColor.a = 0f;
        Bar.color = newColor;
    }

    public void showHealthBar()
    {
        Color newColor = Bar.color;
        newColor.a = 255f;
        Bar.color = newColor;
    }

    public void UpdateHealthBar(float currentValue, float maxValue)
    {
        float healthPercentage = currentValue / maxValue;
        progressBar.SetProgress(healthPercentage);
    }
}
