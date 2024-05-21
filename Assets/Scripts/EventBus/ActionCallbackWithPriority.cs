public class ActionCallbackWithPriority
{
    /// <summary>
    /// ��� ���� Priority, ��� ������ ��������� ��������
    /// </summary>
    public readonly int Priority;
    public readonly object Callback;

    public ActionCallbackWithPriority (int priority, object callback)
    {
        Priority = priority;
        Callback = callback;
    }
}
