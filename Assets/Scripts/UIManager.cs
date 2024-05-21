using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class UIManager : MonoBehaviour
{
    public GameObject damageTextPrefab;
    public GameObject healthTextPrefab;

    public Canvas gameCanvas;

    protected EventBus eventBus;

    private void Start()
    {
        eventBus = ServiceLocator.Current.Get<EventBus>();
        eventBus.Subscribe<E_CharacterHealthChanged>(CharacterHealthChanged);
    }

    private void OnDestroy()
    {
        eventBus.Unsubscribe<E_CharacterHealthChanged>(CharacterHealthChanged);
    }

    private void OnEnable()
    {
        //eventBus.Subscribe<E_CharacterHealthChanged>(CharacterHealthChanged);
        //Events.characterTookDamage += CharacterTookDamage;
        //Events.characterHealed += CharacterHealed;
    }

    private void OnDisable()
    {
        //eventBus.Unsubscribe<E_CharacterHealthChanged>(CharacterHealthChanged);
        //Events.characterTookDamage -= CharacterTookDamage;
        //Events.characterHealed -= CharacterHealed;
    }

    public void CharacterHealthChanged(E_CharacterHealthChanged signal)
    {
        if (signal.changeAmount == 0) return;

        Vector3 spawnPosition = Camera.main.WorldToScreenPoint(signal.damageable.transform.position + new Vector3(0, 1, 0));
        GameObject textPrefab = null;
        
        if (signal.changeAmount > 0)
        {
            textPrefab = healthTextPrefab;
        }
        else if (signal.changeAmount < 0)
        {
            textPrefab = damageTextPrefab;
        }

        TMP_Text tmpText = Instantiate(textPrefab, spawnPosition, Quaternion.identity, gameCanvas.transform).GetComponent<TMP_Text>();
        tmpText.text = signal.changeAmount.ToString();
    }
}
