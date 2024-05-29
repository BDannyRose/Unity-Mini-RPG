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

    private void OnEnable()
    {
        eventBus = ServiceLocator.Current.Get<EventBus>();
        eventBus.Subscribe<E_CharacterHealthChanged>(CharacterHealthChanged);
    }

    private void OnDisable()
    {
        eventBus.Unsubscribe<E_CharacterHealthChanged>(CharacterHealthChanged);
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
