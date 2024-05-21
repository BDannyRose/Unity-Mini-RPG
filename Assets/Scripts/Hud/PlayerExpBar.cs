using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class PlayerExpBar : MonoBehaviour
{
    [SerializeField]
    private TMPro.TextMeshProUGUI levelText;
    private Slider expSlider;

    protected EventBus eventBus;


    private void Start()
    {
        eventBus = ServiceLocator.Current.Get<EventBus>();
        expSlider = GetComponent<Slider>();

        eventBus.Subscribe<E_OnExpChanged>(UpdateExpBar);
        eventBus.Subscribe<E_OnLevelChanged>(UpdateLevelText);
    }

    private void UpdateExpBar(E_OnExpChanged onExpChanged)
    {
        expSlider.value = onExpChanged.levelSystem.GetExpNormalized();
    }

    private void UpdateLevelText(E_OnLevelChanged onLevelChanged)
    {
        levelText.text = onLevelChanged.levelSystem.GetLevelNumber().ToString();
    }
}
