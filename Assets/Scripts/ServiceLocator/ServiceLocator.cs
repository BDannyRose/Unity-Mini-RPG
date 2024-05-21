using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServiceLocator
{
    private ServiceLocator() { }

    /// <summary>
    /// ����� �������� ��� �������
    /// </summary>
    private readonly Dictionary<string, IService> services = new Dictionary<string, IService>();

    public static ServiceLocator Current { get; private set; }

    public static void Initialize()
    {
        Current = new ServiceLocator();
    }

    /// <summary>
    /// ���������� �����, ����������� IService
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public T Get<T>() where T: IService
    {
        string key = typeof(T).Name;
        if (!services.ContainsKey(key))
        {
            Debug.LogError($"{key} is not registered in {GetType().Name}");
            throw new InvalidOperationException();
        }

        return (T)services[key];
    }

    /// <summary>
    /// ����������� ������� � ServiceLocator
    /// </summary>
    /// <typeparam name="T">��� �������</typeparam>
    /// <param name="service">��������� �������</param>
    public void Register<T>(T service) where T: IService
    {
        string key = typeof(T).Name;
        if (services.ContainsKey(key))
        {
            Debug.LogError($"Trying to register service of type {key} but it is already registered in {GetType().Name}");
            return;
        }

        services.Add(key, service);
    }

    /// <summary>
    /// �������� ������� �� ��������
    /// </summary>
    /// <typeparam name="T">��� �������</typeparam>
    public void Unregister<T>() where T: IService
    {
        string key = typeof(T).Name;
        if (!services.ContainsKey(key))
        {
            Debug.LogError($"Trying to unregister service of type {key} but it's not registered in {GetType().Name}");
            return;
        }

        services.Remove(key);
    }

}
