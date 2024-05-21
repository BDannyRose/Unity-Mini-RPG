using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class CustomTrigger : MonoBehaviour
{
    [SerializeField] string tagFilter;
    public UnityEvent EnteredTrigger;
    public UnityEvent ExitedTrigger;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (String.IsNullOrEmpty(tagFilter))
            return;
        if (!other.CompareTag(tagFilter))
            return;

        EnteredTrigger?.Invoke();
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (String.IsNullOrEmpty(tagFilter))
            return;
        if (!other.CompareTag(tagFilter))
            return;
        ExitedTrigger?.Invoke();
    }
}
