using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillXEnemiesOfTypeY : QuestStep
{
    [SerializeField] private EnemyTypes enemyType;
    [SerializeField] private int numToKill;

    private int enemiesKilled;

    private EventBus eventBus;

    private void Awake()
    {
        eventBus = ServiceLocator.Current.Get<EventBus>();
    }

    private void OnEnable()
    {
        eventBus.Subscribe<E_OnCharacterDeath>(OnEnemyKilled);
    }

    private void OnDisable()
    {
        eventBus.Unsubscribe<E_OnCharacterDeath>(OnEnemyKilled);
    }

    private void OnEnemyKilled(E_OnCharacterDeath onCharacterDeath)
    {
        Enemy enemy;
        if (!onCharacterDeath.character.gameObject.TryGetComponent(out enemy)) return;

        if (enemy.enemyType != this.enemyType) return;
        
        if (enemiesKilled < numToKill)
        {
            enemiesKilled++;
            UpdateState(); 
        }

        if (enemiesKilled >= numToKill)
        {
            FinishQuestStep();
        }
    }

    protected override void UpdateState()
    {
        string state = enemiesKilled.ToString();
        ChangeState(state);
    }
}
