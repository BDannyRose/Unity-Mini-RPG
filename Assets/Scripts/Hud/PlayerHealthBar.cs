using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    protected EventBus eventBus;

    [SerializeField] private TMP_Text healthText;
    private Slider healthSlider;

    private void Start()
    {
        eventBus = ServiceLocator.Current.Get<EventBus>();

        healthSlider = GetComponent<Slider>();

        eventBus.Subscribe<E_CharacterHealthChanged>(UpdateHealthbar);
    }

    private void OnDestroy()
    {
        eventBus.Unsubscribe<E_CharacterHealthChanged>(UpdateHealthbar);
    }

    private void UpdateHealthbar(E_CharacterHealthChanged signal)
    {
        if (signal.damageable.GetComponent<PlayerController>() != null)
        {
            healthSlider.value = signal.damageable.Health / signal.damageable.MaxHealth;
            healthText.text = $"Health: {signal.damageable.Health} / {signal.damageable.MaxHealth}";
        }
    }

}
