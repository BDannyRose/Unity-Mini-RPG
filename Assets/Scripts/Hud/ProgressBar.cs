using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] private Image _bar;

    public void SetProgress(float progress)
    {
        _bar.fillAmount = progress;
    }
}
