using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireMovementBlockEvent : StateMachineBehaviour
{
    public bool updateOnStateMachine;
    public bool updateOnState;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (updateOnState)
        {
            ICharacter character;
            if (animator.TryGetComponent(out character))
            {
                EventBus eventBus = ServiceLocator.Current.Get<EventBus>();
                eventBus.Invoke(new E_OnCharacterMovementBlocked(character, true));
            }
            else
            {
                Debug.LogError($"{animator.gameObject.name} has no ICharacter assigned to it");
            }
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (updateOnState)
        {
            ICharacter character;
            if (animator.TryGetComponent(out character))
            {
                EventBus eventBus = ServiceLocator.Current.Get<EventBus>();
                eventBus.Invoke(new E_OnCharacterMovementUnblocked(character, true));
            }
            else
            {
                Debug.LogError($"{animator.gameObject.name} has no ICharacter assigned to it");
            }
        }
    }

    override public void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    {
        if (updateOnStateMachine)
        {
            ICharacter character;
            if (animator.TryGetComponent(out character))
            {
                EventBus eventBus = ServiceLocator.Current.Get<EventBus>();
                eventBus.Invoke(new E_OnCharacterMovementBlocked(character, true));
            }
            else
            {
                Debug.LogError($"{animator.gameObject.name} has no ICharacter assigned to it");
            }
        }
    }

    override public void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    {
        if (updateOnStateMachine)
        {
            ICharacter character;
            if (animator.TryGetComponent(out character))
            {
                EventBus eventBus = ServiceLocator.Current.Get<EventBus>();
                eventBus.Invoke(new E_OnCharacterMovementUnblocked(character, true));
            }
            else
            {
                Debug.LogError($"{animator.gameObject.name} has no ICharacter assigned to it");
            }
        }
    }
}
