using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServiceLocatorLoader_Gameplay : MonoBehaviour
{
    private EventBus eventBus;

    [SerializeField] private PlayerController playerController;
    [SerializeField] private Tooltip tooltip;
    [SerializeField] private TooltipWarning tooltipWarning;

    private void Awake()
    {
        eventBus = new EventBus();

        RegisterServices();
        Init();
        LateInit();
    }

    private void RegisterServices()
    {
        ServiceLocator.Initialize();

        ServiceLocator.Current.Register(eventBus);
        ServiceLocator.Current.Register(playerController);
        ServiceLocator.Current.Register(tooltip);
        ServiceLocator.Current.Register(tooltipWarning);
    }

    private void Init()
    {
        playerController.Init();
    }

    private void LateInit()
    {
        playerController.LateInit();
    }
}
