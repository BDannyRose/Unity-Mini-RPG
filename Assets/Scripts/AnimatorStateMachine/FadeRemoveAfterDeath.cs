using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeRemoveAfterDeath : StateMachineBehaviour
{
    public float fadeTime = 0.5f;
    public float fadeDelay = 0.0f;
    public bool isLockedToDeathPosition;
    private float timeElapsed = 0f;
    private float fadeDelayElapsed = 0f;
    SpriteRenderer spriteRenderer;
    GameObject removableObject;
    Color startColor;

    Vector2 startPosition;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timeElapsed = 0f;
        spriteRenderer = animator.GetComponent<SpriteRenderer>();
        removableObject = animator.gameObject;
        startColor = spriteRenderer.color;
        startPosition = animator.transform.position;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (isLockedToDeathPosition)
        {
            animator.transform.position = startPosition;
        }

        if (fadeDelayElapsed < fadeDelay)
        {
            fadeDelayElapsed += Time.deltaTime;
        }
        else
        {
            timeElapsed += Time.deltaTime;

            float newAlpha = startColor.a * (1 - (timeElapsed / fadeTime));
            spriteRenderer.color = new Color(startColor.a, startColor.g, startColor.b, newAlpha);

            if (timeElapsed > fadeTime)
            {
                Destroy(removableObject);
            }
        }
        
        
    }

    //OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //animator.GetComponent<FadeAfterDeath>().Fade();
        
        
    }


    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
