using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServiceLocatorLoader_Gameplay : MonoBehaviour
{
    private EventBus eventBus;

    [SerializeField] private PlayerController playerController;

    private SkillSystem playerSkills;
    private LevelSystem levelSystem;

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

        ServiceLocator.Current.Register<EventBus>(eventBus);
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
